Imports TrainingSchool.SharedRoutines

Public Class WFMakeItem
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            SharedRoutines.SetAcl(New List(Of String)(New String() {"8", "2"}))

            If Session("idUser") = 0 OrElse Session("idUser") Is Nothing Then
                Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", " <script>ExitSession();</script>")
            End If

        Catch ex As Exception

        Finally

        End Try



    End Sub






End Class