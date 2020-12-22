Imports OxyPlot
Imports System.Windows.Forms

Public Class frmCorr
    Private Sub btnCorr_Click(sender As Object, e As System.EventArgs) Handles btnCorr.Click

        Dim rginput1, rgInput2 As Excel.Range
        Dim ok As Boolean
        Dim bAutoCorr As Boolean = False
        Dim objInput1(,), objInput2(,) As Object
        Dim app As Excel.Application = Globals.ThisAddIn.Application
        Dim wsf As Excel.WorksheetFunction = app.WorksheetFunction


        modUtilities.GetRange(rginput1, ExcelRefedit1.txtAddress.Text, ok)
        If Not ok Then
            MsgBox("Input range 1 is not valid")
            Exit Sub
        End If

        modUtilities.GetRange(rgInput2, ExcelRefedit2.txtAddress.Text, ok)
        If Not ok Then
            MsgBox("Input range 2 is not valid")
            Exit Sub
        End If

        If rginput1.Address = rgInput2.Address Then
            bAutoCorr = True
        End If

        Me.Cursor = Cursors.WaitCursor
        btnCorr.Enabled = False
        Try

            objInput1 = rginput1.Value2
            objInput2 = rgInput2.Value2
            Dim dblInput1(objInput1.Length, 0) As Double
            Dim values1(objInput1.Length) As Double
            Dim dblInput2(objInput2.Length, 0) As Double
            Dim values2(objInput2.Length) As Double
            Dim lg As LagCorr

            'extract first columns from 2D array and copy it into 1D arrays
            Array.Copy(objInput1, dblInput1, objInput1.Length)
            Array.Copy(objInput2, dblInput2, objInput2.Length)
            'inefficient, but found no better way to convert 2D to 1D
            For i = 0 To dblInput1.Length - 1
                values1(i) = dblInput1(i, 0)
            Next
            For i = 0 To dblInput2.Length - 1
                values2(i) = dblInput2(i, 0)
            Next

            If bAutoCorr Then
                Dim outvals() As Double
                outvals = modUtilities.AutoCorrelation(values1)
                PlotAutoCorr(outvals)
            Else
                lg = modUtilities.CrossCorrelation(values1, values2)
                PlotCorrelation(lg)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        btnCorr.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub PlotAutoCorr(vals() As Double)
        Dim plotModel1 As New PlotModel()

        Dim linearAxis1 As New Axes.LinearAxis() With {.Position = Axes.AxisPosition.Bottom, .Title = "Lag"}
        plotModel1.Axes.Add(linearAxis1)

        Dim linearAxis2 As New Axes.LinearAxis() With {.Title = "Correlation"}
        plotModel1.Axes.Add(linearAxis2)

        Dim lineSeries1 As New Series.LineSeries With
        {
                    .Color = OxyColors.Blue,
                    .MarkerFill = OxyColors.Transparent,
                    .DataFieldX = "Lag",
                    .DataFieldY = "Correlation",
                    .MarkerType = MarkerType.Circle
        }

        For i = 0 To vals.Length - 1
            lineSeries1.Points.Add(New DataPoint(i, vals(i)))
        Next

        lineSeries1.Title = "Correlation"
        plotModel1.Series.Add(lineSeries1)
        PlotView1.Model = plotModel1
    End Sub

    Private Sub PlotCorrelation(lg As LagCorr)

        Dim plotModel1 As New PlotModel()

        Dim linearAxis1 As New Axes.LinearAxis() With {.Position = Axes.AxisPosition.Bottom, .Title = "Lag"}
        plotModel1.Axes.Add(linearAxis1)

        Dim linearAxis2 As New Axes.LinearAxis() With {.Title = "Correlation"}
        plotModel1.Axes.Add(linearAxis2)

        Dim lineSeries1 As New Series.LineSeries With
        {
                    .Color = OxyColors.Blue,
                    .MarkerFill = OxyColors.Transparent,
                    .DataFieldX = "Lag",
                    .DataFieldY = "Correlation",
                    .MarkerType = MarkerType.Circle
        }

        For i = 0 To lg.Corr.Length - 2
            lineSeries1.Points.Add(New DataPoint(lg.Lag(i), lg.Corr(i)))
        Next

        lineSeries1.Title = "Correlation"
        plotModel1.Series.Add(lineSeries1)
        PlotView1.Model = plotModel1
    End Sub


    Private Sub frmCorr_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        ExcelRefedit1.ExcelConnector = Globals.ThisAddIn.Application
        ExcelRefedit2.ExcelConnector = Globals.ThisAddIn.Application
        ExcelRefedit3.ExcelConnector = Globals.ThisAddIn.Application
        ExcelRefedit1.ActiveControl = ExcelRefedit1.txtAddress

        Dim addr As String = Globals.ThisAddIn.Application.Selection.Address(False, False)
        If ExcelRefedit1.IncludeSheetName Then
            addr = "'" & ExcelRefedit1.ExcelConnector.ActiveSheet.name & "'!" & addr
        End If
        ExcelRefedit1.Address = addr
    End Sub

    Private Sub btnExtend_Click(sender As Object, e As System.EventArgs) Handles btnExtend.Click

        Dim rg1 As Excel.Range = Nothing, rg2 As Excel.Range = Nothing
        Dim ok As Boolean = False
        Dim addr As String


        modUtilities.GetRange(rg1, ExcelRefedit1.Address, ok)
        If Not ok Then Exit Sub

        modUtilities.ExtendRange(rg1, rg2)
        rg2.Select()
        addr = rg2.Address(False, False)
        If ExcelRefedit1.IncludeSheetName Then
            addr = "'" & ExcelRefedit1.ExcelConnector.ActiveSheet.name & "'!" & addr
        End If
        ExcelRefedit1.Address = addr
    End Sub

    Private Sub btnExtend2_Click(sender As Object, e As System.EventArgs) Handles btnExtend2.Click

        Dim rg1 As Excel.Range = Nothing, rg2 As Excel.Range = Nothing
        Dim ok As Boolean = False
        Dim addr As String

        modUtilities.GetRange(rg1, ExcelRefedit2.Address, ok)
        If Not ok Then Exit Sub

        modUtilities.ExtendRange(rg1, rg2)
        rg2.Select()
        addr = rg2.Address(False, False)
        If ExcelRefedit2.IncludeSheetName Then
            addr = "'" & ExcelRefedit2.ExcelConnector.ActiveSheet.name & "'!" & addr
        End If
        ExcelRefedit2.Address = addr
    End Sub
End Class