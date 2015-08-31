Imports System.Collections.ObjectModel
Imports System.Windows.Media
Imports PropertyChanged

<ImplementPropertyChanged>
Public Class ErrorMessageBox

    Public Shared ReadOnly ErrorResultsProperty As DependencyProperty = DependencyProperty.Register("Results", GetType([String]), GetType(ErrorMessageBox), New FrameworkPropertyMetadata(String.Empty))
    Public Shared ReadOnly AllResultsProperty As DependencyProperty = DependencyProperty.Register("AllResults", GetType([String]), GetType(ErrorMessageBox), New FrameworkPropertyMetadata(String.Empty))

    Public Event CheckChanged(ByVal IsChecked As Boolean)
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'DisTree.ItemsSource = AllResults
    End Sub
    Sub New(Results As FileResults)
        Me.New()
        Results = Results
        If Not IO.File.Exists(Results.path) Then
            BT_Open.Visibility = Windows.Visibility.Collapsed
        End If
    End Sub
    Private _FileResults As FileResults
    Private _FileName As String = "FileName is here"
    Public Property Results As FileResults
        Get
            Return _FileResults
        End Get
        Set(value As FileResults)
            _FileResults = value
            Expander.Header = HeaderText
            DisTree.ItemsSource = AllResults
        End Set
    End Property
    Public ReadOnly Property AllResults As ObservableCollection(Of Result)
        Get
            If Results Is Nothing Then Return Nothing
            Return Results.AllResults
        End Get
    End Property
    Public ReadOnly Property FileName As String
        Get
            Return Results.FileName
        End Get
    End Property
    Public ReadOnly Property HeaderText As String
        Get
            Return "Document : " & FileName & "-" & Results.Description
        End Get
    End Property
    Private Sub Sorted_Checked(sender As Object, e As Windows.RoutedEventArgs) Handles CB_Sorted.Checked
        RaiseEvent CheckChanged(True)
        ChangeColor("#FF03DA52")
        Expander.IsExpanded = False
    End Sub
    Private Sub Sorted_Unchecked(sender As Object, e As Windows.RoutedEventArgs) Handles CB_Sorted.Unchecked
        RaiseEvent CheckChanged(False)
        ChangeColor("#FF000000")
    End Sub
    Sub ChangeColor(color As String)
        Dim converter As New System.Windows.Media.BrushConverter
        Dim brush = converter.ConvertFromString(color)
        Expander.Foreground = brush
        'For Each Item As ErrorText In StackPanel.Children
        '    Item.TextColour = brush
        'Next
    End Sub
    Public Property IsExpanded As Boolean = True
    Public Property IsChecked As Boolean
    Public Property ShowFolderButton As Boolean = True

    Private Sub BT_Open_Click(sender As Object, e As Windows.RoutedEventArgs) Handles BT_Open.Click
        If IO.File.Exists(Results.path) Then
            Dim xlProc As System.Diagnostics.Process = System.Diagnostics.Process.Start(Results.path)
        End If
    End Sub
End Class
