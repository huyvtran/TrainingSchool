Imports System.Data
Imports TrainingSchool.SharedRoutines
Imports System.IO
Imports ICSharpCode.SharpZipLib
Imports System.Xml

Public Class Scorm

    Public Property Coursepath As String

        Set(value As String)
            _coursepath = value
        End Set
        Get
            Return _coursepath
        End Get
    End Property
    Public Property ref As String

        Set(value As String)
            _ref = value
        End Set
        Get
            Return _ref
        End Get
    End Property
    Public Property title As String

        Set(value As String)
            _title = value
        End Set
        Get
            Return _title
        End Get
    End Property


    Public Property Fullname As String

        Set(value As String)
            _Fullname = value
        End Set
        Get
            Return _Fullname
        End Get
    End Property

    Public Property username As String

        Set(value As String)
            _username = value
        End Set
        Get
            Return _username
        End Get
    End Property

    Public Property CourseId As Integer
        Set(value As Integer)
            _idCourse = value
        End Set
        Get
            Return _idCourse
        End Get
    End Property

    Public Property UserID As Integer
        Set(value As Integer)
            _iduser = value
        End Set
        Get
            Return _iduser
        End Get
    End Property


    Public Property Reference As Integer
        Set(value As Integer)
            _idreference = value
        End Set
        Get
            Return _idreference
        End Get
    End Property


    Public Property IDEnter As Integer
        Set(value As Integer)
            _identer = value
        End Set
        Get
            Return _identer
        End Get
    End Property

    Dim _coursepath As String = String.Empty
    Dim _username As String = String.Empty
    Dim _Fullname As String = String.Empty
    Dim _iduser As Integer
    Dim _idCourse As Integer
    Dim _idreference As Integer
    Dim _identer As Integer
    Dim _title As String = String.Empty
    Dim _ref As String = String.Empty
    Dim sqlstring As String = String.Empty
    Dim utility As SharedRoutines
    Dim rconn As rconnection
    Dim scodata As UserSCODataInfo


    Function SaveSessionScorm(idtrack As String, sraw As String, smax As String, session_time As String, lesson_status As String)

        If (lesson_status = "") Then
            Return True
        End If

        If sraw = "" Then
            sraw = "NULL"
        End If

        If (smax = "") Then
            smax = "NULL"
        End If

        If (sraw <> "") Then
            sraw = sraw.Replace(",", ".")
        End If

        Dim sql As String = "INSERT INTO  learning_scorm_tracking_history " & " (id learning_scorm_tracking, date_action, sraw, smax, session_time, lesson_status) " &
" VALUES " &
" (idtrack, " & ConvertToMysqlDateTime(Now()) & ", " & sraw & ", " & smax & ", " & session_time & ", '" & lesson_status & "')"

        Return rconn.Execute(sql, CommandType.Text, Nothing)

        Return False

    End Function

    Public Function GetSCO(ByVal IDUser As Integer, ByVal identifier As String) As UserSCODataInfo

        Dim dr As IDataReader = Nothing
        rconn = CheckDatabase(rconn)

        sqlstring = "select a.*,d.controlvideo from  (learning_scorm_tracking a  join learning_organization c on a.idreference=c.idorg ) join learning_course d on d.idcourse=c.idCourse  where a.iduser=" & IDUser & " and idreference=" & identifier


        Try
            dr = rconn.GetDataReader(sqlstring, CommandType.Text, Nothing)
            dr.Read()
            scodata.myval = 0
            scodata.UserID = dr("Iduser").ToString
            scodata.LessonLocation = dr("lesson_location").ToString
            scodata.LessonStatus = dr("lesson_status").ToString
            scodata.Identifier = dr("idreference").ToString
            scodata.SessionTime = dr("session_time").ToString
            scodata.TotalTime = dr("total_time").ToString
            scodata.SCOExit = dr("exit").ToString
            scodata.SCOEntry = dr("entry").ToString
            scodata.SCOID = dr("idscorm_item").ToString
            scodata.SuspendData = dr("suspend_data").ToString
            scodata.RawScore = dr("score_raw").ToString
            scodata.Credit = dr("credit").ToString
            scodata.idtrack = dr("idscorm_tracking").ToString
            Try
                scodata.myval = dr("controlvideo").ToString
            Catch ex As Exception
            End Try
            dr.Close()
            dr.Dispose()


            Return scodata

        Catch ex As Exception
            LogWrite(ex.ToString)
            dr.Close()
            dr.Dispose()
            Return Nothing
        End Try



    End Function

    Public Function UpdateUserSCOData(SCOID As Integer, UserID As Integer, idreference As String, LessonStatus As String, LessonLocation As String, Credit As String, SCOExit As String, SCOEntry As String, RawScore As String, SuspendData As String, TotalTime As String, SessionTime As String, idCourse As String, idtrack As Integer)



        utility.Update_commonlog(idreference, UserID, idCourse, LessonStatus, idtrack)


        Try
            If SessionTime <> "" Then
                sqlstring = "Update   learning_scorm_tracking  set  lesson_location ='" & LessonLocation & "',  credit ='" & Credit & "',  lesson_status ='" & LessonStatus & "',  entry ='" & SCOEntry & "',  score_raw ='" & RawScore & "',  score_max =NULL,  score_min =NULL,  lesson_mode ='normal',  `exit` ='" & SCOExit & "',  session_time ='" & SessionTime & "',   total_time ='" & TotalTime & "', suspend_data ='" & SuspendData & "' where   idUser =" & UserID & " and   idreference =" & idreference & " and  idscorm_item =" & SCOID
            Else
                sqlstring = "Update   learning_scorm_tracking  set  lesson_location ='" & LessonLocation & "',  credit ='" & Credit & "',  lesson_status ='" & LessonStatus & "',  entry ='" & SCOEntry & "',  score_raw ='" & RawScore & "',  score_max =NULL,  score_min =NULL,  lesson_mode ='normal',  `exit` ='" & SCOExit & "',  suspend_data ='" & SuspendData & "' where   idUser =" & UserID & " and   idreference =" & idreference & " and  idscorm_item =" & SCOID

            End If

            rconn.Execute(sqlstring, CommandType.Text, Nothing)


            SharedRoutines.LogWrite(Now & " IDcourse: " & idCourse & " - IDUser: " & UserID & " - SessionTime:" & SessionTime & " - Totaltime:" & TotalTime)



        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString & sqlstring)
        End Try


        Return False

    End Function

    Function getrefmanifest(path As String)
        Dim xmldoc As New XmlDataDocument()
        Dim xmlnode As XmlNodeList
        Dim i As Integer
        Dim str As String
        Dim fs As New FileStream(HttpContext.Current.Server.MapPath(path) & "/imsmanifest.xml", FileMode.Open, FileAccess.Read)
        xmldoc.Load(fs)
        xmlnode = xmldoc.GetElementsByTagName("resources")
        Return xmlnode(0).ChildNodes.Item(0).FirstChild.Attributes(0).Value
    End Function

    Public Sub GetScormByReference(idreference As Integer, idcourse As Integer)
        Try

            Dim dr As DataRow
            Dim h As Hashtable


            sqlstring = "  select    learning_scorm_package.path, learning_organization.title from   ( learning_scorm_package   join learning_scorm_organizations   on  learning_scorm_organizations.idscorm_package= learning_scorm_package.idscorm_package )  join learning_organization on learning_organization.idresource=learning_scorm_organizations.idscorm_organization where idOrg=" & idreference & " and  objecttype='scormorg' and  idcourse=" & idcourse

            dr = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)
            _coursepath = HttpContext.Current.Session("affiliate") & "/Content/scorm/" & dr("path") & "/"
            _ref = getrefmanifest(_coursepath)
            _username = HttpContext.Current.Session("UserID")
            _Fullname = HttpContext.Current.Session("FullName")
            _idCourse = HttpContext.Current.Session("idCourse")
            _iduser = HttpContext.Current.Session("idUser")
            _idreference = idreference

            h = utility.GetObjDetails(_idCourse)

            HttpContext.Current.Session("reference") = _idreference
            HttpContext.Current.Session("resource") = h(_idreference.ToString).ToString.Split(";")(1)
            HttpContext.Current.Session("title") = h(_idreference.ToString).ToString.Split(";")(0)
            _title = h(_idreference.ToString).ToString.Split(";")(0)
            HttpContext.Current.Session("objecttype") = "scormorg"


            utility.log_Obj("scormorg", HttpContext.Current.Session("iduser"), _idCourse, HttpContext.Current.Session("reference"), HttpContext.Current.Session("resource"))


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub

    Public Sub GetScormByID(id As Integer)
        Try

            Dim dr As DataRow


            sqlstring = "select   learning_scorm_package .* from    learning_scorm_package     where  idscorm_package=" & id

            dr = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)
            _coursepath = "Content/scorm/" & dr("path") & "/"

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub

    Public Function CreateScormEmpty(title As String, nvideo As Integer) As String

        Try

            Dim path As String = HttpContext.Current.Session("lmscontentpath") & "/Content/Scorm"
            Dim n As Long = DateDiff(DateInterval.Second, CDate("1/01/1900"), Now)
            Dim folderscorm As String = HttpContext.Current.Session("iduser") & "_" & n & "_" & Trim(title)
            Dim pathfinal As String = String.Empty


            folderscorm = utility.EscapePath(folderscorm)
            pathfinal = (IO.Path.Combine(path, folderscorm))

            Dim f As DirectoryInfo = IO.Directory.CreateDirectory(pathfinal)

            If Not f Is Nothing Then

                sqlstring = " INSERT INTO    learning_scorm_package  (  idPackage ,   path ,   idUser ,  scormVersion ) " &
               " VALUES ('a" & Year(Now) & "', '" & EscapeMySql(folderscorm) & "',  " & HttpContext.Current.Session("iduser") & ", '1.2');"
                rconn.Execute(sqlstring, CommandType.Text, Nothing)


                sqlstring = " INSERT INTO `learning_scorm_organizations` ( `org_identifier`, `idscorm_package`, `title`, `nChild`, `nDescendant`)  " &
                        "  VALUES ('a001-org', (select max(idscorm_package) from learning_scorm_package), '" & EscapeMySql(title) & "', 1, 1);"

                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  `r_item_id`, `r_type`) " &
                   " VALUES ( '" & EscapeMySql(title) & "', '" & EscapeMySql(title) & "',(select max(idscorm_organization) from learning_scorm_organizations) , 'scormorg');"

                rconn.Execute(sqlstring, CommandType.Text, Nothing)

            End If





        Catch ex As Exception
            Return "Errore inserimento Scorm: " & ex.Message

            LogWrite(ex.ToString)

        End Try

        Return "Oggetto Scorm Aggiornato"



        Return False

    End Function

    Public Function EditScorm(ByVal title As String, ByVal id As String) As String

        Try

            Dim path As String = HttpContext.Current.Session("lmscontentpath") & "/Content/Scorm"
            Dim folderscorm As String = String.Empty


            If id <> 0 Then

                sqlstring = "update learning_kb_res set r_name='" & EscapeMySql(title) & "' where r_item_id=" & id
                rconn.Execute(sqlstring, CommandType.Text, Nothing)

                'sqlstring = "Update  learning_scorm_package set title='" & title & "' where idscorm_package=" & id
                'rconn.Execute(sqlstring, CommandType.Text, Nothing)
            End If

            'sqlstring = "select path from   learning_scorm_package where idscorm_package=" & id
            'folderscorm = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing).Rows(0)("path")

            'Dim pathfinal As String = (IO.Path.Combine(path, folderscorm))

            'CreaScorm12(pathfinal)


            Return "Oggetto Scorm Modificato"

        Catch ex As Exception

            LogWrite(ex.ToString)
            Return ex.Message

        End Try


        Return False

    End Function

    Public Function CreateScormZipContent(id As String)

        Try

            Dim iduser As Integer = HttpContext.Current.Session("iduser")
            Dim path As String = HttpContext.Current.Session("lmscontentpath") & "/Content/Scorm"
            Dim title As String = EscapeMySql(HttpContext.Current.Request.Form("title"))
            Dim n As Long = DateDiff(DateInterval.Second, CDate("1/01/1900"), Now)
            Dim folderscorm As String = HttpContext.Current.Session("iduser") & "_" & n & "_" & title
            Dim pathfinal As String = String.Empty


            folderscorm = utility.EscapePath(folderscorm)

            pathfinal = (IO.Path.Combine(path, folderscorm))


            sqlstring = " INSERT INTO    learning_scorm_package  (  idPackage ,   path ,   idUser ,  scormVersion ) " &
               " VALUES ('a" & Year(Now) & "', '" & EscapeMySql(folderscorm) & "',  " & HttpContext.Current.Session("iduser") & ", '1.2');"
            rconn.Execute(sqlstring, CommandType.Text, Nothing)


            sqlstring = " INSERT INTO `learning_scorm_organizations` ( `org_identifier`, `idscorm_package`, `title`, `nChild`, `nDescendant`)  " &
                        "  VALUES ('a001-org', (select max(idscorm_package) from learning_scorm_package), '" & EscapeMySql(title) & "', 1, 1);"

            rconn.Execute(sqlstring, CommandType.Text, Nothing)


            Dim res_idanno As Integer
            Dim pathitem As String
            utility.makecategory(res_idanno, pathitem)

            sqlstring = " INSERT INTO `learning_kb_res` ( `r_name`, `original_name`,  `r_item_id`, `r_type`,path,lev,idparent, iduser) " &
               " VALUES ( '" & EscapeMySql(title) & "', '" & EscapeMySql(title) & "',(select max(idscorm_organization) from learning_scorm_organizations) , 'scormorg','" & pathitem & "',3," & res_idanno & "," & iduser & ");"


            rconn.Execute(sqlstring, CommandType.Text, Nothing)


            Return folderscorm

        Catch ex As Exception

            LogWrite(ex.ToString)
            Return ""

        End Try




        Return False

    End Function



    Public Sub CreaScorm13(path As String)

        Dim streamred As String = String.Empty

        Dim destination As System.IO.DirectoryInfo
        Dim source As System.IO.DirectoryInfo

        If (path <> "") Then


            Dim f As System.IO.DirectoryInfo() = New System.IO.DirectoryInfo(path).GetDirectories()
            Dim i As Integer = 0

            Do While (i < f.Length)
                destination = f(i)
                source = New System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(("SCORM1.3/filecopy/")))
                Try
                    Me.CopyAll(source, destination)

                Catch ex As Exception
                End Try

                Dim l As List(Of String) = New List(Of String)()
                l = FileHelper.GetFilesRecursive(destination.FullName, "mp4")

                i = 1
                Do While (i <= l.Count)
                    If (i = l.Count) Then
                        Using s As New System.IO.StreamWriter(destination.FullName & "/a001_ud_1_argomento_1_section_1_" & i & ".html")

                            Using r As New System.IO.StreamReader("SCORM1.3/master.html")

                                streamred = r.ReadToEnd()

                            End Using

                            streamred = Strings.Replace(streamred, "<<number>>", i)
                            streamred = Strings.Replace(streamred, "<<numberprev>>", i - 1)
                            streamred = Strings.Replace(streamred, "<<numbernext>>", "a001_ud_1_argomento_1_section_1_fine_modulo")
                            streamred = Strings.Replace(streamred, "<<totalpage>>", l.Count + 1)
                            streamred = Strings.Replace(streamred, ",1,78,0,0,0,", ",1," & l.Count + 3 & ",0,0,0,")
                            streamred = Strings.Replace(streamred, "actionSetPos(78)", "actionSetPos(" & i + 2 & ")")
                            s.Write(streamred)
                        End Using

                        Using s As New System.IO.StreamWriter(destination.FullName & "/a001_ud_1_argomento_1_section_1_fine_modulo.html")

                            Using r As New System.IO.StreamReader("SCORM1.3/masterfinemodulo.html")


                                streamred = r.ReadToEnd()

                            End Using
                            streamred = Strings.Replace(streamred, "<<number>>", l.Count + 1)
                            streamred = Strings.Replace(streamred, "<<totalpage>>", l.Count + 1)
                            streamred = Strings.Replace(streamred, "<<numberprev>>", i - 1)
                            streamred = Strings.Replace(streamred, ",1,78,0,0,0,", ",1," & l.Count + 3 & ",0,0,0,")
                            streamred = Strings.Replace(streamred, "actionSetPos(78)", "actionSetPos(" & i + 2 & ")")

                        End Using
                    Else





                        Using s As New System.IO.StreamWriter(destination.FullName & "/a001_ud_1_argomento_1_section_1_" & i & ".html")

                            Using r As New System.IO.StreamReader("SCORM1.3/master.html")

                                streamred = r.ReadToEnd()
                            End Using

                            streamred = Strings.Replace(streamred, "<<number>>", i)
                            If (i = True) Then
                                streamred = Strings.Replace(streamred, "<<numberprev>>", i)
                            Else
                                streamred = Strings.Replace(streamred, "<<numberprev>>", i - 1)
                            End If
                            streamred = Strings.Replace(streamred, "<<numbernext>>", "a001_ud_1_argomento_1_section_1_" & i + 1)
                            streamred = Strings.Replace(streamred, "<<totalpage>>", l.Count + 1)
                            streamred = Strings.Replace(streamred, ",1,78,0,0,0,", ",1," & l.Count + 3 & ",0,0,0,")
                            streamred = Strings.Replace(streamred, "actionSetPos(78)", "actionSetPos(" & i + 2 & ")")
                            s.Write(streamred)

                        End Using


                    End If
                    i = (i + 1)

                Loop
                i = (i + 1)

            Loop
        End If


    End Sub

    Public Sub CreaScormsingle12(path As String)
        Try

            Dim streamred As String = String.Empty

            Dim destination As New System.IO.DirectoryInfo(path)
            Dim source As System.IO.DirectoryInfo
            Dim i As Integer = 1
            Dim l As List(Of String)
            source = New System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(("LMSContent/ScormCreate/SCORM1.2/filecopy/")))
            Try
                Me.CopyAll(source, destination)

            Catch ex As Exception
            End Try



            i = 1



            Using r As New System.IO.StreamReader(HttpContext.Current.Server.MapPath("LMSContent/ScormCreate/SCORM1.2/master.html"))

                streamred = r.ReadToEnd()


            End Using
            streamred = Strings.Replace(streamred, "<<number>>", i)
            streamred = Strings.Replace(streamred, "<<tagvideo>>", "media/slide1.mp4")

            streamred = Strings.Replace(streamred, "<<numberprev>>", i)
            streamred = Strings.Replace(streamred, "<<numbernext>>", "a001_ud_1_argomento_1_section_1_fine_modulo")
            streamred = Strings.Replace(streamred, "<<totalpage>>", i + 1)
            streamred = Strings.Replace(streamred, ",1,78,0,0,0,", ",1,3,0,0,0,")
            streamred = Strings.Replace(streamred, "actionSetPos(3)", "actionSetPos(" & i + 2 & ")")


            Using s As New System.IO.StreamWriter(destination.FullName & "/a001_ud_1_argomento_1_section_1_" & i & ".html")
                s.Write(streamred)


            End Using



            Using r As New System.IO.StreamReader(HttpContext.Current.Server.MapPath("LMSContent/ScormCreate/SCORM1.2/masterfinemodulo.html"))

                streamred = r.ReadToEnd()

            End Using
            streamred = Strings.Replace(streamred, "<<number>>", i)
            streamred = Strings.Replace(streamred, "<<tagvideo>>", "media/slide1.mp4")


            streamred = Strings.Replace(streamred, "<<totalpage>>", 2)
            streamred = Strings.Replace(streamred, "<<numberprev>>", i)
            streamred = Strings.Replace(streamred, ",1,78,0,0,0,", ",1," & 3 & ",0,0,0,")
            streamred = Strings.Replace(streamred, "actionSetPos(78)", "actionSetPos(" & i + 2 & ")")


            Using s As New System.IO.StreamWriter(destination.FullName & "/a001_ud_1_argomento_1_section_1_fine_modulo.html")
                s.Write(streamred)
            End Using

        Catch ex As System.IO.IOException
            SharedRoutines.LogWrite(ex.ToString)
        End Try

    End Sub
    Public Sub CreaScorm12(path As String)
        Try

            Dim streamred As String = String.Empty

            Dim destination As New System.IO.DirectoryInfo(path)
            Dim source As System.IO.DirectoryInfo
            Dim i As Integer = 1
            Dim l As List(Of String)
            source = New System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(("LMSContent/ScormCreate/SCORM1.2/filecopy/")))
            Try
                Me.CopyAll(source, destination)

            Catch ex As Exception
            End Try

            l = FileHelper.GetFilesRecursive(destination.FullName, "mp4")


            i = 1
            Do While (i <= l.Count)
                If (i = l.Count) Then
                    Using s As New System.IO.StreamWriter(destination.FullName & "/a001_ud_1_argomento_1_section_1_" & i & ".html")

                        Using r As New System.IO.StreamReader(HttpContext.Current.Server.MapPath("LMSContent/ScormCreate/SCORM1.2/master.html"))

                            streamred = r.ReadToEnd()


                        End Using

                        streamred = Strings.Replace(streamred, "<<tagvideo>>", "media/slide" & l.Count + 1 & ".mp4")

                        streamred = Strings.Replace(streamred, "<<numberprev>>", i - 1)
                        streamred = Strings.Replace(streamred, "<<numbernext>>", "a001_ud_1_argomento_1_section_1_fine_modulo")
                        streamred = Strings.Replace(streamred, "<<totalpage>>", l.Count + 1)
                        streamred = Strings.Replace(streamred, ",1,78,0,0,0,", ",1," & l.Count + 3 & ",0,0,0,")
                        streamred = Strings.Replace(streamred, "actionSetPos(3)", "actionSetPos(" & i + 2 & ")")
                        s.Write(streamred)


                    End Using

                    Using s As New System.IO.StreamWriter(destination.FullName & "/a001_ud_1_argomento_1_section_1_fine_modulo.html")

                        Using r As New System.IO.StreamReader(HttpContext.Current.Server.MapPath("LMSContent/ScormCreate/SCORM1.2/masterfinemodulo.html"))

                            streamred = r.ReadToEnd()

                        End Using

                        streamred = Strings.Replace(streamred, "<<tagvideo>>", "media/slide" & l.Count + 1 & ".mp4")


                        streamred = Strings.Replace(streamred, "<<totalpage>>", l.Count + 1)
                        streamred = Strings.Replace(streamred, "<<numberprev>>", i - 1)
                        streamred = Strings.Replace(streamred, ",1,78,0,0,0,", ",1," & l.Count + 3 & ",0,0,0,")
                        streamred = Strings.Replace(streamred, "actionSetPos(78)", "actionSetPos(" & i + 2 & ")")

                        s.Write(streamred)
                    End Using

                Else
                    Using s As New System.IO.StreamWriter(destination.FullName & "/a001_ud_1_argomento_1_section_1_" & i & ".html")

                        Using r As New System.IO.StreamReader(HttpContext.Current.Server.MapPath("LMSContent/ScormCreate/SCORM1.2//master.html"))

                            streamred = r.ReadToEnd()

                        End Using

                        streamred = Strings.Replace(streamred, "<<number>>", i)

                        If (i = 1) Then
                            streamred = Strings.Replace(streamred, "<<numberprev>>", i)
                        Else
                            streamred = Strings.Replace(streamred, "<<numberprev>>", i - 1)
                        End If

                        streamred = Strings.Replace(streamred, "<<numbernext>>", "a001_ud_1_argomento_1_section_1_" & i + 1)
                        streamred = Strings.Replace(streamred, "<<totalpage>>", l.Count + 1)
                        streamred = Strings.Replace(streamred, ",1,78,0,0,0,", ",1," & l.Count + 3 & ",0,0,0,")
                        streamred = Strings.Replace(streamred, "actionSetPos(3)", "actionSetPos(" & i + 2 & ")")
                        s.Write(streamred)

                    End Using

                End If
                i = i + 1

            Loop
        Catch ex As System.IO.IOException
            SharedRoutines.LogWrite(ex.ToString)
        End Try

    End Sub

    Public Sub CreaScorm12youtube(path As String, addressyoutube As String)
        Try

            Dim streamred As String = String.Empty

            Dim destination As New System.IO.DirectoryInfo(path)
            Dim source As System.IO.DirectoryInfo
            Dim i As Integer = 1
            Dim l As List(Of String)
            source = New System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath(("LMSContent/ScormCreate/SCORMYOUTUBE1.2/filecopy/")))
            Try
                Me.CopyAll(source, destination)

            Catch ex As Exception
            End Try




            i = 1


            Using r As New System.IO.StreamReader(HttpContext.Current.Server.MapPath("LMSContent/ScormCreate/SCORMYOUTUBE1.2/master.html"))

                streamred = r.ReadToEnd()


            End Using

            If addressyoutube.IndexOf("embed") > -1 Then
            Else
                addressyoutube = Replace(addressyoutube, "watch?v=", "embed/")
            End If

            streamred = Strings.Replace(streamred, "<<number>>", i)
            streamred = Strings.Replace(streamred, "<<tagvideo>>", addressyoutube)

            streamred = Strings.Replace(streamred, "<<numberprev>>", i)
            streamred = Strings.Replace(streamred, "<<numbernext>>", i)
            streamred = Strings.Replace(streamred, "<<totalpage>>", 2)
            streamred = Strings.Replace(streamred, ",1,78,0,0,0,", ",1," & i & ",0,0,0,")
            streamred = Strings.Replace(streamred, "actionSetPos(3)", "actionSetPos(" & i & ")")



            Using s As New System.IO.StreamWriter(destination.FullName & "\a001_ud_1_argomento_1_section_1_1.html")
                s.Write(streamred)
            End Using




        Catch ex As System.IO.IOException
            SharedRoutines.LogWrite(ex.ToString)
        End Try

    End Sub
    Public Sub CopyFile(ByVal source As System.IO.FileInfo, ByVal target As System.IO.DirectoryInfo)
        Dim fi As System.IO.FileInfo = source
        fi.CopyTo(System.IO.Path.Combine(target.FullName, fi.Name), True)

    End Sub


    Public Sub CopyAll(ByVal source As System.IO.DirectoryInfo, ByVal target As System.IO.DirectoryInfo)
        Try
            If (Not System.IO.Directory.Exists(target.FullName)) Then
                System.IO.Directory.CreateDirectory(target.FullName)
            End If
            Dim f As System.IO.FileInfo() = source.GetFiles()
            Dim i As Integer = 0
            Do While (i < f.Length())
                Dim fi As System.IO.FileInfo = f(i)
                fi.CopyTo(System.IO.Path.Combine(target.FullName, fi.Name), True)
                i = (i + 1)

            Loop
            Dim d As System.IO.DirectoryInfo() = source.GetDirectories()
            i = 0
            Do While (i < d.Length())
                Dim diSourceDir As System.IO.DirectoryInfo = d(i)
                Dim nextTargetDir As System.IO.DirectoryInfo = target.CreateSubdirectory(diSourceDir.Name)
                Me.CopyAll(diSourceDir, nextTargetDir)
                i = (i + 1)

            Loop

        Catch ie As System.IO.IOException
        End Try
    End Sub







    ''' <summary>
    ''' This class contains directory helper method(s).
    ''' </summary>
    Public Class FileHelper

        ''' <summary>
        ''' This method starts at the specified directory, and traverses all subdirectories.
        ''' It returns a List of those directories.
        ''' </summary>
        Public Shared Function GetFilesRecursive(ByVal initial As String, filter As String) As List(Of String)
            ' This list stores the results.
            Dim result As New List(Of String)

            ' This stack stores the directories to process.
            Dim stack As New Stack(Of String)

            ' Add the initial directory
            stack.Push(initial)

            ' Continue processing for each stacked directory
            Do While (stack.Count > 0)
                ' Get top directory string
                Dim dir As String = stack.Pop
                Try
                    ' Add all immediate file paths
                    result.AddRange(Directory.GetFiles(dir, "*." & filter & ""))

                    ' Loop through all subdirectories and add them to the stack.
                    Dim directoryName As String = String.Empty

                    For Each directoryName In Directory.GetDirectories(dir)
                        stack.Push(directoryName)
                    Next

                Catch ex As Exception
                End Try
            Loop

            ' Return the list
            Return result
            '  Return False 

        End Function


    End Class


    Public Sub New()
        scodata = New UserSCODataInfo
        utility = New SharedRoutines
        rconn = CheckDatabase(rconn)
    End Sub
End Class
