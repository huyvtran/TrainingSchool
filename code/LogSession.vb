
Imports TrainingSchool.SharedRoutines



Public Class LogSession



    Dim utility As SharedRoutines
    Dim sqlstring As String = String.Empty

    Dim strHostName As String = String.Empty

    Dim clientIPAddress As String = String.Empty

    Dim rconn As rconnection
    Public Sub New()
        Try
            utility = New SharedRoutines

            clientIPAddress = GetIPAddress()

            rconn = CheckDatabase(rconn)


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

    End Sub




    Public Shared Sub CheckUtente()
        If HttpContext.Current.Session("UtenteCollegato") Is Nothing Then
            HttpContext.Current.Session.RemoveAll()
            HttpContext.Current.Response.Redirect("wflogin.aspx?err=Sessione Scaduta")
        End If

    End Sub

#Region "log session"

    Public Function GetLastActivity(iduser As String)
        Dim content As String = String.Empty

        Try
            Dim sqlstring As String = "SELECT * FROM   learning_tracksession   where iduser=" & iduser & " order by enterTime desc"
            Dim dt As DataTable = Nothing

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            For Each dr In dt.Rows
                content &= vbCrLf & "	<div class=""profile-activity clearfix"">"
                content &= vbCrLf & "	<div>"
                If HttpContext.Current.Session("admin") Then
                    content &= vbCrLf & "		<img id=""avatar"" class=""pull-left"" alt=""Administrator"" src=""assets/images/avatars/user.jpg""></img>"
                Else
                    content &= vbCrLf & "		<img id=""avatar"" class=""pull-left"" alt=""Corsista"" src=""assets/images/avatars/avatar5.jpg""></img>"
                End If
                content &= vbCrLf & "		<a class=""user"" href=""#""> " & HttpContext.Current.Session("Fullname") & " </a>"

                content &= vbCrLf & dr("lastfunction").ToString & "->" & dr("lastOp").ToString & "->" & utility.getCourse(dr("idcourse"))
                content &= vbCrLf & "	<div class=""time"">"
                content &= vbCrLf & "		<i class=""icon-time bigger-110""></i>" & utility.ConvertSecToDate(DateDiff(DateInterval.Second, CDate(dr("enterTime").ToString), CDate(Now))) & " fa"

                content &= vbCrLf & "		</div>"
                content &= vbCrLf & "	</div>"


                content &= vbCrLf & "	</div>"
            Next
            Return content
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    Public Function GetTotAccess(iduser As String)

        Try
            Dim sqlstring As String = "SELECT count(*) as cntotal FROM   learning_tracksession   where iduser=" & iduser

            Dim countoobtotale As Integer = rconn.GetDataTable(sqlstring).Rows(0)("cntotal")


            Return countoobtotale
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    Public Function GetSingleStatUser(iduser As Integer, idCourse As String)


        Try

            Dim sqlstring As String = "SELECT count(*) as cntotal FROM  learning_organization  where idCourse=" & idCourse & " and objectType <> '' order by objectType"

            Dim countoobtotale As Integer = rconn.GetDataTable(sqlstring).Rows(0)("cntotal")


            sqlstring = "select count(*) as cn from learning_commontrack join  learning_organization  on  learning_organization .idOrg = learning_commontrack.idreference where idcourse=" & idCourse & " and  status != 'ab-initio' and   status != 'attempted' and  learning_commontrack.idUser=" & iduser


            'Dim dr As DataRow = dt.Rows(0)
            Dim countob As Integer = rconn.GetDataTable(sqlstring).Rows(0)("cn")
            Dim datastart As String = String.Empty

            Dim dataend As String = String.Empty

            Dim percentuale As String = String.Empty



            If countob > 0 Then
                percentuale = CInt((countob / countoobtotale) * 100)
            Else
                percentuale = 0
            End If


            Return percentuale & ";" & countob & ";" & countoobtotale


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    Sub SetLastAccess(iduser As String)
        Try
            sqlstring = "update  core_user  set lastenter='" & SharedRoutines.ConvertToMysqlDateTime(Now) & "' where idst=" & iduser

            rconn.Execute(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub

    Function StartCourselog()


        Try

            sqlstring = "Select UNIX_TIMESTAMP(MAX(lastTime))  as lastTime FROM  learning_tracksession " &
            "WHERE idCourse =" & HttpContext.Current.Session("idCourse") & " AND idUser = " & HttpContext.Current.Session("iduser")

            HttpContext.Current.Session("lastCourseAccess") = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("lastTime").ToString

            sqlstring = " UPDATE   learning_tracksession      set    Active = 0 		WHERE idUser = " & HttpContext.Current.Session("iduser") & " and active = 1"

            rconn.Execute(sqlstring, CommandType.Text, Nothing)

            sqlstring = "		INSERT INTO  learning_tracksession   " &
            "( idCourse, idUser, session_id, enterTime,lastTime, ip_address, active ) VALUES ( " &
           HttpContext.Current.Session("idCourse") & ", " &
           HttpContext.Current.Session("iduser") & ",'" &
           HttpContext.Current.Session.SessionID & "','" &
            SharedRoutines.ConvertToMysqlDateTime(Now) & "','" &
            SharedRoutines.ConvertToMysqlDateTime(Now) & "','" &
            clientIPAddress & "',1)"
            rconn.Execute(sqlstring, CommandType.Text, Nothing)
            HttpContext.Current.Session("id_enter_course") = rconn.GetDataTable("SELECT LAST_INSERT_ID() as id", CommandType.Text, Nothing).Rows(0)("id")



        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function SaveActionlog(id_user As Integer, id_course As Integer, id_enter As Integer, mod_name As String, mode As String)
        Try




            sqlstring = "UPDATE  learning_tracksession  SET numOp = numOp+1, " &
                "lastFunction = '" & mod_name & "', " &
                "lastOp = '" & mode & "', " &
                "lastTime = '" & SharedRoutines.ConvertToMysqlDateTime(Now) & "'," &
                "ip_address = '" & clientIPAddress & "'" &
                " WHERE idEnter = " & id_enter & " AND idCourse = " & id_course & " AND idUser = " & id_user


            rconn.Execute(sqlstring, CommandType.Text, Nothing)

            '	if(Get::sett('loging') == 'on'  &  &  httpContext.current.Session('levelCourse') <> '2') {

            sqlstring = "	INSERT INTO   learning_trackingeneral   ( idUser, idEnter, idCourse, function, type, timeof, session_id, ip ) VALUES ( " &
                id_user & ", " &
               id_enter & ", " &
                id_course & ",' " &
                mod_name & "',' " &
                mode & "', '" &
                SharedRoutines.ConvertToMysqlDateTime(Now) & "', (Select session_id from  learning_tracksession WHERE idEnter = " & id_enter & " AND idCourse = " & id_course & " AND idUser = " & id_user & ")   ,'" &
                clientIPAddress & "' )"

            rconn.Execute(sqlstring, CommandType.Text, Nothing)


        Catch ex As Exception
            SharedRoutines.LogWrite(sqlstring & "  " & ex.ToString)
        End Try
        Return False

    End Function

    Function EndCourselog(sessionid As String)

        Try
            SaveActionlog(HttpContext.Current.Session("idUser"), HttpContext.Current.Session("idCourse"), HttpContext.Current.Session("id_enter_course"), "courseexit", "list")
            HttpContext.Current.Session("id_enter_course") = Nothing
            Return True
        Catch ex As Exception
        End Try

        Return False

    End Function

    Function ExitCourselog(sessionid As String)
        Try
            If Not HttpContext.Current.Session("idCourse") Is Nothing Then
                SaveActionlog(HttpContext.Current.Session("idUser"), HttpContext.Current.Session("idCourse"), HttpContext.Current.Session("id_enter_course"), "logout", "logout")
            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function GetStartCourse(id_user As Integer, id_course As Integer)
        Try

            Dim tot_time As String = ""

            Dim sqlstring = "  select register_date from  learning_courseuser where idCourse=" & id_course & " and idUser=" & id_user


            Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("register_date").ToString()




        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function GetEndCourse(id_user As Integer, id_course As Integer)
        Try

            Dim tot_time As String = ""

            Dim sqlstring = "  select date_complete from  learning_courseuser where idCourse=" & id_course & " and idUser=" & id_user


            Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("date_complete").ToString()




        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function getUserTotalVideocourseSec(iduser As Integer, idcourse As Integer)

        Dim tot_timeScorm As String = String.Empty


        Dim dtreference As DataTable

        sqlstring = "select total_time from learning_scorm_tracking where idreference in (select idOrg from learning_organization where idCourse=" & idcourse & "  and objecttype='scormorg') and idUser=" & iduser & ""

        dtreference = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)



        Try

            Dim time_sec As Integer = 0
            Dim time_min As Integer = 0
            Dim time_hour As Integer = 0

            For Each dr1 In dtreference.Rows



                Dim time As String = dr1("total_time").ToString

                Dim second As String = String.Empty

                Dim minutes As String = String.Empty

                Dim hour As String = String.Empty



                If time.IndexOf(":") > -1 Then
                    second = time.Split(":")(2)
                    minutes = time.Split(":")(1)
                    hour = time.Split(":")(0)
                    time_sec += second
                    time_min += minutes
                    time_hour += hour
                Else
                    Try
                        second = time.Split("M")(1).ToString.Remove(2)
                        minutes = time.Split("M")(0).ToString.Substring(time.Split("M")(0).ToString.Count - 2, 2)
                        hour = time.Split("H")(0).ToString.Substring(time.Split("H")(0).ToString.Count - 2, 2)
                        time_sec += second
                        time_min += minutes
                        time_hour += hour
                    Catch ex As Exception
                    End Try

                End If




            Next

            time_sec += (time_min * 60) + (time_hour * 3600)


            Return time_sec
        Catch ex As Exception
        End Try



        Return False

    End Function
    Function getUserTotalVideocourse(iduser As Integer, idcourse As Integer)

        Dim tot_timeScorm As String = String.Empty


        Dim dtreference As DataTable

        sqlstring = "select total_time from learning_scorm_tracking where idreference in (select idOrg from learning_organization where idCourse=" & idcourse & "  and objecttype='scormorg') and idUser=" & iduser & ""

        dtreference = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        Try

            Dim time_sec As Integer = 0
            Dim time_min As Integer = 0
            Dim time_hour As Integer = 0

            For Each dr1 In dtreference.Rows



                Dim time As String = dr1("total_time").ToString

                Dim second As String = String.Empty

                Dim minutes As String = String.Empty

                Dim hour As String = String.Empty



                If time.IndexOf(":") > -1 Then
                    second = time.Split(":")(2)
                    minutes = time.Split(":")(1)
                    hour = time.Split(":")(0)
                    time_sec += second
                    time_min += minutes
                    time_hour += hour
                Else
                    Try
                        second = time.Split("M")(1).ToString.Remove(2)
                        minutes = time.Split("M")(0).ToString.Substring(time.Split("M")(0).ToString.Count - 2, 2)
                        hour = time.Split("H")(0).ToString.Substring(time.Split("H")(0).ToString.Count - 2, 2)
                        time_sec += second
                        time_min += minutes
                        time_hour += hour
                    Catch ex As Exception
                    End Try

                End If




            Next

            Dim sec As Integer = (time_sec / 60) Mod 60
            Dim min As Integer = time_min Mod 60
            Dim hou As Integer = time_hour + ((time_min + (time_sec / 60)) / 60)
            Return hou & "h " & min & "m " & sec & "s<br>"
        Catch ex As Exception
        End Try



        Return False

    End Function
    Function GetUserTotalCourseTime(idst_user, id_course)
        Try

            Dim tot_time As String = ""

            Dim sqlstring = "  select SUM(TIME_TO_SEC(TIMEDIFF(lastTime,enterTime))) as totaltime from   learning_tracksession   where idCourse=" & id_course & " and idUser=" & idst_user


            tot_time = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("totaltime").ToString

            Return tot_time


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function GetUserPreviousSessionCourseTime(idst_user, id_course)


        Dim tot_time = 0
        Try
            sqlstring = "Select SUM((UNIX_TIMESTAMP(lastTime) - UNIX_TIMESTAMP(enterTime))) " &
            " FROM   learning_tracksession    WHERE idCourse = " & id_course & " AND idUser = " & idst_user & " " &
             " AND idEnter <> " & HttpContext.Current.Session("id_enter_course") &
             tot_time = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("tottime").ToString

            Return tot_time

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function


    Function GetUserCurrentSessionCourseTime(id_course As String)
        Try
            If Not HttpContext.Current.Session("id_enter_course") Is Nothing Then
                Dim uTime As Integer
                uTime = (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds

                sqlstring = "   Select UNIX_TIMESTAMP(EenterTime) as etime    FROM   learning_tracksession   " &
            " WHERE idCourse = '" & id_course & "' AND idUser = " & HttpContext.Current.Session("iduser") &
                "AND idEnter = '" & HttpContext.Current.Session("id_enter_course") & "'"

                Return uTime - rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("etime").ToString
            Else
                Return False
            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function


    Function GetWhoIsOnline(id_course As Integer, Optional gap_minute As String = "5")
        Dim uTime As Integer

        Try
            uTime = (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds
            Dim gap_time = (uTime - (60 * gap_minute)).ToString("Y-m-d H:i:s")
            sqlstring = "		SELECT COUNT(DISTINCT idUser)    as countuser  FROM   learning_tracksession   WHERE idCourse = '" & id_course & "' AND active = 1 AND lastTime > '" & gap_time & "'"

            Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("countuser").ToString()


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    Function GetListWhoIsOnline(id_course As Integer, Optional gap_minute As String = "5")
        Dim uTime As Integer

        Try
            uTime = (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds
            Dim gap_time = gap_minute = (uTime - (60 * gap_minute)).ToString("Y-m-d H:i:s")

            sqlstring = "SELECT DISTINCT idUser         FROM   learning_tracksession   		WHERE idCourse = '" & id_course & "' AND active = 1 AND (lastTime) > '" & gap_time & "'"

            Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Function GetLastAccessToCourse(id_user) As DataTable

        'if(isset(httpContext.current.Session('is_ghost'))  &  &   httpContext.current.Session('is_ghost') === true) return  0;
        Try
            sqlstring = "		SELECT idCourse, UNIX_TIMESTAMP(MAX(lastTime)) 	FROM   learning_tracksession   		WHERE idUser = " & id_user & "		GROUP BY idCourse"

            Return rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        'Return False 

    End Function


    Public Function SavelogStatusChange(idUser As String, idCourse As String, status As String)

        Try

            sqlstring = "  Select status  FROM  learning_courseuser WHERE idUser = " & idUser & " AND idCourse = " & idCourse
            Dim prevstatus = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("status").ToString

            Dim extra = ""
            If Not prevstatus > status Then
                Select Case status
                    Case 0

                        extra = ", date_inscr = NOW()"

                    Case 1

                        extra = ", date_first_access = NOW()"

                    Case 2

                        extra = ", date_complete = NOW()"

                End Select

                sqlstring = "UPDATE learning_courseuser SET status = " & status & extra & " WHERE idUser = " & idUser & " AND idCourse = " & idCourse

            End If



            rconn.Execute(sqlstring, CommandType.Text, Nothing)

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return False

    End Function

    'nel caso si loggano due persone

    Function CheckSession(id_user)


        'if(isset(httpContext.current.Session('is_ghost'))  &  &   httpContext.current.Session('is_ghost') === true) return true;
        Try
            If Not HttpContext.Current.Session("id_enter_course") Is Nothing Then

                sqlstring = "SELECT COUNT(*)  as num_active  FROM   learning_tracksession   	WHERE idUser = '" & id_user & "' AND idEnter = '" & HttpContext.Current.Session("id_enter_course") & " AND active = 1"
                Dim num_active As Integer = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("num_active")

                Return (num_active = 1)
            Else
                Return True
            End If

        Catch ex As Exception

            SharedRoutines.LogWrite(ex.ToString)
        End Try
        Return False

    End Function

    Sub ResetUserSession(id_user)
        Try
            sqlstring = "UPDATE   learning_tracksession    set Active = 0	WHERE idUser = '" & id_user
            rconn.Execute(sqlstring, CommandType.Text, Nothing)
        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub
    Public Function Savetestlog(idtrack As String, idquest As String, idanswer As String, score As String)

        Try

            Dim sqlstring = "INSERT INTO  learning_testtrack_answer ( idtrack ,  idQuest ,  idAnswer ,  score_assigned ,  more_info ,  manual_assigned ,  user_answer ) " &
        "  VALUES (" & idtrack & ", " & idquest & ", " & idanswer & "," & score & " ,'', 0, 1);"

            rconn.Execute(sqlstring, CommandType.Text, Nothing)


        Catch ex As Exception
            If ex.Message.StartsWith("#23000Duplicate") Then
                sqlstring = "UPDATE  learning_testtrack_answer set score_assigned=" & score & " where idtrack=" & idtrack & " and idanswer=" & idanswer & " and idquest=" & idquest
                rconn.Execute(sqlstring, CommandType.Text, Nothing)
            End If


        End Try

        Return True


    End Function
    Public Function Savepolllog(idtrack As String, idquest As String, idanswer As String)

        Try

            Dim sqlstring = "INSERT INTO  learning_polltrack_answer  ( id_track ,  id_Quest ,  id_Answer ) " &
        "  VALUES (" & idtrack & ", " & idquest & ", " & idanswer & ");"

            rconn.Execute(sqlstring, CommandType.Text, Nothing)


        Catch ex As Exception
            If ex.Message.StartsWith("#23000Duplicate") Then
                '    sqlstring = "UPDATE  learning_polltrack_answer  set where idtrack=" & idtrack & " and idanswer=" & idanswer & " and idquest=" & idquest
                '    rconn.Execute(sqlstring, CommandType.Text, Nothing)
            End If
            Return False

            SharedRoutines.LogWrite(ex.ToString)
        End Try

        Return True
        Return False

    End Function

#End Region
End Class

