Imports Newtonsoft.Json
Public Class Child2
    <JsonProperty("id")> _
    Public Property id() As Integer
        Get
            Return m_id
        End Get
        Set(value As Integer)
            m_id = value
        End Set
    End Property
    Private m_id As Integer
End Class

Public Class Child

    <JsonProperty("id")> _
Public Property id() As Integer
        Get
            Return m_id
        End Get
        Set(value As Integer)
            m_id = value
        End Set
    End Property
    Private m_id As Integer

    <JsonProperty("children")> _
 Public Property children() As List(Of Child2)
        Get
            Return m_children
        End Get
        Set(value As List(Of Child2))
            m_children = value
        End Set
    End Property
    Private m_children As List(Of Child2)
End Class

Public Class RootObject
    <JsonProperty("id")> _
    Public Property id() As Integer
        Get
            Return m_id
        End Get
        Set(value As Integer)
            m_id = value
        End Set
    End Property
    Private m_id As Integer
    <JsonProperty("children")> _
    Public Property children() As List(Of Child)
        Get
            Return m_children
        End Get
        Set(value As List(Of Child))
            m_children = value
        End Set
    End Property
    Private m_children As List(Of Child)
End Class
