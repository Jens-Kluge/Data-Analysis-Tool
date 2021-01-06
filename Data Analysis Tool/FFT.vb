
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices



Namespace Fast_Fourier_Transform

    '' <summary>
    ''' Defining Structure for Complex Data type  N=R+Ii
    '' </summary>
    Structure COMPLEX

        Public real, imag As Double

        Public Sub New(x As Double, y As Double)
            real = x
            imag = y
        End Sub

        Public Function Magnitude() As Double
            Return Math.Sqrt(real * real + imag * imag)
        End Function

        Public Function Phase() As Double
            Return Math.Atan(imag / real)
        End Function

    End Structure

    Class FFT

        Public Obj As Bitmap               'Input Object Image
        Public FourierPlot As Bitmap       'Generated Fourier Magnitude Plot
        Public PhasePlot As Bitmap         'Generated Fourier Phase Plot

        Public GreyImage(,) As Integer      'GreyScale Image Array Generated from input Image
        Public FourierMagnitude(,) As Double
        Public FourierPhase(,) As Double

        Private FFTLog(,) As Double            'Log of Fourier Magnitude
        Private FFTPhaseLog(,) As Double       'Log Of Fourier Phase
        'why are these integer arrays? JK. I would say scale is 0-255 then, not 0-1!
        Public FFTNormalized(,) As Integer     'Normalized FFT Magnitude : Scale 0-1
        Public FFTPhaseNormalized(,) As Integer 'Normalized FFT Phase : Scale 0-1
        Private nx, ny As Integer               'Number Of Points In Width & height
        Private Width, Height As Integer
        Private Fourier(,) As COMPLEX            'Fourier Magnitude  Array Used for Inverse FFT
        Public FFTShifted(,) As COMPLEX          'Shifted FFT 
        Public Output(,) As COMPLEX              ' FFT Normal
        Public FFTNormal(,) As COMPLEX           'FFT Shift Removed - required For Inverse FFT 

        '''<summary>
        '''Parameterized Constructor for FFT Reads Input Bitmap to a Greyscale Array
        ''' </summary>
        ''' <param name="Input">Input Image</param>
        Public Sub New(Input As Bitmap)
            Obj = Input
            nx = Input.Width
            Width = nx
            ny = Input.Height
            Height = ny
            ReadImage()
        End Sub

        ''' <summary>
        ''' Parameterized Constructor for FFT
        ''' </summary>
        ''' <param name="Input">Greyscale Array</param>
        Public Sub New(Input(,) As Integer)
            GreyImage = Input
            nx = Input.GetLength(0)
            Width = nx
            ny = Input.GetLength(1)
            Height = ny
        End Sub

        ''' <summary>
        ''' Constructor for Inverse FFT
        ''' </summary>
        ''' <param name="Input"></param>
        Public Sub New(Input(,) As COMPLEX)
            Width = Input.GetLength(0)
            nx = Width
            Height = Input.GetLength(1)
            ny = Height
            Fourier = Input
        End Sub

        ''' <summary>
        ''' Function to Read Bitmap to greyscale Array
        ''' Convert color image to greyscale by adding up intensities of all three color channels
        ''' </summary>
        Private Sub ReadImage()

            Dim i, j As Integer
            GreyImage = New Integer(Width, Height) {}  '[Row,Column]
            Dim Image As Bitmap = Obj
            Dim bitmapData1 As BitmapData = Image.LockBits(New Rectangle(0, 0, Image.Width, Image.Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)

            Dim total_size As Integer = bitmapData1.Stride * bitmapData1.Height
            Dim ImageBytes(total_size) As Byte


            Dim ptr As Long = 0
            For i = 0 To bitmapData1.Height - 1
                For j = 0 To bitmapData1.Width - 1

                    GreyImage(j, i) = CType((ImageBytes(ptr) + ImageBytes(ptr + 1) + ImageBytes(ptr + 2) / 3.0), Integer)
                    '4 bytes per pixel
                    ptr += 4
                Next 'j
                '4 bytes per pixel
                ptr += bitmapData1.Stride - (bitmapData1.Width * 4)
            Next ' i


            Image.UnlockBits(bitmapData1)

        End Sub


        Public Function Displayimage() As Bitmap

            Dim i, j As Integer
            Dim image As Bitmap = New Bitmap(Width, Height)
            Dim bitmapData1 As BitmapData = image.LockBits(New Rectangle(0, 0, Width, Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)



            Dim total_size As Integer = bitmapData1.Stride * bitmapData1.Height
            Dim ImageBytes(total_size) As Byte
            'copy image data into imagebytes
            Marshal.Copy(bitmapData1.Scan0, ImageBytes, 0, total_size)
            Dim ptr As Long = 0
            For i = 0 To bitmapData1.Height - 1
                For j = 0 To bitmapData1.Width - 1


                    ImageBytes(ptr) = CType(GreyImage(j, i), Byte)
                    ImageBytes(ptr + 1) = CType(GreyImage(j, i), Byte)
                    ImageBytes(ptr + 2) = CType(GreyImage(j, i), Byte)
                    ImageBytes(ptr + 3) = CType(255, Byte)
                    '4 bytes per pixel
                    ptr += 4
                Next 'j

                '4 bytes per pixel
                ptr += (bitmapData1.Stride - (bitmapData1.Width * 4))
            Next 'i

            'write the result back to  bitmapdata1
            Marshal.Copy(ImageBytes, 0, bitmapData1.Scan0, total_size)

            image.UnlockBits(bitmapData1)
            Return image 'col

        End Function

        Function ImageToByteArr(inpImg As Image) As Byte()

            Dim image As Bitmap = New Bitmap(inpImg.Width, inpImg.Height)
            Dim bitmapData1 As BitmapData = image.LockBits(New Rectangle(0, 0, inpImg.Width, inpImg.Height),
                               ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)

            Dim total_size As Integer = bitmapData1.Stride * bitmapData1.Height
            Dim ImageBytes(total_size) As Byte
            'copy image data into imagebytes
            Marshal.Copy(bitmapData1.Scan0, ImageBytes, 0, total_size)
            image.UnlockBits(bitmapData1)

            Return ImageBytes
        End Function

        'convert an integer array into a pixel array of a color image which can be displayed on the screen
        'pixel format Format32bppArgb means: the pixel has 8 bits for R,G,B each and 8 bits for alpha channel
        'R,G and B channel at point (i,j) are all set to the value of image 
        'assumes that the values of image(i,j) are all between 0 and 255
        'value of the alpha channel is set to the maximum, i.e. 255
        Public Function Displayimage(image(,) As Integer) As Bitmap

            Dim i, j As Integer
            Dim output As Bitmap = New Bitmap(image.GetLength(0), image.GetLength(1))

            Try
                'use lock bits to ensure the bits stay at the same position in memory
                'and we know where we are writing to
                Dim bitmapData1 As BitmapData = output.LockBits(New Rectangle(0, 0, image.GetLength(0), image.GetLength(1)),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)



                Dim total_size As Integer = bitmapData1.Stride * bitmapData1.Height
                Dim ImageBytes(total_size) As Byte
                'copy image data into imagebytes
                Marshal.Copy(bitmapData1.Scan0, ImageBytes, 0, total_size)

                Dim ptr As Long = 0
                'write values to the bytes, assuming there are four consecutive bytes assigned to each pixel
                For i = 0 To bitmapData1.Height - 1
                    For j = 0 To bitmapData1.Width - 1
                        '4 bytes per pixel
                        ImageBytes(ptr) = CType(image(j, i), Byte)
                        ImageBytes(ptr + 1) = CType(image(j, i), Byte)
                        ImageBytes(ptr + 2) = CType(image(j, i), Byte)
                        ImageBytes(ptr + 3) = CType(255, Byte)
                        ptr += 4
                    Next
                    'if row is longer than image width add bytes to move to next row 
                    ptr += (bitmapData1.Stride - 4 * bitmapData1.Width)
                Next ' next row

                'write the result back to  bitmapdata1
                Marshal.Copy(ImageBytes, 0, bitmapData1.Scan0, total_size)

                output.UnlockBits(bitmapData1)

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            Return output

        End Function



        ''' <summary>
        ''' Calculate Fast Fourier Transform of Input Image
        ''' </summary>
        Public Sub ForwardFFT()

            'Initializing Fourier Transform Array
            Dim i, j As Integer
            Fourier = New COMPLEX(Width, Height) {}
            Output = New COMPLEX(Width, Height) {}

            'Copy Image Data to the Complex Array
            For i = 0 To Width - 1
                For j = 0 To Height - 1

                    Fourier(i, j).real = CType(GreyImage(i, j), Double)
                    Fourier(i, j).imag = 0
                Next
            Next

            'Calling Forward Fourier Transform
            Output = FFT2D(Fourier, nx, ny, 1)

        End Sub

        ''' <summary>
        ''' Shift The FFT of the Image
        ''' </summary>
        Public Sub FFTShift()
            Dim i, j As Integer
            Dim FFTShifted = New COMPLEX(nx, ny) {}
            'is nx always even? JK
            For i = 0 To (nx / 2) - 1
                For j = 0 To (ny / 2) - 1
                    FFTShifted(i + (nx / 2), j + (ny / 2)) = Output(i, j)
                    FFTShifted(i, j) = Output(i + (nx / 2), j + (ny / 2))
                    FFTShifted(i + (nx / 2), j) = Output(i, j + (ny / 2))
                    FFTShifted(i, j + (nx / 2)) = Output(i + (nx / 2), j)
                Next
            Next
        End Sub

        ''' <summary>
        ''' Removes FFT Shift for FFTshift Array
        ''' </summary>
        Public Sub RemoveFFTShift()

            Dim i, j As Integer
            FFTNormal = New COMPLEX(nx, ny) {}

            For i = 0 To (nx / 2) - 1
                For j = 0 To (ny / 2) - 1

                    FFTNormal(i + (nx / 2), j + (ny / 2)) = FFTShifted(i, j)
                    FFTNormal(i, j) = FFTShifted(i + (nx / 2), j + (ny / 2))
                    FFTNormal(i + (nx / 2), j) = FFTShifted(i, j + (ny / 2))
                    FFTNormal(i, j + (nx / 2)) = FFTShifted(i + (nx / 2), j)
                Next
            Next

        End Sub

        ''' <summary>
        ''' FFT Plot Method for Shifted FFT, 
        ''' writes to the member variables 'Fourierplot' and 'Phaseplot' for use in graphical output
        ''' </summary>
        ''' <param name="Output"></param>
        Public Sub FFTPlot(Output(,) As COMPLEX)

            Dim i, j As Integer
            Dim max As Double

            FFTLog = New Double(nx, ny) {}
            FFTPhaseLog = New Double(nx, ny) {}

            FourierMagnitude = New Double(nx, ny) {}
            FourierPhase = New Double(nx, ny) {}

            FFTNormalized = New Integer(nx, ny) {}
            FFTPhaseNormalized = New Integer(nx, ny) {}

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FourierMagnitude(i, j) = Output(i, j).Magnitude()
                    FourierPhase(i, j) = Output(i, j).Phase()
                    FFTLog(i, j) = Math.Log(1 + FourierMagnitude(i, j))
                    FFTPhaseLog(i, j) = Math.Log(1 + Math.Abs(FourierPhase(i, j)))
                Next
            Next

            'Generating Magnitude Bitmap
            max = FFTLog(0, 0)
            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    If FFTLog(i, j) > max Then
                        max = FFTLog(i, j)
                    End If
                Next
            Next

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FFTLog(i, j) = FFTLog(i, j) / max
                Next
            Next

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FFTNormalized(i, j) = CType(2000 * FFTLog(i, j), Integer)
                Next
            Next

            'Transferring Image to Fourier Plot
            FourierPlot = Displayimage(FFTNormalized)

            'generating phase Bitmap
            FFTPhaseLog(0, 0) = 0
            max = FFTPhaseLog(1, 1)
            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    If FFTPhaseLog(i, j) > max Then
                        max = FFTPhaseLog(i, j)
                    End If
                Next
            Next

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FFTPhaseLog(i, j) = FFTPhaseLog(i, j) / max
                Next
            Next

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FFTPhaseNormalized(i, j) = CType(255 * FFTPhaseLog(i, j), Integer)
                Next
            Next
            'Transferring Image to Fourier Plot
            PhasePlot = Displayimage(FFTPhaseNormalized)

        End Sub

        ''' <summary>
        ''' generate FFT Image for Display Purpose
        ''' </summary>
        Public Sub FFTPlot()

            Dim i, j As Integer
            Dim max As Double
            FFTLog = New Double(nx, ny) {}
            FFTPhaseLog = New Double(nx, ny) {}

            FourierMagnitude = New Double(nx, ny) {}
            FourierPhase = New Double(nx, ny) {}

            FFTNormalized = New Integer(nx, ny) {}
            FFTPhaseNormalized = New Integer(nx, ny) {}

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FourierMagnitude(i, j) = Output(i, j).Magnitude()
                    FourierPhase(i, j) = Output(i, j).Phase()
                    FFTLog(i, j) = Math.Log(1 + FourierMagnitude(i, j))
                    FFTPhaseLog(i, j) = Math.Log(1 + Math.Abs(FourierPhase(i, j)))
                Next
            Next

            'Generating Magnitude Bitmap
            max = FFTLog(0, 0)
            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    If FFTLog(i, j) > max Then
                        max = FFTLog(i, j)
                    End If
                Next
            Next

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FFTLog(i, j) = FFTLog(i, j) / max
                Next
            Next

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FFTNormalized(i, j) = CType(1000 * FFTLog(i, j), Integer)
                Next
            Next

            'Transferring Image to Fourier Plot
            FourierPlot = Displayimage(FFTNormalized)

            'generating phase Bitmap

            max = FFTPhaseLog(0, 0)
            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    If FFTPhaseLog(i, j) > max Then
                        max = FFTPhaseLog(i, j)
                    End If
                Next
            Next

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FFTPhaseLog(i, j) = FFTPhaseLog(i, j) / max
                Next
            Next

            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    FFTPhaseNormalized(i, j) = CType(2000 * FFTLog(i, j), Integer)
                Next
            Next

            'Transferring Image to Fourier Plot
            PhasePlot = Displayimage(FFTPhaseNormalized)

        End Sub

        ''' <summary>
        ''' Calculate Inverse from Complex [,]  Fourier Array
        ''' </summary>
        Public Sub InverseFFT()

            'Initializing Fourier Transform Array
            Dim i, j As Integer

            'Calling Forward Fourier Transform
            Output = New COMPLEX(nx, ny) {}
            Output = FFT2D(Fourier, nx, ny, -1)

            Obj = Nothing  'Setting Object Image To Null
            'Copying Real Image Back to Greyscale
            'Copy Image Data to the Complex Array
            For i = 0 To Width - 1
                For j = 0 To Height - 1
                    GreyImage(i, j) = CType(Output(i, j).Magnitude(), Integer)

                Next
            Next

            Obj = Displayimage(GreyImage)

        End Sub

        ''' <summary>
        ''' Generates Inverse FFT of Given Input Fourier
        ''' </summary>
        ''' <param name="Fourier"></param>
        Public Sub InverseFFT(Fourier(,) As COMPLEX)
            'Initializing Fourier Transform Array
            Dim i, j As Integer

            'Calling reverse Fourier Transform with direction = -1
            Output = New COMPLEX(nx, ny) {}
            Output = FFT2D(Fourier, nx, ny, -1)


            'Copying Real Image Back to Greyscale
            'Copy Image Data to the Complex Array
            For i = 0 To Width - 1
                For j = 0 To Height - 1

                    GreyImage(i, j) = CType(Output(i, j).Magnitude(), Integer)

                Next
            Next

            Obj = Displayimage(GreyImage)

        End Sub
        '------------------------------------------------------------------------
        'Perform a 2D FFT inplace given a complex 2D array
        'The direction dir, 1 for forward, -1 for reverse
        'The size of the array (nx,ny)
        'Return false if there are memory problems or
        'the dimensions are Not powers Of 2
        Public Function FFT2D(c(,) As COMPLEX, nx As Integer, ny As Integer, dir As Integer) As COMPLEX(,)

            Dim i, j As Integer
            Dim m As Integer 'Power of 2 for current number of points
            Dim real() As Double
            Dim imag() As Double
            Dim output(,) As COMPLEX '=New COMPLEX [nx,ny];
            output = c 'Copying Array
            'Transform the Rows 
            real = New Double(nx) {}
            imag = New Double(nx) {}

            For j = 0 To ny - 1 'outer loop iterates over the rows

                'write the values of a row into temporary row vector
                For i = 0 To nx - 1
                    real(i) = c(i, j).real
                    imag(i) = c(i, j).imag
                Next

                ' Calling 1D FFT Function for Rows
                m = CType(Math.Log(CType(nx, Double), 2), Integer) 'Finding power of 2 for current number of points e.g. for nx=512 m=9
                FFT1D(dir, m, real, imag)

                'write the transformed temporary row vector into the output array
                For i = 0 To nx - 1

                    output(i, j).real = real(i)
                    output(i, j).imag = imag(i)
                Next 'row i

            Next 'column j

            ' Transform the columns  
            ' i.e. first dx integration for every y, then dy integration
            real = New Double(ny) {}
            imag = New Double(ny) {}

            For i = 0 To nx - 1 'outer loop iterates over columns

                For j = 0 To ny - 1 'write one column into temporary column vector 
                    real(j) = output(i, j).real
                    imag(j) = output(i, j).imag
                Next 'column j

                'Calling 1D FFT Function for Columns
                m = CType(Math.Log(CType(ny, Double), 2), Integer) 'Finding power of 2 for current number of points e.g. for nx=512 m=9
                FFT1D(dir, m, real, imag)

                For j = 0 To ny - 1
                    output(i, j).real = real(j)
                    output(i, j).imag = imag(j)
                Next 'column j

            Next 'row i

            'return(true);
            Return output
        End Function
        '*-------------------------------------------------------------------------
        'This computes an in-place complex-to-complex FFT
        'x and y are the real and imaginary arrays of 2^m points.
        'dir = 1 gives forward transform
        'dir = -1 gives reverse transform
        'Formula: forward
        '         N-1
        '          ---
        '        1 \         - j k 2 pi n / N
        'X(K) = --- > x(n) e                  = Forward transform
        '        N /                            n=0..N-1
        '          ---
        '         n=0
        'Formula: reverse
        '         N-1
        '         ---
        '         \          j k 2 pi n / N
        'X(n) =    > x(k) e                  = Inverse transform
        '         /                             k=0..N-1
        '         ---
        '         k=0
        '*/

        'TODO: check the loops against the original code JK
        Private Sub FFT1D(dir As Integer, m As Integer, ByRef x() As Double, ByRef y() As Double)

            Dim nn, i, i1, j, k, i2, l, l1, l2 As Long
            Dim c1, c2, tx, ty, t1, t2, u1, u2, z As Double
            ' Calculate the number of points
            nn = 1
            For i = 0 To m - 1
                nn *= 2
            Next

            ' Do the bit reversal */
            i2 = (nn >> 1)
            j = 0
            For i = 0 To nn - 2

                If (i < j) Then
                    tx = x(i)
                    ty = y(i)
                    x(i) = x(j)
                    y(i) = y(j)
                    x(j) = tx
                    y(j) = ty
                End If

                k = i2
                While k <= j
                    j -= k
                    k >>= 1
                End While

                j += k
            Next


            ' Compute the FFT */
            c1 = -1.0
            c2 = 0.0
            l2 = 1
            For l = 0 To m - 1

                l1 = l2
                l2 <<= 1
                u1 = 1.0
                u2 = 0.0
                For j = 0 To l1 - 1
                    For i = j To nn - 1 Step l2

                        i1 = i + l1
                        t1 = u1 * x(i1) - u2 * y(i1)
                        t2 = u1 * y(i1) + u2 * x(i1)
                        x(i1) = x(i) - t1
                        y(i1) = y(i) - t2
                        x(i) += t1
                        y(i) += t2
                    Next
                    z = u1 * c1 - u2 * c2
                    u2 = u1 * c2 + u2 * c1
                    u1 = z
                Next
                c2 = Math.Sqrt((1.0 - c1) / 2.0)
                If dir = 1 Then
                    c2 = -c2
                End If
                c1 = Math.Sqrt((1.0 + c1) / 2.0)
            Next

            'Scaling for forward transform */
            If dir = 1 Then

                For i = 0 To nn - 1
                    x(i) /= CType(nn, Double)
                    y(i) /= CType(nn, Double)
                Next
            End If

        End Sub

    End Class

End Namespace
