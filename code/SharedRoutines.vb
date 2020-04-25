Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.ComponentModel
Imports System.Text.Encoding
Imports MySql.Data.MySqlClient
Imports System.Net.Mail
Imports System.Reflection
Imports System.Threading
Imports Microsoft.Win32
Imports System.Web.Script.Serialization
Imports SelectPdf
Imports System.Drawing
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Core
Imports System.Net

Public Class SharedRoutines



    Dim mailpiattaforma As String = ConfigurationSettings.AppSettings("email")
    Public Shared DBMaster As String = ConfigurationSettings.AppSettings("DBMaster")
    Public Shared HostMaster As String = System.Configuration.ConfigurationSettings.AppSettings("HostMaster").ToString
    Public Shared thirdDomainMaster As String = System.Configuration.ConfigurationSettings.AppSettings("thirdDomainMaster").ToString
    Private username As String = String.Empty
    Private password As String = String.Empty
    Private errorfilewrite As String = String.Empty
    Private elencoDoc As String = String.Empty
    Private mailsent As Boolean = False
    Private boolread As Boolean = True
    Private boolpriority As Boolean = False
    Private smtpAddress As String = String.Empty
    Private omsg As System.Net.Mail.MailMessage
    Private oSmtp As SmtpClient
    Private server As String = String.Empty
    Private smtpUsenaName As String = String.Empty
    Private smtpPassword As String = String.Empty
    Private dt As System.Data.DataTable
    Dim m As Object = Missing.Value
    Private f As String = String.Empty
    Dim sqlstring As String = String.Empty
    Dim sql As String = String.Empty
    Dim body As String = String.Empty
    Dim subject As String = String.Empty
    Dim rconn As rconnection


    Public Shared clIcon As New Collection


    Public Sub New()

        rconn = CheckDatabase(rconn)

    End Sub

#Region "Profile"

    Public Sub UpdateVisite(iduser As Integer, idannuncio As Integer)

        Try
            sqlstring = "update ai_annunci set n_visite=n_visite + 1 where idannuncio=" & idannuncio
            rconn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
        End Try

    End Sub

    Public Sub CheckProfilo(iduser As Integer)

        Try
            sqlstring = "insert into ai_userprofile (iduser) values(" & iduser & ") "
            rconn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
        End Try

    End Sub

    Public Function getFieldAnnuncio(field As String, idannuncio As Integer)
        Dim dr As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Try
            sqlstring = "select " & field & " from ai_annunci where idannuncio=" & idannuncio
            Dim fieldr As String = String.Empty


            dr = rconn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            fieldr = dr(field).ToString
            dr.Close()
            Return fieldr
        Catch ex As Exception
            If Not dr Is Nothing Then
                dr.Close()
            End If

            Return ""
        End Try

        Return False

    End Function
    Public Sub getPercentualeProfilo(iduser As Integer)


        Try
            sqlstring = "select `id_regione`, `id_provincia`, `id_comune`,  `curriculum`,  `sesso`, `datadinascita`, `indirizzo`, `IscrizioneRUI`, `esameAlbo`, `iscrittoAlbo`, `esameElenchi`, `iscrittoElenchi`, `compagnie`,  `dimensionePortafoglioVita`,   `lingua`, `dimensionePortafoglioRc`, `email`, `telefono` from ai_userprofile  where  iduser=" & iduser

            Dim dtoriginal = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            Dim i = 1
            For Each dc As DataColumn In dtoriginal.Columns
                If dtoriginal.Rows(0)(dc.Caption).ToString <> "" Then
                    i = i + 1
                End If
            Next

            HttpContext.Current.Session("percprofile") = CInt((i / dtoriginal.Columns.Count) * 100)

        Catch ex As Exception
        End Try

    End Sub
    Sub CheckMobile()
        Dim u As String = HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT")
        Dim b As New Regex("(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|In)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase)
        Dim v As New Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770S|802S|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|As(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|Do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase)

        If b.IsMatch(u) Or v.IsMatch(Left(u, 4)) Then
            HttpContext.Current.Session("mobile") = True
        Else
            HttpContext.Current.Session("mobile") = False
        End If
    End Sub

    Public Shared Sub SetSession()
        Try

            Dim affiliate As String

            If HttpContext.Current.Request.Url.Authority.IndexOf("localhost") > -1 Then

                HttpContext.Current.Session("master") = True
                HttpContext.Current.Session("database") = DBMaster
                HttpContext.Current.Session("logo") = "Logo.png"
                affiliate = "fad"
                If (IO.Directory.Exists(HttpContext.Current.Server.MapPath("LMSCONTENT") & "\" & affiliate)) Then
                    HttpContext.Current.Session("lmscontentpath") = HttpContext.Current.Server.MapPath("LMSCONTENT") & "\" & affiliate
                    IO.Directory.CreateDirectory(HttpContext.Current.Session("lmscontentpath"))
                Else
                    HttpContext.Current.Session("lmscontentpath") = HttpContext.Current.Server.MapPath("LMSCONTENT") & "\" & affiliate
                End If

                HttpContext.Current.Session("affiliate") = affiliate
                HttpContext.Current.Session("lmscontentpathrel") = "LMSContent/" & affiliate

            Else
                affiliate = HttpContext.Current.Request.Url.Authority.Split(".")(0).ToString
                HttpContext.Current.Session("affiliate") = affiliate
                Dim sqlstring = "select * from trainingschool.core_platform where indirizzoweb='" & affiliate & "'"
                Dim r As New rconnection("trainingschool", HostMaster)
                Dim dr As DataRow = r.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)
                HttpContext.Current.Session("nomeconvenzione") = dr("nomeconvenzione").ToString
                HttpContext.Current.Session("link") = dr("link").ToString
                HttpContext.Current.Session("master") = False
                HttpContext.Current.Session("database") = "school_" & affiliate


                If (IO.Directory.Exists(HttpContext.Current.Server.MapPath("LMSCONTENT") & "\" & affiliate)) Then
                    HttpContext.Current.Session("lmscontentpath") = HttpContext.Current.Server.MapPath("LMSCONTENT") & "\" & affiliate
                    IO.Directory.CreateDirectory(HttpContext.Current.Session("lmscontentpath"))
                Else
                    HttpContext.Current.Session("lmscontentpath") = HttpContext.Current.Server.MapPath("LMSCONTENT") & "\" & affiliate
                End If


                HttpContext.Current.Session("lmscontentpathrel") = "LMSContent/" & affiliate

                If IO.File.Exists(HttpContext.Current.Server.MapPath("LMSContent") & "/affiliate/" & affiliate & ".png") Then
                    HttpContext.Current.Session("logo") = affiliate & ".png"

                Else
                    HttpContext.Current.Session("logo") = "Logo.png"
                End If


            End If


            Dim utility As New SharedRoutines

            utility.CheckMobile()

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub
#End Region

#Region "Report And logging"
    Public Shared Function GetIPAddress() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function

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

    Sub makecategory(ByRef res_idanno, ByRef pathanno)
        Dim iduser As Integer = HttpContext.Current.Session("iduser")
        Dim path As String
        Dim dtmateria As DataTable
        Dim res_idmateria
        Dim pathmateria
        Dim dtanno As DataTable
        Dim anno As String = HttpContext.Current.Request.Form("anno")
        Dim materia As String = HttpContext.Current.Request.Form("materia")
        Dim idparent As Integer
        sqlstring = "Select * from learning_kb_res where iduser=" & iduser
        dtmateria = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        If dtmateria.Select(" r_name='" & materia & "'").Count = 1 Then
            pathmateria = dtmateria.Select(" r_name='" & materia & "'")(0)("path")
            res_idmateria = dtmateria.Select(" r_name='" & materia & "'")(0)("res_id")


            If dtmateria.Select("r_name ='" & anno & "' and idparent=" & res_idmateria & " ").Count = 1 Then

                res_idanno = dtmateria.Select(" r_name='" & anno & "' and idparent=" & res_idmateria & " ")(0)("res_id")
                pathanno = dtmateria.Select(" r_name='" & anno & "' and idparent=" & res_idmateria & " ")(0)("path").ToString & "/" & F8Int(dtmateria.Select(" idparent='" & res_idanno & "'").Count + 1)

            Else
                sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  path,lev,idparent, `r_type`,iduser) " &
               " VALUES ( '" & anno & "', '" & anno & "','" & pathmateria & "/" & F8Int(dtmateria.Select(" idparent='" & res_idmateria & "'").Count + 1) & "' , 2," & res_idmateria & ",''," & iduser & ");"
                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                sqlstring = "select res_id,path from learning_kb_res where iduser=" & iduser & " and r_name='" & anno & "' and idparent=" & res_idmateria & ""
                dtanno = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
                res_idanno = dtanno.Rows(0)("res_id")
                pathanno = dtanno.Rows(0)("path") & "/" & F8Int(dtmateria.Select(" idparent='" & res_idanno & "'").Count + 1)
            End If

        Else

            sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  path,lev,idparent, `r_type`,iduser) " &
              " VALUES ( '" & materia & "', '" & materia & "','/root/" & F8Int(dtmateria.Select(" lev=1").Count + 1) & "' , 1,0,''," & iduser & ");"
            rconn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "select res_id,path from learning_kb_res where iduser=" & iduser & " and r_name='" & materia & "'"
            dtmateria = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            res_idmateria = dtmateria.Rows(0)("res_id")
            pathmateria = dtmateria.Rows(0)("path")


            sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  path,lev,idparent, `r_type`,iduser) " &
               " VALUES ( '" & anno & "', '" & anno & "','" & pathmateria & "/" & F8Int(1) & "' , 2," & res_idmateria & ",''," & iduser & ");"
            rconn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "select res_id,path from learning_kb_res where iduser=" & iduser & " and r_name='" & anno & "'"
            dtanno = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
            res_idanno = dtanno.Rows(0)("res_id")
            pathanno = pathmateria & "/" & F8Int("1")

        End If


    End Sub

    Public Shared Function F8Int(n As Integer)
        Return String.Format("{0:D8}", n)
        Return False

    End Function
    Function getLastAccess(iduser As String, rconn As rconnection)
        Try

            Dim lasttime As String = rconn.GetDataTable("Select lastenter from core_user where idst=" & iduser & "", CommandType.Text, Nothing).Rows(0)("lastenter").ToString
            Return lasttime
        Catch ex As Exception
        End Try

    End Function
    Function getLastCourseAccess(idcourse As String, iduser As String, rconn As rconnection)
        Try

            Dim lasttime As String = rconn.GetDataTable("Select lasttime from learning_tracksession where lasttime != '' and idCourse=" & idcourse & " and iduser=" & iduser & " order by lasttime desc limit 1", CommandType.Text, Nothing).Rows(0)("lasttime").ToString
            Return lasttime
        Catch ex As Exception
        End Try

    End Function
    Function GetPercentuale(iduser As Integer, idcorso As Integer, cn As rconnection, status As String)
        Try
            Dim sqlstring As String
            Dim dr As DataRow
            Dim evaso As String = ""

            sqlstring = "Select count(*) As cntotal FROM learning_organization where idCourse=" & idcorso & " And objectType <> '' order by objectType"

            Dim countoobtotale As Integer = cn.GetDataTable(sqlstring).Rows(0)("cntotal")



            sqlstring = "select count(*) as cn from learning_commontrack join learning_organization on learning_organization.idorg = learning_commontrack.idReference where idcourse=" & idcorso & " and  status != 'ab-initio' and   status != 'attempted' and  learning_commontrack.idUser=" & iduser & ""



            Dim countob As Integer = cn.GetDataTable(sqlstring).Rows(0)("cn")



            Select Case CInt(status)
                Case 0

                    Return "Iscritto" & evaso
                Case 2
                    Return "Completato" & evaso
                Case 1
                    If countob > 0 Then
                        Return CInt((countob / countoobtotale) * 100) & " %" & evaso
                    Else
                        Return 0 & evaso
                    End If
                Case 3
                    Return "Sospeso" & evaso
            End Select





        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
    End Function
    Function getconnection(iduser As String, idcourse As String)

        Dim lconnection As String = rconn.GetDataTable("select count(*) as nconn from learning_tracksession where iduser=" & iduser & " And idcourse=" & idcourse & "", CommandType.Text, Nothing).Rows(0)("nconn")
        Return lconnection
    End Function

    Function getmateriale(iduser As String, idcourse As String)

        Dim completati As String = rconn.GetDataTable("select count(*) as completati from learning_commontrack a left join learning_organization b on a.idreference=b.idorg where a.status in ('completed','passed') and a.iduser=" & iduser & " And idcourse=" & idcourse & "", CommandType.Text, Nothing).Rows(0)("completati")
        Dim materiali As String = rconn.GetDataTable("select count(*) as materiali from learning_organization where objecttype!='' and  idcourse=" & idcourse & "", CommandType.Text, Nothing).Rows(0)("materiali")



        Return completati & " / " & materiali
    End Function

    Public Function log_Obj(obj As Object, iduser As Integer, idCourse As Integer, idreference As Integer, idresource As Integer)

        Dim o As New LogSession
        Dim idtrack As String = String.Empty

        Try


            o.SaveActionlog(iduser, idCourse, HttpContext.Current.Session("id_enter_course"), obj, "view")


            Select Case obj
                Case "scormorg"
                    idtrack = log_Scorm_items_track(iduser, idreference, idresource)
                Case "test"
                    idtrack = log_Test(iduser, idreference, idresource)
                Case "poll"
                    idtrack = log_poll(iduser, idreference, idresource)
                Case Else
                    idtrack = log_Item(iduser, idreference, idresource)
            End Select

            LogWrite("Log: " & Now & ":" & iduser & "," & idreference & "," & idresource & "," & idtrack)


            logAll(idtrack)


        Catch ex As Exception

            LogWrite(Now & ":" & ex.ToString & ex.InnerException.ToString)
        End Try
        Return idtrack


        Return False

    End Function


    Protected Sub logAll(idtrack As Integer)
        Try


            Dim status As String = String.Empty


            Dim firstAccess As String = String.Empty


            Dim dateAttempt As String = String.Empty


            Dim firsttime As String = String.Empty


            Dim lastcomplete As String = String.Empty




            sql = "select * from learning_commontrack where idtrack=" & idtrack & " and idreference=" & HttpContext.Current.Session("reference") & " and  idUser=" & HttpContext.Current.Session("iduser") & "  "

            dt = rconn.GetDataTable(sql)


            Select Case HttpContext.Current.Session("objecttype")
                Case "scormorg"
                    status = StatusObj.FirstOpen
                Case "test"
                    status = StatusObj.TestAttempted
                Case "poll"
                    status = StatusObj.FirstOpen
                Case Else
                    status = StatusObj.Completed

            End Select

            If dt.Rows.Count = 0 Then

                firstAccess = ConvertToMysqlDateTime(Now)
                dateAttempt = ConvertToMysqlDateTime(Now)


                sql = "Insert into  learning_commontrack (idreference, idUser, idtrack, objectType, dateAttempt,firstAttempt,  status) " &
                               " values (" & HttpContext.Current.Session("reference") & "," & HttpContext.Current.Session("idUser") & ", " & idtrack & " ,'" & HttpContext.Current.Session("Objecttype") & "','" & firstAccess & "','" & dateAttempt & "','" & status & "')"

                rconn.Execute(sql, CommandType.Text, Nothing)


                Update_commonlog(HttpContext.Current.Session("Reference"), HttpContext.Current.Session("iduser"), HttpContext.Current.Session("idCourse"), status, idtrack)



            ElseIf dt.Rows.Count > 0 Then
                Update_commonlog(HttpContext.Current.Session("Reference"), HttpContext.Current.Session("iduser"), HttpContext.Current.Session("idCourse"), status, idtrack)

            Else
                LogWrite(Now & ": Duplicate learning_commontrack:" & sql)
            End If

        Catch ex As Exception
            LogWrite(Now & ":" & ex.ToString)
        End Try

    End Sub



    Protected Function log_poll(iduser As Integer, idreference As Integer, idresource As Integer)
        Try

            sql = "SELECT id_track FROM learning_polltrack" &
                    " WHERE id_Reference=" & idreference &
                    "   AND id_User=" & iduser &
              "   AND id_poll=" & idresource
            dt = rconn.GetDataTable(sql, CommandType.Text, Nothing)

            If dt.Rows.Count <= 0 Then

                sql = "INSERT INTO learning_polltrack " &
        "SET id_User = " & iduser & ", " &
            "id_poll = '" & idresource & "',  " &
            "id_Reference = " & idreference & ", " &
            "date_attempt = '" & ConvertToMysqlDateTime(Now) & "' "


                rconn.Execute(sql, CommandType.Text, Nothing)

                sqlstring = "Select max(id_track)  as id from  learning_polltrack"

                Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("id").ToString()

            Else

                Return dt.Rows(0)("id_track").ToString
            End If

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

        Return False

    End Function


    Protected Function log_Test(iduser As Integer, idreference As Integer, idresource As Integer)
        Try

            sql = "Select idtrack FROM learning_testtrack " &
                    " WHERE idreference=" & idreference &
                    " And idUser=" & iduser &
                    " And idTest=" & idresource
            dt = rconn.GetDataTable(sql, CommandType.Text, Nothing)

            If dt.Rows.Count <= 0 Then

                sql = "INSERT INTO learning_testtrack " &
                      "Set idUser = " & iduser & ", " &
                      "idTest = '" & idresource & "',  " &
                      "idreference = " & idreference & ", " &
                      "date_attempt = '" & ConvertToMysqlDateTime(Now) & "', " &
                      "date_end_attempt = '" & ConvertToMysqlDateTime(Now) & "'"

                rconn.Execute(sql, CommandType.Text, Nothing)

                sqlstring = "Select max(idtrack)  as id  from learning_testtrack "
                Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("id").ToString()

            Else

                Return dt.Rows(0)("idtrack").ToString
            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    Protected Function log_Item(iduser As Integer, idreference As Integer, idresource As Integer)
        Try
            sql = "Select idtrack FROM  learning_materials_track " &
                    " WHERE idreference=" & idreference & " And idUser=" & iduser

            Dim dt As System.Data.DataTable
            dt = rconn.GetDataTable(sql, CommandType.Text, Nothing)

            If dt.Rows.Count = 0 Then

                sql = "INSERT INTO   learning_materials_track " &
                       "( idResource, idreference, idUser ) VALUES (" &
                       idresource & "," & idreference & "," & iduser & ")"

                rconn.Execute(sql, CommandType.Text, Nothing)

                sqlstring = "Select max(idtrack)  As id  from learning_materials_track "
                Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("id").ToString()

            Else
                Return dt.Rows(0)("idtrack").ToString()
            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(Now & " LOG ITEM " & ex.ToString)
        End Try

        Return False

    End Function


    Function log_Scorm_items_track(iduser As Integer, idreference As Integer, idscorm_organization As Integer)

        Try
            Dim dt As System.Data.DataTable

            Dim sqlstring As String = "Select idscorm_tracking " &
                     " FROM  learning_scorm_tracking " &
                     " WHERE idreference=" & idreference &
                     " And idscorm_item=" & idscorm_organization & " And idUser=" & iduser

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            If dt.Rows.Count <= 0 Then


                'log_Scorm_items_track(iduser, idreference, idresource)

                sqlstring = "INSERT INTO  learning_scorm_tracking " &
                 " (idUser, idreference, idscorm_item,lesson_status,user_name,credit,lesson_location)" &
                 " VALUES" &
                 " ( " & iduser & ", " & idreference & "," & idscorm_organization & ",'incomplete','" & HttpContext.Current.Session("Fullname") & "','Credit' ,'' )"

                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                sqlstring = "Select max(idscorm_tracking) as id from  learning_scorm_tracking   "
                Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("id").ToString()

            Else
                Return dt.Rows(0)("idscorm_tracking").ToString()
            End If



        Catch ex As Exception

            LogWrite("log_scorm_items: " & ex.ToString)
        End Try


        Return False

    End Function


    Function GetObjDetails(idCourse As String)
        Dim j As New Hashtable
        Try

            Dim sqlstring As String = "select idOrg,title,idresource,isterminator from  learning_organization  where  idcourse=" & idCourse & " order by path asc  "

            dt = rconn.GetDataTable(sqlstring)

            For Each dr In dt.Rows
                j.Add(dr("idOrg").ToString, dr("title").ToString & ";" & dr("idResource").ToString & ";" & dr("isterminator").ToString)
            Next

        Catch ex As Exception
            Return Nothing
        End Try

        Return j
        Return False

    End Function
    Public Function SumScormTime(iduser As Integer, idcourse As Integer)

        Dim sqlstring = " select SUM(TIME_TO_SEC(total_time)) as totaltime from  learning_scorm_tracking where idreference in (select idOrg from  learning_organization  where objecttype='scormorg' and  idCourse=" & idcourse & " ) and idUser=" & iduser
        Try

            Return rconn.GetDataTable(sqlstring).Rows(0)("totaltime").ToString()
        Catch ex As Exception
            LogWrite(ex.ToString)
            Return 0
        End Try

        Return False

    End Function


    Public Function TotalTime(iduser As Integer, idcourse As Integer)

        Dim sqlstring = "  select SUM(TIME_TO_SEC(TIMEDIFF(enterTime, lastTime))) as totaltime from   learning_tracksession   where idCourse=" & idcourse & " and idUser=" & iduser
        Try

            Return rconn.GetDataTable(sqlstring).Rows(0)("totaltime").ToString()
        Catch ex As Exception
            Dim l As New SharedRoutines
            SharedRoutines.LogWrite(ex.ToString)
            Return 0
        End Try

        Return False

    End Function

    Public Function logsession(iduser As Integer, idcourse As Integer, identer As Integer, Optional op As Boolean = True)

        Dim strHostName As String = System.Net.Dns.GetHostName()

        Dim clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(2).ToString()

        Dim idtrack As Integer = 0
        Dim status As String = String.Empty


        Dim firstAccess As String = String.Empty


        Dim dateAttempt As String = String.Empty


        Dim firsttime As String = String.Empty


        Dim lastcomplete As String = String.Empty


        Dim sql As String = String.Empty


        Try
            If op Then

                sql = "INSERT INTO   learning_tracksession    ( idEnter ,  idCourse ,  idUser ,  session_id ,  enterTime ,  numOp ,  lastFunction ,  lastOp ,  lastTime ,  ip_address ,  active ) " &
                                                                        " VALUES (" & identer & "," & HttpContext.Current.Session("idCourse") & ",  " & HttpContext.Current.Session("idUser") & ", '" & HttpContext.Current.Session.SessionID & "', '" & ConvertToMysqlDateTime(Now) & "', numop+numop+1, 'access', '', '" & ConvertToMysqlDateTime(Now) & "', '" & clientIPAddress & "' , 0);"
            Else
                sql = "update   learning_tracksession     set lastop='view',lastTime='" & ConvertToMysqlDateTime(Now) & "' where idEnter=" & identer
            End If

            rconn.Execute(sql, CommandType.Text, Nothing)

        Catch ex As Exception
            Dim l As New SharedRoutines
            SharedRoutines.LogWrite(ex.ToString)
            Return 0
        End Try
        Return False

    End Function

    Function checktime(iduser)
        sqlstring = " select sum(time_to_sec(timediff(lasttime,entertime))) as total_time,iduser from learning_tracksession where  group by (iduser) "


    End Function

    Function ConvertSecToDate(tot_time As String)

        If tot_time <> "0" And tot_time <> "" Then
            Dim hours = (tot_time / 3600)
            Dim minutes = (tot_time Mod 3600) / 60
            Dim seconds = (tot_time Mod 60)


            tot_time = Decimal.Truncate(hours) & "h " & Decimal.Truncate(minutes).ToString & "m " & Decimal.Truncate(seconds).ToString & "s "

            Return tot_time
        Else
            Return "00:00:00"
        End If

        Return False

    End Function
    Public Function LastObject(iduser As Integer, idcourse As Integer, Optional ByRef idOrg As Integer = 0)


        Try



            Dim sqlstring As String = " select idOrg,(select title from learning_organization where idorg=a.idparent) as root,title   from  learning_organization  a join learning_commontrack b on a.idOrg=b.idreference where idCourse=" & idcourse & " and b.iduser=" & iduser & " order by dateAttempt desc limit 1"
            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            idOrg = dt.Rows(0)("idOrg").ToString
            Return dt.Rows(0)("root").ToString & "->" & dt.Rows(0)("title").ToString
        Catch ex As Exception

            LogWrite(ex.ToString)
            Return ""
        End Try

        Return False

    End Function

    Function CheckIsEndOfcourse(Reference As String) As Boolean

        Try


            sqlstring = "select isTerminator from  learning_organization  where  idOrg=" & Reference
            Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("isTerminator")


        Catch ex As Exception
            LogWrite(ex.ToString & sqlstring)
        End Try

        Return False

    End Function

    Function GetField(reference As String, userid As String)
        Dim field_complete As String = String.Empty


        sqlstring = "select first_complete from learning_commontrack where idreference=" & reference & " and  idUser=" & userid & " "
        Try
            If rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("first_complete").ToString() <> "" Then
                field_complete = " last_complete ='" & ConvertToMysqlDateTime(Now) & "'"
            Else
                field_complete = "first_complete ='" & ConvertToMysqlDateTime(Now) & "',last_complete ='" & ConvertToMysqlDateTime(Now) & "'"
            End If

        Catch ex As Exception
            field_complete = "first_complete"
        End Try

        Return field_complete

        Return False

    End Function
    Function Update_commonlog(Reference As String, UserID As String, idCourse As String, status As String, idtrack As Integer)
        Dim fieldcomplete As String = String.Empty


        Try


            If CheckIsEndOfcourse(Reference) And (status = "completed" Or status = "passed") Then
                fieldcomplete = GetField(Reference, UserID)
                sqlstring = "Update learning_commontrack set " & fieldcomplete & ", dateAttempt='" & ConvertToMysqlDateTime(Now) & "', status='" & status & "' where idreference=" & Reference & " and idtrack=" & idtrack & " and  idUser=" & UserID & " "

                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                sqlstring = "Update learning_courseuser set date_complete='" & ConvertToMysqlDateTime(Now) & "',status=2 where idCourse=" & idCourse & " and iduser=" & UserID
                rconn.Execute(sqlstring, CommandType.Text, Nothing)



            ElseIf (status = "completed" Or status = "passed") Then

                fieldcomplete = GetField(Reference, UserID)

                sqlstring = "Update learning_commontrack set " & fieldcomplete & ", dateAttempt='" & ConvertToMysqlDateTime(Now) & "', status='" & status & "' where idreference=" & Reference & " and idtrack=" & idtrack & " and  idUser=" & UserID & " "
                rconn.Execute(sqlstring, CommandType.Text, Nothing)

            ElseIf (status = "failed") Then

                sqlstring = "Update learning_commontrack set dateAttempt='" & ConvertToMysqlDateTime(Now) & "', status='" & status & "' where idreference=" & Reference & " and   idUser=" & UserID & " "
                rconn.Execute(sqlstring, CommandType.Text, Nothing)

            Else

                sqlstring = "Update learning_commontrack set  dateAttempt='" & ConvertToMysqlDateTime(Now) & "' where  idtrack=" & idtrack & " and idreference=" & Reference & " and  idUser=" & UserID & " "
                rconn.Execute(sqlstring, CommandType.Text, Nothing)

            End If



        Catch ex As Exception
            LogWrite(ex.ToString & sqlstring)
            Return False
        End Try

        Return True
        Return False

    End Function


#End Region


#Region "Utility"

    Public ArrayMesi() As String = {"A", "B", "C", "D", "E", "H", "L", "M", "P", "R", "S", "T"} 'Lettere che corrispondono al mese
    Public ArrayDescrizioneMesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}
    Public ArrayCharPosizioneDispari() As Integer = {1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23}
    Public ArrayChar() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}
    Public ArrayPari As New ArrayList
    Public ArrayDispari As New ArrayList
    Public ArrayVocali() As String = {"A", "E", "I", "O", "U"}
    Public Bandiera As Boolean = False
    Public CarattereMese As String 'Si trova nella posizione 9 del codice fiscale
    Public NumeroAnno As String 'Si trova nella posizione 7 e 8 codice fiscale
    Public NumeroGiorno As String 'Si trova nella posizione 10 e 11 codice fiscale
    Dim sesso As String = String.Empty




    Public Shared Function apigeco(service As String, Optional ByVal jsonstring As String = "")
        Try
            Dim uri As New Uri("http://40.67.204.30:9001/trainingschool/" & service)
            Dim jsonDataBytes = Encoding.UTF8.GetBytes(jsonstring)
            Dim req As WebRequest = WebRequest.Create(uri)
            If Not HttpContext.Current.Session("token") Is Nothing Then
                req.Headers.Add("auth", HttpContext.Current.Session("token"))
            End If
            req.ContentType = "application/json"


            If jsonstring = "" Then
                req.Method = "GET"
            Else
                req.Method = "POST"
                req.ContentLength = jsonDataBytes.Length
                Dim stream = req.GetRequestStream()
                stream.Write(jsonDataBytes, 0, jsonDataBytes.Length)
                stream.Close()
            End If

            If HttpContext.Current.Session("token") Is Nothing Then
                HttpContext.Current.Session("token") = req.GetResponse().Headers("auth")
            End If

            Dim response = req.GetResponse().GetResponseStream()
            Dim reader As New StreamReader(response)
            Dim res = reader.ReadToEnd()
            reader.Close()
            response.Close()

            Return res

        Catch ex As Exception
            LogWriteApi(ex.ToString)
            Return Nothing
        End Try
    End Function

    Public Shared Function gettitle(ByVal title As String)

        If title.Length > 50 Then
            Return EscapeMySql(title).ToString.Substring(0, 50) & ".."
        Else
            Return EscapeMySql(title)
        End If
        Return False

    End Function

    Function hitsession(tipo, write, idsessione, iduser)

        Select Case tipo
            Case "LEZIONE"
                Return hitdocument(idsessione, iduser)
            Case "CORSO"
                Return hitcourse(idsessione, iduser)
            Case "VERIFICA SCRITTA"
                If (write) Then
                    Return hitcourse(idsessione, iduser)
                Else
                    Return hitdocument(idsessione, iduser)
                End If
            Case "WEBINAR"
                Return hitdocument(idsessione, iduser)
            Case "RICEVIMENTO"
                Return hitbooking(idsessione, iduser)
        End Select


    End Function

    Function hitbooking(idsessione, iduser)
        sqlstring = "select token from aula_prenotazioni where  iduser=" & iduser & " and  idsessione=" & idsessione
        Dim token As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("token")


        If (token = 0) Then
            Return "backgroundColor: 'red'"

        Else
            Return "backgroundColor: 'green'"
        End If

    End Function

    Function hitbooking(idsessione)
        sqlstring = "select count(*) as hit from aula_prenotazioni where token=1 and idsessione=" & idsessione
        Dim numhit As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("hit")


        If (numhit = 0) Then
            Return "backgroundColor: 'red'"
        ElseIf (numhit < numhit) Then
            Return "backgroundColor: 'orange'"
        Else
            Return "backgroundColor: 'green'"
        End If

    End Function
    Function hitsessionall(tipo, write, idsessione)

        Select Case tipo
            Case "LEZIONE"
                Return hitdocumentall(idsessione)
            Case "CORSO"
                Return hitcourseall(idsessione)
            Case "VERIFICA SCRITTA"
                If (write) Then
                    Return hitcourseall(idsessione)
                Else
                    Return hitdocumentall(idsessione)
                End If
            Case "WEBINAR"
                Return hitdocumentall(idsessione)
            Case "RICEVIMENTO"
                Return hitbooking(idsessione)
            Case Else
                Return "backgroundColor: 'red'"
        End Select


    End Function

    Function hitdocument(idsessione As Integer, iduser As Integer)
        sqlstring = "select count(*) as numdoc from aula_document a join aula_documentstudents b on a.id=b.iddoc where b.iduser=" & iduser & " and  a.idsessione= " & idsessione
        Dim numdoc As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("numdoc")
        sqlstring = "select count(*) as numhit from aula_document a join aula_documentstudents b on a.id=b.iddoc where date_hit <> '' and b.iduser=" & iduser & "  and a.idsessione= " & idsessione
        Dim numhit As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("numhit")



        If (numhit = 0) Then
            Return "backgroundColor: 'red'"
        ElseIf (numhit < numdoc) Then
            Return "backgroundColor: 'orange'"
        Else
            Return "backgroundColor: 'green'"
        End If
    End Function

    Function hitcourse(idsessione As Integer, iduser As Integer)
        Try
            sqlstring = "select status from learning_courseuser a join aula_course b on a.idcourse=b.idcourse where idsessione=" & idsessione & " and a.iduser=" & iduser
            Dim status As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("status")
            If status = 0 Then
                Return "backgroundColor: 'red'"
            ElseIf status < 1 Then
                Return "backgroundColor: 'orange'"
            Else
                Return "backgroundColor: 'green'"
            End If
        Catch ex As Exception
            Return "backgroundColor: 'red'"
        End Try
    End Function

    Function hitdocumentall(idsessione As Integer)
        Try
            sqlstring = "select count(*) as numdoc from aula_document a  left join aula_documentstudents b on a.id=b.iddoc where   a.idsessione= " & idsessione
            Dim numdoc As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("numdoc")


            sqlstring = "select count(*) as numhit from aula_document a join aula_documentstudents b on a.id=b.iddoc where date_hit <> ''   and a.idsessione= " & idsessione

            Dim numhit As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("numhit")

            If (numhit = 0) Then
                Return "backgroundColor: 'red'"
            ElseIf (numhit < numdoc) Then
                Return "backgroundColor: 'orange'"
            Else
                Return "backgroundColor: 'green'"
            End If

        Catch ex As Exception
            Return "backgroundColor: 'red'"
        End Try
    End Function

    Function hitcourseall(idsessione As Integer)
        sqlstring = "select count(*) as nstatus from learning_courseuser a join aula_course b on a.idcourse=b.iduser where status=2 and  idsessione=" & idsessione
        Dim status As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("nstatus")
        sqlstring = "select count(*) as nprenotati from aula_prenotazioni where  idsessione=" & idsessione
        Dim nprenotati As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("nprenotati")

        If status < nprenotati Then
            Return "backgroundColor: 'red'"
        Else
            Return "backgroundColor: 'green'"
        End If
    End Function
    Private Function Mese(ByVal CarattereMese As String) As String

        Try
            CarattereMese = UCase(CarattereMese)
            For i As Integer = 0 To UBound(ArrayMesi) Step 1
                If CarattereMese = ArrayMesi(i).ToString Then
                    Return i
                End If
            Next
            Return -1
        Catch ex As Exception
            MsgBox("Errore : " & ex.ToString, vbCritical, "ERRORE!!!")
            Return -1
        End Try
        Return False

    End Function
    Public Function LetturaCodiceFiscale(codicefiscale As String) As String

        Try
            Dim CarattereMese As String = Mese(codicefiscale.Substring(8, 1).ToString)
            If IsNumeric(codicefiscale.Substring(6, 2).ToString) And IsNumeric(codicefiscale.Substring(9, 2).ToString) Then
                NumeroAnno = codicefiscale.Substring(6, 2).ToString
                NumeroGiorno = codicefiscale.Substring(9, 2).ToString
            Else


            End If

            Dim VerificaNumero As Integer = NumeroGiorno
            If VerificaNumero - 40 > 0 Then
                NumeroGiorno = NumeroGiorno - 40
                Return "a"
            Else
                Return "o"
            End If

            'If NumeroGiorno < 9 Then
            '    'Visualizza la data in questa maniera dd MESE yy ex: 06 Dicembre 86
            '    'txtDataDiNascita.Text = "0" & NumeroGiorno & " " & ArrayDescrizioneMesi(CarattereMese) & " " & NumeroAnno

            '    'Visualizza la data in questa maniera dd / mm / yy ex: 06/12/86
            '    txtDataDiNascita.Text = "0" & NumeroGiorno & "/" & CarattereMese + 1 & "/" & NumeroAnno
            'Else
            'Visualizza la data in questa maniera dd MESE yy ex: 16 Dicembre 86
            'txtDataDiNascita.Text = NumeroGiorno & " " & ArrayDescrizioneMesi(CarattereMese) & " " & NumeroAnno
        Catch ex As Exception
            Return "o"
        End Try


        Return False

    End Function


    Public Shared Function GetStreamAsByteArray(ByVal stream As System.IO.Stream) As Byte()

        Dim streamLength As Integer = System.Convert.ToInt32(stream.Length)

        Dim fileData As Byte() = New Byte(streamLength) {}

        ' Read the file into a byte array
        stream.Read(fileData, 0, streamLength)
        stream.Close()

        Return fileData

        ' Return False 


    End Function


    Public Shared Function getMd5Hash(ByVal input As String) As String

        ' Create a new instance of the MD5 object.
        Dim md5Hasher As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5.Create()

        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input))

        ' Create a new Stringbuilder to collect the bytes
        ' and create a string.
        Dim sBuilder As New StringBuilder()

        ' Loop through each byte of the hashed data 
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string.
        Return sBuilder.ToString()

        Return False

    End Function

    Public Shared Function CreateRandomPassword(ByVal PasswordLength As Integer) As String

        Dim _allowedChars As String = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ"
        Dim randomNumber As New Random()
        Dim chars(PasswordLength - 1) As Char
        Dim allowedCharCount As Integer = _allowedChars.Length
        For i As Integer = 0 To PasswordLength - 1
            chars(i) = _allowedChars.Chars(CInt(Fix((_allowedChars.Length) * randomNumber.NextDouble())))
        Next i
        Return New String(chars)
        Return False

    End Function

    Public Shared Function CheckAcl(page As String)
        Dim conntmp As rconnection
        conntmp = CheckDatabase(conntmp)
        Dim sqlstring = "select * from core_acl a join core_acl_rules b on a.id=b.id where idprofile=" & HttpContext.Current.Session("idprofile") & " and page='" & Replace(page, "/", "") & "'"
        Dim dt As System.Data.DataTable = conntmp.GetDataTable(sqlstring, CommandType.Text, Nothing)

        If dt.Rows.Count > 0 Then
            Return False
        Else
            Return True
        End If


    End Function

    Public Function InvioMail(ByVal a As String, ByVal da As String, ByVal bcc As String, ByVal subject As String, ByVal body As String, ByVal file As String, Optional toDan As Boolean = False) As String



        Dim str As String = String.Empty



        Try

            omsg = New System.Net.Mail.MailMessage()

            Dim sp As String() = Split(a, ";")

            For Each s As String In sp
                If s <> "" Then
                    omsg.To.Add(New MailAddress(Trim(s)))
                Else
                    Try
                        omsg.To.Add(New MailAddress(a))
                    Catch ex As Exception
                    End Try

                End If
            Next



            omsg.From = New MailAddress(da)
            omsg.Subject = subject
            omsg.IsBodyHtml = True
            omsg.Body = body




            omsg.ReplyTo = New System.Net.Mail.MailAddress(da)

            omsg.Sender = New MailAddress(da)


            omsg.Bcc.Add(System.Configuration.ConfigurationSettings.AppSettings("bcc"))




            Dim sbcc As String() = Split(bcc, ";")

            For Each sc As String In sbcc
                If sc <> "" Then
                    If UCase(Trim(a)) <> UCase(Trim(sc)) Then
                        Try
                            omsg.CC.Add(New MailAddress(Trim(Replace(sc, ";", ""))))
                        Catch ex As Exception
                        End Try

                    End If
                End If
            Next





            omsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure

            If boolpriority Then
                omsg.Priority = MailPriority.High
            Else
                omsg.Priority = MailPriority.Normal
            End If

            If boolread Then omsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            Dim aConfig As System.Configuration.ConfigurationSettings


            server = System.Configuration.ConfigurationSettings.AppSettings("Smtp.Server")

            username = System.Configuration.ConfigurationSettings.AppSettings("Smtp.Username")

            password = System.Configuration.ConfigurationSettings.AppSettings("Smtp.Password")



            omsg.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8")
            Dim plainView As System.Net.Mail.AlternateView
            plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(body, "<(.|\n)*?>", String.Empty), Nothing, "text/plain")
            Dim htmlView As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, Nothing, "text/html")

            omsg.AlternateViews.Add(plainView)
            omsg.AlternateViews.Add(htmlView)




            username = "aa322eb16c1e6fad65dd46ec44f65faa"
            password = "43d5fb8c7ccb4cf9d2f08cb17093ac57"
            server = "in.mailjet.com"
            Dim basicauthenticationinfo As _
            New System.Net.NetworkCredential(username, password)
            oSmtp = New SmtpClient(server)
            oSmtp.UseDefaultCredentials = False
            oSmtp.Credentials = basicauthenticationinfo
            oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network

            oSmtp.Port = 25



            AddHandler oSmtp.SendCompleted, AddressOf SendCompletedCallback
            Dim fileadd() As String = file.Split(";")
            For Each fi In fileadd
                If IO.File.Exists(fi) Then
                    Dim oAttch As System.Net.Mail.Attachment = New System.Net.Mail.Attachment(fi, System.Net.Mime.MediaTypeNames.Application.Pdf)
                    omsg.Attachments.Add(oAttch)
                End If

            Next
            Try
                oSmtp.Send(omsg)

                Thread.Sleep(1000)
                Return "Email Inviata " & a & vbCrLf
            Catch ex As InvalidOperationException
                Return "Non è stato specificato il nome Host del server" & a
            Catch ex As SmtpFailedRecipientException
                Return "Tentativo di invio al server locale, ma non è presente una mailbox: " & a
            Catch ex As SmtpException
                Return "Utente non valido/Host non trovato/Altro errore in fase di invio " & a
            Catch ex As Exception
                Return ex.ToString & a
            End Try
        Catch ex As System.Exception

            LogWrite(ex.ToString)
            Return ex.Message
        End Try
        Return False

    End Function

    Sub SendMail(ByRef esito As String, ByVal from As String, nome As String, cognome As String, email As String, username As String, password As String, indirizzoweb As String, format As String, Optional pay As String = "")

        Dim body As String = String.Empty


        Dim subject As String = String.Empty


        Try

            Select Case format
                Case "sign"
                    subject = "Training school | Iscrizione piattaforma "
                    body = Getmailformat(True, "sign")

                    body = body.Replace("[nominativo]", nome & " " & cognome)
                    body = body.Replace("[indirizzoweb]", "https://" & indirizzoweb)
                    body = body.Replace("[username]", username)
                    body = body.Replace("[password]", password)

                    esito = InvioMail(email, from, mailpiattaforma, subject, body, "")

                Case "pass"
                    subject = "Training school | Recupero password "
                    body = Getmailformat(True, "sign")

                    body = body.Replace("[nominativo]", nome & " " & cognome)
                    body = body.Replace("[indirizzoweb]", "https://" & indirizzoweb)
                    body = body.Replace("[username]", username)
                    body = body.Replace("[password]", password)

                    esito = InvioMail(email, from, mailpiattaforma, subject, body, "")


            End Select
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)

        End Try


    End Sub




    Function AddAvatar(filename As String)



        Try
            sqlstring = "Update ai_userprofile set avatar='" & Replace(filename, "\", "\\") & "' where iduser=" & HttpContext.Current.Session("iduser")
            rconn.Execute(sqlstring, CommandType.Text, Nothing)

            Return True

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
            Return False
        End Try

        Return False

        Return False

    End Function



    Function CreateLogo(title As String, FileName As String)
        Try
            sql = "Insert into  imglogo (title,filename) " &
                                  " values ('" & EscapeMySql(title) & "','" & FileName & "')"

            rconn.Execute(sql, CommandType.Text, Nothing)
        Catch ex As Exception
        End Try

        Return False

    End Function

    Public Shared Function GetCourseStatus(st As Integer)
        Select Case st
            Case 1
                Return "In Corso"
            Case 0
                Return "Iscritto"
            Case 2
                Return "completato"
            Case 3
                Return "Sospeso"
        End Select
        Return False

    End Function

    Public Shared Sub WriteCookie(name As String, value As String)
        Dim myCookie As HttpCookie = New HttpCookie(name)
        myCookie(name) = value
        myCookie("Color") = "Blue"
        myCookie.Expires = Now.AddDays(1)
        HttpContext.Current.Response.Cookies.Add(myCookie)
    End Sub

    Public Shared Function ReadCookie(name As String)
        If (HttpContext.Current.Request.Cookies(name) IsNot Nothing) Then
            Dim userSettings As String = String.Empty

            If (HttpContext.Current.Request.Cookies(name) IsNot Nothing) Then
                Return HttpContext.Current.Request.Cookies(name).Value
            End If
        End If
        Return String.Empty

    End Function


    Public Shared Function CheckDatabase(ByVal rconn As rconnection) As rconnection
        Try
            Dim database As String
            If HttpContext.Current.Request.Url.Authority.IndexOf(thirdDomainMaster) > -1 Or HttpContext.Current.Request.Url.Authority.IndexOf("localhost") > -1 Then


                database = DBMaster

            Else
                database = "school_" & HttpContext.Current.Request.Url.Authority.Split(".")(0).ToString


            End If

            If database <> "" Then
                If rconn Is Nothing Then
                    rconn = New rconnection(database, HostMaster)

                    Return rconn
                Else
                    Return rconn
                End If
            Else
                HttpContext.Current.Response.Redirect("Wflogin.aspx")
            End If



        Catch ex As Exception
            LogWrite("ERROR DATABASE: " & ex.ToString)
        End Try



    End Function

    Function getCourse(idcourse As String)
        Try
            Dim strconn = "select name from  learning_course  where idCourse=" & idcourse
            Return rconn.GetDataTable(strconn, CommandType.Text, Nothing).Rows(0)("name")

        Catch ex As Exception
            Return ""
        End Try
        Return False

    End Function

    Function ResetDatabase(db As String)
        Dim strconn = "SHOW TABLES"
        dt = rconn.GetDataTable(strconn, CommandType.Text, Nothing)

        For Each dr In dt.Rows
            Try
                If dr("Tables_in_" & db) <> " core_user " Then
                    strconn = "TRUNCATE " & dr("Tables_in_" & db) & ""
                    rconn.Execute(strconn, CommandType.Text, Nothing)
                End If

            Catch ex As Exception
            End Try

        Next
        Return False

    End Function


    Public Function EscapePath(str As String)
        Return Regex.Replace(str, "[^a-zA-Z0-9_]+", "_", RegexOptions.Compiled)
        Return False

    End Function

    Public Shared Function FormattaNominativo(ByVal Nominativo As String) As String

        Dim spazioPrima As Boolean = True
        Dim sb As New System.Text.StringBuilder(Nominativo.Length)
        For Each c As Char In Nominativo
            If Char.IsPunctuation(c) Then
                sb.Append(c)
            ElseIf Char.IsLetter(c) Then
                If spazioPrima Then
                    c = Char.ToUpper(c)
                Else
                    c = Char.ToLower(c)
                End If
                sb.Append(c)
                spazioPrima = False
            ElseIf Char.IsWhiteSpace(c) Then
                If Not spazioPrima Then
                    sb.Append(" "c)
                End If
                spazioPrima = True
            End If
        Next
        Return sb.ToString()
        Return False

    End Function

    Public Function ExtractZipFile(archiveFilenameIn As String, password As String, outFolder As String)
        Dim zf As ZipFile = Nothing
        Try
            Dim fs As FileStream = File.OpenRead(archiveFilenameIn)
            zf = New ZipFile(fs)
            If Not [String].IsNullOrEmpty(password) Then    ' AES encrypted entries are handled automatically
                zf.Password = password
            End If

            For Each zipEntry As ZipEntry In zf
                If Not zipEntry.IsFile Then     ' Ignore directories
                    Continue For
                End If
                Dim entryFileName As [String] = zipEntry.Name

                Dim buffer As Byte() = New Byte(4095) {}    ' 4K is optimum
                Dim zipStream As Stream = zf.GetInputStream(zipEntry)

                ' Manipulate the output filename here as desired.
                Dim fullZipToPath As [String] = Path.Combine(IO.Path.Combine(IO.Path.GetDirectoryName(archiveFilenameIn), outFolder), entryFileName)
                Dim directoryName As String = Path.GetDirectoryName(fullZipToPath)
                If directoryName.Length > 0 Then
                    Directory.CreateDirectory(directoryName)
                End If


                Using streamWriter As FileStream = File.Create(fullZipToPath)
                    StreamUtils.Copy(zipStream, streamWriter, buffer)
                End Using
            Next


        Finally
            If zf IsNot Nothing Then
                zf.IsStreamOwner = True
                zf.Close()
            End If
        End Try

        Return True
        Return False

    End Function

    Public Shared Function UnEscapeMysql(str As String)
        If str.IndexOf("\") > -1 Then
            Return Replace(str, "\'", "`")
        End If
        Return str
        Return False

    End Function

    Public Shared Function EscapeSql(str As String)
        If str Is Nothing Then
            Return ""
        End If
        If str.IndexOf("''") > -1 Then
            Return str
        ElseIf str.IndexOf(Chr(39)) > -1 Then
            Return Replace(str, "'", "''")

        Else
            Return str
        End If

        Return False

    End Function

    Public Shared Function EscapeMySql(str As String)
        Try
            If str Is Nothing Then
                Return ""
            End If
            If str.IndexOf("\'") > -1 Then
                Return str
            ElseIf str.IndexOf(Chr(39)) > -1 Then
                Return Replace(str, "'", "\'")

            Else
                Return str
            End If
        Catch ex As Exception
        End Try
        Return True
        Return False

    End Function

    Public Function ReadFile(sPath As String) As Byte()
        'Initialize byte array with a null value initially.
        Dim data As Byte() = Nothing

        'Use FileInfo object to get file size.
        Dim fInfo As New FileInfo(sPath)
        Dim numBytes As Long = fInfo.Length

        'Open FileStream to read file
        Using fStream As New FileStream(sPath, FileMode.Open, FileAccess.Read)

            'Use BinaryReader to read file stream into byte array.
            Dim br As New BinaryReader(fStream)

            'When you use BinaryReader, you need to supply number of bytes to read from file.
            'In this case we want to read entire file. So supplying total number of bytes.
            data = br.ReadBytes(CInt(numBytes))
        End Using
        Return data
        ' Return False 


    End Function

    Public Shared Function ConvertToMysqlDateTime(ByVal d As DateTime)
        Return String.Format("{0:yyyy-M-d HH:mm:ss}", CDate(d))
        Return False

    End Function

    Function GetTitleReference(reference As String)
        Try

            sqlstring = "select title from learning_organization  where idOrg=" & reference


            Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("title").ToString()

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString & "query: " & sqlstring)
            Return ""
        End Try

        Return False

    End Function
    Public Function GetQuestionNumber(idOrg As Integer)
        Dim sqlstring = "select count(*) as QuestionNumber from learning_testquest where idtest in (select idresource from  learning_organization  where idOrg=" & idOrg & ")"


        Try
            Return rconn.GetDataTable(sqlstring).Rows(0)("QuestionNumber").ToString()
        Catch ex As Exception

            LogWrite(ex.ToString)
        End Try
        Return True
        Return False

    End Function

    Function getFilePdf(htmlstring As String, corso As String, format As String, nomesito As String, societa As String, nominativo As String, Optional ByVal orientation As Integer = 0, Optional ByVal intestazione As Integer = 0, Optional ByVal preview As Boolean = False)

        Dim f As String
        Dim header As String = "header"
        Dim footer As String = "footer"

        Try
            f = HttpContext.Current.Server.MapPath("temp") & "/Preview.pdf"

            Dim converter As New HtmlToPdf()

            converter.Options.PdfPageSize = PdfPageSize.A4
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait
            converter.Options.MarginLeft = System.Configuration.ConfigurationSettings.AppSettings("ml")
            converter.Options.MarginRight = System.Configuration.ConfigurationSettings.AppSettings("mr")
            converter.Options.MarginBottom = System.Configuration.ConfigurationSettings.AppSettings("mb")
            converter.Options.MarginTop = System.Configuration.ConfigurationSettings.AppSettings("mt")


            converter.Header.Height = System.Configuration.ConfigurationSettings.AppSettings("hh")
            converter.Footer.Height = System.Configuration.ConfigurationSettings.AppSettings("hf")


            If corso.StartsWith("cod") Then
                converter.Options.MarginLeft = System.Configuration.ConfigurationSettings.AppSettings("cl")
                converter.Options.MarginRight = System.Configuration.ConfigurationSettings.AppSettings("cr")
                converter.Options.MarginBottom = System.Configuration.ConfigurationSettings.AppSettings("cb")
                converter.Options.MarginTop = System.Configuration.ConfigurationSettings.AppSettings("ct")
                converter.Header.Height = System.Configuration.ConfigurationSettings.AppSettings("ch")
                converter.Footer.Height = System.Configuration.ConfigurationSettings.AppSettings("cf")
                header = "headerAttestato"
                footer = "footerAttestato"
            End If

            ' create a new pdf document converting the html string of the page
            If orientation = 1 Then
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape
            End If

            If intestazione = 1 Then


                converter.Options.DisplayHeader = 1
                converter.Header.DisplayOnEvenPages = 1

                Dim headerHtml As PdfHtmlSection = New PdfHtmlSection(Getmailformat(True, header), "./")
                headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
                converter.Header.Add(headerHtml)


                converter.Options.DisplayFooter = 1
                converter.Footer.DisplayOnEvenPages = 1


                Dim footerHtml As PdfHtmlSection = New PdfHtmlSection(Getmailformat(True, footer), "./")
                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit

                converter.Footer.Add(footerHtml)
            End If

            Dim Text As New PdfTextSection(0, 10, " {page_number}/{total_pages} ", New Font("Times New Roman", 12))
            Text.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00309c")
            Text.HorizontalAlign = PdfTextHorizontalAlign.Right
            converter.Footer.Add(Text)

            Dim doc As SelectPdf.PdfDocument = converter.ConvertHtmlString(htmlstring)

            If preview Then
                doc.Save(HttpContext.Current.Response, True, "./Sample.pdf")
            Else
                doc.Save(f)
            End If

            ' close pdf document
            doc.Close()






        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return f
    End Function
    Public Function Getmailformat(Optional ByVal f As Boolean = True, Optional ByVal format As String = " mailformat")
        Dim msg As String
        Dim dr As MySqlDataReader
        Try

            Dim sqlstring = " Select * from core_formathtml where meta_key='" & format & "' order by meta_key asc"
            dr = rconn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            If dr.HasRows Then
                Dim formatmail As String = HttpUtility.UrlDecode(dr("meta_value"))
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
            dr.Close()
            LogWrite(ex.ToString)
        End Try

        Return msg
    End Function
    Function GetDocPdf(ifsave As Boolean, name As String, htmlstring As String, orientation As String, intestazione As Boolean, paging As Boolean)

        Dim converter As New HtmlToPdf()

        Dim path As String = HttpContext.Current.Server.MapPath("temp") & "/" & name & ".pdf"

        ' create a new pdf document converting the html string of the page
        If orientation = "L" Then
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape
        Else
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait

        End If



        If intestazione Then

            converter.Options.DisplayHeader = 1
            converter.Header.DisplayOnEvenPages = 1
            converter.Header.Height = System.Configuration.ConfigurationSettings.AppSettings("hh")

            Dim headerHtml As PdfHtmlSection = New PdfHtmlSection(Getmailformat(True, "header"), "./")
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
            converter.Header.Add(headerHtml)


            converter.Options.DisplayFooter = 1
            converter.Footer.DisplayOnEvenPages = 1
            converter.Footer.Height = System.Configuration.ConfigurationSettings.AppSettings("hf")

            Dim footerHtml As PdfHtmlSection = New PdfHtmlSection(Getmailformat(True, "footer"), "./")
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit

            converter.Footer.Add(footerHtml)
        End If

        If paging Then
            Dim Text As New PdfTextSection(0, 10, "Pagina: {page_number} di {total_pages}  ", New System.Drawing.Font("Arial", 8))
            Text.HorizontalAlign = PdfTextHorizontalAlign.Right
            converter.Footer.Add(Text)
        End If
        Dim f As String = "./" & name & ".pdf"
        Dim doc As SelectPdf.PdfDocument = converter.ConvertHtmlString(htmlstring)
        If ifsave Then
            doc.Save(HttpContext.Current.Response, True, f)
        Else
            doc.Save(path)
            Return f
        End If

        ' close pdf document
        doc.Close()

        Return f

    End Function


    Public Shared Function getIcon(ByVal fExt As String, Optional ByVal smallIcon As Boolean = False) As System.Drawing.Icon
        Dim fName As String = String.Empty

        'The file name to get the icon from.
        Try
            Dim hImgSmall As IntPtr  'The handle to the system image list.
            Dim hImgLarge As IntPtr  'The handle to the system image list.

            Dim shinfo As SHFILEINFO
            shinfo = New SHFILEINFO()

            shinfo.szDisplayName = New String(Chr(0), 260)
            shinfo.szTypeName = New String(Chr(0), 80)

            Dim myIcon As System.Drawing.Icon
            If clIcon.Contains(fExt) Then
                myIcon = clIcon(fExt)
            End If

            If myIcon Is Nothing Then
#Disable Warning BC42104 ' La variabile 'myIcon' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
#Disable Warning BC42104 ' La variabile 'myIcon' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
                Using x As IO.FileStream = IO.File.Create(HttpContext.Current.Server.MapPath("images") & "." & fExt)
#Enable Warning BC42104 ' La variabile 'myIcon' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
#Enable Warning BC42104 ' La variabile 'myIcon' è stata usata prima dell'assegnazione di un valore. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null. 
                    fName = x.Name

                    If smallIcon Then
                        'Use this to get the small icon.
                        hImgSmall = SHGetFileInfo(fName, 0, shinfo, Marshal.SizeOf(shinfo),
                            SHGFI_ICON Or SHGFI_SMALLICON)
                    Else
                        'Use this to get the large icon.
                        hImgLarge = SHGetFileInfo(fName, 0, shinfo, Marshal.SizeOf(shinfo),
                        SHGFI_ICON Or SHGFI_LARGEICON)
                    End If

                    'The icon is returned in the hIcon member of the shinfo struct.
                    myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon)

                    clIcon.Add(myIcon, fExt)
                End Using
            End If

            Return myIcon
        Catch ex As Exception

            Return Nothing
        Finally
            If IO.File.Exists(fName) Then IO.File.Delete(fName)
        End Try
        ' Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Icon'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Icon'. 
    End Function


    Public Shared Function FileToArray(ByVal sFilePath As String) As Byte()
        Dim fs As System.IO.FileStream = New System.IO.FileStream(sFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read)
        Dim br As System.IO.BinaryReader = New System.IO.BinaryReader(fs)
        Dim bytes As Byte() = br.ReadBytes(fs.Length)
        br.Close()
        fs.Close()
        Return bytes
        ' Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function


    Public Shared Function ByteArrayToFile(ByVal filename As String, ByVal bytedata As Byte())

        Dim oFileStream As System.IO.FileStream
        oFileStream = New System.IO.FileStream(filename, FileMode.Create, FileAccess.Write)
        oFileStream.Write(bytedata, 0, bytedata.Length)
        oFileStream.Close()

        Return True
        Return False

    End Function

    Public Shared Function CheckPathTemp() As Boolean

        If HttpContext.Current.Session("pathtemp") Is Nothing Then
            If Directory.Exists(IO.Path.Combine(IO.Path.GetTempPath(), HttpContext.Current.Session.SessionID)) Then
                HttpContext.Current.Session("pathtemp") = IO.Path.Combine(IO.Path.GetTempPath(), HttpContext.Current.Session.SessionID)
                Return True
            Else
                Directory.CreateDirectory(IO.Path.Combine(IO.Path.GetTempPath(), HttpContext.Current.Session.SessionID))
                HttpContext.Current.Session("pathtemp") = IO.Path.Combine(IO.Path.GetTempPath(), HttpContext.Current.Session.SessionID)
                Return True
            End If

        Else

            Return True
        End If
        Return False

        Return False

    End Function


    Function Normalizza(str As String)
        If str.IndexOf("à") > -1 Then
            str = str.Replace("à", "a'")
        End If
        If str.IndexOf("ù") > -1 Then
            str = str.Replace("ù", "u'")
        End If
        If str.IndexOf("è") > -1 Then
            str = str.Replace("è", "e'")
        End If
        If str.IndexOf("ì") > -1 Then
            str = str.Replace("ì", "i'")
        End If
        If str.IndexOf("ò") > -1 Then
            str = str.Replace("ò", "o'")
        End If
        If str.IndexOf("é") > -1 Then
            str = str.Replace("é", "e'")
        End If
        If str.IndexOf("é") > -1 Then
            str = str.Replace("è", "e'")
        End If
        Return str
    End Function

    Function createusername(nome As String, cognome As String)
        cognome = cognome.Replace("’", "'")
        cognome = cognome.Replace("\", "")
        nome = nome.Replace("’", "'")
        nome = nome.Replace("\", "")
        nome = Normalizza(nome)
        cognome = Normalizza(cognome)

        If cognome.IndexOf("é") > -1 Then
            cognome = cognome.Replace("è", "e'")
        End If

        Dim username As String = ""

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
            If username.Length < 6 Then
                username = Trim(LCase(Replace(Trim(cognome & nome), "'", "")).Substring(0, 6))
            End If
        End Try
        Return LCase(username)
    End Function
    Public Function iscriviexcel(savedFileName As String, tipo As Integer)

        Dim nominativo As String
        Dim result As String
        Dim esito As String
        Dim i As Integer = 1
        Dim ifnew As String
        Dim sqlstring As String
        Dim utente As String
        Dim msg As String

        Dim s As New System.IO.StreamReader(savedFileName, System.Text.Encoding.Default)
        While Not s.EndOfStream
            Dim appstr() As String = s.ReadLine.Split(";")

            If appstr(0) <> "Cognome" Then
                Dim cognome As String = appstr(0)
                Dim nome As String = appstr(1)
                Dim email As String = appstr(2)
                Dim cf As String = appstr(3)
                Dim tel As String = appstr(4)
                If nome = "" Then
                    Return ""
                End If

                username = createusername(nome, cognome)

                utente = nome & " " & cognome



                Dim idst As Integer
                Dim dataiscrizione As Object = CDate(Now)
                Dim passwordReal As String = LCase(SharedRoutines.CreateRandomPassword(6))
                Dim password = SharedRoutines.getMd5Hash(passwordReal)
                username = findusername(username, nome, cognome, rconn)
                Dim ifexist As Boolean = False
                Dim dtExist As System.Data.DataTable = GetIfUserExist(nome, cognome, cf, email, rconn, "")

                If Not dtExist Is Nothing Then
                    idst = dtExist.Rows(0)("idst")
                    username = Replace(dtExist.Rows(0)("userid"), "/", "")
                    ifexist = True
                    ifnew = ""
                End If
                ifnew = "new"

                If Not ifexist Then
                    passwordReal = LCase(SharedRoutines.CreateRandomPassword(6))
                    password = SharedRoutines.getMd5Hash(passwordReal)
                    username = findusername(username, nome, cognome, rconn)
                    Try
                        sqlstring = "INSERT INTO `core_st` (`idst`)  VALUES(NULL); INSERT INTO  core_user " &
                    " ( idst, userid, firstname, lastname, pass, email ,valid,register_date,idprofile,clearpass,tel,cf) " &
                    "VALUES ( LAST_INSERT_ID(), '/" & Trim(username) & "', '" & Trim(FormattaNominativo(EscapeStr(nome))) & "', '" & Trim(FormattaNominativo(EscapeStr(cognome))) & "', " &
                    "'" & password & "', '" & LCase(email) & "',1,'" & String.Format("{0:u}", dataiscrizione) & "'," & tipo & ",'" & password & "',,'" & tel & "',,'" & cf & "')"
                        rconn.Execute(sqlstring, CommandType.Text, Nothing)


                        SendMail(esito, mailpiattaforma, nome, cognome, email, username, passwordReal, "", "sign")

                        msg &= esito & vbCrLf
                    Catch ex As Exception
                    End Try
                End If
            End If
        End While
        Return msg
    End Function
    Public Function EscapeStr(str As String)
        If str.IndexOf("\") > -1 Then
        Else
            str = Replace(str, "'", "\'")
        End If
        Return str
    End Function

    Function GetIfUserExist(ByVal nome As String, cognome As String, codfis As String, email As String, cnDocebo As rconnection, convenzione As String)

        cognome = EscapeStr(cognome)
        nome = EscapeStr(nome)
        Dim sqlstring As String

        sqlstring = "select idst,userid from core_user where firstname='" & nome & "' and lastname='" & cognome & "' and  cf='" & codfis & "' order by register_date desc"
        Dim dt As System.Data.DataTable = cnDocebo.GetDataTable(sqlstring, CommandType.Text, Nothing)
        Try
            'If dt.Rows(0)("CF") = 1 And dt.Rows(0)("Convenzione") = 1 Then
            If dt.Rows.Count >= 1 Then
                Return dt
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetLastId(id As String, table As String)
        sqlstring = "select max(" & id & ") as id from " & table & " "
        Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("id")


        Return False

    End Function
    Function GetDatatable(sqlstring)
        Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        Return False

    End Function
    Public Function getGridData(page As Integer, totalpage As Integer, totalrecords As Integer, _search As String, sord As String, dtsource As System.Data.DataTable)
        Dim dtRecords As System.Data.DataTable = dtsource
        ' Data Table

        Try
            Dim dv As DataView = dtsource.AsDataView()
            dv.Sort = _search & " " & sord
        Catch ex As Exception
        End Try


        Dim objJqGrid As New JqGridData()
        objJqGrid.page = page
        objJqGrid.total = totalpage
        objJqGrid.records = totalrecords
        objJqGrid.rows = ConvertDT(dtRecords)

        Dim liob As New List(Of String)()
        For Each column As DataColumn In dtRecords.Columns
            liob.Add(column.ColumnName)
        Next
        objJqGrid.rowsHead = liob

        Dim colcontetn As New List(Of Object)()
        For Each item In liob
            Dim obj As New JqGridDataHeading()
            obj.name = item.ToString()
            obj.index = item.ToString()
            colcontetn.Add(obj)
        Next
        objJqGrid.rowsM = colcontetn

        Dim jser As New JavaScriptSerializer()
        Return jser.Serialize(objJqGrid)

    End Function
    Public Shared Function SetAcl(ByVal acl As List(Of String))


        If acl.Contains(HttpContext.Current.Session("idprofile")) Then

        Else
            HttpContext.Current.Response.Redirect("Wflogin.aspx?op=Non autorizzato")
        End If


    End Function

    ''' this method converts the data table to list object
    ''' </summary>
    ''' <param name="dsProducts"></param>
    ''' <returns></returns>
    Public Function ConvertDT(dsProducts As System.Data.DataTable) As List(Of Object)
        Dim objListOfEmployeeEntity As New List(Of Object)()
        For Each dRow As DataRow In dsProducts.Rows
            Dim hashtable As New Hashtable()
            For Each column As DataColumn In dsProducts.Columns
                If column.DataType = GetType(Integer) Then
                    hashtable.Add(column.ColumnName, Integer.Parse(dRow(column.ColumnName).ToString()))
                Else
                    hashtable.Add(column.ColumnName, dRow(column.ColumnName).ToString())
                End If
            Next
            objListOfEmployeeEntity.Add(hashtable)
        Next
        Return objListOfEmployeeEntity

    End Function

    Public Class JqGridData
        Private m_total As Integer
        Private m_page As Integer
        Private m_records As Integer
        Private m_rows As List(Of Object)
        Private m_rowshead As List(Of String)

        Public Property total() As Integer
            Get
                Return m_total
            End Get
            Set
                m_total = Value
            End Set
        End Property

        Public Property page() As Integer
            Get
                Return m_page
            End Get
            Set
                m_page = Value
            End Set
        End Property

        Public Property records() As Integer
            Get
                Return m_records
            End Get
            Set
                m_records = Value
            End Set
        End Property

        Public Property rows() As List(Of Object)
            Get
                Return m_rows
            End Get
            Set
                m_rows = Value
            End Set
        End Property

        Public Property rowsHead() As List(Of String)
            Get
                Return m_rowshead
            End Get
            Set
                m_rowshead = Value
            End Set
        End Property


        Public Property rowsM() As List(Of Object)
            Get
                Return m_rowsM
            End Get
            Set
                m_rowsM = Value
            End Set
        End Property

        Private m_rowsM As List(Of Object)
    End Class

    Public Function GetJson(ByVal dt As System.Data.DataTable) As String

        Try
            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim rows As New List(Of Dictionary(Of String, Object))()
            Dim row As Dictionary(Of String, Object) = Nothing
            For Each dr As DataRow In dt.Rows
                row = New Dictionary(Of String, Object)()
                For Each dc As DataColumn In dt.Columns

                    row.Add(dc.ColumnName.Trim(), dr(dc))

                Next
                rows.Add(row)
            Next
            serializer.MaxJsonLength = 2147483644
            Return serializer.Serialize(rows)

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try

        Return False

    End Function


    Public Shared Sub FillDataTable(ByRef DataTable As System.Data.DataTable, ByVal dtoriginal As System.Data.DataTable)




        'Dim dt As DataTable = nothing


        If DataTable Is Nothing Then
            DataTable = New System.Data.DataTable
        End If



        '-- Create the schema
        Dim col As DataColumn
        For i As Integer = 0 To dtoriginal.Columns.Count - 1
            col = New DataColumn
            col.ColumnName = dtoriginal.Columns(i).Caption
            If dtoriginal.Columns(i).DataType Is GetType(MySql.Data.Types.MySqlDateTime) Or dtoriginal.Columns(i).DataType Is GetType(System.Int32) Then
                col.DataType = GetType(System.String)
            Else
                col.DataType = dtoriginal.Columns(i).DataType
            End If
            DataTable.Columns.Add(col)
        Next


        '-- Populate the datatable
        Dim row As DataRow
        For Each dr In dtoriginal.Rows
            row = DataTable.NewRow
            For Each c As DataColumn In DataTable.Columns
                Dim colName As String = c.ColumnName

                If (colName = "duration") Then
                    Dim utility As New SharedRoutines
                    If Not dr.Item(colName) = " 0" Then
                        row.Item(c) = utility.ConvertSecToDate(dr.Item(colName))
                    End If
                Else

                    row.Item(c) = dr.Item(colName)
                End If
            Next
            DataTable.Rows.Add(row)
        Next




    End Sub

    Public Shared Sub LogWriteApi(s As String)
        Try
            Using stre As New StreamWriter(IO.Path.Combine(HttpContext.Current.Server.MapPath("log"), "logapi.txt"), True)
                stre.WriteLine(Now & ": " & s & vbCrLf)
            End Using
        Catch ex As Exception
        End Try

    End Sub
    Public Shared Sub LogWrite(s As String)
        Try
            Using stre As New StreamWriter(IO.Path.Combine(HttpContext.Current.Server.MapPath("log"), "log.txt"), True)
                stre.WriteLine(Now & ": " & s & vbCrLf)
            End Using
        Catch ex As Exception
        End Try

    End Sub

    Function findusername(ByVal username, ByVal nome, ByVal cognome, ByVal cn)


        Dim sqlstring As String = String.Empty

        sqlstring = "select userid from core_user where userid='" & username & "' "

        Dim tmpusername As String = username

        Dim i As Integer = 65
        While (cn.GetDataTable(sqlstring).Rows.Count >= 1 And i >= 65 And i <= 90)
            tmpusername = LCase(username) & LCase(Chr(i))
            i = i + 1
            sqlstring = "select userid from core_user where userid='" & tmpusername & "' "
        End While
        username = tmpusername
        Return username
        Return False

    End Function

    Public Function InvioMail(ByVal a As String, ByVal da As String, ByVal bcc As String, ByVal subject As String, ByVal body As String, ByVal file As String) As String



        Dim str As String = String.Empty



        Try

            omsg = New System.Net.Mail.MailMessage()
            Dim sp As String() = Split(a, ";")

            For Each s As String In sp
                If s <> "" Then
                    omsg.To.Add(New MailAddress(s))
                Else
                    omsg.To.Add(New MailAddress(a))
                End If
            Next



            omsg.From = New MailAddress(da)
            omsg.Subject = subject
            omsg.IsBodyHtml = True
            omsg.Body = body


            omsg.ReplyTo = New System.Net.Mail.MailAddress(da)

            omsg.Sender = New System.Net.Mail.MailAddress(da)



            Dim sbcc As String() = Split(bcc, ";")

            For Each sc As String In sbcc
                If sc <> "" Then
                    If UCase(Trim(a)) <> UCase(Trim(sc)) Then
                        Try
                            omsg.CC.Add(New MailAddress(Trim(Replace(Replace(sc, ";", ""), "_", ""))))
                        Catch ex As Exception
                        End Try

                    End If
                End If
            Next


            omsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure

            If boolpriority Then
                omsg.Priority = MailPriority.High
            Else
                omsg.Priority = MailPriority.Normal
            End If

            If boolread Then omsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            Dim aConfig As System.Configuration.ConfigurationSettings


            server = System.Configuration.ConfigurationSettings.AppSettings("Smtp.Server")

            username = System.Configuration.ConfigurationSettings.AppSettings("Smtp.Username")

            password = System.Configuration.ConfigurationSettings.AppSettings("Smtp.Password")


            omsg.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8")
            Dim plainView As System.Net.Mail.AlternateView
            plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(body, "<(.|\n)*?>", String.Empty), Nothing, "text/plain")
            Dim htmlView As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, Nothing, "text/html")

            omsg.AlternateViews.Add(plainView)
            omsg.AlternateViews.Add(htmlView)



            Select Case System.Configuration.ConfigurationSettings.AppSettings("smtpmethod")
                Case "sendblue"
                    username = "supporto@rbconsulenza.com"
                    password = "Yz4ANJGc1IvXm9Qw"
                    server = "smtp-relay.sendinblue.com"
                    Dim basicauthenticationinfo As _
                        New System.Net.NetworkCredential(username, password)
                    oSmtp = New SmtpClient(server)
                    oSmtp.UseDefaultCredentials = False
                    oSmtp.Credentials = basicauthenticationinfo
                    oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network
                    oSmtp.Timeout = "4000"
                    oSmtp.Port = 587
                Case "mailjet"
                    username = System.Configuration.ConfigurationSettings.AppSettings("usermailjet")

                    password = System.Configuration.ConfigurationSettings.AppSettings("passmailjet")

                    server = System.Configuration.ConfigurationSettings.AppSettings("smtpmailjet")
                    Dim basicauthenticationinfo As _
                        New System.Net.NetworkCredential(username, password)
                    oSmtp = New SmtpClient(server)
                    oSmtp.UseDefaultCredentials = False
                    oSmtp.Credentials = basicauthenticationinfo
                    oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network
                    oSmtp.Timeout = "4000"
                    oSmtp.Port = 465
                    oSmtp.EnableSsl = True
                Case "other"
                    username = System.Configuration.ConfigurationSettings.AppSettings("Smtp.Username")

                    password = System.Configuration.ConfigurationSettings.AppSettings("Smtp.Password")

                    server = System.Configuration.ConfigurationSettings.AppSettings("Smtp.Server")
                    Dim basicauthenticationinfo As _
                                New System.Net.NetworkCredential(username, password)
                    oSmtp = New SmtpClient(server)
                    oSmtp.UseDefaultCredentials = False
                    oSmtp.Credentials = basicauthenticationinfo
                    oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network
                    oSmtp.Timeout = "4000"
                    oSmtp.Port = 25
            End Select

            AddHandler oSmtp.SendCompleted, AddressOf SendCompletedCallback
            Dim fileadd() As String = file.Split(";")
            For Each fi In fileadd
                If IO.File.Exists(fi) Then
                    Dim oAttch As System.Net.Mail.Attachment = New System.Net.Mail.Attachment(fi, System.Net.Mime.MediaTypeNames.Application.Pdf)
                    omsg.Attachments.Add(oAttch)
                End If

            Next

            Try
                oSmtp.Send(omsg)

                Return " Email Inviata " & a & vbCrLf
            Catch ex As InvalidOperationException
                Return " Non è stato specificato il nome Host del server " & a & vbCrLf
            Catch ex As SmtpFailedRecipientException
                Return " Tentativo di invio al server locale, ma non è presente una mailbox " & a & vbCrLf
            Catch ex As SmtpException
                Return " Utente non valido/Host non trovato/Altro errore in fase di invio " & a & vbCrLf
            Catch ex As Exception
                Return EscapeMySql(ex.ToString & a)
            End Try
        Catch ex As System.Exception


            Return EscapeMySql(ex.Message)
        End Try
        Return False

    End Function
    Public Sub SendCompletedCallback(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
        ' Get the unique identifier for this asynchronous operation.
        Dim token As String = DirectCast(e.UserState, String)

        If e.Cancelled Then
            Console.WriteLine(String.Format("[{0}] Invio e-mail annullato.", token))
        End If
        If e.[Error] IsNot Nothing Then
            Console.WriteLine(String.Format("[{0}] {1}", token, e.[Error].ToString()))
        Else

            Console.WriteLine("e-mail inviata con successo.")
            oSmtp = Nothing
            omsg = Nothing
        End If
    End Sub







    Public Shared Function GetLastOpenSaveFile(extention As String) As String

        Dim regKey As RegistryKey = Registry.CurrentUser
        Dim lastUsedFolder As String = String.Empty


        regKey = regKey.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\OpenSavePidlMRU")

        If String.IsNullOrEmpty(extention) Then
            extention = "html"
        End If

        Dim myKey As RegistryKey = regKey.OpenSubKey(extention)

        If myKey Is Nothing AndAlso regKey.GetSubKeyNames().Length > 0 Then
            myKey = regKey.OpenSubKey(regKey.GetSubKeyNames()(regKey.GetSubKeyNames().Length - 2))
        End If

        If myKey IsNot Nothing Then
            Dim names As String() = myKey.GetValueNames()
            If names IsNot Nothing AndAlso names.Length > 0 Then
                lastUsedFolder = DirectCast(myKey.GetValue(names(names.Length - 2)), String)
            End If
        End If

        Return lastUsedFolder
        Return False

    End Function

    Function SendBenvenuto(ByRef esito As String, iduser As Integer, idcourse As Integer)

        sqlstring = "select * from (core_user a join learning_courseuser b on a.idst=b.iduser) join  learning_course c on c.idcourse=b.idcourse where idst=" & iduser & " and c.idcourse=" & idcourse

        Dim dr As DataRow = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)
        Dim nominativo As String = dr("firstname") & " " & dr("lastname")
        Dim nomesito As String = HttpContext.Current.Request.Url.AbsolutePath
        Dim codfis As String = dr("cf")
        Dim corso As String = dr("name")

        Dim email As String = dr("email")


        Dim finale As String = LetturaCodiceFiscale(codfis)


        subject = "Benvenuto " & nominativo




        body = "<div style='text-align:justify'><span style='font-family:Arial;font-size:12pt;color:#00314C'>Ciao <b>" & Trim(nominativo) & "</b>,<br>" &
    "benvenuto al corso e-learning <b>""" & corso & """</b>. <br>" &
"<span style='font-family:Times New Roman;font-size:12pt;font-style:italic;color:#00314C' >Cordiali saluti, <br>" &
                 "<i>Segreteria Didattica<br> <br><b>Training school</b><br>"



        esito = InvioMail(email, mailpiattaforma, "", subject, body, f)






        Return False

    End Function





#End Region


#Region "Funzioni per Criptare/Decriptare le immagini"

    Private Structure SHFILEINFO
        Public hIcon As IntPtr ' : icon
        Public iIcon As Integer ' : icondex
        Public dwAttributes As Integer ' : SFGAO_ flags
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
        Public szDisplayName As String

        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)>
        Public szTypeName As String

    End Structure

    Private Declare Ansi Function SHGetFileInfo Lib "shell32.dll" (ByVal pszPath As String,
    ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, ByVal cbFileInfo As Integer,
    ByVal uFlags As Integer) As IntPtr

    Private Const SHGFI_ICON = &H100
    Private Const SHGFI_SMALLICON = &H1
    Private Const SHGFI_LARGEICON = &H0         ' Large icon



    Private Const alg As Crypto.Algorithm = Crypto.Algorithm.Rijndael
    Private Const key As String = "LMSSw la talpa è dura e non ci fa paura"
    Private Const enc As Crypto.EncodingType = Crypto.EncodingType.BASE_64
    Public Property StatusObj As Object

    Public Shared Function DecriptFileStream(ByVal FileStream() As Byte) As Byte()

        Try
            Dim Ret() As Byte = Nothing

            If Not FileStream Is Nothing Then
                Crypto.EncryptionAlgorithm = alg
                Crypto.Key = key
                Crypto.Encoding = enc

                Dim b() As Byte = Crypto.DecryptFile(FileStream)

                Dim z As Integer = 0
                While b Is Nothing
                    z += 1
                    If z <= 10 Then
                        b = Crypto.DecryptFile(FileStream)
                    Else
                        z = -1
                        Exit While
                    End If

                    'l.deleo se non riesce dopo la 3° volta metto uno sleep di 1 secondo!
                    If z > 3 Then
                        System.Threading.Thread.Sleep(200)

                    End If
                End While

                If z > 0 Then

                ElseIf z = -1 Then

                End If

                Crypto.Clear()
                Ret = b
            End If

            Return Ret
        Catch ex As System.Exception

            Return Nothing
        End Try
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function


    Public Shared Function EncryptFileStream(ByVal FileStream() As Byte) As Byte()
        Try

            If Not FileStream Is Nothing Then

                Dim i As Integer = 0
                Dim bRet() As Byte = Nothing

                While bRet Is Nothing And i < 3

                    Crypto.EncryptionAlgorithm = alg
                    Crypto.Key = key
                    Crypto.Encoding = enc

                    bRet = Crypto.EncryptFile(FileStream)

                    Dim bdec() As Byte = Crypto.DecryptFile(bRet)

                    If Not bdec Is FileStream Then
                        bRet = Nothing
                    End If

                    i += 1
                End While

                If bRet Is Nothing Then
                    Return Nothing
                End If

                Return bRet
            Else
                Return Nothing
            End If

        Catch ex As System.Exception

            Return Nothing
        Finally
            Crypto.Clear()
        End Try
        ' Return False 


    End Function
#End Region
    Private Class JqGridDataHeading
        Public Sub New()
        End Sub

        Public Property index As String
        Public Property name As String
    End Class



End Class









Public Class Crypto

#Region "Class Variables"
    Public Enum KeySize As Integer
        RC2 = 64
        DES = 64
        TripleDES = 192
        AES = 128
        RSA = 2048
    End Enum

    ''' <summary>
    ''' Enum per gli algoritmi di HASH 
    ''' </summary>
    ''' <remarks></remarks>
    Enum HashMethod
        MD5
        SHA1
        SHA384
    End Enum

    Public Enum Algorithm As Integer
        SHA1 = 0
        SHA256 = 1
        SHA384 = 2
        Rijndael = 3
        TripleDES = 4
        RSA = 5
        RC2 = 6
        DES = 7
        'DSA = 8
        MD5 = 9
        RNG = 10
        'Base64 = 11
        SHA512 = 12
    End Enum

    Public Enum EncodingType As Integer
        HEX = 0
        BASE_64 = 1
    End Enum

    'Initialization Vectors that we will use for symmetric encryption/decryption. These
    'byte arrays are completely arbitrary, and you can change them to whatever you like.
    Private Shared IV_8 As Byte() = New Byte() {2, 63, 9, 36, 235, 174, 78, 12}
    Private Shared IV_16 As Byte() = New Byte() {15, 199, 56, 77, 244, 126, 107, 239,
    9, 10, 88, 72, 24, 202, 31, 108}
    Private Shared IV_24 As Byte() = New Byte() {37, 28, 19, 44, 25, 170, 122, 25,
    25, 57, 127, 5, 22, 1, 66, 65,
    14, 155, 224, 64, 9, 77, 18, 251}
    Private Shared IV_32 As Byte() = New Byte() {133, 206, 56, 64, 110, 158, 132, 22,
    99, 190, 35, 129, 101, 49, 204, 248,
    251, 243, 13, 194, 160, 195, 89, 152,
    149, 227, 245, 5, 218, 86, 161, 124}

    'Salt value used to encrypt a plain text key. Again, this can be whatever you like
    Private Shared SALT_BYTES As Byte() = New Byte() {162, 27, 98, 1, 28, 239, 64, 30, 156, 102, 223}

    'File names to be used for public and private keys
    Private Const KEY_PUBLIC As String = "public.key"
    Private Const KEY_PRIVATE As String = "private.key"

    'Values used for RSA-based asymmetric encryption
    Private Const RSA_BLOCKSIZE As Integer = 58
    Private Const RSA_DECRYPTBLOCKSIZE As Integer = 128

    'Error messages
    Private Const ERR_NO_KEY As String = "No encryption key was provided"
    Private Const ERR_NO_ALGORITHM As String = "No algorithm was specified"
    Private Const ERR_NO_CONTENT As String = "No content was provided"
    Private Const ERR_INVALID_PROVIDER As String = "An invalid cryptographic provider was specified for this method"
    Private Const ERR_NO_FILE As String = "The specified file does not exist"
    Private Const ERR_INVALID_FILENAME As String = "The specified filename is invalid"
    Private Const ERR_FILE_WRITE As String = "Could not create file"
    Private Const ERR_FILE_READ As String = "Could not read file"

    'Initialization variables
    Private Shared _key As String = String.Empty

    Private Shared _algorithm As Algorithm = Nothing
    Private Shared _content As String = String.Empty

    Private Shared _exception As CryptographicException
    Private Shared _encodingType As EncodingType = EncodingType.HEX
#End Region

#Region "Public Functions"
    <Description("The key that is used to encrypt and decrypt data")>
    Public Shared Property Key() As String

        Get
            Return _key
        End Get
        Set(ByVal value As String)
            _key = value
        End Set
    End Property

    <Description("The algorithm that will be used for encryption and decryption")>
    Public Shared Property EncryptionAlgorithm() As Algorithm
        Get
            Return _algorithm
        End Get
        Set(ByVal value As Algorithm)
            _algorithm = value
        End Set
    End Property

    <Description("The format in which content is returned after encryption, or provided for decryption")>
    Public Shared Property Encoding() As EncodingType
        Get
            Return _encodingType
        End Get
        Set(ByVal value As EncodingType)
            _encodingType = value
        End Set
    End Property

    <Description("Encrypted content to be retrieved after an encryption call, or provided for a decryption call")>
    Public Shared Property Content() As String

        Get
            Return _content
        End Get
        Set(ByVal Value As String)
            _content = Value
        End Set
    End Property

    <Description("If an encryption or decryption call returns false, then this will contain the exception")>
    Public Shared ReadOnly Property CryptoException() As CryptographicException
        Get
            Return _exception
        End Get
    End Property

    <Description("Determines whether the currently specified algorithm is a hash")>
    Public Shared ReadOnly Property IsHashAlgorithm() As Boolean
        Get
            Select Case _algorithm
                Case Algorithm.MD5, Algorithm.SHA1, Algorithm.SHA256, Algorithm.SHA384, Algorithm.SHA512
                    Return True
                Case Else
                    Return False
            End Select
        End Get
    End Property

    <Description("Encryption of a string using the 'Key' and 'EncryptionAlgorithm' properties")>
    Public Shared Function EncryptString(ByVal Content As String) As Boolean
        Dim cipherBytes() As Byte

        Try
            cipherBytes = _Encrypt(Content)
        Catch ex As CryptographicException
            _exception = New CryptographicException(ex.Message, ex.InnerException)
            Return False
        End Try

        If _encodingType = EncodingType.HEX Then
            _content = BytesToHex(cipherBytes)
        Else
            _content = System.Convert.ToBase64String(cipherBytes)
        End If

        Return True
        Return False

    End Function

    Public Shared Function DecryptString() As Boolean
        Dim encText As Byte() = Nothing
        Dim clearText As Byte() = Nothing

        Try
            clearText = _Decrypt(_content)
        Catch ex As Exception
            _exception = New CryptographicException(ex.Message, ex.InnerException)
            Return False
        End Try

        _content = UTF8.GetString(clearText)

        Return True
        Return False

    End Function

    Public Shared Function EncryptFile(ByVal FileStream As Byte()) As Byte()
        If FileStream Is Nothing Then
            _exception = New CryptographicException(ERR_NO_FILE)
            Return Nothing
        End If

        ''Make sure the target file can be written
        'Try
        ' Dim fs As FileStream = File.Create(Target)
        ' fs.Close()
        ' fs.Dispose()
        ' File.Delete(Target)
        'Catch ex As Exception
        ' _exception = New CryptographicException(ERR_FILE_WRITE)
        ' Return False
        'End Try

        Dim cipherBytes() As Byte

        Try
            cipherBytes = _Encrypt(FileStream)
        Catch ex As CryptographicException
            _exception = ex
            Return Nothing
        End Try

        Dim encodedString As String = String.Empty



        If _encodingType = EncodingType.BASE_64 Then
            encodedString = System.Convert.ToBase64String(cipherBytes)
        Else
            encodedString = BytesToHex(cipherBytes)
        End If

        Dim encodedBytes() As Byte = UTF8.GetBytes(encodedString)

        ''Create the encrypted file
        'Dim outStream As FileStream = File.Create(Target)

        'outStream.Write(encodedBytes, 0, encodedBytes.Length)
        'outStream.Close()
        'outStream.Dispose()

        Return encodedBytes
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Public Shared Function EncryptFile(ByVal Filename As String) As Byte()
        If Not File.Exists(Filename) Then
            _exception = New CryptographicException(ERR_NO_FILE)
            Return Nothing
        End If

        ''Make sure the target file can be written
        'Try
        ' Dim fs As FileStream = File.Create(Target)
        ' fs.Close()
        ' fs.Dispose()
        ' File.Delete(Target)
        'Catch ex As Exception
        ' _exception = New CryptographicException(ERR_FILE_WRITE)
        ' Return False
        'End Try

        Dim inStream() As Byte
        Dim cipherBytes() As Byte

        Try
            inStream = File.ReadAllBytes(Filename)
        Catch ex As Exception
            _exception = New CryptographicException(ERR_FILE_READ)
            Return Nothing
        End Try

        Try
            cipherBytes = _Encrypt(inStream)
        Catch ex As CryptographicException
            _exception = ex
            Return Nothing
        End Try

        Dim encodedString As String = String.Empty



        If _encodingType = EncodingType.BASE_64 Then
            encodedString = System.Convert.ToBase64String(cipherBytes)
        Else
            encodedString = BytesToHex(cipherBytes)
        End If

        Dim encodedBytes() As Byte = UTF8.GetBytes(encodedString)

        ''Create the encrypted file
        'Dim outStream As FileStream = File.Create(Target)

        'outStream.Write(encodedBytes, 0, encodedBytes.Length)
        'outStream.Close()
        'outStream.Dispose()

        Return encodedBytes
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Public Shared Function DecryptFile(ByVal Filename As String) As Byte()
        If Not File.Exists(Filename) Then
            _exception = New CryptographicException(ERR_NO_FILE)
            Throw New Exception(ERR_NO_KEY)
            Return Nothing
        End If

        ''Make sure the target file can be written
        'Try
        ' Dim fs As FileStream = File.Create(Target)
        ' fs.Close()
        ' fs.Dispose()
        ' File.Delete(Target)
        'Catch ex As Exception
        ' _exception = New CryptographicException(ERR_FILE_WRITE)
        ' Return False
        'End Try

        Dim inStream() As Byte
        Dim clearBytes() As Byte

        Try
            inStream = File.ReadAllBytes(Filename)
        Catch ex As Exception
            _exception = New CryptographicException(ERR_FILE_READ)
            Return Nothing
        End Try

        Try
            clearBytes = _Decrypt(inStream)
        Catch ex As Exception
            _exception = New CryptographicException(ex.Message, ex.InnerException)
            Return Nothing
        End Try

        ''Create the decrypted file
        'Dim outStream As FileStream = File.Create(Target)
        'outStream.Write(clearBytes, 0, clearBytes.Length)
        'outStream.Close()
        'outStream.Dispose()

        Return clearBytes
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Public Shared Function DecryptFile(ByVal FileStream() As Byte) As Byte()
        If FileStream Is Nothing Then
            _exception = New CryptographicException(ERR_NO_FILE)
            Throw New Exception(ERR_NO_KEY)
            Return Nothing
        End If

        ''Make sure the target file can be written
        'Try
        ' Dim fs As FileStream = File.Create(Target)
        ' fs.Close()
        ' fs.Dispose()
        ' File.Delete(Target)
        'Catch ex As Exception
        ' _exception = New CryptographicException(ERR_FILE_WRITE)
        ' Return False
        'End Try

        Dim inStream() As Byte = FileStream
        Dim clearBytes() As Byte

        'Try
        '    inStream = File.ReadAllBytes(Filename)
        'Catch ex As Exception
        '    _exception = New CryptographicException(ERR_FILE_READ)
        '    Return Nothing
        'End Try

        Try
            clearBytes = _Decrypt(inStream)
        Catch ex As Exception
            _exception = New CryptographicException(ex.Message, ex.InnerException)
            Return Nothing
        End Try

        ''Create the decrypted file
        'Dim outStream As FileStream = File.Create(Target)
        'outStream.Write(clearBytes, 0, clearBytes.Length)
        'outStream.Close()
        'outStream.Dispose()

        Return clearBytes
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Public Shared Function GenerateHash(ByVal Content As String) As Boolean
        If Content Is Nothing OrElse Content.Equals(String.Empty) Then

            _exception = New CryptographicException(ERR_NO_CONTENT)
            Return False
        End If

        If _algorithm.Equals(-1) Then
            _exception = New CryptographicException(ERR_NO_ALGORITHM)
            Return False
        End If

        Dim hashAlgorithm As HashAlgorithm = Nothing

        Select Case _algorithm
            Case Algorithm.SHA1
                hashAlgorithm = New SHA1CryptoServiceProvider
            Case Algorithm.SHA256
                hashAlgorithm = New SHA256Managed
            Case Algorithm.SHA384
                hashAlgorithm = New SHA384Managed
            Case Algorithm.SHA512
                hashAlgorithm = New SHA512Managed
            Case Algorithm.MD5
                hashAlgorithm = New MD5CryptoServiceProvider
            Case Else
                _exception = New CryptographicException(ERR_INVALID_PROVIDER)
        End Select

        Try
            Dim hash() As Byte = ComputeHash(hashAlgorithm, Content)
            If _encodingType = EncodingType.HEX Then
                _content = BytesToHex(hash)
            Else
                _content = System.Convert.ToBase64String(hash)
            End If
            hashAlgorithm.Clear()
            Return True
        Catch ex As CryptographicException
            _exception = ex
            Return False
        Finally
            hashAlgorithm.Clear()
        End Try
        Return False

    End Function

    ''' <summary>
    ''' Calcola l'HASH, mediante un algoritmo a scelta tra MD5, SHA1 e SHA384, di una stringa
    ''' </summary>
    ''' <param name="source">Stringa di cui calcolare l'HASH</param>
    ''' <param name="algorithm">Algoritmo di HASH (MD5, SHA1 e SHA384)</param>
    ''' <returns>HASH di tipo "algorithm" in formato stringa; stringa vuota se si verifica un errore</returns>
    ''' <remarks></remarks>
    Public Shared Function GenerateHashDigest(ByVal source As String, ByVal algorithm As HashMethod) As String


        Dim hashAlgorithm As HashAlgorithm = Nothing

        Try

            Select Case algorithm
                Case HashMethod.MD5
                    hashAlgorithm = New MD5CryptoServiceProvider
                Case HashMethod.SHA1
                    hashAlgorithm = New SHA1CryptoServiceProvider
                Case HashMethod.SHA384
                    hashAlgorithm = New SHA384Managed
            End Select

            Dim byteValue() As Byte = UTF8.GetBytes(source)
            Dim hashValue() As Byte = hashAlgorithm.ComputeHash(byteValue)

            Return System.Convert.ToBase64String(hashValue)

        Catch ex As System.Exception
            Return ""
        End Try

        Return False

    End Function

    Public Shared Sub Clear()
        _algorithm = Nothing
        _content = String.Empty


        _key = String.Empty


        _encodingType = EncodingType.HEX
        _exception = Nothing
    End Sub

#End Region

#Region "Shared Cryptographic Functions"
    Private Shared Function _Encrypt(ByVal Content As Byte()) As Byte()
        If Not IsHashAlgorithm AndAlso _key Is Nothing Then
            Throw New CryptographicException(ERR_NO_KEY)
        End If

        If _algorithm.Equals(-1) Then
            Throw New CryptographicException(ERR_NO_ALGORITHM)
        End If

        If Content Is Nothing OrElse Content.Equals(String.Empty) Then
            Throw New CryptographicException(ERR_NO_CONTENT)
        End If


        Dim cipherBytes() As Byte = Nothing
        Dim NumBytes As Integer = 0

        If _algorithm = Algorithm.RSA Then
            'This is an asymmetric call, which has to be treated differently
            Try
                cipherBytes = RSAEncrypt(Content)
            Catch ex As CryptographicException
                Throw ex
            End Try
        Else
            Dim provider As SymmetricAlgorithm

            Select Case _algorithm
                Case Algorithm.DES
                    provider = New DESCryptoServiceProvider
                    NumBytes = KeySize.DES
                Case Algorithm.TripleDES
                    provider = New TripleDESCryptoServiceProvider
                    NumBytes = KeySize.TripleDES
                Case Algorithm.Rijndael
                    provider = New RijndaelManaged
                    NumBytes = KeySize.AES
                Case Algorithm.RC2
                    provider = New RC2CryptoServiceProvider
                    NumBytes = KeySize.RC2
                Case Else
                    Throw New CryptographicException(ERR_INVALID_PROVIDER)
            End Select

            Try
                'Encrypt the string
                cipherBytes = SymmetricEncrypt(provider, Content, _key, NumBytes)
            Catch ex As CryptographicException
                Throw New CryptographicException(ex.Message, ex.InnerException)
            Finally
                'Free any resources held by the SymmetricAlgorithm provider
                provider.Clear()
            End Try
        End If

        Return cipherBytes
        ' Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Private Shared Function _Encrypt(ByVal Content As String) As Byte()
        Return _Encrypt(UTF8.GetBytes(Content))
        ' Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Private Shared Function _Decrypt(ByVal Content As Byte()) As Byte()
        If Not IsHashAlgorithm AndAlso _key Is Nothing Then
            Throw New CryptographicException(ERR_NO_KEY)
        End If

        If _algorithm.Equals(-1) Then
            Throw New CryptographicException(ERR_NO_ALGORITHM)
        End If

        If Content Is Nothing OrElse Content.Length.Equals(0) Then
            Throw New CryptographicException(ERR_NO_CONTENT)
        End If

        Dim encText As String = UTF8.GetString(Content)

        If _encodingType = EncodingType.BASE_64 Then
            'We need to convert the content to Hex before decryption
            encText = BytesToHex(System.Convert.FromBase64String(encText))
        End If

        Dim clearBytes() As Byte = Nothing
        Dim NumBytes As Integer = 0

        If _algorithm = Algorithm.RSA Then
            Try
                clearBytes = RSADecrypt(encText)
            Catch ex As CryptographicException
                Throw ex
            End Try
        Else
            Dim provider As SymmetricAlgorithm

            Select Case _algorithm
                Case Algorithm.DES
                    provider = New DESCryptoServiceProvider
                    NumBytes = KeySize.DES
                Case Algorithm.TripleDES
                    provider = New TripleDESCryptoServiceProvider
                    NumBytes = KeySize.TripleDES
                Case Algorithm.Rijndael
                    provider = New RijndaelManaged
                    NumBytes = KeySize.AES
                Case Algorithm.RC2
                    provider = New RC2CryptoServiceProvider
                    NumBytes = KeySize.RC2
                Case Else
                    Throw New CryptographicException(ERR_INVALID_PROVIDER)
            End Select

            Try
                clearBytes = SymmetricDecrypt(provider, encText, _key, NumBytes)
            Catch ex As CryptographicException
                Throw ex
            Finally
                'Free any resources held by the SymmetricAlgorithm provider
                provider.Clear()
            End Try
        End If

        'Now return the plain text content
        Return clearBytes
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Private Shared Function _Decrypt(ByVal Content As String) As Byte()
        Return _Decrypt(UTF8.GetBytes(Content))
        ' Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Private Shared Function ComputeHash(ByVal Provider As HashAlgorithm, ByVal plainText As String) As Byte()
        'All hashing mechanisms inherit from the HashAlgorithm base class so we can use that to cast the crypto service provider
        Dim hash As Byte() = Provider.ComputeHash(UTF8.GetBytes(plainText))
        Provider.Clear()
        Return hash
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Private Shared Function SymmetricEncrypt(ByVal Provider As SymmetricAlgorithm, ByVal plainText As Byte(), ByVal key As String, ByVal keySize As Integer) As Byte()
        'All symmetric algorithms inherit from the SymmetricAlgorithm base class, to which we can cast from the original crypto service provider
        Dim ivBytes As Byte() = Nothing
        Select Case keySize / 8 'Determine which initialization vector to use
            Case 8
                ivBytes = IV_8
            Case 16
                ivBytes = IV_16
            Case 24
                ivBytes = IV_24
            Case 32
                ivBytes = IV_32
            Case Else
                'TODO: Throw an error because an invalid key length has been passed
        End Select

        Provider.KeySize = keySize

        'Generate a secure key based on the original password by using SALT
        'Dim keyStream As Byte() = DerivePassword(key, keySize / 8)
        Dim keyStream As Byte() = DerivePassword(key, CInt(keySize / 8))

        'Initialize our encryptor object
        Dim trans As ICryptoTransform = Provider.CreateEncryptor(keyStream, ivBytes)

        'Perform the encryption on the textStream byte array
        Dim result As Byte() = trans.TransformFinalBlock(plainText, 0, plainText.GetLength(0))

        'Release cryptographic resources
        Provider.Clear()
        trans.Dispose()

        Return result
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Private Shared Function SymmetricDecrypt(ByVal Provider As SymmetricAlgorithm, ByVal encText As String, ByVal key As String, ByVal keySize As Integer) As Byte()
        'All symmetric algorithms inherit from the SymmetricAlgorithm base class, to which we can cast from the original crypto service provider
        Dim ivBytes As Byte() = Nothing
        Select Case keySize / 8 'Determine which initialization vector to use
            Case 8
                ivBytes = IV_8
            Case 16
                ivBytes = IV_16
            Case 24
                ivBytes = IV_24
            Case 32
                ivBytes = IV_32
            Case Else
                'TODO: Throw an error because an invalid key length has been passed
        End Select

        'Generate a secure key based on the original password by using SALT
        'Dim keyStream As Byte() = DerivePassword(key, keySize / 8)
        Dim keyStream As Byte() = DerivePassword(key, CInt(keySize / 8))

        'Convert our hex-encoded cipher text to a byte array
        Dim textStream As Byte() = HexToBytes(encText)
        Provider.KeySize = keySize

        'Initialize our decryptor object
        Dim trans As ICryptoTransform = Provider.CreateDecryptor(keyStream, ivBytes)

        'Initialize the result stream
        Dim result() As Byte = Nothing

        Try
            'Perform the decryption on the textStream byte array
            result = trans.TransformFinalBlock(textStream, 0, textStream.GetLength(0))
        Catch ex As Exception
            Throw New System.Security.Cryptography.CryptographicException("The following exception occurred during decryption: " & ex.Message)
        Finally
            'Release cryptographic resources
            Provider.Clear()
            trans.Dispose()
        End Try

        Return result
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Private Shared Function RSAEncrypt(ByVal plainText As Byte()) As Byte()
        'Make sure that the public and private key exists
        ValidateRSAKeys()
        Dim publicKey As String = GetTextFromFile(KEY_PUBLIC)
        Dim privateKey As String = GetTextFromFile(KEY_PRIVATE)

        'The RSA algorithm works on individual blocks of unencoded bytes. In this case, the
        'maximum is 58 bytes. Therefore, we are required to break up the text into blocks and
        'encrypt each one individually. Each encrypted block will give us an output of 128 bytes.
        'If we do not break up the blocks in this manner, we will throw a "key not valid for use
        'in specified state" exception

        'Get the size of the final block
        Dim lastBlockLength As Integer = plainText.Length Mod RSA_BLOCKSIZE
        Dim blockCount As Integer = Math.Floor(plainText.Length / RSA_BLOCKSIZE)
        Dim hasLastBlock As Boolean = False
        If Not lastBlockLength.Equals(0) Then
            'We need to create a final block for the remaining characters
            blockCount += 1
            hasLastBlock = True
        End If

        'Initialize the result buffer
        Dim result() As Byte = New Byte() {}

        'Initialize the RSA Service Provider with the public key
        Dim Provider As New RSACryptoServiceProvider(KeySize.RSA)
        Provider.FromXmlString(publicKey)

        'Break the text into blocks and work on each block individually
        For blockIndex As Integer = 0 To blockCount - 1
            Dim thisBlockLength As Integer

            'If this is the last block and we have a remainder, then set the length accordingly
            If blockCount.Equals(blockIndex + 1) AndAlso hasLastBlock Then
                thisBlockLength = lastBlockLength
            Else
                thisBlockLength = RSA_BLOCKSIZE
            End If
            Dim startChar As Integer = blockIndex * RSA_BLOCKSIZE

            'Define the block that we will be working on
            Dim currentBlock(thisBlockLength - 1) As Byte
            Array.Copy(plainText, startChar, currentBlock, 0, thisBlockLength)

            'Encrypt the current block and append it to the result stream
            Dim encryptedBlock() As Byte = Provider.Encrypt(currentBlock, False)
            Dim originalResultLength As Integer = result.Length
            Array.Resize(result, originalResultLength + encryptedBlock.Length)
            encryptedBlock.CopyTo(result, originalResultLength)
        Next

        'Release any resources held by the RSA Service Provider
        Provider.Clear()

        Return result
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    Private Shared Function RSADecrypt(ByVal encText As String) As Byte()
        'Make sure that the public and private key exists
        ValidateRSAKeys()
        Dim publicKey As String = GetTextFromFile(KEY_PUBLIC)
        Dim privateKey As String = GetTextFromFile(KEY_PRIVATE)

        'When we encrypt a string using RSA, it works on individual blocks of up to
        '58 bytes. Each block generates an output of 128 encrypted bytes. Therefore, to
        'decrypt the message, we need to break the encrypted stream into individual
        'chunks of 128 bytes and decrypt them individually

        'Determine how many bytes are in the encrypted stream. The input is in hex format,
        'so we have to divide it by 2
        'Dim maxBytes As Integer = encText.Length / 2
        Dim maxBytes As Integer = CInt(encText.Length / 2)

        'Ensure that the length of the encrypted stream is divisible by 128
        If Not (maxBytes Mod RSA_DECRYPTBLOCKSIZE).Equals(0) Then
            Throw New System.Security.Cryptography.CryptographicException("Encrypted text is an invalid length")
            Return Nothing
        End If

        'Calculate the number of blocks we will have to work on
        'Dim blockCount As Integer = maxBytes / RSA_DECRYPTBLOCKSIZE
        Dim blockCount As Integer = CInt(maxBytes / RSA_DECRYPTBLOCKSIZE)

        'Initialize the result buffer
        Dim result() As Byte = New Byte() {}

        'Initialize the RSA Service Provider
        Dim Provider As New RSACryptoServiceProvider(KeySize.RSA)
        Provider.FromXmlString(privateKey)

        'Iterate through each block and decrypt it
        For blockIndex As Integer = 0 To blockCount - 1
            'Get the current block to work on
            'Dim currentBlockHex = encText.Substring(blockIndex * (RSA_DECRYPTBLOCKSIZE * 2), RSA_DECRYPTBLOCKSIZE * 2)
            Dim currentBlockHex As String = encText.Substring(blockIndex * (RSA_DECRYPTBLOCKSIZE * 2), RSA_DECRYPTBLOCKSIZE * 2)
            Dim currentBlockBytes As Byte() = HexToBytes(currentBlockHex)

            'Decrypt the current block and append it to the result stream
            Dim currentBlockDecrypted() As Byte = Provider.Decrypt(currentBlockBytes, False)
            Dim originalResultLength As Integer = result.Length
            Array.Resize(result, originalResultLength + currentBlockDecrypted.Length)
            currentBlockDecrypted.CopyTo(result, originalResultLength)
        Next

        'Release all resources held by the RSA service provider
        Provider.Clear()

        Return result
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#End Region

#Region "Utility Functions"
    '********************************************************
    '* BytesToHex: Converts a byte array to a hex-encoded
    '* string
    '********************************************************
    Private Shared Function BytesToHex(ByVal bytes() As Byte) As String

        Dim hex As New StringBuilder
        For n As Integer = 0 To bytes.Length - 1
            hex.AppendFormat("{0:X2}", bytes(n))
        Next
        Return hex.ToString
        Return False

    End Function

    '********************************************************
    '* HexToBytes: Converts a hex-encoded string to a
    '* byte array
    '********************************************************
    Private Shared Function HexToBytes(ByVal Hex As String) As Byte()
        'Dim numBytes As Integer = Hex.Length / 2
        Dim numBytes As Integer = CInt(Hex.Length / 2)
        Dim bytes(numBytes - 1) As Byte
        For n As Integer = 0 To numBytes - 1
            Dim hexByte As String = Hex.Substring(n * 2, 2)
            bytes(n) = Integer.Parse(hexByte, Globalization.NumberStyles.HexNumber)
        Next
        Return bytes
        ' Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    '********************************************************
    '* ClearBuffer: Clears a byte array to ensure
    '* that it cannot be read from memory
    '********************************************************
    Private Shared Sub ClearBuffer(ByVal bytes() As Byte)
        If bytes Is Nothing Then Exit Sub
        For n As Integer = 0 To bytes.Length - 1
            bytes(n) = 0
        Next
    End Sub

    '********************************************************
    '* GenerateSalt: No, this is not a culinary routine. This
    '* generates a random salt value for
    '* password generation
    '********************************************************
    Private Shared Function GenerateSalt(ByVal saltLength As Integer) As Byte()
        Dim salt() As Byte
        If saltLength > 0 Then
            salt = New Byte(saltLength) {}
        Else
            salt = New Byte(0) {}
        End If

        Dim seed As New RNGCryptoServiceProvider
        seed.GetBytes(salt)
        Return salt
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    '********************************************************
    '* DerivePassword: This takes the original plain text key
    '* and creates a secure key using SALT
    '********************************************************
    Private Shared Function DerivePassword(ByVal originalPassword As String, ByVal passwordLength As Integer) As Byte()
        Dim derivedBytes As New Rfc2898DeriveBytes(originalPassword, SALT_BYTES, 5)
        Return derivedBytes.GetBytes(passwordLength)
        ' Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'Byte()'. 

    '********************************************************
    '* ValidateRSAKeys: Checks for the existence of a public
    '* and private key file and creates them
    '* if they do not exist
    '********************************************************
    Private Shared Sub ValidateRSAKeys()
        If Not File.Exists(KEY_PRIVATE) OrElse Not File.Exists(KEY_PUBLIC) Then
            'Dim rsa As New RSACryptoServiceProvider
            Dim key As RSA = RSA.Create
            key.KeySize = KeySize.RSA
            Dim privateKey As String = key.ToXmlString(True)
            Dim publicKey As String = key.ToXmlString(False)
            Dim privateFile As StreamWriter = File.CreateText(KEY_PRIVATE)
            privateFile.Write(privateKey)
            privateFile.Close()
            privateFile.Dispose()
            Dim publicFile As StreamWriter = File.CreateText(KEY_PUBLIC)
            publicFile.Write(publicKey)
            publicFile.Close()
            publicFile.Dispose()
        End If
    End Sub

    '********************************************************
    '* GetTextFromFile: Reads the text from a file
    '********************************************************
    Private Shared Function GetTextFromFile(ByVal fileName As String) As String

        If File.Exists(fileName) Then
            Dim textFile As StreamReader = File.OpenText(fileName)
            Dim result As String = textFile.ReadToEnd
            textFile.Close()
            textFile.Dispose()
            Return result
        Else
            Throw New IOException("Specified file does not exist")
            Return Nothing
        End If
        Return False

    End Function
#End Region

    Public Sub New()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
