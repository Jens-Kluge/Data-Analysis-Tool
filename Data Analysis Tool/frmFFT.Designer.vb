<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFFT
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFFT))
        Me.btnFFT = New System.Windows.Forms.Button()
        Me.btnExtend = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PlotView1 = New OxyPlot.WindowsForms.PlotView()
        Me.PlotView2 = New OxyPlot.WindowsForms.PlotView()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ExcelRefedit1 = New Data_Analysis_Tool.ExcelRefedit()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.refeditOutput = New Data_Analysis_Tool.ExcelRefedit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnFFT
        '
        Me.btnFFT.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnFFT.Location = New System.Drawing.Point(49, 454)
        Me.btnFFT.Name = "btnFFT"
        Me.btnFFT.Size = New System.Drawing.Size(222, 36)
        Me.btnFFT.TabIndex = 2
        Me.btnFFT.Text = "compute FFT"
        Me.btnFFT.UseVisualStyleBackColor = True
        '
        'btnExtend
        '
        Me.btnExtend.Image = Global.Data_Analysis_Tool.My.Resources.Resources.Arrows_Down_icon
        Me.btnExtend.Location = New System.Drawing.Point(243, 71)
        Me.btnExtend.Name = "btnExtend"
        Me.btnExtend.Size = New System.Drawing.Size(25, 23)
        Me.btnExtend.TabIndex = 4
        Me.btnExtend.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(52, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 17)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Input range"
        '
        'PlotView1
        '
        Me.PlotView1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.PlotView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PlotView1.Location = New System.Drawing.Point(0, 0)
        Me.PlotView1.Name = "PlotView1"
        Me.PlotView1.PanCursor = System.Windows.Forms.Cursors.Hand
        Me.PlotView1.Size = New System.Drawing.Size(652, 230)
        Me.PlotView1.TabIndex = 3
        Me.PlotView1.Text = "PlotView1"
        Me.PlotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE
        Me.PlotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.PlotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS
        '
        'PlotView2
        '
        Me.PlotView2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.PlotView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PlotView2.Location = New System.Drawing.Point(0, 0)
        Me.PlotView2.Name = "PlotView2"
        Me.PlotView2.PanCursor = System.Windows.Forms.Cursors.Hand
        Me.PlotView2.Size = New System.Drawing.Size(652, 212)
        Me.PlotView2.TabIndex = 6
        Me.PlotView2.Text = "PlotView2"
        Me.PlotView2.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE
        Me.PlotView2.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.PlotView2.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(294, 44)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.PlotView1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.PlotView2)
        Me.SplitContainer1.Size = New System.Drawing.Size(652, 446)
        Me.SplitContainer1.SplitterDistance = 230
        Me.SplitContainer1.TabIndex = 7
        '
        'ExcelRefedit1
        '
        Me.ExcelRefedit1.Address = Nothing
        Me.ExcelRefedit1.ExcelConnector = Nothing
        Me.ExcelRefedit1.ImageMaximized = CType(resources.GetObject("ExcelRefedit1.ImageMaximized"), System.Drawing.Image)
        Me.ExcelRefedit1.ImageMinimized = CType(resources.GetObject("ExcelRefedit1.ImageMinimized"), System.Drawing.Image)
        Me.ExcelRefedit1.Location = New System.Drawing.Point(49, 67)
        Me.ExcelRefedit1.Margin = New System.Windows.Forms.Padding(4)
        Me.ExcelRefedit1.Name = "ExcelRefedit1"
        Me.ExcelRefedit1.RefEditFont = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExcelRefedit1.Size = New System.Drawing.Size(192, 32)
        Me.ExcelRefedit1.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(58, 179)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(92, 17)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Output range"
        '
        'refeditOutput
        '
        Me.refeditOutput.Address = Nothing
        Me.refeditOutput.ExcelConnector = Nothing
        Me.refeditOutput.ImageMaximized = CType(resources.GetObject("refeditOutput.ImageMaximized"), System.Drawing.Image)
        Me.refeditOutput.ImageMinimized = CType(resources.GetObject("refeditOutput.ImageMinimized"), System.Drawing.Image)
        Me.refeditOutput.Location = New System.Drawing.Point(55, 200)
        Me.refeditOutput.Margin = New System.Windows.Forms.Padding(4)
        Me.refeditOutput.Name = "refeditOutput"
        Me.refeditOutput.RefEditFont = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.refeditOutput.Size = New System.Drawing.Size(192, 32)
        Me.refeditOutput.TabIndex = 8
        '
        'frmFFT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(983, 519)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.refeditOutput)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnExtend)
        Me.Controls.Add(Me.btnFFT)
        Me.Controls.Add(Me.ExcelRefedit1)
        Me.Name = "frmFFT"
        Me.Text = "frmFFT"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ExcelRefedit1 As ExcelRefedit
    Friend WithEvents btnFFT As Windows.Forms.Button
    Friend WithEvents btnExtend As Windows.Forms.Button
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents PlotView1 As OxyPlot.WindowsForms.PlotView
    Friend WithEvents PlotView2 As OxyPlot.WindowsForms.PlotView
    Friend WithEvents SplitContainer1 As Windows.Forms.SplitContainer
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents refeditOutput As ExcelRefedit
End Class
