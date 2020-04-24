Imports System.Data
Imports TrainingSchool.SharedRoutines
Imports System.IO

Public Class Item
    Dim sqlstring As String = String.Empty
    Dim utility As SharedRoutines
    Dim rconn As rconnection

    Sub New()
        utility = New SharedRoutines
        rconn = CheckDatabase(rconn)
    End Sub
    Public Function AddEditItem(title As String, description As String, filename As String, id As Integer)
        Try
            filename = filename.Replace("/", "//")

            If id = 0 Then

                sqlstring = " INSERT INTO  learning_materials_lesson  (  author ,  title ,  description ,  path ) " &
                       " VALUES (" & HttpContext.Current.Session("iduser") & ", '" & EscapeMySql(title) & "', '" & EscapeMySql(description) & "', '" & filename & "') "
            Else

                sqlstring = " Update  learning_materials_lesson  set author='" & HttpContext.Current.Session("iduser") & "',  title ='" & EscapeMySql(title) & "',  path = '" & filename & "' where idlesson=" & id

            End If

            rconn.Execute(sqlstring, CommandType.Text, Nothing)


            Dim res_idanno As Integer
            Dim pathitem As String
            utility.makecategory(res_idanno, pathitem)

            sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  `r_item_id`, `r_type`,path,lev,idparent, iduser) " &
               " VALUES ( '" & EscapeMySql(title) & "', '" & EscapeMySql(title) & "',(select max(idLesson) from learning_materials_lesson ), 'item','" & pathitem & "',3," & res_idanno & "," & HttpContext.Current.Session("iduser") & ");"

            rconn.Execute(sqlstring, CommandType.Text, Nothing)


        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
        End Try

        Return True
        Return False

    End Function

End Class