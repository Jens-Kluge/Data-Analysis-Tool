<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCorr
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCorr))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PlotView1 = New OxyPlot.WindowsForms.PlotView()
        Me.btnCorr = New System.Windows.Forms.Button()
        Me.updLag = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkPadding = New System.Windows.Forms.CheckBox()
        Me.btnExtend2 = New System.Windows.Forms.Button()
        Me.btnExtend = New System.Windows.Forms.Button()
        Me.ExcelRefedit3 = New Data_Analysis_Tool.ExcelRefedit()
        Me.ExcelRefedit2 = New Data_Analysis_Tool.ExcelRefedit()
        Me.ExcelRefedit1 = New Data_Analysis_Tool.ExcelRefedit()
        CType(Me.updLag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(40, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "First dataset"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(40, 115)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 17)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Second dataset"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(48, 184)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 17)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Output range"
        '
        'PlotView1
        '
        Me.PlotView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PlotView1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.PlotView1.Location = New System.Drawing.Point(320, 53)
        Me.PlotView1.Name = "PlotView1"
        Me.PlotView1.PanCursor = System.Windows.Forms.Cursors.Hand
        Me.PlotView1.Size = New System.Drawing.Size(551, 407)
        Me.PlotView1.TabIndex = 8
        Me.PlotView1.Text = "PlotView1"
        Me.PlotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE
        Me.PlotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.PlotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS
        '
        'btnCorr
        '
        Me.btnCorr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCorr.Location = New System.Drawing.Point(51, 421)
        Me.btnCorr.Name = "btnCorr"
        Me.btnCorr.Size = New System.Drawing.Size(187, 39)
        Me.btnCorr.TabIndex = 9
        Me.btnCorr.Text = "Compute Correlation"
        Me.btnCorr.UseVisualStyleBackColor = True
        '
        'updLag
        '
        Me.updLag.Location = New System.Drawing.Point(54, 284)
        Me.updLag.Name = "updLag"
        Me.updLag.Size = New System.Drawing.Size(86, 22)
        Me.updLag.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(51, 264)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(175, 17)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Maximum time lag/samples"
        '
        'chkPadding
        '
        Me.chkPadding.AutoSize = True
        Me.chkPadding.Location = New System.Drawing.Point(54, 332)
        Me.chkPadding.Name = "chkPadding"
        Me.chkPadding.Size = New System.Drawing.Size(115, 21)
        Me.chkPadding.TabIndex = 12
        Me.chkPadding.Text = "Zero padding"
        Me.chkPadding.UseVisualStyleBackColor = True
        '
        'btnExtend2
        '
        Me.btnExtend2.Image = Global.Data_Analysis_Tool.My.Resources.Resources.fill_270_icon
        Me.btnExtend2.Location = New System.Drawing.Point(238, 139)
        Me.btnExtend2.Name = "btnExtend2"
        Me.btnExtend2.Size = New System.Drawing.Size(25, 23)
        Me.btnExtend2.TabIndex = 7
        Me.btnExtend2.UseVisualStyleBackColor = True
        '
        'btnExtend
        '
        Me.btnExtend.Image = Global.Data_Analysis_Tool.My.Resources.Resources.fill_270_icon
        Me.btnExtend.Location = New System.Drawing.Point(237, 78)
        Me.btnExtend.Name = "btnExtend"
        Me.btnExtend.Size = New System.Drawing.Size(25, 23)
        Me.btnExtend.TabIndex = 6
        Me.btnExtend.UseVisualStyleBackColor = True
        '
        'ExcelRefedit3
        '
        Me.ExcelRefedit3.Address = Nothing
        Me.ExcelRefedit3.ExcelConnector = Nothing
        Me.ExcelRefedit3.ImageMaximized = CType(resources.GetObject("ExcelRefedit3.ImageMaximized"), System.Drawing.Image)
        Me.ExcelRefedit3.ImageMinimized = CType(resources.GetObject("ExcelRefedit3.ImageMinimized"), System.Drawing.Image)
        Me.ExcelRefedit3.Location = New System.Drawing.Point(46, 213)
        Me.ExcelRefedit3.Margin = New System.Windows.Forms.Padding(4)
        Me.ExcelRefedit3.Name = "ExcelRefedit3"
        Me.ExcelRefedit3.RefEditFont = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExcelRefedit3.Size = New System.Drawing.Size(192, 32)
        Me.ExcelRefedit3.TabIndex = 5
        '
        'ExcelRefedit2
        '
        Me.ExcelRefedit2.Address = Nothing
        Me.ExcelRefedit2.ExcelConnector = Nothing
        Me.ExcelRefedit2.ImageMaximized = CType(resources.GetObject("ExcelRefedit2.ImageMaximized"), System.Drawing.Image)
        Me.ExcelRefedit2.ImageMinimized = CType(resources.GetObject("ExcelRefedit2.ImageMinimized"), System.Drawing.Image)
        Me.ExcelRefedit2.Location = New System.Drawing.Point(43, 136)
        Me.ExcelRefedit2.Margin = New System.Windows.Forms.Padding(4)
        Me.ExcelRefedit2.Name = "ExcelRefedit2"
        Me.ExcelRefedit2.RefEditFont = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExcelRefedit2.Size = New System.Drawing.Size(192, 32)
        Me.ExcelRefedit2.TabIndex = 2
        '
        'ExcelRefedit1
        '
        Me.ExcelRefedit1.Address = Nothing
        Me.ExcelRefedit1.ExcelConnector = Nothing
        Me.ExcelRefedit1.ImageMaximized = CType(resources.GetObject("ExcelRefedit1.ImageMaximized"), System.Drawing.Image)
        Me.ExcelRefedit1.ImageMinimized = CType(resources.GetObject("ExcelRefedit1.ImageMinimized"), System.Drawing.Image)
        Me.ExcelRefedit1.Location = New System.Drawing.Point(43, 74)
        Me.ExcelRefedit1.Margin = New System.Windows.Forms.Padding(4)
        Me.ExcelRefedit1.Name = "ExcelRefedit1"
        Me.ExcelRefedit1.RefEditFont = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExcelRefedit1.Size = New System.Drawing.Size(192, 32)
        Me.ExcelRefedit1.TabIndex = 0
        '
        'frmCorr
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(934, 516)
        Me.Controls.Add(Me.chkPadding)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.updLag)
        Me.Controls.Add(Me.btnCorr)
        Me.Controls.Add(Me.PlotView1)
        Me.Controls.Add(Me.btnExtend2)
        Me.Controls.Add(Me.btnExtend)
        Me.Controls.Add(Me.ExcelRefedit3)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ExcelRefedit2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ExcelRefedit1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmCorr"
        Me.Text = "frmCorr"
        CType(Me.updLag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ExcelRefedit1 As ExcelRefedit
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents ExcelRefedit2 As ExcelRefedit
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents ExcelRefedit3 As ExcelRefedit
    Friend WithEvents btnExtend As Windows.Forms.Button
    Friend WithEvents btnExtend2 As Windows.Forms.Button
    Friend WithEvents PlotView1 As OxyPlot.WindowsForms.PlotView
    Friend WithEvents btnCorr As Windows.Forms.Button
    Friend WithEvents updLag As Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents chkPadding As Windows.Forms.CheckBox
End Class
