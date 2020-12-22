Imports MathNet.Numerics
Imports OxyPlot

Public Class frmFFT

    Private Sub frmFFT_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        ExcelRefedit1.ExcelConnector = Globals.ThisAddIn.Application
        refeditOutput.ExcelConnector = Globals.ThisAddIn.Application
        ExcelRefedit1.ActiveControl = ExcelRefedit1.txtAddress

        Dim addr As String = Globals.ThisAddIn.Application.Selection.Address(False, False)
        If ExcelRefedit1.IncludeSheetName Then
            addr = "'" & ExcelRefedit1.ExcelConnector.ActiveSheet.name & "'!" & addr
        End If
        ExcelRefedit1.Address = addr

    End Sub

    Private Sub btnFFT_Click(sender As Object, e As System.EventArgs) Handles btnFFT.Click
        ComputeFFT()
    End Sub

    Sub ComputeFFT(Optional bImagPartInSecondCol As Boolean = False)

        Dim rginput As Excel.Range
        Dim ok As Boolean

        modUtilities.GetRange(rginput, ExcelRefedit1.txtAddress.Text, ok)

        If rginput Is Nothing Then Exit Sub

        If rginput.Columns.Count > 2 Then
            MsgBox("Input range cannot have more than two columns")
            Exit Sub
        End If

        If bImagPartInSecondCol And rginput.Columns.Count < 2 Then
            MsgBox("Select second column for imaginary part")
            Exit Sub
        End If

        Dim sample(rginput.Rows.Count) As Complex32

        If bImagPartInSecondCol Then
            For i = 1 To rginput.Rows.Count
                sample(i - 1) = New Complex32(rginput(i, 1).value, rginput(i, 2).value)
            Next
        Else
            For i = 1 To rginput.Rows.Count
                sample(i - 1) = New Complex32(rginput(i, 1).value, 0.0)
            Next
        End If

        IntegralTransforms.Fourier.Forward(sample)

        'Plot the result
        'PlotComplexSeries(sample)
        PlotAmplitudeFreqDiagram(sample)
        PlotPhaseFreqDiagram(sample)

        'Export the output to excel
        Dim rgoutput As Excel.Range


        modUtilities.GetRange(rgoutput, refeditOutput.txtAddress.Text, ok)

        'no output range selected => nothing to insert
        If rgoutput Is Nothing Then Exit Sub

        rgoutput = rgoutput.Resize(sample.Length + 1, 1)
        rgoutput(1, 1).Value = "Real Part"
        rgoutput(1, 2).value = "Imaginary Part"

        For i = 1 To sample.Length
            rgoutput(i + 1, 1) = sample(i - 1).Real
            rgoutput(i + 1, 2) = sample(i - 1).Imaginary
        Next

    End Sub

    Sub FourierTest()
        Dim sg As New SignalGenerator
        Dim dblVals() As Double = sg.Sawtooth(1001)
        Dim cmplxVals() As Complex32 = ToComplex(dblVals)
        IntegralTransforms.Fourier.Forward(cmplxVals)

        Dim outputvals(cmplxVals.Length) As Double
        ToDouble(cmplxVals, outputvals)

        'PlotComplexSeries(sample)
        PlotAmplitudeFreqDiagram(cmplxVals)
        PlotPhaseFreqDiagram(cmplxVals)

    End Sub

    ''' <summary>
    ''' Plot a series of complex values in the complex plane
    ''' </summary>
    ''' <param name="values"></param>
    Sub PlotComplexSeries(values() As Complex32)

        Dim plotModel1 As New PlotModel()

        Dim linearAxis1 As New Axes.LinearAxis() With {.Position = Axes.AxisPosition.Bottom, .Title = "Real Part"}
        plotModel1.Axes.Add(linearAxis1)

        Dim linearAxis2 As New Axes.LinearAxis() With {.Title = "Imaginary Part"}
        plotModel1.Axes.Add(linearAxis2)

        Dim lineSeries1 As New Series.LineSeries With
        {
                    .Color = OxyColors.Blue,
                    .MarkerFill = OxyColors.Transparent,
                    .DataFieldX = "Real part",
                    .DataFieldY = "Imaginary part",
                    .MarkerType = MarkerType.Circle
        }

        For i = 1 To values.Length
            lineSeries1.Points.Add(New DataPoint(values(i - 1).Real, values(i - 1).Imaginary))
        Next

        lineSeries1.Title = "FFT"
        plotModel1.Series.Add(lineSeries1)
        PlotView1.Model = plotModel1
    End Sub

    Sub PlotAmplitudeFreqDiagram(values() As Complex32)

        Dim sampleUnit As Double = 1
        Dim sampleFreq As Double = 1 / sampleUnit

        'amplitude plot for upper panel
        Dim plotModel1 As New PlotModel()

        Dim linearAxis1 As New Axes.LinearAxis() With {.Position = Axes.AxisPosition.Bottom, .Title = "Frequency"}
        plotModel1.Axes.Add(linearAxis1)

        Dim linearAxis2 As New Axes.LinearAxis() With {.Title = "Amplitude"}
        plotModel1.Axes.Add(linearAxis2)

        Dim lineSeries1 As New Series.LineSeries With
        {
                    .Color = OxyColors.Blue,
                    .MarkerFill = OxyColors.Transparent,
                    .DataFieldX = "Real part",
                    .DataFieldY = "Imaginary part",
                    .MarkerType = MarkerType.Circle
        }

        'ignore first value for the plot, which contains the average
        For i = 2 To values.Length
            lineSeries1.Points.Add(New DataPoint(i * sampleFreq, values(i - 1).Magnitude))
        Next

        lineSeries1.Title = "FFT"
        plotModel1.Series.Add(lineSeries1)
        PlotView1.Model = plotModel1

    End Sub

    Sub PlotPhaseFreqDiagram(values() As Complex32)


        'todo: computation of frequency scaling
        Dim sampleUnit As Double = 1
        Dim sampleFreq As Double = 1 / sampleUnit

        Dim plotModel1 As New PlotModel()

        Dim linearAxis1 As New Axes.LinearAxis() With {.Position = Axes.AxisPosition.Bottom, .Title = "Frequency"}
        plotModel1.Axes.Add(linearAxis1)

        Dim linearAxis2 As New Axes.LinearAxis() With {.Title = "Phase"}
        plotModel1.Axes.Add(linearAxis2)

        Dim lineSeries1 As New Series.LineSeries With
        {
                    .Color = OxyColors.Blue,
                    .MarkerFill = OxyColors.Transparent,
                    .DataFieldX = "Real part",
                    .DataFieldY = "Imaginary part",
                    .MarkerType = MarkerType.Circle
        }

        For i = 1 To values.Length
            lineSeries1.Points.Add(New DataPoint(i * sampleFreq, values(i - 1).Phase))
        Next

        lineSeries1.Title = "FFT"
        plotModel1.Series.Add(lineSeries1)

        PlotView2.Model = plotModel1

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

End Class