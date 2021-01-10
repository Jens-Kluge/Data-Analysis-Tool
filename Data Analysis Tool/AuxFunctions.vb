Namespace Tests

    Public Class AuxFunctions

        Public Shared Function Median(input() As Double) As Double ''Just gives median

            Dim Buffer() As Double = New Double(input.Length) {}

            For i As Integer = 0 To input.Length - 1
                Buffer(i) = input(i)
            Next i

            Array.Sort(Buffer)
            If Buffer.Length Mod 2 = 0 Then

                Return (Buffer(Buffer.Length / 2) + Buffer(Buffer.Length / 2 - 1)) / 2

            Else
                Return Buffer(Buffer.Length \ 2)
            End If

        End Function

        Public Shared Function MedianOfModulus(input() As Double) As Double 'Gives median Of absolute values Of the original vector

            Dim Buffer() As Double = New Double(input.Length) {}
            Dim i As Integer
            For i = 0 To input.Length - 1
                Buffer(i) = Math.Abs(input(i))
            Next i

            Array.Sort(Buffer)
            If (Buffer.Length Mod 2 = 0) Then

                Return (Buffer(Buffer.Length / 2) + Buffer(Buffer.Length / 2 - 1)) / 2

            Else
                Return Buffer(Buffer.Length \ 2)
            End If
        End Function

        Public Shared Function MedianOfModulus(input(,) As Double) As Double 'Gives median Of absolute values Of the original vector доделать!

            Dim Buffer() As Double = New Double(input.Length) {}

            Dim i, j As Integer

            For i = 0 To input.GetLength(0) - 1
                For j = 0 To input.GetLength(1) - 1
                    Buffer(i) = Math.Abs(input(i, j))
                Next j
            Next i
            Array.Sort(Buffer)
            If (Buffer.Length Mod 2 = 0) Then

                Return (Buffer(Buffer.Length / 2) + Buffer(Buffer.Length / 2 - 1)) / 2
            Else
                Return Buffer(Buffer.Length \ 2)
            End If
        End Function

        Public Shared Function ErrorNorm(clear() As Double, filtrated() As Double) 'Like SNR, but opposite: ErrorNorm = Norm(Filtrated - Clear) / Norm(Clear) 

            Dim ErrorArr() As Double = New Double(clear.Length) {}
            For i = 0 To clear.Length - 1
                ErrorArr(i) = filtrated(i) - clear(i)
            Next
            Return GetNorm(ErrorArr) / GetNorm(clear)
        End Function

        Public Shared Function ErrorNorm(clear(,) As Double, filtrated(,) As Double) 'Like SNR, but opposite: ErrorNorm = Norm(Filtrated - Clear) / Norm(Clear) 

            Dim ErrorArr(,) As Double = New Double(clear.GetLength(0), clear.GetLength(1)) {}

            For i = 0 To clear.GetLength(0) - 1
                For j = 0 To clear.GetLength(1) - 1
                    ErrorArr(i, j) = filtrated(i, j) - clear(i, j)
                Next
            Next
            Return GetNorm(ErrorArr) / GetNorm(clear)

        End Function

        Private Shared Function GetNorm(input() As Double) As Double 'Euclidean norm

            Dim Buff As Double = 0
            For i = 0 To input.Length - 1
                Buff += input(i) * input(i)
            Next
            Return Math.Sqrt(Buff)
        End Function

        Private Shared Function GetNorm(input(,) As Double) As Double 'Euclidean norm 2D

            Dim Buff As Double = 0
            For i = 0 To input.GetLength(0) - 1

                For j = 0 To input.GetLength(1) - 1
                    Buff += input(i, j) * input(i, j)
                Next
            Next

            Return Math.Sqrt(Buff)

        End Function

        Public Shared Function Scale(fromMin As Double, fromMax As Double, toMin As Double, toMax As Double, x As Double) As Double

            If fromMax - fromMin = 0 Then Return 0
            Dim value As Double = (toMax - toMin) * (x - fromMin) / (fromMax - fromMin) + toMin

            If (value > toMax) Then
                value = toMax
            End If
            If (value < toMin) Then
                value = toMin
            End If

            Return value

        End Function
    End Class

End Namespace



