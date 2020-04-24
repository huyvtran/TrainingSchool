Imports System.Data
Imports TrainingSchool.SharedRoutines
Imports System.IO


Public Class Poll
    Dim rconn As rconnection
    Dim sqlstring As String = String.Empty

    Dim utility As SharedRoutines


    Sub New()
        rconn = CheckDatabase(rconn)
        utility = New SharedRoutines
    End Sub
    Public Function CreatePoll(title As String, description As String, id_poll As Integer)
        Try
            Dim iduser = HttpContext.Current.Session("iduser")

            If id_poll <> 0 Then
                sqlstring = "update learning_poll set title='" & HttpContext.Current.Session("title") & "' where id_poll=" & id_poll
                rconn.Execute(sqlstring, CommandType.Text, Nothing)
            Else



                sqlstring = " INSERT INTO learning_poll (  author ,  title ,  description ) " &
                         " VALUES ( " & HttpContext.Current.Session("iduser") & ",'" & EscapeMySql(title) & "', '" & EscapeMySql(description) & "')"


                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                Dim res_idanno As Integer
                Dim pathitem As String
                utility.makecategory(res_idanno, pathitem)

                sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  `r_item_id`, `r_type`,path,lev,idparent, iduser) " &
               " VALUES ( '" & EscapeMySql(title) & "','" & EscapeMySql(title) & "',(select max(id_poll) from learning_poll  ), 'poll','" & pathitem & "',3," & res_idanno & "," & iduser & ");"

                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                sqlstring = "select max(r_item_id) as id_poll from learning_kb_res where r_type='poll' "

                Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("id_poll")

            End If
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try



    End Function
    Public Function getFormData(id_poll As String)

        Dim sqlstring As String = "select * from learning_pollquest a left  join  learning_pollquestanswer b on a.id_quest=b.id_quest where id_poll=" & id_poll & " order by a.id_quest asc, answer asc"
        Dim dt As DataTable = Nothing

        dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        Dim content As String = String.Empty

        Dim question As String = String.Empty

        Dim sequence As Integer
        Dim answer As String = String.Empty

        Dim id_quest As Integer
        Dim id_answer As Integer
        Dim iscorrect As Boolean = False
        Dim answerappend As String = String.Empty


        Dim id As String = String.Empty

        Dim i As Integer = 1




        For j = 0 To dt.Rows.Count - 1

            Dim dr As DataRow = dt.Rows(j)

            id = dr("id_poll")
            question = dr("title_quest").ToString
            answer = dr("answer").ToString
            id_answer = IsDBNull(dr("id_answer"))
            sequence = dr("sequence")


            If id_quest <> dr("id_quest") Then
                id_quest = dr("id_quest")
                answerappend = ""
                content &= vbCrLf & "<div class=""input-group"">  <div class=""tabbable tabs-left"">"
                content &= vbCrLf & "<ul class=""nav nav-tabs"" id=""myTab" & i & """>"
                content &= vbCrLf & "<li class=""active"">"

                content &= vbCrLf & "<a data-toggle=""tab"" href=""#question" & i & """>"
                content &= vbCrLf & "    <span style=""color:blue"" ><b>Domanda</b></span> " & i
                content &= vbCrLf & "		</a>"
                content &= vbCrLf & "		</li>"

                content &= vbCrLf & "	<li>"
                content &= vbCrLf & "	<a data-toggle=""tab"" href=""#answer" & i & """>"
                content &= vbCrLf & "		<i class=""blue ace-icon fa fa-user bigger-110""></i>"
                content &= vbCrLf & "Risposte " & i
                content &= vbCrLf & "	</a>"
                content &= vbCrLf & "</li>"
                content &= vbCrLf & "</ul>"


                content &= vbCrLf & "<div class=""tab-content"">"
                content &= vbCrLf & "	<div id=""question" & i & """ class=""tab-pane in active"">"
                content &= vbCrLf & "		<p>" & question & "</p>"
                content &= vbCrLf & "	</div>"

                If answer <> "" Then
                    answerappend &= vbCrLf & " <img src='assets/images/emoji/" & answer & "' >"

                End If



            ElseIf (j + 1 = dt.Rows.Count) Then
                If answer <> "" Then
                    answerappend &= vbCrLf & " <img src='assets/images/emoji/" & answer & "' >"

                End If


                content &= vbCrLf & "	<div  id=""answer" & i & """  class=""tab-pane"">"
                content &= vbCrLf & answerappend
                content &= vbCrLf & "</div>"

                content &= vbCrLf & "</div>"
                content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
                content &= vbCrLf & " <a class=""green updateobject"" data-obj=""" & id_quest & """  href=""#""><i class=""icon-pencil bigger-130""></i></a><a class=""red deleteobject"" data-obj=""" & id_quest & """  href=""#""><i class=""icon-trash bigger-130""></i></a>"
                content &= vbCrLf & "	</div>"

                content &= vbCrLf & "</div><div style=""padding:5px"" class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""orderpoll(" & id_quest & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""orderpoll(" & id_quest & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div></div>"
                content &= vbCrLf & "<div class=""space-8""></div>"

                i = i + 1
            ElseIf dt.Rows(j + 1)("id_quest") <> id_quest Then
                If answer <> "" Then
                    answerappend &= vbCrLf & " <img src='assets/images/emoji/" & answer & "' >"

                End If

                content &= vbCrLf & "	<div  id=""answer" & i & """  class=""tab-pane"">"
                content &= vbCrLf & answerappend
                content &= vbCrLf & "</div>"

                content &= vbCrLf & "</div>"
                content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
                content &= vbCrLf & " <a class=""green updateobject"" data-obj=""" & id_quest & """  href=""#""><i class=""icon-pencil bigger-130""></i></a><a class=""red deleteobject"" data-obj=""" & id_quest & """  href=""#""><i class=""icon-trash bigger-130""></i></a>"
                content &= vbCrLf & "	</div>"

                content &= vbCrLf & "</div><div style=""padding:5px"" class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""orderpoll(" & id_quest & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""orderpoll(" & id_quest & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div></div>"

                content &= vbCrLf & "<div class=""space-8""></div>"

                i = i + 1
            Else
                If answer <> "" Then
                    answerappend &= vbCrLf & " <img src='assets/images/emoji/" & answer & "' >"

                End If

            End If
        Next


        Return content

        'If id_quest <> dr("id_quest") Then
        '    id_quest = dr("id_quest")



        '    content = vbCrLf & "	<div class=""ace-spinner"" style=""width:500px""><div class=""input-group"">	<div style=""margin-top:20px !important"" class=""panel panel-default"">"
        '    content &= vbCrLf & "  	<div style=""width:500px;height:200px"" class=""panel-heading"">"
        '    content &= vbCrLf & "		<a href=""#poll-1-" & id_quest & """ data-parent=""#poll_list"" data-toggle=""collapse"" class=""accordion-toggle collapsed"">"
        '    content &= vbCrLf & "<i class=""icon-chevron-left pull-right"" data-icon-hide=""icon-chevron-down"" data-icon-show=""icon-chevron-left""></i>"
        '    content &= vbCrLf & "            			<i class=""icon-file bigger-130""></i>"
        '    content &= vbCrLf & "<span id=""question_" & id_quest & """>" & question & "</span>"
        '    content &= vbCrLf & "	</a>"
        '    content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
        '    content &= vbCrLf & " <a class=""green updateobject"" data-obj=""" & id_quest & """  href=""#""><i class=""icon-pencil bigger-130""></i></a><a class=""red deleteobject"" data-obj=""" & id_quest & """  href=""#""><i class=""icon-trash bigger-130""></i></a></div>"
        '    content &= vbCrLf & "	</div>"
        '    content &= vbCrLf & "<div class=""panel-collapse collapse"" id=""poll-1-" & id_quest & """ > "


        '    content &= vbCrLf & "     		<div class=""panel-body"">"
        '    content &= vbCrLf & "<span id=""answer_" & id_answer & """>" & answer & "</span>"
        '    content &= vbCrLf & "		</div>"

        'ElseIf (j + 1 = dt.Rows.Count) Then
        '    content &= vbCrLf & "     		<div class=""panel-body"">"
        '    content &= vbCrLf & "<span id=""answer_" & id_answer & """>" & answer & "</span>"
        '    content &= vbCrLf & "		</div>"
        '    content &= vbCrLf & "	</div>"
        '    content &= vbCrLf & "  </div><div class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""orderpoll(" & id_answer & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""orderpoll(" & id_answer & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div>"
        '    content &= vbCrLf & "  </div></div><div class=""space-6""></div>"
        '    i = i + 1
        '    poll_list.InnerHtml &= content
        'ElseIf dt.Rows(j + 1)("id_quest") <> id_quest Then
        '    content &= vbCrLf & "     		<div class=""panel-body"">"
        '    content &= vbCrLf & "<span id=""answer_" & id_answer & """>" & answer & "</span>"
        '    content &= vbCrLf & "		</div>"
        '    content &= vbCrLf & "	</div>"
        '    content &= vbCrLf & "  </div><div class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""orderpoll(" & id_answer & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""orderpoll(" & id_answer & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div>"
        '    content &= vbCrLf & "  </div></div><div class=""space-6""></div>"
        '    i = i + 1
        '    poll_list.InnerHtml &= content
        'Else


        '    content &= vbCrLf & "     		<div class=""panel-body"">"
        '    content &= vbCrLf & "<span id=""answer_" & id_answer & """>" & answer & "</span>"
        '    content &= vbCrLf & "		</div>"
        'End If





    End Function
    Public Function GetPoll()

        Dim jsonResult As String = String.Empty


        Try
            Dim sqlstring As String = "select * from (learning_poll  t1 join learning_pollquest t2 on t1.id_poll=t2.id_poll) join learning_pollquestanswer t3 on t3.id_quest=t2.id_quest where t1.id_poll in (select idresource from  learning_organization  where objecttype='poll' and idOrg=" & HttpContext.Current.Session("reference") & ") order by t2.id_quest desc"

            Dim dt As DataTable = Nothing

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            Dim descr As String = String.Empty



            descr &= dt.Rows(0)("title") & vbCrLf & dt.Rows(0)("description").ToString

            descr = Replace(Replace(descr, vbCrLf, ""), vbLf, "")

            If dt.Rows.Count > 0 Then
                Dim v = 1
                Dim i As Integer = 1
                Dim question As String = String.Empty


                Dim answer As String = String.Empty



                jsonResult = "{""introduction"":""" & Replace(descr, vbCrLf, "") & ""","
                jsonResult += ""
                jsonResult += ""
                jsonResult += """idlog"":""" & HttpContext.Current.Request.QueryString("idtrackpoll").ToString & ""","
                jsonResult += ""
                jsonResult &= """questions"":["


                For j = 0 To dt.Rows.Count



                    If dt.Rows.Count = j Then
                        answer = answer.Remove(answer.Length - 1, 1)
                        answer = """answers"":[" & answer & "]},"



                        jsonResult &= "{""question"":""" & i & "| " & question & ""","
                        jsonResult &= answer
                        answer = ""
                        question = ""
                        Exit For
                    End If
                    Dim strtagsquest = StripTags(dt.Rows(j)("title_quest"))
                    If question <> "" And question <> (dt.Rows(j)("id_quest").ToString & "|" & strtagsquest) Then

                        answer = answer.Remove(answer.Length - 1, 1)
                        answer = """answers"":[" & answer & "]},"



                        jsonResult &= "{""question"":""" & i & "| " & question & ""","
                        jsonResult &= answer
                        answer = ""
                        question = ""

                        v = 0
                        i = i + 1

                    End If
                    Try

                        Dim tmpanswer As String = dt.Rows(j)("answer").ToString

                        'If tmpanswer.IndexOf("Default") > -1 Then
                        tmpanswer = StripTags(tmpanswer) ' Replace(tmpanswer, """", " \""")


                        Dim tmpquestion As String = dt.Rows(j)("title_quest").ToString
                        tmpquestion = StripTags(tmpquestion) 'Replace(tmpquestion, """", """""")

                        question = StripTags(dt.Rows(j)("id_quest").ToString) & "|" & tmpquestion & ""
                        answer &= """" & StripTags(dt.Rows(j)("id_Answer").ToString) & "|" & tmpanswer & ""","
                    Catch ex As Exception
                    End Try


                Next

                jsonResult = jsonResult.Remove(jsonResult.Length - 1, 1)
                jsonResult &= "]}"



                HttpContext.Current.Response.ContentType = "application/json"
                jsonResult = jsonResult.Replace("	", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            End If

        Catch ex As Exception
            LogWrite( ex.ToString)
        End Try
        'jsonResult = Replace(jsonResult, "style""", "style=\""")
        'jsonResult = Replace(jsonResult, """>", """\>")

        jsonResult = StripTags(jsonResult)
        Return jsonResult

    End Function
    Function StripTags(ByVal html As String) As String
        ' Remove HTML tags.
        Return Regex.Replace(html, "<.*?>", "")
    End Function
    Public Sub GetPollquestion(idquest)

        Dim content As String = String.Empty

        Dim checked As String = String.Empty

        Dim idanswer As Integer
        sqlstring = "select * from learning_pollquest a left  join learning_pollquestanswer b on a.id_quest=b.id_quest where a.id_quest=" & idquest

        Dim dt As DataTable = Nothing

        dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        For i = 1 To dt.Rows.Count


            content &= "<label for=""fieldquestion_ " & i & """><span style=""color:blue""> Domanda " & dt.Rows(i - 1)("sequence") & "</span> </label><div class=""""input-group""""><textarea cols=""60"" rows=""10"" type=""text"" name=""mytext[]"" id=""fieldquestion_" & i & """ >" & dt.Rows(0)("title_quest").ToString & "</textarea>"




        Next
        content &= " <input type=""hidden"" id=""idquest"" value=""" & idquest & """ />"
        content &= " <input type=""hidden"" id=""nanswer"" value=""" & dt.Rows.Count & """ />"

        HttpContext.Current.Response.Write(content)
        HttpContext.Current.Response.End()
    End Sub

    Public Function SavePoll()
        Try

            Dim state As String = HttpContext.Current.Request.Form("state")
            Dim idtrack As String = HttpContext.Current.Request.Form("idLog")
            If state = "complete" Then
                sqlstring = "Update learning_polltrack set date_attempt='" & SharedRoutines.ConvertToMysqlDateTime(Now) & "',status='valid' where id_Reference=" & HttpContext.Current.Session("Reference") & " and  id_user=" & HttpContext.Current.Session("iduser")
                rconn.Execute(sqlstring, CommandType.Text, Nothing)
                utility.Update_commonlog(HttpContext.Current.Session("Reference"), HttpContext.Current.Session("iduser"), HttpContext.Current.Session("idCourse"), "completed", idtrack)

            Else
                sqlstring = "Update learning_polltrack set date_attempt='" & SharedRoutines.ConvertToMysqlDateTime(Now) & "',status='not_complete' where id_reference=" & HttpContext.Current.Session("Reference") & " and  id_user=" & HttpContext.Current.Session("idUser")
                rconn.Execute(sqlstring, CommandType.Text, Nothing)
                utility.Update_commonlog(HttpContext.Current.Session("Reference"), HttpContext.Current.Session("iduser"), HttpContext.Current.Session("idCourse"), "incomplete", idtrack)
            End If

        Catch ex As Exception
            Return False
            LogWrite( ex.ToString)
        End Try

        Return True

        Return False

    End Function

    Function InsertPollGift(ByVal filename As String, title As String, ByVal description As String)




        description = EscapeMySql(description.ToString)
        Dim sqlstring As String = "INSERT INTO  poll  ( author ,  title ,  description ) " &
            " VALUES (" & HttpContext.Current.Session("iduser") & ",'" & EscapeMySql(title) & "','" & description & "')"
        rconn.Execute(sqlstring, CommandType.Text, Nothing)

        Dim lastid_quest As Integer
        Dim lastid_poll As Integer = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")


        Try

            Using str As New IO.StreamReader(filename, True)

                While Not str.EndOfStream
                    Dim id_quest As String = String.Empty


                    Dim id_answer As String = String.Empty


                    Dim getString As String = str.ReadLine
                    Dim question As String = String.Empty

                    Dim answer As String = String.Empty

                    If getString.IndexOf("{") > -1 Then
                        question = Replace(getString, "{", "")
                        sqlstring = "INSERT INTO  learning_pollquest  ( id_poll ,  id_Category ,  type_quest ,  title_quest ) " &
                                                        "  VALUES (" & lastid_poll & ",3, 'choice', '" & EscapeMySql(question) & "') "

                        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                        lastid_quest = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")

                    ElseIf getString.IndexOf("~") > -1 Then
                        answer = getString.Replace("~", "")
                        sqlstring = " INSERT INTO  learning_pollquestanswer  ( id_Quest ,  answer , sequence ) " &
                                                                    " VALUES (" & lastid_quest & ", '" & EscapeMySql(answer) & "',1);"
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                        Dim lastid_answer As Integer = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")
                    Else

                    End If


                End While

            End Using


        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
            Return False
        End Try


        Return False

    End Function


    Function UpdateTrack(idtrack As String, Resultpoll As List(Of ListItem))

        Dim first As Boolean = True
        Try
            sqlstring = "  UPDATE learning_polltrack  SET"

            For Each l As ListItem In Resultpoll

                sqlstring &= l.Text = l.Value & ","

            Next
            sqlstring = sqlstring.Remove(sqlstring.Length - 1, 1)
            sqlstring &= " WHERE idTrack = " & idtrack

            rconn.Execute(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
        End Try
        Return False

    End Function


    Function getTrackInfo(idUser, id_poll, idreference) As DataRow
        Try

            sqlstring = "SELECT idTrack, date_attempt, date_end_attempt, last_page_seen, last_page_saved, number_of_save, number_of_attempt, attempts_for_suspension, suspended_until" &
           " FROM learning_polltrack" &
           " WHERE idUser = " & idUser &
           " id_poll = " & id_poll & " And " &
           "idreference = " & idreference

            Dim dr As DataRow = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)

            Return dr

        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
        End Try
        Return Nothing


    End Function
    Function InsertQuestion(id_poll As Integer, id_quest As Integer, question As String, Rawanswer As String, Rawid_answers As String)



        Dim id_answer As String = String.Empty


        Dim sqlstring As String = String.Empty

        Dim lastid_quest As Integer
        Dim lastid_poll As Integer = id_poll
        Dim numberquest As Integer = 0
        Dim sequence As Integer = 1
        Try



            If id_quest <> 0 Then

                sqlstring = "update  learning_pollquest  set  title_quest ='" & EscapeMySql(Replace(question, "?", "\?")) & "' where id_quest=" & id_quest

                rconn.Execute(sqlstring, CommandType.Text, Nothing)


                'Dim listanswer() As String = Rawanswer.Split("|")

                'Dim listid_answers() As String = Rawid_answers.Split("|")

                'For i = 1 To listanswer.Length - 1

                '    If listanswer(i) <> "" Then

                '        sqlstring = " update   learning_pollquestanswer  set  answer ='" & EscapeMySql(listanswer(i)) & "' where id_answer=" & listid_answers(i)
                '        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                '    End If


                'Next


                For i = 1 To 6

                    sqlstring = " INSERT INTO  learning_pollquestanswer  ( id_quest ,  answer ) " &
                  " VALUES (" & id_quest & ",  '" & i & ".png')"

                    rconn.Execute(sqlstring, CommandType.Text, Nothing)

                Next


                HttpContext.Current.Response.Write("Aggiornamento Completato")


            Else

                sqlstring = "Select max(sequence) As maxsequence from learning_pollquest where id_poll=" & id_poll

                Try
                    sequence = CInt(rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("maxsequence").ToString)
                Catch ex As Exception
                    sequence = 0
                End Try

                sqlstring = "INSERT INTO  learning_pollquest  ( id_poll ,   type_quest ,  title_quest ,  sequence ) " &
                                                "  VALUES (" & lastid_poll & ", 'choice', '" & EscapeMySql(question) & "',  " & sequence + 1 & ") "

                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                lastid_quest = utility.GetLastId("id_quest", "learning_pollquest")





                For i = 1 To 6

                    sqlstring = " INSERT INTO  learning_pollquestanswer  ( id_quest ,  answer ) " &
                  " VALUES (" & lastid_quest & ",  '" & i & ".png')"

                    rconn.Execute(sqlstring, CommandType.Text, Nothing)

                Next

                'Dim listanswer() As String = Rawanswer.Split("|")





                'For i = 1 To listanswer.Length - 1

                '    If listanswer(i) <> "" Then

                '        sqlstring = " INSERT INTO  learning_pollquestanswer  ( id_quest ,  answer ) " &
                '                                                   " VALUES (" & lastid_quest & ",   '" & EscapeMySql(listanswer(i)) & "');"

                '        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                '    End If


                'Next


                HttpContext.Current.Response.Write("Inserimento Completato")

            End If

        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
            HttpContext.Current.Response.Write(ex.Message)
        End Try


        HttpContext.Current.Response.End()

        Return False

    End Function


    Function Registerpoll(iduser As String, id_poll As String, idreference As String)

        Dim newinfo As List(Of ListItem)
        Dim save_score As Integer
        Dim sstatus As String = String.Empty

        Dim loginfo As DataRow
        Try
            loginfo = getlogInfo(iduser, id_poll, idreference)
            newinfo.Add(New ListItem("date_end_attempt", ConvertToMysqlDateTime(Now)))
#Disable Warning BC42104 ' La variabile 'newinfo' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
#Disable Warning BC42104 ' La variabile 'newinfo' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
            newinfo.Add(New ListItem("number_of_save", loginfo("number_of_save").ToString + 1))
#Enable Warning BC42104 ' La variabile 'newinfo' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
#Enable Warning BC42104 ' La variabile 'newinfo' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
            newinfo.Add(New ListItem("score", save_score))
            newinfo.Add(New ListItem("sstatus", sstatus))
            newinfo.Add(New ListItem("number_of_attempt", loginfo("number_of_attempt") + 1))


            Dim r As Integer = Updatelog(loginfo("idtrack"), newinfo)

        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
        End Try

        Return False

    End Function

    Function UpdateQuestion(id_poll As Integer, id_quest As Integer, question As String, Rawanswer As String, Rawid_answer As String)



        Dim title As String = String.Empty


        Dim answer As String = String.Empty


        Dim sqlstring As String = String.Empty

        Dim sequence As Integer = 1

        Try



            Try

                question = EscapeMySql(question)

                If id_quest <> 0 Then
                    sqlstring = " update learning_pollquest  set title_quest='" & question & "'  where id_quest=" & id_quest
                    rconn.Execute(sqlstring, CommandType.Text, Nothing)


                    Dim listanswer() As String = Rawanswer.Split("|")

                    Dim listid_answer() As String = Rawid_answer.Split("|")
                    For i = 1 To listanswer.Length - 1

                        If listanswer(i) <> "" Then

                            sqlstring = " update  learning_pollquestanswer  set  answer ='" & EscapeMySql(listanswer(i)) & "' where id_answer=" & listid_answer(i)

                            rconn.Execute(sqlstring, CommandType.Text, Nothing)

                        End If


                    Next


                End If




            Catch ex As Exception


            End Try

            HttpContext.Current.Response.Write("Aggiornamento Completato")


        Catch ex As Exception
            LogWrite( ex.ToString)
            HttpContext.Current.Response.Write(ex.Message)
        End Try
        HttpContext.Current.Response.End()
        Return False

    End Function
    Function Updatelog(idtrack As String, Resultpoll As List(Of ListItem))

        Dim first As Boolean = True
        Try
            sqlstring = "  UPDATE learning_polltrack  SET"

            For Each l As ListItem In Resultpoll

                sqlstring &= l.Text = l.Value & ","

            Next
            sqlstring = sqlstring.Remove(sqlstring.Length - 1, 1)
            sqlstring &= " WHERE idtrack = " & idtrack

            rconn.Execute(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
        End Try
        Return False

    End Function
    Function getlogInfo(idUser, id_poll, idreference) As DataRow
        Try

            sqlstring = "SELECT idtrack, date_attempt, date_end_attempt, last_page_seen, last_page_saved, number_of_save, number_of_attempt, attempts_for_suspension, suspended_until" &
           " FROM learning_polltrack" &
           " WHERE idUser = " & idUser &
           " id_poll = " & id_poll & " And " &
           "idreference = " & idreference

            Dim dr As DataRow = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)

            Return dr

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return Nothing
        ' Return False

    End Function



End Class