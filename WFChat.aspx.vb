Imports TrainingSchool.SharedRoutines
Public Class WFChat
    Inherits System.Web.UI.Page
    Dim utility As SharedRoutines
    Dim sqlstring As String = String.Empty
    Dim rconn As rconnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        utility = New SharedRoutines
        rconn = CheckDatabase(rconn)

        If Session("idUser") = 0 OrElse Session("idUser") Is Nothing Then
            Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", " <script>ExitSession();</script>")
        End If

        If Not Session("fullname") Is Nothing Then
            If Not (Application.AllKeys.Contains(Session("fullname"))) Then
                Application.Lock()

                If Application(Session("fullname")) Is Nothing Then
                    Application.Add(Session("fullname"), Session("idCourse"))
                End If

                If Application(Session("fullname")) <> Session("idCourse") And Not Application.AllKeys.Contains(Session("fullname")) Then
                    Application.Add(Session("fullname"), Session("idCourse"))
                End If

                Application.UnLock()
            End If


            If Request.QueryString("op") = "insert" Then
                InsertSession()
            End If


            If Request.QueryString("op") = "load" Then
                LoadChat()
            End If

            If Request.QueryString("op") = "loaduserlist" Then
                GetUserList()
            End If

            If Request.QueryString("op") = "delete" Then
                Application.Lock()
                Application.Remove(Session("fullname"))
                Application.UnLock()
            End If
        Else
            Exit Sub
        End If

    End Sub


    Function GetUserList()
        Dim htmlcontent As String = String.Empty



        For Each k In Application.AllKeys
            Try
                If Application(k) = Session("idCourse") Then
                    htmlcontent &= "<li>"
                    htmlcontent &= "<label class=""inline"">"
                    htmlcontent &= "<a href=""#"" OnClick=""window.close();"" >"
                    htmlcontent &= "<span class=""lbl""> " & Replace(k, ",", " ") & "</span></a>"
                    htmlcontent &= "</label>"
                    htmlcontent &= "</div>"
                    htmlcontent &= "</li>"
                End If
            Catch ex As Exception
            End Try

        Next
        Response.Write(htmlcontent)
        Response.End()


        Return False

    End Function


    Public Function InsertSession()
        Try
            sqlstring = "INSERT INTO chat (textof,fullname,iduser,idcourse,date_insert,profile) values('" & utility.EscapeMySql(Request.Form("message")) & "','" & Session("fullname") & "'," & Session("iduser") & "," & CInt(Session("idCourse")) & ",'" & utility.ConvertToMysqlDateTime(Now) & "'," & Session("admin") & ")"
            rconn.Execute(sqlstring, CommandType.Text, Nothing)




        Catch ex As Exception



            SharedRoutines.LogWrite( ex.ToString)
        End Try

        Return False

    End Function

    Public Sub LoadChat()


        Dim s = New SharedRoutines
        Dim htmlcontent As String = String.Empty


        Dim dt As DataTable = Nothing


        If Session("idCourse") Then
            sqlstring = "select * from chat where idCourse=" & Session("idCourse") & " order by date_insert asc"
        Else
            sqlstring = "select * from chat where idCourse=0 order by date_insert asc"
        End If

        dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        For Each dr In dt.Rows
            Try
                htmlcontent &= "<div class=""itemdiv dialogdiv wordwrap"">"
                htmlcontent &= "<div class=""user"">"

                If dr("profile") > 0 Then
                    htmlcontent &= "<img height=""40"" alt=""Avatar"" src=""assets/images/avatars/user.jpg"" />"
                Else

                    If Session("f") Then
                        htmlcontent &= "<img height=""40"" alt=""Avatar"" src=""assets/images/avatars/avatar1.png"" />"
                    Else
                        htmlcontent &= "<img height=""40"" alt=""Avatar"" src=""assets/images/avatars/avatar5.png"" />"
                    End If

                End If

                htmlcontent &= "</div>"
                htmlcontent &= "<div class=""body"">"
                htmlcontent &= "<div class=""time"">"
                htmlcontent &= "<i class=""icon-time""></i>"
                htmlcontent &= "<span class=""orange""> " & s.ConvertSecToDate(DateDiff(DateInterval.Second, CDate(dr("date_insert").ToString), CDate(Now))) & "</span>"
                htmlcontent &= "</div>"
                htmlcontent &= "<div class=""name"">"
                htmlcontent &= "<a href=""#""> " & Replace(dr("fullname").ToString, ",", " ") & " </a>"
                If dr("profile") > 0 Then
                    htmlcontent &= " <span class=""label label-info arrowed arrowed-in-right"">admin</span>"
                Else
                    htmlcontent &= " <span class=""label label-success arrowed arrowed-in-right"">corsista </span>"
                End If

                htmlcontent &= "</div>"
                htmlcontent &= "<div class=""text"">" & dr("textof").ToString & "</div>"

                htmlcontent &= "<div class=""tools"">"
                htmlcontent &= "<a href=""#"" class=""btn btn-minier btn-info"">"
                htmlcontent &= "<i class=""icon-only icon-share-alt""></i>"
                htmlcontent &= "</a>"
                htmlcontent &= "</div>"
                htmlcontent &= "</div>"
                htmlcontent &= "</div>"

            Catch ex As Exception
                htmlcontent &= "<div class=""text"">" & ex.Message & "</div>"
            End Try

        Next

        Response.Write(htmlcontent)
        Response.End()


    End Sub

End Class