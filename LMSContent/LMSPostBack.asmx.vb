Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports TrainingSchool.SharedRoutines

<System.Web.Services.WebService(Namespace:="http://tempuri.org//LMSPostBack")> _
Public Class LMSPostBack
    Inherits System.Web.Services.WebService


    Dim s As Scorm

#Region " Web Services Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Web Services Designer.
        InitializeComponent()

        'Add your own initialization code after the InitializeComponent() call

    End Sub

    'Required by the Web Services Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Web Services Designer
    'It can be modified using the Web Services Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        'CODEGEN: This procedure is required by the Web Services Designer
        'Do not modify it using the code editor.
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

    <WebMethod()> _
    Public Function SaveQuestAnswer(idlog As String, idquest As String, idanswer As String, score As String)
        Try
            Dim ObjUser As New LogSession

            ObjUser.Savetestlog(idlog, idquest, idanswer, score)
        Catch ex As Exception

            SharedRoutines.LogWrite( ex.ToString)
            Return False
        End Try

        Return False

    End Function

    <WebMethod()>
    Public Function SavePollquestanswer(idlog As String, idquest As String, idanswer As String)
        Try
            Dim ObjUser As New LogSession

            ObjUser.Savepolllog(idlog, idquest, idanswer)
        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
            Return False
        End Try
        Return False

    End Function

    <WebMethod()>
    Public Function LMSFinish(ByVal UserID As String, ByVal Reference As String, IDEnter As Integer, CourseID As String, ByVal LessonStatus As String, ByVal LessonLocation As String, ByVal SCOExit As String, ByVal SCOEntry As String, ByVal RawScore As String, ByVal SuspendData As String, ByVal TotalTime As String, SessionTime As String)
        Try
            Dim ObjUser As New LogSession
            Dim ScoData As UserSCODataInfo

            Dim s As New Scorm

            ScoData = s.GetSCO(UserID, Reference) 'Fetch existing information

            ' Update the data
            ScoData.LessonLocation = LessonLocation
            ScoData.LessonStatus = LessonStatus
            ScoData.RawScore = RawScore
            ScoData.SCOEntry = "resume" 'SCOEntry - We set this to Resume since we know the SCO has been visited if it is saving data back
            ScoData.SCOExit = SCOExit
            ScoData.SuspendData = SuspendData
            ScoData.TotalTime = TotalTime
            ScoData.SessionTime = SessionTime

            'Save the data back to the database
            UserSCODataController.Update(ScoData, CourseID)

            ObjUser.SaveActionlog(UserID, CourseID, IDEnter, "scorm", "close")


        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
            Return False
        End Try

        Return True

        Return False

    End Function

    <WebMethod()>
    Public Function LoadDatabaseFromWS(UserID As Integer, Identifier As Integer) As String


        Try
            ' This function returns the lesson_status string for a learning event
            ' This string informs the TinyLMS API of the status of each sco from previous launches
            ' The string is represented like 3.c.5.p.7.n
            ' The first charactor is the identifier, the second is status, then it repeats


            Dim strDatabaseValues As String = ""
            Dim objSCO As UserSCODataInfo
            Dim status As String = String.Empty

            Dim i As Integer = 0
            Dim s As New Scorm
            objSCO = s.GetSCO(UserID, Identifier)

            Select Case objSCO.LessonStatus
                Case "passed"
                    status = "p"
                Case "completed"
                    status = "c"
                Case "failed"
                    status = "f"
                Case "incomplete"
                    status = "i"
                Case "browsed"
                    status = "b"
                Case "not attempted"
                    status = "n"
                Case Else
                    status = "n"
            End Select
            If strDatabaseValues <> "" Then
                'Add seperator for TimyLMS
                strDatabaseValues = strDatabaseValues & "."
            End If
            strDatabaseValues = strDatabaseValues & objSCO.Identifier & "." & status






            Return strDatabaseValues

        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
            Return False
        End Try

        'Return "3.i.5.i"

        Return False

    End Function

    <WebMethod()>
    Public Function GetSCODataFromWS(ByVal UserId As Integer, ByVal Identifier As String) As UserSCODataInfo
        Dim SCOData As UserSCODataInfo
        Try


            s = New Scorm
            SCOData = s.GetSCO(UserId, Identifier)
        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
            Return Nothing
        End Try
        Return SCOData

        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'UserSCODataInfo'. 
    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'UserSCODataInfo'. 

    <WebMethod()>
    Public Function SaveSCODataToWS(ByVal UserID As String, ByVal Reference As String, ByVal LessonStatus As String, ByVal LessonLocation As String, ByVal SCOExit As String, ByVal SCOEntry As String, ByVal RawScore As String, ByVal SuspendData As String, ByVal SessionTime As String, ByVal TotalTime As String, idcourse As String)

        Try

            Dim SCOData As UserSCODataInfo
            Dim s As New Scorm
            SCOData = s.GetSCO(UserID, Reference) 'Fetch existing information

            ' Update the data
            SCOData.LessonLocation = LessonLocation
            SCOData.LessonStatus = LessonStatus
            SCOData.RawScore = RawScore
            SCOData.SCOEntry = "resume" 'SCOEntry - We set this to Resume since we know the SCO has been visited if it is saving data back
            SCOData.SCOExit = SCOExit
            SCOData.SuspendData = SuspendData
            SCOData.SessionTime = SessionTime

            SCOData.TotalTime = TotalTime


            ' Save the data back to the database
            UserSCODataController.Update(SCOData, idcourse)
        Catch ex As Exception
            SharedRoutines.LogWrite( ex.ToString)
            Return False
        End Try

        Return True

        Return False

    End Function

End Class
