Imports System.IO
Imports Newtonsoft.Json
Imports System.Web.Services
Imports TrainingSchool.SharedRoutines
Public Class WFCategoryUsers
    Inherits System.Web.UI.Page

    Dim utility As SharedRoutines
    Dim conn As rconnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SharedRoutines.SetAcl(New List(Of String)(New String() {"1", "4"}))

        utility = New SharedRoutines
        conn = CheckDatabase(conn)





    End Sub






End Class