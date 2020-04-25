Imports TrainingSchool.SharedRoutines

Public Class WfMakeTest
    Inherits System.Web.UI.Page


    Dim rconn As rconnection
    Dim sqlstring As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SharedRoutines.SetAcl(New List(Of String)(New String() {"8", "2"}))
        rconn = CheckDatabase(rconn)

        If Session("idUser") = 0 OrElse Session("idUser") Is Nothing Then
            Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", " <script>ExitSession();</script>")
        End If
        Try

            Select Case Request.QueryString("op")
                Case "deletesource"
                    deleteObject(Request.QueryString("id"))
                Case "get"
                   ' getFormData(Request.QueryString("idtest"))
                Case "get,savetest"
                    Dim idTest = Request.Form("idtest")
                    SaveAdditionalTest(idTest)
                Case "modtest"
                    Dim idquest = Request.QueryString("idquest")
                    UpdateSequence(Request.QueryString("sequence"), idquest, Request.QueryString("offset"))

            End Select

        Catch ex As Exception

        Finally

        End Try



    End Sub
    Public Sub UpdateSequence(ByVal sequence As Integer, ByVal id As Integer, Optional offset As Integer = 0)

        Try



            Dim newsequence As Integer = sequence + offset


            sqlstring = "select idquest,title_quest,sequence,(select max(sequence) from learning_testquest where idtest=" & Request.QueryString("idTest") & ") as maxsequence from learning_testquest where idquest=" & id & " and sequence in (" & sequence & "," & newsequence & ") "

            Dim dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)



#Disable Warning BC42104 ' La variabile 'conn' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
#Disable Warning BC42104 ' La variabile 'conn' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
            If newsequence > 0 And newsequence <= dt.Rows(0)("maxsequence") Then
#Enable Warning BC42104 ' La variabile 'conn' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
#Enable Warning BC42104 ' La variabile 'conn' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 

                For Each dr In dt.Rows

                    If dr("idquest") = id Then
                        sqlstring = "UPDATE learning_testquest set sequence=" & newsequence & " where idquest=" & dr("idquest")
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    Else
                        sqlstring = "UPDATE learning_testquest set sequence=" & sequence & " where idquest=" & dr("idquest")
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    End If

                Next
            End If


        Catch ex As Exception
            LogWrite( ex.ToString)
        End Try



    End Sub

    Function deleteObject(idquest As String)

        sqlstring = "delete from learning_testquest where idquest=" & idquest
        Try

            rconn.Execute(sqlstring, CommandType.Text, Nothing)
            sqlstring = "delete from learning_testquestanswer where idquest=" & idquest
            rconn.Execute(sqlstring, CommandType.Text, Nothing)

            Response.Write("Domanda eliminata")
        Catch ex As Exception
            LogWrite( ex.ToString)
            Response.Write("Errore nell'inserimento")
        End Try

        Response.End()
        Return False

    End Function
    Function SaveAdditionalTest(ByVal idtest As String)
        Dim question As String = String.Empty


        Dim title As String = String.Empty


        Dim answer As String = String.Empty


        Dim idquest As String = String.Empty


        Dim idanswer As String = String.Empty


        Dim sequence As Integer = 1
        Try


            For Each key In Request.Form.AllKeys
                Try
                    If key.StartsWith("fieldquestion") Then
                        idquest = key.Split("-")(2)
                        question = Request.Form(key)

                        sqlstring = " UPDATE  learning_testquest  set  title_quest = '" & question & "' where idtest" & idquest

                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    End If

                    If key.StartsWith("fieldanswer") Then
                        idanswer = key.Split("-")(2)
                        answer = Request.Form(key)
                        Dim listanswer = answer.Split("|")

                        For j = 0 To listanswer.Length - 1
                            sqlstring = " UPDATE  learning_testquestanswer  set  answer ='" & answer & "' where idanswer=" & idanswer
                            rconn.Execute(sqlstring, CommandType.Text, Nothing)
                        Next
                    End If

                Catch ex As Exception


                End Try
            Next

            Response.Write("Aggiornamento Completato")


        Catch ex As Exception
            LogWrite( ex.ToString)
            Response.Write("Errore nell'inserimento")
        End Try
        Response.End()
        Return False

    End Function

    'Public Sub getFormData(idtest As String)

    '    Dim content As String = String.Empty

    '    Dim question As String = String.Empty

    '    Dim sequence As Integer
    '    Dim answer As String = String.Empty

    '    Dim idquest As Integer
    '    Dim idanswer As Integer
    '    Dim iscorrect As Boolean = False
    '    Dim answerappend As String = String.Empty


    '    Dim id As String = String.Empty

    '    Dim i As Integer = 1



    '    Dim sqlstring As String = "select * from learning_testquest join learning_testquestanswer on learning_testquest.idQuest=learning_testquestanswer.idquest where idtest=" & idtest & " order by learning_testquest.idquest asc"
    '    Dim dt As DataTable = Nothing

    '    dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)







    '    For j = 0 To dt.Rows.Count - 1



    '        Dim dr As DataRow = dt.Rows(j)

    '        id = dr("idtest")
    '        question = dr("title_quest").ToString
    '        answer = dr("answer").ToString
    '        idanswer = dr("idanswer")
    '        sequence = dr("sequence")
    '        iscorrect = dr("is_correct")

    '        If idquest <> dr("idquest") Then
    '            idquest = dr("idquest")
    '            answerappend = ""
    '            content &= vbCrLf & "<div class=""input-group"">  <div class=""tabbable tabs-left"">"
    '            content &= vbCrLf & "<ul class=""nav nav-tabs"" id=""myTab" & i & """>"
    '            content &= vbCrLf & "<li class=""active"">"

    '            content &= vbCrLf & "<a data-toggle=""tab"" href=""#question" & i & """>"
    '            content &= vbCrLf & "    <span style=""color:blue"" ><b>Domanda</b></span> " & i
    '            content &= vbCrLf & "		</a>"
    '            content &= vbCrLf & "		</li>"

    '            content &= vbCrLf & "	<li>"
    '            content &= vbCrLf & "	<a data-toggle=""tab"" href=""#answer" & i & """>"
    '            content &= vbCrLf & "		<i class=""blue ace-icon fa fa-user bigger-110""></i>"
    '            content &= vbCrLf & "Risposte " & i
    '            content &= vbCrLf & "	</a>"
    '            content &= vbCrLf & "</li>"
    '            content &= vbCrLf & "</ul>"


    '            content &= vbCrLf & "<div class=""tab-content"">"
    '            content &= vbCrLf & "	<div id=""question" & i & """ class=""tab-pane in active"">"
    '            content &= vbCrLf & "		<p>" & question & "</p>"
    '            content &= vbCrLf & "	</div>"

    '            If iscorrect Then
    '                answerappend &= vbCrLf & "		<p style=""color:green"" ><b>" & answer & "</b></p>"

    '            Else
    '                answerappend &= vbCrLf & "		<p>" & answer & "</p>"

    '            End If



    '        ElseIf (j + 1 = dt.Rows.Count) Then
    '            If iscorrect Then
    '                answerappend &= vbCrLf & "<p style=""color:green"" ><b>" & answer & "</b></p>"

    '            Else
    '                answerappend &= vbCrLf & "<p>" & answer & "</p>"

    '            End If

    '            content &= vbCrLf & "	<div  id=""answer" & i & """  class=""tab-pane"">"
    '            content &= vbCrLf & answerappend
    '            content &= vbCrLf & "</div>"

    '            content &= vbCrLf & "</div>"
    '            content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
    '            content &= vbCrLf & " <a class=""green updateobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-pencil bigger-130""></i></a><a class=""red deleteobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-trash bigger-130""></i></a>"
    '            content &= vbCrLf & "	</div>"

    '            content &= vbCrLf & "</div><div style=""padding:5px"" class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""ordertest(" & idquest & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""ordertest(" & idquest & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div></div>"
    '            content &= vbCrLf & "<div class=""space-8""></div>"

    '            i = i + 1
    '        ElseIf dt.Rows(j + 1)("idquest") <> idquest Then
    '            If iscorrect Then
    '                answerappend &= vbCrLf & "		<p style=""color:green"" ><b>" & answer & "</b></p>"

    '            Else
    '                answerappend &= vbCrLf & "		<p>" & answer & "</p>"

    '            End If
    '            content &= vbCrLf & "	<div  id=""answer" & i & """  class=""tab-pane"">"
    '            content &= vbCrLf & answerappend
    '            content &= vbCrLf & "</div>"

    '            content &= vbCrLf & "</div>"
    '            content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
    '            content &= vbCrLf & " <a class=""green updateobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-pencil bigger-130""></i></a><a class=""red deleteobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-trash bigger-130""></i></a>"
    '            content &= vbCrLf & "	</div>"

    '            content &= vbCrLf & "</div><div style=""padding:5px"" class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""ordertest(" & idquest & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""ordertest(" & idquest & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div></div>"

    '            content &= vbCrLf & "<div class=""space-8""></div>"

    '            i = i + 1
    '        Else
    '            If iscorrect Then
    '                answerappend &= vbCrLf & "<p style=""color:green"" ><b>" & answer & "</b></p>"

    '            Else
    '                answerappend &= vbCrLf & "<p>" & answer & "</p>"

    '            End If
    '        End If
    '    Next


    '    Test_list.InnerHtml &= content

    '    'If idquest <> dr("idquest") Then
    '    '    idquest = dr("idquest")



    '    '    content = vbCrLf & "	<div class=""ace-spinner"" style=""width:500px""><div class=""input-group"">	<div style=""margin-top:20px !important"" class=""panel panel-default"">"
    '    '    content &= vbCrLf & "  	<div style=""width:500px;height:200px"" class=""panel-heading"">"
    '    '    content &= vbCrLf & "		<a href=""#Test-1-" & idquest & """ data-parent=""#Test_list"" data-toggle=""collapse"" class=""accordion-toggle collapsed"">"
    '    '    content &= vbCrLf & "<i class=""icon-chevron-left pull-right"" data-icon-hide=""icon-chevron-down"" data-icon-show=""icon-chevron-left""></i>"
    '    '    content &= vbCrLf & "            			<i class=""icon-file bigger-130""></i>"
    '    '    content &= vbCrLf & "<span id=""question_" & idquest & """>" & question & "</span>"
    '    '    content &= vbCrLf & "	</a>"
    '    '    content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
    '    '    content &= vbCrLf & " <a class=""green updateobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-pencil bigger-130""></i></a><a class=""red deleteobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-trash bigger-130""></i></a></div>"
    '    '    content &= vbCrLf & "	</div>"
    '    '    content &= vbCrLf & "<div class=""panel-collapse collapse"" id=""Test-1-" & idquest & """ > "


    '    '    content &= vbCrLf & "     		<div class=""panel-body"">"
    '    '    content &= vbCrLf & "<span id=""answer_" & idanswer & """>" & answer & "</span>"
    '    '    content &= vbCrLf & "		</div>"

    '    'ElseIf (j + 1 = dt.Rows.Count) Then
    '    '    content &= vbCrLf & "     		<div class=""panel-body"">"
    '    '    content &= vbCrLf & "<span id=""answer_" & idanswer & """>" & answer & "</span>"
    '    '    content &= vbCrLf & "		</div>"
    '    '    content &= vbCrLf & "	</div>"
    '    '    content &= vbCrLf & "  </div><div class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""ordertest(" & idanswer & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""orderTest(" & idanswer & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div>"
    '    '    content &= vbCrLf & "  </div></div><div class=""space-6""></div>"
    '    '    i = i + 1
    '    '    Test_list.InnerHtml &= content
    '    'ElseIf dt.Rows(j + 1)("idquest") <> idquest Then
    '    '    content &= vbCrLf & "     		<div class=""panel-body"">"
    '    '    content &= vbCrLf & "<span id=""answer_" & idanswer & """>" & answer & "</span>"
    '    '    content &= vbCrLf & "		</div>"
    '    '    content &= vbCrLf & "	</div>"
    '    '    content &= vbCrLf & "  </div><div class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""ordertest(" & idanswer & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""orderTest(" & idanswer & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div>"
    '    '    content &= vbCrLf & "  </div></div><div class=""space-6""></div>"
    '    '    i = i + 1
    '    '    Test_list.InnerHtml &= content
    '    'Else


    '    '    content &= vbCrLf & "     		<div class=""panel-body"">"
    '    '    content &= vbCrLf & "<span id=""answer_" & idanswer & """>" & answer & "</span>"
    '    '    content &= vbCrLf & "		</div>"
    '    'End If




    'End Sub
End Class