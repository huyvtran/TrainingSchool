Imports MySql
Imports System.Data.DataTable

Public Class rconnection
    Inherits MarshalByRefObject

    Public mconn As MySql.Data.MySqlClient.MySqlConnection

    Private _Transaction As MySql.Data.MySqlClient.MySqlTransaction = Nothing

    Public Enum eConntype
        SqlClient = 0
        mysql = 1
        OleDb = 2

    End Enum

    Private _connType As eConntype = eConntype.SqlClient

    Public ReadOnly Property Type() As eConntype
        Get
            Return _connType
        End Get
    End Property

    Public ReadOnly Property State() As System.Data.ConnectionState
        Get
            Return mconn.State
        End Get
    End Property

    Public Property Transaction() As MySql.Data.MySqlClient.MySqlTransaction
        Get
            Return _Transaction
        End Get
        Set(ByVal value As MySql.Data.MySqlClient.MySqlTransaction)
            _Transaction = value
        End Set
    End Property

    ''' <summary>
    ''' Crea Nuova connessione
    ''' </summary>
    ''' <remarks>la connessione verra aperta e chiusa secondo necessita</remarks>
    Public Sub New()
        Try
            Me.getConnection("ActiveConn")
        Catch ex As Exception

            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Crea Nuova connessione a partire da un nome connessione
    ''' </summary>
    ''' <param name="ConnName">nome connessione</param>
    ''' <remarks>la connessione verra aperta e chiusa secondo necessita</remarks>
    Public Sub New(ByVal ConnName As String)
        Try
            Me.getConnection(ConnName)
        Catch ex As Exception


        End Try
    End Sub
    Public Sub New(ByVal ConnName As String, Optional ByVal server1 As String = "", Optional ByVal user As String = "", Optional ByVal pass As String = "")
        Try
            Me.getConnection(ConnName, server1, user, pass)
        Catch ex As Exception


        End Try
    End Sub


    Private Sub getConnection(ByVal ConnName As String, Optional ByVal server1 As String = "", Optional user As String = "", Optional pass As String = "")
        Dim ConnSell As String = ""

        Try

            mconn = New MySql.Data.MySqlClient.MySqlConnection
            If server1 = "" Then
                server1 = System.Configuration.ConfigurationManager.AppSettings("HostMaster")
            End If
            If user = "" Then
                user = System.Configuration.ConfigurationManager.AppSettings("user")
                pass = System.Configuration.ConfigurationManager.AppSettings("pass")
            End If

            mconn.ConnectionString = "server=" & server1 & ";Database=" & ConnName & " ;Uid=" & user & ";Pwd=" & pass & ";Max Pool Size=1000;Allow Zero Datetime=true;"




        Catch ex As System.Exception

            Throw ex
        End Try
    End Sub

    Public Sub ClearConnectionPool()
        Try
            mconn.Dispose()
        Catch ex As System.Exception
            SharedRoutines.LogWrite(ex.ToString)
        End Try
    End Sub

    Public Function TransactionBegin() As MySql.Data.MySqlClient.MySqlTransaction
        Try
            If mconn.State = ConnectionState.Closed Then
                mconn.Open()
                cntConn += 1
                cntTotConn += 1
                'GenericFunctions.writelog("Aperta connessione ---------------- Tot con: " & cntTotConn & " di cui aperte: " & cntConn)
            End If
            _Transaction = mconn.BeginTransaction
            Return _Transaction
        Catch ex As System.Exception

            Throw ex
        End Try
        ' Return False 


    End Function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'MySqlTransaction'. 
    Public Sub TransactionRollBack()
        Try
            _Transaction.Rollback()
        Catch ex As System.Exception

            Throw ex
        Finally
            mconn.Close()
            cntConn -= 1
            'GenericFunctions.writelog("Chiusa connessione - Con aperte: " & cntConn)
        End Try
    End Sub

    Public Sub TransactionCommit()
        Try
            _Transaction.Commit()
        Catch ex As System.Exception

            Throw ex
        Finally
            mconn.Close()
            cntConn -= 1
            'GenericFunctions.writelog("Chiusa connessione - Con aperte: " & cntConn)
        End Try
    End Sub

    ''' <summary>
    ''' Esegue un DataReader
    ''' </summary>
    ''' <param name="strComand">comando SQL</param>
    ''' <returns>Common.DbDataReader</returns>
    ''' <remarks></remarks>
    Public Function GetDataReader(ByVal strComand As String) As MySql.Data.MySqlClient.MySqlDataReader
        Return GetDataReader(strComand)
        ' Return False 


    End Function

    ''' <summary>
    ''' Esegue un DataReader
    ''' </summary>
    ''' <param name="strComand">comando SQL/StoredProcedure/TableDirect</param>
    ''' <param name="enumCommandType">Tipo comando (CommandType)
    ''' Text
    ''' StoredProcedure
    ''' TableDirect
    ''' </param>
    ''' <param name="arrParam">array Parameter</param>
    ''' <returns>Common.DbDataReader</returns>
    ''' <remarks></remarks>
    Public Function GetDataReader(ByVal strComand As String, ByVal enumCommandType As CommandType, _
                                    ByVal arrParam() As MySql.Data.MySqlClient.MySqlParameter) As MySql.Data.MySqlClient.MySqlDataReader
        Dim mcmd As New MySql.Data.MySqlClient.MySqlCommand


        mcmd.CommandType = enumCommandType
        mcmd.CommandText = strComand
        mcmd.Connection = mconn

        If Not _Transaction Is Nothing Then mcmd.Transaction = _Transaction

        Try
            If Not (arrParam Is Nothing) Then
                Dim i As Integer
                For i = 0 To arrParam.Length - 1
                    mcmd.Parameters.Add(arrParam(i))
                Next
            End If

            If mcmd.Connection.State = ConnectionState.Closed Then
                mcmd.Connection.Open()
                cntConn += 1
                cntTotConn += 1
                'GenericFunctions.writelog("Aperta connessione ---------------- Tot con: " & cntTotConn & " di cui aperte: " & cntConn)
            End If

            If Not _Transaction Is Nothing Then
                Return mcmd.ExecuteReader
            Else
                cntConn -= 1
                'GenericFunctions.writelog("Chiusa connessione - Con aperte: " & cntConn)
                Return mcmd.ExecuteReader(CommandBehavior.CloseConnection)
            End If

        Catch ex As System.Exception

            Throw ex
        Finally

        End Try

        'Return False 


    End Function


    ''' <summary>
    ''' Esegue un command Execute
    ''' </summary>
    ''' <param name="strComand">comando SQL</param>
    ''' <returns>int record modificati
    ''' -1 se errore</returns>
    ''' <remarks></remarks>
    Public Function Execute(ByVal strComand As String) As Integer
        Return Execute(strComand)
    Return False 

 End function

    ''' <summary>
    ''' Esegue un command Execute
    ''' </summary>
    ''' <param name="strComand">comando SQL/StoredProcedure/TableDirect</param>
    ''' <param name="enumCommandType">Tipo comando (CommandType)
    ''' Text
    ''' StoredProcedure
    ''' TableDirect
    ''' </param>
    ''' <param name="arrParam">array Parameter</param>
    ''' <returns>int record modificati
    ''' -1 se errore</returns>
    ''' <remarks></remarks>
    Public Function Execute(ByVal strComand As String, ByVal enumCommandType As CommandType, _
                            ByVal arrParam() As MySql.Data.MySqlClient.MySqlParameter) As Integer
        Dim mcmd As New MySql.Data.MySqlClient.MySqlCommand


        mcmd.CommandType = enumCommandType
        mcmd.CommandText = strComand
        mcmd.Connection = mconn

        If Not _Transaction Is Nothing Then mcmd.Transaction = _Transaction

        Try
            If Not (arrParam Is Nothing) Then
                Dim i As Integer
                For i = 0 To arrParam.Length - 1
                    mcmd.Parameters.Add(arrParam(i))
                Next
            End If

            If mcmd.Connection.State = ConnectionState.Closed Then
                mcmd.Connection.Open()
                cntConn += 1
                cntTotConn += 1
                'GenericFunctions.writelog("Aperta connessione ---------------- Tot con: " & cntTotConn & " di cui aperte: " & cntConn)
            End If

            Return mcmd.ExecuteNonQuery()

        Catch ex As System.Exception
            SharedRoutines.LogWrite(ex.Message & ": " & strComand)
            Return False
        Finally
            If _Transaction Is Nothing Then
                mconn.Close()
                cntConn -= 1
                'GenericFunctions.writelog("Chiusa connessione - Con aperte: " & cntConn)
            End If
        End Try



    End function

    ''' <summary>
    ''' Esegue un DataTable
    ''' </summary>
    ''' <param name="strComand">comando SQL</param>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetDataTable(ByVal strComand As String) As System.Data.DataTable
        Return GetDataTable(strComand, CommandType.Text, Nothing)
        'Return False 


    End Function


    ''' <summary>
    ''' Esegue un DataTable
    ''' </summary>
    ''' <param name="strComand">comando SQL/StoredProcedure/TableDirect</param>
    ''' <param name="enumCommandType">Tipo comando (CommandType)
    ''' Text
    ''' StoredProcedure
    ''' TableDirect
    ''' </param>
    ''' <param name="arrParam">Array Parameter</param>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetDataTable(ByVal strComand As String, ByVal enumCommandType As CommandType,
                                    ByVal arrParam() As MySql.Data.MySqlClient.MySqlParameter) As System.Data.DataTable

        Dim mcmd As New MySql.Data.MySqlClient.MySqlCommand
        Dim mda As MySql.Data.MySqlClient.MySqlDataAdapter

        Dim mdt As New System.Data.DataTable

        mcmd.CommandType = enumCommandType
        mcmd.CommandText = strComand
        mcmd.Connection = mconn

        If Not _Transaction Is Nothing Then mcmd.Transaction = _Transaction

        Try
            If Not (arrParam Is Nothing) Then
                Dim i As Integer
                For i = 0 To arrParam.Length - 1
                    mcmd.Parameters.Add(arrParam(i))
                Next
            End If

            If mcmd.Connection.State = ConnectionState.Closed Then
                mcmd.Connection.Open()
                cntConn += 1
                cntTotConn += 1
                'GenericFunctions.writelog("Aperta connessione ---------------- Tot con: " & cntTotConn & " di cui aperte: " & cntConn)
            End If


            mda = New MySql.Data.MySqlClient.MySqlDataAdapter(mcmd)
            mda.Fill(mdt)



        Catch ex As System.Exception
            If ex.Message.StartsWith("There is already an open") Then
                mconn.Close()
                mconn.Open()
                mda.Fill(mdt)
            End If
            SharedRoutines.LogWrite(ex.Message & strComand)
            mconn.Close()
            mconn.Dispose()



        Finally
            If _Transaction Is Nothing Then
                mconn.Close()
                mconn.Dispose()
                cntConn -= 1
                'GenericFunctions.writelog("Chiusa connessione - Con aperte: " & cntConn)
            End If
        End Try

        Return mdt
        'Return False 


    End Function


    ''' <summary>
    ''' Restituisce un Parameter per il db corrente
    ''' </summary>
    ''' <returns>Data.Common.DbParameter</returns>
    ''' <remarks></remarks>
    Public Function GetParameter() As MySql.Data.MySqlClient.MySqlParameter
        Dim mpar As New MySql.Data.MySqlClient.MySqlParameter

        Return mpar
        ' Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'MySqlParameter'. 
    End function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'MySqlParameter'. 

    ''' <summary>
    ''' Restituisce un Parameter per il db corrente
    ''' </summary>
    ''' <param name="paramName"></param>
    ''' <param name="paramValue"></param>
    ''' <returns>Data.Common.DbParameter</returns>
    ''' <remarks></remarks>
    Public Function GetParameter(ByVal paramName As String, ByVal paramValue As Object) As MySql.Data.MySqlClient.MySqlParameter
        Dim mpar As MySql.Data.MySqlClient.MySqlParameter = Nothing
        Try

            mpar = New MySql.Data.MySqlClient.MySqlParameter(paramName, paramValue)


        Catch ex As Exception
            Throw ex
        End Try
        Return mpar
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'MySqlParameter'. 
    End function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'MySqlParameter'. 

    ''' <summary>
    ''' Restituisce un Parameter per il db corrente
    ''' </summary>
    ''' <param name="paramName"></param>
    ''' <param name="paramValue"></param>
    ''' <param name="isLargeValue">True se il valore Stringa è maggiore di 4000</param>
    ''' <returns>Data.Common.DbParameter</returns>
    ''' <remarks></remarks>
    Public Function GetParameter(ByVal paramName As String, ByVal paramValue As Object, ByVal isLargeValue As Boolean) As MySql.Data.MySqlClient.MySqlParameter
        If isLargeValue = False Then Return GetParameter(paramName, paramValue)

        Dim mpar As MySql.Data.MySqlClient.MySqlParameter = Nothing
        Try

            mpar = New Data.MySqlClient.MySqlParameter(paramName, MySql.Data.MySqlClient.MySqlDbType.VarChar)
            mpar.Value = paramValue

        Catch ex As Exception
            Throw ex
        End Try
        Return mpar
        'Return False 

#Disable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'MySqlParameter'. 
    End function
#Enable Warning BC30311 ' Non è possibile convertire il valore di tipo 'Boolean' in 'MySqlParameter'. 
End Class