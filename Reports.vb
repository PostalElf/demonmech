Public Class Reports
    Private Shared Reports As New List(Of String)
    Public Shared Sub Add(ByVal value As String)
        Reports.Add(value)
    End Sub
    Public Shared Sub Clear()
        Reports.Clear()
    End Sub
    Public Shared Sub ConsoleReport(Optional ByVal prefix As String = "")
        For Each value In Reports
            Console.WriteLine(prefix & value)
        Next
    End Sub
End Class
