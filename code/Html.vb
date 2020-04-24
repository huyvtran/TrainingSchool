Imports System.Data
Imports TrainingSchool.SharedRoutines
Imports System.IO

Public Class Html
    Dim sqlstring As String = String.Empty

    Dim rconn As rconnection
    Dim msg As String = String.Empty
    Dim utility As SharedRoutines
    Sub New()
        utility = New SharedRoutines
        rconn = CheckDatabase(rconn)
    End Sub
    Public Function SaveHtml(title As String, description As String, idPage As Integer)
        Try

            If idPage = 0 Then
                Try
                    sqlstring = "INSERT INTO   learning_htmlpage  (  title ,  textof ,  author ) " &
                 " VALUES ( '" & EscapeMySql(title) & "', '" & EscapeMySql(description) & "', " & HttpContext.Current.Session("iduser") & ") "

                    rconn.Execute(sqlstring, CommandType.Text, Nothing)


                    Dim res_idanno As Integer
                    Dim pathitem As String
                    utility.makecategory(res_idanno, pathitem)

                    sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  `r_item_id`, `r_type`,path,lev,idparent, iduser) " &
               " VALUES ( '" & EscapeMySql(title) & "','" & EscapeMySql(title) & "',(select max(idPage) from  learning_htmlpage), 'htmlpage','" & pathitem & "',3," & res_idanno & "," & HttpContext.Current.Session("iduser") & ");"

                    rconn.Execute(sqlstring, CommandType.Text, Nothing)

                    Return utility.GetLastId("idPage", "learning_htmlpage")


                Catch ex As Exception
                    msg = "Errore Creazione Html"
                    SharedRoutines.LogWrite(ex.ToString)
                End Try



            Else


                sqlstring = "update   learning_htmlpage     set   title ='" & EscapeMySql(title) & "',  textof ='" & EscapeMySql(description) & "' where idPage=" & idPage

                rconn.Execute(sqlstring, CommandType.Text, Nothing)


                sqlstring = " update `learning_kb_res` set r_name='" & EscapeMySql(title) & "', original_name='" & EscapeMySql(title) & "' where r_item_id=" & idPage
                rconn.Execute(sqlstring, CommandType.Text, Nothing)
            End If

            Return idPage

        Catch ex As Exception
            msg = "Errore creazione elemento"
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        msg = 0



    End Function

    Public Function CreateHtml(title As String, description As String)
        Try

            sqlstring = "INSERT INTO   learning_htmlpage  (  title ,  textof ,  author ) " &
                             " VALUES ( '" & EscapeMySql(title) & "', '" & EscapeMySql(description) & "', " & HttpContext.Current.Session("iduser") & ") "

            rconn.Execute(sqlstring, CommandType.Text, Nothing)


            sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  `r_item_id`, `r_type`) " &
                   " VALUES ( '" & EscapeMySql(title) & "', '" & EscapeMySql(title) & "',(select max(idPage) from  learning_htmlpage) , 'htmlpage');"

            rconn.Execute(sqlstring, CommandType.Text, Nothing)



            msg = "Elemento Html creato"
        Catch ex As Exception
            msg = "Errore Creazione Html"
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return msg

        Return False

    End Function


    Public Function getHtml(idpage As Integer)
        Try

            Dim sqlstring As String = "select *  from learning_htmlpage   where idPage =" & idpage

            msg = rconn.GetDataTable(sqlstring).Rows(0)("textof").ToString

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return msg
        Return False

    End Function

End Class