Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Sub New()
        AddHandler Me.DispatcherUnhandledException, AddressOf EpicError
    End Sub
    Sub EpicError(sender As Object, e As System.Windows.Threading.DispatcherUnhandledExceptionEventArgs) Handles Me.DispatcherUnhandledException
        Dim errorMessage As String = String.Format("An unhandled exception occurred: {0}", e.Exception.Message)
        MsgBox(errorMessage, MsgBoxStyle.Critical, "Error")
        e.Handled = True
    End Sub

End Class
