
Imports System
Imports System.Diagnostics

Namespace signal

	Public Class TimeSignalGenerator

		Private _SignalType As enSignalType = enSignalType.Sine
		''' <summary>
		''' Signal Type.
		''' </summary>
		Public Property SignalType As enSignalType
			Get
				Return _SignalType
			End Get
			Set(ByVal value As enSignalType)
				_SignalType = value
			End Set
		End Property

		Private _frequency As Double = 1.0F
		''' <summary>
		''' Signal Frequency.
		''' </summary>
		Public Property Frequency As Double
			Get
				Return _frequency
			End Get
			Set(ByVal value As Double)
				_frequency = value
			End Set
		End Property

		Private _phase As Double = 0F
		''' <summary>
		''' Signal Phase.
		''' </summary>
		Public Property Phase As Double
			Get
				Return _phase
			End Get
			Set(ByVal value As Double)
				_phase = value
			End Set
		End Property

		Private _amplitude As Double = 1.0F
		''' <summary>
		''' Signal Amplitude.
		''' </summary>
		Public Property Amplitude As Double

			Get
				Return _amplitude
			End Get
			Set(ByVal value As Double)
				_amplitude = value
			End Set

		End Property

		Private _offset As Double = 0F
		''' <summary>
		''' Signal Offset.
		''' </summary>
		Public Property Offset As Double
			Get
				Return _offset
			End Get
			Set(ByVal value As Double)
				_offset = value
			End Set
		End Property

		Private _invert As Double = 1 'Yes=-1, No=1
		''' <summary>
		''' Signal Inverted?
		''' </summary>
		Public Property Invert As Boolean
			Get
				Return IIf(_invert = -1, True, False)
			End Get
			Set(ByVal value As Boolean)
				_invert = IIf(value, -1, 1)
			End Set
		End Property

		Private _getValueCallback As GetValueDelegate = Nothing
		''' <summary>
		''' GetValue Callback?
		''' </summary>
		Public Property GetValueCallback As GetValueDelegate
			Get
				Return _getValueCallback
			End Get
			Set(ByVal value As GetValueDelegate)
				_getValueCallback = value
			End Set
		End Property

		''' <summary>
		''' Random provider for noise generator
		''' </summary>
		Private random As Random = New Random()

		''' <summary>
		''' Time the signal generator was started
		''' </summary>
		Protected startTime As Long = Stopwatch.GetTimestamp()

		''' <summary>
		''' Ticks per second on this CPU
		''' </summary>
		Protected ticksPerSecond As Long = Stopwatch.Frequency

		Public Delegate Function GetValueDelegate(time As Double) As Double

		Public Sub New(initialSignalType As enSignalType)
			_SignalType = initialSignalType
		End Sub

		Public Sub New()
			MyBase.New()
		End Sub

		Private Function GetValue(time As Double) As Double

			Dim value As Double = 0F

			Dim t As Double = Frequency * time + Phase
			Select Case _SignalType
				Case enSignalType.Sine      ' sin( 2 * pi * t )
					value = Math.Sin(2.0F * Math.PI * t)
				Case enSignalType.Square       ' sign( sin( 2 * pi * t ) )
					value = Math.Sign(Math.Sin(2.0F * Math.PI * t))
				Case enSignalType.Triangle        ' 2 * abs( t - 2 * floor( t / 2 ) - 1 ) - 1
					value = 1.0F - 4.0F * Math.Abs(Math.Round(t - 0.25F) - (t - 0.25F))
				Case enSignalType.Sawtooth      ' 2 * ( t/a - floor( t/a + 1/2 ) )
					value = 2.0F * (t - Math.Floor(t + 0.5F))
				Case enSignalType.Pulse
					value = IIf(Math.Abs(Math.Sin(2 * Math.PI * t)) < 1.0 - 0.01, 0, 1)
				Case enSignalType.WhiteNoise    ' http:'en.wikipedia.org/wiki/White_noise
					value = 2.0F * random.Next(Integer.MaxValue) / Integer.MaxValue - 1.0F
				Case enSignalType.GaussNoise    ' http:'en.wikipedia.org/wiki/Gaussian_noise
					value = StatisticFunction.NORMINV(random.Next(Integer.MaxValue) / Integer.MaxValue, 0.0, 0.4)
				Case enSignalType.DigitalNoise    'Binary Bit Generators
					value = random.Next(2)
				Case enSignalType.UserDefined
					value = IIf(_getValueCallback Is Nothing, 0F, GetValueCallback)
			End Select

			Return (Invert * Amplitude * value + Offset)
		End Function


		Public Function GetValue() As Double
			Dim time As Double = (Stopwatch.GetTimestamp() - startTime) / ticksPerSecond
			Return GetValue(time)
		End Function

		Public Sub Reset()
			startTime = Stopwatch.GetTimestamp()
		End Sub

		Public Sub Synchronize(instance As TimeSignalGenerator)
			startTime = instance.startTime
			ticksPerSecond = instance.ticksPerSecond
		End Sub

	End Class

	Public Enum enSignalType
		Sine
		Square
		Triangle
		Sawtooth
		Pulse
		WhiteNoise    ' random between -1 And 1
		GaussNoise ' random between -1 And 1 With normal distribution
		DigitalNoise
		UserDefined    ' user defined between -1 And 1
	End Enum

	'provides helper functions for the time signal generator
	Public Class StatisticFunction

		Public Shared Function Mean(values() As Double) As Double
			Dim tot As Double = 0
			For Each val As Double In values
				tot += val
			Next
			Return (tot / values.Length)
		End Function

		Public Shared Function StandardDeviation(values() As Double) As Double
			Return Math.Sqrt(Variance(values))
		End Function

		Public Shared Function Variance(values() As Double) As Double
			Dim m As Double = Mean(values)
			Dim result As Double = 0
			For Each d As Double In values
				result += Math.Pow((d - m), 2)
			Next
			Return (result / values.Length)
		End Function

		'
		' Lower tail quantile for standard normal distribution function.
		'
		' This function returns an approximation of the inverse cumulative
		' standard normal distribution function.  I.e., given P, it returns
		' an approximation to the X satisfying P = Pr{Z <= X} where Z Is a
		' random variable from the standard normal distribution.
		'
		' The algorithm uses a minimax approximation by rational functions
		' And the result has a relative error whose absolute value Is less
		' than 1.15e-9.
		'
		' Author:      Peter J. Acklam
		' (Javascript version by Alankar Misra @ Digital Sutras (alankar@digitalsutras.com))
		' Time-stamp:  2003-05-05 05:15:14
		' E-mail:      pjacklam@online.no
		' WWW URL:     http:'home.online.no/~pjacklam

		' An algorithm with a relative error less than 1.15*10-9 in the entire region.

		Public Shared Function NORMSINV(p As Double) As Double

			' Coefficients in rational approximations
			Dim a() As Double = {-39.696830286653757, 220.9460984245205,
				-275.92851044696869, 138.357751867269,
				-30.66479806614716, 2.5066282774592392}

			Dim b() As Double = {-54.476098798224058, 161.58583685804089,
					-155.69897985988661, 66.80131188771972,
					-13.280681552885721}

			Dim c() As Double = {-0.0077848940024302926, -0.32239645804113648,
					-2.4007582771618381, -2.5497325393437338,
					4.3746641414649678, 2.9381639826987831}

			Dim d() As Double = {0.0077846957090414622, 0.32246712907003983,
					2.445134137142996, 3.7544086619074162}

			' Define break-points.
			Dim plow As Double = 0.02425
			Dim phigh As Double = 1 - plow

			Dim q As Double
			' Rational approximation for lower region
			If p < plow Then

				q = Math.Sqrt(-2 * Math.Log(p))
				Return (((((c(0) * q + c(1)) * q + c(2)) * q + c(3)) * q + c(4)) * q + c(5)) /
						((((d(0) * q + d(1)) * q + d(2)) * q + d(3)) * q + 1)
			End If

			' Rational approximation for upper region
			If phigh < p Then
				q = Math.Sqrt(-2 * Math.Log(1 - p))
				Return -(((((c(0) * q + c(1)) * q + c(2)) * q + c(3)) * q + c(4)) * q + c(5)) /
						((((d(0) * q + d(1)) * q + d(2)) * q + d(3)) * q + 1)
			End If

			' Rational approximation for central region

			q = p - 0.5
			Dim r As Double = q * q
			Return (((((a(0) * r + a(1)) * r + a(2)) * r + a(3)) * r + a(4)) * r + a(5)) * q /
					(((((b(0) * r + b(1)) * r + b(2)) * r + b(3)) * r + b(4)) * r + 1)

		End Function

		Public Shared Function NORMINV(probability As Double, mean As Double, standard_deviation As Double) As Double
			Return (NORMSINV(probability) * standard_deviation + mean)
		End Function

		Public Shared Function NORMINV(probability As Double, values() As Double) As Double
			Return NORMINV(probability, Mean(values), StandardDeviation(values))
		End Function

	End Class

End Namespace

