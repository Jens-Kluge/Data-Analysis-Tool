<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExcelRefedit
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.btnState = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtAddress
        '
        Me.txtAddress.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAddress.Location = New System.Drawing.Point(2, 4)
        Me.txtAddress.Margin = New System.Windows.Forms.Padding(4)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(162, 22)
        Me.txtAddress.TabIndex = 0
        '
        'btnState
        '
        Me.btnState.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnState.Image = Global.Data_Analysis_Tool.My.Resources.Resources.imgMaximized
        Me.btnState.Location = New System.Drawing.Point(168, 4)
        Me.btnState.Margin = New System.Windows.Forms.Padding(4)
        Me.btnState.Name = "btnState"
        Me.btnState.Size = New System.Drawing.Size(24, 23)
        Me.btnState.TabIndex = 1
        Me.btnState.UseVisualStyleBackColor = True
        '
        'ExcelRefedit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnState)
        Me.Controls.Add(Me.txtAddress)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "ExcelRefedit"
        Me.Size = New System.Drawing.Size(192, 32)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtAddress As Windows.Forms.TextBox
    Friend WithEvents btnState As Windows.Forms.Button
End Class
