Imports System.IO

Public Class WFStatObj
    Inherits System.Web.UI.Page
    Dim idreference As String = String.Empty

    Dim sqlstring As String = String.Empty

    Dim rconn As rconnection
    Dim obj As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Request.QueryString("iduser") = 0 OrElse Request.QueryString("iduser") Is Nothing Then
            Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", " <script>ExitSession();</script>")
        End If

        Dim l As New SharedRoutines


        rconn = rconn
        Try

            If Request.QueryString("obj") <> "" And Request.QueryString("reference") <> "" Then
                obj = Request.QueryString("obj")
                idreference = Request.QueryString("reference")




                viewstat.Visible = True
                quizPage.Visible = True

                logStat()
                logStatTest()
                StatTest()




            End If

        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
        End Try

    End Sub




    Protected Sub StatTest()
        Dim i As Integer = 1
        Dim content As String = String.Empty

        Dim question As String = String.Empty

        sqlstring = "select idtrack from learning_testtrack where idreference=" & idreference & " and iduser=" & Request.QueryString("iduser")
        Dim idlog As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("idtrack").ToString

        sqlstring = "select c.title_quest,b.answer,score_correct,score_assigned from  ( learning_testquestanswer b  join learning_testtrack_answer a  on a.idanswer=b.idanswer) join learning_testquest  c on c.idQuest=a.idQuest  where idtrack=" & idlog & " order by a.idquest asc"
        Dim dt As DataTable = Nothing

        Try

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            content &= vbCrLf & "  <div class=""widget-box"">"
            content &= vbCrLf & "<div class=""widget-header widget-header-flat"">"
            content &= vbCrLf & "	<h4>Ultimo test</h4>"
            content &= vbCrLf & "</div>"

            content &= vbCrLf & "<div class=""widget-body"">"
            content &= vbCrLf & "<div class=""widget-main"">"

            content &= vbCrLf & "	<div class=""row"">"
            content &= vbCrLf & "	<div class=""col-xs-12"">"

            For Each dr In dt.Rows

                If dr("title_quest") = question Then


                    If dr("score_assigned") = 0 And dr("score_correct") = 0 Then
                        content &= vbCrLf & "	<li class=""muted"" >"
                        content &= vbCrLf & "	<i class=""icon-angle-right bigger-110""></i>"
                        content &= vbCrLf & "	" & dr("answer")
                        content &= vbCrLf & "	</li>"
                    ElseIf dr("score_correct") = 10 And dr("score_assigned") = 1 Then
                        content &= vbCrLf & "	<li>"
                        content &= vbCrLf & "	<i class=""icon-ok bigger-110 green""></i>"
                        content &= vbCrLf & "	" & dr("answer")
                        content &= vbCrLf & "	</li>"
                    ElseIf dr("score_assigned") = 1 And dr("score_correct") = 0 Then
                        content &= vbCrLf & "	<li>"
                        content &= vbCrLf & "		<i class=""icon-remove bigger-110 red""></i>"
                        content &= vbCrLf & "	" & dr("answer")
                        content &= vbCrLf & "	</li>"

                    ElseIf dr("score_assigned") = 0 And dr("score_correct") = 10 Then
                        content &= vbCrLf & "	<li class=""muted"" >"
                        content &= vbCrLf & "	<i class=""icon-angle-right bigger-110""></i>"
                        content &= vbCrLf & "	" & dr("answer")
                        content &= vbCrLf & "	</li>"

                    End If






                Else
                    content &= vbCrLf & "	</ul>"
                    question = dr("title_quest")
                    content &= vbCrLf & "	<h2>" & i & "." & dr("title_quest") & "</h2>"
                    i = i + 1
                    content &= vbCrLf & "	<ul class=""list-unstyled spaced"">"
                    If dr("score_assigned") = 0 And dr("score_correct") = 0 Then
                        content &= vbCrLf & "	<li class=""muted"" >"
                        content &= vbCrLf & "	<i class=""icon-angle-right bigger-110""></i>"
                        content &= vbCrLf & "	" & dr("answer")
                        content &= vbCrLf & "	</li>"
                    ElseIf dr("score_correct") = 10 And dr("score_assigned") = 1 Then
                        content &= vbCrLf & "	<li>"
                        content &= vbCrLf & "	<i class=""icon-ok bigger-110 green""></i>"
                        content &= vbCrLf & "	" & dr("answer")
                        content &= vbCrLf & "	</li>"
                    ElseIf dr("score_assigned") = 1 And dr("score_correct") = 0 Then
                        content &= vbCrLf & "	<li>"
                        content &= vbCrLf & "		<i class=""icon-remove bigger-110 red""></i>"
                        content &= vbCrLf & "	" & dr("answer")
                        content &= vbCrLf & "	</li>"

                    ElseIf dr("score_assigned") = 0 And dr("score_correct") = 10 Then
                        content &= vbCrLf & "	<li class=""muted"" >"
                        content &= vbCrLf & "	<i class=""icon-angle-right bigger-110""></i>"
                        content &= vbCrLf & "	" & dr("answer")
                        content &= vbCrLf & "	</li>"
                    End If

                End If


            Next

            content &= vbCrLf & "	</div>"
            content &= vbCrLf & "	</div>"
            content &= vbCrLf & "</div>"
            content &= vbCrLf & "</div>"
            content &= vbCrLf & "	</div>"
            quizPage.InnerHtml = content
        Catch ex As Exception
        End Try

    End Sub


    Protected Sub logStatTest()

        Try
            Dim content As String = String.Empty




            sqlstring = "select * from learning_testtrack where idreference=" & idreference & " and iduser=" & Request.QueryString("iduser")
            Dim dt As DataTable = Nothing

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            Dim dr = dt.Rows(0)
            content &= vbCrLf & " <div class=""table-responsive""> "
            content &= vbCrLf & " 			<table id=""sample-table-1"" class=""table table-striped table-bordered table-hover"">"
            content &= vbCrLf & " <thead>"
            content &= vbCrLf & " 	<tr>"
            content &= vbCrLf & " 	<th>Punteggio</th>"
            content &= vbCrLf & " 	<th>Tentativi</th>"
            content &= vbCrLf & " 	<th>Stato</th>"

            content &= vbCrLf & " 	<th class=""hidden-480"">  "
            content &= vbCrLf & " 	<i class=""icon-time bigger-110 hidden-480""></i>Data accesso</th>"

            content &= vbCrLf & " 	</tr>"
            content &= vbCrLf & " </thead>"

            content &= vbCrLf & " 	<tbody>"


            content &= vbCrLf & " 		<tr>"
            content &= vbCrLf & " 		<td class=""center"">"
            content &= vbCrLf & " 		<label>"
            content &= vbCrLf & " <span >" & dr("score").ToString & "</span>"
            content &= vbCrLf & " 	<span class=""lbl""></span>"
            content &= vbCrLf & " 	</label>"
            content &= vbCrLf & " 	</td>"
            content &= vbCrLf & " 		<td class=""center"">"
            content &= vbCrLf & " 		<label>"
            content &= vbCrLf & " <span class=""label label-sm label-warning"">" & dr("number_of_attempt").ToString & "</span>"
            content &= vbCrLf & " 	<span class=""lbl""></span>"
            content &= vbCrLf & " 	</label>"
            content &= vbCrLf & " 	</td>"

            content &= vbCrLf & " 		<td class=""center"">"
            content &= vbCrLf & " 		<label>"
            content &= vbCrLf & " <span >" & dr("score_status").ToString & "</span>"
            content &= vbCrLf & " 	<span class=""lbl""></span>"
            content &= vbCrLf & " 	</label>"
            content &= vbCrLf & " 	</td>"
            content &= vbCrLf & " 		<td class=""center"">"
            content &= vbCrLf & " 		<label>"
            content &= vbCrLf & " <span >" & dr("date_end_attempt").ToString & "</span>"
            content &= vbCrLf & " 	<span class=""lbl""></span>"
            content &= vbCrLf & " 	</label>"
            content &= vbCrLf & " 	</td>"


            content &= vbCrLf & " </tr>"

            sqlstring = "select * from learning_testtrack_times where idtrack=" & dr("idtrack") & " and idtest=" & dr("idtest") & " and idreference=" & dr("idreference")

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            For Each dr1 In dt.Rows
                Try
                    content &= vbCrLf & " 		<tr>"
                    content &= vbCrLf & " 		<td class=""center"">"
                    content &= vbCrLf & " 		<label>"
                    content &= vbCrLf & " <span >" & dr1("score").ToString & "</span>"
                    content &= vbCrLf & " 	<span class=""lbl""></span>"
                    content &= vbCrLf & " 	</label>"
                    content &= vbCrLf & " 	</td>"
                    content &= vbCrLf & " 		<td class=""center"">"
                    content &= vbCrLf & " 		<label>"
                    content &= vbCrLf & " <span class=""label label-sm label-warning"">" & dr1("number_time").ToString & "</span>"
                    content &= vbCrLf & " 	<span class=""lbl""></span>"
                    content &= vbCrLf & " 	</label>"
                    content &= vbCrLf & " 	</td>"

                    content &= vbCrLf & " 		<td class=""center"">"
                    content &= vbCrLf & " 		<label>"
                    content &= vbCrLf & " <span >" & dr1("score_status").ToString & "</span>"
                    content &= vbCrLf & " 	<span class=""lbl""></span>"
                    content &= vbCrLf & " 	</label>"
                    content &= vbCrLf & " 	</td>"
                    content &= vbCrLf & " 		<td class=""center"">"
                    content &= vbCrLf & " 		<label>"
                    content &= vbCrLf & " <span >" & dr1("date_attempt").ToString & "</span>"
                    content &= vbCrLf & " 	<span class=""lbl""></span>"
                    content &= vbCrLf & " 	</label>"
                    content &= vbCrLf & " 	</td>"


                    content &= vbCrLf & " </tr>"


                Catch ex As Exception
                    'Response.Redirect("s404.aspx")
                End Try
            Next
            content &= vbCrLf & " 	</tbody>"
            content &= vbCrLf & " 	</table>"
            content &= vbCrLf & " 				</div>"
            viewstat.InnerHtml &= content

        Catch ex As Exception
            'Response.Redirect("s404.aspx")
        End Try

    End Sub
    Protected Sub logStat()

        Try
            Dim content As String = String.Empty




            Dim sqlstring = "select * from  learning_commontrack where iduser=" & Request.QueryString("iduser") & " and idReference=" & idreference
            Dim dr As DataRow = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)

            content &= vbCrLf & " <div class=""table-responsive""> "
            content &= vbCrLf & " 			<table id=""sample-table-1"" class=""table table-striped table-bordered table-hover"">"
            content &= vbCrLf & " <thead>"
            content &= vbCrLf & " 	<tr>"
            content &= vbCrLf & " 	<th>Tipo</th>"
            content &= vbCrLf & " 	<th>Stato</th>"
            content &= vbCrLf & " 	<th>Data Accesso</th>"

            content &= vbCrLf & " 	<th class=""hidden-480"">  "
            content &= vbCrLf & " 	<i class=""icon-time bigger-110 hidden-480""></i>Data Completamento</th>"

            content &= vbCrLf & " 	</tr>"
            content &= vbCrLf & " </thead>"

            content &= vbCrLf & " 	<tbody>"
            content &= vbCrLf & " 		<tr>"
            content &= vbCrLf & " 		<td class=""center"">"
            content &= vbCrLf & " 		<label>"
            content &= vbCrLf & " <span >" & obj & "</span>"
            content &= vbCrLf & " 	<span class=""lbl""></span>"
            content &= vbCrLf & " 	</label>"
            content &= vbCrLf & " 	</td>"
            content &= vbCrLf & " 		<td class=""center"">"
            content &= vbCrLf & " 		<label>"
            content &= vbCrLf & " <span class=""label label-sm label-warning"">" & dr("status").ToString & "</span>"
            content &= vbCrLf & " 	<span class=""lbl""></span>"
            content &= vbCrLf & " 	</label>"
            content &= vbCrLf & " 	</td>"

            content &= vbCrLf & " 		<td class=""center"">"
            content &= vbCrLf & " 		<label>"
            content &= vbCrLf & " <span >" & dr("firstattempt").ToString & "</span>"
            content &= vbCrLf & " 	<span class=""lbl""></span>"
            content &= vbCrLf & " 	</label>"
            content &= vbCrLf & " 	</td>"
            content &= vbCrLf & " 		<td class=""center"">"
            content &= vbCrLf & " 		<label>"
            content &= vbCrLf & " <span >" & dr("first_complete").ToString & "</span>"
            content &= vbCrLf & " 	<span class=""lbl""></span>"
            content &= vbCrLf & " 	</label>"
            content &= vbCrLf & " 	</td>"


            content &= vbCrLf & " </tr>"
            content &= vbCrLf & " 	</tbody>"
            content &= vbCrLf & " 	</table>"
            content &= vbCrLf & " 				</div>"

            viewstat.InnerHtml = content

        Catch ex As Exception
            'Response.Redirect("s404.aspx")
        End Try

    End Sub






    Function GetPath(str As String)

        Dim path = str.Split("_")
        Dim filename As String = String.Empty

        For i = 2 To path.Length - 1

            filename &= path(i)

        Next
        Return filename
        Return False

    End Function

End Class