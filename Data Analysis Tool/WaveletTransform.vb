
Imports System

Namespace WaveletTransform

    Public Class Transform

        Public Shared Sub FastForward1d(input() As Double, ByRef output() As Double, wavelet As CWavelet, Level As Integer)

            Dim Len As Integer = wavelet.DecompositionLow.Length
            Dim CircleInd As Integer
            output = New Double(input.Length) {}
            Dim Buff() As Double = New Double(input.Length) {}
            Buffer.BlockCopy(input, 0, output, 0, input.Length * 8)
            Dim BufScal As Double = 0
            Dim BufDet As Double = 0
            Dim DecLow() As Double = New Double(Len) {}
            Dim DecHigh() As Double = New Double(Len) {}

            For i As Integer = 0 To Len - 1
                DecLow(i) = wavelet.DecompositionLow(i)
                DecHigh(i) = wavelet.DecompositionHigh(i)
            Next


            Dim Bound As Integer
            Dim StartIndex As Integer

            For _level = 0 To Level - 1

                Bound = input.Length >> _level
                StartIndex = -((Len >> 1) - 1)
                Buffer.BlockCopy(output, 0, Buff, 0, Bound * 8)

                For i As Integer = 0 To (Bound >> 1) - 1

                    Dim j As Integer = StartIndex
                    Dim k As Integer = 0

                    While k < Len
                        If (StartIndex < 0) OrElse j >= Bound Then
                            CircleInd = ((j Mod Bound) + Bound) Mod Bound
                        Else
                            CircleInd = j
                        End If
                        BufScal += DecLow(k) * output(CircleInd)
                        BufDet += DecHigh(k) * output(CircleInd)
                        j += 1 : k += 1
                    End While

                    StartIndex += 2
                    Buff(i) = BufScal
                    Buff(i + (Bound >> 1)) = BufDet
                    BufScal = 0
                    BufDet = 0
                Next 'i
                Buffer.BlockCopy(Buff, 0, output, 0, Bound * 8)
            Next 'level


        End Sub

        Public Shared Sub FastInverse1d(input() As Double, ByRef output() As Double, wavelet As CWavelet, startLevel As Integer)

            Dim Len As Integer = wavelet.ReconstructionLow.Length
            Dim CircleInd As Integer
            output = New Double(input.Length) {}
            Dim Buff() As Double = New Double(input.Length) {}
            Dim BufferLow() As Double = New Double(input.Length) {}
            Dim BufferHigh() As Double = New Double(input.Length) {}
            Buffer.BlockCopy(input, 0, output, 0, input.Length * 8)
            Dim Buf As Double = 0
            Dim RecLow() As Double = New Double(Len) {}
            Dim RecHigh() As Double = New Double(Len) {}

            For i As Integer = 0 To Len - 1

                RecLow(i) = wavelet.ReconstructionLow(i)
                RecHigh(i) = wavelet.ReconstructionHigh(i)
            Next

            For level = startLevel To 1 Step -1

                Dim Bound As Integer = input.Length >> level
                Dim StartIndex As Integer = -((Len >> 1) - 1)

                Dim i As Integer = 0
                Dim j As Integer = 0
                While i < (Bound << 1)

                    BufferLow(i) = 0
                    BufferHigh(i) = 0
                    BufferLow(i + 1) = output(j)
                    BufferHigh(i + 1) = output(Bound + j)
                    i += 2 : j += 1

                End While
                Dim k As Integer

                For i = 0 To (Bound << 1) - 1

                    j = StartIndex
                    k = 0
                    While k < Len

                        If (StartIndex < 0) OrElse j >= (Bound << 1) Then
                            CircleInd = (j Mod (Bound << 1) + (Bound << 1)) Mod (Bound << 1)
                        Else
                            CircleInd = j
                        End If

                        Buf += RecLow(k) * BufferLow(CircleInd) + RecHigh(k) * BufferHigh(CircleInd)
                        j += 1
                        k += 1

                    End While


                    StartIndex += 1
                    Buff(i) = Buf
                Next i

                Buffer.BlockCopy(Buff, 0, output, 0, Bound * 16)

            Next
        End Sub


        Private Shared Function StepForward(input() As Double, wavelet As CWavelet, CurrentLevel As Integer) As Double()

            Dim Bound As Integer = input.Length >> CurrentLevel
            Dim Len As Integer = wavelet.DecompositionLow.Length
            Dim StartIndex As Integer = -((Len >> 1) - 1)
            Dim Output() As Double = New Double(input.Length) {}
            Array.Copy(input, Bound, Output, Bound, input.Length - Bound)
            Dim BufScal As Double = 0
            Dim BufDet As Double = 0
            Dim CircleInd As Integer

            Dim i As Integer = 0
            Dim r As Integer = 0
            While i < Bound

                Dim j As Integer = StartIndex
                Dim k As Integer = 0
                While k < Len

                    If (StartIndex < 0) OrElse j >= Bound Then
                        CircleInd = ((j Mod Bound) + Bound) Mod Bound
                    Else
                        CircleInd = j
                    End If

                    BufScal += wavelet.DecompositionLow(k) * input(CircleInd)
                    BufDet += wavelet.DecompositionHigh(k) * input(CircleInd)
                    j += 1
                    k += 1
                End While

                StartIndex += 2
                Output(r) = BufScal
                Output(r + (Bound >> 1)) = BufDet
                BufScal = 0
                BufDet = 0
                i += 2 : r += 1
            End While

            Return Output
        End Function

        Public Shared Sub Forward1D(input() As Double, ByRef output() As Double, wavelet As CWavelet, Level As Integer)

            Dim f As Boolean = (input.Length And (input.Length - 1)) = 0
            If Not f OrElse input.Length = 0 Then Throw New Exception("input length must be a power of two")

            output = New Double(input.Length) {}
            Array.Copy(input, output, input.Length)
            For i As Integer = 0 To Level - 1
                output = StepForward(output, wavelet, i)
            Next
        End Sub

        Private Shared Function StepInverse(input() As Double, wavelet As CWavelet, CurrentLevel As Integer) As Double()

            Dim Bound As Integer = input.Length >> CurrentLevel
            Dim Len As Integer = wavelet.ReconstructionLow.Length
            Dim StartIndex As Integer = -((Len >> 1) - 1)
            Dim Output() As Double = New Double(input.Length) {}
            Dim BuffLow() As Double = New Double(Bound << 1) {}
            Dim BuffHi() As Double = New Double(Bound << 1) {}
            Array.Copy(input, 0, Output, 0, input.Length)
            Dim BufScal As Double = 0
            Dim CircleInd As Integer

            Dim i As Integer = 0
            Dim j As Integer = 0
            While i < (Bound << 1)

                BuffLow(i) = 0
                BuffHi(i) = 0
                BuffLow(i + 1) = input(j)
                BuffHi(i + 1) = input(Bound + j)
                i += 2 : j += 1
            End While

            For i = 0 To (Bound << 1) - 1

                Dim k As Integer = 0
                For j = StartIndex To Len - 1

                    If StartIndex < 0 OrElse j >= (Bound << 1) Then
                        CircleInd = (j Mod (Bound << 1) + (Bound << 1)) Mod (Bound << 1)
                    Else
                        CircleInd = j
                    End If
                    BufScal += wavelet.ReconstructionLow(k) * BuffLow(CircleInd) + wavelet.ReconstructionHigh(k) * BuffHi(CircleInd)
                    k += 1
                Next
                StartIndex += 1
                Output(i) = BufScal
                BufScal = 0

            Next
            Return Output
        End Function


        Public Shared Sub Inverse1D(input() As Double, ByRef output() As Double, wavelet As CWavelet, StartLevel As Integer)

            Dim f As Boolean = (input.Length And (input.Length - 1)) = 0
            If Not f OrElse input.Length = 0 Then Throw New Exception("input length must be a power of two")

            output = New Double(input.Length) {}
            Array.Copy(input, output, input.Length)
            For i As Integer = StartLevel To 1 Step -1
                output = StepInverse(output, wavelet, i)
            Next
        End Sub


        Private Shared Sub FastStepForward(ByRef input() As Double, buff() As Double, len As Integer, CurrentLevel As Integer, DecLow() As Double, DecHigh() As Double)

            Dim CircleInd As Integer
            Dim BufScal As Double = 0
            Dim BufDet As Double = 0
            Dim Bound As Integer = input.Length >> CurrentLevel
            Dim StartIndex As Integer = -((len >> 1) - 1)

            For i As Integer = 0 To (Bound >> 1) - 1
                Dim k As Integer = 0
                Dim j As Integer = StartIndex
                While k < len

                    If StartIndex < 0 OrElse j >= Bound Then
                        CircleInd = ((j Mod Bound) + Bound) Mod Bound
                    Else
                        CircleInd = j
                    End If

                    BufScal += DecLow(k) * input(CircleInd)
                    BufDet += DecHigh(k) * input(CircleInd)
                    j += 1 : k += 1
                End While

                StartIndex += 2
                buff(i) = BufScal
                buff(i + (Bound >> 1)) = BufDet
                BufScal = 0
                BufDet = 0

            Next 'i
            Buffer.BlockCopy(buff, 0, input, 0, Bound * 8)

        End Sub

        Private Shared Sub FastStepInverse(ByRef input() As Double, buffLow() As Double, buffHigh() As Double, len As Integer, CurrentLevel As Integer, RecLow() As Double, RecHigh() As Double)

            Dim Bound As Integer = input.Length >> CurrentLevel
            Dim StartIndex As Integer = -((len >> 1) - 1)
            Dim Buf As Double = 0
            Dim CircleInd As Integer

            Dim i As Integer = 0
            Dim j As Integer = 0
            While i < (Bound << 1)

                buffLow(i) = 0
                buffHigh(i) = 0
                buffLow(i + 1) = input(j)
                buffHigh(i + 1) = input(Bound + j)
                i += 2 : j += 1
            End While

            For i = 0 To (Bound << 1) - 1
                j = StartIndex
                Dim k As Integer = 0
                While k < len

                    If StartIndex < 0 OrElse j >= (Bound << 1) Then
                        CircleInd = (j Mod (Bound << 1) + (Bound << 1)) Mod (Bound << 1)
                    Else
                        CircleInd = j
                    End If

                    Buf += RecLow(k) * buffLow(CircleInd) + RecHigh(k) * buffHigh(CircleInd)
                    j += 1 : k += 1
                End While
                StartIndex += 1
                input(i) = Buf
                Buf = 0
            Next i

        End Sub

        Public Shared Sub FastForward2d(input(,) As Double, ByRef output(,) As Double, wavelet As CWavelet, Level As Integer)

            Dim DataLen As Integer = input.GetLength(0)
            Dim Len As Integer = wavelet.DecompositionHigh.Length
            Dim Bound As Integer

            output = New Double(DataLen, DataLen) {}
            Dim buff() As Double = New Double(DataLen) {}
            Dim buffData() As Double = New Double(DataLen) {}

            Dim DecLow() As Double = New Double(Len) {}
            Dim DecHigh() As Double = New Double(Len) {}
            For i = 0 To Len - 1
                DecLow(i) = wavelet.DecompositionLow(i)
                DecHigh(i) = wavelet.DecompositionHigh(i)
            Next

            For i = 0 To DataLen - 1
                For j = 0 To DataLen - 1
                    output(i, j) = input(i, j)
                Next
            Next

            For lev = 0 To Level - 1

                Bound = DataLen >> lev
                For i = 0 To Bound - 1
                    For j = 0 To Bound - 1
                        buffData(j) = output(i, j)
                    Next
                    FastStepForward(buffData, buff, Len, lev, DecLow, DecHigh)
                    For j = 0 To Bound - 1
                        output(i, j) = buffData(j)
                    Next j
                Next i

                For j = 0 To Bound - 1
                    For i = 0 To Bound - 1
                        buffData(i) = output(i, j)
                    Next i

                    FastStepForward(buffData, buff, Len, lev, DecLow, DecHigh)
                    For i = 0 To Bound - 1
                        output(i, j) = buffData(i)
                    Next
                Next j

            Next lev
        End Sub

        Public Shared Sub FastInverse2d(input(,) As Double, ByRef output(,) As Double, wavelet As CWavelet, Level As Integer)

            Dim DataLen As Integer = input.GetLength(0)
            Dim Len As Integer = wavelet.DecompositionHigh.Length
            Dim Bound As Integer
            output = New Double(DataLen, DataLen) {}
            Dim buffData() As Double = New Double(DataLen) {}
            Dim buffLow() As Double = New Double(DataLen) {}
            Dim buffHigh() As Double = New Double(DataLen) {}

            Dim RecLow() As Double = New Double(Len) {}
            Dim RecHigh() As Double = New Double(Len) {}

            For i = 0 To Len - 1
                RecLow(i) = wavelet.ReconstructionLow(i)
                RecHigh(i) = wavelet.ReconstructionHigh(i)
            Next i

            For i = 0 To DataLen - 1
                For j = 0 To DataLen - 1
                    output(i, j) = input(i, j)
                Next j
            Next i

            For lev = Level To 1 Step -1

                Bound = DataLen >> lev
                For j = 0 To (Bound << 1) - 1

                    For i = 0 To (Bound << 1) - 1
                        buffData(i) = output(i, j)
                    Next i

                    FastStepInverse(buffData, buffLow, buffHigh, Len, lev, RecLow, RecHigh)

                    For i = 0 To (Bound << 1) - 1
                        output(i, j) = buffData(i)
                    Next i
                Next j

                For i = 0 To (Bound << 1) - 1

                    For j = 0 To (Bound << 1) - 1
                        buffData(j) = output(i, j)
                    Next j

                    FastStepInverse(buffData, buffLow, buffHigh, Len, lev, RecLow, RecHigh)

                    For j = 0 To (Bound << 1) - 1
                        output(i, j) = buffData(j)
                    Next j
                Next i
            Next lev

        End Sub

        Public Shared Function GetAllDetail(input() As Double, currentLevel As Integer) As Double()

            Dim Shift As Integer = input.Length >> currentLevel
            Dim output() As Double = New Double(input.Length - Shift) {}
            Array.Copy(input, Shift, output, 0, input.Length - Shift)
            Return output
        End Function

        Public Shared Function GetAllDetail(input(,) As Double, currentLevel As Integer) As Double()

            Dim Len As Integer = input.GetLength(0) >> currentLevel
            Dim output() As Double = New Double(input.GetLength(0) * input.GetLength(0) - (Len * Len)) {}
            Dim OutIndex As Integer = 0

            For lev = currentLevel To 1 Step -1

                Dim Bound As Integer = input.GetLength(0) >> lev - 1
                Len = input.GetLength(0) >> lev
                For i = 0 To Len - 1
                    For j = Len To Bound - 1
                        output(OutIndex) = input(i, j)
                        OutIndex += 1
                    Next j
                Next i

                For i = Len To Bound - 1
                    For j = 0 To Len - 1
                        output(OutIndex) = input(i, j)
                        OutIndex += 1
                    Next j
                Next i

                For i = Len To Bound - 1
                    For j = Len To Bound - 1
                        output(OutIndex) = input(i, j)
                        OutIndex += 1
                    Next j
                Next i

            Next lev
            Return output

        End Function

        Public Shared Function GetDetailOfLevel(input(,) As Double, level As Integer) As Double()

            Dim Len As Integer = input.GetLength(0) >> level
            Dim lev As Integer = level
            Dim Bound As Integer = input.GetLength(0) >> lev - 1
            ''Double[] output = New Double[input.GetLength(0)* input.GetLength(0) - (Len * Len) ];
            Dim output() As Double = New Double(Bound * Bound - (Len * Len)) {}
            Dim OutIndex As Integer = 0

            For i = 0 To Len - 1
                For j = Len To Bound - 1
                    output(OutIndex) = input(i, j)
                    OutIndex += 1
                Next j
            Next i

            For i = Len To Bound - 1
                For j = 0 To Len - 1
                    output(OutIndex) = input(i, j)
                    OutIndex += 1
                Next j
            Next i

            For i = Len To Bound - 1
                For j = Len To Bound - 1
                    output(OutIndex) = input(i, j)
                    OutIndex += 1
                Next j
            Next i

            Return output
        End Function

        Public Shared Sub SetAllDetail(input() As Double, coefs(,) As Double, currentLevel As Integer)

            Dim Shift As Integer = coefs.Length >> currentLevel
            Dim Len As Integer = coefs.GetLength(0) >> currentLevel
            Dim InpIndex As Integer = 0

            ''for (int lev = 1; lev > currentLevel; lev++)
            For lev = currentLevel To 1 Step -1

                Dim Bound As Integer = coefs.GetLength(0) >> lev - 1
                Len = coefs.GetLength(0) >> lev
                For i = 0 To Len - 1
                    For j = Len To Bound - 1
                        coefs(i, j) = input(InpIndex)
                        InpIndex += 1
                    Next j
                Next i

                For i = Len To Bound - 1
                    For j = 0 To Len - 1
                        coefs(i, j) = input(InpIndex)
                        InpIndex += 1
                    Next j
                Next i

                For i = Len To Bound - 1
                    For j = Len To Bound - 1
                        coefs(i, j) = input(InpIndex)
                        InpIndex += 1
                    Next
                Next

            Next lev
        End Sub

        Public Shared Sub SetDetailofLevel(input() As Double, coefs(,) As Double, level As Integer)

            Dim Len As Integer = coefs.GetLength(0) >> level
            Dim InpIndex As Integer = 0

            Dim lev As Integer = level
            Dim Bound As Integer = (coefs.GetLength(0) >> lev) - 1
            Len = coefs.GetLength(0) >> lev

            For i = 0 To Len - 1
                For j = Len To Bound - 1
                    coefs(i, j) = input(InpIndex)
                    InpIndex += 1
                Next j
            Next i

            For i = Len To Bound - 1
                For j = 0 To Len - 1
                    coefs(i, j) = input(InpIndex)
                    InpIndex += 1
                Next j
            Next i

            For i = Len To Bound - 1
                For j = Len To Bound - 1
                    coefs(i, j) = input(InpIndex)
                    InpIndex += 1
                Next j
            Next i

        End Sub

        Public Shared Function GetDetailOfLevel(Input() As Double, currentLevel As Integer, level As Integer) As Double()
            Dim Len As Integer = (Input.Length >> level)
            Dim output() As Double = New Double(Len) {}
            Array.Copy(Input, Len, output, 0, Len)
            Return output
        End Function

        Public Shared Function GetScaling(input() As Double, currentLevel As Integer) As Double()
            Dim Len As Integer = input.Length >> currentLevel
            Dim output() As Double = New Double(Len) {}
            Array.Copy(input, output, Len)
            Return output
        End Function

        Public Shared Sub SetDetailOfLevel(input() As Double, inpDetails() As Double)
            Array.Copy(inpDetails, 0, input, inpDetails.Length, inpDetails.Length)
        End Sub

    End Class
End namespace
