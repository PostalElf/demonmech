Public MustInherit Class Part
    Public Category As String               'main class of part
End Class

Public MustInherit Class PartContainer
    Private PartsCompulsory As New List(Of String)
    Private PartsFilled As New List(Of String)
    Private Parts As New List(Of Part)
    Private PlanModifiers As Part

    Public MustOverride Function ConstructBit() As Part
    Public Function AddPart(ByVal part As Part) As String
        Dim c As String = part.Category
        If PartsCompulsory.Contains(c) = False Then Return "Invalid part category"

        PartsCompulsory.Remove(c)
        PartsFilled.Add(c)
        Parts.Add(part)
        Return Nothing
    End Function
    Public Function RemovePart(ByVal part As Part) As String
        If Parts.Contains(part) = False Then Return "Part not in list"

        Dim c As String = part.Category
        Parts.Remove(part)
        PartsFilled.Remove(c)
        PartsCompulsory.Add(c)
        Return Nothing
    End Function
End Class