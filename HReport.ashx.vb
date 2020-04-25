Imports System
Imports System.Drawing
Imports System.IO
Imports System.Web
Imports System.Web.UI.ClientScriptManager
Imports ICSharpCode.SharpZipLib.Zip
Imports TrainingSchool.SharedRoutines
Imports SelectPdf

Public Class HReport
    Implements IHttpHandler, IReadOnlySessionState

    Dim utility As SharedRoutines
    Dim logsession As LogSession
    Dim title As String = String.Empty
    Dim description As String = String.Empty
    Dim id As String = String.Empty
    Dim msg As String = String.Empty
    Dim sqlstring As String
    Dim mycontext As HttpContext
    Dim iduser As Integer
    Dim idsessione As Integer
    Dim conn As rconnection
    Dim dt As DataTable
    Public Sub Processrequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim result As String = String.Empty

        Try
            SharedRoutines.SetAcl(New List(Of String)(New String() {"1", "2", "8"}))
            mycontext = context

            utility = New SharedRoutines
            logsession = New LogSession()
            conn = CheckDatabase(conn)

            If mycontext.Session("iduser") Is Nothing Then
                msg = "Session Terminata"
            End If

            iduser = context.Request.QueryString("iduser")
            idsessione = context.Request.QueryString("idsessione")

            Select Case mycontext.Request.QueryString("op")
                Case "modreport"
                    Select Case mycontext.Request.QueryString("oper")

                        Case "getstatusercourse"

                            getstatusercourse(iduser, idsessione)


                        Case "getreportcourse"
                            getReportCourse(mycontext.Request.QueryString("tipo"), mycontext.Request.QueryString("idsessione"), mycontext.Request.QueryString("iduser"), "", mycontext.Request.QueryString("nomesessione"), mycontext.Request.QueryString("fullname"))
                        Case "getreportzip"
                            DownloadZipToBrowserReport()
                        Case "getvalutazionibyiduser"
                            getvalutazionibyiduser()
                        Case "getvalutazioni"
                            getvalutazioni(mycontext.Request.QueryString("idcourse"))

                        Case "getvalutazionibyuser"
                            getvalutazionibyuser(mycontext.Request.QueryString("iduser"))
                        Case "getreportvalutazioni"
                            getreportvalutazioni(mycontext.Request.QueryString("iduser"), mycontext.Request.QueryString("idcourse"))
                        Case "downloadexcel"
                            downloadreport()

                    End Select
            End Select


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.Message)
        End Try

        mycontext.Response.Write(msg)
        mycontext.Response.Flush() ' Sends all currently buffered output to the client.
        mycontext.Response.SuppressContent = True  ' Gets or sets a value indicating whether to send HTTP content to the client.
        mycontext.ApplicationInstance.CompleteRequest()


    End Sub


#Region "report"


    Private Sub DownloadZipToBrowserReport()

        Dim selectmonth As String = mycontext.Request.QueryString("data")
        Dim iduser = mycontext.Request.QueryString("iduser")

        If selectmonth = 0 Then
            sqlstring = "select datastart,dataend,b.iduser,c.firstname,c.lastname,a.nomesessione,a.id,tipo,ifwrite from (aula_sessioni a join aula_prenotazioni b on a.id=b.idsessione) join core_user c on c.idst=b.iduser where b.iduser=" & iduser & " order by id asc"

        Else
            sqlstring = "select datastart,dataend,b.iduser,c.firstname,c.lastname,a.nomesessione,a.id,tipo,ifwrite from (aula_sessioni a join aula_prenotazioni b on a.id=b.idsessione) join core_user c on c.idst=b.iduser where b.iduser=" & iduser & " And  datastart >= '" & Year(Now) & "-" & selectmonth & "-01 00:00:00' and dataend <= '" & Year(Now) & "-" & selectmonth & "-31 23:59:59' order by id asc"

            End If
        Dim dt As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        Dim fullname As String = FormattaNominativo(Replace(dt.Rows(0)("firstname") & " " & dt.Rows(0)("lastname"), "'", ""))

        mycontext.Response.ContentType = "application/zip"
        mycontext.Response.AppendHeader("content-disposition", "attachment; filename=""" & fullname & " - " & Year(Now) & " - " & MonthName(selectmonth) & ".zip""")
        mycontext.Response.CacheControl = "Private"
        mycontext.Response.Cache.SetExpires(DateTime.Now.AddMinutes(3))
        Dim buffer As Byte() = New Byte(4095) {}
        Dim zipOutputStream As ZipOutputStream = New ZipOutputStream(mycontext.Response.OutputStream)
        zipOutputStream.SetLevel(3)

        For Each dr In dt.Rows


            Dim filename As String = mycontext.Server.MapPath("report") & "\" & Year(Now) & iduser & dr("id") & "_" & fullname & ".pdf"

            Dim pathfile = mycontext.Server.MapPath("report") & "/" & filename



            Try
                Select Case dr("tipo")

                    Case "WEBINAR"
                        getreportMeetDocument(dr("tipo"), dr("id"), dr("iduser"), filename, dr("nomesessione"), fullname, True)
                    Case "LEZIONE"
                        getreportDocument(dr("tipo"), dr("id"), dr("iduser"), filename, dr("nomesessione"), fullname, True)
                    Case "CORSO"
                        getReportCourse(dr("tipo"), dr("id"), dr("iduser"), filename, dr("nomesessione"), fullname, True)
                    Case "VERIFICA ORALE"
                        getreportMeet(dr("tipo"), dr("id"), dr("iduser"), filename, dr("nomesessione"), fullname, True)
                    Case "VERIFICA SCRITTA"
                        If (dr("ifwrite") = 1) Then
                            filename = getReportMeetCourse(dr("tipo"), dr("id"), dr("iduser"), filename, dr("nomesessione"), fullname, True)
                        Else
                            getreportMeetDocument(dr("tipo"), dr("id"), dr("iduser"), idsessione, fullname, True)
                        End If

                End Select


                Dim fs As Stream = File.OpenRead(filename)
                Dim entry As ZipEntry = New ZipEntry(ZipEntry.CleanName(filename))
                entry.Size = fs.Length
                zipOutputStream.PutNextEntry(entry)
                Dim count As Integer = fs.Read(buffer, 0, buffer.Length)

                While count > 0
                    zipOutputStream.Write(buffer, 0, count)
                    count = fs.Read(buffer, 0, buffer.Length)

                    If Not mycontext.Response.IsClientConnected Then
                        Exit While
                    End If

                    mycontext.Response.Flush()
                End While

                fs.Close()
            Catch ex As Exception
            End Try
        Next

        zipOutputStream.Close()
        mycontext.Response.Flush()
        mycontext.Response.[End]()
    End Sub
    Sub downloadreport()

        Dim idcourse = mycontext.Request.QueryString("idcourse")
        Dim iduser = mycontext.Request.QueryString("iduser")
        Try
            If iduser <> "" Then
                sqlstring = "Select firstname, lastname, concat(coalesce((Select title from learning_organization where idorg=g.idparent),''),' -> ',g.title ) as test, b.number_of_attempt,a.score,a.number_time,date_format(a.date_begin,'%d/%m/%y %h:%mm') as 'data inizio' ,date_format(a.date_end,'%d/%m/%y %h:%mm') as 'data fine' ,b.score as lastscore,d.score_max,d.point_required from (((learning_testtrack_times a right join learning_testtrack b on a.idtrack=b.idtrack) join core_user c on b.iduser=c.idst ) join learning_test d on d.idtest=b.idtest ) join learning_organization g on g.idresource=b.idtest where objecttype='test' and b.iduser=" & iduser & " and g.idcourse=" & idcourse & ""

            Else

                sqlstring = "select firstname,lastname,concat(coalesce((select title from learning_organization where idorg=g.idparent),''),' -> ',g.title ) as test, b.number_of_attempt,a.score,a.number_time,date_format(a.date_begin,'%d/%m/%y %h:%mm') as 'data inizio' ,date_format(a.date_end,'%d/%m/%y %h:%mm') as 'data fine' ,b.score as lastscore,d.score_max,d.point_required from (((learning_testtrack_times a right join learning_testtrack b on a.idtrack=b.idtrack) join core_user c on b.iduser=c.idst ) join learning_test d on d.idtest=b.idtest ) join learning_organization g on g.idresource=b.idtest where objecttype='test' and g.idcourse=" & idcourse & ""

            End If

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            Dim filename As String

            If iduser <> "" Then
                filename = mycontext.Server.MapPath("lms") & "/report_" & idcourse & "-" & SharedRoutines.EscapeSql(dt.Rows(0)("firstname") & " " & dt.Rows(0)("lastname")) & ".xls"
            Else
                filename = mycontext.Server.MapPath("temp") & "/report_" & idcourse & ".xls"
            End If

            Dim g = Guid.NewGuid().ToString
            Dim fs As System.IO.FileStream
            Dim xtw As System.Xml.XmlTextWriter

            dt.TableName = "report"

            fs = New System.IO.FileStream(filename, System.IO.FileMode.Create)

            xtw = New System.Xml.XmlTextWriter(fs, System.Text.Encoding.Unicode)

            xtw.WriteProcessingInstruction("xml", "version='1.0'")

            xtw.WriteProcessingInstruction("mso-application", "progid='excel.sheet'")

            dt.WriteXml(xtw)

            xtw.Close()

            msg = "/temp/" & System.IO.Path.GetFileName(filename)

        Catch ex As Exception

            LogWrite(ex.ToString)

            msg = "nessuna risposta"

        End Try

    End Sub

    Public Sub getreportvalutazioni(iduser, idcourse)

        Try

            sqlstring = "select a.idorg as id,concat(coalesce((select title from learning_organization where idorg=a.idparent),''),' -> ',a.title ) as test,b.score,b.number_of_attempt,b.date_attempt,b.score_status from ((learning_organization a join learning_testtrack b on a.idorg=b.idreference)) where a.idcourse=" & idcourse & " and b.iduser=" & iduser & " and objecttype='test' order by path asc;"

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            mycontext.Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub


    Sub getreporthtmldocument(ByRef strhtml As String, idsessione As Integer, iduser As Integer)
        sqlstring = "select id,nomedocupload,idsessione,iduser,firstname,lastname,date_hit from aula_documentstudents a join aula_document b on a.id=b.iddoc where b.idsessione=" & idsessione & " and   a.iduser=" & iduser & " order by id asc"

        Dim dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        strhtml &= "<table><tr><td>Documento</td><td>Date Hit</b>"

        For Each dr In dt.Rows

            strhtml &= "<tr><td>" & dr("nomedoc") & "</td><td>" & dr("date_hit") & "</td></tr>"

        Next
    End Sub


    Sub getreporthtmlMeet(ByRef strhtml As String, idsessione As Integer, iduser As Integer)
        Dim sqlstringFirst = " Select SUM(TIME_TO_SEC(TIMEDIFF( datejoin,dateleft))) As totaltime from aula_sessionilog where idsessione=" & idsessione & " and iduser=" & iduser & " order by datejoin asc "
        Dim dt As DataTable = conn.GetDataTable(sqlstringFirst, CommandType.Text, Nothing)
        Dim tot_time As String = dt.Rows(0)("totaltime").ToString


        strhtml &= "<br>Ore permanenza in piattaforma: <b>" & utility.ConvertSecToDate(tot_time) & "</b>"

    End Sub


    Sub getreporthtmlCourse(ByVal iduser As Integer, ByVal idsessione As Integer, ByVal idcourse As Integer, ByRef strhtml As String)

        Try


            Dim sqlstringFirst = " Select SUM(TIME_TO_SEC(TIMEDIFF( lasttime,entertime))) As totaltime from learning_tracksession where idCourse=" & idcourse & "  And idUser=" & iduser
            Dim dt As DataTable = conn.GetDataTable(sqlstringFirst, CommandType.Text, Nothing)
            Dim tot_time As String = dt.Rows(0)("totaltime").ToString

            strhtml &= "<br>Ore permanenza in piattaforma: <b>" & utility.ConvertSecToDate(tot_time) & "</b>"
            strhtml &= "<br>Numero connessioni: <b>" & utility.getconnection(iduser, idcourse) & "</b>"
            strhtml &= "<br>Oggetti completati: <b>" & utility.getmateriale(iduser, idcourse) & "</b>"
            strhtml &= "<br>Totale visione videocorsi: <b>" & utility.ConvertSecToDate(logsession.getUserTotalVideocourseSec(iduser, idcourse)) & "</b>"


            sqlstring = "select (select title from learning_organization where idorg=a.idparent) as modulo ,title,b.last_complete,status,b.idreference,b.objecttype,b.firstattempt from learning_organization a join learning_commontrack b on a.idorg=idreference where  idcourse=" & idcourse & " And b.idUser=" & iduser & " order by b.firstattempt asc"

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            If dt.Rows.Count > 0 Then
                strhtml &= vbCrLf

                strhtml &= "<h2>Dettaglio Oggetti Didattici</h2>"
                strhtml &= "<table width=""1100px"" border=1 > <tr style=""font-weight: bold"">"
                strhtml &= "<td>Oggetto didattico</td><td>Data accesso</td><td>Data completamento</td><td>Stato</td><td>Tempo permanenza</td><td>Voto</td></tr>"
                strhtml &= vbCrLf


                For Each row As DataRow In dt.Rows
                    Try
                        If (row("objecttype") = "test") Then
                            strhtml &= "<tr><td>" & row("modulo") & " - " & row("title") & "</td><td>" & row("firstattempt").ToString & "</td><td>" & row("last_complete").ToString & "</td><td>" & row("status") & "</td><td></td><td>" & getvototest(iduser, row("idreference"), "") & "</td></tr>"
                        ElseIf row("objecttype") = "scormorg" Then

                            sqlstring = "select a.title,total_time,duration,(select title from learning_organization where idorg=a.idparent) as modulo from ((learning_organization a left join learning_scorm_tracking b on a.idorg=b.idreference) join  learning_Scorm_organizations c on a.idresource=c.idscorm_organization) join learning_Scorm_package d on d.idscorm_package=c.idscorm_package where a.objecttype='scormorg' and a.idorg=" & row("idreference") & " and idcourse=" & idcourse & " and b.idUser=" & iduser & " order by a.path asc"
                            Dim totaltime As String = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("total_time")


                            strhtml &= "<tr><td>" & row("modulo") & " - " & row("title") & "</td><td>" & row("firstattempt").ToString & "</td><td>" & row("last_complete").ToString & "</td><td>" & row("status") & "</td><td>" & totaltime & "</td><td></td></tr>"

                        Else

                            strhtml &= "<tr><td>" & row("modulo") & " - " & row("title") & "</td><td>" & row("firstattempt").ToString & "</td><td>" & row("last_complete").ToString & "</td><td>" & row("status") & "</td><td><td></td></tr>"

                        End If
                    Catch ex As Exception
                        LogWrite(ex.ToString)
                    End Try
                    strhtml &= vbCrLf
                Next
                strhtml &= "</table>"




                strhtml &= "<h2>Numero Connessioni</h2>"
                strhtml &= "<table width=""1100px"" border=1 > <tr style=""font-weight: bold"">"
                strhtml &= "<td>N.</td><td>Inizio Sessione</td><td>Fine Sessione</td><td>Durata Sessione</td><td>N. Operazioni</td><td>IP</td></tr>"
                strhtml &= vbCrLf
            End If


            sqlstring = "select identer as id, entertime,lasttime,numop,ip_address,(select  CONCAT(' ',SUM(TIME_TO_SEC(TIMEDIFF(lasttime,entertime)))) from learning_tracksession  where identer=id) as duration from learning_tracksession where entertime != lasttime and  idcourse=" & idcourse & " and iduser=" & iduser & "  order by entertime asc "
            Dim i As Integer = 1

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    Try

                        strhtml &= " <tr><td> " & i & "</td><td>" & Convert.ToDateTime(row("entertime")) & "</td><td>" & Convert.ToDateTime(row("lasttime")) & "</td><td>" & utility.ConvertSecToDate(row("duration")) & "</td><td>" & row("numop") & "</td><td>" & row("ip_address") & "</td></tr>"
                        i = i + 1

                    Catch ex As Exception
                        LogWrite(ex.ToString)
                    End Try
                    strhtml &= vbCrLf
                Next
            End If
            strhtml &= "</table>"
        Catch ex As Exception

        End Try

    End Sub


    Function getreportMeetDocument(tipo As String, idsessione As Integer, iduser As Integer, filename As String, nomesessione As String, fullname As String, Optional savefile As Boolean = False)


        sqlstring = "select  * from aula_sessionilog where idsessione=" & idsessione & " and iduser=" & iduser & " order by datejoin asc "
        Dim dr As DataRow = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)
        Dim strhtml As String = String.Empty
        Dim sb As New StringBuilder


        Try
            strhtml &= "<center><h1>REPORT " & tipo & " N." & Year(Now) & iduser & idsessione & "</h1></center><br>"
            strhtml &= "Utente: <b >" & fullname & "</b><br>"
            strhtml &= "Corso: <b >" & nomesessione & "</b><br>"

            getreporthtmldocument(strhtml, idsessione, iduser)

            getreporthtmlMeet(strhtml, idsessione, iduser)

        Catch ex As Exception

        End Try
        sb.Append(strhtml)





        Try

            Dim converter As New HtmlToPdf()
            converter.Options.PdfPageSize = PdfPageSize.A4
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait
            converter.Options.MarginLeft = System.Configuration.ConfigurationSettings.AppSettings("ml")
            converter.Options.MarginRight = System.Configuration.ConfigurationSettings.AppSettings("mr")
            converter.Options.MarginBottom = System.Configuration.ConfigurationSettings.AppSettings("mb")
            converter.Options.MarginTop = System.Configuration.ConfigurationSettings.AppSettings("mt")



            ' create a new pdf document converting the html string of the page
            '   If Orientation = 1 Then
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape
            ' End If



            converter.Options.DisplayHeader = 1
            converter.Header.DisplayOnEvenPages = 2
            converter.Header.Height = System.Configuration.ConfigurationSettings.AppSettings("hh") + 30

            Dim headerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "header"), "./")
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
            converter.Header.Add(headerHtml)


            converter.Options.DisplayFooter = 1
            converter.Footer.DisplayOnEvenPages = 2
            converter.Footer.Height = System.Configuration.ConfigurationSettings.AppSettings("hf")

            Dim footerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "footer"), "./")
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit

            converter.Footer.Add(footerHtml)


            Dim Text As New PdfTextSection(0, 10, " {page_number}/{total_pages} ", New Font("Times New Roman", 12))
            Text.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00309c")
            Text.HorizontalAlign = PdfTextHorizontalAlign.Right
            converter.Footer.Add(Text)

            Dim doc As SelectPdf.PdfDocument = converter.ConvertHtmlString(sb.ToString)
            If (savefile) Then

                doc.Save(filename)
                doc.Close()
                Return filename
            Else
                doc.Save(mycontext.Response, True, "./" & IO.Path.GetFileName(fullname) & ".pdf")
                doc.Close()
            End If

            ' close pdf document
            doc.Close()


        Catch ex As Exception
            LogWrite("report: " & ex.ToString)
        End Try


    End Function
    Sub getreportDocument(tipo As String, idsessione As Integer, iduser As Integer, filename As String, nomesessione As String, fullname As String, Optional savefile As Boolean = False)


        Dim options = ""


        Dim strhtml As String = String.Empty
        Dim sb As New StringBuilder


        Try
            strhtml &= "<center><h1>REPORT " & tipo & " N." & Year(Now) & iduser & idsessione & "</h1></center><br>"
            strhtml &= "Utente: <b >" & fullname & "</b><br>"
            strhtml &= "Corso: <b >" & nomesessione & "</b><br>"

            getreporthtmldocument(strhtml, idsessione, iduser)

        Catch ex As Exception

        End Try
        sb.Append(strhtml)

        sb.Append("</table>")

        Try

            Dim converter As New HtmlToPdf()
            converter.Options.PdfPageSize = PdfPageSize.A4
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait
            converter.Options.MarginLeft = System.Configuration.ConfigurationSettings.AppSettings("ml")
            converter.Options.MarginRight = System.Configuration.ConfigurationSettings.AppSettings("mr")
            converter.Options.MarginBottom = System.Configuration.ConfigurationSettings.AppSettings("mb")
            converter.Options.MarginTop = System.Configuration.ConfigurationSettings.AppSettings("mt")



            ' create a new pdf document converting the html string of the page
            '   If Orientation = 1 Then
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape
            ' End If



            converter.Options.DisplayHeader = 1
            converter.Header.DisplayOnEvenPages = 2
            converter.Header.Height = System.Configuration.ConfigurationSettings.AppSettings("hh") + 30

            Dim headerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "header"), "./")
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
            converter.Header.Add(headerHtml)


            converter.Options.DisplayFooter = 1
            converter.Footer.DisplayOnEvenPages = 2
            converter.Footer.Height = System.Configuration.ConfigurationSettings.AppSettings("hf")

            Dim footerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "footer"), "./")
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit

            converter.Footer.Add(footerHtml)


            Dim Text As New PdfTextSection(0, 10, " {page_number}/{total_pages} ", New Font("Times New Roman", 12))
            Text.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00309c")
            Text.HorizontalAlign = PdfTextHorizontalAlign.Right
            converter.Footer.Add(Text)

            Dim doc As SelectPdf.PdfDocument = converter.ConvertHtmlString(sb.ToString)
            If (savefile) Then

                doc.Save(filename)
                doc.Close()

            Else
                doc.Save(mycontext.Response, True, "./" & IO.Path.GetFileName(filename) & ".pdf")
                doc.Close()
            End If

            ' close pdf document
            doc.Close()


        Catch ex As Exception
            LogWrite("report: " & ex.ToString)
        End Try

    End Sub
    Function getReportMeetCourse(tipo As String, idsessione As Integer, iduser As Integer, filename As String, nomesessione As String, fullname As String, Optional savefile As Boolean = False)
        Dim sb As New StringBuilder()
        Dim idcourse As Integer


        sqlstring = "select idcourse from aula_course a join aula_sessioni b on a.idcourse=b.idcourse where b.idsessione=" & idsessione


        dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        For Each drcourse In dt.Rows
            idcourse = drcourse("idcourse")
            sqlstring = "select code,name,date_inscr,date_complete,learning_courseuser.status,learning_courseuser.idCourse,learning_courseuser.iduser from learning_courseuser join learning_course on learning_course.idCourse=learning_courseuser.idCourse where learning_courseuser.idcourse=" & idcourse & " and learning_courseuser.iduser=" & iduser & " order by date_inscr asc "
            Dim dr As DataRow = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)
            Dim strhtml As String = String.Empty



            strhtml &= "<center><h1>REPORT " & tipo & " N." & Year(Now) & iduser & idsessione & "</h1></center><br>"
            strhtml &= "Utente: <b >" & fullname & "</b><br>"
            strhtml &= "Corso: <b >" & nomesessione & "</b><br>"
            strhtml &= "Data Iscrizione:<b> " & dr("data_prenotazione").ToString & "</b><br>"
            strhtml &= "Data Completamento:<b> " & dr("date_complete").ToString & "</b><br>"
            strhtml &= "Ultimo Accesso al corso:<b> " & utility.getLastCourseAccess(idcourse, iduser, conn) & "</b><br>"
            strhtml &= "Ultimo Accesso alla piattaforma:<b> " & utility.getLastAccess(iduser, conn) & "</b><br>"
            strhtml &= "Stato:<b> " & utility.GetPercentuale(iduser, idcourse, conn, dr("status").ToString) & "</b>"


            getreporthtmlCourse(iduser, idsessione, idcourse, strhtml)

            getreporthtmlMeet(iduser, idsessione, strhtml)
        Next
        Try

            Dim converter As New HtmlToPdf()
            converter.Options.PdfPageSize = PdfPageSize.A4
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait
            converter.Options.MarginLeft = System.Configuration.ConfigurationSettings.AppSettings("ml")
            converter.Options.MarginRight = System.Configuration.ConfigurationSettings.AppSettings("mr")
            converter.Options.MarginBottom = System.Configuration.ConfigurationSettings.AppSettings("mb")
            converter.Options.MarginTop = System.Configuration.ConfigurationSettings.AppSettings("mt")



            ' create a new pdf document converting the html string of the page
            '   If Orientation = 1 Then
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape
            ' End If



            converter.Options.DisplayHeader = 1
            converter.Header.DisplayOnEvenPages = 2
            converter.Header.Height = System.Configuration.ConfigurationSettings.AppSettings("hh") + 30

            Dim headerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "header"), "./")
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
            converter.Header.Add(headerHtml)


            converter.Options.DisplayFooter = 1
            converter.Footer.DisplayOnEvenPages = 2
            converter.Footer.Height = System.Configuration.ConfigurationSettings.AppSettings("hf")

            Dim footerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "footer"), "./")
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit

            converter.Footer.Add(footerHtml)


            Dim Text As New PdfTextSection(0, 10, " {page_number}/{total_pages} ", New Font("Times New Roman", 12))
            Text.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00309c")
            Text.HorizontalAlign = PdfTextHorizontalAlign.Right
            converter.Footer.Add(Text)

            Dim doc As SelectPdf.PdfDocument = converter.ConvertHtmlString(sb.ToString)
            If (savefile) Then

                doc.Save(filename)
                doc.Close()
                Return ""
            Else
                doc.Save(mycontext.Response, True, "./" & IO.Path.GetFileName(fullname) & ".pdf")
                doc.Close()
            End If

            ' close pdf document
            doc.Close()


        Catch ex As Exception
            LogWrite("report: " & ex.ToString)
        End Try


    End Function

    Function getreportMeet(tipo As String, idsessione As Integer, iduser As Integer, filename As String, nomesessione As String, fullname As String, Optional savefile As Boolean = False)


        sqlstring = "select  * from aula_sessionilog where idsessione=" & idsessione & " and iduser=" & iduser & " order by datejoin asc "
        Dim dr As DataRow = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)
        Dim strhtml As String = String.Empty
        Dim sb As New StringBuilder


        Try
            strhtml &= "<center><h1>REPORT " & tipo & " N." & Year(Now) & iduser & idsessione & "</h1></center><br>"
            strhtml &= "Utente: <b >" & fullname & "</b><br>"
            strhtml &= "Corso: <b >" & nomesessione & "</b><br>"


            getreporthtmlMeet(strhtml, idsessione, iduser)

        Catch ex As Exception

        End Try
        sb.Append(strhtml)





        Try

            Dim converter As New HtmlToPdf()
            converter.Options.PdfPageSize = PdfPageSize.A4
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait
            converter.Options.MarginLeft = System.Configuration.ConfigurationSettings.AppSettings("ml")
            converter.Options.MarginRight = System.Configuration.ConfigurationSettings.AppSettings("mr")
            converter.Options.MarginBottom = System.Configuration.ConfigurationSettings.AppSettings("mb")
            converter.Options.MarginTop = System.Configuration.ConfigurationSettings.AppSettings("mt")



            ' create a new pdf document converting the html string of the page
            '   If Orientation = 1 Then
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape
            ' End If



            converter.Options.DisplayHeader = 1
            converter.Header.DisplayOnEvenPages = 2
            converter.Header.Height = System.Configuration.ConfigurationSettings.AppSettings("hh") + 30

            Dim headerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "header"), "./")
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
            converter.Header.Add(headerHtml)


            converter.Options.DisplayFooter = 1
            converter.Footer.DisplayOnEvenPages = 2
            converter.Footer.Height = System.Configuration.ConfigurationSettings.AppSettings("hf")

            Dim footerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "footer"), "./")
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit

            converter.Footer.Add(footerHtml)


            Dim Text As New PdfTextSection(0, 10, " {page_number}/{total_pages} ", New Font("Times New Roman", 12))
            Text.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00309c")
            Text.HorizontalAlign = PdfTextHorizontalAlign.Right
            converter.Footer.Add(Text)

            Dim doc As SelectPdf.PdfDocument = converter.ConvertHtmlString(sb.ToString)
            If (savefile) Then

                doc.Save(filename)
                doc.Close()
                Return filename
            Else
                doc.Save(mycontext.Response, True, "./" & IO.Path.GetFileName(filename) & ".pdf")
                doc.Close()
            End If

            ' close pdf document
            doc.Close()


        Catch ex As Exception
            LogWrite("report: " & ex.ToString)
        End Try


    End Function
    Function getReportCourse(tipo As String, idsessione As Integer, iduser As Integer, filename As String, nomesessione As String, fullname As String, Optional savefile As Boolean = False)


        If filename = "" Then
            filename = mycontext.Server.MapPath("report") & "\" & Year(Now) & iduser & idsessione & "_" & fullname & ".pdf"

        End If

        Dim sqlstring = "select idcourse from aula_course where idsessione=" & idsessione & ""
        Dim idcourse As Integer = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("idcourse")

        Dim sb As New StringBuilder()

        sqlstring = "select code,name,date_inscr,date_complete,learning_courseuser.status,learning_courseuser.idCourse,learning_courseuser.iduser from learning_courseuser join learning_course on learning_course.idCourse=learning_courseuser.idCourse where learning_courseuser.idcourse=" & idcourse & " and learning_courseuser.iduser=" & iduser & " order by date_inscr asc "
        Dim dr As DataRow = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)
        Dim strhtml As String = String.Empty



        strhtml &= "<center><h1>REPORT " & tipo & " N." & Year(Now) & iduser & idsessione & "</h1></center><br>"
        strhtml &= "Utente: <b >" & fullname & "</b><br>"
        strhtml &= "Corso: <b >" & nomesessione & "</b><br>"
        strhtml &= "Data Iscrizione:<b> " & dr("date_inscr").ToString & "</b><br>"
        strhtml &= "Data Completamento:<b> " & dr("date_complete").ToString & "</b><br>"
        strhtml &= "Ultimo Accesso al corso:<b> " & utility.getLastCourseAccess(idcourse, iduser, conn) & "</b><br>"
        strhtml &= "Ultimo Accesso alla piattaforma:<b> " & utility.getLastAccess(iduser, conn) & "</b><br>"
        strhtml &= "Stato:<b> " & utility.GetPercentuale(iduser, idcourse, conn, dr("status").ToString) & "</b>"


        getreporthtmlCourse(iduser, idsessione, idcourse, strhtml)



        Try
            sb.Append(strhtml)
            Dim converter As New HtmlToPdf()
            converter.Options.PdfPageSize = PdfPageSize.A4
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait
            converter.Options.MarginLeft = System.Configuration.ConfigurationSettings.AppSettings("ml")
            converter.Options.MarginRight = System.Configuration.ConfigurationSettings.AppSettings("mr")
            converter.Options.MarginBottom = System.Configuration.ConfigurationSettings.AppSettings("mb")
            converter.Options.MarginTop = System.Configuration.ConfigurationSettings.AppSettings("mt")



            ' create a new pdf document converting the html string of the page
            '   If Orientation = 1 Then
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape
            ' End If



            converter.Options.DisplayHeader = 1
            converter.Header.DisplayOnEvenPages = 2
            converter.Header.Height = System.Configuration.ConfigurationSettings.AppSettings("hh") + 30

            Dim headerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "header"), "./")
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
            converter.Header.Add(headerHtml)


            converter.Options.DisplayFooter = 1
            converter.Footer.DisplayOnEvenPages = 2
            converter.Footer.Height = System.Configuration.ConfigurationSettings.AppSettings("hf")

            Dim footerHtml As PdfHtmlSection = New PdfHtmlSection(utility.Getmailformat(True, "footer"), "./")
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit

            converter.Footer.Add(footerHtml)


            Dim Text As New PdfTextSection(0, 10, " {page_number}/{total_pages} ", New Font("Times New Roman", 12))
            Text.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00309c")
            Text.HorizontalAlign = PdfTextHorizontalAlign.Right
            converter.Footer.Add(Text)

            Dim doc As SelectPdf.PdfDocument = converter.ConvertHtmlString(sb.ToString)
            If (savefile) Then

                doc.Save(filename)
                doc.Close()
                Return ""
            Else
                doc.Save(mycontext.Response, True, "./" & IO.Path.GetFileName(filename) & ".pdf")
                doc.Close()
            End If

            ' close pdf document
            doc.Close()


        Catch ex As Exception
            LogWrite("report: " & ex.ToString)
        End Try


    End Function


    Function getvoto(iduser As Integer, idcorso As String, convenzione As String)
        Try


            Dim sqlstring As String = "select score_max,score from  learning_testtrack a  join learning_test b on a.idtest=b.idtest where b.idtest=(select idresource from learning_organization where isterminator=1 and idcourse=" & idcorso & ") and iduser=" & iduser
            Dim dr As DataRow = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)

            Return dr("score") & "/" & dr("score_max")
        Catch ex As Exception
            Return ""
        End Try

    End Function
    Function getvototest(iduser As Integer, idreference As String, convenzione As String)
        Try


            Dim sqlstring As String = "select score_max,score from  learning_testtrack a  join learning_test b on a.idtest=b.idtest where a.idreference=" & idreference & " and iduser=" & iduser
            Dim dr As DataRow = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)

            Return Trim(dr("score")) & "\" & Trim(dr("score_max"))
        Catch ex As Exception
            Return ""
        End Try

    End Function
    Public Sub getvalutazionibyiduser()

        Dim idcourse As Integer = mycontext.Request.QueryString("idcourse")

        Dim iduser As String = mycontext.Request.QueryString("iduser")

        Dim dttrack = utility.createdatatable()

        Dim objectlearning As Integer = conn.GetDataTable("select count(*) as n from learning_organization where idcourse=" & idcourse & " and objecttype='test'").Rows(0)("n")

        Dim dt1 As DataTable = conn.GetDataTable("select idorg from learning_organization where idcourse=" & idcourse & " and objecttype='test'", CommandType.Text, Nothing)

        Dim idreferences As String = String.Empty

        Dim sumscore As Integer = 0

        Dim attempt As Integer = 0

        Try

            For Each d In dt1.Rows

                idreferences &= d("idorg") & ","

            Next

            idreferences = idreferences.Remove(idreferences.Length - 1, 1)

            Dim r As DataRow

            r = dttrack.newrow

            sqlstring = "select sum(score) as sum from learning_testtrack where iduser= " & iduser & " and idreference in (" & idreferences & ") "

            Try

                sumscore = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("sum")

                r("id") = iduser

                r("media") = sumscore / objectlearning

                sqlstring = "select sum(number_of_attempt) as attempt from learning_testtrack where iduser=" & iduser & " and idreference in (" & idreferences & ") "

                attempt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("attempt")

                r("mediatentativi") = attempt / objectlearning

            Catch ex As Exception

            End Try

            dttrack.rows.add(r)

            FillDataTable(dt, dttrack)

            Dim jsonresult As String = utility.GetJson(dt)

            mycontext.Response.ContentType = "application/json"

            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception

        End Try

    End Sub

    Public Sub getvalutazionibyuser(iduser)
        Try
            If iduser <> "" Then

                ' ' sqlstring = "select concat(firstname,' ' ,lastname) as nominativo," &
                '"((select sum(score) from learning_testtrack where iduser=b.iduser and idreference in (select idorg from learning_organization where idcourse=b.idcourse and objecttype='test') ) / (select count(*)from learning_organization where idcourse=b.idcourse and objecttype='test')) as media, " &
                '"((select sum(number_of_attempt) from learning_testtrack where iduser=b.iduser and idreference in (select idorg from learning_organization where idcourse=b.idcourse and objecttype='test')) / (select count(*)from learning_organization where idcourse=b.idcourse and objecttype='test') ) as mediatentativi, " &
                '"b.iduser as id from core_user a join learning_courseuser b on a.idst=b.iduser where b.`status`=2 and b.idcourse=" & idcourse

                Dim page As String = mycontext.Request.QueryString("page")

                Dim pagesize As String = mycontext.Request.QueryString("rows")

                Dim sidx As String = mycontext.Request.QueryString("sidx")

                Dim sord As String = mycontext.Request.QueryString("sord")

                Dim totalpage As String
                Dim totalrecords As Integer

                sqlstring = "select c.name as nominativo,b.idcourse as id,date_inscr,date_complete,b.status as statocorso,b.idcourse from (core_user a join learning_courseuser b on a.idst=b.iduser) join learning_course c on c.idcourse=b.idcourse where b.iduser=" & iduser & " order by " & sidx & " " & sord

                Dim dtoriginal As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
                totalrecords = dtoriginal.Rows.Count

                If totalrecords > 0 Then

                    Dim start

                    Dim limit

                    If pagesize = "tutti" Then

                        start = 0

                        limit = totalrecords

                    Else

                        If page = 1 Then

                            start = (page * pagesize) - pagesize
                            limit = (page * pagesize) - 1
                        Else
                            start = (page * pagesize) - pagesize
                            limit = (page * pagesize)
                            If limit > totalrecords Then
                                limit = totalrecords Mod page
                            End If
                        End If

                        If totalrecords <= pagesize Then
                            limit = totalrecords - 1
                        End If

                        totalpage = CInt((totalrecords / pagesize) + 1)

                    End If

                    Dim dttrack = utility.createdatatableuser()

                    Dim dtresult As DataTable
                    Dim idcourse As String
                    Dim objectlearning As Integer
                    Dim countoobtotale As Integer
                    Dim dtcountb As DataTable
                    Dim dtime As DataTable
                    Dim dtimevideo As DataTable

                    For i = 0 To totalrecords - 1
                        Dim dr = dtoriginal.Rows(i)

                        Dim r As DataRow

                        idcourse = dr("idcourse")

                        objectlearning = conn.GetDataTable("select count(idorg) as n from learning_organization where idcourse=" & idcourse & " and objecttype='test'").Rows(0)("n")

                        countoobtotale = conn.GetDataTable("select count(idorg) as cntotal from learning_organization where idcourse=" & idcourse & " and objecttype <> '' order by objecttype").Rows(0)("cntotal")

                        sqlstring = "select count(*) as cn,a.iduser from (learning_commontrack a left join learning_organization b on b.idorg = a.idreference ) join learning_courseuser c on c.iduser=a.iduser where c.status != 2 and b.iduser=" & iduser & " b.idcourse=" & idcourse & " and  a.status not in( 'incomplete','failed','ab-initio','attempted') "

                        dtcountb = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                        sqlstring = "select sum(number_of_attempt) as attempt,sum(score)as sum ,iduser from learning_testtrack a where a.iduser=" & iduser & " and idreference in (select idorg from learning_organization where idcourse=" & idcourse & " and objecttype='test') "

                        dtresult = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                        sqlstring = " select sum(time_to_sec(timediff(lasttime,entertime))) as total_time,iduser from learning_tracksession where iduser=" & iduser & " and idcourse=" & idcourse & " "

                        dtime = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                        sqlstring = "select total_time,iduser from learning_scorm_tracking where idreference in (select idorg from learning_organization where iduser=" & iduser & " and idcourse=" & idcourse & " and objecttype='scormorg') "

                        dtimevideo = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                        r = dttrack.newrow

                        r("id") = dr("idcourse")
                        r("iduser") = iduser
                        r("nominativo") = dr("nominativo").ToString
                        r("date_inscr") = dr("date_inscr").ToString
                        r("date_complete") = dr("date_complete").ToString

                        Dim countob As Integer
                        Try
                            countob = dtcountb.Rows(0)("cn")


                            Select Case CInt(dr("statocorso").ToString)
                                Case 0
                                    r("percentuale") = "iscritto"
                                Case 2

                                    r("percentuale") = "completato"

                                Case 1
                                    If countob > 0 Then
                                        r("percentuale") = CInt((countob / countoobtotale) * 100) & " %"
                                    Else
                                        r("percentuale") = 0
                                    End If
                                Case 3
                                    r("percentuale") = "sospeso"
                            End Select

                            Dim sumscore As Integer = 0

                            Dim attempt As Integer = 0




                            Dim result As String = dtresult.Rows(0)("sum")

                            If result <> "" Then
                                sumscore = result

                                r("media") = sumscore / objectlearning

                                attempt = dtresult.Rows(0)("attempt")

                                r("mediatentativi") = attempt / objectlearning

                            End If



                            Dim u As New LogSession

                            Dim time As String = dtimevideo.Rows(0)("total_time")

                            Dim second As String = String.Empty

                            Dim minutes As String = String.Empty

                            Dim hour As String = String.Empty

                            Dim time_sec As Integer = 0
                            Dim time_min As Integer = 0
                            Dim time_hour As Integer = 0

                            r("tempovideocorso") = utility.ConvertSecToDate(u.getUserTotalVideocourseSec(iduser, idcourse))
                            r("temposessione") = utility.ConvertSecToDate(dtime.Rows(0)("total_time"))

                        Catch ex As Exception

                        End Try

                        dttrack.rows.add(r)

                    Next

                    'filldatatable(dt, dttrack)

                    ' dim jsonresult as string = utility.getjson(dt)

                    ' mycontext.response.contenttype = "application/json"

                    ' jsonresult = jsonresult.replace(" ", " ").replace(vbcrlf, " ").replace("\t", " ")

                    ' msg = jsonresult

                    msg = utility.getGridData(page, totalpage, totalrecords, sidx, sord, dttrack)
                Else
                    msg = ""

                End If

            End If

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub

    Public Sub getvalutazioni(idcourse)
        Try
            If idcourse <> "" Then

                ' ' sqlstring = "select concat(firstname,' ' ,lastname) as nominativo," &
                '"((select sum(score) from learning_testtrack where iduser=b.iduser and idreference in (select idorg from learning_organization where idcourse=b.idcourse and objecttype='test') ) / (select count(*)from learning_organization where idcourse=b.idcourse and objecttype='test')) as media, " &
                '"((select sum(number_of_attempt) from learning_testtrack where iduser=b.iduser and idreference in (select idorg from learning_organization where idcourse=b.idcourse and objecttype='test')) / (select count(*)from learning_organization where idcourse=b.idcourse and objecttype='test') ) as mediatentativi, " &
                '"b.iduser as id from core_user a join learning_courseuser b on a.idst=b.iduser where b.`status`=2 and b.idcourse=" & idcourse

                Dim page As String = mycontext.Request.QueryString("page")

                Dim pagesize As String = mycontext.Request.QueryString("rows")

                Dim sidx As String = mycontext.Request.QueryString("sidx")

                Dim sord As String = mycontext.Request.QueryString("sord")

                Dim range As String = mycontext.Request.QueryString("range")

                Dim filter As String
                Dim filter2 As String
                Dim filter4 As String
                Dim filter3 As String

                If range <> "" Then
                    filter = "date_inscr >= '" & ConvertToMysqlDateTime(range.Split(" - ")(0) & " 00:00:00") & "' and date_inscr <= '" & ConvertToMysqlDateTime(range.Split(" - ")(2) & " 23:59:59") & "' and "
                    filter2 = "firstattempt >= '" & ConvertToMysqlDateTime(range.Split(" - ")(0) & " 00:00:00") & "' and firstattempt <= '" & ConvertToMysqlDateTime(range.Split(" - ")(2) & " 23:59:59") & " 23:59:59' and "
                    filter4 = "entertime >= '" & ConvertToMysqlDateTime(range.Split(" - ")(0) & " 00:00:00") & "' and entertime <= '" & ConvertToMysqlDateTime(range.Split(" - ")(2) & " 23:59:59") & " ' and "
                    filter3 = "date_attempt >= '" & ConvertToMysqlDateTime(range.Split(" - ")(0) & " 00:00:00") & "' and date_attempt <= '" & ConvertToMysqlDateTime(range.Split(" - ")(2) & " 23:59:59") & "' and "

                End If

                Dim totalpage As String

                Dim totalrecords As Integer

                sqlstring = "select concat(a.firstname, ' ', a.lastname) as nominativo,b.iduser,b.idcourse as id,date_inscr,date_complete,b.status as statocorso from (core_user a join learning_courseuser b on a.idst=b.iduser) join learning_course c on c.idcourse=b.idcourse where " & filter & " b.idcourse=" & idcourse & " order by " & sidx & " " & sord

                Dim dttrack = utility.createdatatableuser()

                Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                totalrecords = dtoriginal.Rows.Count

                If totalrecords > 0 Then

                    Dim start

                    Dim limit

                    If pagesize = "tutti" Then

                        start = 0

                        limit = totalrecords

                    Else

                        If page = 1 Then

                            start = (page * pagesize) - pagesize
                            limit = (page * pagesize) - 1
                        Else
                            start = (page * pagesize) - pagesize
                            limit = (page * pagesize)
                            If limit > totalrecords Then
                                limit = totalrecords Mod page
                            End If
                        End If

                        If totalrecords <= pagesize Then
                            limit = totalrecords - 1
                        End If

                        totalpage = CInt((totalrecords / pagesize) + 1)

                    End If

                    Dim objectlearning As Integer = conn.GetDataTable("select count(idorg) as n from learning_organization where idcourse=" & idcourse & " and objecttype='test'").Rows(0)("n")

                    Dim countoobtotale As Integer = conn.GetDataTable("select count(idorg) as cntotal from learning_organization where idcourse=" & idcourse & " and objecttype <> '' order by objecttype").Rows(0)("cntotal")

                    sqlstring = "select count(*) as cn,a.iduser from (learning_commontrack a left join learning_organization b on b.idorg = a.idreference ) join learning_courseuser c on c.iduser=a.iduser where " & filter2 & " c.status != 2 and b.idcourse=" & idcourse & " and  a.status not in( 'incomplete','failed','ab-initio','attempted') group by (a.iduser)"

                    Dim dtcountb As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                    sqlstring = "select sum(number_of_attempt) as attempt,sum(score)as sum ,iduser from learning_testtrack where " & filter3 & " idreference in (select idorg from learning_organization where idcourse=" & idcourse & " and objecttype='test') group by (iduser) "

                    Dim dtresult As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                    sqlstring = " select sum(time_to_sec(timediff(lasttime,entertime))) as total_time,iduser from learning_tracksession where " & filter4 & " idcourse=" & idcourse & " group by (iduser) "

                    Dim dtime As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                    sqlstring = "select total_time,iduser from learning_scorm_tracking where idreference in (select idorg from learning_organization where idcourse=" & idcourse & " and objecttype='scormorg') group by (iduser) "

                    Dim dtimevideo As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                    For i = 0 To totalrecords - 1

                        Dim dr = dtoriginal.Rows(i)

                        Dim r As DataRow

                        r = dttrack.newrow
                        r("id") = dr("iduser")
                        r("nominativo") = dr("nominativo").ToString
                        r("date_inscr") = dr("date_inscr").ToString
                        r("date_complete") = dr("date_complete").ToString

                        Dim iduser As String = dr("iduser")

                        Dim countob As Integer
                        Try
                            countob = dtcountb.Select("iduser=" & iduser)(0)("cn")
                        Catch ex As Exception
                        End Try

                        Select Case CInt(dr("statocorso").ToString)
                            Case 0
                                r("percentuale") = "iscritto"
                            Case 2

                                r("percentuale") = "completato"

                            Case 1
                                If countob > 0 Then
                                    r("percentuale") = CInt((countob / countoobtotale) * 100) & " %"
                                Else
                                    r("percentuale") = 0
                                End If
                            Case 3
                                r("percentuale") = "sospeso"
                        End Select

                        Dim sumscore As Integer = 0

                        Dim attempt As Integer = 0

                        Try

                            Try
                                Dim result As String = dtresult.Select("iduser=" & iduser)(0)("sum")

                                If result <> "" Then
                                    sumscore = result

                                    r("media") = sumscore / objectlearning

                                    attempt = dtresult.Select("iduser=" & iduser)(0)("attempt")

                                    r("mediatentativi") = attempt / objectlearning

                                End If

                            Catch ex As Exception
                                LogWrite(ex.Message)
                            End Try



                            Dim time As String = dtimevideo.Select(" iduser=" & iduser & " ")(0)("total_time")

                            Dim second As String = String.Empty

                            Dim minutes As String = String.Empty

                            Dim hour As String = String.Empty

                            Dim time_sec As Integer = 0
                            Dim time_min As Integer = 0
                            Dim time_hour As Integer = 0
                            Dim objlog As New LogSession
                            r("tempovideocorso") = utility.ConvertSecToDate(objlog.getUserTotalVideocourseSec(iduser, idcourse))
                            r("temposessione") = utility.ConvertSecToDate(dtime.Select("iduser=" & iduser & "")(0)("total_time"))

                        Catch ex As Exception
                            LogWrite(ex.ToString)
                        End Try

                        dttrack.rows.add(r)

                    Next

                    'filldatatable(dt, dttrack)

                    ' dim jsonresult as string = utility.getjson(dt)

                    ' mycontext.response.contenttype = "application/json"

                    ' jsonresult = jsonresult.replace(" ", " ").replace(vbcrlf, " ").replace("\t", " ")

                    ' msg = jsonresult

                    msg = utility.getGridData(page, totalpage, totalrecords, sidx, sord, dttrack)
                Else
                    msg = ""

                End If

            End If

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub


    Function getstatusercourse(iduser, idcourse)

        Try

            sqlstring = "select identer as id, entertime,lasttime,numop,(select concat(' ',sum(time_to_sec(timediff(lasttime,entertime)))) from learning_tracksession where identer=id) as duration from learning_tracksession where entertime != lasttime and idcourse=" & idcourse & " and iduser=" & iduser & " order by entertime asc "

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            mycontext.Response.ContentType = "application/json"

            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

        Return False

    End Function


    Public Sub getreportlesson()


    End Sub


    Public Sub getcertificatePON(iduser, idproject)
        sqlstring = "select * from ((aula_sessioni a  join aula_prenotazioni b on a.id=b.idsessione) join core_project c on a.idproject=c.id) join core_user d on b.iduser=d.idst  where b.iduser=" & iduser & " and presente=1 and   idproject=" & idproject

        dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        Dim body As String = utility.Getmailformat(, "certificatepon")

        body = body.Replace("[NOMINATIVO]", FormattaNominativo(dt.Rows(0)("firstanme") & " " & dt.Rows(0)("lastname")))
        body = body.Replace("[CF]", FormattaNominativo(dt.Rows(0)("cf")))
        body = body.Replace("[DATARILASCIO]", FormattaNominativo(dt.Rows(0)("data_end")))
        body = body.Replace("[LUOGO]", FormattaNominativo(dt.Rows(0)("luogo")))


        Dim strhtml As String = "<table><tr style='background-color:lightblue'><tr>Articolazione del Modulo per contenuti</td><td>Ore frequenta te/Ore totali</td></tr>"

        For Each dr In dt.Rows
            Select Case dr("tipo")
                Case "LEZIONE"
                    getreporthtmldocument(strhtml, dr("id"), iduser)

                Case "WEBINAR"
                    getreporthtmlMeet(strhtml, dr("id"), iduser)
                Case "CORSO"

                Case "VERIFICA SCRITTA"

                Case "VERIFICA ORALE"
            End Select
        Next

    End Sub

#End Region


    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property




End Class

