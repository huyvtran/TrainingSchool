Imports System.IO

Public Class WfSetProp
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Session("idUser") = 0 OrElse Session("idUser") Is Nothing Then
            Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", " <script>ExitSession();</script>")
        End If

        Try
            Dim l As New SharedRoutines
            titleobj.Text = l.GetTitleReference(Request.QueryString("idOrg"))
        Catch ex As Exception
          SharedRoutines.LogWrite(Now & " "   & ex.tostring)
        End Try

    End Sub

    



End Class