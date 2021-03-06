# Data-Analysis-Tool
VSTO Addin for Excel in vb.net

- 1D FFT of a signal, data is read in from an Excel sheet
- 1D Pearson cross correlation of two signals, with or without zero padding, data is read in from an excel sheet
- 2D FFT of a bitmap, data is read in from a bitmap or a png image

### 2D Fourier transform sample
#### a) Grayscale
![2D FFT](https://github.com/Jens-Kluge/Data-Analysis-Tool/blob/master/screenshots/lena_fft.gif)
#### b) color
![2D FFT color](https://github.com/Jens-Kluge/Data-Analysis-Tool/blob/master/screenshots/lena_FFTcolor.GIF)

The fourier amplitude A is scaled with log(1+A) ("dyamic range compression") so that the image does not appear black. I am using the Fourier transform of mathnet.numerics libaray. 2D FFT is not implemented for the .net version, so I transform row by row and then column by column using the 1D FFT. Pixels are accessed via marshal.readbyte/writebyte, which replaces the pointer arithmetic in VB.net. 

The 1D FFT from the mathnet library is able to process arbitrary vector sizes, not only powers of two. According to my information this applies to a vector length of up to 41000. This would mean that the program can process arbitrary sized bitmaps as long as the side length does not exceed 41000.

### Cross-Correlation sample
![cross correlatioin](https://github.com/Jens-Kluge/Data-Analysis-Tool/blob/master/corr%20windspeed%2040-140%2C%20max%20lag%20100.GIF)

Cross correlation between windspeeds at 40 m and 140 m. Maximum lag is 100 samples. 144 samples correspond to 24h. As expected the maximum correlation is at zero lag and it is less than 1. Towards greater lags the correlation becomes negative, indicating a negative correlation between day/night.

![cross correlation](https://github.com/Jens-Kluge/Data-Analysis-Tool/blob/master/corr%20wind%20speed%2040-140%20max%20time%20lag%2016600.GIF)
The same picture for a maximum lag of 16600 samples. Here you see that the cross-correcation has a peak at zero lag and goes down to zero with greater time lags.
