Imports MathNet.Numerics

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

End Module
