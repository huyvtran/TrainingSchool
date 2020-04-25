Public Class WFImpostazioni
    Inherits System.Web.UI.Page
    Dim rconn As rconnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SharedRoutines.SetAcl(New List(Of String)(New String() {"1", "4"}))
    End Sub





End Class