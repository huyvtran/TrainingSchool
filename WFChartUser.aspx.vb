Imports System.IO

Public Class WFChartUser
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("idUser") = 0 OrElse Session("idUser") Is Nothing Then
            Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", " <script>ExitSession();</script>")
        End If
        Try
            Dim utility As New SharedRoutines
            Dim u As New LogSession
            lbtotaltime.Text = utility.ConvertSecToDate(u.GetUserTotalCourseTime(Request.QueryString("iduser"), Request.QueryString("idcourse")))
            Lbtotaltimevideocorsi.Text = u.getUserTotalVideocourse(Request.QueryString("iduser"), Request.QueryString("idcourse"))

        Catch ex As Exception
          SharedRoutines.LogWrite(Now & " "   & ex.tostring)
        End Try

    End Sub





End Class