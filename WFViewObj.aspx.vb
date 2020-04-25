Imports System.IO
Imports TrainingSchool.SharedRoutines
Public Class WFViewObjLMS
    Inherits System.Web.UI.Page
    Dim msg As String =String.Empty 

    Dim minhour
    Dim vhour As String =String.Empty 

    Dim vmin As String =String.Empty 

    Dim alertmsg As String =String.Empty

    Dim rconn As rconnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        rconn = CheckDatabase(rconn)

        If Session("idUser") = 0 OrElse Session("idUser") Is Nothing Then
            Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", " <script>ExitSession();</script>")
        End If

        Dim l As New SharedRoutines
        Dim h As Hashtable = Nothing
        Dim idreference As String = String.Empty
        Dim obj As String = String.Empty
        Dim idtrack As String = String.Empty


        Try

            If Request.QueryString("obj") <> "" And Request.QueryString("reference") <> "" And (Session("admin") Or CInt(Session("idCourse")) <> 0) Then
                obj = Request.QueryString("obj")
                idreference = Request.QueryString("reference")


                h = l.GetObjDetails(Session("idCourse"))

                Session("objecttype") = obj
                Session("reference") = idreference
                Session("resource") = h(idreference).ToString.Split(";")(1)
                Session("title") = h(idreference).ToString.Split(";")(0)
                Session("isterminator") = h(idreference).ToString.Split(";")(2)






                idtrack = l.log_Obj(obj, Session("iduser"), Session("idCourse"), idreference, Session("resource"))



                    Select Case obj
                        Case "faq"
                            htmlpage.Visible = False
                            faq_list.Visible = True
                            quizPage.Visible = False
                            pollPage.Visible = False
                            ViewFaq()
                        Case "test"
                            htmlpage.Visible = False
                            faq_list.Visible = False
                            quizPage.Visible = True
                            pollPage.Visible = False
                            ViewTest(idtrack)

                        Case "poll"
                            htmlpage.Visible = False
                            faq_list.Visible = False
                            quizPage.Visible = False
                            pollPage.Visible = True
                            ViewPoll(idtrack)
                        Case "htmlpage"
                            htmlpage.Visible = True
                            faq_list.Visible = False
                            quizPage.Visible = False
                            pollPage.Visible = False
                            ViewHtml()
                    End Select



                End If

        Catch ex As Exception
            SharedRoutines.LogWrite("Page_load: " & Now & " " & ex.ToString)
        End Try


        If obj = "item" Then

            htmlpage.Visible = False
            faq_list.Visible = False
            quizPage.Visible = False
            pollPage.Visible = False
            ViewItem()

            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End If


    End Sub
    Protected Sub ViewTest(idtracktest As String)


        Dim scriptframe As String =String.Empty 


        scriptframe = "   quizMaster.execute(""AdminAjaxLMS.aspx?op=modtest&oper=gettest&idtracktest=" & idtracktest & """, "".quizdisplay"", function (result) {"
        scriptframe &= "     $.ajax({"
        scriptframe &= " url : ""AdminAjaxLMS.aspx?op=modtest&oper=resultest"","
        scriptframe &= "  type: ""POST"","
        scriptframe &= " data : result,"
        scriptframe &= " success: function(data)"
        scriptframe &= " {"
        scriptframe &= ""
        scriptframe &= " },"
        scriptframe &= " error: function (data)"
        scriptframe &= " {"
        scriptframe &= " alert(data);"
        scriptframe &= " }"
        scriptframe &= "});"
        scriptframe &= "      console.dir(result);"
        scriptframe &= " });"

        Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", "<link rel=""stylesheet"" type=""text/css"" href=""https://code.jquery.com/mobile/1.4.0/jquery.mobile-1.4.0.min.css"">")
        ClientScript.RegisterClientScriptInclude("js2", "https://code.jquery.com/jquery-1.9.1.min.js")
        ClientScript.RegisterClientScriptInclude("js3", "https://code.jquery.com/mobile/1.4.0/jquery.mobile-1.4.0.min.js")



        ClientScript.RegisterStartupScript(Me.GetType(),
"ScriptQuiz", scriptframe.ToString, True)



    End Sub
    Protected Sub ViewItem()



        Dim sqlstring As String = "select  learning_materials_lesson.path from  learning_materials_lesson  join  learning_organization  on  learning_organization.idResource= learning_materials_lesson.idLesson where  idCourse=" & Session("idCourse") & " and  learning_organization.idOrg= " & Session("reference")
        Try
            Dim path As String = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)(0)("path").ToString


            Dim FileSize As Long


            Dim fexist As New FileInfo(IO.Path.Combine(Session("lmscontentpath"), "Content\item\" & path))

            If Not fexist.Exists Then
                Response.Write("Oggetto: " & fexist.FullName & " non trovato")
            Else
                Dim ext As String = Replace(IO.Path.GetExtension(IO.Path.Combine(Session("lmscontentpath"), "Content\item\" & path)), ".", "")
                Using MyFileStream As New FileStream(IO.Path.Combine(Session("lmscontentpath"), "Content\item\" & path), FileMode.Open)
                    FileSize = MyFileStream.Length

                    Dim Buffer(CInt(FileSize)) As Byte
                    MyFileStream.Read(Buffer, 0, CInt(FileSize))
                    MyFileStream.Close()
                    Response.ContentType = "application/" & ext & ""
                    Response.AddHeader("Content-Disposition", "inline;filename=""" & GetPath(path) & "")
                    Response.Buffer = True
                    Response.Clear()
                    Response.OutputStream.Write(Buffer, 0, FileSize)
                    Response.OutputStream.Flush()
                End Using

            End If




        Catch ex As Exception
            msg = "Impossibile trovare l'oggetto didattico!"
            HttpContext.Current.Response.Write(msg)
            SharedRoutines.LogWrite(ex.ToString & vbCrLf)
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

 End function
    Protected Sub ViewFaq()



        Dim sqlstring As String = "select question,answer from learning_faq where idcategory = (select idresource from   learning_organization   where objecttype='faq' and idCourse=" & Session("idCourse") & " and  learning_organization .idOrg= " & Request.QueryString("reference") & ") order by sequence asc"

        Dim dt As DataTable = Nothing

        dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        Dim content As String = String.Empty

        Dim question As String =String.Empty 

        Dim answer As String =String.Empty 

        Dim i As Integer = 1
        For Each dr In dt.Rows

            question = dr("question").ToString
            answer = dr("answer").ToString

            content = vbCrLf & "		<div class=""panel panel-default"">"
            content &= vbCrLf & "  	<div class=""panel-heading"">"
            content &= vbCrLf & "		<a href=""#faq-1-" & i & """ data-parent=""#faq_list"" data-toggle=""collapse"" class=""accordion-toggle collapsed"">"
            content &= vbCrLf & "<i class=""icon-chevron-left pull-right"" data-icon-hide=""icon-chevron-down"" data-icon-show=""icon-chevron-left""></i>"
            content &= vbCrLf & "            			<i class=""icon-comment bigger-130""></i>"
            content &= vbCrLf & question
            content &= vbCrLf & "	</a>"
            content &= vbCrLf & "	</div>"
            content &= vbCrLf & "<div class=""panel-collapse collapse"" id=""faq-1-" & i & """ > "
            content &= vbCrLf & "     		<div class=""panel-body"">"
            content &= vbCrLf & answer
            content &= vbCrLf & "		</div>"
            content &= vbCrLf & "	</div>"
            content &= vbCrLf & "  </div>"

            i = i + 1
            faq_list.InnerHtml &= content
        Next





    End Sub
    Protected Sub ViewHtml()
        Try

            Dim sqlstring As String = "select * from   learning_htmlpage   where idpage in (select idresource from  learning_organization  where objecttype='htmlpage' and idOrg=" & Session("reference") & ")"

            Dim title As String = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("title")
            Dim content As String = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("textof")
            htmlpage.InnerHtml &= "<h2>" & title & "</h2><br>"
            htmlpage.InnerHtml &= content

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try


    End Sub
    Protected Sub ViewPoll(ByVal idtrackpoll As Integer)

        Try
            Dim scriptframe As String = String.Empty


            scriptframe = "   pollMaster.execute(""AdminAjaxLMS.aspx?op=modpoll&oper=getpoll&idtrackpoll=" & idtrackpoll & """, "".polldisplay"", function (result) {"
            scriptframe &= "     $.ajax({"
            scriptframe &= " url : ""AdminAjaxLMS.aspx?op=modpoll&oper=resultpoll"","
            scriptframe &= "  type: ""POST"","
            scriptframe &= " data : result,"
            scriptframe &= " success: function(data)"
            scriptframe &= " {"
            scriptframe &= ""
            scriptframe &= " },"
            scriptframe &= " error: function (data)"
            scriptframe &= " {"
            scriptframe &= " alert(data);"
            scriptframe &= " }"
            scriptframe &= "});"
            scriptframe &= "      console.dir(result);"
            scriptframe &= " });"

            Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", "<link rel=""stylesheet"" type=""text/css"" href=""https://code.jquery.com/mobile/1.4.0/jquery.mobile-1.4.0.min.css"">")
            ClientScript.RegisterClientScriptInclude("js2", "https://code.jquery.com/jquery-1.9.1.min.js")
            ClientScript.RegisterClientScriptInclude("js3", "https://code.jquery.com/mobile/1.4.0/jquery.mobile-1.4.0.min.js")



            ClientScript.RegisterStartupScript(Me.GetType(),
    "ScriptPoll", scriptframe.ToString, True)

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try


    End Sub
End Class