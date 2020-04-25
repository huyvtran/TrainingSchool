
Imports TrainingSchool.SharedRoutines
Imports System.IO
Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json
Imports Ovh.Api
Imports SelectPdf
Imports System.Drawing
Imports Microsoft.Web.Administration
Imports System.Net

Public Class Ajaxadminlms
    Inherits System.Web.UI.Page
    Dim conn As rconnection

    Dim sqlstring As String = String.Empty
    Dim dt As DataTable = Nothing
    Dim utility As SharedRoutines
    Dim html As String = String.Empty
    Dim msg As String = String.Empty
    Dim esito As String = String.Empty
    Dim idcourse As String = String.Empty
    Dim iduser As String = String.Empty
    Dim idcertificate As String = String.Empty
    Dim ckendcourse As Boolean
    Dim ckvisible As Boolean
    Dim idorgs As String = String.Empty
    Dim idorg As String = String.Empty
    Dim objuser As LogSession
    Private prefix As String = "ctl00$contentplaceholder1$"""
    Dim mailpiattaforma = ConfigurationSettings.AppSettings("email")
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

    Public Shared dbInsertfrom As String = System.Configuration.ConfigurationSettings.AppSettings("DBInsertfrom")


    Dim pathInt As Integer = 1





    Protected Sub page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        conn = CheckDatabase(conn)

        utility = New SharedRoutines

        objuser = New LogSession
        Try

            If Request.QueryString("op") <> "loginuser" And Request.QueryString("op") <> "signuser" Then

                If Session("iduser") = 0 OrElse Session("iduser") Is Nothing Then
                    msg = "sessione scaduta"
                End If


                Select Case Request.QueryString("op")


                    Case "getmailformat"
                        getmailformat(, Request.QueryString("format"))
                    Case "updateformatmail"
                        updatemailformat()

                    Case "checkdata"
                        checkdata()

                    Case "jgetallclass"
                        jgetallclass()

                    Case "jgettest"
                        jgettest()



                    Case "modprenotazioni"
                        Dim idsessione As Integer
                        Try
                            idsessione = Request.QueryString("idsessione")
                        Catch ex As Exception
                            idsessione = 0
                        End Try
                        Select Case (Request.QueryString("oper"))
                            Case "getcorsisti"
                                getstudentsession()
                            Case "prenotastudente"
                                bookingstudent()
                            Case "prenota"
                                booking()
                            Case "sendinvite"
                                sendinvite()
                            Case "downloaddomande"
                                downloaddomande(Request.QueryString("idprenotazione"))
                            Case "downloadrisposte"
                                downloadrisposte(Request.QueryString("idprenotazione"))
                            Case "downloaddoc"
                                downloaddoc(Request.QueryString("id"))
                            Case "downloaduploaddoc"
                                downloaduploaddoc(Request.QueryString("id"))
                            Case "get"
                                getprenotazioni(iduser)
                            Case "getprenotati"
                                getprenotati(Request.QueryString("idsessione"))
                            Case "getdocumenti"
                                getdocumenti(idsessione)
                            Case "deletedoc"
                                deletedoc(Request.Form("id"))
                            Case "getdocumentistudenti"
                                getdocumentistudenti(Request.QueryString("id"))
                            Case "deletedocstudents"
                                deletedocstudents(Request.QueryString("id"))
                            Case "updatepresente"
                                updatepresente(Request.Form("presente"), Request.Form("idprenotazione"))
                            Case "updatepresenteall"
                                updatepresenteall()
                            Case "inviasingolamail"
                                inviamailpecsingola(Request.Form("idsessione"), Request.Form("iduser"))
                            Case "deletebooking"
                                deletebooking(Request.Form("idprenotazione"), Request.Form("tipo"), Request.Form("iduser"), Request.Form("idsessione"), Request.Form("write"))
                            Case "inviacomunicazionestudenti"
                                inviacomunicazionecorsista()
                            Case "inviacomunicazionestudentetutti"

                                inviacomunicazionecorsistatutti()

                        End Select
                    Case "modsessioni"
                        ' if session("admin") then

                        Select Case (Request.QueryString("oper"))
                            Case "getcalendar"
                                getcalendar()
                            Case "insertestexam"
                                maketestcourse()
                            Case "gettesteassigned"
                                gettesteassigned()
                            Case "getcourseassigned"
                                getcourseassigned()
                            Case "assigncourse"
                                insertprenotazionecourse(Request.QueryString("idsessione"), Request.QueryString("idcategory"), Request.QueryString("idcourse").ToString)
                            Case "getcoursestudents"
                                getcoursestudents()
                            Case "getcoursestudentssingles"
                                getcoursestudentssingle()
                            Case "addcompiti"
                                adddocsessione(Request.Form("idsessione"), "")
                            Case "attendance"

                            Case "getemailflag"
                                getemailflag(Request.Form("id"))
                            Case "getall"
                                getsessioni()
                            Case "getcoursesession"
                                getcoursesession()
                            Case "getprenotati"
                                getprenotati(Request.QueryString("idsessione"), Request.QueryString("filter"))
                            Case "getmailformat"
                                getmailformat(, Request.QueryString("format").ToString)
                            Case "updateformatmail"
                                updatemailformat()
                            Case "updatevisibile"
                                updatevisibile()
                            Case "updateattivo"
                                updateattivo()


                            Case "hitdoc"
                                addhit()
                        End Select

                        Select Case Request.Form("oper")

                            Case "del"
                                deletesessione()
                            Case "edit"
                                editsessione(Request.Form("idsessione"))
                            Case "add2"
                                Dim idsessione As Integer
                                Try
                                    idsessione = Request.Form("idsessione").ToString
                                Catch ex As Exception
                                    idsessione = 0
                                End Try
                                insertsessione(idsessione)
                        End Select






                    Case "modhtml"

                        Dim h As New Html
                        Select Case Request.QueryString("oper")

                            Case "savehtml"

                                Dim content As String = String.Empty

                                Dim title As String = String.Empty

                                Dim idpage As Integer = Request.QueryString("idpage")
                                title = Request.Form("title").ToString
                                content = EscapeMySql(System.Web.HttpUtility.UrlDecode(Request.Form("content")))
                                h.SaveHtml(title, content, idpage)

                            Case "createhtml"

                                Dim content As String = String.Empty

                                Dim title As String = String.Empty

                                title = Request.Form("title").ToString
                                content = EscapeMySql(System.Web.HttpUtility.UrlDecode(Request.Form("content")))
                                msg = h.CreateHtml(title, content)
                            Case "gethtml"
                                Dim idpage As Integer = Request.QueryString("idpage")
                                msg = h.getHtml(idpage)
                        End Select

                    Case "modscorm"
                        Dim s As New Scorm

                        Select Case Request.QueryString("oper")

                            Case "createscorm"
                                msg = s.CreateScormEmpty(Request.Form("title").ToString, Request.Form("nvideo").ToString)

                            Case "editscorm"
                                msg = s.EditScorm(Request.Form("title"), Request.Form("id"))

                        End Select

                    Case "scormtime"
                        msg = utility.ConvertSecToDate(utility.SumScormTime(Session("iduser"), Session("idcourse")))

                    Case "lastobject"
                        msg = utility.LastObject(Session("iduser"), Session("idcourse"))

                    Case "totaltime"
                        msg = objuser.GetUserTotalCourseTime(Session("iduser"), Session("idcourse"))

' utility.totaltime(session("iduser"), session("idcourse"))
                    Case "modfaq"
                        Dim h As New Faq
                        Select Case Request.QueryString("oper")

                            Case "createfaq"

                                Dim content As String = String.Empty

                                Dim title As String = String.Empty

                                title = Request.Form("title").ToString
                                content = EscapeMySql(System.Web.HttpUtility.UrlDecode(Request.Form("content")))
                                msg = h.CreateFaq(title, content)

                            Case "addfield"
                                saveadditionalfaq(Request.QueryString("idcategory"))
                        End Select

                    Case "modtest"

                        Dim t As New TestObject
                        Select Case Request.QueryString("oper")
                            Case "addsequence"
                                msg = t.AddSequence(Request.QueryString("idtest"))
                            Case "addquestion"
                                msg = t.InsertQuestion(Request.QueryString("idtest"), Request.QueryString("idquest"), Request.Form("question"), Request.Form("answer"), Request.Form("check"), Request.Form("idanswer"))

                            Case "get"
                                msg = t.getFormData(Request.QueryString("idtest"))
                            Case "gettest"
                                msg = t.GetTest()
                            Case "getquestion"
                                msg = t.GetQuestion(Request.QueryString("idquest"))
                            Case "resultest"
                                msg = t.SaveTest()
                            Case "createtest"
                                Dim soglia As String = Context.Request.Form("soglia").ToString
                                Dim title As String = Context.Request.Form("title").ToString
                                Dim tentativi As String = Context.Request.Form("tentativi").ToString
                                Dim random As String = Context.Request.Form("random").ToString
                                Dim idtest As Integer
                                t.InsertTest(title, soglia, tentativi, random, idtest)

                            Case "updatetest"
                                Dim soglia As String = Context.Request.Form("soglia").ToString
                                Dim title As String = Context.Request.Form("title").ToString
                                Dim tentativi As String = Context.Request.Form("tentativi").ToString
                                Dim random As String = Context.Request.Form("random").ToString
                                Dim idtest = Context.Request.Form("idtest").ToString
                                t.updatetest(title, soglia, tentativi, random, idtest)
                        End Select

                    Case "modpoll"
                        Dim p As New Poll
                        Select Case Request.QueryString("oper")
                            Case "addquestion"

                                p.InsertQuestion(Request.QueryString("idpoll"), Request.QueryString("idquest"), Request.Form("question"), Request.Form("answer"), Request.Form("idanswer"))
                            Case "getquestion"
                                p.GetPollquestion(Request.QueryString("idquest"))
                            Case "createpoll"
                                Dim description As String = Context.Request.Form("description").ToString
                                Dim title As String = Context.Request.Form("title").ToString
                                msg = p.CreatePoll(title, description, Request.QueryString("id_poll"))
                            Case "getpoll"
                                msg = p.GetPoll()
                            Case "resultpoll"
                                p.SavePoll()

                        End Select

                    Case "modmarketplace"
                        Select Case Request.QueryString("oper")
                            Case "get"
                                getPreferiti()
                        End Select
                    Case "getcourseteacher"
                        Select Case Request.QueryString("oper")
                            Case "get"
                                getcourseteacher()
                        End Select
                    Case "modcourse"
                        idcourse = Request.Form("id")

                        Select Case Request.QueryString("oper")
                            Case "get"
                                getcourse()
                            Case "gettime"
                                getTime(Request.QueryString("iduser"), Request.QueryString("idcourse"), Request.QueryString("fullname"))
                            Case "addcat"
                                insertcat()
                            Case "cat"
                                getallcatcourse()


                            Case "getusers"
                                getuserscourse(idcourse)
                            Case "status"
                                getstatus()

                            Case "controlnav"
                                msg = conn.GetDataTable("Select controlnav from learning_course where idcourse=" & Session("idcourse") & "", CommandType.Text, Nothing).Rows(0)("controlnav").ToString()
                            Case "getvalutazionibyiduser"
                                getvalutazionibyiduser()
                            Case "getvalutazioni"
                                getvalutazioni(Request.QueryString("idcourse"))

                            Case "getvalutazionibyuser"
                                getvalutazionibyuser(Request.QueryString("iduser"))
                            Case "getreportvalutazioni"
                                getreportvalutazioni(Request.QueryString("iduser"), Request.QueryString("idcourse"))


                            Case "getcoursecompleted"
                                getcoursecompleted(Request.QueryString("iduser"))
                        End Select

                        Select Case Request.Form("oper")
                            Case "add"
                                Dim description As String = Request.Form("description")
                                Dim credits As String = Request.Form("credits")
                                Dim code As String = Request.Form("code")
                                Dim name As String = Request.Form("name")
                                Dim idcourse As Integer
                                insertcourse(description, code, name, credits, idcourse)
                            Case "edit"
                                editcourse(idcourse)
                            Case "del"
                                deletecourse(idcourse)

                        End Select
                    Case "modcoursestudents"

                        iduser = Request.QueryString("iduser")

                        Dim idcourse = Request.QueryString("idcourse")

                        Select Case Request.QueryString("oper")
                            Case "add"

                                insertstudents(iduser, idcourse)
                            Case "usersavailable"

                                getstudents(idcourse)

                            Case "usersassigned"

                                getstudentsassign(idcourse)
                            Case "usersdelete"
                                delstudenti(Request.Form("id"), idcourse)
                        End Select
                    Case "modteachers"

                        iduser = Request.QueryString("iduser")

                        Dim idcategory = Request.QueryString("idcategory")

                        Select Case Request.QueryString("oper")
                            Case "add"

                                assigngroupteachers(iduser, idcategory)
                            Case "usersavailable"

                                getteachersavailable(idcategory)

                            Case "usersassigned"

                                getgroupteachersassigned(Int(idcategory))
                        End Select
                        Select Case Request.Form("oper")

                            Case "del"
                                delgroupteachers(Request.Form("id"), idcategory)
                        End Select
                    Case "modstudents"

                        iduser = Request.QueryString("iduser")

                        Dim idcategory = Request.QueryString("idcategory")

                        Select Case Request.QueryString("oper")
                            Case "add"

                                assigngroupstudents(iduser, idcategory)
                            Case "usersavailable"

                                getstudentsavailable(idcategory)

                            Case "usersassigned"

                                getgroupstudentsassigned(idcategory)
                            Case "usersdelete"


                        End Select
                        Select Case Request.Form("oper")

                            Case "del"
                                delgroupstudents(Request.Form("id"), Request.QueryString("idcategory"))

                        End Select

                    Case "modcategoryuser"
                        Select Case Request.Form("oper")
                            Case "add"
                                insertcategory()
                            Case "edit"
                               ' editcategory(iduser)
                            Case "del"
                                'delcate(iduser)
                            Case "get"
                        End Select
                    Case "moduser"
                        iduser = Request.Form("id")



                        Select Case Request.QueryString("oper")

                            Case "getcategory"
                                getcategoryuser()
                            Case "profile"
                                getallcatprofile()
                            Case "get"
                                getuser()

                        End Select

                        Select Case Request.Form("oper")
                            Case "add"
                                insertusers()
                            Case "edit"
                                editusers(iduser)
                            Case "del"
                                deleteusers(iduser)
                            Case "get"
                                getuser()

                        End Select





                    Case "modprops"
                        idorgs = Request.Form("id")

                        Select Case Request.QueryString("oper")
                            Case "update"
                                Dim res As String = String.Empty

                                Dim s As Scorm = Nothing

                                idorgs = Request.QueryString("prerequisites")
                                ckendcourse = Request.QueryString("ckendcourse")
                                ckvisible = Request.QueryString("ckvisible")
                                idorg = Request.QueryString("idorg")
                                Title = Request.QueryString("title")
                                idcourse = Request.QueryString("idcourse")
                                res = updateprops(idorgs, idorg, ckendcourse, ckvisible, Title, idcourse)

                                Dim nvideo As String = Request.QueryString("txtvideo")
                                msg = res

                            Case "propsavailable"
                                getpropsavailable(Request.QueryString("idorg"), Request.QueryString("idcourse"))

                        End Select


                    Case "modstats"

                        Select Case Request.QueryString("oper")
                            Case "getstatusercourse"
                                idcourse = Request.QueryString("idcourse")
                                iduser = Request.QueryString("iduser")
                                getstatusercourse(iduser, idcourse)

                        End Select

                    Case "modplatform"
                        Dim idplatform = Request.Form("id")

                        Select Case Request.Form("oper")
                            Case "add"
                                insertplatform()
                            Case "edit"
                                editplatform(idplatform)
                            Case "del"
                                deleteplatform(idplatform)
                            Case "get"
                                getplatform()

                        End Select

                        Select Case Request.QueryString("oper")

                            Case "get"
                                getplatform()
                        End Select

                End Select

            Else

                Select Case Request.QueryString("op")
                    Case "loginuser"
                        loginuser()
                    Case "signuser"
                        signusers()
                End Select

            End If

            HttpContext.Current.Response.Write(msg)
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub







#Region "admin"
    Sub RecuperaPassword()

        Dim dr As DataRow

        Dim email As String = Request.Form("email").ToString

        Dim pass As String = LCase(CreateRandomPassword(6))

        Try


            Dim sqlstring As String

            sqlstring = "update core_user set pass=md5('" & pass & "') where email='" & email & "'"

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "select userid,user_entry as password from core_user where  email='" & email & "'"

            dr = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)


        Catch ex As Exception
            msg &= " $('#alertsignmsg').removeClass('hide');$('.txtalert').html('Impossibile recuperare password contattare il supporto tecnico');"
            SharedRoutines.LogWrite(Now & "  " & ex.ToString)
            Exit Sub
        End Try




        Dim body As String = "<div style='text-align:justify'><span style='font-family:Arial;font-size:12pt;color:#00314C'>    Gentile Utente <b>" & FormattaNominativo(dr("firstname") & " " & dr("lastname")) & "</b>,<br>" &
"da questo momento può accedere alla piattaforma e ai suoi contenuti come utente regolarmente iscritto.<br>" &
"Le assegniamo una <b>Password</b> e una <b>Username personali</b> che dovrà conservare e che Le permetteranno di visualizzare i contenuti formativi.<br>" &
"Per accedere alla piattaforma streaming utilizzi il seguente indirizzo:<br> <br> " &
"<center>" & Request.Url.Authority & "</center><br>" &
"Al centro dello schermo troverà l’area riservata ""Accedi alla piattaforma"" in cui inserire i Suoi dati personali di accesso alla piattaforma, che sono:<br>" &
"<p style='line-height:0.8em;'><b>Username:           " & dr("userid") & " <br><br>" &
"Password:            " & pass & " </b></p><br>" &
" Dopo l'autentificazione avrà accesso alla pagina per la visione dei materiali formativi.<br><br>" &
            "Restiamo a Sua disposizione per qualsiasi chiarimento,<br><br>" &
            "cordiali saluti, <br>"



        Dim esito As String = utility.InvioMail(email, mailpiattaforma, "", "Credenziali piattaforma Training-school", body, "")

        If esito.StartsWith("Email Inviata") Then
            msg &= " $('#alertrecmsg').addClass('hide'); $('#successmsg1').removeClass('hide');$('.txtsuccess').html('Email di recupero password inviata - " & esito & "');"

        Else
            msg &= " $('#alertrecmsg').removeClass('hide');$('.txtalert').html('Email non valida risposta del server : " & esito & "' );"

        End If


    End Sub

    Sub signusers()

        Dim nome As String = String.Empty
        Dim cognome As String = String.Empty
        Dim email As String = String.Empty
        Dim cf As String = String.Empty
        Dim datanascita As String = String.Empty
        Dim codiceclasse As String = String.Empty
        Dim scuola As String = String.Empty
        Dim profile As String = Request.QueryString("profile")

        If IsNumeric(profile) Then

            Select Case profile
                Case "3"
                    nome = EscapeMySql(Request.Form("firstname").ToString)
                    cognome = EscapeMySql(Request.Form("lastname").ToString)
                    email = Request.Form("email").ToString
                    datanascita = Request.Form("datanascita").ToString
                    codiceclasse = Request.Form("codice").ToString

                Case "2"
                    nome = EscapeMySql(Request.Form("firstname").ToString)
                    cognome = EscapeMySql(Request.Form("lastname").ToString)
                    email = Request.Form("email").ToString
                    scuola = Request.Form("scuola").ToString


            End Select

            Dim username As String = String.Empty

            Dim password As String = LCase(CreateRandomPassword(6))
            Dim dataiscrizione As String = ConvertToMysqlDateTime(Now)

            Dim sconn As rconnection
            sconn = conn
            If sconn Is Nothing Then
                msg &= " $('#alertsignmsg').removeClass('hide');$('.txtalert').html('errore generale' );"
                Exit Sub
            End If

            Try

                Try
                    If cognome.Length >= 3 Then
                        username = Trim(Replace(Replace(cognome, " ", ""), "'", "")).Substring(0, 3)
                    End If
                    If nome.Length >= 3 Then
                        username &= Trim(Replace(nome, " ", "")).Substring(0, 3)
                    End If
                    If username.Length < 6 Then
                        username = Trim(LCase(Replace(Trim(cognome & nome), "'", "")).Substring(0, 6))
                    End If
                Catch ex As Exception
                    username = Trim(cognome)
                End Try

                username = LCase(username)
                username = utility.findusername(username, nome, cognome, conn)
            Catch ex As Exception

            End Try

            Try
                sqlstring = " insert into core_st ( idst ) values(null); insert into core_user " &
    " ( idst, userid, firstname, lastname, pass, email ,register_date,idprofile,clearpass,datanascita,scuola) " &
    "values ( last_insert_id(), '" & Trim(username) & " ', '" & Trim(FormattaNominativo(nome)) & "', '" & Trim(FormattaNominativo(cognome)) & "', " &
    "'" & getMd5Hash(password) & "', '" & LCase(email) & "', '" & String.Format("{0:u}", dataiscrizione) & "'," & profile & ",'" & password & "','" & datanascita & "','" & scuola & "')"

                sconn.Execute(sqlstring, CommandType.Text, Nothing)

            Catch ex As Exception
                msg &= " $('.alertsignmsg').removeClass('hide');$('.txtalert').html('email già registrata riprova con un nuovo indirizzo email');"
                SharedRoutines.LogWrite(ex.ToString)
                Exit Sub
            End Try

            Dim idst As Integer
            Try

                idst = sconn.GetDataTable("select max(idst) as idst from core_user order by idst desc").Rows(0)("idst")

            Catch ex As Exception
                SharedRoutines.LogWrite(ex.ToString)
            End Try

            If profile = 3 Then
                sqlstring = "insert into aula_studenti (iduser,idcategory) values(" & idst & ",(Select idcategory from core_category_users where codice='" & codiceclasse & "'))"
                sconn.Execute(sqlstring, CommandType.Text, Nothing)
            End If



            utility.SendMail(esito, mailpiattaforma, nome, cognome, email, username, password, Session("convenzione"), "sign")

            If esito.StartsWith("Email Inviata") Then
                msg &= " $('.alertsignmsg').addClass('hide'); $('.successmsg').removeClass('hide');$('.txtsuccess').html('registrazione completata - " & esito & "');"

            Else
                msg &= " $('.alertsignmsg').removeClass('hide');$('.txtalert').html('email non valida risposta del server : " & esito & "' );"

            End If
        Else
            msg &= " $('.alertsignmsg').removeClass('hide');$('.txtalert').html('Errore registrazione : " & esito & "' );"

        End If
    End Sub

    Protected Sub loginuser2()



        Dim username As String = Request.Form("username")
        Dim password As String = Request.Form("password")

        Dim uri As New Uri("http://40.67.204.30:9001/trainingschool/public/login")

        Dim jsonString As String = "{""userId"":""" & username & """,""password"":""" & password & """}"
        Try
            '  ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim jsonDataBytes = Encoding.UTF8.GetBytes(jsonString)


            Dim req As WebRequest = WebRequest.Create(uri)

            req.ContentType = "application/json"
            req.Method = "POST"
            req.ContentLength = jsonDataBytes.Length


            Dim stream = req.GetRequestStream()
            stream.Write(jsonDataBytes, 0, jsonDataBytes.Length)
            stream.Close()

            Session("token") = req.GetResponse().Headers("auth")
            Dim response = req.GetResponse().GetResponseStream()

            Dim reader As New StreamReader(response)

            Dim res = reader.ReadToEnd()
            reader.Close()
            response.Close()

        Catch ex As Exception
        End Try

    End Sub
    Protected Sub loginuser()



        Dim username As String = Request.Form("username")
        Dim password As String = Request.Form("password")
        Dim streval As String = String.Empty

        Dim strconn As String = "Select *,b.nome from core_user a left join core_profile b On a.idprofile=b.id  where userid= '" & username & "' and pass='" & getMd5Hash(password) & "' "
        Dim dt As System.Data.DataTable = conn.GetDataTable(strconn, CommandType.Text, Nothing)

        Try

            If dt.Rows.Count > 0 Then

                Session("profile") = dt.Rows(0)("nome").ToString
                Session("idprofile") = dt.Rows(0)("idprofile").ToString
                Session("iduser") = dt.Rows(0)("idst").ToString
                Session("userid") = dt.Rows(0)("userid").ToString
                Session("idcategory") = getclass(Session("iduser"))

                If dt.Rows(0)("firstname").ToString.EndsWith("a") Then
                    Session("f") = True
                Else
                    Session("f") = False
                End If

                Session("fullname") = dt.Rows(0)("firstname") & " " & dt.Rows(0)("lastname")
                Session("lastaccess") = dt.Rows(0)("lastenter").ToString
                Session("registerdate") = dt.Rows(0)("register_date").ToString


                objuser.SetLastAccess(Session("iduser"))

                If Session("idprofile") = 2 Then
                    streval = " top.location.href='wfdashadmin.aspx';"
                Else
                    streval = " top.location.href='wfdashstudent.aspx';"
                End If


            Else
                streval = " $('#alertmsg').removeClass('hide');$('.txtalert').text('Credenziali errate');"
            End If

        Catch ex As Exception
            LogWrite(ex.ToString)
            streval = " $('#alertmsg').removeClass('hide');$('.txtalert').text('Credenziali errate');"
        End Try

        msg = streval

    End Sub
    Function getclass(iduser)
        Try

            Select Case Session("idprofile")
                Case 1
                    Return 0
                Case 2
                    Return conn.GetDataTable("SELECT k.iduser, GROUP_CONCAT(k.idcategory) as idc  FROM aula_docenti AS k  where iduser=" & iduser & " Group BY k.iduser", CommandType.Text, Nothing).Rows(0)("idc")


                Case 3
                    Return conn.GetDataTable("SELECT k.iduser, GROUP_CONCAT(k.idcategory) as idc  FROM aula_studenti AS k  where iduser=" & iduser & " Group BY k.iduser", CommandType.Text, Nothing).Rows(0)("idc")
            End Select
        Catch ex As Exception
            Return -1
        End Try

    End Function
#End Region
#Region "marketplace"
    Public Sub getPreferiti()

        Dim strmedia As String
        Dim id_comune As Integer
        Dim id_regione As Integer
        Dim id_provincia As Integer
        Dim id_contratto As Integer
        Dim id_orario As Integer
        Dim id_mansione_lavorativa As Integer

        Dim text As String
        Dim order As String
        Dim limit As String
        Dim pagination As String
        Dim highlight As String
        Dim csname2 As String = "ButtonClickScript"
        Dim cstype As Type = Me.GetType()
        Dim cs As ClientScriptManager = Page.ClientScript
        Dim cstext2 As New StringBuilder()
        Dim npage As String
        Try

            pagination = Request.Form("rpage")

            If pagination = "" Then
                pagination = 1
                limit = " limit 0," & npage
            Else
                limit = "limit " & pagination * npage & " , " & npage
            End If


            Dim where As String = " and "



            Try
                If Request.QueryString("order") <> "" Then
                    order = " order by " & Request.QueryString("order")
                End If
            Catch ex As Exception
            End Try

            Try
                If Request.Form("text") <> "" Then
                    text = Request.Form("text")
                End If
            Catch ex As Exception
            End Try


            sqlstring = "select *  from ai_annunci"




            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            If dtoriginal.Rows.Count > 0 Then



                Session("resultpage") = 1 ' dtoriginal.Rows(0)("total")

                cstext2.Append("<script type=""text/javascript""> function ShowButton() {}")
                cstext2.Append("</script>")
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "MyScript", cstext2.ToString, True)
            Else

                If (Not cs.IsClientScriptBlockRegistered(cstype, csname2)) Then


                    cstext2.Append("<script type=""text/javascript""> function HideButton() {}")
                    cstext2.Append("</script>")
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "MyScript", cstext2.ToString, True)

                End If
                Session("resultpage") = 0
            End If

            If Session("resultpage") > npage Then
                strmedia = "<b class=""text-primary"">Annunci trovati:</b> <span class=""annuncitrovati""> " & Session("resultpage") & "</span> - <b class=""text-primary"">Pagina</b> " & pagination & " <b class=""text-primary"">di</b> 1 <div class=""space-10""></div>"
            End If

            Dim avatar As String
            For Each dr In dtoriginal.Rows
                Try


                    highlight = "<div class=""media search-media""><div class=""media-left"">" &
                              "<a href=""#"" onclick=""ViewModalModifica('WfIOViewProfile.aspx?iduser=" & dr("iduser") & "','Profilo Inserzionista');"" >    <img height=""72px"" class=""media-object"" src=""" & dr("img") & """/></a>" &
                          "</div>" &
                                 " <div class=""media-body""><div> " &
                               "   <h4 class=""media-heading""> " &
                                     " <a href=""#"" onclick=""ViewModalModifica('WfIOViewAds.aspx?idannuncio=" & dr("idannuncio") & "','Annuncio');"" class=""blue"">" & dr("titolo").ToString & "</a> " &
                                "  </h4> Pubblicato il: " & FormatDateTime(dr("dataannuncio").ToString, DateFormat.GeneralDate) & " <b> " &
                            "  </div>" &
                             " <p>" & dr("testoannuncio").ToString & " ...</p> " &
    "                           <div class=""search-actions text-center""> " &
                                 " <span class=""text-info"">€</span> " &
           " <span class=""blue bolder bigger-150"">" & dr("compenso") & "</span>  " & dr("annualemensile") & "" &
           "<div class=""action-buttons bigger-125"">" &
                                     " <a href=""#""  onclick=""RequestCall(" & dr("idannuncio").ToString & "," & Session("iduser") & ");""> <i class=""ace icon-phone green""></i> </a>" &
           " <a href=""#""  onclick=""RequestFavourite(" & dr("idannuncio").ToString & ");""> " &
           " <i class=""ace icon-heart red""></i>   </a> " &
           "  <a href=""#""  onclick=""RequestRating(" & dr("idannuncio").ToString & ");"" > <i class=""ace icon-star orange2""></i> </a> " &
                                 " </div> "


                    If dr("id_tipoannuncio") = 1 Then
                        highlight &= " <a  onclick=""RequestCurriculum(" & dr("idannuncio").ToString & "," & Session("iduser") & ");""  class=""search-btn-action btn btn-sm btn-block btn-info"">Acquista!</a> " &
                                        " </div> </div>  </div> "
                    Else

                        highlight &= " <a  onclick=""AddCandidatura(" & dr("idannuncio").ToString & "," & Session("iduser") & ");""  class=""search-btn-action btn btn-sm btn-block btn-info"">Acquista!</a> " &
                                                  " </div> </div>  </div> "
                    End If


                    If text <> "" Then
                        highlight = Replace(highlight, text, "<mark>" & text & "</mark>")
                    End If
                    strmedia &= highlight
                Catch ex As Exception
                End Try

            Next
            If strmedia <> "" Then


            Else
                strmedia = "Nessun risultato con i criteri selezionati"
            End If

            msg = strmedia
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub



#End Region

#Region "profilo"

    Public Function getavatar(iduser As String)
        Dim avatar As String = String.Empty

        sqlstring = "select avatar from ai_userprofile where iduser=" & iduser
        Dim dr As MySql.Data.MySqlClient.MySqlDataReader = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)

        Try
            dr.Read()
            If dr("avatar").ToString <> "" Then
                avatar = "images/avatar/" & dr("avatar")
            Else
                avatar = "images/avatar/user.jpg"
            End If

            dr.Close()
        Catch ex As Exception
            dr.Close()
            avatar = "images/avatar/user.jpg"
        End Try

        Return avatar
        Return False

    End Function

    Public Sub getexperience(iduser As String, num As Integer)

        sqlstring = "select a.*,b.firstname,b.lastname,b.email from (ai_userprofile_experience a join core_user b on a.iduser=b.idst ) where num=" & num & " and iduser=" & iduser

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")
        msg = jsonresult

    End Sub

    Public Sub geteducation(iduser As String, num As Integer)
        sqlstring = "select a.*,b.firstname,b.lastname,b.email from (ai_userprofile_experience a join core_user b on a.iduser=b.idst ) where num=" & num & " and iduser=" & iduser
        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")
        msg = jsonresult

    End Sub
    Public Sub getprofilo(iduser As String)

        sqlstring = "select a.*,b.firstname,b.lastname,b.email from (ai_userprofile a join core_user b on a.iduser=b.idst ) where iduser=" & iduser

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")
        msg = jsonresult

    End Sub

    Function getcategoryuser()

        If Session("idprofile") = 1 Then
            sqlstring = "select (select count(*) from aula_docenti where idcategory=a.idCategory) as iscrittidocenti,  (select count(*)  from aula_studenti where idcategory=a.idCategory) as  iscrittistudenti ,a.date_create,a.description,a.codice,a.idcategory as id from core_category_users a"
        Else
            sqlstring = "select date_create,description,codice,idcategory as id from core_category_users where idcreator=" & Session("iduser")
        End If


        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")
        msg = jsonresult
    End Function
    Function getallcatprofile()

        Dim jsonresult As String = String.Empty

        Dim dt As DataTable = Nothing

        Try

            sqlstring = "select * from core_profile order by sequence asc"

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            jsonresult &= " <select > "

            For Each dr In dt.Rows
                jsonresult &= "<option value='" & dr("id") & "'>" & dr("nome").ToString & "</option> "

            Next

            jsonresult &= "</select>"
            msg = jsonresult

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function
    Function insertcategory()
        Try
            Dim codiceclasse As String = LCase(CreateRandomPassword(6))

            sqlstring = "insert into core_category_users (description,codice,idcreator) values('" & EscapeMySql(Request.Form("description")) & "','" & codiceclasse & "'," & Session("iduser") & ")"

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "select max(idcategory) as idcategory from core_category_users where idcreator= " & Session("iduser")

            Dim idcategory As Integer = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("idcategory")

            sqlstring = "insert into aula_docenti (iduser,idcategory) values(" & Session("iduser") & "," & idcategory & ")"


            conn.Execute(sqlstring, CommandType.Text, Nothing)
            msg = "Classe creata"
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Function


    Function insertcategoryinvite()
        Try

            sqlstring = "insert into core_category_invite (nome,email,idcategory) values('" & EscapeMySql(Request.Form("nome")) & "','" & Request.Form("email") & "'," & Request.Form("idcategory") & ")"

            conn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
        End Try
    End Function

    Function deletecategory()

        Try

            sqlstring = "delete from core_category_user where id=" & Request.Form("id")

            conn.Execute(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
        End Try


    End Function
    Public Sub updateprofile(iduser As Integer)

        For i = 3 To Request.Form.AllKeys.Length - 1

            Try
                sqlstring = " update ai_userprofile set " & Replace(Request.Form.Keys(i), "[]", "") & " = '" & Request.Form(i) & "' where iduser= " & iduser
                conn.Execute(sqlstring, CommandType.Text, Nothing)
            Catch ex As Exception
            End Try

        Next

        msg = "Aggiornamento completato!"

    End Sub

#End Region
#Region "stats"

    Public Sub getreportvalutazioni(iduser, idcourse)

        Try

            sqlstring = "select a.idorg as id,concat(coalesce((select title from learning_organization where idorg=a.idparent),''),' -> ',a.title ) as test,b.score,b.number_of_attempt,b.date_attempt,b.score_status from ((learning_organization a join learning_testtrack b on a.idorg=b.idreference)) where a.idcourse=" & idcourse & " and b.iduser=" & iduser & " and objecttype='test' order by path asc;"

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub

    Public Function createdatatable()
        Dim mydatatable As New DataTable

        Dim mydatacolumn As New DataColumn()
        mydatacolumn.ColumnName = "id"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "nominativo"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "date_inscr"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "date_complete"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "mediatentativi"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "media"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        Return mydatatable
    End Function
    Public Function createdatatableuser()
        Dim mydatatable As New DataTable

        Dim mydatacolumn As New DataColumn()
        mydatacolumn.ColumnName = "id"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "iduser"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)


        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "idcourse"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)


        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "nominativo"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "date_inscr"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "date_complete"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "temposessione"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "tempovideocorso"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "mediatentativi"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "media"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "percentuale"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        mydatacolumn = New DataColumn()
        mydatacolumn.ColumnName = "percorso"
        mydatacolumn.DataType = System.Type.GetType("System.String")
        mydatatable.Columns.Add(mydatacolumn)

        Return mydatatable
    End Function
    Function getTime(iduser As Integer, idcourse As Integer, fullname As String, Optional savefile As Boolean = False)

        Dim tempivideocorso As String
        Dim nomefile As String = FormattaNominativo(EscapeMySql(fullname))
        Dim sb As New StringBuilder()

        sqlstring = "select code,name,date_inscr,date_complete,learning_courseuser.status,learning_courseuser.idCourse,learning_courseuser.iduser from learning_courseuser join learning_course on learning_course.idCourse=learning_courseuser.idCourse where learning_courseuser.idcourse=" & idcourse & " and learning_courseuser.iduser=" & iduser & " order by date_inscr asc "
        Dim dr As DataRow = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)
        Dim strhtml As String = String.Empty


        Try
            strhtml &= "<center><h1>REPORT</h1></center><br>"
            strhtml &= "Utente: <b >" & nomefile & "</b><br>"
            strhtml &= "Corso: <b >" & dr("code").ToString & "| " & dr("name").ToString & "</b><br>"
            strhtml &= "Data Iscrizione:<b> " & dr("date_inscr").ToString & "</b><br>"
            strhtml &= "Data Completamento:<b> " & dr("date_complete").ToString & "</b><br>"
            strhtml &= "Ultimo Accesso al corso:<b> " & utility.getLastCourseAccess(idcourse, iduser, conn) & "</b><br>"
            strhtml &= "Ultimo Accesso alla piattaforma:<b> " & utility.getLastAccess(iduser, conn) & "</b><br>"
            strhtml &= "Stato:<b> " & utility.GetPercentuale(iduser, idcourse, conn, dr("status").ToString) & "</b>"

            Dim sqlstringFirst = " Select SUM(TIME_TO_SEC(TIMEDIFF( lasttime,entertime))) As totaltime from learning_tracksession where idCourse=" & dr("idCourse").ToString & "  And idUser=" & iduser
            Dim dt As DataTable = conn.GetDataTable(sqlstringFirst, CommandType.Text, Nothing)
            Dim tot_time As String = dt.Rows(0)("totaltime").ToString





            tempivideocorso = ""

            strhtml &= "<br>Ore VideoCorsi :<b> " & tempivideocorso & "</b>"
            strhtml &= "<br>Ore permanenza in piattaforma: <b>" & utility.ConvertSecToDate(tot_time) & "</b>"
            strhtml &= "<br>Numero connessioni: <b>" & utility.getconnection(iduser, idcourse) & "</b>"
            strhtml &= "<br>Materiale visionato: <b>" & utility.getmateriale(iduser, idcourse) & "</b>"

        Catch ex As Exception

        End Try
        sb.Append(strhtml)


        sqlstring = "select a.title,total_time,duration,(select title from learning_organization where idorg=a.idparent) as modulo from ((learning_organization a left join learning_scorm_tracking b on a.idorg=b.idreference) join  learning_Scorm_organizations c on a.idresource=c.idscorm_organization) join learning_Scorm_package d on d.idscorm_package=c.idscorm_package where a.objecttype='scormorg' and idcourse=" & idcourse & " and b.idUser=" & iduser & " order by a.path asc"
        dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)





        If dt.Rows.Count > 0 Then
            sb.Append(vbCrLf)
            sb.Append("<h2>Tempi Videocorsi</h2>")
            sb.Append("<table width=""1100px"" border=1><tr style=""font-weight: bold"">")
            sb.Append("<td>Titolo Videocorso</td>td><Durata visione utente</td></tr>")
            sb.Append(vbCrLf)
            Dim durationtotal As Integer = 0
            Dim duration As Integer = 0
            For Each row As DataRow In dt.Rows
                durationtotal += 0 ' Int(row("duration"))
                duration = 0 ' & row("duration")
                'duration 
                sb.Append("<tr><td>" & row("modulo") & " - " & row("title") & "</td><td>" & row("total_time") & " </td></tr>")
                sb.Append(vbCrLf)
            Next
            sb.Append("<tr style=""font-weight: bold"" ><td>Totali</td><td><b>" & tempivideocorso & "</b> </td></tr>")

            sb.Append("</table>")
        End If

        sqlstring = "select (select title from learning_organization where idorg=a.idparent) as modulo ,title,b.last_complete,status,b.idreference,b.objecttype,b.firstattempt from learning_organization a join learning_commontrack b on a.idorg=idreference where  idcourse=" & idcourse & " and b.idUser=" & iduser & " order by b.firstattempt asc"

        dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        If dt.Rows.Count > 0 Then
            sb.Append(vbCrLf)

            sb.Append("<h2>Dettaglio Oggetti Didattici</h2>")
            sb.Append("<table width=""1100px"" border=1 > <tr style=""font-weight: bold"">")
            sb.Append("<td>Oggetto didattico</td><td>Data accesso</td><td>Data completamento</td><td>Stato</td><td>Voto</td></tr>")
            sb.Append(vbCrLf)



            For Each row As DataRow In dt.Rows
                Try
                    If (row("objecttype") = "test") Then
                        sb.Append("<tr><td>" & row("modulo") & " - " & row("title") & "</td><td>" & row("firstattempt").ToString & "</td><td>" & row("last_complete").ToString & "</td><td>completato</td><td>" & getvototest(iduser, row("idreference"), "") & "</td></tr>")
                    Else
                        sb.Append("<tr><td>" & row("modulo") & " - " & row("title") & "</td><td>" & row("firstattempt").ToString & "</td><td>" & row("last_complete").ToString & "</td><td>completato</td><td></tr></tr>")

                    End If
                Catch ex As Exception
                    LogWrite(ex.ToString)
                End Try
                sb.Append(vbCrLf)
            Next
            sb.Append("</table>")




            sb.Append("<h2>Numero Connessioni</h2>")
            sb.Append("<table width=""1100px"" border=1 > <tr style=""font-weight: bold"">")
            sb.Append("<td>N.</td><td>Inizio Sessione</td><td>Fine Sessione</td><td>Durata Sessione</td><td>N. Operazioni</td></tr>")
            sb.Append(vbCrLf)
        End If
        Try

            Dim sqlstring = "select identer as id, entertime,lasttime,numop,(select  CONCAT(' ',SUM(TIME_TO_SEC(TIMEDIFF(lasttime,entertime)))) from learning_tracksession  where identer=id) as duration from learning_tracksession where entertime != lasttime and  idcourse=" & idcourse & " and iduser=" & iduser & "  order by entertime asc "
            Dim i As Integer = 1

            Dim dt As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    Try

                        sb.Append("<tr><td>" & i & "</td><td>" & Convert.ToDateTime(row("entertime")) & "</td><td>" & Convert.ToDateTime(row("lasttime")) & "</td><td>" & utility.ConvertSecToDate(row("duration")) & "</td><td>" & row("numop") & "</td></tr>")
                        i = i + 1

                    Catch ex As Exception
                        LogWrite(ex.ToString)
                    End Try
                    sb.Append(vbCrLf)
                Next
            End If

        Catch ex As Exception

        End Try
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
                nomefile = nomefile
                doc.Save(HttpContext.Current.Server.MapPath("report") & "\Report - " & nomefile & ".pdf")
                doc.Close()
                Return HttpContext.Current.Server.MapPath("report") & "\Report - " & nomefile & ".pdf"
            Else
                doc.Save(HttpContext.Current.Response, True, "./Report - " & nomefile & ".pdf")
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

        Dim idcourse As Integer = Request.QueryString("idcourse")

        Dim iduser As String = Request.QueryString("iduser")

        Dim dttrack = createdatatable()

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

            Response.ContentType = "application/json"

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

                Dim page As String = Request.QueryString("page")

                Dim pagesize As String = Request.QueryString("rows")

                Dim sidx As String = Request.QueryString("sidx")

                Dim sord As String = Request.QueryString("sord")

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

                    Dim dttrack = createdatatableuser()

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





                            Dim time As String = dtimevideo.Rows(0)("total_time")

                            Dim second As String = String.Empty

                            Dim minutes As String = String.Empty

                            Dim hour As String = String.Empty

                            Dim time_sec As Integer = 0
                            Dim time_min As Integer = 0
                            Dim time_hour As Integer = 0

                            r("tempovideocorso") = utility.ConvertSecToDate(objuser.getUserTotalVideocourseSec(iduser, idcourse))
                            r("temposessione") = utility.ConvertSecToDate(dtime.Rows(0)("total_time"))

                        Catch ex As Exception

                        End Try

                        dttrack.rows.add(r)

                    Next

                    'filldatatable(dt, dttrack)

                    ' dim jsonresult as string = utility.getjson(dt)

                    ' response.contenttype = "application/json"

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

                Dim page As String = Request.QueryString("page")

                Dim pagesize As String = Request.QueryString("rows")

                Dim sidx As String = Request.QueryString("sidx")

                Dim sord As String = Request.QueryString("sord")

                Dim range As String = Request.QueryString("range")

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

                Dim dttrack = createdatatableuser()

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

                            r("tempovideocorso") = utility.ConvertSecToDate(objuser.getUserTotalVideocourseSec(iduser, idcourse))
                            r("temposessione") = utility.ConvertSecToDate(dtime.Select("iduser=" & iduser & "")(0)("total_time"))

                        Catch ex As Exception
                            LogWrite(ex.ToString)
                        End Try

                        dttrack.rows.add(r)

                    Next

                    'filldatatable(dt, dttrack)

                    ' dim jsonresult as string = utility.getjson(dt)

                    ' response.contenttype = "application/json"

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

            Response.ContentType = "application/json"

            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

        Return False

    End Function
#End Region

#Region "faq"
    Function saveadditionalfaq(idcategory As String)

        Dim question As String = String.Empty

        Dim title As String = String.Empty

        Dim answer As String = String.Empty

        Dim id_catagory As String = String.Empty

        Dim sequence As Integer = 1
        Dim idfaq As Integer = Request.QueryString("idfaq")
        Try

            For i = 0 To Request.Form.AllKeys.Length Step 2
                Try

                    question = EscapeMySql(Request.Form(i).ToString)
                    answer = EscapeMySql(Request.Form(i + 1).ToString)
                    If idfaq <> "0" Then
                        sqlstring = " update learning_faq set question='" & question & "',answer='" & answer & "' where idcategory=" & idcategory & " and idfaq=" & idfaq & " "

                    Else
                        sqlstring = "select max(sequence) as maxsequence from learning_faq where idcategory=" & idcategory

                        Try
                            sequence = CInt(conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("maxsequence").ToString)
                        Catch ex As Exception
                            sequence = 1
                        End Try

                        sqlstring = " insert into learning_faq ( idcategory , question , title , answer , sequence ) " &
" values (" & idcategory & ", '" & question & "', '" & title & "', '" & answer & "'," & sequence + 1 & ") "

                    End If

                    conn.Execute(sqlstring, CommandType.Text, Nothing)
                    sequence = sequence + 1

                Catch ex As Exception
                    LogWrite(ex.ToString)

                End Try
            Next

            msg = "Aggiornamento completato"

        Catch ex As Exception
            LogWrite(ex.ToString)

        End Try

        Return False

    End Function

#End Region


#Region "propedeuticità"

    Function updateprops(ids As String, idorg As String, ckendcourse As Boolean, ckivisible As Boolean, title As String, idcourse As Integer)
        Try

            If ckendcourse Then
                sqlstring = "update learning_organization set isterminator=0 where idcourse=" & idcourse
                conn.Execute(sqlstring, CommandType.Text, Nothing)
            End If

            sqlstring = "update learning_organization set title='" & EscapeMySql(title) & "',isterminator=" & ckendcourse & " , visible=" & ckivisible & ", prerequisites='" & ids & "' where idorg=" & idorg

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            Return "oggetto modificato "

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)

            Return ex.Message
        End Try

        Return False

    End Function

    Function editprops(iduser As String, idcourse As String, status As Integer)
        Try

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function deleteprops(prerequisites As String, idorg As String)
        Try

            sqlstring = "update learning_organization set prerequisites=replace(" & prerequisites & ",'') where idorg=" & idorg

            conn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function getpropsassigned(idorg As String)

        sqlstring = "select idorg as id,title from learning_organization where idorg in (select prerequisites from learning_organization where idorg=" & idorg & " ) order by path asc "

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

        Return False

    End Function
    Function getpropsavailable(idorg As Integer, idcourse As String)

        sqlstring = "select idorg as id,title,prerequisites from learning_organization where objecttype!='' and idcourse=" & idcourse & " order by path asc"

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

        Return False

    End Function
    Function getprops(idorg As String)

        sqlstring = "select iduser,userid,firstname,lastname,register_date,email from core_user join learning_courseuser on learning_courseuser.iduser= core_user .idst where idcourse=" & Request.QueryString("courseid") & " order by register_date desc "

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

        Return False

    End Function
#End Region


#Region "students"
    Function insertstudents(ids As String, idcourse As String)
        Dim esito As String
        Dim listusers() As String = ids.Split(",")
        Try
            Dim j = 1
            For i = 0 To listusers.Length - 1
                Try
                    If (IsNumeric(Num(listusers(i)))) Then
                        sqlstring = "insert into learning_courseuser ( iduser , idcourse , date_inscr , status , date_begin_validity , date_expire_validity ) " &
"values (" & Num(listusers(i)) & " ," & idcourse & ", '" & ConvertToMysqlDateTime(Now) & "',0, null, null);"

                        conn.Execute(sqlstring, CommandType.Text, Nothing)
                        utility.SendBenvenuto(esito, Num(listusers(i)), idcourse)

                        msg = j & " utente/i iscritti al corso"
                        j = j + 1
                    End If


                Catch ex As Exception
                    SharedRoutines.LogWrite(ex.ToString)
                End Try
            Next
            Return True
        Catch ex As Exception
        End Try


    End Function

    Function editstudents(iduser As String, idcourse As String, validfrom As String, validto As String, status As Integer)
        Try
            If iduser <> "" Then
                Try
                    validto = ConvertToMysqlDateTime(validto)
                Catch ex As Exception
                    validto = ""
                End Try

                Try
                    validfrom = ConvertToMysqlDateTime(validfrom)
                Catch ex As Exception
                    validfrom = ""
                End Try

                sqlstring = "update learning_courseuser set date_complete='" & ConvertToMysqlDateTime(Now) & "', date_begin_validity='" & validfrom & "',date_expire_validity='" & validto & "', status=" & status & " where idcourse=" & idcourse & " and iduser=" & iduser
                conn.Execute(sqlstring, CommandType.Text, Nothing)
            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function getstudents(idcourse As String)

        sqlstring = "select idst as id,firstname,lastname,concat(firstname , ' ',lastname) as fullname,email,(select description from core_category_users where idCategory=b.idcategory) as category, (select iduser from learning_courseuser where iduser=idst and idcourse=" & idcourse & " ) as iscritto,userid  from core_user a  left join aula_studenti b on a.idst=b.iduser where a.idprofile=3 and idst not in (select iduser from learning_courseuser where idcourse=" & idcourse & ")"

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

        Return False

    End Function

    Function getstudentsassign(idcourse As String)


        sqlstring = "select idst as id,userid ,firstname,lastname,concat(firstname , ' ',lastname) as fullname,(select description from core_category_users where idcategory in (select idcategory from aula_studenti where iduser=a.idst) ) as category,register_date,email,status,date_complete,date_begin_validity,date_expire_validity, (select iduser from learning_courseuser where iduser=idst and idcourse=" & idcourse & " ) as iscritto from core_user a left join learning_courseuser on learning_courseuser.iduser=a.idst where idcourse=" & idcourse & ""

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

        Return False

    End Function



#End Region


#Region "course"

    Function insertcat()

        Try

            sqlstring = "insert into learning_category" & " ( path) values ( '" & Request.Form("cat").ToString & "')"

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            msg = "Inserimento completato"
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    Function getallcatcourse()

        Dim jsonresult As String = String.Empty

        Dim dt As DataTable = Nothing

        Try

            sqlstring = "select * from learning_category order by description asc "
            '              sqlstring = "select node.idcategory, concat( concat(up1.description ,' -> ' ,node.description ),' -> ', cast(ifnull(up2.description, ' ') as char(50))) as description " &
            '" from learning_category as node left outer join learning_category as up1 on up1.idcategory = node.idparent left outer join learning_category as up2 " &
            '" on up2.idcategory = up1.idparent left outer join learning_category as up3 on up3.idcategory = up2.idparent where up1.description is not null and node.description is not null order by up1.description asc "

            sqlstring = "select * from learning_category order  by path asc "

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            jsonresult &= " <select > "

            jsonresult &= "<option value='-1'>senza categoria</option> "

            For Each dr In dt.Rows
                jsonresult &= "<option value='" & dr("idcategory") & "'>" & dr("description").ToString & "</option> "

            Next

            jsonresult &= "</select>"
            msg = jsonresult

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Public Function getcoursecompleted(iduser As String)

        Try

            sqlstring = "Select a.flagcertificate, a.idcourse As id,b.name,a.date_inscr,a.date_complete,a.status,a.date_begin_validity,a.date_expire_validity from (learning_courseuser a join learning_course b On a.idcourse=b.idcourse )     where  a.iduser=" & iduser & " "

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            Dim dt As DataTable = Nothing

            FillDataTable(dt, dtoriginal)
            Dim jsonresult As String = utility.GetJson(dt)
            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")
            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    Function getstatus()

        Dim jsonresult As String = String.Empty

        Try

            jsonresult &= " <select> "

            jsonresult &= "<option value='-1'>vuoto</option> "

            jsonresult &= " <option value='1'>in corso</option> "
            jsonresult &= " <option value='0'>iscritto</option> "
            jsonresult &= " <option value='2'>completato</option> "
            jsonresult &= " <option value='3'>sospeso</option> "

            jsonresult &= "</select>"
            msg = jsonresult

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function
    Public Sub getcourseteacher()
        Try
            '            Dim sqlstring As String = "select idcourse as id,a.code,a.name,a.controlvideo,a.img_course,credits,path,c.name as certificate,e.name as certificateattendance,(select concat( concat(up1.description ,' -> ' ,node.description ),' -> ', cast(ifnull(up2.description, ' ') as char(50))) as title from learning_category as node left outer join learning_category as up1 on node.idparent=up1.idcategory left outer join learning_category as up2 on up2.idcategory = up1.idparent left outer join learning_category as up3 on up3.idcategory = up2.idparent where up1.description is not null and node.description is not null and node.idcategory=a.idcategory order by up1.description asc) as category ," &
            '"(select count(*) from learning_courseuser where idcourse=id) as students " &
            '"from ((((learning_course a left join learning_category on a.idcategory=learning_category.idcategory) left join learning_certificate_course b on a.idcourse=b.id_course) left join learning_certificate c on c.id_certificate=b.id_certificate) left join learning_certificate_attendance_course d on a.idcourse=d.id_course) left join learning_certificate_attendance e on e.id_certificate=d.id_certificate " &
            '" and idcreator=" & Session("iduser") & " order by idcourse desc"

            Dim sqlstring As String = "select * from learning_courseteacher order by idcourse desc"


            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            Dim dt As DataTable = Nothing

            FillDataTable(dt, dtoriginal)
            Dim jsonresult As String = utility.GetJson(dt)
            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")
            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Sub

    Public Sub getcoursetest()
        Try


            Dim sqlstring As String = "select a.idcourse as id,a.name,a.code,a.credits,c.* from learning_course a left join learning_category  c on a.idcategory=c.idCategory where idcreator=" & Session("iduser") & " and a.idcategory=0  order by a.idcourse desc"


            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            Dim dt As DataTable = Nothing

            FillDataTable(dt, dtoriginal)
            Dim jsonresult As String = utility.GetJson(dt)
            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")
            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub
    Public Sub getcourse(Optional ByVal idcategory As Integer = 0)
        Try


            Dim sqlstring As String = "select a.idcourse as id,a.name,a.code,a.credits,c.* from learning_course a left join learning_category  c on a.idcategory=c.idCategory where idcreator=" & Session("iduser") & " and a.idcategory <> 0  order by a.idcourse desc"


            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            Dim dt As DataTable = Nothing

            FillDataTable(dt, dtoriginal)
            Dim jsonresult As String = utility.GetJson(dt)
            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")
            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub

    Function insertcourse(description As String, code As String, name As String, credits As String, ByRef idcourse As Integer)




        Try

            sqlstring = "insert into learning_course ( idcategory, code , name ,  credits,idcreator) " &
" values ( '" & description & "','" & code & "','" & name & "', '" & credits & "'," & Session("iduser") & ");"
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "select max(idcourse) as idcoursenew from learning_course"
            idcourse = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("idcoursenew")
            msg = "Inserimento completato"
        Catch ex As Exception
            msg = "Errore inserimento corso"
            SharedRoutines.LogWrite("Inserimento Corso " & Now & " " & ex.ToString)
        End Try


        Return False

    End Function
    Function insertcoursetest(description As String, code As String, name As String, credits As String, ByRef idcourse As Integer)




        Try

            sqlstring = "insert into learning_course ( description, code , name ,  credits,idcreator ,idcategory) " &
" values ( '" & description & "','" & code & "','" & name & "', '" & credits & "'," & Session("iduser") & ",0);"
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "select max(idcourse) as idcoursenew from learning_course"
            idcourse = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("idcoursenew")
            msg = "Inserimento completato"
        Catch ex As Exception
            msg = "Errore inserimento corso"
            SharedRoutines.LogWrite("Inserimento Corso " & Now & " " & ex.ToString)
        End Try


        Return False

    End Function

    Function editcourse(idcourse As String)
        Try
            ' featuredcourse = " & request.form("featuredcourse") & ", linkcourse = '" & request.form("linkcourse") & "', blockstatus = '" & request.form("blockstatus") & "'
            sqlstring = "update learning_course set idcategory=" & Request.Form("description") & " ,code = '" & Request.Form("code") & "', name ='" & EscapeMySql(Request.Form("name").ToString) & "',  credits = '" & Request.Form("credits") & "' where idcourse= " & idcourse

            conn.Execute(sqlstring, CommandType.Text, Nothing)
            msg = "Aggiornamento completato"
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    Function deletecourse(idcourse As String)

        Try
            sqlstring = "delete from learning_course where idcourse=" & idcourse
            conn.Execute(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Sub getuserscourse(id As Integer)

        Dim jsonresult As String = String.Empty

        Dim dt As DataTable = Nothing

        Try

            sqlstring = "select count(*) as num from learning_courseuser where idcourse=" & id

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            For Each dr In dt.Rows
                jsonresult &= dr("num").ToString

            Next

            msg = jsonresult

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

    End Sub

#End Region


#Region "user"



    Public Sub getuser()

        Try

            sqlstring = "select (select count(*) from learning_courseuser where iduser=a.idst) as iscrizioni,idst as id,firstname,lastname,userid,pass,email,cf,scuola,datanascita,register_date as datestart,clearpass,p.nome as profile from core_user a left join core_profile p on p.id=a.idprofile where (flagdelete is null or flagdelete=0) order by register_date desc"

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub

    Sub insertusers()

        Try

            Dim nome As String = Request.Form("firstname").ToString
            Dim cognome As String = Request.Form("lastname").ToString
            Dim email As String = Request.Form("email").ToString
            Dim cf As String = Request.Form("cf").ToString
            Dim tel As String = Request.Form("tel").ToString
            Dim profile As Integer = Request.Form("profile")

            Dim username As String = utility.createusername(nome, cognome)
            Dim passwordReal As String = LCase(SharedRoutines.CreateRandomPassword(6))
            Dim password = SharedRoutines.getMd5Hash(passwordReal)
            username = utility.findusername(username, nome, cognome, conn)


            Dim dataiscrizione As String = ConvertToMysqlDateTime(Now)



            sqlstring = " insert into core_st ( idst ) values(null); insert into core_user " &
" ( idst,idprofile, userid, firstname, lastname, pass, email ,cf,tel,register_date,clearpass) " &
"values ( last_insert_id(), " & profile & ",'" & username & "', '" & Trim(FormattaNominativo(nome.Replace("'", "\'"))) & "', '" & Trim(FormattaNominativo(cognome.Replace("'", "\'"))) & "', " &
"'" & password & "', '" & LCase(email) & "', '" & UCase(cf) & "', '" & UCase(tel) & "','" & String.Format("{0:u}", dataiscrizione) & "','" & passwordReal & "')"

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            Dim idst As Integer
            Try

                idst = conn.GetDataTable("select idst from core_user order by idst desc")(0)("idst")

            Catch ex As Exception
                SharedRoutines.LogWrite(ex.ToString)
            End Try

            utility.SendMail(esito, mailpiattaforma, nome, cognome, email, username, passwordReal, HttpContext.Current.Request.Url.Host, "sign")
            msg = "Inserimento completato"

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

    End Sub

    Function editusers(iduser As String)
        Try
            Dim userid As String = Request.Form("userid").ToString
            If iduser <> "" Then

                If Request.Form("password") <> "" And Request.Form("confpass") <> "" And Request.Form("confpass").Equals(Request.Form("password")) Then
                    sqlstring = "update core_user set userid='" & userid & "',pass='" & getMd5Hash(Request.Form("password")) & " firstname='" & Request.Form("firstname") & "', lastname='" & Request.Form("lastname") & "',email='" & Request.Form("email") & "' where idst=" & iduser
                Else
                    sqlstring = "update core_user set idprofile=" & Request.Form("profile") & ", userid='" & userid & "', firstname='" & Request.Form("firstname") & "', lastname='" & Request.Form("lastname") & "',email='" & Request.Form("email") & "',cf='" & Request.Form("cf") & "', idcategory=" & Request.Form("category") & " where idst=" & iduser

                End If
                Session("fullname") = Request.Form("firstname") & "," & Request.Form("lastname")

                conn.Execute(sqlstring, CommandType.Text, Nothing)

            End If

            msg = "Aggiornamento completato"

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function deleteusers(iduser As String)

        Try
            If iduser <> "" Then
                sqlstring = "update core_user set flagdelete=1 where idst in(" & iduser & ")"
                conn.Execute(sqlstring, CommandType.Text, Nothing)

            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

#End Region

#Region "teacher"
    Function getteacher(idsede As String)

        'sqlstring = "select iduser as id,userid ,firstname,lastname,register_date,email,status,date_complete,date_begin_validity,date_expire_validity, (select iduser from " & dbtable.userevent & " where iduser=idst and idcourse=" & idcourse & ") as iscritto from " & dbtable.userlist & " join " & dbtable.userevent & " on " & dbtable.userevent & ".iduser= " & dbtable.userlist & " .idst order by register_date desc "
        sqlstring = "select idst as id,firstname,lastname,concat(firstname , ' ',lastname) as fullname,email, (select idteacher from aula_sedi where idteacher=idst and id=" & idsede & " ) as iscritto,userid from core_user where idprofile=2 and idst not in (select idteacher from aula_sedi where idteacher is not null and id=" & idsede & ")"

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

    End Function

    Function getteachersassign(idsede As String)

        'sqlstring = "select iduser as id,userid ,firstname,lastname,register_date,email,status,date_complete,date_begin_validity,date_expire_validity, (select iduser from " & dbtable.userevent & " where iduser=idst and idcourse=" & idcourse & ") as iscritto from " & dbtable.userlist & " join " & dbtable.userevent & " on " & dbtable.userevent & ".iduser= " & dbtable.userlist & " .idst order by register_date desc "
        sqlstring = "select idst as id,userid ,firstname,lastname,concat(firstname , ' ',lastname) as fullname,register_date,core_user.email, (select idteacher from aula_sedi where idteacher=idst and id=" & idsede & " ) as iscritto from core_user left join aula_sedi a on a.idteacher= core_user.idst where idprofile=2 and a.id=" & idsede & ""

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

    End Function
#End Region


#Region "aula"



    Sub jgettest()

        Dim jsonresult As String = String.Empty
        Dim dt As DataTable = Nothing
        Dim selected As String = String.Empty
        Dim bvis As String = String.Empty
        Dim strselect As String = String.Empty

        Try

            sqlstring = "Select * from learning_kb_res where r_type='test' and r_name like '%aula%' order by r_name asc"

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            strselect &= "$('select[name=""testaula""]').empty();" & vbCrLf

            For Each dr In dt.Rows
                strselect &= "$('select[name=""testaula""').append($('<option ></option>').val(""" & dr("r_item_id") & """).html(""" & Replace(dr("r_name").ToString, "'", "\'") & """).prop('selected',""" & selected & """));" & vbCrLf
            Next

            msg = strselect

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

    End Sub

    Public Function getquestions(dt As DataTable, ByRef h As Hashtable)

        Dim jsonresult As String = String.Empty

        Dim idquestion As Integer
        Dim question As String = String.Empty
        Dim answer As String = String.Empty
        Dim iscorrect As String = String.Empty

        Dim i As Integer = 1
        Dim v As Integer = 0

        Try
            For k = 0 To dt.Rows.Count - 1

                sqlstring = "select a.idquest,title_quest,answer,score_correct,is_correct from learning_testquest a join learning_testquestanswer b on a.idquest=b.idquest where a.idquest=" & dt.Rows(k)("idquest") & " order by a.idquest asc "

                Dim dt1 As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                For j = 0 To dt1.Rows.Count

                    If dt1.Rows.Count = j Then
                        v = 0
                        answer = answer.Remove(answer.Length - 1, 1)
                        answer = answer & vbCrLf

                        jsonresult &= i & "." & question & vbCrLf
                        If Not (answer.StartsWith("a)") Or answer.StartsWith("b)") Or answer.StartsWith("c)")) Then
                            Select Case v
                                Case 0
                                    answer = "a) " & answer
                                Case 1
                                    answer = "b) " & answer
                                Case 2
                                    answer = "c) " & answer
                            End Select
                        End If
                        jsonresult &= answer & vbCrLf
                        answer = ""
                        question = ""
                        Exit For
                    End If

                    Dim tmpanswer As String = dt1.Rows(j)("answer").ToString

                    Dim tmpquestion As String = dt1.Rows(j)("title_quest").ToString

                    idquestion = dt1.Rows(j)("idquest").ToString

                    question = tmpquestion

                    If Not (tmpanswer.StartsWith("a)") Or tmpanswer.StartsWith("b)") Or tmpanswer.StartsWith("c)")) Then
                        Select Case v
                            Case 0
                                tmpanswer = "a) " & tmpanswer
                            Case 1
                                tmpanswer = "b) " & tmpanswer
                            Case 2
                                tmpanswer = "c) " & tmpanswer
                        End Select
                    End If

                    answer &= tmpanswer & vbCrLf

                    iscorrect = dt1.Rows(j)("is_correct").ToString

                    If iscorrect = 1 Then
                        iscorrect = "*"
                    Else
                        iscorrect = ""
                    End If

                    Select Case v
                        Case 0
                            h.Add("[a" & i & "]", iscorrect)
                        Case 1
                            h.Add("[b" & i & "]", iscorrect)
                        Case 2
                            h.Add("[c" & i & "]", iscorrect)
                    End Select

                    v = v + 1

                Next
                i = i + 1
            Next

        Catch ex As Exception
            msg &= ex.Message
            LogWrite(ex.ToString)
        End Try

        Return jsonresult
    End Function



    Sub checkdata()
        Dim date1 As String = Convert.ToDateTime(Now).ToString("yyyy-MM-dd")
        Dim date2 As String = Convert.ToDateTime(DateAdd(DateInterval.Day, 1, Now)).ToString("yyyy-MM-dd")
        Dim sqlstring As String = "select * from aula_sessioni a join aula_prenotazioni b on a.id=b.idsessione where date(datastart) = '" & date1 & "' or date(datastart) = '" & date2 & "' "
        Dim dt As DataTable = Nothing

        dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        Dim sqldate As String = String.Empty

        For Each dr In dt.Rows
            If System.Configuration.ConfigurationManager.AppSettings("time") = DateTime.Now.ToString("hh:mm") Then
                sqldate = "evento imminente"
            End If
        Next

        msg = sqldate
    End Sub

    Sub getaula()
        Try

            sqlstring = "select * from aula_aula order by numordine desc"

            Dim dtoriginal As DataTable = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            For i = 0 To dtoriginal.Rows.Count - 1
                dtoriginal.Rows(i)("luogo") = dtoriginal.Rows(i)("luogo").ToString.Split("=")(1)

            Next

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Sub



    Sub getregione()

        Dim jsonresult As String = String.Empty

        Dim dt As DataTable = Nothing

        Dim selected As String = String.Empty

        Dim bvis As String = String.Empty

        Try

            sqlstring = "select distinct(regione) from regioni_province order by regione asc"

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            jsonresult &= " <select> "

            jsonresult &= "<option value='-1'></option> "

            For Each dr In dt.Rows
                Try
                    If dr("idsedi") = dr("id") Then
                        selected = "selected='selected'"
                    End If
                Catch ex As Exception
                End Try
                jsonresult &= "<option " & selected & " value='" & dr("regione") & "'>" & dr("regione").ToString & "</option> "
                selected = ""
            Next

            jsonresult &= "</select>"
            msg = jsonresult

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub

    Sub getprovincia()

        Dim jsonresult As String = String.Empty

        Dim dt As DataTable = Nothing

        Dim selected As String = String.Empty

        Dim bvis As String = String.Empty

        Try

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            jsonresult &= " <select> "

            jsonresult &= "<option value='-1'></option> "

            For Each dr In dt.Rows
                Try
                    'if dr("idsedi") = dr("provincia") then
                    ' selected = "selected='selected'"
                    'end if
                Catch ex As Exception
                End Try
                jsonresult &= "<option " & selected & " value='" & dr("provincia") & "'>" & dr("provincia").ToString & "</option> "
                selected = ""
            Next

            jsonresult &= "</select>"
            msg = jsonresult

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub

    Sub getcomuni()

        Dim jsonresult As String = String.Empty

        Dim dt As DataTable = Nothing

        Dim selected As String = String.Empty

        Dim bvis As String = String.Empty

        Try

            If Request.QueryString("visibile") = "1" Then
                bvis = " where visibile=1 "
            End If
            If Request.QueryString("id") <> "" Then
                sqlstring = "select *,(select idsede from aula_sessioni where id=" & Request.QueryString("id") & ") as idsedi from aula_sedi " & bvis & " order by comune asc"
            Else
                sqlstring = "select * from aula_sedi " & bvis & " order by comune asc"

            End If

            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            jsonresult &= " <select> "

            jsonresult &= "<option value='-1'></option> "

            For Each dr In dt.Rows
                Try
                    If dr("idsedi") = dr("id") Then
                        selected = "selected='selected'"
                    End If
                Catch ex As Exception
                End Try
                jsonresult &= "<option " & selected & " value='" & dr("id") & "'>" & dr("comune").ToString & " - " & dr("lastname").ToString & " " & dr("firstname").ToString & " </option> "
                selected = ""
            Next

            jsonresult &= "</select>"
            msg = jsonresult

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub


    Sub jgetallclass()


        Dim dt As DataTable = Nothing

        Dim selected As String = String.Empty

        Dim bvis As String = String.Empty

        Dim input As String = Request.QueryString("type")



        Dim strselect As String = String.Empty
        Try
            sqlstring = "select a.description,a.idcategory from core_category_users a  join aula_docenti b on a.idcategory=b.idcategory where iduser=" & Session("iduser") & " order by a.description asc"


            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = JsonConvert.SerializeObject(dt) ' utility.getjson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

            'dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            'strselect &= "$('select[name=""" & input & """]').empty();" & vbCrLf

            'strselect &= "$('select[name=""" & input & """]').append('<option value=""0"">Tutte le classi</option>');" & vbCrLf

            'For Each dr In dt.Rows
            '    Try
            '        If dr("idcategory") = Request.QueryString("idcategory") Then
            '            selected = "selected"
            '        Else
            '            selected = ""
            '        End If
            '    Catch ex As Exception
            '    End Try

            '    strselect &= "$('select[name=""" & input & """').append($('<option ></option>').val(""" & Replace(dr("idcategory"), "'", "\'") & """).html(""" & Replace(dr("description").ToString, "'", "\'") & """).prop('selected',""" & selected & """));" & vbCrLf

            'Next

            'msg = strselect

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub





    Sub getstudentsession()




        sqlstring = "select idst as id,firstname,lastname,email,tel from core_user a join aula_studenti b on a.idst=b.iduser where b.idcategory=" & Request.QueryString("idcategory") & " and a.idprofile=3 and b.iduser not in(select iduser from aula_prenotazioni where idsessione=" & Request.QueryString("idsessione") & ")  order by lastname asc "


        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = JsonConvert.SerializeObject(dt) ' utility.getjson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

    End Sub


#Region "Assegnazione docenti"

    Function getteachersavailable(idcategory As Integer)


        sqlstring = "select  idst as id,firstname,lastname,email,cf  from core_user a  where a.idst not in (select iduser from aula_docenti where idcategory=" & idcategory & ") and  a.idprofile=2 order by lastname asc"

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

        Return False

    End Function
    Function getgroupteachersassigned(idcategory As Integer)



        sqlstring = "select distinct firstname,lastname,idst as id,b.idcategory,cf,email from (core_user a right  join aula_docenti b on a.idst=b.iduser ) where b.idcategory=" & idcategory

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

        Return False

    End Function

    Public Sub assigngroupteachers(iduser, idcategory)

        Dim listusers() As String = iduser.Split(",")
        Try
            For i = 0 To listusers.Length - 1


                Try
                    If IsNumeric(Num(listusers(i))) Then
                        sqlstring = " insert into `aula_docenti` ( iduser , idcategory  ) " &
" values (" & Num(listusers(i)) & " ," & idcategory & ")"

                        conn.Execute(sqlstring, CommandType.Text, Nothing)

                        msg = i + 1 & " docente/i assegnato/i alla classe"
                    End If
                Catch ex As Exception

                End Try
            Next


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
            msg = ex.ToString
        End Try

    End Sub

    Function delgroupteachers(iduser As String, idcategory As String)
        Try
            If iduser <> "" And idcategory Then
                sqlstring = "delete from aula_docenti where iduser=" & iduser & " And idcategory=" & idcategory
                conn.Execute(sqlstring, CommandType.Text, Nothing)
            End If

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

#End Region
#Region "Assegnazione studenti"
    'Gestione assegnazione studenti

    Function getstudentsavailable(idcategory As Integer)


        sqlstring = "Select  idst As id,firstname,lastname,email,cf  from core_user  a where idst not in (select iduser from aula_studenti ) and a.idprofile=3 order  by lastname asc"

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

        Return False

    End Function
    Function getgroupstudentsassigned(idcategory As Integer)



        sqlstring = "select distinct firstname,lastname,idst as id,b.idcategory,cf,email from (core_user a right  join aula_studenti b on a.idst=b.iduser ) where b.idcategory=" & idcategory

        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult

        Return False

    End Function
    Public Sub assigngroupstudents(iduser, idcategory)

        Dim listusers() As String = iduser.Split(",")
        Try
            For i = 0 To listusers.Length - 1


                Try
                    If IsNumeric(Num(listusers(i))) Then
                        sqlstring = " insert into `aula_studenti` ( iduser , idcategory  ) " &
" values (" & Num(listusers(i)) & " ," & idcategory & ")"

                        conn.Execute(sqlstring, CommandType.Text, Nothing)

                        msg = i + 1 & " studente/i assegnato/i alla classe"
                    End If
                Catch ex As Exception

                End Try
            Next


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
            msg = ex.ToString
        End Try

    End Sub

    Function delgroupstudents(iduser As String, idcategory As String)
        Try
            If iduser <> "" And idcategory Then
                sqlstring = "delete from aula_studenti where iduser=" & iduser & " And idcategory=" & idcategory
                conn.Execute(sqlstring, CommandType.Text, Nothing)
            End If

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function
    Private Shared Function Num(ByVal value As String) As Integer
        Dim returnVal As String = String.Empty
        Dim collection As MatchCollection = Regex.Matches(value, "\d+")
        For Each m As Match In collection
            returnVal += m.ToString()
        Next
        Return Convert.ToInt32(returnVal)
    End Function
    Public Sub editstudenti()
        Try
            sqlstring = "update `aula_studenti` Set iduser='" & EscapeMySql(Request.Form("iduser")) & "' ,idcategory='" & EscapeMySql(Request.Form("idcategory")) & "' where id=" & Request.Form("id") & ""
            conn.Execute(sqlstring, CommandType.Text, Nothing)
            msg = "Aggiornamento completato"

        Catch ex As Exception
            msg = ex.Message

        End Try

    End Sub

    Public Sub delstudenti(id, idcourse)
        Try
            sqlstring = "delete from learning_courseuser where iduser=" & id & " and idcourse=" & idcourse
            conn.Execute(sqlstring, CommandType.Text, Nothing)
            msg = "eliminazione completata"
            Response.End()
        Catch ex As Exception
            msg = ex.Message

        End Try

    End Sub

#End Region



#End Region

#Region "aula_sessioni"
    Function getcalendar()


        Dim options As String = String.Empty
            Dim idcategory As Integer = 0
            Dim datastorica As String = String.Empty
            Dim support As String = String.Empty



        sqlstring = "select *,datastart as start,dataend as end,concat(UPPER(b.firstname),' ', b.lastname) as docente,a.nomesessione as title,c.description from (aula_sessioni a join core_user b on a.iduser=b.idst) join core_category_users c on a.idcategory=c.idcategory where  " & support & "  " & datastorica & "   visible=1 "


        Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        FillDataTable(dt, dtoriginal)

        Dim jsonresult As String = utility.GetJson(dt)

        Response.ContentType = "application/json"
        jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

        msg = jsonresult


    End Function

    Public Sub getcoursesession(Optional ByVal idcategory As Integer = 0)
        Try


            Dim sqlstring As String = "select a.idcourse as id,a.name,a.code,a.credits,c.* from learning_course a left join learning_category  c on a.idcategory=c.idCategory where idcreator=" & Session("iduser") & " and a.idcategory <> 0  and idcourse not in (select idcourse from aula_course) order by a.idcourse desc"


            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            Dim dt As DataTable = Nothing

            FillDataTable(dt, dtoriginal)
            Dim jsonresult As String = utility.GetJson(dt)
            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")
            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub
    Function getcoursestudentssingle()
        Try
            Dim totalpage As String
            Dim filter2 As String
            Dim filter3 As String
            Dim filter4 As String

            Dim sidx As String = "idcourse"
            Dim sord As String = " asc"
            Dim totalrecords As Integer

            sqlstring = "select a.name,b.iduser,a.idcourse,b.idcourse as id,date_inscr,date_complete,b.status as statocorso from (learning_course a join learning_courseuser b on a.idcourse=b.idcourse) left  join aula_course c on a.idcourse=c.idcourse where idsessione=" & Request.QueryString("idsessione") & " and  b.iduser=" & Session("iduser") & " "

            Dim dttrack = createdatatableuser()


            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            Dim idcourse As Integer = dtoriginal.Rows(0)("id")

            totalrecords = dtoriginal.Rows.Count



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
                r("idcourse") = dr("idcourse")
                r("nominativo") = dr("name").ToString
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

                    End Try


                    Dim time As String = dtimevideo.Select(" iduser=" & iduser & " ")(0)("total_time")
                    Dim second As String = String.Empty
                    Dim minutes As String = String.Empty
                    Dim hour As String = String.Empty
                    Dim time_sec As Integer = 0
                    Dim time_min As Integer = 0
                    Dim time_hour As Integer = 0

                    r("tempovideocorso") = utility.ConvertSecToDate(objuser.getUserTotalVideocourseSec(iduser, idcourse))
                    r("temposessione") = utility.ConvertSecToDate(dtime.Select("iduser=" & iduser & "")(0)("total_time"))

                Catch ex As Exception

                End Try

                dttrack.rows.add(r)

            Next

            'filldatatable(dt, dttrack)

            ' dim jsonresult as string = utility.getjson(dt)

            ' response.contenttype = "application/json"

            ' jsonresult = jsonresult.replace(" ", " ").replace(vbcrlf, " ").replace("\t", " ")

            ' msg = jsonresult

            msg = utility.getGridData(1, totalpage, totalrecords, sidx, sord, dttrack)

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Function

    Function getcoursestudents()
        Try
            Dim totalpage As String
            Dim filter2 As String
            Dim filter3 As String
            Dim filter4 As String
            Dim iduser As String
            Dim idcourse As Integer
            Dim sidx As String = "lastname"
            Dim sord As String = " asc"
            Dim totalrecords As Integer

            sqlstring = "select concat(a.firstname, ' ', a.lastname) as nominativo,b.iduser,b.idcourse,b.iduser as id,date_inscr,date_complete,b.status as statocorso from (core_user a join learning_courseuser b on a.idst=b.iduser) right join aula_prenotazioni c on b.iduser=c.iduser where c.idsessione=" & Request.QueryString("idsessione") & " and  b.idcourse in (Select idcourse from aula_course where idsessione=" & Request.QueryString("idsessione") & ") order by " & sidx & " " & sord & ""

            Dim dttrack = createdatatableuser()


            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            idcourse = dtoriginal.Rows(0)("idcourse")

            totalrecords = dtoriginal.Rows.Count

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

                r("idcourse") = idcourse
                r("nominativo") = dr("nominativo").ToString
                r("date_inscr") = dr("date_inscr").ToString
                r("date_complete") = dr("date_complete").ToString

                iduser = dr("iduser")

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

                    End Try



                    'Dim time As String = dtimevideo.Select(" iduser=" & iduser & " ")(0)("total_time")

                    Dim second As String = String.Empty

                    Dim minutes As String = String.Empty

                    Dim hour As String = String.Empty

                    Dim time_sec As Integer = 0
                    Dim time_min As Integer = 0
                    Dim time_hour As Integer = 0

                    r("tempovideocorso") = utility.ConvertSecToDate(objuser.getUserTotalVideocourseSec(iduser, idcourse))
                    r("temposessione") = utility.ConvertSecToDate(dtime.Select("iduser=" & iduser & "")(0)("total_time"))

                Catch ex As Exception

                End Try

                dttrack.rows.add(r)

            Next


            msg = utility.getGridData(1, totalpage, totalrecords, sidx, sord, dttrack)




        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Function

    Function gettesteassigned()
        Try
            Dim options = ""

            sqlstring = "select name,code,date_create,a.idcourse as id,idresource as idtest from (learning_course a join aula_course  b on a.idcourse =b.idcourse ) join learning_organization c on a.idcourse=c.idcourse  where idsessione=" & Request.QueryString("idsessione")

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Function
    Function getcourseassigned()
        Try
            Dim options = ""

            sqlstring = "select name,code,date_create,a.idcourse as id from learning_course a join aula_course  b on a.idcourse =b.idcourse where idsessione=" & Request.QueryString("idsessione")

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Function
    Function maketestcourse()

        Try


            Dim idsessione As Integer = Request.QueryString("idsessione")
            Dim idcategory As Integer = Request.QueryString("idcategory")
            Dim soglia As String = Context.Request.Form("soglia").ToString
            Dim title As String = Context.Request.Form("title").ToString
            Dim tentativi As String = Context.Request.Form("tentativi").ToString
            Dim random As String = Context.Request.Form("random").ToString
            Dim idtest As String = Context.Request.Form("idtest")
            Dim t As New TestObject
            If idtest <> "" Then
                t.updatetest(title, soglia, tentativi, random, idtest)
            Else
                sqlstring = "select count(*) as ncount from aula_course where iduser=" & Session("iduser") & " and idsessione=" & idsessione & ""


                If conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("ncount") = 0 Then




                    t.InsertTest(title, soglia, tentativi, random, idtest)

                    Dim description As String = ""
                    Dim credits As String = Request.Form("time")
                    Dim code As String = Session("iduser") & idtest
                    Dim name As String = title
                    Dim idcourse As Integer

                    insertcoursetest(description, code, name, credits, idcourse)


                    sqlstring = "insert into learning_organization (title,iduser,idcourse,path,idresource,objecttype,isterminator,lev) values('" & title & "'," & Session("iduser") & "," & idcourse & ",'/root/00000001'," & idtest & ",'test',1,1)"
                    conn.Execute(sqlstring, CommandType.Text, Nothing)


                    insertprenotazionecourse(idsessione, idcategory, idcourse)

                    msg = idtest
                Else
                    msg = 0
                End If
            End If
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Function

    Sub bookingstudent()

        Dim idsessione As Integer = Request.QueryString("idsessione")
        Dim iduser As String = Request.QueryString("iduser")

        sqlstring = "update aula_prenotazioni set token=1 where idsessione=" & idsessione & " and iduser=" & iduser

        conn.Execute(sqlstring, CommandType.Text, Nothing)
        msg = "Prenotazione avvenuta con successo"
    End Sub

    Sub booking()

        Dim idsessione As Integer = Request.QueryString("idsessione")
        Dim ids As String = Request.QueryString("ids")



        insertprenotazione(idsessione, ids)

        Dim listusers() As String = ids.Split(",")

        Dim j = 1
        For i = 0 To listusers.Length - 1

            If (IsNumeric(Num(listusers(i)))) Then


                Dim iduser As Integer = Num(listusers(i))

                Select Case Request.QueryString("tipo")
                    Case 1

                        sqlstring = "insert into learning_courseuser ( iduser , idcourse , date_inscr , status , date_begin_validity , date_expire_validity ) select " & iduser & " ,idcourse, '" & ConvertToMysqlDateTime(Now) & "',0, datastart, dataend from aula_sessioni a join aula_course b on a.id=b.idsessione where idsessione=" & idsessione

                    Case 2

                        sqlstring = "insert into aula_documentstudents (iduser,iddoc,idsessione)  select " & iduser & ",id," & idsessione & " from aula_document b where b.idsessione=" & idsessione


                End Select

                conn.Execute(sqlstring, CommandType.Text, Nothing)
            End If
        Next
    End Sub
    Public Sub sendinvite()

        Dim sqlstring As String = String.Empty


        Dim nome As String = EscapeMySql(Request.Form("nome"))
        Dim cognome As String = EscapeMySql(Request.Form("cognome"))
        Dim email As String = EscapeMySql(Request.Form("email"))
        Dim subject As String = " Invito alla classe da parte di " & Session("fullname")
        Dim body As String = String.Empty

        body = getmailformat(False, "mailformatinvite")
        body = body.Replace("[firstnamedocente]", Session("fullname"))
        body = body.Replace("[firstname]", Request.Form("nome"))
        Dim address As String = "http://" & Session("affiliate") & ".training-school.it/Wflogin.aspx?op=invite&codice=" & Request.Form("codice") & "&email=" & Request.Form("email") & "&cognome=" & cognome & "&nome=" & nome
        body = body.Replace("[address]", address)


        sqlstring = "insert into core_category_invite (nome,cognome,email,idcategory) values('" & nome & "','" & cognome & "','" & email & "'," & Request.Form("idcategory") & ")"

        conn.Execute(sqlstring, CommandType.Text, Nothing)


        msg &= utility.InvioMail(email, mailpiattaforma, "", subject, body, "") & vbCrLf



    End Sub
    Public Sub inviacomunicazionecorsista()

        Dim sqlstring As String = String.Empty


        sqlstring = "select firstname,lastname,email,tel,nomesessione,datastart,dataend,joinurl,a.id as idprenotazione,a.iduser,tipo,b.notenewpublic,materia from (aula_prenotazioni a join aula_sessioni b on a.idsessione=b.id) join core_user c on b.iduser=c.idst  where a.iduser=" & Request.Form("iduser") & " b.id=" & Request.Form("idsessione") & " "

        dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        Dim firstnamedocente As String = dt.Rows(0)("firstname").ToString
        Dim lastnamedocente As String = dt.Rows(0)("lastname").ToString
        Dim emaildocente As String = dt.Rows(0)("email").ToString
        Dim teldocente As String = dt.Rows(0)("tel").ToString
        Dim nomesessione = dt.Rows(0)("nomesessione").ToString
        Dim datastart As String = dt.Rows(0)("datastart").ToString
        Dim dataend As String = dt.Rows(0)("dataend").ToString
        Dim joinurl As String = dt.Rows(0)("joinurl").ToString
        Dim idprenotazione As String = dt.Rows(0)("idprenotazione").ToString
        Dim notepubbliche As String = dt.Rows(0)("notenewpublic").ToString
        Dim tipo As String = dt.Rows(0)("tipo")
        Dim materia As String = dt.Rows(0)("materia")


        For Each dr In dt.Rows


            sqlstring = "Select a.email,a.firstname ,a.lastname from core_user a where idst=" & dr("iduser")

            Dim dr1 As MySqlDataReader
            dr1 = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr1.Read()


            Dim firstname As String = dr("firstname").ToString
            Dim lastname As String = dr("lastname").ToString
            Dim email As String = dr("email").ToString


            dr1.Close()

            Dim subject As String = " Conferma evento " & tipo & " - " & materia & " - " & nomesessione & " - Docente " & lastnamedocente
            Dim body As String = String.Empty

            body = getmailformat(False, "mailformat")

            body = body.Replace("[firstname]", firstname)
            body = body.Replace("[lastname]", lastname)
            body = body.Replace("[firstnamedocente]", firstnamedocente)
            body = body.Replace("[lastnamedocente]", lastnamedocente)
            body = body.Replace("[emaildocente]", emaildocente)
            body = body.Replace("[teldocente]", teldocente)
            body = body.Replace("[note]", notepubbliche)

            If tipo <> "LEZIONE" And tipo <> "CORSO" Then
                body = body.Replace("[AULA]", "per poter partecipare all'evento on line collegarsi alla piattaforma e accedere all'evento sul calendario cliccando su Accedi alla Webinar oppure collegarsi al seguente indirizzo:<a href='https://meet.training-school.it/" & joinurl & "' >https://meet.training-school.it/" & joinurl & "</a>")
            Else
                body = body.Replace("[AULA]", "per poter partecipare all'evento collegarsi alla piattaforma e accedere all'evento sul calendario e scaricare il materiale didattico")


            End If



            body = body.Replace("[datastart]", FormatDateTime(datastart, DateFormat.ShortDate))
            body = body.Replace("[ora]", FormatDateTime(datastart, DateFormat.ShortTime))
            body = body.Replace("[end]", FormatDateTime(dataend, DateFormat.ShortTime))

            sqlstring = "update aula_prenotazioni set flagmail=1 where id=" & idprenotazione

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            Dim y As New SharedRoutines


            msg &= y.InvioMail(email, mailpiattaforma, "", subject, body, googlecalendar(datastart, dataend, idprenotazione, subject, nomesessione, FormattaNominativo(firstnamedocente & " " & lastnamedocente))) & vbCrLf

        Next

    End Sub
    Public Sub inviacomunicazionecorsistatutti()

        Dim sqlstring As String = String.Empty
        Dim idsessione As Integer = Request.Form("idsessione")

        sqlstring = "select firstname,lastname,email,tel,nomesessione,datastart,dataend,joinurl,a.iduser,tipo,b.notenewpublic,materia from (aula_prenotazioni a join aula_sessioni b on a.idsessione=b.id) join core_user c on b.iduser=c.idst  where b.id=" & idsessione & " "

        dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        Dim firstnamedocente As String = dt.Rows(0)("firstname").ToString
        Dim lastnamedocente As String = dt.Rows(0)("lastname").ToString
        Dim emaildocente As String = dt.Rows(0)("email").ToString
        Dim teldocente As String = dt.Rows(0)("tel").ToString
        Dim nomesessione = dt.Rows(0)("nomesessione").ToString
        Dim datastart As String = dt.Rows(0)("datastart").ToString
        Dim dataend As String = dt.Rows(0)("dataend").ToString
        Dim joinurl As String = dt.Rows(0)("joinurl").ToString

        Dim notepubbliche As String = dt.Rows(0)("notenewpublic").ToString
        Dim tipo As String = dt.Rows(0)("tipo")
        Dim materia As String = dt.Rows(0)("materia")


        For Each dr In dt.Rows


            sqlstring = "Select a.email,a.firstname ,a.lastname,b.id as idprenotazione from core_user a join aula_prenotazioni b on a.idst=b.iduser where b.idsessione=" & idsessione & " and  idst=" & dr("iduser")




            Dim dr1 As MySqlDataReader
            dr1 = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr1.Read()

            Dim idprenotazione As String = dr1("idprenotazione").ToString
            Dim firstname As String = dr1("firstname").ToString
            Dim lastname As String = dr1("lastname").ToString
            Dim email As String = dr1("email").ToString



            dr1.Close()
            Dim subject As String = " Conferma evento " & tipo & " - " & materia & " - " & nomesessione & " - Docente " & lastnamedocente
            Dim body As String = String.Empty

            body = getmailformat(False, "mailformat")

            body = body.Replace("[firstname]", firstname)
            body = body.Replace("[lastname]", lastname)
            body = body.Replace("[firstnamedocente]", firstnamedocente)
            body = body.Replace("[lastnamedocente]", lastnamedocente)
            body = body.Replace("[emaildocente]", emaildocente)
            body = body.Replace("[teldocente]", teldocente)
            body = body.Replace("[note]", notepubbliche)

            If tipo <> "LEZIONE" And tipo <> "CORSO" Then
                body = body.Replace("[AULA]", "per poter partecipare all'evento on line collegarsi alla piattaforma e accedere all'evento sul calendario cliccando su Accedi alla Webinar oppure collegarsi al seguente indirizzo: https://meet.training.school.it/" & joinurl & "")
            Else
                body = body.Replace("[AULA]", "per poter partecipare all'evento collegarsi alla piattaforma e accedere all'evento sul calendario e scaricare il materiale didattico")


            End If



            body = body.Replace("[datastart]", FormatDateTime(datastart, DateFormat.ShortDate))
            body = body.Replace("[ora]", FormatDateTime(datastart, DateFormat.ShortTime))
            body = body.Replace("[end]", FormatDateTime(dataend, DateFormat.ShortTime))

            sqlstring = "update aula_prenotazioni set flagmail=1 where id=" & idprenotazione

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            Dim y As New SharedRoutines

            Dim filecalendar = googlecalendar(datastart, dataend, idprenotazione, subject, nomesessione, FormattaNominativo(firstnamedocente & " " & lastnamedocente))
            msg &= y.InvioMail(email, mailpiattaforma, "", subject, body, filecalendar) & vbCrLf
            Try
                If File.Exists(filecalendar) Then
                    File.Delete(filecalendar)
                End If
            Catch ex As Exception
            End Try
        Next

    End Sub

    Public Sub getemailflag(idsessione As Integer)
        sqlstring = "select count(*) as emaildainviare from token a join aula_sessioni b on a.idsessione=b.idst where flagmail=0 and b.idst=" & idsessione

        dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        If dt.Rows.Count > 0 Then
            msg = dt.Rows(0)("emaildainviare")

        Else
            msg = 0

        End If

    End Sub

    Public Function gettemplate(b As Boolean)

        sqlstring = "select meta_value from core_formathtml where meta_key='mailformat' "

        Dim emailtemplate As String = conn.GetDataTable(sqlstring).Rows(0)("meta_value")

        If b Then
            msg = HttpUtility.UrlDecode(emailtemplate)

        Else
            Return HttpUtility.UrlDecode(emailtemplate)
        End If

        Return False

    End Function




    Public Sub deletesessione()

        Try
            sqlstring = "select ifwrite from aula_sessioni where id=" & Request.Form("id")
            Dim ifwrite As Integer = Nothing
            Try
                ifwrite = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("ifwrite")
            Catch ex As Exception
                ifwrite = 0
            End Try

            sqlstring = "delete from aula_sessioni where id=" & Request.Form("id")
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "delete from aula_prenotazioni where idsessione=" & Request.Form("id")
            conn.Execute(sqlstring, CommandType.Text, Nothing)


            sqlstring = "delete a from learning_courseuser a join aula_course b on a.idcourse=b.idcourse where idsessione=" & Request.Form("id")
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            If ifwrite = 1 Then
                sqlstring = "delete a from learning_course a join aula_course b on a.idcourse=b.idcourse where idsessione=" & Request.Form("id")
                conn.Execute(sqlstring, CommandType.Text, Nothing)
                sqlstring = "delete a from learning_organization a join aula_course b on a.idcourse=b.idcourse where idsessione=" & Request.Form("id")
                conn.Execute(sqlstring, CommandType.Text, Nothing)
            End If
            sqlstring = "delete from aula_course where idsessione=" & Request.Form("id")
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "delete from aula_document where idsessione=" & Request.Form("id")
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "delete from aula_documentstudents where idsessione=" & Request.Form("id")
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            msg = "Evento cancellato"
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub



    Public Sub editsessione(ByVal idsessione As Integer)
        Try


            sqlstring = "update aula_sessioni set notenew='" & EscapeMySql(Request.Form("notenew")) & "',notenewpublic='" & EscapeMySql(Request.Form("notenewpublic")) & "'  where id=" & Request.Form("idsessione")
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            msg = "Aggiornamento completato"


        Catch ex As Exception
            LogWrite(ex.ToString & sqlstring)
            msg = "Errore Inserimento evento"
        End Try

    End Sub
    Public Sub insertsessione(ByVal idsessione As Integer)
        Try
            Dim meetingid As String
            Dim joinurl As String

            Dim maxposti As Integer = 0 ' CInt(Request.Form("maxposti").Trim())
            Dim minposti As Integer = 0 'CInt(Request.Form("minposti").Trim())

            Dim datastart = Request.Form("datastart").Split("-")(0)
            Dim dataend = Request.Form("datastart").Split("-")(1)
            datastart = Replace(Convert.ToDateTime(datastart).ToString("yyyy-MM-dd HH:mm"), ".", ":")
            dataend = Replace(Convert.ToDateTime(dataend).ToString("yyyy-MM-dd HH:mm"), ".", ":")



            joinurl = Request.Form("nomecorso") & Guid.NewGuid.ToString

            Dim write As Integer
            Try
                write = Request.Form("write").ToString
            Catch ex As Exception
                write = 0
            End Try

            sqlstring = "insert into aula_sessioni (datastart,dataend,nomesessione,meetingid,joinurl,tipo,iduser,idcategory,materia,notenew,notenewpublic,ifwrite) values('" & datastart & "','" & dataend & "','" & EscapeMySql(Request.Form("nomecorso")) & "','" & meetingid & "','" & joinurl & "','" & Request.Form("tipo") & "'," & Session("iduser") & "," & Request.Form("idcategory") & ",'" & EscapeMySql(Request.Form("materia")) & "','" & EscapeMySql(Request.Form("notenew")) & "','" & EscapeMySql(Request.Form("notenewpublic")) & "'," & write & ")"

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "select max(id) as idsessione from aula_sessioni"
            idsessione = conn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("idsessione")
            insertprenotazione(idsessione, CInt(Request.Form("idcategory")))
            msg = idsessione & ";" & joinurl



        Catch ex As Exception
            LogWrite(ex.ToString & sqlstring)
            msg = 0
        End Try

    End Sub



    Sub updateattivo()
        Try
            sqlstring = "update aula_sessioni set attivo=" & Request.Form("attivo") & " where id=" & Request.Form("id")
            conn.Execute(sqlstring, CommandType.Text, Nothing)
            sqlstring = "update aula_prenotazioni set attivo=" & Request.Form("attivo") & " where idsessione=" & Request.Form("id")
            conn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub
    Sub updatevisibile()
        Try
            sqlstring = "update aula_sessioni set visible=" & Request.Form("visibile") & " where id=" & Request.Form("id")
            conn.Execute(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub


    Function inviamailpecsingola(idsessione As Integer, iduser As String)
        Try
            sqlstring = "select a.firstname,a.lastname,a.email,e.sedi,e.indirizzo,e.provincia,e.recapito,c.datastart,c.nomesessione from ((core_user a join aula_prenotazioni b on a.idst=b.iduser) join aula_sessioni c on c.id=b.idsessione) join aula_sedi e on e.id=c.idsede where a.idst=" & iduser & " and c.id=" & idsessione & " order by lastname desc "
            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            Dim body As String = getmailformat(False)
            Dim subject As String = getsubject()

            If dt.Rows.Count = 1 Then
                Dim dr As DataRow = dt.Rows(0)
                Dim firstname As String = dr("firstname")
                Dim lastname As String = dr("lastname")
                Dim citta As String = dr("provincia")
                Dim nomesession As String
                Dim indirizzo As String = dr("indirizzo")
                Dim ragionesociale As String = dr("lastname") & " " & dr("firstname")
                Dim datastart As String = dr("datastart")
                Dim nomesessione As String = dr("nomesessione")
                Dim cf As String = ""
                Dim recapito As String = dr("recapito")
                Dim email As String = dr("email")
                body = body.Replace("[email]", email)
                body = body.Replace("[firstname]", firstname)
                body = body.Replace("[lastname]", lastname)
                body = body.Replace("[cf]", cf)
                body = body.Replace("[ticket]", recapito)
                body = body.Replace("[indirizzo]", indirizzo)
                body = body.Replace("[ente]", citta)
                body = body.Replace("[firstnamesessione]", ragionesociale)
                body = body.Replace("[datastart]", Convert.ToDateTime(datastart).ToString("dd/m/yyyy hh:mm"))

                'utility.inviomailpec(pec, subject, body)
                msg &= utility.InvioMail(email, mailpiattaforma, "", "Training school| conferma prenotazione corso:" & nomesessione, body, "")

            End If

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

        Return False

    End Function
    Private Sub deletedoc(id As String)



        Try
            sqlstring = "delete from aula_document  where id=" & id & " "
            conn.Execute(sqlstring, CommandType.Text, Nothing)


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        msg = "Operazione completata"
    End Sub
    Private Sub deletedocstudents(id As String)



        Try
            sqlstring = "delete from aula_documentstudents  where id=" & id & " "
            conn.Execute(sqlstring, CommandType.Text, Nothing)


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        msg = "Operazione completata"
    End Sub
    Sub getdocumentistudenti(ByVal iddoc As Integer)

        Try
            Dim options = ""

            sqlstring = "select id,nomedocupload,idsessione,iduser,firstname,lastname,date_hit from aula_documentstudents a join core_user b on a.iduser=b.idst where iddoc=" & iddoc & " order by id asc"

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub

    Sub getdocumenti(ByVal idsessione As Integer)

        Try
            Dim options = ""

            sqlstring = "select a.nomedoc,a.id,a.uploaddocstudent,a.idsessione,a.internetaddress,a.descrizione from aula_document a where a.idsessione=" & idsessione & " order by id asc"

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub

    Sub getprenotati(ByVal idsessione As Integer, Optional filter As String = "")

        Try
            Dim options = ""

            Select Case filter
                Case "convalidati"
                    options = "and b.convalida=1 "
                Case "non convalidati"
                    options = "and b.convalida=0 "
                Case "in attesa di convalida"
                    options = "and b.convalida is null "
                Case "rimborsi"
                    options = "and b.rimborso=1 "
                Case "ticket"
                    options = "and b.attivo=1 "
            End Select

            sqlstring = "select * from aula_prenotazioni a join core_user b on a.iduser=b.idst where idsessione=" & idsessione & " " & options & " "

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub


    Sub updatemailformat()
        Try
            Dim mailformat As String = Request.Form("mailformat")
            sqlstring = "update core_formathtml set meta_value='" & EscapeSql(mailformat) & "' where meta_key='" & Request.QueryString("format") & "' "
            conn.Execute(sqlstring, CommandType.Text, Nothing)
            msg = "Aggiornamento completato!"

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Sub
    Public Function getmailformat(Optional ByVal f As Boolean = True, Optional ByVal format As String = "mailformat")

        Try
            sqlstring = "select * from core_formathtml where meta_key='" & format & "' order by meta_key asc"
            Dim dr As MySqlDataReader = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then
                Dim formatmail As String = dr("meta_value")
                If f Then
                    dr.Close()
                    msg = formatmail

                Else
                    dr.Close()
                    Return formatmail
                End If

            End If

            dr.Close()

        Catch ex As Exception

            LogWrite(ex.ToString)
        End Try

        Return False

    End Function
    Public Function getsubject()

        Try
            sqlstring = "select * from core_formathtml where meta_key='subject' order by meta_key asc"
            Dim dr As MySqlDataReader = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then
                Dim subj As String = dr("meta_value")
                dr.Close()
                Return subj
            End If

            dr.Close()

        Catch ex As Exception
            LogWrite(ex.ToString)

        End Try

        Return False

    End Function
    Public Sub getsessioni()

        Try

            sqlstring = "select * from aula_sessioni where iduser=" & Session("iduser") & " order by id desc"

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub
    'Function getsessionnumber()
    '    Dim sessionmax As String = String.Empty

    '    sqlstring = "select max(firstnamesessione) as maxsession from aula_sessioni where firstnamesessione like '%" & Year(Now) & "%'"
    '    Dim dr As MySqlDataReader = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
    '    dr.Read()
    '    If dr.HasRows Then
    '        sessionmax = dr("maxsession").ToString.Split("/")(0) + 1 & "/" & Year(Now)
    '    Else
    '        dr.Close()

    '    End If

    '    dr.Close()
    '    Return sessionmax
    '    Return False

    'End Function
    'Sub addsession(pathfile As String)

    '    Try
    '            Dim objstream As New System.IO.StreamReader(pathfile, System.Text.Encoding.GetEncoding(1252))

    '            While Not objstream.EndOfStream

    '                Dim field As String() = objstream.ReadLine.Split(";")

    '                sqlstring = "insert into [aula_sessioni]" &
    '" ([firstnamesessione] " &
    '" ,[data_attivazione]" &
    '" ,[data_termine]" &
    '" ,[maxposti]" &
    '" ,[postidisponibili]" &
    '" ,[visible]" &
    '" ,[attivo]" &
    '" ,[datastart]" &
    '" ,[sede]" &
    '" ,[indirizzo])" &
    '" values() " &
    '"'" & getsessionnumber() & "'" &
    '"'" & field(0) & "'" &
    '"'" & field(1) & "'" &
    '"'" & field(2) & "'" &
    '"'" & field(3) & "'" &
    '"'" & field(3) & "'" &
    '"0" &
    '"0" &
    '"'" & field(4) & "'" &
    '"'" & EscapeSql(field(5)) & "'" &
    '"'" & EscapeSql(field(6)) & "')"

    '                conn.Execute(sqlstring, CommandType.Text, Nothing)

    '            End While
    '        Catch ex As Exception
    '        End Try

    '    End Sub

#End Region

#Region "aula_prenotazioni"



    Private Sub deletebooking(idprenotazione As String, tipo As String, iduser As String, idsessione As Integer, write As Integer)


        Try
            sqlstring = "delete from aula_prenotazioni  where id=" & idprenotazione
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            Select Case tipo
                Case "VERIFICA SCRITTA"
                    If write = 1 Then
                        sqlstring = "delete a from learning_courseuser a join aula_course b On a.idcourse=b.idcourse where idsessione" & idsessione & " And a.iduser=" & iduser & ""
                    Else
                        sqlstring = "delete From aula_documentstudents where iduser=" & iduser & " and  idsessione=" & idsessione
                    End If
                Case "LEZIONE", "WEBINAR"
                    sqlstring = "delete From aula_documentstudents where iduser=" & iduser & " and  idsessione=" & idsessione
                Case "CORSO"
                    sqlstring = "delete a from learning_courseuser a join aula_course b On a.idcourse=b.idcourse where idsessione" & idsessione & " And a.iduser=" & iduser & ""
            End Select
            conn.Execute(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        msg = "Operazione completata"
    End Sub


    Function downloaddomande(ByVal idtoken As String)
        Dim dr As MySqlDataReader = Nothing
        Try
            sqlstring = "Select b.firstname,b.lastname,blobdomande from aula_prenotazioni a join core_user b On a.iduser=b.idst where a.id=" & idtoken

            dr = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then

                Dim bytes() As Byte = CType(dr("blobdomande"), Byte())
                Dim ctypecont As String = "application/pdf"
                Dim firstname As String = dr("firstname")
                Dim lastname As String = dr("lastname")
                dr.Close()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)

                Response.ContentType = "application/pdf"

                Response.AddHeader("content-disposition", "inline;filename=domande_" & firstname & "_" & lastname & ".pdf")
                Response.BinaryWrite(bytes)
                Response.Flush()

            End If

            dr.Close()

        Catch ex As Exception
            dr.Close()

            LogWrite(ex.ToString)

            msg = "nessun domanda"
            Response.Flush()

        End Try
        Return True
        Return False

    End Function

    'function downloadverbale(byval idtoken as string)
    ' dim dr as mysqldatareader
    ' try
    ' sqlstring = "Select b.firstname,b.lastname,blobverbale from aula_prenotazioni a join core_user b On a.iduser=b.idst where a.id=" & idtoken

    ' dr = conn.getdatareader(sqlstring, commandtype.text, nothing)
    ' dr.read()
    ' if dr.hasrows then

    ' dim bytes() as byte = ctype(dr("blobverbale"), byte())
    ' dim firstname as string = dr("firstname")
    ' dim lastname as string = dr("lastname")
    ' dr.close()
    ' response.buffer = true
    ' response.charset = ""
    ' response.cache.setcacheability(httpcacheability.nocache)

    ' response.contenttype = "application/pdf"

    ' response.addheader("content-disposition", "inline;filename=verbale_" & firstname & "_" & lastname & ".pdf")
    ' response.binarywrite(bytes)
    ' response.flush()

    ' end if

    ' dr.close()

    ' catch ex as exception
    ' dr.close()

    ' logWrite( ex.tostring)

    ' msg = "nessuna risposta"
    ' response.flush()

    ' end try

    'return false

    'end function

    Function downloadrisposte(ByVal idtoken As String)
        Dim dr As MySqlDataReader
        Try
            sqlstring = "Select b.firstname,b.lastname,blobrisposte from aula_prenotazioni a join core_user b On a.iduser=b.idst where a.id=" & idtoken

            dr = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then

                Dim bytes() As Byte = CType(dr("blobrisposte"), Byte())
                Dim firstname As String = dr("firstname")
                Dim lastname As String = dr("lastname")
                dr.Close()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)

                Response.ContentType = "application/pdf"

                Response.AddHeader("content-disposition", "inline;filename=risposte_" & firstname & "_" & lastname & ".pdf")
                Response.BinaryWrite(bytes)
                Response.Flush()

            End If

            dr.Close()

        Catch ex As Exception
            dr.Close()

            LogWrite(ex.ToString)


            msg = "nessuna risposta"
            Response.Flush()

        End Try

        Return False

    End Function


    Function downloaduploaddoc(ByVal id As String)
        Dim dr As MySqlDataReader
        Dim firstnamedoc As String = String.Empty

        Try
            sqlstring = "Select firstnamedoc,uploaddoc from aula_document where id=" & id

            dr = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then

                Dim bytes() As Byte = CType(dr("uploaddoc"), Byte())
                firstnamedoc = dr("firstnamedoc").ToString
                dr.Close()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)

                Response.ContentType = "application/pdf"

                Response.AddHeader("content-disposition", "inline;filename=" & firstnamedoc & ".pdf")
                Response.BinaryWrite(bytes)
                Response.Flush()

            End If

            dr.Close()

        Catch ex As Exception
            dr.Close()

            LogWrite(ex.ToString)


            msg = "nessuna risposta"
            Response.Flush()

        End Try
        Return False

    End Function

    Function downloaddoc(ByVal id As String)
        Dim firstnamedoc As String = String.Empty

        Dim dr As MySqlDataReader
        Try
            sqlstring = "Select nomedoc,doc from aula_document where id=" & id

            dr = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then

                Dim bytes() As Byte = CType(dr("doc"), Byte())
                firstnamedoc = dr("nomedoc").ToString
                dr.Close()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)

                Response.ContentType = "application/pdf"

                Response.AddHeader("content-disposition", "inline;filename=" & firstnamedoc & ".pdf")
                Response.BinaryWrite(bytes)
                Response.Flush()

            End If

            dr.Close()

        Catch ex As Exception
            dr.Close()

            LogWrite(ex.ToString)

            msg = "nessuna risposta"
            Response.Flush()

        End Try

        Return False

    End Function

    Sub updatepresenteall()
        Try



            sqlstring = "update aula_prenotazioni Set presente=1 where idsessione=" & Request.QueryString("idsessione")
            conn.Execute(sqlstring, CommandType.Text, Nothing)


        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Sub

    Sub updatepresente(presente As Integer, idprenotazione As Integer)
        Try
            If (presente = 1) Then
                sqlstring = "update aula_prenotazioni Set presente=1 where iduser=" & iduser & " And id=" & idprenotazione
                conn.Execute(sqlstring, CommandType.Text, Nothing)

            Else
                sqlstring = "update aula_prenotazioni Set presente=0 where iduser=" & iduser & " And id=" & idprenotazione
                conn.Execute(sqlstring, CommandType.Text, Nothing)
            End If
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Sub
    Sub insertaulacourse(idsessione As Integer, idcorsista As String)

        Dim datastart = Request.Form("datastart").Split("-")(0)
        Dim dataend = Request.Form("datastart").Split("-")(1)
        Dim idcourse = Request.Form("idcourse")
        Dim esito As String
        Dim listusers() As String = idcorsista.Split(",")

        Try
            Dim j = 1
            For i = 0 To listusers.Length - 1

                If (IsNumeric(Num(listusers(i)))) Then





                    sqlstring = "insert into aula_course (iduser,idsessione,data_prenotazione)" &
            " values ( " & Num(listusers(i)) & "," & idsessione & ",'" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm") & "') "

                    result = conn.Execute(sqlstring, CommandType.Text, Nothing)

                    If result = 1 Then


                        datastart = Replace(Convert.ToDateTime(datastart).ToString("yyyy-MM-dd HH:mm"), ".", ":")
                        dataend = Replace(Convert.ToDateTime(dataend).ToString("yyyy-MM-dd HH:mm"), ".", ":")

                        sqlstring = "insert into learning_courseuser ( iduser , idcourse , date_inscr , status , date_begin_validity , date_expire_validity ) " &
        "values (" & Num(listusers(i)) & " ," & idcourse & ", '" & ConvertToMysqlDateTime(Now) & "',0, '" & datastart & "', '" & dataend & "');"

                        conn.Execute(sqlstring, CommandType.Text, Nothing)
                        'utility.SendBenvenuto(esito, Num(listusers(i)), idcourse)

                        msg = j & " utente/i iscritti al corso"
                        j = j + 1



                        msg = "<h3>Aggiunta avvenuta con successo!</h3>"




                    End If
                End If
            Next

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub

    Sub insertprenotazionecourse(idsessione As Integer, idcateogry As Integer, idcourse As String)
        Try
            sqlstring = "insert into aula_course (idsessione,idcourse,iduser) values(" & idsessione & "," & idcourse & "," & Session("iduser") & ")"
            result = conn.Execute(sqlstring, CommandType.Text, Nothing)

            If result Then
                Dim listusers() As String = idcourse.Split(",")

                Dim j = 1
                For i = 0 To listusers.Length - 1

                    If (IsNumeric(Num(listusers(i)))) Then

                        sqlstring = "select iduser from aula_studenti where idcategory=" & idcateogry
                        dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

                        For Each dr In dt.Rows


                            sqlstring = "insert into learning_courseuser (iduser,idcourse,date_inscr)" &
" values ( " & dr("iduser") & "," & idcourse & ",'" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm") & "') "

                            result = conn.Execute(sqlstring, CommandType.Text, Nothing)


                            If result Then
                                msg = j + 1 & " Studente/i iscritti al corso"
                            Else
                                LogWrite("errore prenotazione " & dr("iduser"))
                                msg = j + 1 & " Studente/i già iscritto al corso " & iduser
                            End If
                            j = j + 1

                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            msg = "Corso assegnato già ad un altra sessione"
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub
    Sub insertprenotazione(idsessione As Integer, idcateogry As Integer)
        Try

            sqlstring = "select iduser from aula_studenti where idcategory=" & idcateogry
            dt = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            For Each dr In dt.Rows

                sqlstring = "insert into aula_prenotazioni (iduser,idsessione,data_prenotazione)" &
" values ( " & dr("iduser") & "," & idsessione & ",'" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm") & "') "

                result = conn.Execute(sqlstring, CommandType.Text, Nothing)

                If result Then
                Else
                    LogWrite("errore prenotazione" & dr("iduser"))
                End If

            Next

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub
    Sub insertprenotazione(idsessione As Integer, idcorsista As String)
        Try
            Dim iduser(0) As String

            Dim result As String
            Try
                iduser = idcorsista.ToString.Split(",")
            Catch ex As Exception
                iduser(0) = idcorsista
            End Try


            For i = 0 To iduser.Length - 1

                sqlstring = "insert into aula_prenotazioni (iduser,idsessione,data_prenotazione)" &
" values ( " & iduser(i) & "," & idsessione & ",'" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm") & "') "

                result = conn.Execute(sqlstring, CommandType.Text, Nothing)

                If result = 1 Then

                    msg = "<h3>Prenotazione avvenuta con successo!</h3>"

                End If

            Next

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub

    Sub sbloccainvio(idsessione As String)

        Try

            sqlstring = "update aula_sessioni set flaginvia=1 where id=" & idsessione

            conn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub

    Sub getprenotazioni(iduser As Integer)
        Try

            sqlstring = "select a.attivo,a.id,b.firstname,b.lastname,a.citta,a.token,a.data_prenotazione,a.ente,a.indirizzo,c.token, from ( aula_prenotazioni a join core_user b on a.iduser=b.idst) join token c on a.idtoken=c.id where a.iduser=" & iduser & " order by a.id desc"

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Sub






#End Region


#Region "documenti"

    Public Sub checkdocument(iduser)

        Dim dr As MySqlDataReader
        Dim docdatascadenza As String = String.Empty

        Try
            sqlstring = "select documentoidentita from core_user where datascadenzadoc > getdate() and id=" & iduser

            dr = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then
                msg = "okscadenza"
                If dr("documentoidentita") Is Nothing Then
                    msg = "kodoc"
                End If

            Else
                msg = "koscadenza"
            End If

            dr.Close()

        Catch ex As Exception
            If Not dr Is Nothing Then

                dr.Close()
            End If

            msg = "errore check document"

        End Try

    End Sub
    Function getdocumento(ByVal iduser As String)
        Dim dr As MySqlDataReader
        Dim docdatascadenza As String = String.Empty

        Try
            sqlstring = "select datascadenzadoc from core_user where documentoidentita is not null and idst=" & iduser

            dr = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then

                Try
                    docdatascadenza = FormatDateTime(dr("datascadenzadoc").ToString, DateFormat.ShortDate).ToString
                Catch ex As Exception
                End Try
            End If

            dr.Close()

            msg = docdatascadenza
        Catch ex As Exception
            dr.Close()
            msg = "nessun documento"

        End Try

        Return False

    End Function
    Function downloaddocumento(ByVal iduser As String)
        Dim dr As MySqlDataReader
        Try
            sqlstring = "select documentoidentita,contenttypedoc from core_user where idst=" & iduser

            dr = conn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then

                Dim bytes() As Byte = CType(dr("documentoidentita"), Byte())
                Dim ctypecont As String = Replace(dr("contenttypedoc"), ".", "")
                dr.Close()
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                If (ctypecont = "pdf") Then
                    Response.ContentType = "application/pdf"
                Else
                    Response.ContentType = "image/" & ctypecont
                End If
                Response.AddHeader("content-disposition", "inline;filename=documento." & ctypecont & "")
                Response.BinaryWrite(bytes)
                Response.Flush()

            End If

            dr.Close()

        Catch ex As Exception
            dr.Close()

            msg = "nessun documento"
        End Try

        Return False

    End Function
    Function adddocstudent(idsessione As Integer, filedoc As String)

        Try



            If conn Is Nothing Then
                conn = CheckDatabase(conn)
            End If

            sqlstring = "update aula_documentstudents  set nomedocupload='" & System.IO.Path.GetFileName(filedoc) & "' where iduser=" & Context.Request.Form("iduser") & " and  iddoc=" & Context.Request.Form("iddoc") & ""
            conn.Execute(sqlstring, CommandType.Text, Nothing)

            conn.Execute(sqlstring, CommandType.Text, Nothing)
            Return True
        Catch ex As Exception


        End Try

        Return False



    End Function

    Function addhit()

        Try



            If conn Is Nothing Then
                conn = CheckDatabase(conn)
            End If

            sqlstring = "update aula_documentstudents  set date_hit='" & ConvertToMysqlDateTime(Now) & "' where iduser=" & Request.Form("iduser") & " and  iddoc=" & Request.Form("iddoc") & ""
            conn.Execute(sqlstring, CommandType.Text, Nothing)


            Return True
        Catch ex As Exception


        End Try

        Return False



    End Function

    Function adddocsessione(idsessione As Integer, filedoc As String)

        Try

            sqlstring = "insert into aula_document (nomedoc,idsessione,uploaddocstudent,iduser,internetaddress,descrizione) values('" & filedoc & "','" & idsessione & "'," & Context.Request.Form("uploadstudent") & ",'" & Context.Request.Form("iduser") & "','" & EscapeMySql(Context.Request.Form("internetaddress")) & "','" & EscapeMySql(Context.Request.Form("descrizione")) & "')"

            If conn Is Nothing Then
                conn = CheckDatabase(conn)
            End If

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            Dim iddoc = conn.GetDataTable("select max(id) as iddoc from aula_document").Rows(0)("iddoc")

            Return iddoc
        Catch ex As Exception
            Return False
        End Try

        Return 0



    End Function

    Function adddocdocuments(idsessione As Integer, iddoc As Integer, idcategory As Integer)

        Try

            If conn Is Nothing Then
                conn = CheckDatabase(conn)
            End If


            sqlstring = "insert into aula_documentstudents (iduser,iddoc,idsessione)  select  a.iduser," & iddoc & "," & idsessione & " from aula_prenotazioni  a join aula_studenti b on a.iduser=b.iduser where idcategory=" & idcategory & " and idsessione=" & idsessione


            conn.Execute(sqlstring, CommandType.Text, Nothing)

            Return True
        Catch ex As Exception
            Return False
        End Try

        Return False



    End Function








#End Region


#Region "function"
    Function googlecalendar(datastart, dataend, iduser, subject, corso, docente)
        Try
            Dim filePath As String = IO.Path.Combine(Server.MapPath("temp"), "calendar_event_" & iduser & ".ics")
            Dim location As String = "ON LINE"
            Dim subject1 As String = subject
            Dim subject2 As String = corso

            Dim writer As New StreamWriter(filePath)

            writer.WriteLine(“BEGIN:VCALENDAR”)

            writer.WriteLine(“VERSION:2.0”)

            writer.WriteLine(“PRODID:-//hacksw/handcal//NONSGML v1.0//EN”)

            writer.WriteLine(“BEGIN:VEVENT”)

            writer.WriteLine(“DTSTART:” + DateTime.Parse(datastart).ToUniversalTime().ToString(“yyyyMMddT090000”))

            writer.WriteLine(“DTEND:” + DateTime.Parse(dataend).ToUniversalTime().ToString(“yyyyMMddT090000”))

            writer.WriteLine(“ORGANIZER::” + docente)


            writer.WriteLine(“SUMMARY:” + subject1)

            writer.WriteLine(“LOCATION:” + location)

            writer.WriteLine(“END:VEVENT”)
            writer.WriteLine(“BEGIN:VEVENT”)

            writer.WriteLine(“DTSTART:” + DateTime.Parse(datastart).ToUniversalTime().ToString(“yyyyMMddT090000”))

            writer.WriteLine(“DTEND:” + DateTime.Parse(dataend).ToUniversalTime().ToString(“yyyyMMddT090000”))

            writer.WriteLine(“SUMMARY:” + subject2)

            writer.WriteLine(“LOCATION:” + location)

            writer.WriteLine(“END:VEVENT”)

            writer.WriteLine(“END:VCALENDAR”)

            writer.Close()
            Return filePath
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try


    End Function
    Private Shared Function getintonly(ByVal value As String) As Integer
        Dim returnval As String = String.Empty
        Dim collection As MatchCollection = Regex.Matches(value, "\d+")
        For Each m As Match In collection
            returnval += m.ToString()
        Next
        Return Convert.ToInt32(returnval)
    End Function
#End Region


#Region "platform"
    Sub insertplatform()



        Dim nome As String = Request.Form("nome")
        Dim cognome As String = Request.Form("cognome")

        Dim indirizzoweb As String = Request.Form("indirizzoweb")
        Dim email As String = Request.Form("email")
        Dim datacreazione As String = ConvertToMysqlDateTime(Now)
        Dim password As String = LCase(CreateRandomPassword(6))

        Try

            sqlstring = " insert into core_platform " &
" (nome,cognome, indirizzoweb, email,useradmin,userpassword,datacreazione,datafine ) " &
"values ( '" & EscapeMySql(nome) & "', '" & EscapeMySql(cognome) & "', '" & EscapeMySql(indirizzoweb) & "','" & LCase(email) & "', '" & "admin" & " ','" & LCase(password) & "','" & DateAdd(DateInterval.Second, 1, ConvertToMysqlDateTime(Now)) & "','" & DateAdd(DateInterval.Day, 365, ConvertToMysqlDateTime(Now)) & "')"

            conn.Execute(sqlstring, CommandType.Text, Nothing)

            If CreatePlatform(indirizzoweb, nome, cognome, email, password) Then

                msg = "Inserimento completato."
                utility.SendMail(esito, mailpiattaforma, nome, cognome, email, "admin", password, indirizzoweb, "sign")

            Else
                sqlstring = "delete from core_platform where indirizzoweb='" & indirizzoweb & "'"
                conn.Execute(sqlstring, CommandType.Text, Nothing)

            End If



        Catch ex As Exception
            msg &= ex.ToString
            SharedRoutines.LogWrite(ex.ToString)
        End Try

    End Sub

    Function editplatform(idprovider As String)
        Try
            Dim providerid As String = Request.Form("providerid").ToString
            If idprovider <> "" Then

                sqlstring = "update core_platform set nome ='" & Request.Form("nome") & "',email='" & Request.Form("email") & "' where id=" & idprovider

                conn.Execute(sqlstring, CommandType.Text, Nothing)

            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function
    Function CreatePlatform(indirizzoweb As String, nome As String, cognome As String, email As String, password As String)


        Dim session
        Dim sql As String

        conn = CheckDatabase(conn)

        sql = "DROP DATABASE IF EXISTS  school_" & indirizzoweb & "; CREATE DATABASE IF NOT EXISTS school_" & indirizzoweb & ";"

        Try
            conn.Execute(sql, CommandType.Text, Nothing)


            Using s As New IO.StreamReader(IO.Path.Combine(Server.MapPath("/install"), "db.sql"))
                sql = s.ReadToEnd
                Dim tmpconn As New rconnection("school_" & indirizzoweb, HostMaster)

                tmpconn.Execute(sql, CommandType.Text, Nothing)

                sql = " insert into core_st ( idst ) values(null); insert into core_user " &
" ( idst, userid, firstname, lastname, pass, email ,register_date,idprofile) " &
"values ( last_insert_id(), 'admin', '" & Trim(FormattaNominativo(nome)) & "', '" & Trim(FormattaNominativo(cognome)) & "', " &
"'" & getMd5Hash(password) & "', '" & LCase(email) & "','" & String.Format("{0:u}", Now) & "',1)"

                tmpconn.Execute(sql, CommandType.Text, Nothing)

            End Using
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Dim c As New Client("ovh-eu", "fAEwkAxYggNGGaO6", "6d5rmWYZQ44qXqW0LDMPCP7g6kk5HSK5", "QZheVYpsb1IXdAgqbfiR62vSSbhwhgOu")


            Dim json As String = "{""fieldType"":""A"",""subDomain"":""" & Request.Form("indirizzoweb") & """,""target"":""40.68.206.138""}"
            Dim result As String = c.Post("/domain/zone/training-school.it/record", json)

            result = c.Post("/domain/zone/training-school.it/refresh", "")

            Try
                Dim iisManager As New ServerManager()
                Dim site = iisManager.Sites("trainingschool")
                Dim straddress As String = indirizzoweb & "." & System.Configuration.ConfigurationSettings.AppSettings("domain").ToString
                site.Bindings.Add("*:80:" & straddress, "http")
                iisManager.CommitChanges()

            Catch ex As Exception
                LogWrite(ex.ToString)
            End Try


        Catch ex As Exception

            sql = "DROP DATABASE  'school_" & indirizzoweb & "' ;"
            Try
                conn.Execute(sql, CommandType.Text, Nothing)
            Catch ex2 As Exception
                LogWrite("error drop")
            End Try

            LogWrite(ex.ToString)
            msg = "Errore creazione piattaforma! Database cancellato!"
            Return False


        End Try

        Return True
    End Function

    Function deleteplatform(idplatform As String)

        Try
            If idplatform <> "" Then
                sqlstring = "update core_platform set flagdelete=1 where id in(" & idplatform & ")"
                conn.Execute(sqlstring, CommandType.Text, Nothing)

            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Public Sub getplatform()

        Try

            sqlstring = "select * from core_platform where (flagdelete is null or flagdelete=0) order by nome asc"

            Dim dtoriginal = conn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            FillDataTable(dt, dtoriginal)

            Dim jsonresult As String = utility.GetJson(dt)

            Response.ContentType = "application/json"
            jsonresult = jsonresult.Replace(" ", " ").Replace(vbCrLf, " ").Replace("\t", " ")

            msg = jsonresult

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

    End Sub
#End Region



End Class