
Imports TrainingSchool.SharedRoutines
Imports System.Drawing




Partial Public Class WfCourseRoom
    Inherits System.Web.UI.Page

    Dim sql As String =String.Empty
    Dim dt As DataTable = Nothing
    Dim obj As String = String.Empty
    Dim idreference As String = String.Empty
    Dim h As Hashtable
    Dim percent As String =String.Empty
    Dim objectState As String = String.Empty
    Dim scriptText As StringBuilder
    Dim scriptFinal As StringBuilder
    Dim l As ListItem
    Dim folder1 As String = String.Empty
    Dim idOrg As Integer = 0
    Dim prerequisites As String = String.Empty
    Dim state As String = String.Empty
    Dim link As String = String.Empty
    Dim link2 As String = String.Empty
    Dim endlink As String = String.Empty
    Dim objecttype As String = String.Empty
    Dim templatelink As String = String.Empty
    Dim path As String = String.Empty
    Dim iconitem As String = "icon-file-text orange"
    Dim icontest As String = "icon-edit blue"
    Dim iconscorm As String = "icon-film green"
    Dim iconfaq As String = "icon-question-sign blue"
    Dim iconhtmlpage As String = "icon-file yellow"
    Dim iconpoll As String = "icon-info-sign orange"
    Dim selectreference As String = String.Empty
    Dim selectitem As String = String.Empty
    Dim levpath As String = String.Empty
    Dim levpath2 As String = String.Empty
    Dim first As String = String.Empty
    Dim objUser As LogSession
    Dim utility As SharedRoutines
    Dim rconn As rconnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            rconn = CheckDatabase(rconn)
            objUser = New LogSession
            scriptText = New StringBuilder()
            scriptFinal = New StringBuilder

            Session("Coursename") = Request.QueryString("name")
            Session("idCourse") = Request.QueryString("course")
            objUser.SavelogStatusChange(Session("iduser"), Session("idcourse"), 1)


            utility = New SharedRoutines
            If Not Session("iduser") Is Nothing Then
                If Session("idcourse") <> "" Then


                    If Not Page.IsPostBack And HttpContext.Current.Session("id_enter_course") = Nothing Then
                        objUser.SavelogStatusChange(Session("iduser"), Session("idcourse"), 1)
                        objUser.StartCourselog()
                    End If


                    'If Request.QueryString("course") <> "" Session("idCourse")Then
                    '    utility.  learning_tracksession  (Session("iduser"), Session("idCourse"), )
                    'End If
                    Try
                        percent = objUser.GetSingleStatUser(Session("iduser"), Session("idcourse"))
                        Session("percent") = percent.Split(";")(0)
                        Session("totobj") = percent.Split(";")(2)
                        Session("readobj") = percent.Split(";")(1)
                        parentpage.Text = Session("Coursename")
                        readobj.Text = Session("readobj")
                        totobj.Text = Session("totobj")

                        percentUser.Text = Session("percent")

                    Catch ex As Exception
                    End Try
                    materials.Visible = True

                    view.Visible = True

                    Try
                        lbTimeViewVideo.Text = utility.ConvertSecToDate(utility.SumScormTime(Session("idUser"), Session("idCourse")))
                        lbTotalTimeSession.Text = utility.ConvertSecToDate(objUser.GetUserTotalCourseTime(Session("idUser"), Session("idCourse")))
                        LastObjectView.Text = utility.LastObject(Session("idUser"), Session("idCourse"), idOrg)
                    Catch ex As Exception

                    End Try


                    FillCorso(idOrg)


                Else
                    If Not Session("id_enter_course") Is Nothing Then
                        objUser.EndCourselog(Session.SessionID)
                    End If


                End If
            End If


            If Not Page.IsPostBack Then
                Try
                    Session("minhour") = rconn.GetDataTable("select credits from  learning_course  where idCourse=" & Request.QueryString("course") & "").Rows(0)("credits").ToString
                    Session("myval") = rconn.GetDataTable("select controlvideo from learning_course where idcourse=" & Session("idCourse") & "", CommandType.Text, Nothing).Rows(0)("controlvideo").ToString()


                Catch ex As Exception
                    LogWrite(ex.ToString)
                End Try
            End If


        Catch ex As Exception
            LogWrite(Now & " "   & ex.tostring)
            If Not Session("id_enter_course") Is Nothing Then
                objUser.EndCourselog(Session.SessionID)
            End If
        End Try


    End Sub

    Function GetObjectState()

        Dim j As New Hashtable
        Try

            Dim sqlstring As String = "select idreference,status from learning_commontrack where  idUser=" & Session("iduser") & "  "

            dt = rconn.GetDataTable(sqlstring)

            For Each dr In dt.Rows
                Try

                    j.Add(dr("idreference").ToString, dr("status").ToString)
                Catch ex As Exception

                End Try
            Next

        Catch ex As Exception

        End Try

        Return j
    Return False 

 End function


    Dim vhour As String =String.Empty 

    Dim vmin As String =String.Empty 

    Dim alertmsg As String =String.Empty 


    Protected Sub FillCorso(Optional ByVal selectreference As Integer = 0)



        Dim i As Integer = 0
        Dim scripttextChildren(0) As ListItem
        Dim scripttextChildrenLevNext(0) As ListItem
        Dim title As String =String.Empty 

        Dim isTerminate As Boolean


        Try



            Try
                vhour = Decimal.Truncate(CDbl(utility.SumScormTime(Session("idUser"), Session("idCourse"))).ToString / 3600)
            Catch ex As Exception
                vhour = 0
            End Try


            Try
                vmin = Decimal.Truncate(CDbl((utility.SumScormTime(Session("idUser"), Session("idCourse"))).ToString Mod 3600) / 60)
            Catch ex As Exception
                vmin = 0
            End Try

            alertmsg = "Il tempo totale non è sufficiente per iniziare il test di verifica finale.Il suo tempo di permanenza sulla piattaforma  è di " & vhour & " ore e  " & vmin & " min. La invitiamo a completare la visualizzazione delle parti mancanti per raggiungere il monte ore di " & Session("minhour") & " previsto dal corso"



            'Recupero lo stato degli oggetti
            h = GetObjectState()


            'Analizzo gli oggetti di primo livello

            Dim sqlstring As String = "select * from  learning_organization    where idCourse=" & Session("idCourse") & " and lev=1  order by path asc"

            dt = rconn.GetDataTable(sqlstring)
            If dt.Rows.Count > 0 Then

                scriptText.Append(" var tree_data_2 = {")

                For Each dr In dt.Rows

                    state = ""
                    l = New ListItem
                    isTerminate = dr("isTerminator")
                    prerequisites = dr("prerequisites").ToString
                    idOrg = dr("idOrg").ToString
                    title = EscapeMySql(dr("title"))
                    objecttype = dr("objecttype").ToString
                    path = dr("path").ToString



                    If isTerminate And vhour < Session("minhour") Then
                        link = "<a href=""#"" OnClick=""javascript:alert(\'" & alertmsg & "\')"" > "
                    Else
                        link = "<a href=""#"" OnClick=""openwindow(\'" & objecttype & "\'," & idOrg & ",\'" & EscapeMySql(title).replace("\", "\\\") & "\')"" > "
                    End If


                    endlink = "</a>"


                    'prelevo  lo stato attuale degli oggetti

                    state = SetTreeObject(idOrg, prerequisites, title)


                    Select Case objecttype
                        Case "item"
                            folder1 = vbCrLf & "'" & LCase(path) & "' :{name: '" & link & "<i class=""" & iconitem & """></i> " & title & endlink & state & " ', type: 'item', additionalParameters: { id: '" & idOrg & "' }},"
                        Case "test"
                            folder1 = vbCrLf & "'" & LCase(path) & "' : {name: '" & link & "<i class=""" & icontest & """></i> " & title & endlink & state & " ', type: 'item', additionalParameters: { id: '" & idOrg & "' }},"
                        Case "scormorg"
                            folder1 = vbCrLf & "'" & LCase(path) & "' : {name: '" & link & "<i class=""" & iconscorm & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }},"
                        Case "htmlpage"
                            folder1 = vbCrLf & "'" & LCase(path) & "' :{name: '" & link & "<i class=""" & iconhtmlpage & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }},"
                        Case "faq"
                            folder1 = vbCrLf & "'" & LCase(path) & "' :{name: '" & link & "<i class=""" & iconfaq & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }},"
                        Case ("poll")
                            folder1 = vbCrLf & "'" & LCase(path) & "' : {name: '" & link & "<i class=""" & iconpoll & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }},"
                        Case ""
                            folder1 = vbCrLf & "'" & LCase(path) & "' : {name: '" & title & "', type: 'folder', 'icon-class':'blue'},"

                            If Not GetFolderLeaf(idOrg) Then
                                ReDim Preserve scripttextChildren(scripttextChildren.Length)
                                l.Text = LCase(path)
                                l.Value = vbCrLf & "tree_data_2['" & LCase(path) & "']['additionalParameters'] = { 'children' : {"
                                scripttextChildren(i) = l
                                i = i + 1
                            End If

                    End Select

                    scriptText.Append(folder1)

                    If selectreference = idOrg Then
                        selectitem = folder1
                    End If
                Next




                scriptText = scriptText.Remove(scriptText.Length - 1, 1)
                scriptText.Append(" }")

                scriptText.Append(vbCrLf)
                scriptText.Append(vbCrLf)

                scriptFinal.Append(scriptText.ToString)


                sqlstring = "select max(lev) as MaxLevel from  learning_organization    where idCourse=" & Session("idCourse") & "  order by path asc"

                Dim maxlevel As String = rconn.GetDataTable(sqlstring).Rows(0)("MaxLevel")





                For x As Integer = 2 To maxlevel


                    GetChild(x, scripttextChildren, scripttextChildrenLevNext, selectreference)


                    For i = 0 To scripttextChildren.Length - 1
                        If Not scripttextChildren(i) Is Nothing Then
                            Try
                                scripttextChildren(i).Value = Trim(scripttextChildren(i).Value)
                                scripttextChildren(i).Value = scripttextChildren(i).Value.Remove(scripttextChildren(i).Value.Length - 1, 1)
                                scripttextChildren(i).Value &= vbCrLf & " }}"
                                scriptFinal.Append(scripttextChildren(i).Value)
                            Catch ex As Exception
                            End Try

                        End If

                    Next

                    scripttextChildren = scripttextChildrenLevNext
                    Dim scripttext(0) As ListItem
                    scripttextChildrenLevNext = scripttext

                Next




                scriptFinal.Append(vbCrLf & "      var treeDataSource2 = new DataSourceTree({data: tree_data_2});")
                scriptFinal.Append(vbCrLf)
                ClientScript.RegisterStartupScript(Me.GetType(),
          "treescript", scriptFinal.ToString, True)





                If selectitem <> "" Then
                    Try

                        If (selectitem.IndexOf("id") > -1) Then
                            selectitem = "{" & selectitem.Substring(selectitem.IndexOf("name"), selectitem.IndexOf("}},") - selectitem.IndexOf("name")) & "}};"
                        Else
                            selectitem = "{" & selectitem.Split("{")(1)
                        End If

                    Catch ex As Exception

                    End Try


                    Dim scriptjs As String = String.Empty



                    scriptjs = GetParent(selectitem, selectreference)

                    ClientScript.RegisterStartupScript(Me.GetType(), "treescript2", scriptjs, True)



                Else
                    Dim scriptselect = "function StartObj(){}"

                    ClientScript.RegisterStartupScript(Me.GetType(), "treescript2", scriptselect, True)

                End If

            Else
                Dim scriptselect = "function StartObj(){}"

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "treescript2", scriptselect, True)


                scriptText.Append(" var tree_data_2 = { }")


                scriptText.Append(vbCrLf)
                scriptText.Append(vbCrLf)

                scriptFinal.Append(scriptText.ToString)

                scriptFinal.Append(vbCrLf & "      var treeDataSource2 = new DataSourceTree({data: tree_data_2});")
                scriptFinal.Append(vbCrLf)
                Page.ClientScript.RegisterStartupScript(Me.GetType(),
      "treescript", scriptFinal.ToString, True)

            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(Now & " "   & ex.tostring)
        End Try

    End Sub

    Function GetParent(item As String, idOrg As Integer)


        Dim dtparent As DataTable
        Dim idparent = 1
        Dim scriptselect As String = String.Empty 


        Dim folder As String = String.Empty 


        Dim title As String = String.Empty 


        Dim i = 1


        scriptselect = "function StartObj(){"
        While (idparent > 0)

            Dim sqlstring As String = "select (select title from  learning_organization  a where a.idOrg=b.idparent) as parent,b.idparent,title from  learning_organization  b  where  idOrg=" & idOrg
            dtparent = rconn.GetDataTable(sqlstring)
            idparent = dtparent.Rows(0)("idparent")
            If idparent > 0 Then

                title = EscapeMySql( dtparent.Rows(0)("title"))

                scriptselect &= vbCrLf & " var folder" & i & " = { name: '" & Replace(dtparent.Rows(0)("parent"), "'", "\'") & "', type: 'folder', additionalParameters: { id: '" & idparent & "' } };"
                idOrg = idparent
                i = i + 1
            End If

        End While

        scriptselect &= vbCrLf & " var item1 = " & item

        i = i - 1
        Dim lastfolder As Integer = i
        Dim j As Integer = 0
        For j = i To 1 Step -1

            If j = 1 Then
                scriptselect &= vbCrLf & "folder" & j & ".children = [item1, ];"
                Exit For
            End If

            If j = i - 1 Then
                scriptselect &= vbCrLf & "folder" & i - 1 & ".children = [item1, ];"
                Exit For
            End If
            scriptselect &= vbCrLf & "folder" & j & ".children = [folder" & j - 1 & ", ];"
        Next



        'var folder2 = { name: 'Test Folder 2', type: 'folder', additionalParameters: { id: 'F2' } };
        'var item1 = { name: 'Test Item 1', type: 'item', additionalParameters: { id: 'I1' } };
        'folder1.children = [folder2, ];
        'folder2.children = [item1, ];
        Dim startwith As String =String.Empty 

        If lastfolder = 0 Then
            startwith = "item1"
        Else
            startwith = "folder" & lastfolder
        End If


        scriptselect &= vbCrLf & "selectTreeItem($('#tree2'), " & startwith & ");"
        scriptselect &= "}"
        Return scriptselect
    Return False 

 End function
    Function GetFolderLeaf(idOrg As Integer)

        Dim sqlstring As String = "select idparent from  learning_organization  where idparent=" & idOrg & " and  objecttype != '' "



        dt = rconn.GetDataTable(sqlstring)

        If dt.Rows.Count > 0 Then
            Return False
        Else
            Return True
        End If


    Return False 

 End Function



    Public Function SetTreeObject(idorg As String, prequisites As String, title As String)

        Select Case h(idorg)


            Case "completed"
                state = " <div class=""pull-right action-buttons""><i  OnClick=""openwindowStat(\'" & objecttype & "\'," & idorg & ",\'" & EscapeMySql(title).replace("\", "\\\") & "\'," & Session("iduser") & ")"" Class=""icon-ok green bigger-200""></i></div>"
            Case "incomplete"
                state = " <div Class=""pull-right action-buttons""><i style=""color#CCC000"" Class=""icon-ok yellow bigger-200""></i></div>"
            Case "passed"
                state = " <div Class=""pull-right action-buttons""><i OnClick=""openwindowStat(\'" & objecttype & "\'," & idorg & ",\'" & EscapeMySql(title).replace("\", "\\\") & "\'," & Session("iduser") & ")""  class=""icon-ok green bigger-200""></i></div>"
            Case "failed"
                state = " <div class=""pull-right action-buttons""><i  OnClick=""openwindowStat(\'" & objecttype & "\'," & idorg & ",\'" & EscapeMySql(title).replace("\", "\\\") & "\'," & Session("iduser") & ")"" Class=""icon-ok red bigger-200""></i></div>"
                'endlink = ""
                'link = ""
            Case "ab-initio"
                state = " <div Class=""pull-right action-buttons""><i style=""color:#CCC000""  class=""normal-icon icon-eye-open  bigger-200""></i></div>"
            Case "attempted"
                state = " <div class=""pull-right action-buttons""><i  OnClick=""openwindowStat(\'" & objecttype & "\'," & idorg & ",\'" & EscapeMySql(title).replace("\", "\\\") & "\'," & Session("iduser") & ")"" style=""color:#CCC000""  class=""normal-icon icon-eye-open bigger-200""></i></div>"
            Case Else


                If prerequisites = "" Then

                    state = " <div class=""pull-right action-buttons""><i class=""normal-icon icon-eye-open green bigger-200""></i></div>"
                ElseIf h(prerequisites) = "completed" Or h(prerequisites) = "passed" Then
                    state = " <div class=""pull-right action-buttons""><i class=""normal-icon icon-eye-open green bigger-200""></i></div>"
                ElseIf (h(prerequisites) = "failed") Then
                    state = " <div class=""pull-right action-buttons""><i class=""icon-warning-lock red  bigger-200""></i></div>"
                    'endlink = ""
                    'link = ""
                ElseIf (h(prerequisites) = "attempted") Or h(prerequisites) = "incomplete" Or h(prerequisites) = "ab-initio" Then
                    state = " <div class=""pull-right action-buttons""><i  class=""icon-lock red bigger-200""></i></div>"
                    endlink = ""
                    link = ""
                ElseIf h(prerequisites) Is Nothing Then
                    state = " <div class=""pull-right action-buttons""><i class=""icon-lock red  bigger-200""></i></div>"
                    endlink = ""
                    link = ""
                Else
                    state = " <div class=""pull-right action-buttons""><i class=""normal-icon icon-eye-open green bigger-200""></i></div>"
                End If

        End Select


        Return state
        Return False

    End Function


    Function GetChild(level As Integer, ByRef scripttextChildren() As ListItem, ByRef scripttextChildrenLev2() As ListItem, lastidOrg As Integer)

        Dim sqlstring As String =String.Empty 

        Dim isTerminate As String =String.Empty 

        Dim oldpath As String =String.Empty 

        Dim title As String =String.Empty 

        sqlstring = "select * from  learning_organization   where idCourse=" & Session("idCourse") & " and lev=" & level & " order by path asc"

        dt = rconn.GetDataTable(sqlstring)

        Dim j As Integer = 0
        Dim g As Integer = 0

        For Each dr In dt.Rows
            Try
                l = New ListItem
                isTerminate = dr("isterminator").ToString
                prerequisites = dr("prerequisites").ToString
                idOrg = dr("idOrg").ToString
                title = EscapeMySql(dr("title").ToString)
                objecttype = dr("objecttype").ToString
                path = dr("path").ToString
                oldpath = scripttextChildren(j).Text

                For n = 1 To level
                    levpath &= "/" & LCase(path).Split("/")(n)
                Next


                For n = 1 To level
                    levpath2 &= "/" & LCase(oldpath).Split("/")(n)
                Next


                If (levpath = levpath2) Then
                    j = j
                Else
                    j = j + 1
                End If
                levpath = ""
                levpath2 = ""
                link2 = "" '<span  OnClick=""openwindowStat(\'" & objecttype & "\'," & idOrg & ")"" ><i class=""icon-bar-chart  blue bigger-200 icon-only""></i></span> "

                If isTerminate And vhour < Session("minhour") Then
                    link = "<a href=""#"" OnClick=""javascript:alert(\'" & alertmsg & "\')"" > "
                Else
                    link = "<a href=""#"" OnClick=""openwindow(\'" & objecttype & "\'," & idOrg & ",\'" & EscapeMySql(title).replace("\", "\\\") & "\')"" > "
                End If



                endlink = "</a>"


                'Dim url As String =String.Empty 

                'If objecttype = "scormorg" Then
                '    url = "LMSContent/RunTimePlayer.aspx?reference=" & idOrg
                'Else
                '    url = "WfViewObj.aspx?obj=" & objecttype & "&reference=" & idOrg
                'End If
                'link = "<span class=""button small pop2"" data-bpopup=\'{""contentContainer"":"".content"",""loadUrl"":""" & url & """}\'>"
                'endlink = "</span>"


                If objecttype <> "" Then

                    state = SetTreeObject(idOrg, prerequisites, title)
                End If

                Select Case objecttype
                    Case "item"
                        scripttextChildren(j).Value &= vbCrLf & "'" & LCase(path) & "' : {name: '" & link & "<i class=""" & iconitem & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }}, "
                    Case "test"
                        scripttextChildren(j).Value &= vbCrLf & "'" & LCase(path) & "' : {name: '" & link & "<i class=""" & icontest & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }}, "
                    Case "scormorg"
                        scripttextChildren(j).Value &= vbCrLf & "'" & LCase(path) & "' : {name: '" & link & "<i class=""" & iconscorm & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }}, "
                    Case "htmlpage"
                        scripttextChildren(j).Value &= vbCrLf & "'" & LCase(path) & "' :{name: '" & link & "<i class=""" & iconhtmlpage & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }}, "
                    Case "faq"
                        scripttextChildren(j).Value &= vbCrLf & "'" & LCase(path) & "' : {name: '" & link & "<i class=""" & iconfaq & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }}, "
                    Case "poll"
                        scripttextChildren(j).Value &= vbCrLf & "'" & LCase(path) & "' : {name: '" & link & "<i class=""" & iconpoll & """></i> " & title & endlink & state & "', type: 'item', additionalParameters: { id: '" & idOrg & "' }}, "
                    Case ""
                        scripttextChildren(j).Value &= "'" & LCase(path) & "' : {name: '" & title & "', type: 'folder', 'icon-class':'blue'},"

                        If Not GetFolderLeaf(idOrg) Then
                            ReDim Preserve scripttextChildrenLev2(scripttextChildrenLev2.Length)
                            l.Text = LCase(path)
                            Dim s As String = "/" & LCase(path).Split("/")(1) & "/" & LCase(path).Split("/")(2)

                            l.Value = vbCrLf & "tree_data_2['" & s & "']['additionalParameters']['children']['" & LCase(path) & "']['additionalParameters'] = { 'children' : {"
                            scripttextChildrenLev2(g) = l
                            g = g + 1
                        End If


                End Select


                If lastidOrg = idOrg Then
                    selectitem = scripttextChildren(j).Value
                End If
            Catch ex As Exception
                LogWrite(Now & " "   & ex.tostring)
            End Try

        Next

    Return False 

 End Function




End Class