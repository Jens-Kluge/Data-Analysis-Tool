
Imports Microsoft.Office.Interop
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Drawing

''' <summary>
''' This structure is used by the Resize method to store Parent and Control dimensions.
''' </summary>
''' <remarks></remarks>
Public Structure RefEditState
        Public ParentClientSize As Size
        Public IsParentMinimized As Boolean
        Public ParentPrevBorder As FormBorderStyle
        Public ShowParentControlBox As Boolean
        Public ControlPrevX As Integer
        Public ControlPrevY As Integer
        Public ControlAnchor As AnchorStyles
        Public ActualParent As Control
    End Structure

<DefaultEvent("Changed")>
Public Class ExcelRefedit

    Private WithEvents xlBook As Excel.Workbook
    Private WithEvents xlSheet As Excel.Worksheet
    Private DisplayState As RefEditState

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.btnState.Image = _ImageMaximized

    End Sub

#Region "Base Properties that are Browsable = False"
    <Browsable(False)>
    Public Overrides Property Font As System.Drawing.Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            MyBase.Font = value
        End Set
    End Property

    <Browsable(False)>
    Public Overrides Property ForeColor As System.Drawing.Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            MyBase.ForeColor = value
        End Set
    End Property

    <Browsable(False)>
    Public Overrides Property RightToLeft As System.Windows.Forms.RightToLeft
        Get
            Return MyBase.RightToLeft
        End Get
        Set(ByVal value As System.Windows.Forms.RightToLeft)
            MyBase.RightToLeft = value
        End Set
    End Property
#End Region

#Region "Properties"

    Private _ExcelConnector As Excel.Application
    Private _ImageMinimized As Image = My.Resources.imgMinimized
    Private _ImageMaximized As Image = My.Resources.imgMaximized
    Private _IncludeSheetName As Boolean = True
    Private _ShowRowAbsoluteIndicator As Boolean = True
    Private _ShowColumnAbsoluteIndicator As Boolean = True
    Private _address As String

    ''' <summary>
    ''' This connections the control to an instance of Excel.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>It is not visible in the controls PropertyGrid.</remarks>
    <Browsable(False), Category("RefEdit"), Description("Connects the control to an instance of Excel.")>
    Public Property ExcelConnector As Excel.Application
        Get
            Return _ExcelConnector
        End Get
        Set(ByVal value As Excel.Application)
            _ExcelConnector = value

            If _ExcelConnector Is Nothing Then Return

            If _ExcelConnector.Workbooks.Count > 0 Then
                xlBook = _ExcelConnector.ActiveWorkbook
                If xlBook.Sheets.Count > 0 Then
                    xlSheet = xlBook.ActiveSheet
                    AddHandler xlSheet.SelectionChange, AddressOf SelectionChange
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Indicates whether the component should draw right-to-left for RTL languages.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit Appearance"), Description("Indicates whether the component should draw right-to-left for RTL languages."), DisplayName("RightToLeaf") _
    , DefaultValue(GetType(RightToLeft), "No")>
    Public Property RefEditRightToLeft As System.Windows.Forms.RightToLeft
        Get
            Return Me.txtAddress.RightToLeft
        End Get
        Set(ByVal value As System.Windows.Forms.RightToLeft)
            Me.txtAddress.RightToLeft = value
        End Set
    End Property

    ''' <summary>
    ''' The font used to display text in the RefEdit control.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit Appearance"), Description("The font used to display text in the RefEdit control."), DisplayName("Font")>
    Public Property RefEditFont As System.Drawing.Font
        Get
            Return Me.txtAddress.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            Me.txtAddress.Font = value
        End Set
    End Property

    ''' <summary>
    ''' The foreground color of this RefEdit control, which is used to display text.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit Appearance"), Description("The foreground color of this RefEdit control, which is used to display text."), DisplayName("Forecolor") _
    , DefaultValue(GetType(Color), "WindowText")>
    Public Property RefEditForecolor As System.Drawing.Color
        Get
            Return Me.txtAddress.ForeColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            Me.txtAddress.ForeColor = value
        End Set
    End Property

    ''' <summary>
    ''' The RefEdit Controls displayed range.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit Appearance"), Description("The selected range's address.")>
    Public Property Address As String
        Get
            Return _address
        End Get

        Set(value As String)
            _address = value
            txtAddress.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates if the worksheet name should be included in the selected range.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit Appearance"), Description("Indicates if the worksheet name should be included in the selected range."), DefaultValue(True)>
    Public Property IncludeSheetName As Boolean
        Get
            Return _IncludeSheetName
        End Get
        Set(ByVal value As Boolean)
            _IncludeSheetName = value
            Me.Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Shows the row absolute indicator ($) in the selected range.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit Appearance"), Description("Shows the row absolute indicator ($) in the selected range."), DefaultValue(True)>
    Public Property ShowRowAbsoluteIndicator As Boolean
        Get
            Return _ShowRowAbsoluteIndicator
        End Get
        Set(ByVal value As Boolean)
            _ShowRowAbsoluteIndicator = value
            Me.Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Shows the column absolute indicator ($) in the selected range.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit Appearance"), Description("Shows the column absolute indicator ($) in the selected range."), DefaultValue(True)>
    Public Property ShowColumnAbsoluteIndicator As Boolean
        Get
            Return _ShowColumnAbsoluteIndicator
        End Get
        Set(ByVal value As Boolean)
            _ShowColumnAbsoluteIndicator = value
            Me.Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' For buttons whose FlatStyle is FlatStyle.Flat, determines the appearance of the border and the colors used to indicate check state and mouse state.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit DropButton"), Description("For buttons whose FlatStyle is FlatStyle.Flat, determines the appearance of the border and the colors used to indicate check state and mouse state.") _
    , DisplayName("FlatAppearance")>
    Public Property FlatAppearance As FlatButtonAppearance
        Get
            Return Me.btnState.FlatAppearance
        End Get
        Set(ByVal value As FlatButtonAppearance)
            With Me.btnState.FlatAppearance
                .BorderColor = value.BorderColor
                .BorderSize = value.BorderSize
                .CheckedBackColor = value.CheckedBackColor
                .MouseDownBackColor = value.MouseDownBackColor
                .MouseOverBackColor = value.MouseOverBackColor
            End With

        End Set
    End Property

    ''' <summary>
    ''' Determines the appearance of the control when a user moves the mouse over the control and clicks.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit DropButton"), Description("Determines the appearance of the control when a user moves the mouse over the control and clicks."), DisplayName("FlatStyle") _
    , DefaultValue(GetType(FlatStyle), "Standard")>
    Public Property DropButtonFlatStyle As FlatStyle
        Get
            Return Me.btnState.FlatStyle
        End Get
        Set(ByVal value As FlatStyle)
            Me.btnState.FlatStyle = value
        End Set
    End Property

    ''' <summary>
    ''' The image displayed when the control has been minimized.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit DropButton"), Description("The image displayed when the control has been minimized.")>
    Public Property ImageMinimized As Image
        Get
            Return _ImageMinimized
        End Get
        Set(ByVal value As Image)

            If value Is Nothing Then
                _ImageMinimized = My.Resources.imgMinimized
            Else
                _ImageMinimized = value
            End If

            Me.Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' The image displayed when the control has been maximized.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("RefEdit DropButton"), Description("The image displayed when the control has been maximized.")>
    Public Property ImageMaximized As Image
        Get
            Return _ImageMaximized
        End Get
        Set(ByVal value As Image)

            If value Is Nothing Then
                _ImageMaximized = My.Resources.imgMaximized
            Else
                _ImageMaximized = value
            End If

            btnState.Image = _ImageMaximized

        End Set
    End Property

#End Region

#Region "Events"

    ''' <summary>
    ''' Event is fired after a selection has been made.
    ''' </summary>
    ''' <remarks></remarks>
    <Category("RefEdit"), Description("This event is fired after a selection is made on the Excel spreadsheet.")>
    Public Event Changed As EventHandler

    ''' <summary>
    ''' Event is fired after the Dropbutton has been clicked.
    ''' </summary>
    ''' <remarks></remarks>
    <Category("RefEdit"), Description("This event is fired after the DropButton is clicked.")>
    Public Event DropButtonClicked As EventHandler

    ''' <summary>
    ''' Event is fired after the control has been minimized/maximized using the DropButton.
    ''' </summary>
    ''' <remarks></remarks>
    <Category("RefEdit"), Description("This event is fired after the control is resized via the DropButton.")>
    Public Event AfterResize As EventHandler(Of EventArgs.AfterResizeEventArgs)

    ''' <summary>
    ''' Event is fired before the control has been minimized/maximized using the DropButton.
    ''' </summary>
    ''' <remarks>This event can be cancelled.</remarks>
    <Category("RefEdit"), Description("This event is fired before the control is resized via the DropButton.")>
    Public Event BeforeResize As EventHandler(Of EventArgs.BeforeResizeEventArgs)

#End Region

#Region "Internal SelectionChange Events"
    ''' <summary>
    ''' Used to manage cross threading.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Private Delegate Sub WriteValue(ByVal value As String)

    ''' <summary>
    ''' Internal method that handles the xlSheet.SelectionChange event.
    ''' </summary>
    ''' <param name="target"></param>
    ''' <remarks></remarks>
    Private Sub SelectionChange(ByVal target As Excel.Range) Handles xlSheet.SelectionChange

        If ParentForm IsNot Nothing AndAlso ParentForm.ActiveControl IsNot Nothing Then

            'If this control is not active, then dont change the textbox content
            If Name <> ParentForm.ActiveControl.Name Then
                Exit Sub
            End If

        End If

        Dim address As String = ""
        Dim SheetName As String = "'" & target.Worksheet.Name & "'!"

        For Each rng As Excel.Range In target.Areas

            If address.Length > 0 Then address &= ","

            If _IncludeSheetName Then address &= SheetName

            address &= rng.Address(_ShowRowAbsoluteIndicator, _ShowColumnAbsoluteIndicator, Excel.XlReferenceStyle.xlA1)

            Call WriteData(address)
            'Call _NAR(rng)
        Next

        'Call _NAR(target)
    End Sub

    ''' <summary>
    ''' This method writes the value to the textbox and Address property.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Private Sub WriteData(ByVal value As String)
        If Me.InvokeRequired Then
            Me.Invoke(New WriteValue(AddressOf WriteData), New Object() {value})
        Else
            Me.txtAddress.Text = value
            Me.Address = value
            RaiseEvent Changed(Me, New System.EventArgs())
        End If
    End Sub

#End Region

#Region "Internal txtAddress Events"

    ''' <summary>
    ''' Maintains the Handler to the SelectionChange event.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtAddress_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddress.Enter
        If ExcelConnector Is Nothing Then Return

        If Me.Address <> String.Empty Then xlSheet.Range(Me.Address).Select()
        'AddHandler xlSheet.SelectionChange, AddressOf SelectionChange

    End Sub

    ''' <summary>
    ''' Removes the Handler once the textbox is no longer in use.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtAddress_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddress.Leave
        If ExcelConnector Is Nothing Then Return
        'RemoveHandler xlSheet.SelectionChange, AddressOf SelectionChange
    End Sub

    ''' <summary>
    ''' Maintains the resize of the control if applicable.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtAddress_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles txtAddress.PreviewKeyDown
        If e.KeyCode = Keys.F4 Then
            Call _Resize()
            Me.txtAddress.Focus()
        End If
    End Sub

#End Region

#Region "Internal btnState Events"
    ''' <summary>
    ''' Maintains the resize of the control.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnState.Click
        Call _Resize()
        Me.txtAddress.Focus()

        RaiseEvent DropButtonClicked(Me, New System.EventArgs)
    End Sub

    ''' <summary>
    ''' Executes prior to the resize occuring. Allowing for the chance of the resize event to be canceled.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OnBeforeResize(ByVal e As EventArgs.BeforeResizeEventArgs)
        RaiseEvent BeforeResize(Me, e)
    End Sub

    ''' <summary>
    ''' This method minimizes/maximizes the control based on its current display state.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _Resize()

        ' Manages the BeforeResize Event
        Dim args As New EventArgs.BeforeResizeEventArgs With {.DisplayState = Me.DisplayState}
        Call OnBeforeResize(args)
        If args.Cancel Then Return

        ' Hides/Shows all of the controls on the form
        For Each c As Control In ParentForm.Controls
            c.Visible = DisplayState.IsParentMinimized
        Next

        ' Makes sure the control is visible
        Me.Visible = True

        ' This is a fix related to placing the control into other container types such
        ' as TabControls or Groupboxes.
        If DisplayState.ActualParent Is Nothing Then DisplayState.ActualParent = Me.Parent

        If Not DisplayState.IsParentMinimized Then

            ' This is a fix related to placing the control into other container types such
            ' as TabControls or Groupboxes.
            If Not TypeOf DisplayState.ActualParent Is Form Then
                Me.ParentForm.Controls.Add(Me)
            End If

            ' Set the button's image to the minimized image
            Me.btnState.Image = _ImageMinimized

            ' Store the current state of the form
            DisplayState.ParentClientSize = ParentForm.ClientSize
            DisplayState.ParentPrevBorder = ParentForm.FormBorderStyle
            DisplayState.ShowParentControlBox = ParentForm.ControlBox

            ' Store the current state of the control
            DisplayState.ControlPrevX = Me.Left
            DisplayState.ControlPrevY = Me.Top
            DisplayState.ControlAnchor = Me.Anchor

            ' Set the new state minimized state of the form.
            ParentForm.ClientSize = New Size(Me.Width, Me.Height)
            Me.ParentForm.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.ParentForm.ControlBox = False

            ' Set the location of the control
            Me.Anchor = AnchorStyles.Left
            Me.Left = 0
            Me.Top = 0

        Else

            ' This is a fix related to placing the control into other container types such
            ' as TabControls or Groupboxes.
            If Not TypeOf DisplayState.ActualParent Is Form Then
                Me.ParentForm.Controls.Remove(Me)
                DisplayState.ActualParent.Controls.Add(Me)
            End If

            ' Set the button's image to the maximized image
            Me.btnState.Image = _ImageMaximized

            ' Set the form to the stored state
            ParentForm.ClientSize = DisplayState.ParentClientSize
            ParentForm.FormBorderStyle = DisplayState.ParentPrevBorder
            ParentForm.ControlBox = DisplayState.ShowParentControlBox

            ' Set the control to the stored state
            Me.Anchor = DisplayState.ControlAnchor
            Me.Left = DisplayState.ControlPrevX
            Me.Top = DisplayState.ControlPrevY

        End If

        ' Set the display state of the parent
        DisplayState.IsParentMinimized = Not DisplayState.IsParentMinimized

        ' Raises the AfterResize event
        RaiseEvent AfterResize(Me, New EventArgs.AfterResizeEventArgs With {.DisplayState = DisplayState})

    End Sub

#End Region

#Region "Release of COM objects"

    ''' <summary>
    ''' Releases a COM object.
    ''' </summary>
    ''' <param name="ComObj">The COM object to be released and disposed.</param>
    ''' <remarks></remarks>
    Private Sub _NAR(ByVal ComObj As Object)
        Try
            Marshal.ReleaseComObject(ComObj)

            ComObj = Nothing

            GC.Collect()
            GC.WaitForPendingFinalizers()
        Catch ex As Runtime.InteropServices.COMException
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' When the control is disposed, make sure all the COM Objects are disposed of and the handlers removed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Excel2007RefEdit_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

        Try
            If _ExcelConnector Is Nothing Then Return

            'does not make sense for an excel addin, there is only one excel instance throughout
            'RemoveHandler xlSheet.SelectionChange, AddressOf SelectionChange

            'Call _NAR(xlSheet)
            'Call _NAR(xlBook)
            'Call _NAR(ExcelConnector)

            DisplayState = Nothing
        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class

Namespace EventArgs
    ''' <summary>
    ''' Event Args related to the AfterResize Event.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AfterResizeEventArgs
        Inherits System.EventArgs

        Public Property DisplayState As RefEditState

    End Class

    ''' <summary>
    ''' Event Args related to the BeforeResize Event.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BeforeResizeEventArgs
        Inherits System.ComponentModel.CancelEventArgs

        Public Property DisplayState As RefEditState

    End Class
End Namespace

