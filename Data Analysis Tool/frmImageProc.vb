Imports System.Drawing
Imports System.Diagnostics
Imports OxyPlot
Imports System.Windows.Forms
Imports System.IO
Imports mnum = MathNet.Numerics
Imports System.Runtime.InteropServices


Public Class frmImageProc
    Private img As OxyPlot.OxyImage
    Private imgpath As String = ""

    Private Sub btnLoadImage_Click(sender As Object, e As System.EventArgs) Handles btnLoadImage.Click
        Dim ofd As New OpenFileDialog
        Dim FilePath As String

        Dim fs As FileStream

        ofd.Title = "Open an Image File" 'Set the title name of the OpenDialog Box  
        ofd.Filter = "Image Files (*.bmp, *.png)|*.bmp;*.png"
        ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        ofd.CheckFileExists = True

        If Not ofd.ShowDialog() = vbOK Then
            Exit Sub
        End If

        'open oxyplot image from filestream
        FilePath = ofd.FileName
        fs = New FileStream(FilePath, mode:=FileMode.Open)
        'TODO: convert to BMP for jpg and other image formats
        img = New OxyPlot.OxyImage(fs)
        fs.Close()
        imgpath = FilePath


        'now show oxyplot image in oxyplot control
        Dim pm As New PlotModel
        pm.Annotations.Add(New Annotations.ImageAnnotation With {
                    .ImageSource = img,
                    .X = New PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea),
                    .Y = New PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea),
                    .Width = New PlotLength(1.0, PlotLengthUnit.RelativeToPlotArea),
                    .Height = New PlotLength(1.0, PlotLengthUnit.RelativeToPlotArea),
                    .Opacity = 1,
                    .Interpolate = False,
                    .OffsetX = New PlotLength(0.0, PlotLengthUnit.RelativeToPlotArea),
                    .OffsetY = New PlotLength(0.0, PlotLengthUnit.RelativeToPlotArea)})


        pltImage.Model = pm

        SetImageSizes()
        'delete previous FFT image
        pbOutput1.Image = Nothing
        pbFFTPhase.Image = Nothing

    End Sub

    Private Sub btnFFT_Click(sender As Object, e As System.EventArgs) Handles btnFFT.Click
        Dim fs As FileStream
        Dim bm As Bitmap
        Dim bmPhase As Bitmap
        Dim maxval As Double

        If img Is Nothing Then Exit Sub


        'byteArrayIn = img.GetData
        Try
            fs = New FileStream(imgpath, FileMode.Open, FileAccess.Read)
            bm = New Bitmap(fs)
            fs.Close()


            Dim in_row(bm.Width) As mnum.Complex32
            Dim in_col(bm.Height) As mnum.Complex32

            Dim in_vecarr As New mnum.LinearAlgebra.Complex32.DenseMatrix(bm.Height, bm.Width)

            Dim clr As Integer 'color value


            Dim rect As New Rectangle(0, 0, bm.Width, bm.Height)
            Dim bmpData As System.Drawing.Imaging.BitmapData = bm.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb)
            Dim bytes As Integer = Math.Abs(bmpData.Stride) * bm.Height
            Dim rgbValues(bytes - 1) As Byte
            Dim ptr As IntPtr = bmpData.Scan0
            Dim pixptr As Integer
            Dim byteval As Byte

            Marshal.Copy(ptr, rgbValues, 0, bytes)

            'iterate over the rows to read in and transform the rows
            For i = 0 To bmpData.Height - 1
                For j = 0 To bmpData.Width - 1
                    pixptr = (bmpData.Stride * i) + (4 * j)
                    clr = CType(Marshal.ReadByte(bmpData.Scan0, pixptr), Integer)  'B
                    clr += CType(Marshal.ReadByte(bmpData.Scan0, pixptr + 1), Integer)  'G
                    clr += CType(Marshal.ReadByte(bmpData.Scan0, pixptr + 2), Integer)  'R
                    in_vecarr(i, j) = New mnum.Complex32(clr / 3.0, 0.0)
                Next
                in_row = in_vecarr.Row(i).ToArray
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(in_row)
                in_vecarr.SetRow(i, in_row)
            Next

            'now iterate over the columns to transform the columns
            For j = 0 To bmpData.Width - 1
                in_col = in_vecarr.Column(j).ToArray
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(in_col)
                in_vecarr.SetColumn(j, in_col)
            Next

            maxval = in_vecarr.Enumerate().Max(Function(x) mnum.Complex32.Abs(x))
            maxval = scaleData(maxval)
            'shift quadrants
            in_vecarr = FFTShift(in_vecarr)

            'write the normalized amplitude back into the bitmap
            For i = 0 To bmpData.Height - 1
                For j = 0 To bmpData.Width - 1
                    byteval = CByte((scaleData(in_vecarr(i, j).Magnitude) / maxval) * 255)
                    pixptr = (bmpData.Stride * i) + (4 * j)
                    Marshal.WriteByte(bmpData.Scan0, pixptr, byteval)  'B
                    Marshal.WriteByte(bmpData.Scan0, pixptr + 1, byteval)  'G
                    Marshal.WriteByte(bmpData.Scan0, pixptr + 2, byteval) 'R
                    Marshal.WriteByte(bmpData.Scan0, pixptr + 3, 255) 'alpha value, set transparency to zero
                Next

            Next
            'System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes)
            bm.UnlockBits(bmpData)

            'attach first output bitmap to first picture box
            pbOutput1.Image = bm

            '*****Write the phase information into second output bitmap
            bmPhase = New Bitmap(bm.Width, bm.Height)
            Dim rect2 As New Rectangle(0, 0, bmPhase.Width, bmPhase.Height)
            Dim bmpData2 As System.Drawing.Imaging.BitmapData = bmPhase.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb)

            maxval = in_vecarr.Enumerate().Max(Function(x As mnum.Complex32) Math.Abs(x.Phase))

            For i = 0 To bmPhase.Height - 1
                For j = 0 To bmPhase.Width - 1
                    'convert into value btw 0 and 255, then convert into byte
                    byteval = CByte(Math.Abs(in_vecarr(i, j).Phase) / maxval * 255)
                    pixptr = (bmpData2.Stride * i) + (4 * j)
                    Marshal.WriteByte(bmpData2.Scan0, pixptr, byteval)  'B
                    Marshal.WriteByte(bmpData2.Scan0, pixptr + 1, byteval)  'G
                    Marshal.WriteByte(bmpData2.Scan0, pixptr + 2, byteval) 'R
                    Marshal.WriteByte(bmpData2.Scan0, pixptr + 3, 255) 'alpha value, set transparency to zero
                Next

            Next

            bmPhase.UnlockBits(bmpData2)
            'attach bitmap to second picture box
            pbFFTPhase.Image = bmPhase

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    'contract the scale for display, 
    'so that the FFT output Is not dominated by a few high frequency components
    'which would result in a black picture
    Function scaleData(val As Double) As Double
        Return Math.Log(1 + val)
    End Function

    ''' <summary>
    ''' Shift The FFT of the Image such that zero frequency is at the center
    ''' </summary>
    Public Function FFTShift(FFTmat As mnum.LinearAlgebra.Matrix(Of mnum.Complex32)) As mnum.LinearAlgebra.Matrix(Of mnum.Complex32)
        Dim i, j As Integer
        Dim i_shift, j_shift As Integer
        Dim i_max, j_max As Integer

        i_shift = FFTmat.RowCount
        If isEven(i_shift) Then
            i_shift = i_shift / 2
        Else
            i_shift = i_shift \ 2 + 1
        End If
        i_max = FFTmat.RowCount \ 2

        j_shift = FFTmat.ColumnCount

        If isEven(j_shift) Then
            j_shift = j_shift / 2
        Else
            j_shift = j_shift \ 2 + 1
        End If
        j_max = FFTmat.ColumnCount \ 2

        Dim FFTShifted As New mnum.LinearAlgebra.Complex32.DenseMatrix(FFTmat.RowCount, FFTmat.ColumnCount)

        For i = 0 To i_max - 1
            For j = 0 To j_max - 1
                FFTShifted(i + i_shift, j + j_shift) = FFTmat(i, j)
                FFTShifted(i, j) = FFTmat(i + i_shift, j + j_shift)
                FFTShifted(i + i_shift, j) = FFTmat(i, j + j_shift)
                FFTShifted(i, j + j_shift) = FFTmat(i + i_shift, j)
            Next
        Next

        Return FFTShifted
    End Function

    Function isEven(i As Integer)

        If i Mod 2 = 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Sub SetImageSizes()

        Try

            'Me.Height = img.Height + Me.heightDiff
            'Me.Width = img.Width + Me.widthDiff
            pltImage.Dock = DockStyle.None
            pbOutput1.Dock = DockStyle.None
            pbFFTPhase.Dock = DockStyle.None

            pltImage.Width = img.Width
            pltImage.Height = img.Height

            pbOutput1.Width = img.Width
            pbOutput1.Height = img.Height

            pbFFTPhase.Width = img.Width
            pbFFTPhase.Height = img.Height

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

End Class