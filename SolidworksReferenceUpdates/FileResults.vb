Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports PropertyChanged

<ImplementPropertyChanged>
Public Class FileResults
    Dim _FileName, _Description As String
    Dim _Status As FilerCheckerResult = FilerCheckerResult.Unknown
    Dim _Errors As New ObservableCollection(Of Result)
    Dim _Warnings As New ObservableCollection(Of Result)
    Dim _HasDrawing As Boolean
    Dim _Path As String
    Public Sub New(ByVal FileName As String)
        _FileName = FileName
    End Sub
#Region "Properties"
    ReadOnly Property FileName() As String
        Get
            Return _FileName
        End Get
    End Property
    Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            Status = FilerCheckerResult.Ok
            _Description = value
        End Set
    End Property
    Property Status() As FilerCheckerResult
        Get
            Return _Status
        End Get
        Set(ByVal value As FilerCheckerResult)
            If value > _Status Then
                _Status = value
            End If
            If HasDrawing Then
                If _Status = FilerCheckerResult.Ok Then
                    _Status = FilerCheckerResult.OkWithDrawing
                ElseIf _Status = FilerCheckerResult.IsWrong Then
                    _Status = FilerCheckerResult.IsWrongWithDrawing
                ElseIf _Status = FilerCheckerResult.Warning Then
                    _Status = FilerCheckerResult.WarningWithDrawing
                End If
            End If
        End Set
    End Property
    Public Property HasDrawing() As Boolean
        Get
            Return _HasDrawing
        End Get
        Set(ByVal value As Boolean)
            _HasDrawing = value
            Status = FilerCheckerResult.Ok
        End Set
    End Property
    ReadOnly Property Errors() As ObservableCollection(Of Result)
        Get
            Return _Errors
        End Get
    End Property
    ReadOnly Property Warnings() As ObservableCollection(Of Result)
        Get
            Return _Warnings
        End Get
    End Property
    ReadOnly Property AllResults As ObservableCollection(Of Result)
        Get
            Dim Items As New ObservableCollection(Of Result)

            If Errors.Count > 0 Then
                For Each Res As Result In Errors
                    Items.Add(Res)
                Next
            End If
            If Warnings.Count > 0 Then
                For Each Res As Result In Warnings
                    Items.Add(Res)
                Next
            End If
            Return Items
        End Get
    End Property


#End Region

    Property path As String
        Get
            Return _Path
        End Get
        Set(value As String)
            _Path = value
        End Set
    End Property

    Public Sub AddResult(ByVal Heading As String, ByVal Message As String, ByVal IsError As Boolean)
        If IsError Then
            Status = FilerCheckerResult.IsWrong
            _Errors.Add(New Result(Heading, Message))
        Else
            Status = FilerCheckerResult.Warning
            _Warnings.Add(New Result(Heading, Message))
        End If
    End Sub

End Class
Public Enum FilerCheckerResult As Integer
    Unknown = 0
    NotApplicable = 1
    Ok = 2
    OkWithDrawing = 3
    Warning = 4
    WarningWithDrawing = 5
    IsWrong = 6
    IsWrongWithDrawing = 7
End Enum

