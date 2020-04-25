Imports LMS
Imports System.IO
Imports System.Web
Imports System.Web.SessionState

Public Class WFViewScorm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim scr As New Scorm
        Page.Title = scr.title

        If Request.QueryString("reference") <> "" Then
            Try

                scr.GetScormByReference(Request.QueryString("reference"), Session("idcourse"))

            Catch ex As Exception
                Response.Redirect("~/s500.aspx")
            End Try
        End If

    End Sub

End Class