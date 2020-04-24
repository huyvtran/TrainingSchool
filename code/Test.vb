Imports System.Data
Imports TrainingSchool.SharedRoutines
Imports System.IO

Public Class TestObject
    Dim rconn As rconnection
    Dim sqlstring As String =String.Empty 

    Dim utility As SharedRoutines
    Dim msg As String =String.Empty

    Sub New()
        utility = New SharedRoutines
        rconn = CheckDatabase(rconn)
    End Sub

    Function updatenumberquest(idtest)
        Try
            sqlstring = "select count(*) as num from learning_Testquest where idtest=" & idtest
            Dim num As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("num")

            sqlstring = "update learning_test set score_max=" & num * 10 & " where idtest=" & idtest
            rconn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Function

    Function InsertTestGift(filename As String, title As String, description As String, soglia As Integer)





        sqlstring = "INSERT INTO  learning_test (   title ,  description ,  point_type ,  point_required ,  display_type ,  order_type ,  shuffle_answer ,  question_random_number ,  save_keep ,  mod_doanswer ,  can_travel ,  show_only_status ,  show_score ,  show_score_cat ,  show_doanswer ,  show_solution ,  time_dependent ,  time_assigned ,  penality_test ,  penality_time_test ,  penality_quest ,  penality_time_quest ,  max_attempt ,  hide_info ,  order_info ,  use_suspension ,  suspension_num_attempts ,  suspension_num_hours ,  suspension_prerequisites ,  chart_options ,  mandatory_answer ) " &
            " VALUES ('" & EscapeMySql(title) & "','" & EscapeMySql(description) & "', 0, " & soglia & ", 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, '', 0, 0, 0, 0, '', 0);"

        rconn.Execute(sqlstring, CommandType.Text, Nothing)

        Dim lastidquest As Integer
        Dim lastidtest As Integer = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")
        Dim numberquest As Integer = 0

        Try

            Using str As New IO.StreamReader(filename, True)

                While Not str.EndOfStream
                    Dim idquest As String = String.Empty


                    Dim idanswer As String = String.Empty


                    Dim getString As String = str.ReadLine
                    Dim question As String = String.Empty

                    Dim answer As String = String.Empty

                    If getString.IndexOf("{") > -1 Then
                        question = Replace(getString, "{", "")
                        sqlstring = "INSERT INTO  learning_testquest  ( idTest ,  idCategory ,  type_quest ,  title_quest ,  difficult ,  time_assigned ,  sequence ,  page ,  shuffle ) " &
                                                        "  VALUES (" & lastidtest & ",3, 'choice', '" & EscapeMySql(question) & "', 3, 0, 1, 1, 0) "

                        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                        lastidquest = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")
                        numberquest = numberquest + 1
                    ElseIf getString.IndexOf("~") > -1 Then
                        answer = getString.Replace("~", "")
                        sqlstring = " INSERT INTO  learning_testquestanswer  ( idQuest ,  sequence ,  is_correct ,  answer ,  scorrect ) " &
                                                                    " VALUES (" & lastidquest & ", 0, 0, '" & EscapeMySql(answer) & "',  0);"
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                        Dim lastidanswer As Integer = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")
                    ElseIf getString.IndexOf("=%") > -1 Then
                        answer = getString.Replace("=%1000%", "")
                        sqlstring = " INSERT INTO  learning_testquestanswer  (  idQuest ,  sequence ,  is_correct ,  answer ,   scorrect ) " &
                                                                  " VALUES (" & lastidquest & ", 0, 1, '" & EscapeMySql(answer) & "',  10);"


                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    Else


                    End If


                End While

            End Using

            sqlstring = "update  learning_test set   score_max =" & numberquest * 10 & " where idtest=" & lastidtest
            rconn.Execute(sqlstring, CommandType.Text, Nothing)



        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
            Return False
        End Try


        Return Nothing

    End Function

    Function addquestionGift(filename As String, lastidtest As Integer)




        Dim lastidquest As Integer

        Dim numberquest As Integer = 0

        Try

            Using str As New IO.StreamReader(filename, True)

                While Not str.EndOfStream
                    Dim idquest As String = String.Empty


                    Dim idanswer As String = String.Empty


                    Dim getString As String = str.ReadLine
                    Dim question As String = String.Empty

                    Dim answer As String = String.Empty

                    If getString.IndexOf("{") > -1 Then
                        question = Replace(getString, "{", "")
                        sqlstring = "INSERT INTO  learning_testquest  ( idTest ,  idCategory ,  type_quest ,  title_quest ,  difficult ,  time_assigned ,  sequence ,  page ,  shuffle ) " &
                                                        "  VALUES (" & lastidtest & ",3, 'choice', '" & EscapeMySql(question) & "', 3, 0, 1, 1, 0) "

                        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                        lastidquest = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")
                        numberquest = numberquest + 1
                    ElseIf getString.IndexOf("~") > -1 Then
                        answer = getString.Replace("~", "")
                        sqlstring = " INSERT INTO  learning_testquestanswer  ( idQuest ,  sequence ,  is_correct ,  answer ,  score_correct ) " &
                                                                    " VALUES (" & lastidquest & ", 0, 0, '" & EscapeMySql(answer) & "',  0);"
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                        Dim lastidanswer As Integer = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")
                    ElseIf getString.IndexOf("=%") > -1 Then
                        answer = getString.Replace("=%1000%", "")
                        sqlstring = " INSERT INTO  learning_testquestanswer  (  idQuest ,  sequence ,  is_correct ,  answer ,   score_correct ) " &
                                                                  " VALUES (" & lastidquest & ", 0, 1, '" & EscapeMySql(answer) & "',  10);"


                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    Else


                    End If


                End While

            End Using

            sqlstring = "update  learning_test set   score_max =" & numberquest * 10 & " where idtest=" & lastidtest
            rconn.Execute(sqlstring, CommandType.Text, Nothing)



        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
            Return False
        End Try


        Return True

    End Function

    Function UpdateTrack(idtrack As String, idtest As Integer, idreference As Integer, dateattempt As String, score As Integer, scorestatus As String, datebegin As String, dateend As String, time As String)

        Dim first As Boolean = True
        Try

            sqlstring = "select Count(*) as numbertime from  learning_testtrack_times where idtrack=" & idtrack & "  and idtest=" & idtest

            Dim dt As DataTable = Nothing

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            Dim numbertime As Integer = dt.Rows(0)("numbertime") + 1

            sqlstring = "INSERT INTO `learning_testtrack_times` (`idTrack`, `idReference`, `idTest`, `date_attempt`, `number_time`, `score`, `score_status`, `date_begin`, `date_end`, `time`) " &
            "VALUES (" & idtrack & ", " & idreference & ", " & idtest & ", '" & dateattempt & "'," & numbertime & ", " & score & " ,'" & scorestatus & "', '" & datebegin & "', '" & dateend & "', '" & time & "');"


            rconn.Execute(sqlstring, CommandType.Text, Nothing)




        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function


    Function getTrackInfo(idUser, idTest, idreference) As DataRow
        Try

            sqlstring = "Select idTrack, date_attempt, date_end_attempt, last_page_seen, last_page_saved, number_of_save, number_of_attempt, attempts_for_suspension, suspended_until" &
           " FROM  learning_testtrack " &
           " WHERE idUser = " & idUser &
           " idTest = " & idTest & " And " &
           "idreference = " & idreference

            Dim dr As DataRow = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)

            Return dr

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return Nothing


    End Function
    Function InsertQuestion(idtest As Integer, idquest As Integer, question As String, Rawanswer As String, Rawchecks As String, Rawidanswers As String)



        Dim idanswer As String = String.Empty


        Dim sqlstring As String = String.Empty

        Dim lastidquest As Integer
        Dim lastidtest As Integer = idtest
        Dim numberquest As Integer = 0
        Dim sequence As Integer = 1
        Try



            If idquest <> 0 Then

                sqlstring = "update  learning_testquest  Set  title_quest ='" & question & "' where idquest=" & idquest

                rconn.Execute(sqlstring, CommandType.Text, Nothing)


                Dim listanswer() As String = Rawanswer.Split("|")
                Dim listcheck() As String = Rawchecks.Split("|")
                Dim listidanswers() As String = Rawidanswers.Split("|")

                For i = 1 To listanswer.Length - 1

                    If listanswer(i) <> "" Then

                        sqlstring = " update   learning_testquestanswer  set  is_correct =" & listcheck(i) & ", answer ='" & EscapeMySql(listanswer(i)) & "' where idanswer=" & listidanswers(i)
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                    Else
                        sqlstring = " delete  from  learning_testquestanswer   where idanswer=" & listidanswers(i)
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)


                    End If



                Next





            Else

                sqlstring = "select max(sequence) as maxsequence from learning_testquest where idtest=" & idtest

                Try
                    sequence = CInt(rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("maxsequence").ToString)
                Catch ex As Exception
                    sequence = 1
                End Try

                sqlstring = "INSERT INTO  learning_testquest  ( idTest ,  idCategory ,  type_quest ,  title_quest ,  difficult ,  time_assigned ,  sequence ,  page ,  shuffle ) " &
                                                "  VALUES (" & lastidtest & ",3, 'choice', '" & EscapeMySql(question) & "', 3, 0, " & sequence + 1 & ", 1, 0) "

                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                lastidquest = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")

                Dim listanswer() As String = Rawanswer.Split("|")
                Dim listcheck() As String = Rawchecks.Split("|")

                For i = 1 To listanswer.Length - 1

                    If listanswer(i) <> "" Then

                        sqlstring = " INSERT INTO  learning_testquestanswer  ( idQuest ,  is_correct ,  answer ,   score_correct ) " &
                                                                   " VALUES (" & lastidquest & ", " & listcheck(i) & ",  '" & EscapeMySql(listanswer(i)) & "',  10);"

                        rconn.Execute(sqlstring, CommandType.Text, Nothing)

                    End If


                Next


                msg = "Inserimento Completato"

            End If
            updatenumberquest(idtest)




        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
            msg = ex.Message
        End Try




    End Function
    Public Function getFormData(idtest As String)

        Dim content As String = String.Empty

        Dim question As String = String.Empty

        Dim sequence As Integer
        Dim answer As String = String.Empty

        Dim idquest As Integer
        Dim idanswer As Integer
        Dim iscorrect As Boolean = False
        Dim answerappend As String = String.Empty


        Dim id As String = String.Empty

        Dim i As Integer = 1



        Dim sqlstring As String = "select * from learning_testquest join learning_testquestanswer on learning_testquest.idQuest=learning_testquestanswer.idquest where idtest=" & idtest & " order by learning_testquest.idquest asc"
        Dim dt As DataTable = Nothing

        dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)







        For j = 0 To dt.Rows.Count - 1



            Dim dr As DataRow = dt.Rows(j)

            id = dr("idtest")
            question = dr("title_quest").ToString
            answer = dr("answer").ToString
            idanswer = dr("idanswer")
            sequence = dr("sequence")
            iscorrect = dr("is_correct")

            If idquest <> dr("idquest") Then
                idquest = dr("idquest")
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

                If iscorrect Then
                    answerappend &= vbCrLf & "		<p style=""color:green"" ><b>" & answer & "</b></p>"

                Else
                    answerappend &= vbCrLf & "		<p>" & answer & "</p>"

                End If



            ElseIf (j + 1 = dt.Rows.Count) Then
                If iscorrect Then
                    answerappend &= vbCrLf & "<p style=""color:green"" ><b>" & answer & "</b></p>"

                Else
                    answerappend &= vbCrLf & "<p>" & answer & "</p>"

                End If

                content &= vbCrLf & "	<div  id=""answer" & i & """  class=""tab-pane"">"
                content &= vbCrLf & answerappend
                content &= vbCrLf & "</div>"

                content &= vbCrLf & "</div>"
                content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
                content &= vbCrLf & " <a class=""green updateobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-pencil bigger-130""></i></a><a class=""red deleteobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-trash bigger-130""></i></a>"
                content &= vbCrLf & "	</div>"

                content &= vbCrLf & "</div><div style=""padding:5px"" class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""ordertest(" & idquest & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""ordertest(" & idquest & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div></div>"
                content &= vbCrLf & "<div class=""space-8""></div>"

                i = i + 1
            ElseIf dt.Rows(j + 1)("idquest") <> idquest Then
                If iscorrect Then
                    answerappend &= vbCrLf & "		<p style=""color:green"" ><b>" & answer & "</b></p>"

                Else
                    answerappend &= vbCrLf & "		<p>" & answer & "</p>"

                End If
                content &= vbCrLf & "	<div  id=""answer" & i & """  class=""tab-pane"">"
                content &= vbCrLf & answerappend
                content &= vbCrLf & "</div>"

                content &= vbCrLf & "</div>"
                content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
                content &= vbCrLf & " <a class=""green updateobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-pencil bigger-130""></i></a><a class=""red deleteobject"" data-obj=""" & idquest & """  href=""#""><i class=""icon-trash bigger-130""></i></a>"
                content &= vbCrLf & "	</div>"

                content &= vbCrLf & "</div><div style=""padding:5px"" class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""ordertest(" & idquest & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""ordertest(" & idquest & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div></div>"

                content &= vbCrLf & "<div class=""space-8""></div>"

                i = i + 1
            Else
                If iscorrect Then
                    answerappend &= vbCrLf & "<p style=""color:green"" ><b>" & answer & "</b></p>"

                Else
                    answerappend &= vbCrLf & "<p>" & answer & "</p>"

                End If
            End If
        Next


        Return content





    End Function
    Function AddSequence(idtest As Integer)
        Try


            sqlstring = "select * from learning_testquest where idtest=" & idtest & " order by idquest asc"
            Dim dt As DataTable = Nothing

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            Dim sequence As Integer = 1
            For Each dr In dt.Rows


                sqlstring = "update  learning_testquest  set  sequence ='" & sequence & "' where idquest=" & dr("idquest")
                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                sequence = sequence + 1

            Next

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
            msg = ex.Message
        End Try
        Return msg
        Return False

    End Function


    Public Function GetQuestion(idquest)

        Dim content As String = String.Empty

        Dim checked As String = String.Empty

        Dim idanswer As Integer

        Try
            sqlstring = "select a.idquest,idanswer,title_quest,answer,is_correct, a.sequence from learning_testquest a join learning_testquestanswer b on a.idquest=b.idquest where a.idquest=" & idquest & " order by idquest asc"

            Dim dt As DataTable = Nothing

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            For i = 1 To dt.Rows.Count

                idanswer = dt.Rows(i - 1)("idanswer")

                If dt.Rows(i - 1)("is_correct") = 1 Then
                    checked = "checked"
                Else
                    checked = ""
                End If


                If i = 1 Then
                    content &= "<label for=""fieldquestion_ " & i & """><span style=""color:blue""> Domanda " & CInt(dt.Rows(0)("sequence") - 1) & "</span> </label><div class=""""input-group""""><textarea cols=""60"" rows=""2"" type=""text"" name=""mytext[]"" id=""fieldquestion_" & i & """ >" & dt.Rows(0)("title_quest").ToString & "</textarea>"

                    content &= " <br>Risposta " & i & "  <input name=""mycheck""   id=""fieldcheck_" & i & """ type=""radio"" " & checked & "> Esatta<br><textarea  name=""" & idanswer & """ id=""fieldanswer_" & i & """  cols=""60"" rows=""2"" >" & dt.Rows(i - 1)("answer").ToString & "</textarea> "

                Else
                    content &= " <br>Risposta " & i & "  <input name=""mycheck""  id=""fieldcheck_" & i & """ type=""radio""  " & checked & "> Esatta <br><textarea name=""" & idanswer & """ id=""fieldanswer_" & i & """  cols=""60"" rows=""2"" >" & dt.Rows(i - 1)("answer").ToString & "</textarea>"

                End If

            Next
            content &= " <input type=""hidden"" id=""idquest"" value=""" & idquest & """ />"
            content &= " <input type=""hidden"" id=""nanswer"" value=""" & dt.Rows.Count & """ />"

            msg = content

            Return msg

        Catch ex As Exception
            msg = ex.Message
        End Try

        Return False

    End Function

    Function GetTotalQuestion(idtest As Integer)
        Dim sqlstring As String = "select count(*) as nquest from learning_testquest where idtest=" & idtest

        Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("nquest")


        Return False

    End Function
    Public Function GetTest()


        Dim dt As DataTable = Nothing


        Dim sqlstring As String = String.Empty

        Dim randomrow As String = ""
        Dim randomquest As Integer = 0
        Dim strid As String = ""
        Dim stridquest As String = ""
        sqlstring = "select question_random_number from learning_test where idtest in (select idResource from  learning_organization  where objecttype='test' and  idOrg=" & HttpContext.Current.Session("reference") & ") "

        dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        If dt.Rows(0)("question_random_number") > 0 Then
            randomquest = dt.Rows(0)("question_random_number")
            randomrow = " order by rand() limit " & dt.Rows(0)("question_random_number") & ""


            sqlstring = "select idquest from learning_testquest  a join learning_test b on a.idTest=b.idTest where a.idtest in (select idResource from  learning_organization  where objecttype='test' and  idOrg=" & HttpContext.Current.Session("reference") & ")  " & randomrow

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)



            For Each dr In dt.Rows
                strid &= dr("idquest").ToString & ","
            Next
            strid = strid.Remove(strid.Length - 1, 1)

            stridquest = " and learning_testquest.idquest in (" & strid & ") "
        End If


        Dim jsonResult As String = String.Empty



        Try
            Dim iscorrect As Integer

            sqlstring = "select * from (learning_testquest join learning_test on learning_test.idtest=learning_testquest.idTest) join learning_testquestanswer on learning_testquestanswer.idquest=learning_testquest.idQuest where learning_test.idtest in (select idResource from  learning_organization  where objecttype='test' and  idOrg=" & HttpContext.Current.Session("reference") & ") " & stridquest & "   order by learning_testquest.idQuest asc "

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            ' "idTest"	"author"	"title"	"description"	"point_type"	"point_required"	"display_type"	"order_type"	"shuffle_answer"	"question_random_number"	"save_keep"	"mod_doanswer"	"can_travel"	"show_only_status"	"show_score"	"show_scat"	"show_doanswer"	"show_solution"	"time_dependent"	"time_assigned"	"penality_test"	"penality_time_test"	"penality_quest"	"penality_time_quest"	"max_attempt"	"hide_info"	"order_info"	"use_suspension"	"suspension_num_attempts"	"suspension_num_hours"	"suspension_prerequisites"	"chart_options"	"mandatory_answer"	"smax"
            Dim total As String = String.Empty


            If randomquest > 0 Then
                total = randomquest
            Else
                total = GetTotalQuestion(CInt(dt.Rows(0)("idtest").ToString))
            End If


            If dt.Rows.Count > 0 Then
                Dim descr As String = String.Empty

                descr = dt.Rows(0)("title") & " <br><br>"
                descr &= "<p style='font-size:18px'>Caratteristiche del test :" & " <br>"
                descr &= "<ul><li>Il punteggio massimo conseguibile è di  : <b>" & (total * 10) & "</b> </li>"
                descr &= "<li>Il test è composto da <b> " & total & "</b> domande" & " </li>"
                descr &= "<li>Il punteggio minimo per passare il test è : <b>" & CInt((CInt(dt.Rows(0)("point_required").ToString) * (total * 10)) / 100) & " </b> </li>"
                descr &= "<li>non è possibile salvare il test per riprenderlo successivamente " & " </li>"
                descr &= "<li>è possibile modificare una risposta dopo averla data " & " </li>"
                descr &= "<li>è possibile scorrere le diverse pagine del test </li>"
                descr &= "<li>verrà visualizzato il punteggio finale ottenuto </li>"
                descr &= "<li>verranno visualizzate le soluzioni </li>"

                If dt.Rows(0)("time_assigned").ToString <> 0 Then
                    descr &= "il tempo assegnato per lo svolgimento di tutto il test è : " & dt.Rows(0)("time_assigned").ToString & " minuti" & "</p> <br><br>"
                End If

                descr &= " <br><br>"

                Dim i As Integer = 1
                jsonResult = "{""introduction"":""" & descr & ""","
                jsonResult += ""
                jsonResult += """soglia"":""" & dt.Rows(0)("point_required").ToString & ""","
                jsonResult += ""
                jsonResult += """idtest"":""" & dt.Rows(0)("idtest").ToString & ""","
                jsonResult += ""
                jsonResult += """idlog"":""" & HttpContext.Current.Request.QueryString("idtracktest").ToString & ""","
                jsonResult += ""
                jsonResult &= """questions"":["
                Dim v = 0
                Dim question As String = String.Empty


                Dim idquestion As Integer
                Dim answer As String = String.Empty





                For j = 0 To dt.Rows.Count


                    If dt.Rows.Count = j Then
                        answer = answer.Remove(answer.Length - 1, 1)
                        answer = """answers"":[" & answer & "],""correct"":" & iscorrect & "},"


                        jsonResult &= "{""question"":""" & i & ". " & question & ""","
                        jsonResult &= answer
                        answer = ""
                        question = ""
                        Exit For
                    End If


                    If idquestion <> 0 And idquestion <> (dt.Rows(j)("idquest").ToString) Then

                        answer = answer.Remove(answer.Length - 1, 1)
                        answer = """answers"":[" & answer & "],""correct"":" & iscorrect & "},"


                        jsonResult &= "{""question"":""" & i & ". " & question & ""","
                        jsonResult &= answer
                        answer = ""
                        question = ""

                        v = 0
                        i = i + 1

                    End If

                    Dim tmpanswer As String = dt.Rows(j)("answer").ToString

                    'If tmpanswer.IndexOf("Default") > -1 Then
                    tmpanswer = Replace(tmpanswer, """", " \""")


                    Dim tmpquestion As String = dt.Rows(j)("title_quest").ToString
                    tmpquestion = Replace(tmpquestion, """", "\""")
                    idquestion = dt.Rows(j)("idquest").ToString
                    question = idquestion & "|" & tmpquestion
                    answer &= """" & dt.Rows(j)("idAnswer").ToString & "|" & tmpanswer & ""","

                    If CBool(dt.Rows(j)("is_correct").ToString) Then
                        iscorrect = v
                    End If

                    v = v + 1



                Next

                jsonResult = jsonResult.Remove(jsonResult.Length - 1, 1)
                jsonResult &= "]}"



                HttpContext.Current.Response.ContentType = "application/json"
                jsonResult = jsonResult.Replace("	", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            End If

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

        msg = jsonResult
        Return msg

        Return False

    End Function

    Public Function SaveTest()


        Try

            Dim state As String = HttpContext.Current.Request.Form("state")
            Dim correct As String = HttpContext.Current.Request.Form("correct") * 10
            Dim total As String = HttpContext.Current.Request.Form("CurrentQuestionNumber")
            Dim soglia As String = HttpContext.Current.Request.Form("soglia")
            Dim idtrack As Integer = HttpContext.Current.Request.Form("idLog")
            Dim idtest As Integer = HttpContext.Current.Request.Form("idtest")
            Dim sqlstring As String = String.Empty

            Dim point_required As Integer = CInt(CInt(HttpContext.Current.Request.Form("soglia") * CInt(HttpContext.Current.Request.Form("num") * 10) / 100))
            Dim status As String = String.Empty

            Dim dt As DataTable = Nothing



            If correct >= point_required Then
                sqlstring = "Update learning_testtrack set date_end_attempt='" & utility.ConvertToMysqlDateTime(Now) & "', number_of_attempt= number_of_attempt+1,score=" & correct & ",score_status='valid' where idreference=" & HttpContext.Current.Session("Reference") & " and  iduser=" & HttpContext.Current.Session("iduser")


                rconn.Execute(sqlstring, CommandType.Text, Nothing)


                status = "passed"
            Else

                sqlstring = "select   number_of_attempt  > max_attempt   as result from learning_test a join learning_testtrack b on a.idTest=b.idTest where max_attempt !=0 and  iduser=" & HttpContext.Current.Session("iduser") & "  and idreference=" & HttpContext.Current.Session("Reference") & ""
                dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                If dt.Rows.Count > 0 Then

                    If dt.Rows(0)("result") Then

                        sqlstring = "Update learning_testtrack set date_end_attempt='" & utility.ConvertToMysqlDateTime(Now) & "', score=" & correct & ",score_status='not_complete' where idreference=" & HttpContext.Current.Session("Reference") & " and  iduser=" & HttpContext.Current.Session("idUser")


                        rconn.Execute(sqlstring, CommandType.Text, Nothing)



                        status = "failed"
                    Else

                        sqlstring = "Update learning_testtrack set date_end_attempt='" & utility.ConvertToMysqlDateTime(Now) & "', number_of_attempt= number_of_attempt+1,score=" & correct & ",score_status='not_complete' where idreference=" & HttpContext.Current.Session("Reference") & " and  iduser=" & HttpContext.Current.Session("idUser")


                        rconn.Execute(sqlstring, CommandType.Text, Nothing)


                        status = "incompelte"
                    End If
                Else
                    sqlstring = "Update learning_testtrack set date_end_attempt='" & utility.ConvertToMysqlDateTime(Now) & "', number_of_attempt= number_of_attempt+1,score=" & correct & ",score_status='not_complete' where idreference=" & HttpContext.Current.Session("Reference") & " and  iduser=" & HttpContext.Current.Session("idUser")


                    rconn.Execute(sqlstring, CommandType.Text, Nothing)


                    status = "incomplete"
                End If
            End If

            utility.Update_commonlog(HttpContext.Current.Session("Reference"), HttpContext.Current.Session("iduser"), HttpContext.Current.Session("idCourse"), status, idtrack)

            UpdateTrack(idtrack, idtest, HttpContext.Current.Session("Reference"), utility.ConvertToMysqlDateTime(Now), correct, status, utility.ConvertToMysqlDateTime(Now), utility.ConvertToMysqlDateTime(Now), "")

        Catch ex As Exception
            Return False




            LogWrite(ex.ToString)




        End Try


        Return True


        Return False


    End Function




    Sub updatetest(title As String, soglia As Integer, tentativi As String, random As String, idtest As Integer)


        Try


            Dim sqlstring As String = "update  learning_test set  title='" & EscapeMySql(title) & "',    point_required =" & soglia & ",   question_random_number =" & random & ",   max_attempt =" & tentativi & " where idtest=" & idtest


            rconn.Execute(sqlstring, CommandType.Text, Nothing)
            msg = idtest

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
            msg = ex.Message
        End Try




    End Sub


    Sub InsertTest(title As String, soglia As Integer, tentativi As String, random As String, ByRef idtest As String)


        Try
            Dim iduser = HttpContext.Current.Session("iduser")

            Dim sqlstring As String = "INSERT INTO  learning_test (   title ,   point_type ,  point_required ,  display_type ,  order_type ,  shuffle_answer ,  question_random_number ,  save_keep ,  mod_doanswer ,  can_travel ,  show_only_status ,  show_score ,  show_score_cat ,  show_doanswer ,  show_solution ,  time_dependent ,  time_assigned ,  penality_test ,  penality_time_test ,  penality_quest ,  penality_time_quest ,  max_attempt ,  hide_info ,  order_info ,  use_suspension ,  suspension_num_attempts ,  suspension_num_hours ,  suspension_prerequisites ,  chart_options ,  mandatory_answer ,  score_max ) " &
                                                     " VALUES ('" & EscapeMySql(title) & "',0, " & soglia & ", 1, 1, 1, " & random & ", 0, 1, 1, 0, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, " & tentativi & ", 0, '', 0, 0, 0, 0, '', 0, 0);"

            rconn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "select max(idtest) as idtestnew from learning_Test "
            idtest = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("idtestnew")

            Dim res_idanno As Integer
            Dim pathitem As String
            utility.makecategory(res_idanno, pathitem)

            sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  `r_item_id`, `r_type`,path,lev,idparent, iduser) " &
               " VALUES ( '" & EscapeMySql(title) & "', '" & EscapeMySql(title) & "'," & idtest & ", 'test','" & pathitem & "',3," & res_idanno & "," & iduser & ");"



            rconn.Execute(sqlstring, CommandType.Text, Nothing)





        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
            msg = ex.Message
        End Try




    End Sub

    Function RegisterTest(iduser As String, idtest As String, idreference As String)

        Dim newinfo As List(Of ListItem)
        Dim save_score As Integer
        Dim sstatus As String = String.Empty

        Dim loginfo As DataRow
        Try
            loginfo = getlogInfo(iduser, idtest, idreference)
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
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    Function UpdateQuestion(idtest As Integer, idquest As Integer, question As String, Rawanswer As String, Rawchecks As String, Rawidanswer As String)



        Dim title As String = String.Empty


        Dim answer As String = String.Empty


        Dim sqlstring As String = String.Empty

        Dim sequence As Integer = 1

        Try



            Try

                question = EscapeMySql(question)

                If idquest <> 0 Then
                    sqlstring = " update learning_testquest  Set title_quest='" & question & "'  where idquest=" & idquest
                    rconn.Execute(sqlstring, CommandType.Text, Nothing)


                    Dim listanswer() As String = Rawanswer.Split("|")
                    Dim listcheck() As String = Rawchecks.Split("|")
                    Dim listidanswer() As String = Rawidanswer.Split("|")
                    For i = 1 To listanswer.Length - 1

                        If listanswer(i) <> "" Then

                            sqlstring = " update  learning_testquestanswer  set  is_correct =" & listcheck(i) & ",  answer ='" & EscapeMySql(listanswer(i)) & "' where idAnswer=" & listidanswer(i)

                            rconn.Execute(sqlstring, CommandType.Text, Nothing)

                        Else

                            sqlstring = " delete  learning_testquestanswer   where idAnswer=" & listidanswer(i)

                            rconn.Execute(sqlstring, CommandType.Text, Nothing)

                        End If



                    Next


                End If




            Catch ex As Exception


            End Try

            msg = "Aggiornamento Completato"


        Catch ex As Exception
            LogWrite(ex.ToString)
            msg = ex.Message
        End Try
        Return msg
        Return False

    End Function
    Function Updatelog(idtrack As String, Resulttest As List(Of ListItem))

        Dim first As Boolean = True
        Try
            sqlstring = "  UPDATE learning_testtrack  SET"

            For Each l As ListItem In Resulttest

                sqlstring &= l.Text = l.Value & ","

            Next
            sqlstring = sqlstring.Remove(sqlstring.Length - 1, 1)
            sqlstring &= " WHERE idtrack = " & idtrack

            rconn.Execute(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function


    Function getlogInfo(idUser, idTest, idreference) As DataRow
        Try

            sqlstring = "SELECT idtrack, date_attempt, date_end_attempt, last_page_seen, last_page_saved, number_of_save, number_of_attempt, attempts_for_suspension, suspended_until" &
           " FROM learning_testtrack" &
           " WHERE idUser = " & idUser &
           " idTest = " & idTest & " And " &
           "idreference = " & idreference

            Dim dr As DataRow = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)

            Return dr

        Catch ex As Exception
            SharedRoutines.LogWrite(Now & " "   & ex.tostring)
        End Try
        Return Nothing


    End function
End Class
