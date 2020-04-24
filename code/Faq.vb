Imports System.Data
Imports TrainingSchool.SharedRoutines
Imports System.IO

Public Class Faq
    Dim sqlstring As String = String.Empty
    Dim utility As SharedRoutines
    Dim rconn As rconnection
    Sub New()
        utility = New SharedRoutines
        rconn = CheckDatabase(rconn)
    End Sub
    Public Function CreateFaq(title As String, description As String, idfaq As Integer)
        Try
            Dim iduser = HttpContext.Current.Session("iduser")

            If idfaq > 0 Then

                sqlstring = "update learning_kb_res set r_name='" & title & "' where r_item_id=" & idfaq
                rconn.Execute(sqlstring, CommandType.Text, Nothing)
            Else

                sqlstring = " INSERT INTO  learning_faq_cat  (  title ,  description ) " &
                    " VALUES ( '" & EscapeMySql(title) & "', '" & EscapeMySql(description) & "') "


                rconn.Execute(sqlstring, CommandType.Text, Nothing)


                Dim res_idanno As Integer
                Dim pathitem As String
                utility.makecategory(res_idanno, pathitem)

                sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  `r_item_id`, `r_type`,path,lev,idparent, iduser) " &
               " VALUES ( '" & EscapeMySql(title) & "','" & EscapeMySql(title) & "',(select max(idCategory) from learning_faq_cat ), 'faq','" & pathitem & "',3," & res_idanno & "," & iduser & ");"


                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                idfaq = rconn.GetDataTable("select max(r_item_id) as idfaq from learning_kb_res where r_type='faq'", CommandType.Text, Nothing).Rows(0)("idfaq")
            End If


        Catch ex As Exception
            Return "Errore Inserimento Faq:" & ex.Message
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return idfaq



    End Function
    Public Function getFormData(idcategory As String)


        Dim sqlstring As String = "select idfaq,question,answer,sequence from learning_faq where idcategory=" & idcategory & " order by sequence asc"
        Dim dt As DataTable = Nothing

        dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        Dim content As String = String.Empty

        Dim question As String = String.Empty

        Dim sequence As Integer
        Dim answer As String = String.Empty

        Dim id As String = String.Empty

        Dim i As Integer = 1

        Dim contenthtml As String


        For Each dr In dt.Rows

            question = dr("question").ToString
            answer = dr("answer").ToString
            sequence = dr("sequence")
            id = dr("idfaq")
            content = vbCrLf & "	<div class=""ace-spinner"" ><div class=""input-group"">	<div style=""margin-top:20px !important"" class=""panel panel-default"">"
            content &= vbCrLf & "  	<div class=""panel-heading"">"
            content &= vbCrLf & "		<a href=""#faq-1-" & i & """ data-parent=""#faq_list"" data-toggle=""collapse"" class=""accordion-toggle collapsed"">"
            content &= vbCrLf & "<i class=""icon-chevron-left pull-right"" data-icon-hide=""icon-chevron-down"" data-icon-show=""icon-chevron-left""></i>"
            content &= vbCrLf & "            			<i class=""icon-file bigger-130""></i>"
            content &= vbCrLf & "<span id=""question_" & id & """>" & question & "</span>"
            content &= vbCrLf & "	</a>"
            content &= vbCrLf & "     <div class=""pull-right action-buttons"">"
            content &= vbCrLf & " <a class=""green updateobject"" data-obj=""" & id & """  href=""#""><i class=""icon-pencil bigger-130""></i></a><a class=""red deleteobject"" data-obj=""" & id & """  href=""#""><i class=""icon-trash bigger-130""></i></a></div>"
            content &= vbCrLf & "	</div>"
            content &= vbCrLf & "<div class=""panel-collapse collapse"" id=""faq-1-" & i & """ > "
            content &= vbCrLf & "     		<div class=""panel-body"">"
            content &= vbCrLf & "<span id=""answer_" & id & """>" & answer & "</span>"
            content &= vbCrLf & "		</div>"
            content &= vbCrLf & "	</div>"
            content &= vbCrLf & "  </div><div class=""spinner-buttons input-group-btn btn-group-vertical""><button class=""btn spinner-up btn-xs btn-info"" onClick=""orderfaq(" & id & "," & sequence & ", -1)""><i class=""icon-chevron-up""></i></button><button class=""btn spinner-down btn-xs btn-info"" onClick=""orderfaq(" & id & "," & sequence & ", 1)""><i class=""icon-chevron-down""></i></button></div>"
            content &= vbCrLf & "  </div></div><div class=""space-6""></div>"
            contenthtml &= content
            i = i + 1

        Next

        Return contenthtml

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

    End Function

    Friend Function CreateFaq(title As String, content As String) As String
        Throw New NotImplementedException()
    End Function
End Class