<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImageProc
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnLoadImage = New System.Windows.Forms.Button()
        Me.pltImage = New OxyPlot.WindowsForms.PlotView()
        Me.btnFFT = New System.Windows.Forms.Button()
        Me.btnDim = New System.Windows.Forms.Button()
        Me.pbOutput1 = New System.Windows.Forms.PictureBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        CType(Me.pbOutput1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLoadImage
        '
        Me.btnLoadImage.Location = New System.Drawing.Point(61, 38)
        Me.btnLoadImage.Name = "btnLoadImage"
        Me.btnLoadImage.Size = New System.Drawing.Size(135, 31)
        Me.btnLoadImage.TabIndex = 0
        Me.btnLoadImage.Text = "Load Image"
        Me.btnLoadImage.UseVisualStyleBackColor = True
        '
        'pltImage
        '
        Me.pltImage.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.pltImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pltImage.Location = New System.Drawing.Point(0, 0)
        Me.pltImage.Name = "pltImage"
        Me.pltImage.PanCursor = System.Windows.Forms.Cursors.Hand
        Me.pltImage.Size = New System.Drawing.Size(447, 360)
        Me.pltImage.TabIndex = 1
        Me.pltImage.Text = "Image Plot"
        Me.pltImage.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE
        Me.pltImage.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.pltImage.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS
        '
        'btnFFT
        '
        Me.btnFFT.Location = New System.Drawing.Point(343, 38)
        Me.btnFFT.Name = "btnFFT"
        Me.btnFFT.Size = New System.Drawing.Size(135, 31)
        Me.btnFFT.TabIndex = 2
        Me.btnFFT.Text = "2D FFT"
        Me.btnFFT.UseVisualStyleBackColor = True
        '
        'btnDim
        '
        Me.btnDim.Location = New System.Drawing.Point(202, 38)
        Me.btnDim.Name = "btnDim"
        Me.btnDim.Size = New System.Drawing.Size(135, 30)
        Me.btnDim.TabIndex = 3
        Me.btnDim.Text = "Original Size"
        Me.btnDim.UseVisualStyleBackColor = True
        '
        'pbOutput1
        '
        Me.pbOutput1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbOutput1.Location = New System.Drawing.Point(0, 0)
        Me.pbOutput1.Name = "pbOutput1"
        Me.pbOutput1.Size = New System.Drawing.Size(468, 360)
        Me.pbOutput1.TabIndex = 5
        Me.pbOutput1.TabStop = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(61, 106)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.Controls.Add(Me.pltImage)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.pbOutput1)
        Me.SplitContainer1.Size = New System.Drawing.Size(919, 360)
        Me.SplitContainer1.SplitterDistance = 447
        Me.SplitContainer1.TabIndex = 7
        '
        'frmImageProc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1056, 503)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnDim)
        Me.Controls.Add(Me.btnFFT)
        Me.Controls.Add(Me.btnLoadImage)
        Me.Name = "frmImageProc"
        Me.Text = "Image data analysis"
        CType(Me.pbOutput1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnLoadImage As Windows.Forms.Button
    Friend WithEvents pltImage As OxyPlot.WindowsForms.PlotView
    Friend WithEvents btnFFT As Windows.Forms.Button
    Friend WithEvents btnDim As Windows.Forms.Button
    Friend WithEvents pbOutput1 As Windows.Forms.PictureBox
    Friend WithEvents SplitContainer1 As Windows.Forms.SplitContainer
End Class
