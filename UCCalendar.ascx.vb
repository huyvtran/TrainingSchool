
Imports System.Globalization
Imports TrainingSchool.SharedRoutines


Imports MySql.Data.MySqlClient

Public Class UCCalendar
    Inherits System.Web.UI.UserControl


    Public _thismonth As String = String.Empty

    Public _iduser As String = String.Empty


    Private salaryValue As Double

    Public Property thismonth() As String

        Get
            Return _thismonth
        End Get
        Private Set(ByVal value As String)
            _thismonth = value
        End Set
    End Property

    Public Property meiduser() As String

        Get
            Return _iduser
        End Get
        Private Set(ByVal value As String)
            _iduser = value
        End Set
    End Property


    Dim dt As DataTable = Nothing


    Dim r As rconnection
    Dim sqlstring As String = String.Empty

    Dim utility As New SharedRoutines
    Dim annosel As Integer


    Dim rconn As rconnection


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            rconn = CheckDatabase(rconn)
            VisualizzaCalendari()
            ' VisualizzaGiorni()

        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub




    Protected Sub VisualizzaCalendari()
        Dim tutte As String = String.Empty

        'If Not Request.QueryString("ac") = "tutte" Then
        '    tutte = "  b.regione like '%" & Request.QueryString("ac") & "' and "
        'End If

        Dim datastorica As String = String.Empty


        If Request.QueryString("op") = "storico" Then

            annosel = Year(Now) - 1
            datastorica = " datastart >= '" & annosel & "-01-01 00:00""' and  datastart <= '" & annosel & "-12-31 00:00""' "


        ElseIf Request.QueryString("op") = "prossimo" Then
            annosel = Year(Now) + 1
            datastorica = " datastart >= '" & annosel & "-01-01 00:00""' and  datastart <= '" & annosel & "-12-31 00:00""' "

        Else
            annosel = Year(Now)
            datastorica = " datastart >= '" & annosel & "-01-01 00:00""' and  datastart <= '" & annosel & "-12-31 00:00""' "

        End If



        sqlstring = "select  a.*,a.nomesessione as sessione, concat(firstname, ' ' ,lastname) as docente,email,tel,description  from  ((aula_sessioni a right  join aula_prenotazioni b on a.id=b.idsessione  ) join core_category_users d on a.idcategory=d.idcategory ) left join core_user c on b.iduser=c.idst where b.iduser=" & Session("iduser") & " and  " & tutte & "  " & datastorica & "  And visible=1 "



        Dim dt As DataTable = Nothing

        dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

        Dim scriptcalendar As String = String.Empty





        Dim scriptjavascript As String = "<script type='text/javascript' >" & vbCrLf
        scriptjavascript &= "function renderCalendar(){" & vbCrLf


        Dim dr() As DataRow
        If dt.Rows.Count > 0 Then

            For i = 1 To 12
                Dim mese As String = i.ToString("00")
                Dim d1 As String = annosel & "-" & mese & "-01 00:00"
                Dim d2 As String = annosel & "-" & mese & "-" & DateTime.DaysInMonth(annosel, mese) & " 23:59"
                'Dim d1 As Date = "01/" & mese & "/" & annosel & " 00:00"
                'Dim d2 As Date = DateTime.DaysInMonth(annosel, mese) & "/" & mese & "/" & annosel & " 00:00"


                '  If (Date.TryParse(d2, dc2) And Date.TryParse(d1, dc1)) Then
                dr = dt.Select("datastart >= '" & d1.ToString & "' and datastart <= '" & d2.ToString & "' ", "datastart asc")
                ' End If




                If dr.Length > 0 Then
                    scriptcalendar &= RenderCalendar(mese, dr)
                    lcalendar.InnerHtml &= "<div id='calendar" & mese & "'></div><br>" & vbCrLf
                    scriptjavascript &= "renderCalendar" & mese & "();" & vbCrLf
                End If

            Next

            If (Month(Now) = 12) Then
                For i = 1 To 11
                    Dim mese As String = i.ToString("00")
                    Dim d1 As String = annosel + 1 & "-" & mese & "-01 00:00"
                    Dim d2 As String = annosel + 1 & "-" & mese & "-" & DateTime.DaysInMonth(annosel + 1, mese) & " 23:59"
                    'Dim d1 As Date = "01/" & mese & "/" & annosel & " 00:00"
                    'Dim d2 As Date = DateTime.DaysInMonth(annosel, mese) & "/" & mese & "/" & annosel & " 00:00"


                    '  If (Date.TryParse(d2, dc2) And Date.TryParse(d1, dc1)) Then
                    dr = dt.Select("datastart >= '" & d1.ToString & "' and datastart <= '" & d2.ToString & "' ", "datastart asc")
                    ' End If




                    If dr.Length > 0 Then
                        scriptcalendar &= RenderCalendar(mese, dr)
                        lcalendar.InnerHtml &= "<div id='calendar" & mese & "'></div><br>" & vbCrLf
                        scriptjavascript &= "renderCalendar" & mese & "();" & vbCrLf
                    End If
                Next
            End If


            scriptjavascript &= "}" & vbCrLf

            scriptjavascript &= scriptcalendar
            scriptjavascript &= "  </script> "
            Page.RegisterClientScriptBlock("cal", scriptjavascript)


        Else
            scriptjavascript &= "$('.msgerror').text('Nessuna sessione programmata');"
            scriptjavascript &= "}" & vbCrLf


            scriptjavascript &= "  </script> "
            Me.Page.RegisterClientScriptBlock("cal", scriptjavascript)


        End If




    End Sub


    Sub RenderDay()
    End Sub
    Function RenderCalendar(ByVal mese As String, ByVal dr() As DataRow)
        Try
            Dim i As Integer = 1
            Dim scriptcalendar As String = String.Empty
            Dim joinurl As String = Trim(dr(0)("joinurl").ToString)
            Dim tipo As String = Trim(dr(0)("tipo").ToString)

            Dim flaginvia As Boolean = dr(0)("flaginvia")
            Dim datcorso As DateTime = FormatDateTime(dr(0)("datastart"), DateFormat.ShortDate)
            Dim today As DateTime = FormatDateTime(Now, DateFormat.ShortDate)
            Dim datastartora As DateTime = FormatDateTime(dr(0)("datastart"), DateFormat.GeneralDate)


            scriptcalendar &= "  function renderCalendar" & mese & "() {" & vbCrLf &
                         "   $('#calendar" & mese & "').fullCalendar({" & vbCrLf &
                           "     header: {" & vbCrLf &
    "left:   ' '," & vbCrLf &
    "center:  'title', " & vbCrLf &
    "right:  'month,agendaDay,agendaWeek' " & vbCrLf &
            "	                }, " & vbCrLf &
    "defaultDate:  '" & today.ToString("s", DateTimeFormatInfo.InvariantInfo) & "', " & vbCrLf &
    "lang:   'it', " & vbCrLf &
                              "buttonIcons: true, " & vbCrLf &
                               " theme:true, " & vbCrLf &
                               " weekNumbers: false, " & vbCrLf &
                                "editable:false, " & vbCrLf &
                                "eventLimit: false, " & vbCrLf &
                                  "defaultView: 'agendaDay', " & vbCrLf &
     "contentHeight:  'auto'," & vbCrLf &
                                "eventClick: function(calEvent, jsEvent, view) {" & vbCrLf &
     "$(this).css('border-color', 'green'); " &
            "var modal = $('#modal-event'); modal.find('.modal-title').html(calEvent.title); changeview(calEvent.tipo,calEvent.joinurl,calEvent.id,calEvent.write,calEvent.start,calEvent.end); " & vbCrLf


            scriptcalendar &= "$('#idsessione').val(calEvent.id);" & vbCrLf
            scriptcalendar &= "  if (calEvent.flaginvia=='1') {$(""#btninvia"").show();        } else {               $(""#btninvia"").hide();            }" & vbCrLf
            scriptcalendar &= " $('#nomecorso').val(calEvent.nomecorso);" & vbCrLf
            scriptcalendar &= " $('#divcalendar').empty();$('#divcalendar').html(calEvent.note +'<br>');" & vbCrLf
            scriptcalendar &= " $('#notemod').val(calEvent.note);" & vbCrLf






            scriptcalendar &= " }, " & vbCrLf &
                                "events: [" & vbCrLf

            For d = 0 To dr.Length - 1
                Dim dom As String = String.Empty




                Dim sedi As String = String.Empty


                Dim strcalendar = "<h3><br> <b>Inizio</b>: " & FormatDateTime(dr(d)("datastart").ToString) & "" & "\n"
                strcalendar &= "<br> <b>Fine</b>: " & FormatDateTime(SharedRoutines.EscapeMySql(dr(d)("dataend").ToString)) & "" & "\n"
                strcalendar &= "<br> <b>Docente</b>: " & SharedRoutines.EscapeMySql(dr(d)("docente").ToString) & "\n"
                strcalendar &= " <br><b>Recapito </b>: " & SharedRoutines.EscapeMySql(dr(d)("tel").ToString) & " " & SharedRoutines.EscapeMySql(dr(d)("email").ToString) & "" & "<br></h3>"

                strcalendar &= "Note:" & SharedRoutines.EscapeMySql(dr(d)("notenewpublic").ToString)

                scriptcalendar &= "      { " & vbCrLf &
                       "id: " & dr(d)("id") & ", " & vbCrLf &
        "title: '" & dr(d)("tipo") & " - " & Replace(dr(d)("nomesessione").ToString, "'", "\'") & " - Materia: " & EscapeMySql(dr(d)("materia")) & " - Classe: " & EscapeMySql(dr(d)("description")) & " - Docente: " & SharedRoutines.EscapeMySql(dr(d)("docente").ToString) & " " & "  ', " & vbCrLf &
        "start: '" & Convert.ToDateTime(dr(d)("datastart")).ToString("s") & "Z', " &
         "end: '" & Convert.ToDateTime(dr(d)("dataend")).ToString("s") & "Z', " &
            "flaginvia: '" & dr(d)("flaginvia") & "', " &
             "data: '" & Convert.ToDateTime(dr(d)("datastart")) & " - " & Convert.ToDateTime(dr(d)("dataend")) & "', " &
             "joinurl: '" & dr(d)("joinurl") & "', " &
              "tipo: '" & dr(d)("tipo") & "', " &
        "nomecorso: '" & Replace(dr(d)("nomesessione").ToString, "'", "\'") & "', " &
                 "note: '" & strcalendar & "', " & vbCrLf


                Dim colorprenotazione As String = String.Empty
                Try
                    colorprenotazione = utility.hitsession(dr(d)("tipo").ToString, dr(d)("ifwrite").ToString, dr(d)("id").ToString, Session("iduser"))
                Catch e As Exception
                    colorprenotazione = "backgroundColor: 'red'"
                End Try

                scriptcalendar &= colorprenotazione

                scriptcalendar &= "  },"


            Next

            scriptcalendar = scriptcalendar.Remove(scriptcalendar.Length - 1, 1)
            scriptcalendar &= "    ], timeFormat: 'HH:mm' " & vbCrLf &
                                 "  });" & vbCrLf

            scriptcalendar &= " }" & vbCrLf






            Return scriptcalendar

        Catch ex As Exception

            Return ex.ToString
        End Try
        Return False

    End Function



End Class

