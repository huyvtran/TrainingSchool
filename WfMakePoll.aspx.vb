Imports TrainingSchool.SharedRoutines

Public Class WfMakePoll
    Inherits System.Web.UI.Page


    Dim rconn As rconnection
    Dim sqlstring As String =String.Empty

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

                Case "get,savepoll"
                    Dim id_poll = Request.Form("idpoll")
                    SaveAdditionalpoll(id_poll)
                Case "modpoll"
                    Dim id_quest = Request.QueryString("idquest")
                    UpdateSequence(Request.QueryString("sequence"), id_quest, Request.QueryString("offset"))

            End Select

        Catch ex As Exception

        Finally

        End Try



    End Sub
    Public Sub UpdateSequence(ByVal sequence As Integer, ByVal id As Integer, Optional offset As Integer = 0)

        Try


            Dim newsequence As Integer = sequence + offset


            sqlstring = "select id_quest,title_quest,sequence,(select max(sequence) from learning_pollquest where id_poll=" & Request.QueryString("idpoll") & ") as maxsequence from learning_pollquest where id_quest=" & id & " and sequence in (" & sequence & "," & newsequence & ") "

            Dim dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)



            If newsequence > 0 And newsequence <= dt.Rows(0)("maxsequence") Then

                For Each dr In dt.Rows

                    If dr("id_quest") = id Then
                        sqlstring = "UPDATE learning_pollquest set sequence=" & newsequence & " where id_quest=" & dr("id_quest")
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    Else
                        sqlstring = "UPDATE learning_pollquest set sequence=" & sequence & " where id_quest=" & dr("id_quest")
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    End If

                Next
            End If


        Catch ex As Exception
            LogWrite(Now & " "   & ex.tostring)
        End Try



    End Sub

    Function deleteObject(id_quest As String)

        sqlstring = "delete from learning_pollquest where id_quest=" & id_quest
        Try

            rconn.Execute(sqlstring, CommandType.Text, Nothing)
            sqlstring = "delete from learning_pollquestanswer where id_quest=" & id_quest
            rconn.Execute(sqlstring, CommandType.Text, Nothing)

            Response.Write("Domanda eliminata")
        Catch ex As Exception
            LogWrite(Now & " "   & ex.tostring)
            Response.Write("Errore nell'inserimento")
        End Try

        Response.End()
    Return False 

 End function
    Function SaveAdditionalpoll(ByVal id_poll As String)
        Dim question As String = String.Empty 


        Dim title As String = String.Empty 


        Dim answer As String = String.Empty 


        Dim id_quest As String = String.Empty 


        Dim id_answer As String = String.Empty 


        Dim sequence As Integer = 1
        Try


            For Each key In Request.Form.AllKeys
                Try
                    If key.StartsWith("fieldquestion") Then
                        id_quest = key.Split("-")(2)
                        question = Request.Form(key)

                        sqlstring = " UPDATE  learning_pollquest  set  title_quest = '" & question & "' where id_poll" & id_quest

                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    End If

                    If key.StartsWith("fieldanswer") Then
                        id_answer = key.Split("-")(2)
                        answer = Request.Form(key)
                        Dim listanswer = answer.Split("|")

                        For j = 0 To listanswer.Length - 1
                            sqlstring = " UPDATE  learning_pollquestanswer  set  answer ='" & answer & "' where id_answer=" & id_answer
                            rconn.Execute(sqlstring, CommandType.Text, Nothing)
                        Next
                    End If

                Catch ex As Exception
                    LogWrite(ex.ToString)

                End Try
            Next

            Response.Write("Aggiornamento Completato")


        Catch ex As Exception
            LogWrite(Now & " "   & ex.tostring)
            Response.Write("Errore nell'inserimento")
        End Try
        Response.End()
    Return False 

 End Function


End Class