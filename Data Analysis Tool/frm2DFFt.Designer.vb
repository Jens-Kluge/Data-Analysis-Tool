<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm2DFFt
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
        Me.pbOutput1 = New System.Windows.Forms.PictureBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.pbFFTPhase = New System.Windows.Forms.PictureBox()
        Me.pbInvFFT = New System.Windows.Forms.PictureBox()
        Me.splithorz = New System.Windows.Forms.SplitContainer()
        Me.btnInvFFT = New System.Windows.Forms.Button()
        Me.chkRGB = New System.Windows.Forms.CheckBox()
        CType(Me.pbOutput1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.pbFFTPhase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbInvFFT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.splithorz, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splithorz.Panel1.SuspendLayout()
        Me.splithorz.Panel2.SuspendLayout()
        Me.splithorz.SuspendLayout()
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
        Me.pltImage.Size = New System.Drawing.Size(435, 246)
        Me.pltImage.TabIndex = 1
        Me.pltImage.Text = "Image Plot"
        Me.pltImage.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE
        Me.pltImage.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.pltImage.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS
        '
        'btnFFT
        '
        Me.btnFFT.Location = New System.Drawing.Point(226, 38)
        Me.btnFFT.Name = "btnFFT"
        Me.btnFFT.Size = New System.Drawing.Size(135, 31)
        Me.btnFFT.TabIndex = 2
        Me.btnFFT.Text = "2D FFT"
        Me.btnFFT.UseVisualStyleBackColor = True
        '
        'pbOutput1
        '
        Me.pbOutput1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.pbOutput1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbOutput1.Location = New System.Drawing.Point(0, 0)
        Me.pbOutput1.Name = "pbOutput1"
        Me.pbOutput1.Size = New System.Drawing.Size(457, 246)
        Me.pbOutput1.TabIndex = 5
        Me.pbOutput1.TabStop = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
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
        Me.SplitContainer1.Size = New System.Drawing.Size(896, 246)
        Me.SplitContainer1.SplitterDistance = 435
        Me.SplitContainer1.TabIndex = 7
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.AutoScroll = True
        Me.SplitContainer2.Panel1.Controls.Add(Me.pbFFTPhase)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.AutoScroll = True
        Me.SplitContainer2.Panel2.Controls.Add(Me.pbInvFFT)
        Me.SplitContainer2.Size = New System.Drawing.Size(896, 243)
        Me.SplitContainer2.SplitterDistance = 436
        Me.SplitContainer2.TabIndex = 8
        '
        'pbFFTPhase
        '
        Me.pbFFTPhase.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.pbFFTPhase.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbFFTPhase.Location = New System.Drawing.Point(0, 0)
        Me.pbFFTPhase.Name = "pbFFTPhase"
        Me.pbFFTPhase.Size = New System.Drawing.Size(436, 243)
        Me.pbFFTPhase.TabIndex = 0
        Me.pbFFTPhase.TabStop = False
        '
        'pbInvFFT
        '
        Me.pbInvFFT.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.pbInvFFT.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbInvFFT.Location = New System.Drawing.Point(0, 0)
        Me.pbInvFFT.Name = "pbInvFFT"
        Me.pbInvFFT.Size = New System.Drawing.Size(456, 243)
        Me.pbInvFFT.TabIndex = 0
        Me.pbInvFFT.TabStop = False
        '
        'splithorz
        '
        Me.splithorz.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.splithorz.Location = New System.Drawing.Point(61, 91)
        Me.splithorz.Name = "splithorz"
        Me.splithorz.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splithorz.Panel1
        '
        Me.splithorz.Panel1.Controls.Add(Me.SplitContainer1)
        '
        'splithorz.Panel2
        '
        Me.splithorz.Panel2.Controls.Add(Me.SplitContainer2)
        Me.splithorz.Size = New System.Drawing.Size(896, 493)
        Me.splithorz.SplitterDistance = 246
        Me.splithorz.TabIndex = 10
        '
        'btnInvFFT
        '
        Me.btnInvFFT.Location = New System.Drawing.Point(391, 38)
        Me.btnInvFFT.Name = "btnInvFFT"
        Me.btnInvFFT.Size = New System.Drawing.Size(135, 31)
        Me.btnInvFFT.TabIndex = 11
        Me.btnInvFFT.Text = "Inverse FFT"
        Me.btnInvFFT.UseVisualStyleBackColor = True
        '
        'chkRGB
        '
        Me.chkRGB.AutoSize = True
        Me.chkRGB.Location = New System.Drawing.Point(584, 38)
        Me.chkRGB.Name = "chkRGB"
        Me.chkRGB.Size = New System.Drawing.Size(121, 21)
        Me.chkRGB.TabIndex = 12
        Me.chkRGB.Text = "RGB channels"
        Me.chkRGB.UseVisualStyleBackColor = True
        '
        'frm2DFFt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1056, 638)
        Me.Controls.Add(Me.chkRGB)
        Me.Controls.Add(Me.btnInvFFT)
        Me.Controls.Add(Me.splithorz)
        Me.Controls.Add(Me.btnFFT)
        Me.Controls.Add(Me.btnLoadImage)
        Me.Name = "frm2DFFt"
        Me.Text = "Image data analysis"
        CType(Me.pbOutput1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.pbFFTPhase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbInvFFT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splithorz.Panel1.ResumeLayout(False)
        Me.splithorz.Panel2.ResumeLayout(False)
        CType(Me.splithorz, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splithorz.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnLoadImage As Windows.Forms.Button
    Friend WithEvents pltImage As OxyPlot.WindowsForms.PlotView
    Friend WithEvents btnFFT As Windows.Forms.Button
    Friend WithEvents pbOutput1 As Windows.Forms.PictureBox
    Friend WithEvents SplitContainer1 As Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As Windows.Forms.SplitContainer
    Friend WithEvents splithorz As Windows.Forms.SplitContainer
    Friend WithEvents pbFFTPhase As Windows.Forms.PictureBox
    Friend WithEvents btnInvFFT As Windows.Forms.Button
    Friend WithEvents pbInvFFT As Windows.Forms.PictureBox
    Friend WithEvents chkRGB As Windows.Forms.CheckBox
End Class
