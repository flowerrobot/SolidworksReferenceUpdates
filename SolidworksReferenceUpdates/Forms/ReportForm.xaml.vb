Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports PropertyChanged

<ImplementPropertyChanged>
Public Class ReportForm
    Public Shared ReadOnly ResultsProperty As DependencyProperty = DependencyProperty.Register("Results", GetType([String]), GetType(ReportForm), New FrameworkPropertyMetadata(String.Empty))

    Private _Results As ObservableCollection(Of FileResults)
    Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        Me.DataContext = Me

        DisTree.ItemsSource = Errors
    End Sub
    Sub New(AllResults As ObservableCollection(Of FileResults))
        Me.New()
        Results = AllResults
    End Sub
    Public Property Errors As New ObservableCollection(Of FileResults)
    Public Property Warnings As New ObservableCollection(Of FileResults)
    Public Property Ok As New ObservableCollection(Of FileResults)

    Public Property Results As ObservableCollection(Of FileResults)
        Get
            Return _Results
        End Get
        Set(value As ObservableCollection(Of FileResults))
            _Results = value

            Errors.Clear()
            Warnings.Clear()
            Ok.Clear()
            For Each Res As FileResults In Results
                If Res.Errors.Count > 0 Then
                    Errors.Add(Res)
                End If
            Next
            For Each Res As FileResults In Results
                If Not Errors.Contains(Res) AndAlso Res.Warnings.Count > 0 Then
                    Warnings.Add(Res)
                End If
            Next
            For Each Res As FileResults In Results
                If Not Errors.Contains(Res) AndAlso Res.Warnings.Count > 0 Then
                    Ok.Add(Res)
                End If
            Next


            'SP_Errors.Children.Clear()
            'SP_Warnings.Children.Clear()
            'SP_OK.Children.Clear()


            'Dim NewLIst As New List(Of FileResults)
            'For Each Res As FileResults In Results
            '    If Res.Errors.Count > 0 Then
            '        NewLIst.Add(Res)
            '        SP_Errors.Children.Add(New ErrorMessageBox(Res))
            '    End If
            'Next

            'EXP_Errors.Header = "Errors : [" & NewLIst.Count & "]"
            'Dim OldList As New List(Of FileResults)
            'OldList.AddRange(NewLIst)
            'NewLIst.Clear()
            'For Each Res As FileResults In Results
            '    If Res.Warnings.Count > 0 Then
            '        If Not OldList.Contains(Res) Then
            '            OldList.Add(Res)
            '            SP_Warnings.Children.Add(New ErrorMessageBox(Res))
            '        End If
            '    End If
            'Next
            'EXP_Warnings.Header = "Warnings : [" & NewLIst.Count & "]"

            'OldList.AddRange(NewLIst)
            'NewLIst.Clear()
            'For Each Res As FileResults In Results
            '    If Not OldList.Contains(Res) Then
            '        OldList.Add(Res)
            '        SP_OK.Children.Add(New ErrorMessageBox(Res))
            '    End If
            'Next
            'EXP_Ok.Header = "Ok : [" & NewLIst.Count & "]"
            'ChecksHaveChanged(False)

            ''add events to update qty on check
            'For Each Child As ErrorMessageBox In SP_Errors.Children
            '    AddHandler Child.CheckChanged, AddressOf ChecksHaveChanged
            'Next
            'For Each Child As ErrorMessageBox In SP_Warnings.Children
            '    AddHandler Child.CheckChanged, AddressOf ChecksHaveChanged
            'Next
            'For Each Child As ErrorMessageBox In SP_OK.Children
            '    AddHandler Child.CheckChanged, AddressOf ChecksHaveChanged
            'Next
        End Set
    End Property
    Public Property ExportButtonVisable As Boolean


    Private Sub Btn_Export_Click(sender As Object, e As Windows.RoutedEventArgs) Handles Btn_Export.Click
        'Dim Export As New FileCheckerResults_Export()
        'Export.Write2File(Results.ToArray)
    End Sub
    Public Sub ChecksHaveChanged(ByVal IsChecked As Boolean)
        UpdateQtys()
    End Sub
    Public Sub UpdateQtys()
        'EXP_Errors.Header = "Errors : [" & GatherQty(SP_Errors) & "]"
        'EXP_Warnings.Header = "Warnings : [" & GatherQty(SP_Warnings) & "]"
        'EXP_Ok.Header = "Ok : [" & SP_OK.Children.Count & "]"
    End Sub
    Public Function GatherQty(Panel As Windows.Controls.StackPanel) As String
        Dim Total As Integer = Panel.Children.Count
        If Total = 0 Then Return Total.ToString
        If Panel.Name = "EXP_Ok" Then Return Total.ToString
        Dim AreChecked As Integer = 0
        For Each Child As ErrorMessageBox In Panel.Children
            If Child.IsChecked Then
                AreChecked += 1
            End If
        Next
        Return AreChecked & "/" & Total
    End Function
End Class
