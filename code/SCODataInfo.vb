Imports System
Imports System.Data


#Region "SCODataInfo"
Public Class SCODataInfo

    ' local property declarations
    Dim _sCOID As Integer
    Dim _courseID As Integer
    Dim _identifier As String = String.Empty

    Dim _type As String = String.Empty

    Dim _title As String = String.Empty

    Dim _launch As String = String.Empty

    Dim _parameterString As String = String.Empty

    Dim _dataFromLMS As String = String.Empty

    Dim _prerequisites As String = String.Empty

    Dim _masteryScore As String = String.Empty

    Dim _maxTimeAllowed As String = String.Empty

    Dim _timeLimitAction As String = String.Empty

    Dim _sequence As Integer
    Dim _theLevel As Integer

#Region "Constructors"
    Public Sub New()
    End Sub

    Public Sub New(ByVal sCOID As Integer, ByVal courseID As Integer, ByVal identifier As String, ByVal type As String, ByVal title As String, ByVal launch As String, ByVal parameterString As String, ByVal dataFromLMS As String, ByVal prerequisites As String, ByVal masteryScore As String, ByVal maxTimeAllowed As String, ByVal timeLimitAction As String, ByVal sequence As Integer, ByVal theLevel As Integer)
        Me.CourseID = courseID
        Me.SCOID = sCOID
        Me.Identifier = identifier
        Me.Type = type
        Me.Title = title
        Me.Launch = launch
        Me.ParameterString = parameterString
        Me.DataFromLMS = dataFromLMS
        Me.Prerequisites = prerequisites
        Me.MasteryScore = masteryScore
        Me.MaxTimeAllowed = maxTimeAllowed
        Me.TimeLimitAction = timeLimitAction
        Me.Sequence = sequence
        Me.TheLevel = theLevel
    End Sub
#End Region

#Region "Public Properties"
    Public Property SCOID() As Integer
        Get
            Return _sCOID
        End Get
        Set(ByVal Value As Integer)
            _sCOID = Value
        End Set
    End Property

    Public Property CourseID() As Integer
        Get
            Return _courseID
        End Get
        Set(ByVal Value As Integer)
            _courseID = Value
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

    Public Property Type() As String

        Get
            Return _type
        End Get
        Set(ByVal Value As String)
            _type = Value
        End Set
    End Property

    Public Property Title() As String

        Get
            Return _title
        End Get
        Set(ByVal Value As String)
            _title = Value
        End Set
    End Property

    Public Property Launch() As String

        Get
            Return _launch
        End Get
        Set(ByVal Value As String)
            _launch = Value
        End Set
    End Property

    Public Property ParameterString() As String

        Get
            Return _parameterString
        End Get
        Set(ByVal Value As String)
            _parameterString = Value
        End Set
    End Property

    Public Property DataFromLMS() As String

        Get
            Return _dataFromLMS
        End Get
        Set(ByVal Value As String)
            _dataFromLMS = Value
        End Set
    End Property

    Public Property Prerequisites() As String

        Get
            Return _prerequisites
        End Get
        Set(ByVal Value As String)
            _prerequisites = Value
        End Set
    End Property

    Public Property MasteryScore() As String

        Get
            Return _masteryScore
        End Get
        Set(ByVal Value As String)
            _masteryScore = Value
        End Set
    End Property

    Public Property MaxTimeAllowed() As String

        Get
            Return _maxTimeAllowed
        End Get
        Set(ByVal Value As String)
            _maxTimeAllowed = Value
        End Set
    End Property

    Public Property TimeLimitAction() As String

        Get
            Return _timeLimitAction
        End Get
        Set(ByVal Value As String)
            _timeLimitAction = Value
        End Set
    End Property

    Public Property Sequence() As Integer
        Get
            Return _sequence
        End Get
        Set(ByVal Value As Integer)
            _sequence = Value
        End Set
    End Property

    Public Property TheLevel() As Integer
        Get
            Return _theLevel
        End Get
        Set(ByVal Value As Integer)
            _theLevel = Value
        End Set
    End Property
#End Region
End Class
#End Region

#Region "SCODataController"
Public Class SCODataController

    'If you name this method Get you must put the name between []
    Public Function [Get](ByVal sCOID As Integer) As SCODataInfo

        ' Return CType(CBO.FillObject(SqlDataProvider.GetSCOData(sCOID), GetType(SCODataInfo)), SCODataInfo)

        '   Return False 

    End Function

#Disable Warning BC42105 ' La funzione 'Get' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 
#Disable Warning BC42105 ' La funzione 'Get' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 
    Public Function List() As ArrayList
#Enable Warning BC42105 ' La funzione 'Get' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 
#Enable Warning BC42105 ' La funzione 'Get' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 

        ' Return CBO.FillCollection(SqlDataProvider.ListSCOData(), GetType(SCODataInfo))

        'Return False 

    End function

#Disable Warning BC42105 ' La funzione 'List' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 
#Disable Warning BC42105 ' La funzione 'List' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 
    Public Shared Function GetByCourseID(ByVal courseID As Integer) As ArrayList
#Enable Warning BC42105 ' La funzione 'List' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 
#Enable Warning BC42105 ' La funzione 'List' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 

        '  Return CBO.FillCollection(SqlDataProvider.GetSCODataByCourseData(courseID), GetType(SCODataInfo))

        'Return False 

    End function

#Disable Warning BC42105 ' La funzione 'GetByCourseID' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 
#Disable Warning BC42105 ' La funzione 'GetByCourseID' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 
    Public Function Add(ByVal objSCOData As SCODataInfo) As Integer
#Enable Warning BC42105 ' La funzione 'GetByCourseID' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 
#Enable Warning BC42105 ' La funzione 'GetByCourseID' non restituisce un valore in tutti i percorsi del codice. È possibile che in fase di esecuzione venga restituita un'eccezione dovuta a un riferimento Null quando viene usato il risultato. 

        ' Return CType(SqlDataProvider.AddSCOData(objSCOData.CourseID, objSCOData.Identifier, objSCOData.Type, objSCOData.Title, objSCOData.Launch, objSCOData.ParameterString, objSCOData.DataFromLMS, objSCOData.Prerequisites, objSCOData.MasteryScore, objSCOData.MaxTimeAllowed, objSCOData.TimeLimitAction, objSCOData.Sequence, objSCOData.TheLevel), Integer)

        Return False

    End Function

    Public Sub Update(ByVal objSCOData As SCODataInfo)

        ' SqlDataProvider.UpdateSCOData(objSCOData.SCOID, objSCOData.CourseID, objSCOData.Identifier, objSCOData.Type, objSCOData.Title, objSCOData.Launch, objSCOData.ParameterString, objSCOData.DataFromLMS, objSCOData.Prerequisites, objSCOData.MasteryScore, objSCOData.MaxTimeAllowed, objSCOData.TimeLimitAction, objSCOData.Sequence, objSCOData.TheLevel)

    End Sub

    Public Sub Delete(ByVal sCOID As Integer)

        ' SqlDataProvider.DeleteSCOData(sCOID)

    End Sub

End Class
#End Region


