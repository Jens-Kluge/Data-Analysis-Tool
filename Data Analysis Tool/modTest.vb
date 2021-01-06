Imports MathNet.Numerics
Imports System.Drawing
Imports System.Drawing.Imaging
Imports opencv = Emgu.CV
Imports FFTW.NET

Module modTest

    ''' <summary>
    ''' signals for testing the fourier transform
    ''' </summary>
    Class SignalGenerator

        Public Function Sine(Optional size As Integer = 1024) As Double()

            Const N As Integer = 64  'sampling rate

            Return Generate.Sinusoidal(size, N, 1.0, 20.0)

        End Function


        Public Function Sine2(Optional size As Integer = 1024) As Double()

            Const N As Integer = 64  'sampling rate

            Dim a() As Double = Generate.Sinusoidal(size, N, 1.0, 20.0)
            Dim b() As Double = Generate.Sinusoidal(size, N, 2.0, 10.0)

            For i = 0 To size - 1
                a(i) += b(i)
            Next

            Return a

        End Function

        Public Function Sine3(Optional size As Integer = 1024) As Double()

            Const N As Integer = 64 'sampling rate

            Dim a() As Double = Generate.Sinusoidal(size, N, 1.0, 20.0)
            Dim b() As Double = Generate.Sinusoidal(size, N, 2.0, 10.0)
            Dim c() As Double = Generate.Sinusoidal(size, N, 4.0, 5.0)

            For i = 0 To size - 1
                a(i) += (b(i) + c(i))
            Next

            Return a

        End Function

        Public Function Square(Optional size As Integer = 1024) As Double()


            Const N As Integer = 32

            Return Generate.Square(size, N, N, -20.0, 20.0)

        End Function

        Public Function Sawtooth(Optional size As Integer = 1024) As Double()

            Const N As Integer = 32

            Return Generate.Sawtooth(size, N, -20.0, 20.0)

        End Function

        Public Function Triangle(Optional size As Integer = 1024) As Double()

            Const N As Integer = 32

            Return Generate.Triangle(size, N, N, -20.0, 20.0)

        End Function

        Private Function ApplyWindow(signal() As Double) As Double()

            Dim n As Integer = signal.Length

            Dim window() As Double = MathNet.Numerics.Window.Hamming(n)

            For i = 0 To n - 1

                signal(i) *= window(i)

            Next

            Return signal

        End Function

    End Class


    Public Function ToComplex(data() As Double) As Complex32()

        Dim length As Integer = data.Length

        Dim result(length) As Complex32

        For i = 0 To length - 1
            result(i) = New Complex32(data(i), 0.0)
        Next

        Return result

    End Function


    Public Sub ToDouble(data() As Complex32, target() As Double)

        Dim length As Integer = data.Length

        For i = 0 To length - 1
            target(i) = data(i).Real
        Next
    End Sub



    'functions for testing emgu.cv
    Sub testEmguFFT()


        ''Load image
        'Dim image As opencv.Image(Of opencv.Structure.Gray, Double) = New opencv.Image(Of opencv.Structure.Gray, Double)("C:\Users\me\Desktop\lines.png");

        ''Transform 1 channel gray scale image into 2 channel imaged
        'Dim complexImage As IntPtr = opencv.CvInvoke.cvCreateImage(image.Size, opencv.CvEnum.DepthType.Cv32F, 2)
        'opencv.CvInvoke.cvSetImageCOI(complexImage, 1) 'Select the channel To copy into
        'opencv.CvInvoke.cvCopy(image, complexImage, IntPtr.Zero)
        'opencv.CvInvoke.cvSetImageCOI(complexImage, 0) 'Select all channels

        ''This will hold the DFT data
        'Dim forwardDft As opencv.Matrix(Of Double) = New opencv.Matrix(Of Double)(image.Rows, image.Cols, 2)
        'opencv.CvInvoke.Dft(complexImage, forwardDft, opencv.CvEnum.DxtType.Forward, 0)

        'opencv.CvInvoke.cvReleaseImage(complexImage);

        '' We'll display the magnitude
        'Dim forwardDftMagnitude As opencv.Matrix(Of Double) = GetDftMagnitude(forwardDft)
        'SwitchQuadrants(forwardDftMagnitude)

        ''Now compute the inverse to see if we can get back the original
        'Dim reverseDft As opencv.Matrix(Of Double) = New opencv.Matrix(Of Double)(forwardDft.Rows, forwardDft.Cols, 2)
        'opencv.CvInvoke.Dft(forwardDft, reverseDft, opencv.CvEnum.DxtType.InvScale, 0)
        'Dim reverseDftMagnitude As opencv.Matrix(Of Double) = GetDftMagnitude(reverseDft)

        'pictureBox1.Image = image.ToBitmap();
        'pictureBox2.Image = Matrix2Bitmap(forwardDftMagnitude)
        'pictureBox3.Image = Matrix2Bitmap(reverseDftMagnitude)

    End Sub

    'Private Function Matrix2Bitmap(matrix As opencv.Matrix(Of Double)) As Bitmap

    '    opencv.CvInvoke.Normalize(matrix, matrix, 0.0, 255.0, opencv.CvEnum.NormType.MinMax, IntPtr.Zero)

    '    Dim image As opencv.Image(Of opencv.Structure.Gray, Double) = New opencv.Image(Of opencv.Structure.Gray, Double)(matrix.Size)
    '    matrix.CopyTo(image)

    '    Return image.Bytes

    'End Function

    ' Real part Is magnitude, imaginary Is phase. 
    ' Here we compute log(sqrt(Re^2 + Im^2) + 1) to get the magnitude And 
    ' rescale it so everything Is visible
    Private Function GetDftMagnitude(fftData As opencv.Matrix(Of Double)) As opencv.Matrix(Of Double)

        'The Real part of the Fourier Transform
        Dim outReal As opencv.Matrix(Of Double) = New opencv.Matrix(Of Double)(fftData.Size)
        'The imaginary part of the Fourier Transform
        Dim outIm As opencv.Matrix(Of Double) = New opencv.Matrix(Of Double)(fftData.Size)
        'opencv.CvInvoke.Split(fftData, outReal, outIm)

        opencv.CvInvoke.Pow(outReal, 2.0, outReal)
        opencv.CvInvoke.Pow(outIm, 2.0, outIm)

        opencv.CvInvoke.Add(outReal, outIm, outReal)
        opencv.CvInvoke.Pow(outReal, 0.5, outReal)


        opencv.CvInvoke.ScaleAdd(outReal, 1.0, outReal, outReal)  '1 + Mag


        Return outReal
    End Function

    ' We have to switch quadrants so that the origin Is at the image center
    Private Sub SwitchQuadrants(ByRef matrix As opencv.Matrix(Of Double))

        Dim cx As Integer = matrix.Cols / 2
        Dim cy As Integer = matrix.Rows / 2

        Dim q0 As opencv.Matrix(Of Double) = matrix.GetSubRect(New Rectangle(0, 0, cx, cy))
        Dim q1 As opencv.Matrix(Of Double) = matrix.GetSubRect(New Rectangle(cx, 0, cx, cy))
        Dim q2 As opencv.Matrix(Of Double) = matrix.GetSubRect(New Rectangle(0, cy, cx, cy))
        Dim q3 As opencv.Matrix(Of Double) = matrix.GetSubRect(New Rectangle(cx, cy, cx, cy))
        Dim tmp As opencv.Matrix(Of Double) = New opencv.Matrix(Of Double)(q0.Size)

        q0.CopyTo(tmp)
        q3.CopyTo(q0)
        tmp.CopyTo(q3)
        q1.CopyTo(tmp)
        q2.CopyTo(q1)
        tmp.CopyTo(q2)
    End Sub

End Module
