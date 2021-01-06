Module modFFT

    ' Radix-2 FFT von Murphy McCauley
    Private Const PI As Double = 3.14159265358979
    Private Const PI2 As Double = PI * 2

    Private m_lngP2(16) As Long

    Private Function ReverseBits(ByVal Index As Long, NumBits As Long) As Long
        Dim i As Long, Rev As Long

        For i = 0& To NumBits - 1&
            Rev = (Rev * 2&) Or (Index And 1&)
            Index = Index \ 2&
        Next

        ReverseBits = Rev
    End Function

    Public Sub RealFFT(
    ByVal NumSamples As Long,
    RealIn() As Double,
    RealOut() As Double, ImagOut() As Double,
    Optional InverseTransform As Boolean = False)

        Dim AngleNumerator As Double

        Dim NumBits As Long

        Dim i As Long, j As Long
        Dim K As Long, n As Long

        Dim BlockSize As Long, BlockEnd As Long

        Dim DeltaAngle As Double, DeltaAr As Double
        Dim Alpha As Double, Beta As Double

        Dim TR As Double, TI As Double
        Dim AR As Double, AI As Double

        If m_lngP2(0) = 0 Then
            For i = 0 To 16
                m_lngP2(i) = 2 ^ i
            Next
        End If

        If InverseTransform Then
            AngleNumerator = -PI2
        Else
            AngleNumerator = PI2
        End If

        For i = 0 To 16
            If (NumSamples And m_lngP2(i)) <> 0 Then
                NumBits = i
                Exit For
            End If
        Next

        For i = 0 To (NumSamples - 1)
            j = ReverseBits(i, NumBits)
            RealOut(j) = RealIn(i)
            ImagOut(j) = 0
        Next

        BlockEnd = 1
        BlockSize = 2

        Do While BlockSize <= NumSamples
            DeltaAngle = AngleNumerator / BlockSize
            Alpha = Math.Sin(0.5 * DeltaAngle)
            Alpha = 2.0# * Alpha * Alpha
            Beta = Math.Sin(DeltaAngle)

            i = 0
            Do While i < NumSamples
                AR = 1.0#
                AI = 0#

                j = i
                For n = 0 To BlockEnd - 1
                    K = j + BlockEnd
                    TR = AR * RealOut(K) - AI * ImagOut(K)
                    TI = AI * RealOut(K) + AR * ImagOut(K)
                    RealOut(K) = RealOut(j) - TR
                    ImagOut(K) = ImagOut(j) - TI
                    RealOut(j) = RealOut(j) + TR
                    ImagOut(j) = ImagOut(j) + TI
                    DeltaAr = Alpha * AR + Beta * AI
                    AI = AI - (Alpha * AI - Beta * AR)
                    AR = AR - DeltaAr
                    j = j + 1
                Next

                i = i + BlockSize

            Loop

            BlockEnd = BlockSize
            BlockSize = BlockSize * 2
        Loop

        If InverseTransform Then
            For i = 0 To NumSamples - 1
                RealOut(i) = RealOut(i) / NumSamples
                ImagOut(i) = ImagOut(i) / NumSamples
            Next
        End If
    End Sub

End Module
