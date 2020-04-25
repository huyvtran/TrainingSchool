Imports System.IO
Imports Newtonsoft.Json
Imports System.Web.Services
Imports TrainingSchool.SharedRoutines
Public Class WFCourseStructure
    Inherits System.Web.UI.Page


    Dim dt As DataTable = Nothing
    Dim idobj As Integer = 0
    Dim str As String = String.Empty
    Dim idcourse As Integer
    Dim idauthor As String = String.Empty
    Dim objecttype As String = String.Empty
    Dim scriptadd As New StringBuilder
    Dim titleobj As String = String.Empty
    Dim description As String = String.Empty
    Dim sqlstring As String = String.Empty
    Dim idOrg As Integer
    Dim endfolder As String = "</ol>"
    Dim enditem As String = "</li>"
    Dim result As Boolean
    Dim iconitem As String = "icon-file-text orange"
    Dim icontest As String = "icon-edit blue"
    Dim iconscorm As String = "icon-film green"
    Dim iconfaq As String = "icon-question-sign blue"
    Dim iconhtmlpage As String = "icon-file blue"
    Dim iconpoll As String = "icon-info-sign orange"
    Dim iconfolder As String = "icon-folder-close gray"
    Dim utility As SharedRoutines
    Dim conn As rconnection
    Dim pathInt As Integer = 1
    Dim numobj = 0
    Dim msgon = "l'Oggetto che si sta eliminando esiste già in un corso, eliminare prima l\'oggetto dal corso"
    Dim msgoff = "Eliminazione completata"
    Dim dbInsert As String
    Dim scriptFinal As New StringBuilder
    Dim objectState As String = String.Empty
    Dim idparent As String = String.Empty
    Dim i As Integer = 1
    Dim title As String = String.Empty

    Dim state As String = String.Empty
    Dim link As String = String.Empty
    Dim endlink As String = String.Empty
    Dim id As String = String.Empty

    Dim level As String = String.Empty
    Dim oldidOrg As Integer = -1
    Dim oldidparent As Integer = -1
    Dim newidparent As Integer = -1
    Dim newidOrg As Integer = -1
    Dim oldobjecttype As String = String.Empty
    Dim oldlevel As Integer = -1
    Dim newlevel As Integer = -1
    Dim newobjecttype As String = String.Empty
    Dim open As Integer = 0
    Dim folderitem As String
    Dim itemstr As String
    Dim typeicon As String = String.Empty
    Public Shared dbInsertfrom As String = System.Configuration.ConfigurationSettings.AppSettings("DBInsertfrom")


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            SharedRoutines.SetAcl(New List(Of String)(New String() {"2", "8"}))
            If Session("profile") Is Nothing OrElse Session("idprofile") = 3 Then
                Response.Redirect("wflogin.aspx?err=Non autorizzato")
            End If

            dbInsert = Session("database")
            utility = New SharedRoutines
            conn = CheckDatabase(conn)


            If Request.QueryString("idCourse") <> "" Then
                idcourse = Request.QueryString("idCourse")
                Session("idCourse") = idcourse
            End If
            If Trim(Request.QueryString("ThisCourse")) <> "" Then

                idcourse = Request.QueryString("ThisCourse")
                Session("idCourse") = idcourse
                LoadCourseStructure(idcourse)
            End If

        Catch ex As Exception
        End Try


        idauthor = Session("idUser")

        If Trim(Request.QueryString("op")) = "deleteobjectsource" Then

            deleteobjsource()
        End If



        If Trim(Request.QueryString("op")) = "prerequisites" Then
            ApplyPrerequisites(Request.QueryString("idcourse"))
        End If
        If Trim(Request.QueryString("AvailableCourse")) <> "" Then
            Session("AvailableCourse") = Request.QueryString("AvailableCourse")
            LoadCourseAvailable(Request.QueryString("AvailableCourse"))
        End If



        If Request.QueryString("op") = "update" Then

            Dim jsonstring As String = String.Empty


            Try
                Context.Request.InputStream.Position = 0
                Using inputStream As New StreamReader(Context.Request.InputStream)
                    jsonstring = inputStream.ReadToEnd
                End Using

            Catch ex As Exception

            End Try


            jsonstring = "{ROOT:" & jsonstring & "}"
            Dim tree As RootObject = JsonConvert.DeserializeObject(Of RootObject)(jsonstring)
            UpdateStructure(tree)

        ElseIf Request.QueryString("op") = "deletesource" Then
            Dim iddelete As Integer = Request.QueryString("id")
            Dim objecttype As String = Request.QueryString("objecttype")
            deleteobj(iddelete, objecttype)

        End If




        If Request.QueryString("loadobj") <> "" Then

            loadobjectAvailable()

        End If


    End Sub

    Function deleteobjsource()

        Dim id As String = Request.QueryString("id").Split("|")(1)
        sqlstring = "delete from learning_kb_res where r_item_id=" & id
        result = conn.Execute(sqlstring, CommandType.Text, Nothing)

    End Function

    Public Sub deleteall(idcourse)
        sqlstring = "delete from learning_organization where idcourse=" & idcourse
        result = conn.Execute(sqlstring, CommandType.Text, Nothing)


    End Sub

    Public Sub deleteobj(ByVal id As Integer, ByVal objecttype As String)

        Try




            Select Case objecttype
                Case "scormorg"
                    sqlstring = "delete from   learning_scorm_package  where idscorm_package not in (select idresource from  learning_organization  where objecttype='" & objecttype & "') and  idscorm_package=" & id
                    result = conn.Execute(sqlstring, CommandType.Text, Nothing)

                    If Not result Then
                        Response.Write(msgon)
                    Else
                        Response.Write(msgoff)
                    End If

                Case "item"
                    sqlstring = "delete from  learning_materials_lesson  where idlesson  not in (select idresource from  learning_organization  where  objecttype='" & objecttype & "') and idlesson=" & id
                    result = conn.Execute(sqlstring, CommandType.Text, Nothing)

                    If Not result Then
                        Response.Write(msgon)
                    Else
                        Response.Write(msgoff)
                    End If
                Case "test"
                    sqlstring = "delete from learning_testwhere idtest not in (select idresource from  learning_organization  where objecttype='" & objecttype & "') and idtest=" & id
                    result = conn.Execute(sqlstring, CommandType.Text, Nothing)

                    If Not result Then
                        Response.Write(msgon)
                    Else
                        sqlstring = "delete fromlearning_testquestanswer where idquest in (select idquest from learning_testquest where idtest=" & id & ")"
                        conn.Execute(sqlstring, CommandType.Text, Nothing)
                        sqlstring = "delete from learning_testquest where  idtest=" & id
                        conn.Execute(sqlstring, CommandType.Text, Nothing)
                        Response.Write(msgoff)
                    End If
                Case "faq"
                    sqlstring = "delete from learning_faq_cat where idcategory  not in (select idresource from  learning_organization  where objecttype='" & objecttype & "') and idCategory=" & id
                    result = conn.Execute(sqlstring, CommandType.Text, Nothing)

                    If Not result Then
                        Response.Write(msgon)
                    Else
                        sqlstring = "delete from learning_faq where  idCategory=" & id
                        conn.Execute(sqlstring, CommandType.Text, Nothing)
                        Response.Write(msgoff)
                    End If
                Case "poll"
                    sqlstring = "delete from learning_poll where id_poll  not in (select idresource from  learning_organization   where objecttype='" & objecttype & "') and id_poll=" & id
                    result = conn.Execute(sqlstring, CommandType.Text, Nothing)

                    If Not result Then
                        Response.Write(msgon)
                    Else


                        sqlstring = "delete from learning_pollquestanswer where id_quest in (select id_quest from learning_pollquest where id_poll=" & id & ")"
                        conn.Execute(sqlstring, CommandType.Text, Nothing)
                        sqlstring = "delete from learning_pollquest where  id_poll=" & id
                        conn.Execute(sqlstring, CommandType.Text, Nothing)
                        Response.Write(msgoff)
                    End If
                Case "htmlpage"
                    sqlstring = "delete from   learning_htmlpage   where idPage not in (select idresource from  learning_organization   where objecttype='" & objecttype & "') and idpage=" & id
                    result = conn.Execute(sqlstring, CommandType.Text, Nothing)

                    If Not result Then
                        Response.Write(msgon)
                    Else
                        Response.Write(msgoff)
                    End If
            End Select

            sqlstring = "delete from learning_kb_res where r_item_id=" & id & " and r_type='" & objecttype & "'"

            result = conn.Execute(sqlstring, CommandType.Text, Nothing)



        Catch ex As Exception
            Response.Write("Errore nell'eliminazione dell'oggetto")
            LogWrite(ex.Message)
        End Try
        Response.End()
    End Sub


    Public Sub LoadCourseAvailable(idCourse As String)


        Dim scriptFinal As New StringBuilder



        Try


            Dim objectState As String = String.Empty


            Dim idparent As String = String.Empty


            Dim i As Integer = 1
            Dim title As String = String.Empty


            Dim idOrg As Integer
            Dim prerequisites As String = String.Empty

            Dim state As String = String.Empty


            Dim link As String = String.Empty


            Dim endlink As String = String.Empty


            Dim path As String = String.Empty


            Dim h As Hashtable
            Dim id As String = String.Empty

            Dim objecttype As String = String.Empty


            Dim level As String = String.Empty


            Dim oldidOrg As Integer = -1
            Dim oldidparent As Integer = -1
            Dim newidparent As Integer = -1
            Dim newidOrg As Integer = -1
            Dim oldobjecttype As String = String.Empty


            Dim oldlevel As Integer = -1
            Dim newlevel As Integer = -1
            Dim newobjecttype As String = String.Empty



            Dim open As Integer = 0
            Dim isterminator As Integer
            Dim isvisible As Integer
            Dim idresource As Integer
            Dim typeicon As String = String.Empty



            h = GetPrerequisites(conn)


            Dim sqlstring As String = "select * from  " & dbInsertfrom & ".learning_organization    where idCourse=" & idCourse & "  order by path asc"

            dt = conn.GetDataTable(sqlstring)
            scriptFinal.Append("<ol class=""dd-list"">")


            If dt.Rows.Count > 0 Then

                For j = 0 To dt.Rows.Count - 1

                    If j > 0 Then
                        oldidOrg = dt.Rows(j - 1)("idOrg").ToString
                        oldlevel = dt.Rows(j - 1)("lev").ToString
                        oldidparent = dt.Rows(j - 1)("idparent").ToString
                        oldobjecttype = dt.Rows(j - 1)("objecttype").ToString
                    End If

                    If j < dt.Rows.Count - 1 Then
                        newidOrg = dt.Rows(j + 1)("idOrg").ToString
                        newlevel = dt.Rows(j + 1)("lev").ToString
                        newidparent = dt.Rows(j + 1)("idparent").ToString
                        newobjecttype = dt.Rows(j + 1)("objecttype").ToString

                    End If




                    objecttype = dt.Rows(j)("objecttype").ToString
                    path = dt.Rows(j)("path").ToString
                    prerequisites = dt.Rows(j)("prerequisites").ToString
                    idOrg = dt.Rows(j)("idOrg").ToString
                    title = dt.Rows(j)("title").ToString.Replace("'", "\'")
                    idparent = dt.Rows(j)("idparent").ToString
                    level = dt.Rows(j)("lev").ToString
                    idresource = dt.Rows(j)("idresource").ToString
                    isvisible = dt.Rows(j)("visible").ToString
                    isterminator = dt.Rows(j)("isterminator").ToString


                    id = "newfrom|" & idresource & "|" & EscapeMySql(title) & "|" & objecttype


                    Dim folderitem As String = "<li class=""dd-item dd2-item " & j & """ data-id=""" & id & """ > " &
                           "<div class=""dd-handle dd2-handle"">" &
                           "<i class=""normal-icon <<typeicon>>  bigger-130""></i>" &
                           "<i class=""drag-icon icon-move bigger-125""></i></div>" &
                           "<div class=""dd2-content"">" & UnEscapeMysql(title) & "" &
                                                                  "</div>"






                    Dim item As String = "<li class=""dd-item dd2-item " & j & """ data-id=""" & id & """ > " &
                           "<div class=""dd-handle dd2-handle"">" &
                            "<i class=""normal-icon <<typeicon>>  bigger-130""></i>" &
                            "<i class=""drag-icon icon-move bigger-125""></i></div>" &
                            "<div class=""dd2-content"">" & UnEscapeMysql(title) & "" &
                                                                    "</div>" &
                                    "</li>"


                    Select Case objecttype

                        Case "item"
                            item = Replace(item, "<<typeicon>>", iconitem)
                            scriptFinal.Append(item)
                        Case "test"
                            item = Replace(item, "<<typeicon>>", icontest)
                            scriptFinal.Append(item)
                        Case "scormorg"
                            item = Replace(item, "<<typeicon>>", iconscorm)
                            scriptFinal.Append(item)
                        Case "htmlpage"
                            item = Replace(item, "<<typeicon>>", iconhtmlpage)
                            scriptFinal.Append(item)
                        Case "faq"
                            item = Replace(item, "<<typeicon>>", iconfaq)
                            scriptFinal.Append(item)
                        Case "poll"
                            item = Replace(item, "<<typeicon>>", iconpoll)
                            scriptFinal.Append(item)
                        Case ""

                            If newlevel <= level Then

                                If Not newlevel <= level Then
                                    scriptFinal.Append(endfolder)
                                End If

                                scriptFinal.Append(enditem)
                            Else
                                folderitem &= "<ol class=""dd-list"">"
                            End If

                            folderitem = Replace(folderitem, "<<typeicon>>", iconfolder)
                            scriptFinal.Append(folderitem)
                    End Select



                    If newlevel < level Then
                        For g = 1 To level - newlevel
                            scriptFinal.Append(endfolder)
                            scriptFinal.Append(enditem)
                        Next
                    End If










                Next

            Else

                Dim j = 0


                Dim item As String = "<li class=""dd-item " & j & """ data-id=""0"" > </li>"
                scriptFinal.Append(item)
            End If




        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try



        scriptFinal.Append(endfolder)
        Response.Write(scriptFinal.ToString)
        Response.End()

    End Sub


    Function gettitle(title As String)

        If title.Length > 50 Then
            Return EscapeMySql(title).ToString.Substring(0, 50) & ".."
        Else
            Return EscapeMySql(title)
        End If
        Return False

    End Function

    Public Sub LoadCourseStructure(idCourse As String)


        Dim scriptFinal As New StringBuilder
        Dim objectState As String = String.Empty
        Dim idparent As String = String.Empty
        Dim i As Integer = 1
        Dim title As String = String.Empty
        Dim idOrg As Integer
        Dim prerequisites As String = String.Empty
        Dim state As String = String.Empty
        Dim link As String = String.Empty
        Dim endlink As String = String.Empty
        Dim path As String = String.Empty
        Dim h As Hashtable
        Dim id As String = String.Empty
        Dim objecttype As String = String.Empty
        Dim level As String = String.Empty
        Dim oldidOrg As Integer = -1
        Dim oldidparent As Integer = -1
            Dim newidparent As Integer = -1
            Dim newidOrg As Integer = -1
            Dim oldobjecttype As String = String.Empty
        Dim oldlevel As Integer = -1
        Dim newlevel As Integer = -1
            Dim newobjecttype As String = String.Empty
        Dim open As Integer = 0
        Dim isterminator As Integer
            Dim isvisible As Integer
            Dim idresource As Integer
            Dim typeicon As String = String.Empty

        Try

            h = GetPrerequisites(conn)


            Dim sqlstring As String = "select * from  learning_organization    where idCourse=" & idCourse & "  order by path asc"

            dt = conn.GetDataTable(sqlstring)
            scriptFinal.Append("<ol class=""dd-list"">")


            If dt.Rows.Count > 0 Then

                For j = 0 To dt.Rows.Count - 1

                    If j > 0 Then
                        oldidOrg = dt.Rows(j - 1)("idOrg").ToString
                        oldlevel = dt.Rows(j - 1)("lev").ToString
                        oldidparent = dt.Rows(j - 1)("idparent").ToString
                        oldobjecttype = dt.Rows(j - 1)("objecttype").ToString
                    End If

                    If j < dt.Rows.Count - 1 Then
                        newidOrg = dt.Rows(j + 1)("idOrg").ToString
                        newlevel = dt.Rows(j + 1)("lev").ToString
                        newidparent = dt.Rows(j + 1)("idparent").ToString
                        newobjecttype = dt.Rows(j + 1)("objecttype").ToString

                    End If




                    objecttype = dt.Rows(j)("objecttype").ToString
                    path = dt.Rows(j)("path").ToString
                    prerequisites = dt.Rows(j)("prerequisites").ToString
                    idOrg = dt.Rows(j)("idOrg").ToString
                    title = dt.Rows(j)("title").ToString.Replace("'", "\'")
                    idparent = dt.Rows(j)("idparent").ToString
                    level = dt.Rows(j)("lev").ToString
                    idresource = dt.Rows(j)("idresource").ToString
                    isvisible = dt.Rows(j)("visible").ToString
                    isterminator = dt.Rows(j)("isterminator").ToString


                    id = "|" & idOrg & "|" & idparent & "|" & path & "|" & level & "|" & EscapeMySql(title) & "|" & objecttype & "|" & idresource & "|" & idCourse & "|" & prerequisites & "|" & isterminator & "|" & isvisible


                    '                   Dim folderitem As String = "<li class=""dd-item"" data-id=""" & id & """><div class=""dd-handle"">" & EscapeMysql(title) & "" &
                    '"<div class=""pull-right action-buttons"">" &
                    ' "<a class=""red deleteobject"" data-obj=""" & j & """  href=""#"">" &
                    '                            "<i class=""icon-trash bigger-130""></i></a> " &
                    '                                       "</div></div>" &
                    '                                    "<ol class=""dd-list"">"

                    Dim folderitem As String = "<li class=""dd-item dd2-item " & j & """ data-id=""" & id & """ > " &
                           "<div class=""dd-handle dd2-handle"">" &
                           "<i class=""normal-icon <<typeicon>>  bigger-130""></i>" &
                           "<i class=""drag-icon icon-move bigger-125""></i></div>" &
                           "<div class=""dd2-content"">" & gettitle(UnEscapeMysql(title)) & "" &
                            "<div class=""pull-right action-buttons"">" &
" <a class=""red deleteobject"" data-obj=""" & j & """  href=""#"">" &
                            "<i class=""icon-trash bigger-130""></i></a></div> " &
                                       "</div>"


                    Dim badge As String = ""
                    Dim prebadge As String = ""
                    If isterminator Then
                        badge = "<span class=""icon icon-certificate red""></span>"
                    End If
                    If prerequisites <> "" Then
                        prebadge = "<span title=""" & utility.GetTitleReference(prerequisites) & """  class=""ui-icon icon-info green""></span>"
                    End If

                    Dim item As String = "<li class=""dd-item dd2-item " & j & """ data-id=""" & id & """ > " &
                           "<div class=""dd-handle dd2-handle"">" &
                            "<i class=""normal-icon <<typeicon>>  bigger-130""></i> " & prebadge & " " &
                            "<i class=""drag-icon icon-move bigger-125""></i></div> " &
                           "<div class=""dd2-content"" title=""" & title & """>" & badge & " " & gettitle(title) & " " &
                             "<div Class=""pull-right action-buttons"">" &
                              "<a title='Anteprima' class=""green"" data-obj=""" & j & """  OnClick=""Viewer('" & objecttype & "'," & idOrg & ")"" href=""#"">" &
                             "<i class=""icon-eye-open bigger-130""></i>" &
                             "</a>" &
   "<a title='Modifica' class=""green"" data-obj=""" & j & """  OnClick=""openwindowEdit('" & objecttype & "'," & idresource & ",'" & UnEscapeMysql(title) & "')"" href=""#"">" &
                             "<i class=""icon-cog red bigger-130""></i>" &
                             "</a>" &
   "<a title='Proprietà' class=""blue"" data-obj=""" & j & """  OnClick=""openwindow('" & objecttype & "'," & idOrg & "," & idCourse & "," & isvisible & "," & isterminator & ",'" & UnEscapeMysql(title) & "')"" href=""#"">" &
                             "<i class=""icon-pencil bigger-130""></i>" &
                             "</a>" &
                             " <a title='Elimina' class=""red deleteobject"" data-obj=""" & j & """  href=""#"">" &
                             "<i class=""icon-trash bigger-130""></i></a></div> " &
                                        "</div>" &
                                    "</li>"

                    '"<a class=""blue modObjDidattico"" data-obj=""" & j & """ href=""#"">" &
                    '                             "<i class=""icon-pencil bigger-130""></i></a>" &
                    Select Case objecttype

                        Case "item"
                            item = Replace(item, "<<typeicon>>", iconitem)
                            scriptFinal.Append(item)
                        Case "test"
                            item = Replace(item, "<<typeicon>>", icontest)
                            scriptFinal.Append(item)
                        Case "scormorg"
                            item = Replace(item, "<<typeicon>>", iconscorm)
                            scriptFinal.Append(item)
                        Case "htmlpage"
                            item = Replace(item, "<<typeicon>>", iconhtmlpage)
                            scriptFinal.Append(item)
                        Case "faq"
                            item = Replace(item, "<<typeicon>>", iconfaq)
                            scriptFinal.Append(item)
                        Case "poll"
                            item = Replace(item, "<<typeicon>>", iconpoll)
                            scriptFinal.Append(item)
                        Case ""

                            If newlevel <= level Then

                                If Not newlevel <= level Then
                                    scriptFinal.Append(endfolder)
                                End If

                                scriptFinal.Append(enditem)
                            Else
                                folderitem &= "<ol class=""dd-list"">"
                            End If

                            folderitem = Replace(folderitem, "<<typeicon>>", iconfolder)
                            scriptFinal.Append(folderitem)
                    End Select



                    If newlevel < level Then
                        For g = 1 To level - newlevel
                            scriptFinal.Append(endfolder)
                            scriptFinal.Append(enditem)
                        Next
                    End If










                Next

            Else

                Dim j = 0


                Dim item As String = "<li class=""dd-item " & j & """ data-id=""0"" > </li>"
                scriptFinal.Append(item)
            End If




        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try



        scriptFinal.Append(endfolder)
        Response.Write(scriptFinal.ToString)
        Response.End()

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function LoadCorsi() As ArrayList
        Dim lstArrCorsi As New ArrayList()
        Try

            Dim conn
            If conn Is Nothing Then
                conn = New rconnection("" & dbInsertfrom & "", "")

            End If

            Dim sqlstring = "select idCourse,code,name from  learning_course where idcourse in (select distinct(idcourse) from learning_organization)  order by idcourse desc"

            Dim dt As DataTable = Nothing

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            For Each dr In dt.Rows
                Dim l1 As New ListItem
                l1.Text = dr("code") & "-" & dr("name")
                l1.Value = dr("idCourse")
                lstArrCorsi.Add(l1)
            Next
            Return lstArrCorsi

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

        'Return False

    End Function


    Sub GetNumbObject(objnode As ROOT)



        If Not objnode.CHILDREN Is Nothing Then

            For Each obj As ROOT In objnode.CHILDREN
                numobj = numobj + 1
                GetNumbObject(obj)


            Next

        End If



    End Sub

    Function ApplyPrerequisites(idcourse As String)
        Try
            sqlstring = "select * from  learning_organization  where objecttype !='' and idcourse=" & idcourse & " order by path asc"

            Dim dt As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            For i = 1 To dt.Rows.Count - 1
                sqlstring = "update learning_organization set prerequisites=" & dt.Rows(i - 1)("idorg") & " where idorg=" & dt.Rows(i)("idOrg")
                conn.Execute(sqlstring, CommandType.Text, Nothing)

                If i = dt.Rows.Count - 1 Then
                    sqlstring = "update learning_organization Set isterminator=1 where idorg=" & dt.Rows(i)("idOrg")
                    conn.Execute(sqlstring, CommandType.Text, Nothing)
                End If
            Next
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Function

    Protected Sub UpdateStructure(tree As RootObject)


        Dim path As String = String.Empty

        Dim idcourse As String = Request.QueryString("idCourse")
        Dim idauthor As String = Session("idUser")
        Dim i = 1

        sqlstring = "select count(*) as n from  learning_organization  where idcourse=" & idcourse

        Dim objinCourse As Integer = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("n")

        Dim trem As ROOT

        For Each t In tree.ROOT
            If t.ID = "0" Then
                trem = t
            End If
            GetNumbObject(t)
            numobj = numobj + 1
        Next

        tree.ROOT.Remove(trem)


        If objinCourse <= numobj Then

            If idauthor <> "" And idcourse <> "" Then

                For Each objnode In tree.ROOT

                    path = "/root/" & F8Int(i)
                    UpdateNodo(objnode, path, 0)
                    i = i + 1

                Next

            End If
        End If

        LoadCourseStructure(idcourse)
    End Sub






    Function isLo(id As String) As Boolean

        If id.IndexOf("scormorg") > -1 Or id.IndexOf("htmlpage") > -1 Or id.IndexOf("item") > -1 Or id.IndexOf("faq") > -1 Or id.IndexOf("poll") > -1 Then
            Return True
        Else
            Return False
        End If

        Return False

    End Function
    Function UpdateNodo(objnode As ROOT, path As String, idparent As String)

        Dim str() As String


        Dim pathchild As String = String.Empty

        Dim updateidorg As String = String.Empty

        str = objnode.ID.Split("|")


        updateidorg = UpdateNodeChild(str, path, objnode, idparent)


        If Not isLo(objnode.ID) Then
            Dim i As Integer = 1
            For Each obj As ROOT In objnode.CHILDREN

                pathchild = path & "/" & F8Int(i)
                UpdateNodo(obj, pathchild, updateidorg)
                i = i + 1

            Next
        End If
        Return False

    End Function

    Function UpdateNodeChild(ByVal str() As String, ByVal path As String, ByVal objnode As ROOT, Optional idparent As Integer = 0)



        Dim prerequisites As String = String.Empty

        Dim title As String = String.Empty

        Dim level As String = String.Empty

        Dim isterminator As Integer
        Dim idresource As Integer
        Dim objecttype As String = String.Empty




        level = path.Split("/").Length - 2

        If str(0).StartsWith("new") Then

            Try
                idresource = str(1)
                title = str(2)
                objecttype = str(3)


                If str(0).StartsWith("newfrom") Then

                    Select Case objecttype


                        Case "test"
                            idresource = GetCopyResource(idresource, objecttype)
                        Case "scormorg"
                            idresource = GetCopyResource(idresource, objecttype)
                        Case "poll"
                            idresource = GetCopyResource(idresource, objecttype)
                        Case "item"
                            idresource = GetCopyResource(idresource, objecttype)
                        Case "faq"
                            idresource = GetCopyResource(idresource, objecttype)
                    End Select
                End If


                sqlstring = "INSERT INTO  learning_organization  ( idParent ,  path ,  lev ,  title ,  objectType ,  idResource ,  idCategory ,  idUser ,  dateInsert ,  idCourse ,  prerequisites ,  isTerminator ,  visible )  " &
                                         " VALUES (" & idparent & ", '" & path & "', " & level & ", '" & EscapeMySql(title) & "', '" & objecttype & "', " & idresource & ", 0,  '" & Session("iduser") & "', '" & utility.ConvertToMysqlDateTime(Now) & "', " & idcourse & ", '" & prerequisites & "', 0 , 1);"



                Try
                    conn.Execute(sqlstring, CommandType.Text, Nothing)


                    idOrg = conn.GetDataTable("SELECT max(idorg)  as id from learning_organization ", CommandType.Text, Nothing).Rows(0)("id")




                    'SetDParam(idOrg, objecttype)

                Catch ex As Exception
                    SharedRoutines.LogWrite(ex.ToString)
                    Return -1
                End Try

            Catch ex As Exception
                SharedRoutines.LogWrite(ex.ToString)
                Return -1
            End Try

        Else



            idOrg = str(1)
            title = str(5)
            objecttype = str(6)
            idresource = str(7)
            idcourse = str(8)
            prerequisites = str(9)
            isterminator = str(10)
            Visible = str(11)

            If (str(0) = "delete" And (objnode.CHILDREN Is Nothing OrElse objnode.CHILDREN.Count = 0)) Then
                sqlstring = "delete from  learning_organization  where idOrg=" & idOrg
            Else
                sqlstring = "update  learning_organization  set lev=" & level & ",idparent=" & idparent & ", path='" & path & "' where idOrg=" & idOrg & " and  idCourse=" & idcourse & "  "

            End If



            Try
                conn.Execute(sqlstring, CommandType.Text, Nothing)

            Catch ex As Exception
                SharedRoutines.LogWrite(ex.ToString)
                Return -1
            End Try


        End If

        Return idOrg
        Return False

    End Function


    Function GetCopyResource(idresource As String, objecttype As String)

        Dim newidresource As String = String.Empty

        Dim dt As DataTable = Nothing


        Dim dt1 As DataTable
        Dim dr As DataRow

        Try

            Select Case objecttype

                Case "test"

                    sqlstring = "INSERT INTO " & dbInsert & ".learning_test ( `author`, `title`, `description`, `point_type`, `point_required`, `display_type`, `order_type`, `shuffle_answer`, `question_random_number`, `save_keep`, `mod_doanswer`, `can_travel`, `show_only_status`, `show_score`, `show_score_cat`, `show_doanswer`, `show_solution`, `time_dependent`, `time_assigned`, `penality_test`, `penality_time_test`, `penality_quest`, `penality_time_quest`, `max_attempt`, `hide_info`, `order_info`, `use_suspension`, `suspension_num_attempts`, `suspension_num_hours`, `suspension_prerequisites`, `chart_options`, `mandatory_answer`, `score_max`) "
                    sqlstring &= " select  `author`, `title`, `description`, `point_type`, `point_required`, `display_type`, `order_type`, `shuffle_answer`, `question_random_number`, `save_keep`, `mod_doanswer`, `can_travel`, `show_only_status`, `show_score`, `show_score_cat`, `show_doanswer`, `show_solution`, `time_dependent`, `time_assigned`, `penality_test`, `penality_time_test`, `penality_quest`, `penality_time_quest`, `max_attempt`, `hide_info`, `order_info`, `use_suspension`, `suspension_num_attempts`, `suspension_num_hours`, `suspension_prerequisites`, `chart_options`, `mandatory_answer`, `score_max`  from " & dbInsertfrom & ".learning_test where idtest=" & idresource

                    conn.Execute(sqlstring, CommandType.Text, Nothing)

                    newidresource = utility.GetLastId("idtest", "learning_test")


                    sqlstring = " Select `idQuest`,  `idCategory`, `type_quest`, `title_quest`, `difficult`, `time_assigned`, `sequence`, `page`, `shuffle` from " & dbInsertfrom & ".learning_testquest where idtest=" & idresource

                    dt = utility.GetDatatable(sqlstring)

                    For Each dr In dt.Rows

                        sqlstring = "INSERT INTO " & dbInsert & ".learning_testquest ( `idTest`, `idCategory`, `type_quest`, `title_quest`, `difficult`, `time_assigned`, `sequence`, `page`, `shuffle`) VALUES " &
                        " (" & newidresource & "," & dr("idcategory") & ",'" & dr("type_quest") & "','" & EscapeMySql(dr("title_quest")) & "'," & dr("difficult") & "," & dr("time_assigned") & "," & dr("sequence") & "," & dr("page") & "," & dr("shuffle") & ")"
                        Try
                            conn.Execute(sqlstring, CommandType.Text, Nothing)
                        Catch ex As Exception
                        End Try

                        Dim newidquest As Integer = utility.GetLastId("idquest", "learning_testquest")

                        sqlstring = "select * from " & dbInsertfrom & ".learning_testquestanswer where idquest=" & dr("idquest")


                        dt1 = utility.GetDatatable(sqlstring)

                        For Each dr1 In dt1.Rows

                            sqlstring = "INSERT INTO " & dbInsert & ".learning_testquestanswer (`idQuest`, `sequence`, `is_correct`, `answer`, `comment`, `score_correct`, `score_incorrect`) VALUES " &
                         " (" & newidquest & "," & dr1("sequence") & "," & dr1("is_correct") & ",'" & EscapeMySql(dr1("answer")) & "',''," & dr1("score_correct") & "," & dr1("score_incorrect") & ")"
                            Try
                                conn.Execute(sqlstring, CommandType.Text, Nothing)
                            Catch ex As Exception
                                Exit Function
                            End Try
                        Next
                    Next
                'sqlstring = "select * from '" & dbtable.test & "' where idtest=" & idresource
                'sqlstring = "select * from 'learning_testquestanswer' a join   'learning_testquest'  b on a.idquest=b.idquest where idtest=" & idresource & "' order by b.idquest asc"


                Case "scormorg"
                    Dim newidscormpackage As Integer
                    sqlstring = "select * from " & dbInsertfrom & ".learning_scorm_organizations where idscorm_organization=" & idresource

                    dr = utility.GetDatatable(sqlstring).rows(0)

                    Dim orgidentifier As String = dr("org_identifier").ToString
                    Dim title As String = EscapeMySql(dr("title").ToString)
                    Dim oldscrompackage As Integer = dr("idscorm_package")

                    sqlstring = "INSERT INTO " & dbInsert & ".learning_scorm_package ( `idpackage`, `idProg`, `path`, `defaultOrg`, `idUser`, `scormVersion`) "
                    sqlstring &= "select  `idpackage`, `idProg`, `path`, `defaultOrg`, `idUser`, `scormVersion` from " & dbInsertfrom & ".learning_scorm_package where idscorm_package= " & oldscrompackage

                    conn.Execute(sqlstring, CommandType.Text, Nothing)


                    newidscormpackage = utility.GetLastId("idscorm_package", "learning_scorm_package")


                    sqlstring = "select * from " & dbInsertfrom & ".learning_scorm_resources where idscorm_package=" & oldscrompackage

                    dt = utility.GetDatatable(sqlstring)


                    For Each dr In dt.Rows

                        sqlstring = "INSERT INTO " & dbInsert & ".learning_scorm_resources (`idsco`, `idscorm_package`, `scormtype`, `href`) " &
                               " VALUES ( '" & dr("idsco") & "'," & newidscormpackage & ", '" & dr("scormtype") & "','" & dr("href") & "');"
                        Try
                            conn.Execute(sqlstring, CommandType.Text, Nothing)
                        Catch ex As Exception
                            LogWrite(ex.ToString)
                        End Try
                    Next


                    sqlstring = " INSERT INTO " & dbInsert & ".learning_scorm_organizations ( `org_identifier`, `idscorm_package`, `title`, `nChild`, `nDescendant`) " &
                "  VALUES('" & orgidentifier & "'," & newidscormpackage & ",'" & title & "',1,1)"


                    conn.Execute(sqlstring, CommandType.Text, Nothing)

                    newidresource = utility.GetLastId("idscorm_organization", "learning_scorm_organizations")


                    sqlstring = "INSERT INTO learning_scorm_items ( `idscorm_organization`, `idscorm_parentitem`, `adlcp_prerequisites`, `adlcp_maxtimeallowed`, `adlcp_timelimitaction`, `adlcp_datafromlms`, `adlcp_masteryscore`, `item_identifier`, `identifierref`, `idscorm_resource`, `isvisible`, `parameters`, `title`, `nChild`, `nDescendant`, `adlcp_completionthreshold`) "
                    sqlstring &= " select   (select max(idscorm_organization) from learning_scorm_organizations) as id, `idscorm_parentitem`, `adlcp_prerequisites`, `adlcp_maxtimeallowed`, `adlcp_timelimitaction`, `adlcp_datafromlms`, `adlcp_masteryscore`, `item_identifier`, `identifierref`,(select `idscorm_resource` from learning_Scorm_resources where idscorm_package=" & newidscormpackage & " and scormType='sco') as idscorm_resource, `isvisible`, `parameters`, `title`, `nChild`, `nDescendant`, `adlcp_completionthreshold` from " & dbInsertfrom & ".learning_scorm_items where idscorm_organization= " & idresource



                    conn.Execute(sqlstring, CommandType.Text, Nothing)


                Case "poll"



                    sqlstring = "INSERT INTO " & dbInsert & ".learning_poll ( `author`, `title`, `description`) " &
                    "select  `author`, `title`, `description` from " & dbInsertfrom & ".learning_poll where id_poll=" & idresource

                    conn.Execute(sqlstring, CommandType.Text, Nothing)

                    newidresource = utility.GetLastId("id_poll", "learning_poll")


                    sqlstring = " Select * from " & dbInsertfrom & ".learning_pollquest where id_poll=" & idresource

                    dt = utility.GetDatatable(sqlstring)

                    For Each dr In dt.Rows

                        sqlstring = "INSERT INTO `learning_pollquest` (`id_poll`, `id_category`, `type_quest`, `title_quest`, `sequence`, `page`) " &
                     " values(" & newidresource & "," & dr("id_category") & ",'" & dr("type_quest") & "','" & EscapeMySql(dr("title_quest")) & "'," & dr("sequence") & "," & dr("page") & ")"

                        Try
                            conn.Execute(sqlstring, CommandType.Text, Nothing)
                        Catch ex As Exception
                            LogWrite(ex.ToString)
                        End Try

                        Dim newidquest As Integer = utility.GetLastId("id_quest", "learning_pollquest")

                        sqlstring = "select * from " & dbInsertfrom & ".learning_pollquestanswer where id_quest=" & dr("id_quest")


                        dt1 = utility.GetDatatable(sqlstring)

                        For Each dr1 In dt1.Rows

                            sqlstring = "INSERT INTO " & dbInsert & ".learning_pollquestanswer (`id_Quest`, `sequence`,  `answer`) VALUES " &
                         " (" & newidquest & "," & dr1("sequence") & ",'" & EscapeMySql(dr1("answer").ToString) & "')"
                            Try
                                conn.Execute(sqlstring, CommandType.Text, Nothing)
                            Catch ex As Exception
                                LogWrite(ex.ToString & sqlstring)
                            End Try
                        Next
                    Next
                Case "faq"

                    sqlstring = "INSERT INTO " & dbInsert & ".learning_faq_cat ( `author`, `title`, `description`) " &
                    "select  `author`, `title`, `description` from " & dbInsertfrom & ".learning_faq_cat where idCategory=" & idresource

                    conn.Execute(sqlstring, CommandType.Text, Nothing)

                    newidresource = utility.GetLastId("idCategory", "learning_faq_cat")

                    sqlstring = "INSERT INTO `learning_faq` ( `idCategory`, `question`, `title`, `keyword`, `answer`, `sequence`) " &
                "select  " & newidresource & ", `question`, `title`, `keyword`, `answer`, `sequence` from " & dbInsertfrom & ".learning_faq where idCategory=" & idresource

                Case "item"

                    Try
                        sqlstring = "INSERT INTO " & dbInsert & ".`learning_materials_lesson` ( `author`, `title`, `description`, `path`)  " &
                  " select  `author`, `title`, `description`, `path` from   " & dbInsertfrom & ".learning_materials_lesson where idLesson=" & idresource
                        conn.Execute(sqlstring, CommandType.Text, Nothing)
                        newidresource = utility.GetLastId("idLesson", "learning_materials_lesson")
                    Catch ex As Exception
                        LogWrite("Insert Item: " & ex.ToString)

                    End Try

                Case "htmlpage"

                    Try
                        sqlstring = "INSERT INTO " & dbInsert & ".`learning_htmlplage`  (`idPage`, `title`, `textof`, `author`)   " &
                  " select   `title`, `textof`, `author`  from   " & dbInsertfrom & ".learning_htmlplage where idPage=" & idresource
                        conn.Execute(sqlstring, CommandType.Text, Nothing)
                        newidresource = utility.GetLastId("idPage", "learning_htmlpage")
                    Catch ex As Exception
                        LogWrite("Insert page: " & ex.ToString)

                    End Try


            End Select

            Return newidresource
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try


        Return False

    End Function


    Function GetPrerequisites(conn As rconnection)
        Dim j As New Hashtable
        Try

            Dim sqlstring As String = "Select idreference,status from " & dbInsertfrom & ".learning_commontrack a where  idUser=" & Session("iduser") & "  "

            dt = conn.GetDataTable(sqlstring)

            For Each dr In dt.Rows
                j.Add(dr("idreference").ToString, dr("status").ToString)
            Next

        Catch ex As Exception
            Return Nothing
        End Try

        Return j
        Return False

    End Function





    Public Sub loadobjectAvailable()



        Dim anno As String
        Dim materia As String


        Try

            Dim sqlstring As String = "select res_id,r_name as title ,r_item_id as id,lev,idparent,path,r_type,(select r_name from learning_kb_res where res_id=a.idparent) as anno,(select distinct(r_name) from learning_kb_res where res_id in(select idparent from learning_kb_res where res_id=a.idparent)) as materia From learning_kb_res a where iduser=" & Session("iduser") & " ORDER BY res_id ASC,idparent asc"

            dt = conn.GetDataTable(sqlstring)


            If dt.Select("r_type='" & Request.QueryString("loadobj") & "'").Count = 0 Then
                itemstr = "<li class=""dd-item 0"" data-id=""0"" > </li>"
                scriptFinal.Append(itemstr)
                scriptFinal.Append(endfolder)
                'scorm_list.InnerHtml = scriptadd.ToString
                Response.Write(scriptFinal.ToString)
                Response.End()
            End If



            scriptFinal.Append("<ol class=""dd-list"">")

            If dt.Rows.Count > 0 Then

                For j = 0 To dt.Rows.Count - 1

                    If j > 0 Then
                        oldidOrg = dt.Rows(j - 1)("res_id").ToString
                        oldlevel = dt.Rows(j - 1)("lev").ToString
                        oldidparent = dt.Rows(j - 1)("idparent").ToString
                        oldobjecttype = dt.Rows(j - 1)("r_type").ToString
                    End If

                    If j < dt.Rows.Count - 1 Then
                        newidOrg = dt.Rows(j + 1)("res_id").ToString
                        newlevel = dt.Rows(j + 1)("lev").ToString
                        newidparent = dt.Rows(j + 1)("idparent").ToString
                        newobjecttype = dt.Rows(j + 1)("r_type").ToString

                    End If

                    idobj = dt.Rows(j)("id").ToString
                    objecttype = dt.Rows(j)("r_type").ToString
                    idOrg = dt.Rows(j)("res_id").ToString
                    title = dt.Rows(j)("title").ToString.Replace("'", "\'")
                    idparent = dt.Rows(j)("idparent").ToString
                    level = dt.Rows(j)("lev").ToString
                    anno = dt.Rows(j)("anno").ToString
                    materia = dt.Rows(j)("materia").ToString
                    id = "new|" & idobj & "|" & gettitle(UnEscapeMysql(title)) & "|" & objecttype
                    Dim iconobj As String
                    Select Case objecttype


                        Case Request.QueryString("loadobj")

                            Select Case Request.QueryString("loadobj")
                                Case "scormorg"
                                    iconobj = iconscorm
                                Case "htmlpage"
                                    iconobj = iconhtmlpage
                                Case "item"
                                    iconobj = iconitem
                                Case "poll"
                                    iconobj = iconpoll
                                Case "test"
                                    iconobj = icontest
                                Case "faq"
                                    iconobj = iconfaq
                            End Select


                            itemstr = "<li class=""dd-item"" data-id=""" & id & """ > " &
                       "<div class=""dd-handle dd2-handle"">" &
                       "<i class=""normal-icon " & iconobj & " bigger-130""></i>" &
                         " <i class=""drag-icon icon-move bigger-125""></i></div>" &
                        "<div class=""dd2-content"">" & gettitle(EscapeMySql(title)) & "" &
                           "<div class=""pull-right action-buttons"">" &
                             "<a class=""blue"" data-obj=""" & idobj & """  OnClick=""openwindowEdit('" & Request.QueryString("loadobj") & "'," & idobj & ",'" & HttpUtility.HtmlEncode(title) & "','" & anno & "','" & materia & "')"" href=""#"">" &
                                    "<i class=""icon-pencil bigger-130""></i>" &
                                 "</a> <a class=""red deleteobjectsource"" data-obj=""" & id & """ data-obj=""" & id & """ href=""#"">" &
                                 "<i class=""icon-trash bigger-130""></i></a></div>" &
    "</div><input name=""scormid[]"" type=""hidden"" value=""" & id & """></li>"



                            scriptFinal.Append(itemstr)

                        Case ""
                            folderitem = "<li class=""dd-item dd2-item dd-collapsed " & j & """ data-id=""" & id & """ ><button data-action=""collapse"" type=""button"" style=""display: none;"">Collapse</button><button data-action=""expand"" type=""button"" style=""display: block;"">Expand</button>	 " &
                           "<div class=""dd-handle dd2-handle"">" &
                           "<i class=""normal-icon <<typeicon>>  bigger-130""></i>" &
                           "<i class=""drag-icon icon-move bigger-125""></i></div>" &
                           "<div class=""dd2-content"">" & gettitle(UnEscapeMysql(title)) & "" &
                            "<div class=""pull-right action-buttons"">" &
"</div> " &
                                       "</div>"
                            If newlevel <= level Then

                                If Not newlevel <= level Then
                                    scriptFinal.Append(endfolder)
                                End If

                                scriptFinal.Append(enditem)
                            Else
                                folderitem &= "<ol class=""dd-list"">"
                            End If

                            folderitem = Replace(folderitem, "<<typeicon>>", iconfolder)
                            scriptFinal.Append(folderitem)
                    End Select



                    If newlevel < level Then
                        For g = 1 To level - newlevel
                            scriptFinal.Append(endfolder)
                            scriptFinal.Append(enditem)
                        Next
                    End If


                Next

            Else

                Dim j = 0


                itemstr = "<li class=""dd-item " & j & """ data-id=""0"" > </li>"
                scriptFinal.Append(itemstr)
            End If




        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try



        scriptFinal.Append(endfolder)
        'scorm_list.InnerHtml = scriptadd.ToString
        Response.Write(scriptFinal.ToString)
        Response.End()

    End Sub




    Protected Sub loadtestCourseAvailable()

        '  Dim sqlstring As String = "select idtest as id,title from " & dbtable.test & ""

        Dim sqlstring As String = "select r_name as title ,r_item_id as id From learning_kb_res where objecttype='test' and iduser=" & Session("iduser")

        dt = conn.GetDataTable(sqlstring)

        scriptadd.Append("<ol class=""dd-list"">")


        For Each dr In dt.Rows
            idobj = dr("id").ToString
            titleobj = dr("title").ToString
            objecttype = "test"
            str = "newfrom|" & idobj & "|" & titleobj & "|" & objecttype
            Dim item As String = "<li class=""dd-item"" data-id=""" & str & """ > " &
                       "<div class=""dd-handle dd2-handle"">" &
                       "<i class=""normal-icon icon-user red bigger-130""></i>" &
                         " <i class=""drag-icon icon-move bigger-125""></i></div>" &
                        "<div class=""dd2-content"">" & titleobj & "" &
                          "<input name=""testid[]"" type=""hidden"" value=""" & str & """></div>" &
    "</div></li>"

            scriptadd.Append(item)

        Next





        scriptadd.Append("</ol>")
        Response.Write(scriptadd.ToString)
        Response.End()
    End Sub




    Public Class RootObject

        Public Property ROOT As List(Of ROOT)
            Get
                Return m_ROOT
            End Get
            Set(value As List(Of ROOT))
                m_ROOT = value
            End Set
        End Property


        Private m_ROOT As List(Of ROOT)

    End Class


    Public Class ROOT

        Public Property ID As String

            Get
                Return m_ID
            End Get
            Set(value As String)
                m_ID = value
            End Set
        End Property

        Private m_ID As String = String.Empty



        Public Property CHILDREN As List(Of ROOT)
            Get
                Return m_CHILDREN
            End Get

            Set(value As List(Of ROOT))
                m_CHILDREN = value
            End Set
        End Property


        Private m_CHILDREN As New List(Of ROOT)
    End Class

    Protected Sub btndeleteall_Click(sender As Object, e As EventArgs) Handles btndeleteall.Click
        deleteall(Request.QueryString("idcourse"))
    End Sub





    'Public Class Child

    '    Public Property ID As String =String.Empty 

    '        Get
    '            Return m_ID
    '        End Get
    '        Set(value As String)
    '            m_ID = value
    '        End Set

    '    End Property


    '    Public Property CHILDREN As List(Of Child2)
    '        Get
    '            Return m_CHILDREN
    '        End Get

    '        Set(value As List(Of Child2))
    '            m_CHILDREN = value
    '        End Set
    '    End Property


    '    Private m_CHILDREN As List(Of Child2)

    '    Private m_ID As String =String.Empty 


    'End Class




    'Public Class Child2

    '    Public Property ID As String =String.Empty 

    '        Get
    '            Return m_ID
    '        End Get
    '        Set(value As String)
    '            m_ID = value
    '        End Set

    '    End Property


    '    Private m_ID As String =String.Empty 


    'End Class

End Class