Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports Microsoft.Win32

Class MainWindow
    Implements INotifyPropertyChanged
    Dim Rules As New ObservableCollection(Of ReplaceRules)
    Dim _FileName As String
    Sub New()
        InitializeComponent()
        Me.DataContext = Me
        DG_Replaces.ItemsSource = Rules
    End Sub
    Public Property FileName As String
        Get
            Return _FileName
        End Get
        Set(value As String)
            _FileName = value
            PropertyHasChanged("FileName")
        End Set
    End Property
    Private Sub Btn_Broswe_Click(sender As Object, e As RoutedEventArgs)
        Dim Dia As New OpenFileDialog
        With Dia
            .CheckFileExists = True
            .Filter = "Solidworks files (*.sldasm;*.sldprt)|*.sldasm;*.sldprt|Assembly (*.sldasm)|*.sldasm|Part (*.sldprt)|*.sldprt "
            .Multiselect = False
        End With
        If Dia.ShowDialog Then
            FileName = Dia.FileName
        End If

    End Sub

    Private Sub Btn_Ok_Click(sender As Object, e As RoutedEventArgs)
        If IO.File.Exists(FileName) And Rules.Count > 0 Then
            Dim UpdateFiles As New UpdateFiles(FileName)
            UpdateFiles.Update(Rules)
        End If
        ' Me.DialogResult = True
        Me.Close()
    End Sub
    Private Sub Btn_Cancel_Click(sender As Object, e As RoutedEventArgs)
        '  Me.DialogResult =        False
        Me.Close()
    End Sub
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Private Sub PropertyHasChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
End Class
