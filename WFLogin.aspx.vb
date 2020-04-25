Imports TrainingSchool.SharedRoutines
Public Class WLogin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetSession()
    End Sub

End Class