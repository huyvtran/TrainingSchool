Imports System.Web
Imports System.Web.Services
Imports TrainingSchool.SharedRoutines

Public Class KeepAliveSession
    Implements System.Web.IHttpHandler, IRequiresSessionState
    Dim conn As rconnection
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            'conn = CheckDatabase(conn)

            'Dim todaystart = Year(Now) & "-" & Month(Now).ToString("00") & "- 00:00:00"
            'Dim todayend = Year(Now) & "-" & Month(Now).ToString("00") & "- 23:59:59"
            'Dim sqlstringFirst = " Select SUM(TIME_TO_SEC(TIMEDIFF( lasttime,entertime))) As totaltime from learning_tracksession where iduser=" & context.Session("iduser") & " and  entertime >='" & todaystart & "' and lasttime <='" & todayend & "' "
            'Dim dt As DataTable = conn.GetDataTable(sqlstringFirst, CommandType.Text, Nothing)
            'Dim tot_time As String = dt.Rows(0)("totaltime").ToString


            context.Session("KeepSessionAlive") = DateTime.Now
            'Dim utility As New SharedRoutines
            'context.Response.Write(utility.ConvertSecToDate(tot_time))
        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try
        ' SharedRoutines.SetSession()
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class

