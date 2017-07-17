Public Class Blueprint
    Private BlueprintName As String
    Private MechPartSlot As String
    Private ComponentsCompulsory As New List(Of String)
    Private ComponentsFilled As New List(Of String)
    Private Components As New List(Of Component)
    Private BlueprintModifiers As New Component                             'hidden component that holds the modifiers from the blueprint

    Public Shared Function Load(ByVal blueprintName As String) As Blueprint
        Const path As String = "data/blueprints.txt"
        Dim q As Queue(Of String) = SquareBracketLoader(path, blueprintName)

        Dim blueprint As New Blueprint
        With blueprint
            .BlueprintName = q.Dequeue()
            While q.Count > 0
                Dim line As String() = q.Dequeue.Split(":")
                Dim key As String = line(0).Trim
                Dim value As String = line(1).Trim
                .Construct(key, value)
            End While
        End With
        Return blueprint
    End Function
    Public Sub Construct(ByVal key As String, ByVal value As String)
        Select Case key
            Case "Slot" : MechPartSlot = value
            Case "Component" : ComponentsCompulsory.Add(value)
            Case Else : BlueprintModifiers.Construct(key, value)
        End Select
    End Sub
    Public Function ConstructMechPart(ByVal mechPartName As String) As MechPart
        If ComponentsCompulsory.Count > 0 Then Return Nothing
        If BlueprintModifiers Is Nothing = False AndAlso BlueprintModifiers.IsNotEmpty = True Then Components.Add(BlueprintModifiers)
        Return MechPart.Construct(BlueprintName, mechPartName, MechPartSlot, Components)
    End Function

    Public Function AddComponent(ByVal component As Component) As String
        Dim c As String = component.Slot
        If ComponentsCompulsory.Contains(c) Then
            ComponentsCompulsory.Remove(c)
            ComponentsFilled.Add(c)
        Else
            Return "Invalid component type."
        End If

        Components.Add(component)
        Return Nothing
    End Function
    Public Function RemoveComponent(ByVal component As Component) As String
        Dim c As String = component.Slot
        If Components.Contains(component) = False Then Return "Component not in list."

        Components.Remove(component)
        ComponentsFilled.Remove(c)
        ComponentsCompulsory.Add(c)
        Return Nothing
    End Function
End Class
