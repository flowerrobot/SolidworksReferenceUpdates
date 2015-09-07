Public Class CreateCSVFile
    Public Property FileName As String
    Sub New(ByRef iFileName As String)
        FileName = iFileName
    End Sub
    Public Sub CreateFile(ByVal Information As Dictionary(Of String, dmDocument))
        Dim File As New IO.StreamWriter(FileName)
        File.WriteLine("Name,Configuration,Path")
        For Each Item As dmDocument In Information.Values
            File.WriteLine(Item.FileName & "," & Item.SelectedConfiguration & "," & Item.FullFileName)
        Next
        File.Close()
    End Sub
End Class
