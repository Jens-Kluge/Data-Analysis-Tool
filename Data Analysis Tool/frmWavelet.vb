Imports MathNet.Numerics
Imports OxyPlot
Imports Data_Analysis_Tool.WaveletTransform

Public Class frmWavelet

    Private Sub frmWavelet_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        ExcelRefedit1.ExcelConnector = Globals.ThisAddIn.Application
        refeditOutput.ExcelConnector = Globals.ThisAddIn.Application
        ExcelRefedit1.ActiveControl = ExcelRefedit1.txtAddress

        Dim addr As String = Globals.ThisAddIn.Application.Selection.Address(False, False)
        If ExcelRefedit1.IncludeSheetName Then
            addr = "'" & ExcelRefedit1.ExcelConnector.ActiveSheet.name & "'!" & addr
        End If
        ExcelRefedit1.Address = addr

    End Sub

    Private Sub btnWavelet_Click(sender As Object, e As System.EventArgs) Handles btnWavelet.Click
        ComputeWaveletTransform()
    End Sub

    Sub ComputeWaveletTransform()

        Dim rginput As Excel.Range
        Dim ok As Boolean

        modUtilities.GetRange(rginput, ExcelRefedit1.txtAddress.Text, ok)

        If rginput Is Nothing Then Exit Sub

        If rginput.Columns.Count > 2 Then
            MsgBox("Input range cannot have more than two columns")
            Exit Sub
        End If


        Dim sample(rginput.Rows.Count) As Double
        Dim output(rginput.Rows.Count) As Double

        Dim wvlts As List(Of CWavelet)

        If cmbWvltType.SelectedIndex = wvltTypes.Daubechies Then
            wvlts = CWavelet.WaveletConstructor.CreateAllDaubechies()
        ElseIf cmbWvltType.SelectedIndex = wvltTypes.Coiflet Then
            wvlts = CWavelet.WaveletConstructor.CreateAllCoiflets()
        Else
            wvlts = CWavelet.WaveletConstructor.CreateAllSymlets()
        End If


        'read in data
        For i = 1 To rginput.Rows.Count
            sample(i - 1) = CDbl(rginput(i, 1).value)
        Next

        'compute wavelet transform
        Transform.FastForward1d(sample, output, wvlts(cmbLevel.SelectedIndex), cmbLevel.SelectedIndex)
        Dim detail As Double()
        detail = Transform.GetDetailOfLevel(output, cmbLevel.SelectedIndex, cmbLevel.SelectedIndex)
        'Plot the signal
        PlotDataSeries(sample, PlotView1)
        'Plot the result
        PlotDataSeries(detail, PlotView2)

        'Export the output to excel
        Dim rgoutput As Excel.Range
        modUtilities.GetRange(rgoutput, refeditOutput.txtAddress.Text, ok)

        'no output range selected => nothing to insert
        If rgoutput Is Nothing Then Exit Sub

        rgoutput = rgoutput.Resize(sample.Length + 1, 1)
        rgoutput(1, 1).Value = "Signal"

        For i = 1 To sample.Length
            rgoutput(i + 1, 1) = output(i - 1)
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
    ''' Plot a data series
    ''' </summary>
    ''' <param name="values"></param>
    Sub PlotDataSeries(values() As Double, pvData As WindowsForms.PlotView)

        Dim plotModel1 As New PlotModel()

        Dim linearAxis1 As New Axes.LinearAxis() With {.Position = Axes.AxisPosition.Bottom, .Title = "sample"}
        plotModel1.Axes.Add(linearAxis1)

        Dim linearAxis2 As New Axes.LinearAxis() With {.Title = "signal value"}
        plotModel1.Axes.Add(linearAxis2)

        Dim lineSeries1 As New Series.LineSeries With
        {
                    .Color = OxyColors.Blue,
                    .MarkerFill = OxyColors.Transparent,
                    .DataFieldX = "Signal",
                    .DataFieldY = "Sample",
                    .MarkerType = MarkerType.Circle
        }

        For i = 1 To values.Length
            lineSeries1.Points.Add(New DataPoint(i, values(i - 1)))
        Next


        lineSeries1.Title = "Signal"
        plotModel1.Series.Add(lineSeries1)
        pvData.Model = plotModel1
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

        lineSeries1.Title = "Wavelet Transform"
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

    Private Sub cmbWvltType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbWvltType.SelectedIndexChanged

        Select Case cmbWvltType.SelectedIndex

            Case wvltTypes.Daubechies

                cmbLevel.Items.Clear()
                For i = 1 To 20
                    cmbLevel.Items.Add(i)
                Next

            Case wvltTypes.Coiflet

                cmbLevel.Items.Clear()
                For i = 1 To 5
                    cmbLevel.Items.Add(i)
                Next

            Case wvltTypes.Symlet

                cmbLevel.Items.Clear()
                For i = 1 To 10
                    cmbLevel.Items.Add(i)
                Next

        End Select
    End Sub

    Enum wvltTypes
        Daubechies = 0
        Coiflet = 1
        Symlet = 2
    End Enum

End Class