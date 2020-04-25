Imports TrainingSchool.SharedRoutines

Public Class WfMakeFaq
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

                Case "get,savefaq"
                    Dim idfaq = Request.Form("idcategory")
                    SaveAdditionalFAQ(idfaq)
                Case "modfaq"
                    Dim idfaq = Request.QueryString("idfaq")
                    UpdateSequence(Request.QueryString("sequence"), Request.QueryString("idfaq"), Request.QueryString("offset"))

            End Select

        Catch ex As Exception

        Finally

        End Try



    End Sub
    Public Sub UpdateSequence(ByVal sequence As Integer, ByVal id As Integer, Optional offset As Integer = 0)

        Try




            Dim newsequence As Integer = sequence + offset


            sqlstring = "select idfaq,question,answer,sequence,(select max(sequence) from learning_faq where idcategory=" & Request.QueryString("idcategory") & ") as maxsequence from learning_faq where idcategory=" & Request.QueryString("idcategory") & " and sequence in (" & sequence & "," & newsequence & ") "

            Dim dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)




            If newsequence > 0 And newsequence <= dt.Rows(0)("maxsequence") Then

                For Each dr In dt.Rows

                    If dr("idfaq") = id Then
                        sqlstring = "UPDATE learning_faq set sequence=" & newsequence & " where idfaq=" & dr("idfaq")
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    Else
                        sqlstring = "UPDATE learning_faq set sequence=" & sequence & " where idfaq=" & dr("idfaq")
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    End If

                Next
            End If


        Catch ex As Exception
            LogWrite(Now & " "   & ex.tostring)
        End Try



    End Sub

    Function deleteObject(idfaq As String)

        sqlstring = "delete from learning_faq where idfaq=" & idfaq
        Try

            rconn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
            LogWrite(Now & " "   & ex.tostring)
            Response.Write("Errore nell'inserimento")
        End Try
    Return False 

 End function
    Function SaveAdditionalFAQ(ByVal idfaq As String)
        Dim question As String = String.Empty 


        Dim title As String = String.Empty 


        Dim answer As String = String.Empty 


        Dim id_catagory As String = String.Empty 


        Dim sequence As Integer = 1
        Try


            For Each key In Request.Form.AllKeys
                Try
                    If key.StartsWith("fieldquestion") Then
                        idfaq = key.Split("-")(2)
                        question = Request.Form(key)

                        sqlstring = " UPDATE  learning_faq  set  question = '" & question & "' where idfaq=" & idfaq

                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
                    End If

                    If key.StartsWith("fieldanswer") Then
                        idfaq = key.Split("-")(2)
                        answer = Request.Form(key)

                        sqlstring = " UPDATE  learning_faq  set  answer ='" & answer & "' where idfaq=" & idfaq

                        rconn.Execute(sqlstring, CommandType.Text, Nothing)
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