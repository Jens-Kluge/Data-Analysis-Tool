Imports System.Drawing
Imports System.Diagnostics
Imports OxyPlot
Imports System.Windows.Forms
Imports System.IO
Imports mnum = MathNet.Numerics
Imports System.Runtime.InteropServices


Public Class frm2DFFt
    Private m_img As OxyPlot.OxyImage
    Private m_imgpath As String = ""
    'member variable to hold result of grayscale image
    Private m_FFT_result As mnum.LinearAlgebra.Complex32.Matrix
    'members variables to hold transform results of R,G, B channels
    Private m_FFT_red As mnum.LinearAlgebra.Complex32.Matrix
    Private m_FFT_green As mnum.LinearAlgebra.Complex32.Matrix
    Private m_FFT_blue As mnum.LinearAlgebra.Complex32.Matrix

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
        m_img = New OxyPlot.OxyImage(fs)
        fs.Close()
        m_imgpath = FilePath


        'now show oxyplot image in oxyplot control
        Dim pm As New PlotModel
        pm.Annotations.Add(New Annotations.ImageAnnotation With {
                    .ImageSource = m_img,
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

        btnFFT.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        If chkRGB.Checked Then
            FFT_RGBchannels()
        Else
            FFT_Grayscale()
        End If

        Me.Cursor = Cursors.Default
        btnFFT.Enabled = True

    End Sub

    Sub FFT_Grayscale()

        Dim fs As FileStream
        Dim bm As Bitmap
        Dim bmPhase As Bitmap
        Dim maxval As Double

        If m_img Is Nothing Then Exit Sub


        'byteArrayIn = img.GetData
        Try
            fs = New FileStream(m_imgpath, FileMode.Open, FileAccess.Read)
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

            'store the result in member variable
            m_FFT_result = in_vecarr.Clone

            '***** Prepare data for plotting and write the results into the bitmaps
            '1) Write amplitude data into first bitmap
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

            '2) Write the phase information into second output bitmap
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

    ''' <summary>
    ''' Transform each channel separately
    ''' </summary>
    Sub FFT_RGBchannels()

        Dim fs As FileStream
        Dim bm As Bitmap
        Dim bmPhase As Bitmap

        Dim maxval_red, maxval_green, maxval_blue As Double

        If m_img Is Nothing Then Exit Sub


        'byteArrayIn = img.GetData
        Try
            fs = New FileStream(m_imgpath, FileMode.Open, FileAccess.Read)
            bm = New Bitmap(fs)
            fs.Close()


            Dim in_row_red(bm.Width) As mnum.Complex32
            Dim in_col_red(bm.Height) As mnum.Complex32
            Dim in_row_blue(bm.Width) As mnum.Complex32
            Dim in_col_blue(bm.Height) As mnum.Complex32
            Dim in_row_green(bm.Width) As mnum.Complex32
            Dim in_col_green(bm.Height) As mnum.Complex32

            Dim in_arr_red As New mnum.LinearAlgebra.Complex32.DenseMatrix(bm.Height, bm.Width)
            Dim in_arr_blue As New mnum.LinearAlgebra.Complex32.DenseMatrix(bm.Height, bm.Width)
            Dim in_arr_green As New mnum.LinearAlgebra.Complex32.DenseMatrix(bm.Height, bm.Width)

            Dim RedVal, GreenVal, BlueVal As Integer 'color value

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
                    BlueVal = CType(Marshal.ReadByte(bmpData.Scan0, pixptr), Integer)  'B
                    GreenVal = CType(Marshal.ReadByte(bmpData.Scan0, pixptr + 1), Integer)  'G
                    RedVal = CType(Marshal.ReadByte(bmpData.Scan0, pixptr + 2), Integer)  'R
                    in_arr_blue(i, j) = New mnum.Complex32(BlueVal, 0.0)
                    in_arr_green(i, j) = New mnum.Complex32(GreenVal, 0.0)
                    in_arr_red(i, j) = New mnum.Complex32(RedVal, 0.0)
                Next

                in_row_green = in_arr_green.Row(i).ToArray
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(in_row_green)
                in_arr_green.SetRow(i, in_row_green)

                in_row_blue = in_arr_blue.Row(i).ToArray
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(in_row_blue)
                in_arr_blue.SetRow(i, in_row_blue)

                in_row_red = in_arr_red.Row(i).ToArray
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(in_row_red)
                in_arr_red.SetRow(i, in_row_red)
            Next

            'now iterate over the columns to transform the columns
            For j = 0 To bmpData.Width - 1
                in_col_green = in_arr_green.Column(j).ToArray
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(in_col_green)
                in_arr_green.SetColumn(j, in_col_green)

                in_col_blue = in_arr_blue.Column(j).ToArray
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(in_col_blue)
                in_arr_blue.SetColumn(j, in_col_blue)

                in_col_red = in_arr_red.Column(j).ToArray
                MathNet.Numerics.IntegralTransforms.Fourier.Forward(in_col_red)
                in_arr_red.SetColumn(j, in_col_red)
            Next

            'store the results in member variables
            m_FFT_blue = in_arr_blue.Clone
            m_FFT_red = in_arr_red.Clone
            m_FFT_green = in_arr_green.Clone

            '***** Prepare data for plotting and write the results into the bitmaps
            '1) Write amplitude data into first bitmap
            maxval_red = in_arr_red.Enumerate().Max(Function(x) mnum.Complex32.Abs(x))
            maxval_red = scaleData(maxval_red)

            maxval_green = in_arr_green.Enumerate().Max(Function(x) mnum.Complex32.Abs(x))
            maxval_green = scaleData(maxval_green)

            maxval_blue = in_arr_blue.Enumerate().Max(Function(x) mnum.Complex32.Abs(x))
            maxval_blue = scaleData(maxval_blue)
            'shift quadrants
            in_arr_red = FFTShift(in_arr_red)
            in_arr_green = FFTShift(in_arr_green)
            in_arr_blue = FFTShift(in_arr_blue)

            'write the normalized amplitude back into the bitmap
            For i = 0 To bmpData.Height - 1
                For j = 0 To bmpData.Width - 1
                    pixptr = (bmpData.Stride * i) + (4 * j)
                    byteval = CByte((scaleData(in_arr_blue(i, j).Magnitude) / maxval_blue) * 255)
                    Marshal.WriteByte(bmpData.Scan0, pixptr, byteval)  'B
                    byteval = CByte((scaleData(in_arr_green(i, j).Magnitude) / maxval_green) * 255)
                    Marshal.WriteByte(bmpData.Scan0, pixptr + 1, byteval)  'G
                    byteval = CByte((scaleData(in_arr_red(i, j).Magnitude) / maxval_red) * 255)
                    Marshal.WriteByte(bmpData.Scan0, pixptr + 2, byteval) 'R
                    Marshal.WriteByte(bmpData.Scan0, pixptr + 3, 255) 'alpha value, set transparency to zero
                Next

            Next
            'System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes)
            bm.UnlockBits(bmpData)

            'attach first output bitmap to first picture box
            pbOutput1.Image = bm

            '2) Write the phase information into second output bitmap
            bmPhase = New Bitmap(bm.Width, bm.Height)
            Dim rect2 As New Rectangle(0, 0, bmPhase.Width, bmPhase.Height)
            Dim bmpData2 As System.Drawing.Imaging.BitmapData = bmPhase.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb)


            maxval_red = in_arr_red.Enumerate().Max(Function(x) Math.Abs(x.Phase))
            maxval_green = in_arr_green.Enumerate().Max(Function(x) Math.Abs(x.Phase))
            maxval_blue = in_arr_blue.Enumerate().Max(Function(x) Math.Abs(x.Phase))


            For i = 0 To bmPhase.Height - 1
                For j = 0 To bmPhase.Width - 1
                    'convert into value btw 0 and 255, then convert into byte
                    pixptr = (bmpData2.Stride * i) + (4 * j)
                    byteval = CByte(Math.Abs(in_arr_blue(i, j).Phase) / maxval_blue * 255)
                    Marshal.WriteByte(bmpData2.Scan0, pixptr, byteval)  'B
                    byteval = CByte(Math.Abs(in_arr_green(i, j).Phase) / maxval_green * 255)
                    Marshal.WriteByte(bmpData2.Scan0, pixptr + 1, byteval)  'G
                    byteval = CByte(Math.Abs(in_arr_red(i, j).Phase) / maxval_red * 255)
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
    ''' input: matrix with FFT coefficients. output: matrix with shifted 
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

    ''' <summary>
    ''' helper function for FFT_shift
    ''' </summary>
    ''' <param name="i"></param>
    ''' <returns></returns>
    Function isEven(i As Integer)

        If i Mod 2 = 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    ''' <summary>
    ''' Set height and width to "pixel sizes"
    ''' </summary>
    Sub SetImageSizes()

        Try

            'Me.Height = img.Height + Me.heightDiff
            'Me.Width = img.Width + Me.widthDiff
            pltImage.Dock = DockStyle.None
            pbOutput1.Dock = DockStyle.None
            pbFFTPhase.Dock = DockStyle.None
            pbInvFFT.Dock = DockStyle.None

            pltImage.Width = m_img.Width
            pltImage.Height = m_img.Height

            pbOutput1.Width = m_img.Width
            pbOutput1.Height = m_img.Height

            pbFFTPhase.Width = m_img.Width
            pbFFTPhase.Height = m_img.Height

            pbInvFFT.Width = m_img.Width
            pbInvFFT.Height = m_img.Height

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btnInvFFT_Click(sender As Object, e As System.EventArgs) Handles btnInvFFT.Click

        btnInvFFT.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        If chkRGB.Checked Then
            INV_FFT_RGBchannels()
        Else
            INV_FFT_grayscale()
        End If

        Me.Cursor = Cursors.Default
        btnInvFFT.Enabled = True

    End Sub

    Sub INV_FFT_grayscale()

        If m_FFT_result Is Nothing Then Exit Sub

        Dim bm As Bitmap = New Bitmap(m_img.Width, m_img.Height)
        Dim in_row(bm.Width) As mnum.Complex32
        Dim in_col(bm.Height) As mnum.Complex32

        Dim invFFTarr As mnum.LinearAlgebra.Complex32.Matrix


        Dim rect As New Rectangle(0, 0, bm.Width, bm.Height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = bm.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb)
        Dim bytes As Integer = Math.Abs(bmpData.Stride) * bm.Height
        Dim rgbValues(bytes - 1) As Byte
        Dim ptr As IntPtr = bmpData.Scan0
        Dim pixptr As Integer
        Dim byteval As Byte

        invFFTarr = m_FFT_result.Clone()
        'iterate over the rows to read in and transform the rows
        For i = 0 To bmpData.Height - 1
            in_row = invFFTarr.Row(i).ToArray
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(in_row)
            invFFTarr.SetRow(i, in_row)
        Next

        'now iterate over the columns to transform the columns
        For j = 0 To bmpData.Width - 1
            in_col = invFFTarr.Column(j).ToArray
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(in_col)
            invFFTarr.SetColumn(j, in_col)
        Next

        Dim maxval As Double
        maxval = invFFTarr.Enumerate().Max(Function(x As mnum.Complex32) x.Magnitude)

        For i = 0 To bm.Height - 1
            For j = 0 To bm.Width - 1
                'convert into value btw 0 and 255, then convert into byte
                byteval = CByte(Math.Abs(invFFTarr(i, j).Magnitude) / maxval * 255)
                pixptr = (bmpData.Stride * i) + (4 * j)
                Marshal.WriteByte(bmpData.Scan0, pixptr, byteval)  'B
                Marshal.WriteByte(bmpData.Scan0, pixptr + 1, byteval)  'G
                Marshal.WriteByte(bmpData.Scan0, pixptr + 2, byteval) 'R
                Marshal.WriteByte(bmpData.Scan0, pixptr + 3, 255) 'alpha value, set transparency to zero
            Next
        Next

        bm.UnlockBits(bmpData)
        'attach bitmap to second picture box
        pbInvFFT.Image = bm

    End Sub

    Sub INV_FFT_RGBchannels()

        If m_FFT_red Is Nothing Then Exit Sub

        Dim bm As Bitmap = New Bitmap(m_img.Width, m_img.Height)
        Dim in_row_red(bm.Width) As mnum.Complex32
        Dim in_row_green(bm.Width) As mnum.Complex32
        Dim in_row_blue(bm.Width) As mnum.Complex32
        Dim in_col_red(bm.Height) As mnum.Complex32
        Dim in_col_green(bm.Height) As mnum.Complex32
        Dim in_col_blue(bm.Height) As mnum.Complex32

        Dim invFFTarr_red As mnum.LinearAlgebra.Complex32.Matrix
        Dim invFFTarr_blue As mnum.LinearAlgebra.Complex32.Matrix
        Dim invFFTarr_green As mnum.LinearAlgebra.Complex32.Matrix


        Dim rect As New Rectangle(0, 0, bm.Width, bm.Height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = bm.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb)
        Dim bytes As Integer = Math.Abs(bmpData.Stride) * bm.Height
        Dim rgbValues(bytes - 1) As Byte
        Dim ptr As IntPtr = bmpData.Scan0
        Dim pixptr As Integer
        Dim byteval As Byte

        invFFTarr_red = m_FFT_red.Clone()
        invFFTarr_blue = m_FFT_blue.Clone()
        invFFTarr_green = m_FFT_green.Clone()

        'iterate over the rows to read in and transform the rows
        For i = 0 To bmpData.Height - 1
            in_row_red = invFFTarr_red.Row(i).ToArray
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(in_row_red)
            invFFTarr_red.SetRow(i, in_row_red)

            in_row_green = invFFTarr_green.Row(i).ToArray
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(in_row_green)
            invFFTarr_green.SetRow(i, in_row_green)

            in_row_blue = invFFTarr_blue.Row(i).ToArray
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(in_row_blue)
            invFFTarr_blue.SetRow(i, in_row_blue)
        Next

        'now iterate over the columns to transform the columns
        For j = 0 To bmpData.Width - 1
            in_col_red = invFFTarr_red.Column(j).ToArray
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(in_col_red)
            invFFTarr_red.SetColumn(j, in_col_red)

            in_col_green = invFFTarr_green.Column(j).ToArray
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(in_col_green)
            invFFTarr_green.SetColumn(j, in_col_green)

            in_col_blue = invFFTarr_blue.Column(j).ToArray
            MathNet.Numerics.IntegralTransforms.Fourier.Inverse(in_col_blue)
            invFFTarr_blue.SetColumn(j, in_col_blue)
        Next


        Dim maxval_red, maxval_blue, maxval_green, maxval As Double
        maxval_blue = invFFTarr_blue.Enumerate().Max(Function(x As mnum.Complex32) x.Magnitude)
        maxval_red = invFFTarr_red.Enumerate().Max(Function(x As mnum.Complex32) x.Magnitude)
        maxval_green = invFFTarr_green.Enumerate().Max(Function(x As mnum.Complex32) x.Magnitude)
        maxval = Math.Max(Math.Max(maxval_blue, maxval_green), maxval_red)

        For i = 0 To bm.Height - 1
            For j = 0 To bm.Width - 1
                'convert into value btw 0 and 255, then convert into byte
                pixptr = (bmpData.Stride * i) + (4 * j)
                byteval = CByte(invFFTarr_blue(i, j).Magnitude / maxval * 255)
                Marshal.WriteByte(bmpData.Scan0, pixptr, byteval)  'B
                byteval = CByte(invFFTarr_green(i, j).Magnitude / maxval * 255)
                Marshal.WriteByte(bmpData.Scan0, pixptr + 1, byteval)  'G
                byteval = CByte(invFFTarr_red(i, j).Magnitude / maxval * 255)
                Marshal.WriteByte(bmpData.Scan0, pixptr + 2, byteval) 'R
                Marshal.WriteByte(bmpData.Scan0, pixptr + 3, 255) 'alpha value, set transparency to zero
            Next
        Next

        bm.UnlockBits(bmpData)
        'attach bitmap to second picture box
        pbInvFFT.Image = bm
    End Sub

End Class