Imports Microsoft.Office.Tools.Ribbon

Public Class Ribbon1
    Private fmMain As MainForm

    Private Sub btnMainFrm_Click(sender As Object, e As RibbonControlEventArgs) Handles btnMainFrm.Click
        ShowForm(fmMain, GetType(MainForm))
        BringFormsToFront()
    End Sub

End Class
