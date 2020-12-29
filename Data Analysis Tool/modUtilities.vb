Imports Microsoft.Office.Interop
Imports System.Windows.Forms
Imports MathNet.Numerics
Imports MathNet.Numerics.Statistics
Imports mncla = MathNet.Numerics.LinearAlgebra
Imports mnc32 = MathNet.Numerics.LinearAlgebra.Complex32


Module modUtilities

#Region "Windows Forms functions"
    Public Sub BringFormsToFront()

        Dim fms As FormCollection = System.Windows.Forms.Application.OpenForms

        If (fms Is Nothing) Then
            Return
        End If

        For Each fm As Form In fms
            fm.BringToFront()
        Next

    End Sub

    Public Sub ShowForm(ByRef fm As Windows.Forms.Form, formType As Type, Optional modal As Boolean = False)


        If fm Is Nothing OrElse fm.IsDisposed() Then
            fm = Activator.CreateInstance(formType)
        End If

        If fm.Visible Then
            fm.BringToFront()
        Else
            If modal Then
                fm.ShowDialog()
            Else
                fm.Show()
            End If
        End If

    End Sub
#End Region

#Region "Excel Range functions"
    Public Sub GetRange(ByRef rg As Excel.Range, s As String, ByRef ok As Boolean)

        ' Get range with address s, set ok to true if this address Is valid
        ok = True
        Try
            rg = Globals.ThisAddIn.Application.ActiveSheet.Range(s)
            Return
        Catch ex As Exception
            ok = False
            Return
        End Try
    End Sub

    Public Sub ExtendRange(rg As Excel.Range, ByRef rg1 As Excel.Range)

        ''For a given range rg, return range rg1 which extends the first row of rg until a row of empty cells is found
        ''If the end of the kth column is reached continue downwards in next column 
        Dim n, j As Long

        Dim bFinish As Boolean

        Dim ce As Excel.Range
        Dim ce1 As Excel.Range
        Dim s As String

        If rg Is Nothing Then Exit Sub

        n = rg.Columns.Count
        ce = rg.Cells(1, 1)

        Try
            Do
                bFinish = True
                ce = ce.Offset(1, 0)

                While ce.Value IsNot Nothing
                    ce = ce.Offset(1, 0)

                    ce1 = ce.Offset(0, 1)

                    For j = 2 To n
                        'no empty cell => go down further
                        If ce1.Value IsNot Nothing Then
                            bFinish = False
                            Exit For
                        End If

                        ce1 = ce1.Offset(0, 1)
                    Next

                End While
            Loop While Not bFinish

            rg1 = rg.Cells(1, 1).Resize(ce.Row - rg.Cells(1, 1).Row, n)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    'the upper left corner is used as starting point of a range of size mxn
    'then it is tested if this range is empty
    Private Function IsEmptyRange(rgIn As Excel.Range, rgOut As Excel.Range) As Boolean

        'return true if output range Is empty
        Dim m As Long       ' # rows in output range
        Dim n As Long      ' # columns in output range
        Dim retval As Boolean

        'this should be set to the number of rows actually needed
        m = 12
        n = 2

        Dim rg1 As Excel.Range = Nothing        'range under consideration

        SetRange2(rg1, rgOut, m, n)

        If Globals.ThisAddIn.Application.WorksheetFunction.CountA(rg1) = 0 Then
            retval = True
        Else
            retval = (MessageBox.Show("Overwrite existing data?", "", MessageBoxButtons.OKCancel) = DialogResult.OK)
        End If

        Return retval
    End Function

    Sub SetRange2(ByVal rg As Excel.Range, ByVal rg0 As Excel.Range, r As Long, c As Long)
        'set rg to a range with first cell in rg And height r And width c
        rg = rg0.Cells(1, 1).Resize(r, c)
    End Sub
#End Region

    Public Function Index2Freq(i As Integer, samples As Double, nFFT As Integer) As Double
        Return i * (samples / nFFT / 2)
    End Function

    Public Class LagCorr

        Public Property Lag As Double()
        Public Property Corr As Double()

    End Class
    Public Function AutoCorrelation(x1() As Double) As Double()
        Return Correlation.Auto(x1)
    End Function

    'jk: so far i didnt succeed in writing a *normalized* cross correlation using FFT
    'so this method is not currently used
    ''' <summary>
    ''' Compute cross correlation by:
    ''' - Fourier transform the signals x1 and x2
    ''' - multiply x1 and x2*
    ''' - apply inverse Fourier transform to the product
    ''' </summary>
    ''' <param name="x1"></param>
    ''' <param name="x2"></param>
    ''' <returns></returns>
    Public Function CrossCorrelationFFT(x1() As Double, x2() As Double) As LagCorr

        If x1.Length <> x2.Length Then
            Throw New Exception("Samples must have same size.")
        End If

        Dim x1Cplx(x1.Length) As Complex32
        Dim x2Cplx(x2.Length) As Complex32

        'normalize the cross correlation with the product of the standard deviations
        For i = 1 To x1.Length
            x1Cplx(i - 1) = New Complex32(x1(i - 1), 0.0)
            x2Cplx(i - 1) = New Complex32(x2(i - 1), 0.0)
        Next

        IntegralTransforms.Fourier.Forward(x1Cplx)
        IntegralTransforms.Fourier.Forward(x2Cplx)
        'compute the conjugate of the second factor
        For i = 1 To x2Cplx.Length
            x2Cplx(i - 1) = x2Cplx(i - 1).Conjugate
        Next

        Dim vec1 = mnc32.DenseVector.Create(x1Cplx.Length, 0)
        vec1.SetValues(x1Cplx)
        Dim norm1 = vec1.L2Norm

        Dim vec2 = mnc32.DenseVector.Create(x2Cplx.Length, 0)
        vec2.SetValues(x2Cplx)
        Dim norm2 = vec2.L2Norm

        'multiply the arrays
        Dim resultCplx(x2Cplx.Length) As Complex32
        For i = 1 To x2Cplx.Length
            resultCplx(i - 1) = x1Cplx(i - 1) * x2Cplx(i - 1)
        Next

        'apply FFT inverse transform to the product
        IntegralTransforms.Fourier.Inverse(resultCplx)
        Dim cor(resultCplx.Length) As Double
        Dim lag(resultCplx.Length) As Double

        Dim norm = resultCplx.MaximumMagnitudePhase
        'store the result in a lagCorr structure
        For i = 1 To resultCplx.Length
            cor(i - 1) = resultCplx(i - 1).Real / norm.Magnitude
            lag(i - 1) = i
        Next

        Return New LagCorr With {.Corr = cor, .Lag = lag}
    End Function


    Public Function CrossCorrelation(x1() As Double, x2() As Double,
                                     Optional bZeroPadding As Boolean = False,
                                     Optional MaxTimeLag As Integer = 0) As LagCorr

        If x1.Length <> x2.Length Then
            Throw New Exception("Samples must have same size.")
        End If

        If MaxTimeLag = 0 Then
            MaxTimeLag = x1.Length
        End If

        Dim len = x1.Length
        Dim len2 = len + MaxTimeLag
        Dim len3 = len + 2 * MaxTimeLag
        Dim cor() As Double
        Dim lag() As Double

        If bZeroPadding Then

            '3x the length for zero padding before and after
            Dim s1 = New Double(len3) {}
            Dim s2 = New Double(len3) {}
            'twice the length for negative and positive lag


            'copy x1 into the center and x2 into the beginning of the
            'window of length MaxTimeLag + len + MaxTimeLag
            Array.Copy(x1, 0, s1, MaxTimeLag, len)
            Array.Copy(x2, 0, s2, 0, Len)

            cor = New Double(2 * MaxTimeLag + 1) {}
            lag = New Double(2 * MaxTimeLag + 1) {}

            For i = 0 To 2 * MaxTimeLag

                cor(i) = Correlation.Pearson(s1, s2)
                lag(i) = i - MaxTimeLag
                'move s2 forward one unit
                Array.Copy(s2, 0, s2, 1, s2.Length - 1)
                s2(0) = 0
            Next

        Else 'no zero padding
            Dim s1 = New Double(Len) {}
            Dim s2 = New Double(Len) {}


            Array.Copy(x1, s1, Len)
            Array.Copy(x2, s2, Len)

            If MaxTimeLag = 0 Then
                MaxTimeLag = len
            End If

            cor = New Double(2 * MaxTimeLag + 1) {}
            lag = New Double(2 * MaxTimeLag + 1) {}
            For i = 0 To MaxTimeLag
                cor(MaxTimeLag + i) = Correlation.Pearson(s1, s2)
                lag(MaxTimeLag + i) = i

                'discard last element of s3
                '=>end of second array is shifted back by one position
                ReDim Preserve s2(len - (i + 1))

                'beginning of first array is shifted forward by one position
                'thereby moving arr1 fwd in time and increasing lag of arr1 by one unit
                Array.Copy(s1, 1, s1, 0, s1.Length - 1)
                ReDim Preserve s1(len - (i + 1))

            Next
            'now in the other direction, for negative lags 
            s1 = New Double(len) {}
            s2 = New Double(len) {}
            Array.Copy(x1, s1, len)
            Array.Copy(x2, s2, len)
            For i = 0 To MaxTimeLag
                cor(MaxTimeLag - i) = Correlation.Pearson(s1, s2)
                lag(MaxTimeLag - i) = -i

                'discard last element of s1
                '=>end of first array is shifted back by one position
                ReDim Preserve s1(len - (i + 1))

                'beginning of second array is shifted forward by one position
                'thereby moving arr2 fwd in time and increasing lag of arr1 by one unit
                Array.Copy(s2, 1, s2, 0, s2.Length - 1)
                ReDim Preserve s2(len - (i + 1))

            Next
        End If

        Return New LagCorr With {.Corr = cor, .Lag = lag}

    End Function

End Module
