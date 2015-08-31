Imports PropertyChanged

<ImplementPropertyChanged>
Public Class ErrorText
    Public Shared ReadOnly HeaderTextProperty As DependencyProperty = DependencyProperty.Register("HeaderText", GetType([String]), GetType(ErrorText), New FrameworkPropertyMetadata(String.Empty))
    Public Shared ReadOnly ContentTextProperty As DependencyProperty = DependencyProperty.Register("ContentText", GetType([String]), GetType(ErrorText), New FrameworkPropertyMetadata(String.Empty))

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.    
    End Sub
    Sub New(Header As String, Content As String)
        Me.New()
        HeaderText = Header
        ContentText = Content
    End Sub
    Public Property HeaderText As String
    Public Property ContentText As String

    Public Property TextColour As Windows.Media.Brush
        Get
            Return Header.Foreground
        End Get
        Set(value As Windows.Media.Brush)
            Header.Foreground = value
            BodyContent.Foreground = value
        End Set
    End Property

End Class
