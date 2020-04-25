

Public Class WFCalendar
    Inherits System.Web.UI.Page

    Dim dt As DataTable = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SharedRoutines.SetAcl(New List(Of String)(New String() {"3", "7"}))

    End Sub


End Class