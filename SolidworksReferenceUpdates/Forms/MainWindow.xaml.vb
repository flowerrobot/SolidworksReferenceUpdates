Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports Microsoft.Win32
Imports PropertyChanged
Imports SwDocumentMgr
Imports MahApps.Metro.Controls

<ImplementPropertyChanged>
Class MainWindow
    Inherits MetroWindow
    Dim Rules As New ObservableCollection(Of ReplaceRules)
    Dim Results As New ObservableCollection(Of FileResults)
    Dim _FileName As String

    Sub New()
        InitializeComponent()
        Me.DataContext = Me
        DG_Replaces.ItemsSource = Rules


        'Dim Report As New ReportForm

        'For index = 1 To 10
        '    Dim Res As New FileResults("aFileName" & index)
        '    Res.Description = "LOL" & index
        '    Res.AddResult("Test" & index, "mssage", True)
        '    Res.AddResult("Test" & index, "mssage2", True)
        '    Results.Add(Res)
        'Next

        'Report.Results = Results
        ''Report.Results = New ObservableCollection(Of FileResults)
        ''Report.Results.Add(New FileResults("Test") With {.Description = "HI"})
        'Dim Win As New Window
        'With Win
        '    .Title = "Report"
        '    .Content = Report
        '    .SizeToContent = SizeToContent.WidthAndHeight
        '    .ResizeMode = ResizeMode.CanResizeWithGrip
        'End With
        'Win.ShowDialog()
    End Sub
    Public Property TopAssemblyPath As String
        Get
            Return _FileName
        End Get
        Set(value As String)
            _FileName = value

            Dim doc As SwDMDocument18 = dmDocument.OpenDocument(TopAssemblyPath)
            AllConfigurations.Clear()

            If doc IsNot Nothing Then
                For Each name As String In doc.ConfigurationManager.GetConfigurationNames()
                    AllConfigurations.Add(name)
                Next
                doc.CloseDoc()

                SelectedConfiguration = AllConfigurations(0)
            End If
        End Set
    End Property
    Public Property AllConfigurations As New ObservableCollection(Of String)
    Public Property SelectedConfiguration As String

    Private Sub Btn_Broswe_Click(sender As Object, e As RoutedEventArgs)
        Dim Dia As New OpenFileDialog
        With Dia
            .CheckFileExists = True
            .Filter = "Solidworks files (*.sldasm;*.sldprt)|*.sldasm;*.sldprt|Assembly (*.sldasm)|*.sldasm|Part (*.sldprt)|*.sldprt "
            .Multiselect = False
        End With
        If Dia.ShowDialog Then
            TopAssemblyPath = Dia.FileName
        End If
    End Sub
    Private Sub Btn_Ok_Click(sender As Object, e As RoutedEventArgs)
        If IO.File.Exists(TopAssemblyPath) And Rules.Count > 0 Then
            Dim UpdateFiles As New UpdateFiles(TopAssemblyPath)
            UpdateFiles.Update(Rules)
        End If
        Dim Report As New ReportForm
        'Report.Results = UpdateFiles.Results
        'Dim Win As New Window
        'With Win
        '    .Title = "Report"
        '    .Content = Report
        '    .SizeToContent = SizeToContent.WidthAndHeight
        '    .ResizeMode = ResizeMode.CanResizeWithGrip
        'End With
        'Win.ShowDialog()
        MsgBox("Complete")
        '  Me.Close()
    End Sub
    Private Sub Btn_Cancel_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Btn_CheckReferences_Click(sender As Object, e As RoutedEventArgs)
        Dim REF As New ReferenceList
        REF.TopLevelFile(SelectedConfiguration) = TopAssemblyPath
        REF.ShowDialog()
        'If IO.File.Exists(FileName) And Rules.Count > 0 Then
        '    Dim UpdateFiles As New UpdateFiles(FileName)
        '    UpdateFiles.Update(New ObservableCollection(Of ReplaceRules))
        'End If
        'Dim Report As New ReportForm
        'Report.Results = UpdateFiles.Results
        'Dim Win As New Window
        'With Win
        '    .Title = "Report"
        '    .Content = Report
        '    .SizeToContent = SizeToContent.WidthAndHeight
        '    .ResizeMode = ResizeMode.CanResizeWithGrip
        'End With
        'Win.Show()
    End Sub

    Public ReadOnly Property ProgramVersion As String
        Get
            Return Reflection.Assembly.GetEntryAssembly.GetName.Version.ToString
        End Get
    End Property

End Class
