Imports PropertyChanged

<ImplementPropertyChanged>
Public Class Result
    Public Property Heading As String
    Public Property Message As String
    Sub New(ByVal inHeading As String, ByVal inMessage As String)
        Heading = inHeading
        Message = inMessage
    End Sub
End Class