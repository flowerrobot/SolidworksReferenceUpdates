Public Class ReplaceRules
    Public Property Find As String
    Public Property ReplaceWith As String

    Sub New()

    End Sub
    Sub New(iFind As String, iReplaceWith As String)
        Find = iFind
        ReplaceWith = iReplaceWith
    End Sub
    Public Function IsValid() As Boolean
        Return Not String.IsNullOrWhiteSpace(Find) AndAlso Not String.IsNullOrWhiteSpace(ReplaceWith)
    End Function
End Class
