Imports TrainingSchool.SharedRoutines
Public Class WfReportSurvey
    Inherits System.Web.UI.Page
    Dim rconn As rconnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SharedRoutines.SetAcl(New List(Of String)(New String() {"5", "4"}))

        rconn = CheckDatabase(rconn)
        If Request.QueryString("idCourse") <> "" Then

            LoadQuestionario()
        End If

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function LoadCorsi() As ArrayList
        Dim lstArrCorsi As New ArrayList()
        Try
            Dim rconn As rconnection
            rconn = CheckDatabase(rconn)
            Dim sqlstring = "select idCourse,code,name from  learning_course  order by name asc"

            Dim dt As DataTable = Nothing

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            For Each dr In dt.Rows
                Dim l1 As New ListItem
                l1.Text = dr("code") & "-" & dr("name")
                l1.Value = dr("idCourse")
                lstArrCorsi.Add(l1)
            Next
            Return lstArrCorsi

        Catch ex As Exception

        End Try

        'Return False

    End Function

    Public Function LoadQuestionario() As String

        Dim dtquest As DataTable

        Dim sqlstring As String = "select * from  learning_course  where idcourse=" & Request.QueryString("idcourse") & " order by idcourse desc"
        Dim dtcourse As DataTable = rconn.GetDataTable(sqlstring)
        Dim nomecorso As String = String.Empty
        Dim t1, t2, t3, t4, t5, t6 As Integer
        Dim scriptappend As String = String.Empty

        For Each dr In dtcourse.Rows

            nomecorso = dr("Name")

            scriptappend &= "<table  class=""table table-bordered table-striped"" >"
            scriptappend &= "<thead class=""thin-border-bottom""><h2>" & dr("code") & ":" & nomecorso & "</h2></thead>"

            sqlstring = "select idresource from  learning_organization  where objecttype='poll' and idcourse=" & dr("idCourse")
            Dim idresource As Integer
            Try
                idresource = rconn.GetDataTable(sqlstring).Rows(0)("idresource")
                sqlstring = "select * from learning_pollquest where id_poll=" & idresource
                dtquest = rconn.GetDataTable(sqlstring)

                For Each drquest In dtquest.Rows
                    scriptappend &= "<tr><th>" & drquest("title_quest").ToString & "<th>"
                    sqlstring = "select * from learning_pollquestanswer where id_quest =" & drquest("id_quest").ToString

                    Dim dtanswer As DataTable = rconn.GetDataTable(sqlstring)

                    For Each dranswer In dtanswer.Rows
                        Dim idanswer As Integer = dranswer("id_answer")
                        If idanswer = 0 Then
                            sqlstring = "select more_info from learning_polltrack_answer where id_quest=" & dranswer("id_quest") & " and  id id_answer=0"
                            Dim dtmoreinfo As DataTable = rconn.GetDataTable(sqlstring)

                            For Each drmoreinfo In dtmoreinfo.Rows
                                scriptappend &= "<th>" & drmoreinfo("more_info") & "<br> </th>"
                            Next
                        Else
                            sqlstring = "select count(*) as numanswer from learning_polltrack_answer where id_answer=" & idanswer

                            Dim countanswer As String = rconn.GetDataTable(sqlstring).Rows(0)("numanswer").ToString
                            Select Case dranswer("answer")
                                Case "1.png"
                                    t1 += countanswer
                                Case "2.png"
                                    t2 += countanswer
                                Case "3.png"
                                    t3 += countanswer
                                Case "4.png"
                                    t4 += countanswer
                                Case "5.png"
                                    t5 += countanswer
                                Case "6.png"
                                    t6 += countanswer

                            End Select



                            scriptappend &= "<th><img src=""assets/images/emoji/" & dranswer("answer") & """ > (" & countanswer & ")" & " </th>"
                        End If


                    Next


                    scriptappend &= "</tr>"
                Next

                scriptappend &= "<tr style=""font-size:medium;font-weight:bold""><td>TOTALI</td><td></td><td>" & t1 & "</td><td>" & t2 & "</td><td>" & t3 & "</td><td>" & t4 & "</td><td>" & t5 & "</td><td>" & t6 & "</td></tr>"
            Catch ex As Exception
                Response.Write(scriptappend & "<br>Nessun risultato")
                Response.End()
            End Try
            scriptappend &= "</table>"
        Next
        Try
            If dtquest.Rows.Count > 0 Then
                Response.Write(scriptappend)
            Else
                Response.Write(scriptappend & "<br>Nessun risultato")

            End If
        Catch ex As Exception
        End Try
        Response.End()
        Return False

    End Function

End Class