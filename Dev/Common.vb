Public Module Common
    Public Rng As New Random(5)
    Public Function FormatCommaList(Of T)(ByVal list As List(Of t)) As String
        Dim total As String = ""
        For n = 0 To list.Count - 1
            total &= list(n).ToString
            If n <> list.Count - 1 Then total &= ", "
        Next
        Return total
    End Function
End Module
