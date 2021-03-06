﻿Imports System.Collections.ObjectModel
Imports PropertyChanged

<ImplementPropertyChanged>
Public Class ReferenceList
    Friend Property References As New ObservableCollection(Of dmDocument)
    Private _TopLevelFile As String
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.DataContext = Me
        TV_Tree.ItemsSource = References
    End Sub
    Public Property TopLevelFile(config As String) As String
        Get
            Return _TopLevelFile
        End Get
        Set(value As String)
            References.Clear()
            _TopLevelFile = value
            dmDocument.AllFiles.Clear()
            Dim Doc As New dmDocument(value, config)
            Doc.GetReferences()
            References.Add(Doc)

        End Set
    End Property
    Private Sub Btn_Ok_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
    Private Sub Btn_Cancel_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Btn_Export_Click(sender As Object, e As RoutedEventArgs)
        Dim Save As New Microsoft.Win32.SaveFileDialog
        Save.Filter = "CSV (*.csv)|*.csv;"
        If Save.ShowDialog Then
            Try
                Dim CSVFIle As New CreateCSVFile(Save.FileName)
                CSVFIle.CreateFile(dmDocument.AllFiles)
                MsgBox("Completed")
            Catch ex As Exception
                MsgBox("An error occured")
            End Try
        End If
    End Sub
End Class
