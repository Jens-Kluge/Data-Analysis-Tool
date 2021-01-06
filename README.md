# Data-Analysis-Tool
VSTO Addin for Excel in vb.net

- 1D FFT of a signal, data is read in from an Excel sheet
- 1D Pearson cross correlation of two signals, with or without zero padding, data is read in from an excel sheet
- 2D FFT of a bitmap, data is read in from a bitmap or a png image

## 2D Fourier transform sample
![2D FFT](https://github.com/Jens-Kluge/Data-Analysis-Tool/blob/master/2DFFT%20Capture.GIF)

Note that the fourier amplitude A is scaled with log(1+A) so that the image does not appear black. Zero frequency is at the corners. I am using the Fourier transform of mathnet.numerics libaray. 2D FFT is not implemented for the .net version, so I transform row by row and then column by column using the 1D FFT. Pixels are accessed via marshal.readbyte/writebyte, which replaces the pointer arithmetic in VB.net. The program is able to process arbitrary bitmap sizes, not only powers of two.
