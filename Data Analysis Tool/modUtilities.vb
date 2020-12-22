Imports Microsoft.Office.Interop
Imports System.Windows.Forms
Imports MathNet.Numerics.Statistics

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

        Catch ex As exception
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

    Public Function CrossCorrelation(x1() As Double, x2() As Double) As LagCorr

        If x1.Length <> x2.Length Then
            Throw New Exception("Samples must have same size.")
        End If

        Dim len = x1.Length
        Dim len2 = 2 * len
        Dim len3 = 3 * len
        Dim s1 = New Double(len3) {}
        Dim s2 = New Double(len3) {}
        Dim cor = New Double(len2) {}
        Dim lag = New Double(len2) {}

        Array.Copy(x1, 0, s1, len, len)
        Array.Copy(x2, 0, s2, 0, len)

        For i = 0 To len2 - 1

            cor(i) = Correlation.Pearson(s1, s2)
            lag(i) = i - len
            Array.Copy(s2, 0, s2, 1, s2.Length - 1)
            s2(0) = 0
        Next

        Return New LagCorr With {.Corr = cor, .lag = lag}

    End Function

End Module
