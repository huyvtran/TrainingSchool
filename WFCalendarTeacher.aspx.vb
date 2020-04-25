Option Explicit On


Imports System
Imports System.Globalization
Imports System.Data.SqlClient
Imports System.IO
Imports Newtonsoft.Json
Imports MySql.Data.MySqlClient
Imports TrainingSchool.SharedRoutines


Public Class WFHome
    Inherits System.Web.UI.Page
    Dim dt As DataTable = Nothing



    Dim sqlstring As String = String.Empty

    Dim utility As New SharedRoutines
    Dim annosel As Integer
    Dim rconn As rconnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Try

            SharedRoutines.SetAcl(New List(Of String)(New String() {"8", "2", "9"}))


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
            Response.Write(ex.ToString)
        End Try

    End Sub

End Class