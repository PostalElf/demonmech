Public Class Blueprint
    Private Name As String
    Private ComponentsCompulsory As New List(Of String)
    Private ComponentsFilled As New List(Of String)
    Private Components As New List(Of Component)
    Private BlueprintModifiers As New Component                             'hidden component that holds the modifiers from the blueprint

    Public Shared Function Load(ByVal blueprintName As String) As Blueprint
        Const path As String = "data/blueprints.txt"
        Dim q As Queue(Of String) = SquareBracketLoader(path, blueprintName)
        Return Construct(q)
    End Function
    Public Shared Function Construct(ByVal q As Queue(Of String)) As Blueprint
        Dim blueprint As New Blueprint
        With blueprint
            .Name = q.Dequeue()
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
            Case "Component" : ComponentsCompulsory.Add(value)
            Case Else : BlueprintModifiers.Construct(key, value)
        End Select
    End Sub

    Public Function ConstructMechPart() As MechPart
        If ComponentsCompulsory.Count > 0 Then Return Nothing
        If BlueprintModifiers Is Nothing = False AndAlso BlueprintModifiers.IsNotEmpty = True Then Components.Add(BlueprintModifiers)
        Return MechPart.Construct(Name, Components)
    End Function

    Public Function AddComponent(ByVal component As Component) As String
        Dim c As String = component.Category
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
        Dim c As String = component.Category
        If Components.Contains(component) = False Then Return "Component not in list."

        Components.Remove(component)
        ComponentsFilled.Remove(c)
        ComponentsCompulsory.Add(c)
        Return Nothing
    End Function
End Class
