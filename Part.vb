Public MustInherit Class Part
    Public Category As String               'main class of part
End Class

Public MustInherit Class PartContainer
    Protected PartsCompulsory As New List(Of String)
    Protected PartsFilled As New List(Of String)
    Protected Parts As New List(Of Part)
    Protected PlanModifiers As Part

    Public Function ConstructBit() As Part
        If PartsCompulsory.Count > 0 Then Return Nothing
        If PlanModifiers Is Nothing = False Then Parts.Add(PlanModifiers)
    End Function
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