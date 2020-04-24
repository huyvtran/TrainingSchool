Imports System
Imports System.Data


#Region "UserSCODataInfo"
Public Class UserSCODataInfo

    ' local property declarations
    Dim _scoID As Integer
    Dim _UserID As Integer
    Dim _identifier As String = String.Empty

    Dim _lessonStatus As String = String.Empty

    Dim _lessonLocation As String = String.Empty

    Dim _sCOExit As String = String.Empty

    Dim _sCOEntry As String = String.Empty

    Dim _rawScore As String = String.Empty

    Dim _suspendData As String = String.Empty

    Dim _totalTime As String = String.Empty

    Dim _sessionTime As String = String.Empty

    Dim _credit As String = String.Empty

    Dim _idtrack As String = String.Empty

    Dim _myval As Integer
#Region "Constructors"
    Public Sub New()
    End Sub

    Public Sub New(ByVal sCOID As Integer, ByVal UserID As Integer, ByVal identifier As String, ByVal lessonStatus As String, ByVal sCOExit As String, ByVal sCOEntry As String, ByVal rawScore As String, ByVal suspendData As String, idtrack As Integer)
        Me.UserID = UserID
        Me.SCOID = sCOID
        Me.Identifier = identifier
        Me.LessonStatus = lessonStatus
        Me.SCOExit = sCOExit
        Me.SCOEntry = sCOEntry
        Me.RawScore = rawScore
        Me.SuspendData = suspendData
        Me.idtrack = idtrack
        Me.Credit = Credit
        Me.myval = myval
    End Sub

#End Region

#Region "Public Properties"

    Public Property myval() As Integer
        Get
            Return _myval
        End Get
        Set(ByVal Value As Integer)
            _myval = Value
        End Set
    End Property
    Public Property SCOID() As Integer
        Get
            Return _scoID
        End Get
        Set(ByVal Value As Integer)
            _scoID = Value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal Value As Integer)
            _UserID = Value
        End Set
    End Property

    Public Property Identifier() As String

        Get
            Return _identifier
        End Get
        Set(ByVal Value As String)
            _identifier = Value
        End Set
    End Property

    Public Property LessonStatus() As String

        Get
            Return _lessonStatus
        End Get
        Set(ByVal Value As String)
            _lessonStatus = Value
        End Set
    End Property

    Public Property LessonLocation() As String

        Get
            Return _lessonLocation
        End Get
        Set(ByVal Value As String)
            _lessonLocation = Value
        End Set
    End Property

    Public Property SCOExit() As String

        Get
            Return _sCOExit
        End Get
        Set(ByVal Value As String)
            _sCOExit = Value
        End Set
    End Property

    Public Property SCOEntry() As String

        Get
            Return _sCOEntry
        End Get
        Set(ByVal Value As String)
            _sCOEntry = Value
        End Set
    End Property

    Public Property RawScore() As String

        Get
            Return _rawScore
        End Get
        Set(ByVal Value As String)
            _rawScore = Value
        End Set
    End Property

    Public Property SuspendData() As String

        Get
            Return _suspendData
        End Get
        Set(ByVal Value As String)
            _suspendData = Value
        End Set
    End Property
    Public Property TotalTime() As String

        Get
            Return _totalTime
        End Get
        Set(ByVal Value As String)
            _totalTime = Value
        End Set
    End Property

    Public Property SessionTime() As String

        Get
            Return _sessionTime
        End Get
        Set(ByVal Value As String)
            _sessionTime = Value
        End Set
    End Property

    Public Property Credit() As String

        Get
            Return _credit
        End Get
        Set(ByVal Value As String)
            _credit = Value
        End Set
    End Property

    Public Property idtrack() As String
        Get
            Return _idtrack
        End Get
        Set(ByVal Value As String)
            _idtrack = Value
        End Set
    End Property


#End Region
End Class
#End Region

#Region "UserSCODataController"
Public Class UserSCODataController

  
   

    Public Function List() As ArrayList

        ' Return CBO.FillCollection(SqlDataProvider.ListUserSCOData(), GetType(UserSCODataInfo))

        'Return False 

    End Function
    Public Function GetByUserCourses(ByVal CourseID As Integer) As ArrayList

        ' Return CBO.FillCollection(SqlDataProvider.GetUserSCODataByCourseID(CourseID), GetType(UserSCODataInfo))

        'Return False 

    End Function
    Public Shared Function Add(ByVal objUserSCOData As UserSCODataInfo) As Integer

        ' Return CType(SqlDataProvider.AddUserSCOData(objUserSCOData.CourseID, objUserSCOData.Identifier, objUserSCOData.LessonStatus, objUserSCOData.SCOExit, objUserSCOData.SCOEntry, objUserSCOData.RawScore, objUserSCOData.SuspendData), Integer)

        Return False

    End Function

    Public Shared Sub Update(ByVal objUserSCOData As UserSCODataInfo, idCourse As String)
        Dim u As New Scorm
        u.UpdateUserSCOData(objUserSCOData.SCOID, objUserSCOData.UserID, objUserSCOData.Identifier, objUserSCOData.LessonStatus, objUserSCOData.LessonLocation, objUserSCOData.Credit, objUserSCOData.SCOExit, objUserSCOData.SCOEntry, objUserSCOData.RawScore, objUserSCOData.SuspendData, objUserSCOData.TotalTime, objUserSCOData.SessionTime, idCourse, objUserSCOData.idtrack)

    End Sub

    Public Sub Delete(ByVal sCOID As Integer)

        ' SqlDataProvider.DeleteUserSCOData(sCOID)

    End Sub

    'Public Shared Sub UpdateStatus(ByVal CourseID As Integer, ByVal Identifier As String, ByVal status As String)


    '    Dim u As New Scorm
    '    u.UpdateUserSCOStatus(CourseID, Identifier, status)


    'End Sub

End Class
#End Region


