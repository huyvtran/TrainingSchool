Imports System
Imports System.Web
Imports System.Web.UI.ClientScriptManager
Public Class HUpload
    Implements IHttpHandler, IReadOnlySessionState

    Dim utility As SharedRoutines
    Dim title As String = String.Empty

    Dim description As String = String.Empty

    Dim id As String = String.Empty

    Dim UploadFilesTempPath As String = String.Empty

    Dim UploadFilesLMS As String = String.Empty

    Dim UploadFilesImagesPath As String = String.Empty


    Dim pathscorm As String
    Dim pathscormmedia As String
    Dim savedFileName As String = String.Empty

    Dim msg As String = String.Empty

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim result As String = String.Empty

        Try
            utility = New SharedRoutines
            If context.Session("iduser") Is Nothing Then
                msg = "Session Terminata"
            End If

            Dim options As String = context.Request.QueryString("load")


            If options <> "" Then


                Try
                    title = context.Request.Form("title").ToString

                Catch ex As Exception
                    title = ""
                End Try

                Try
                    id = context.Request.Form("id").ToString
                    If id = "" Then
                        id = 0
                    End If
                Catch ex As Exception
                    id = 0
                End Try

                Try
                    description = context.Request.Form("description").ToString
                Catch ex As Exception

                    description = String.Empty


                End Try

                If HttpContext.Current.Request.QueryString("load") = "createyoutubevideo" Then

                    Dim s As New Scorm
                    savedFileName = StorageRoot(context, options)
                    Dim pathfilename = s.CreateScormZipContent(0)
                    If Not IO.Directory.Exists(savedFileName & pathfilename) Then
                        IO.Directory.CreateDirectory(savedFileName & pathfilename)
                        pathfilename = savedFileName & pathfilename
                    Else
                        pathfilename = savedFileName & "/" & pathfilename
                    End If
                    s.CreaScorm12youtube(pathfilename, HttpContext.Current.Request.Form("addressyoutube"))
                    msg = " Scorm Creato"
                Else
                    msg = Uploadfile(context, options)
                End If

            Else
                context.Response.ClearHeaders()
                context.Response.StatusCode = 405
            End If


        Catch ex As Exception
            SharedRoutines.LogWrite(ex.Message)
        End Try

        HttpContext.Current.Response.Write(msg)
        HttpContext.Current.Response.Flush() ' Sends all currently buffered output to the client.
        HttpContext.Current.Response.SuppressContent = True  ' Gets or sets a value indicating whether to send HTTP content to the client.
        HttpContext.Current.ApplicationInstance.CompleteRequest()


    End Sub


    'Function getscormid(id As Integer, idcourse As Integer)

    '    Dim s As New Scorm
    '    s.GetScormByReference(id)
    '    Return s.Coursepath

    'Return False 

    'End function
    Private Shared Function Num(ByVal value As String) As Integer
        Dim returnVal As String = String.Empty


        Dim collection As MatchCollection = Regex.Matches(value, "\d+")
        For Each m As Match In collection
            returnVal += m.ToString()
        Next
        Return Convert.ToInt32(returnVal)
        Return False

    End Function
    Function Uploadfile(ByVal context As HttpContext, options As String)

        Dim i As Integer
        Dim r As New Generic.LinkedList(Of ViewDataUploadFilesResult)
        Dim files As String()

        Dim js As New Script.Serialization.JavaScriptSerializer

        Dim jsonObj
        Dim FileName As String = String.Empty

        Try

            If context.Request.Files.Count >= 1 Then

                Dim maximumFileSize As Integer = ConfigurationManager.AppSettings("UploadFilesMaximumFileSize")

                context.Response.ContentType = "text/plain"



                Dim s As New Scorm
                Dim l As New SharedRoutines

                For i = 0 To context.Request.Files.Count - 1
                    Dim hpf As HttpPostedFile

                    Dim titlefile As String = String.Empty

                    hpf = context.Request.Files.Item(i)
                    If title = "" Then
                        title = IO.Path.GetFileNameWithoutExtension(hpf.FileName)
                    Else
                        titlefile = title
                    End If

                    'If HttpContext.Current.Request.Browser.Browser.ToUpper = "IE" Then
                    '    files = hpf.FileName.Split(CChar("\\"))
                    '    FileName = files(files.Length - 1)
                    'Else
                    '    FileName = hpf.FileName
                    'End If

                    Select Case options
                        Case "createbackcertificate"
                            FileName = IO.Path.GetFileName(hpf.FileName)
                        Case "createitem"
                            FileName = HttpContext.Current.Session("idUser") & "_" & DateDiff(DateInterval.Minute, CDate("01/01/1900"), Now) & "_" & IO.Path.GetFileName(hpf.FileName)
                        Case "createscormmedia"
                            FileName = context.Request.QueryString("filename")
                        Case "createvideozip"
                            pathscorm = context.Server.MapPath(context.Session("lmscontentpathrel")) & "\Content\scorm\" & s.CreateScormZipContent(0)

                            If Not IO.Directory.Exists(pathscorm) Then
                                IO.Directory.CreateDirectory(pathscorm)
                                pathscormmedia = pathscorm & "\media\"
                                IO.Directory.CreateDirectory(pathscormmedia)
                                savedFileName = pathscormmedia
                                FileName = "slide1.mp4"
                            Else
                            End If
                        Case "createrecordvideo"

                            pathscorm = context.Server.MapPath(context.Session("lmscontentpathrel")) & "\Content\scorm\" & s.CreateScormZipContent(0)

                            If Not IO.Directory.Exists(pathscorm) Then
                                IO.Directory.CreateDirectory(pathscorm)
                                pathscormmedia = pathscorm & "\media\"
                                IO.Directory.CreateDirectory(pathscormmedia)
                                savedFileName = pathscormmedia
                                FileName = "slide1.mp4"
                            Else
                            End If
                        Case "loadcurriculum"

                            FileName = "Curriculum_" & HttpContext.Current.Session("iduser") & IO.Path.GetExtension(hpf.FileName)
                        Case "loadavatar"
                            FileName = "Avatar" & HttpContext.Current.Session("iduser") & IO.Path.GetExtension(hpf.FileName)
                        Case "loadexcel"
                            FileName = "Excel" & IO.Path.GetExtension(hpf.FileName)
                        Case "loaddocstudents"
                            FileName = HttpContext.Current.Session("fullname") & "_" & IO.Path.GetFileNameWithoutExtension(context.Request.Form("nomefile")) & IO.Path.GetExtension(hpf.FileName)
                        Case Else
                            FileName = IO.Path.GetFileName(hpf.FileName)
                    End Select



                    If hpf.ContentLength >= 0 And (hpf.ContentLength <= maximumFileSize * 10000000 Or maximumFileSize = 0) Then

                        If savedFileName = "" Then
                            savedFileName = StorageRoot(context, options, FileName)

                        End If


                        savedFileName = savedFileName & FileName
                        hpf.SaveAs(savedFileName)

                        msg = FileName

                        Select Case options
                            Case "loadexcel"
                                msg = utility.iscriviexcel(savedFileName, context.Request.QueryString("tipo"))
                            Case "addquestgift"
                                Dim t As New TestObject
                                'Dim soglia As String = context.Request.Form("soglia").ToString
                                ' t.InsertTestGift(savedFileName, titlefile, description, soglia)

                                t.addquestionGift(savedFileName, context.Request.Form("idtest"))
                            Case "createscormzip"

                                Dim outfolder As String = s.CreateScormZipContent(id)

                                If outfolder <> "" Then
                                    l.ExtractZipFile(savedFileName, "", outfolder)

                                End If
                            Case "createvideozip"

                                s.CreaScormsingle12(pathscorm)
                            Case "createrecordvideo"

                                s.CreaScormsingle12(pathscorm)
                            Case "createscormmedia"

                                s.CreaScorm12(Replace(savedFileName, "media", ""))
                            Case "createpoll"
                                Dim p As New Poll
                                p.InsertPollGift(savedFileName, titlefile, description)
                            Case "createitem"
                                Dim it As New Item
                                it.AddEditItem(titlefile, description, FileName, id)
                            Case "createlogo"
                                Dim it As New SharedRoutines
                                it.CreateLogo(title, FileName)

                            Case "loadavatar"
                                Dim it As New SharedRoutines
                                it.AddAvatar(FileName)
                            Case "loaddoc"

                            Case "loaddocstudents"
                                Dim f As New Ajaxadminlms

                                Try


                                    f.adddocstudent(context.Request.Form("idsessione"), savedFileName)

                                Catch ex As Exception
                                    SharedRoutines.LogWrite(ex.ToString & savedFileName)
                                End Try
                        End Select








                    Else

                        msg = "Nessun file"

                    End If
                Next

            End If



        Catch ex As Exception
            Throw
        End Try


    End Function



    Private Sub DeleteFile(ByVal context As HttpContext, options As String)
        Try
            Dim path = StorageRoot(context, options)
            Dim file = context.Request("f")
            path &= file

            If System.IO.File.Exists(path) Then
                System.IO.File.Delete(path)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub



    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

#Region "Generic helpers"

    Private Function StorageRoot(ByVal context As HttpContext, options As String, Optional filename As String = "noname") As String

        Try

            Dim temppath As String = ""

            Dim initPath As String = ""

            UploadFilesTempPath = ConfigurationManager.AppSettings("UploadFilesTempPath")
            UploadFilesLMS = context.Session("lmscontentpathrel")
            UploadFilesImagesPath = "images/"

            Select Case options
                Case "createpoll"
                    initPath = context.Server.MapPath(UploadFilesTempPath)
                Case "addquestgift"
                    initPath = context.Server.MapPath(UploadFilesTempPath)
                Case "createscormzip"
                    initPath = context.Server.MapPath(UploadFilesLMS & "\Content\scorm")
                Case "createyoutubevideo"
                    initPath = context.Server.MapPath(UploadFilesLMS & "\Content\scorm")
                Case "createvideozip"
                    initPath = context.Server.MapPath(UploadFilesLMS & "\Content\scorm\media")
                Case "createrecordvideo"
                    initPath = context.Server.MapPath(UploadFilesLMS & "\Content\scorm\media")

                Case "createscormmedia"
                    Dim s As New Scorm
                    s.GetScormByID(context.Request.QueryString("scormid"))
                    Dim path As String = IO.Path.Combine(context.Server.MapPath("\"), s.Coursepath) & "media"
                    If Not IO.Directory.Exists(path) Then
                        IO.Directory.CreateDirectory(path)
                    End If

                    initPath = path
                Case "createitem"
                    initPath = context.Server.MapPath(UploadFilesLMS & "\content\item")


                Case "loadavatar"
                    initPath = context.Server.MapPath(UploadFilesImagesPath & "avatar/")
                Case "loaddoc"

                    Dim f As New Ajaxadminlms
                    Dim iddoc
                    Try

                        iddoc = f.adddocsessione(context.Request.Form("idsessione"), filename)

                        If (iddoc) Then
                            f.adddocdocuments(context.Request.Form("idsessione"), iddoc, context.Request.Form("idcategory"))
                        End If
                    Catch ex As Exception
                        SharedRoutines.LogWrite(ex.ToString & savedFileName)
                    End Try


                    If HttpContext.Current.Request.Url.Authority.IndexOf("localhost") > -1 Then
                        temppath = HttpContext.Current.Server.MapPath("temp") & "\localhost"
                    Else
                        temppath = HttpContext.Current.Session("lmscontentpath")
                    End If


                    If Not IO.Directory.Exists(temppath) Then
                        IO.Directory.CreateDirectory(temppath)
                        initPath = temppath
                    Else
                        initPath = temppath
                    End If

                    temppath = initPath & "\" & context.Request.Form("idsessione")
                    If Not IO.Directory.Exists(temppath) Then
                        IO.Directory.CreateDirectory(temppath)
                        initPath = temppath
                    Else
                        initPath = temppath
                    End If


                    If Not IO.Directory.Exists(IO.Path.Combine(initPath, iddoc)) Then
                        IO.Directory.CreateDirectory(IO.Path.Combine(initPath, iddoc))
                        initPath = IO.Path.Combine(initPath, iddoc)
                    Else
                        initPath = IO.Path.Combine(initPath, iddoc)
                    End If

                Case "loaddocstudents"



                    Try
                        If HttpContext.Current.Request.Url.Authority.IndexOf("localhost") > -1 Then
                            temppath = HttpContext.Current.Server.MapPath("temp") & "\localhost"
                        Else
                            temppath = HttpContext.Current.Session("lmscontentpath")
                        End If


                        If Not IO.Directory.Exists(temppath) Then
                            IO.Directory.CreateDirectory(temppath)
                            initPath = temppath
                        Else
                            initPath = temppath
                        End If

                        temppath = initPath & "\" & context.Request.Form("idsessione")
                        If Not IO.Directory.Exists(temppath) Then
                            IO.Directory.CreateDirectory(temppath)
                            initPath = temppath
                        Else
                            initPath = temppath
                        End If


                        If Not IO.Directory.Exists(IO.Path.Combine(initPath, context.Request.Form("iddoc"))) Then
                            IO.Directory.CreateDirectory(IO.Path.Combine(initPath, context.Request.Form("iddoc")))
                            initPath = IO.Path.Combine(initPath, context.Request.Form("iddoc"))
                        Else
                            initPath = IO.Path.Combine(initPath, context.Request.Form("iddoc"))
                        End If

                    Catch ex As Exception

                    End Try






            End Select



            CheckPath(initPath)

            Return initPath
        Catch ex As Exception
            Throw
        End Try
        Return False

    End Function

    Private Sub CheckPath(ByRef serverPath As String)
        Dim initPath As String = String.Empty


        Dim tempPath As String = String.Empty


        Dim folders As String()

        Try

            folders = serverPath.Split(CChar("\\"))

            ' Save file to a server
            If serverPath.Contains("\\") Then
                initPath = "\\"
            Else
                ' Save file to a local folders
            End If

            For i As Integer = 0 To folders.Length - 1
                If tempPath.Trim = String.Empty And folders(i) <> String.Empty Then



                    tempPath = initPath & folders(i)
                ElseIf tempPath.Trim <> String.Empty And folders(i).Trim <> String.Empty Then


                    tempPath = tempPath & "\" & folders(i)

                    ' Doesn't check if it's a network connection
                    If Not tempPath.Contains("\\") And
                    Not folders(i).Contains("$") Then

                        If Not System.IO.Directory.Exists(tempPath) Then
                            System.IO.Directory.CreateDirectory(tempPath)
                        End If

                    Else
                        If Not System.IO.Directory.Exists(tempPath) Then
                            System.IO.Directory.CreateDirectory(tempPath)
                        End If

                    End If

                End If

            Next

            serverPath = tempPath & "\"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function GivenFilename(ByVal context As HttpContext) As Boolean
        Try
            Return Not String.IsNullOrEmpty(context.Request("f"))
        Catch ex As Exception
            Throw
        End Try
        Return False

    End Function

    Private Sub DeliverFile(ByVal context As HttpContext, options As String)
        Try
            Dim file = context.Request("f")
            Dim filePath = StorageRoot(context, options) + file

            If System.IO.File.Exists(filePath) Then
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + file)
                context.Response.ContentType = "application/octet-stream"
                context.Response.ClearContent()
                context.Response.WriteFile(filePath)
            Else
                context.Response.StatusCode = 404
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

End Class

#Region "local Class"

Public Class ViewDataUploadFilesResult
    Public _name As String = String.Empty

    Public _length As Integer
    Public _type As String = String.Empty

    Public _url As String = String.Empty

    Public delete_url As String = String.Empty

    Public delete_type As String = String.Empty

    Public _errorMSG As String = String.Empty


    Sub New()
        Try

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub New(ByVal Name As String, ByVal Length As Integer, ByVal Type As String, ByVal URL As String)
        Try
            _name = Name
            _length = Length
            _type = Type
            _url = "Handler.ashx?f=" + Name
            delete_url = "Handler.ashx?f=" + Name
            delete_type = "POST"
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub New(ByVal Name As String, ByVal Length As Integer, ByVal Type As String, ByVal URL As String, ByVal errorMSG As String)
        Try
            _name = Name
            _length = Length
            _type = Type
            _url = "Upload.ashx?f=" + Name
            delete_url = "Upload.ashx?f=" + Name
            delete_type = "POST"
            _errorMSG = errorMSG
        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class

#End Region