Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Threading.Tasks
Imports System.IO
Imports Data_Analysis_Tool.SignalGenerator
Imports Data_Analysis_Tool.WaveletTransform
Imports System.Drawing

Namespace Tests

    Class Program

        Shared Sub Main(args() As String)

            Dim prog As Program = New Program()
            Dim Daublets As List(Of CWavelet) = CWavelet.WaveletConstructor.CreateAllDaubechies()
            Dim Symlets As List(Of CWavelet) = CWavelet.WaveletConstructor.CreateAllSymlets()
            Dim Coiflets As List(Of CWavelet) = CWavelet.WaveletConstructor.CreateAllCoiflets()
            ''prog.Test1();
            ''prog.TestGetSet2d(Symlets);
            Console.WriteLine("Daublet 10 length:" & Daublets(0).FilterLength)
            ''prog.Test2D(Symlets);
            ''prog.TestGetSet(Daublets);


            'System.Diagnostics.Stopwatch sw = New System.Diagnostics.Stopwatch()
            'TimeSpan ts

            'sw.Start()
            'prog.TestSafe(Symlets)
            'ts = sw.Elapsed
            'String elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10)
            'Console.WriteLine("Test Safe finished in " + elapsedTime)
            'Console.WriteLine()

            'sw.Restart()
            'prog.TestFast(Symlets)
            'ts = sw.Elapsed
            'elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10)
            'Console.WriteLine("Test Fast finished in " + elapsedTime)

            Console.Read()
        End Sub

        Sub Test1()

            Dim filePath As String = "test1.csv"
            Dim delimiter As String = ","
            Dim sb As StringBuilder = New StringBuilder()
            Dim TestSignalOne() As Double '= CreateSignal(SignalList.Doppler, 0, 1024)
            Dim TestSignalTwo() As Double = New Double(TestSignalOne.Length) {}
            Dim Daublets As List(Of CWavelet) = CWavelet.WaveletConstructor.CreateAllDaubechies()
            Transform.Forward1D(TestSignalOne, TestSignalTwo, Daublets(1), 1)
            Transform.Inverse1D(TestSignalTwo, TestSignalOne, Daublets(1), 1)
            ''''
            For i = 0 To TestSignalOne.Length - 1
                sb.AppendLine(String.Join(delimiter, TestSignalOne(i)))
            Next
            File.WriteAllText(filePath, sb.ToString())
        End Sub

        Sub TestSafe(wavelets As List(Of CWavelet))

            Dim Level As Integer = 5
            Dim Len As Integer = 2048
            'Dim len As Integer = 32768
            'dim len as integer = 65536
            'dim len as integer = 131072
            Dim signal() As Double
            Dim signal2() As Double
            Dim SumError1 As Double = 0
            Dim SumError2 As Double = 0
            Dim SumError3 As Double = 0
            'signal = CreateSignal(SignalList.Doppler, 0, Len)
            For Each wavelet As CWavelet In wavelets
                Transform.Forward1D(signal, signal2, wavelet, Level)
                Transform.Inverse1D(signal2, signal2, wavelet, Level)
                SumError1 = AuxFunctions.ErrorNorm(signal, signal2)
                Console.WriteLine(wavelet._Name & " " & SumError1)
            Next
            '    signal = CreateSignal(SignalList.Mixed, 0, Len);
            '    foreach(Wavelet wavelet In wavelets)
            '    {
            '        Transform.Forward1D(signal, out signal2, wavelet, Level);
            '        Transform.Inverse1D(signal2, out signal2, wavelet, Level);
            '        SumError2 = AuxFunctions.ErrorNorm(signal, signal2);
            '        Console.WriteLine(wavelet.Name + " " + SumError2);
            '    }
            '    signal = CreateSignal(SignalList.Rectangular, 0, Len);
            '    foreach(Wavelet wavelet In wavelets)
            '    {
            '        Transform.Forward1D(signal, out signal2, wavelet, Level);
            '        Transform.Inverse1D(signal2, out signal2, wavelet, Level);
            '        SumError3 = AuxFunctions.ErrorNorm(signal, signal2);
            '        Console.WriteLine(wavelet.Name + " " + SumError3);
            '    }
        End Sub

        'Void TestFast(List < Wavelet > wavelets)
        '{
        '    Int32 Level = 5;
        '    Int32 Len() = 2048;
        '    ''Int32 len = 32768;
        '    ''Int32 len = 65536;
        '    ''Int32 len = 131072;
        '    Double[] signal;
        '    Double[] signal2;
        '    Double SumError1 = 0;
        '    Double SumError2 = 0;
        '    Double SumError3 = 0;
        '    signal = CreateSignal(SignalList.Doppler, 0, Len);
        '    foreach(Wavelet wavelet In wavelets)
        '    {
        '        Transform.FastForward1d(signal, out signal2, wavelet, Level);
        '        Transform.FastInverse1d(signal2, out signal2, wavelet, Level);
        '        SumError1 = AuxFunctions.ErrorNorm(signal, signal2);
        '        Console.WriteLine(wavelet.Name + " " + SumError1);
        '    }
        '    signal = CreateSignal(SignalList.Mixed, 0, Len);
        '    foreach(Wavelet wavelet In wavelets)
        '    {
        '        Transform.FastForward1d(signal, out signal2, wavelet, Level);
        '        Transform.FastInverse1d(signal2, out signal2, wavelet, Level);
        '        SumError2 = AuxFunctions.ErrorNorm(signal, signal2);
        '        Console.WriteLine(wavelet.Name + " " + SumError2);
        '    }
        '    signal = CreateSignal(SignalList.Rectangular, 0, Len);
        '    foreach(Wavelet wavelet In wavelets)
        '    {
        '        Transform.FastForward1d(signal, out signal2, wavelet, Level);
        '        Transform.FastInverse1d(signal2, out signal2, wavelet, Level);
        '        SumError3 = AuxFunctions.ErrorNorm(signal, signal2);
        '        Console.WriteLine(wavelet.Name + " " + SumError3);
        '    }
        '}

        'Void Test2D(List < Wavelet > wavelets)
        '{
        '    Bitmap OrImage = New Bitmap("LENA1.bmp");
        '    Double[,] ImageVals = New Double[OrImage.Width, OrImage.Height];
        '    Color Col;

        '    For (Int() i = 0; i < OrImage.Width; i++)
        '    {
        '        For (Int() j = 0; j < OrImage.Height; j++)
        '        {
        '            Col = OrImage.GetPixel(i, j);
        '            ImageVals[i, j] = AuxFunctions.Scale(0, 255, -1, 1, Col.R);
        '        }
        '    }
        '    Transform.FastForward2d(ImageVals, out ImageVals, wavelets[9], 4);
        '    Transform.FastInverse2d(ImageVals, out ImageVals, wavelets[9], 4);
        '    For (Int() i = 0; i < OrImage.Width; i++)
        '    {
        '        For (Int() j = 0; j < OrImage.Height; j++)
        '        {
        '            Int buff = (Int32)AuxFunctions.Scale(-1, 1, 0, 255, ImageVals[i, j]);
        '            Col = Color.FromArgb(buff, buff, buff);
        '            OrImage.SetPixel(i, j, Col);
        '        }
        '    }

        '    OrImage.Save("processed.bmp");
        '}

        'Public void TestGetSet(List<Wavelet> waves)
        '{
        '    Double[] Ssig = { 6, 3, 0, 8 };
        '    Transform.FastForward1d(Ssig, out Ssig, waves[0], 2);
        '    Double[] Details1 = Transform.GetDetailOfLevel(Ssig, 2, 1);
        '    Double[] Details2 = Transform.GetDetailOfLevel(Ssig, 2, 2);
        '    foreach(Double val In Ssig) Console.WriteLine(val);
        '    Console.WriteLine();
        '    foreach(Double val In Details1) Console.WriteLine(val);
        '    foreach(Double val In Details2) Console.WriteLine(val);
        '    Transform.SetDetailOfLevel(Ssig, New Double[] { 0.666, 0.777});
        '    Console.WriteLine("We just set up new details of level one...");
        '    Details1 = Transform.GetDetailOfLevel(Ssig, 2, 1);
        '    foreach(Double val In Details1) Console.WriteLine(val);
        '    Console.WriteLine("and here's new coefs:");
        '    foreach(Double val In Ssig) Console.WriteLine(val);
        '}
        'Public void TestGetSet2d(List<Wavelet> wavelets)
        '{
        '    Double[,] Signal = { { 0, 1, 2, 3 }, { 4, 5, 6, 7 }, { 8, 9, 10, 11 }, { 12, 13, 14, 15 } };
        '    ''Double[] details = Transform.GetAllDetail(Signal, 1);
        '    Double[] details = Transform.GetDetailOfLevel(Signal, 2);
        '    For (Int() i = 0; i < details.Length; i++)
        '    {
        '        details[i] = 666;
        '    }
        '    ''Transform.SetAllDetail(details, Signal, 1);
        '    Transform.SetDetailofLevel(details, Signal, 2);

        '    /*
        '    foreach(Double det In details)
        '    {
        '        Console.Write(det + " ");
        '    }

        '    Console.ReadKey();
        '    */
        '    foreach(Double det In Signal)
        '    {
        '        Console.WriteLine(det + " ");
        '    }

        '}
    End Class
End Namespace
