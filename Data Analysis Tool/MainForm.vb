﻿Public Class MainForm
    Private fmFFT As frmFFT
    Private fmCorr As frmCorr

    Private Enum enSelection
        FFT = 0
        CORR = 1
    End Enum

    Private Sub btnOK_Click(sender As Object, e As System.EventArgs) Handles btnOK.Click

        Select Case ListBox1.SelectedIndex
            Case enSelection.FFT
                ShowForm(fmFFT, GetType(frmFFT))
            Case enSelection.CORR
                ShowForm(fmCorr, GetType(frmCorr))
        End Select

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

        Select Case ListBox1.SelectedIndex
            Case enSelection.FFT
                ShowForm(fmFFT, GetType(frmFFT))
            Case enSelection.CORR
                ShowForm(fmCorr, GetType(frmCorr))
        End Select

    End Sub
End Class