Imports TrainingSchool.SharedRoutines

Public Class WFMakeCategory
    Inherits System.Web.UI.Page

    Dim utility As New SharedRoutines
    Dim rconn As rconnection
    Dim sqlstring As String =String.Empty 

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        rconn = CheckDatabase(rconn)
        If Session("idUser") = 0 OrElse Session("idUser") Is Nothing Then
            Page.ClientScript.RegisterStartupScript(GetType(String), "TreeCSSResultsList", " <script>ExitSession();</script>")
        End If

        Try

            Select Case Request.QueryString("op")
                Case "deletesource"
                    deleteObject(Request.QueryString("id"))
                Case "get"
                    getFormData()
                Case "get,savefaq"
                    Dim idfaq = Request.Form("idcategory")
                    SaveAdditionalCat(idfaq)
            End Select

        Catch ex As Exception

        Finally

        End Try



    End Sub


    Function deleteObject(idcat As String)

        sqlstring = "delete from   learning_category" & " where idcategory=" & idcat

        Try

            rconn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
            LogWrite( ex.ToString)
            Response.Write("Errore nell'inserimento")
        End Try
    Return False 

 End function
    Function SaveAdditionalCat(ByVal idfaq As String)
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
            LogWrite( ex.ToString)
            Response.Write("Errore nell'inserimento")
        End Try
        Response.End()

    Return False 

 End function



    Public Function getFormData()


        Dim sqlstring As String = "select idcategory,path from  learning_category"
        Dim dt As DataTable = Nothing

        dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        Dim content As String = String.Empty

        Dim question As String = String.Empty

        Dim answer As String = String.Empty

        Dim id As String = String.Empty

        Dim i As Integer = 1
        For Each dr In dt.Rows
            Dim cat = dr("path").ToString.Split("/")
            Dim Title = cat(cat.Length - 1)

            id = dr("idcategory")
            content = vbCrLf & "		<div class=""panel panel-default"">"
            content &= vbCrLf & "  	<div class=""panel-heading"">"
            content &= vbCrLf & "		<a href=""#faq-1-" & i & """ data-parent=""#faq_list"" data-toggle=""collapse"" class=""accordion-toggle collapsed"">"
            content &= vbCrLf & "<i class=""icon-chevron-left pull-right"" data-icon-hide=""icon-chevron-down"" data-icon-show=""icon-chevron-left""></i>"
            content &= vbCrLf & "            			<i class=""icon-comment bigger-130""></i>"
            content &= vbCrLf & Title
            content &= vbCrLf & "	</a>"
            content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
            content &= vbCrLf & " <a class=""red deleteobject"" data-obj=""" & id & """  href=""#""><i class=""icon-trash bigger-130""></i></a></div>"
            content &= vbCrLf & "	</div></div>"


            i = i + 1
            Categoria_list.InnerHtml &= content
        Next



        'sqlstring = "select * from learning_faq where idCategory=" & idcategory
        ' CheckConn(rconn)
        'Dim dt As DataTable = nothing


        'dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        'Dim innerhtml As String = String.Empty 


        'Dim j As Integer

        'For Each dr In dt.Rows
        '    Try
        '        Dim h As New HyperLink
        '        Dim div As New HtmlGenericControl("div")
        '        Dim div2 As New HtmlGenericControl("div")
        '        Dim div3 As New HtmlGenericControl("div")
        '        Dim div4 As New HtmlGenericControl("div")
        '        div2.Attributes.Add("class", "col-sm-9")
        '        div.Attributes.Add("class", "form-group")
        '        Dim lb As New HtmlGenericControl("label")
        '        lb.Attributes.Add("for", "fieldquestion-" & j & "-" & dr("idfaq"))
        '        lb.InnerText = "Domanda"
        '        lb.Attributes.Add("class", "col-sm-3 control-label no-padding-right")

        '        Dim lbq As New HtmlGenericControl("label")
        '        lbq.Attributes.Add("for", "fieldanswer-" & j & "-" & dr("idfaq"))
        '        lbq.InnerText = "Risposta"
        '        lbq.Attributes.Add("class", "col-sm-3 control-label no-padding-right")


        '        Dim txt As New TextBox
        '        txt.ID = "fieldquestion-" & j & "-" & dr("idfaq")
        '        txt.Text = dr("question").ToString
        '        txt.Width = 400

        '        Dim txtarea As New TextBox
        '        txtarea.ID = "fieldanswer-" & j & "-" & dr("idfaq")
        '        txtarea.Text = dr("answer").ToString
        '        txtarea.Width = 400
        '        txtarea.TextMode = TextBoxMode.MultiLine

        '        div.Controls.Add(lb)
        '        div3.Controls.Add(lbq)
        '        div.Controls.Add(div2)
        '        div3.Controls.Add(div4)
        '        div2.Controls.Add(txt)
        '        div2.Controls.Add(New HtmlGenericControl("br"))
        '        div4.Controls.Add(txtarea)

        '        'innerhtml &= "<div class=""form-group"">" &
        '        '            "<label class=""col-sm-3 control-label no-padding-right"" for=""form-field-" & dr("translation").ToString & """> " & dr("translation").ToString & " </label>" &
        '        '            " <div class=""col-sm-9"">" &
        '        '            "<input type=""text"" id=""form-field-" & dr("translation").ToString & """ Value=""" & dr("user_entry").ToString & """ placeholder=""" & dr("translation").ToString & """ class=""col-xs-10 col-sm-5"" />" &
        '        '            "</div></div><div class=""space-4""></div>"
        '        j = j + 1
        '        form1.Controls.Add(div)
        '        form1.Controls.Add(div3)
        '        form1.Controls.Add(h)
        '        h.Attributes.Add("Onclick", "deletefaq(" & dr("idfaq") & ")")
        '        h.ID = "fielddelete-" & j & "-" & dr("idfaq")
        '        h.NavigateUrl = "#"
        '        h.Text = "Cancella Domanda"
        '        form1.Controls.Add(New HtmlGenericControl("hr"))
        '    Catch ex As Exception
        '    End Try

        'Next


        'Dim input As New HtmlInputHidden
        'input.ID = "idcategory"
        'input.Value = Request.QueryString("idcategory")

        'form1.Controls.Add(input)


        Return False 

 End function
End Class