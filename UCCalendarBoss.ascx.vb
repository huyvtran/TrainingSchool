
Imports System.Globalization
Imports TrainingSchool.SharedRoutines
Imports MySql.Data.MySqlClient

Public Class UCalendarBoss
    Inherits System.Web.UI.UserControl

    Dim dt As DataTable = Nothing



    Dim sqlstring As String = String.Empty

    Dim utility As New SharedRoutines
    Dim annosel As Integer
    Dim rconn As rconnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        rconn = CheckDatabase(rconn)
        Try

            If Me.Visible = True Then
                VisualizzaCalendari()

            End If

        Catch ex As Exception
            SharedRoutines.LogWrite(ex.ToString)
            Response.Write(ex.ToString)
        End Try

    End Sub



    Protected Sub VisualizzaCalendari()


        Try
            Dim tutte As String = String.Empty

            'If Not Request.QueryString("ac") = "tutte" Then
            '    tutte = "  b.regione like '%" & Request.QueryString("ac") & "' and "
            'End If

            Dim datastorica As String = String.Empty


            If Request.QueryString("op") = "storico" Then

                annosel = Year(Now) - 1
                datastorica = " datastart >= '" & annosel & "-01-01 00:00:00""' and  datastart <= '" & annosel & "-12-31 00:00:00""' "


            ElseIf Request.QueryString("op") = "prossimo" Then
                annosel = Year(Now) + 1
                datastorica = " datastart >= '" & annosel & "-01-01 00:00""' and  datastart <= '" & annosel & "-12-31 00:00""' "

            Else
                annosel = Year(Now)
                datastorica = " datastart >= '" & annosel & "-01-01 00:00""' and  datastart <= '" & annosel & "-12-31 00:00""' "

            End If
            Dim idcategory As String
            Try

                sqlstring = "select *,datastart,dataend,concat(UPPER(b.firstname),' ', b.lastname) as dirigente,a.nomesessione,nome from (aula_boss a join core_user b on a.iduser=b.idst) join core_profile c on b.idprofile=c.id where a.iduser=" & Session("iduser") & " and  " & tutte & "  " & datastorica & "  and visible=1 "


            Catch ex As Exception
            End Try



            'sqlstring = "select *,datastart as datastart,UPPER(b.sedi) as sessione,a.domicilio,a.flagsedi,a.idsede from aula_boss a join aula_sedi b on a.idsede=b.id where b.codice=" & Session("code") & " and " & tutte & "  " & datastorica & "  and visible=1 "
            Dim dt As DataTable = Nothing

            dt = rconn.GetDataTable(sqlstring, CommandType.Text, Nothing)

            Dim scriptcalendar As String = String.Empty





            Dim scriptjavascript As String = "<script type='text/javascript' >" & vbCrLf
            scriptjavascript &= "function renderCalendar(){" & vbCrLf


            Dim dr() As DataRow
            If dt.Rows.Count > 0 Then

                For i = 11 To 1 Step -1
                    Dim mese As String = i.ToString("00")
                    Dim d1 As String = annosel & "-" & mese & "-01 00:00"
                    Dim d2 As String = annosel & "-" & mese & "-" & DateTime.DaysInMonth(annosel, mese) & " 23:59"
                    'Dim d1 As Date = "01/" & mese & "/" & annosel & " 00:00:00"
                    'Dim d2 As Date = DateTime.DaysInMonth(annosel, mese) & "/" & mese & "/" & annosel & " 00:00:00"


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
                        'Dim d1 As Date = "01/" & mese & "/" & annosel & " 00:00:00"
                        'Dim d2 As Date = DateTime.DaysInMonth(annosel, mese) & "/" & mese & "/" & annosel & " 00:00:00"


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

        Catch ex As Exception
            LogWrite(ex.ToString)
        End Try



    End Sub





    Sub RenderDay()
    End Sub
    Function RenderCalendar(ByVal mese As String, ByVal dr() As DataRow)
        Try
            Dim i As Integer = 1
            Dim a As New Ajaxadminlms
            Dim scriptcalendar As String = String.Empty
            Dim datcorso As DateTime = FormatDateTime(dr(0)("datastart"), DateFormat.ShortDate)
            Dim today As DateTime = FormatDateTime(Now, DateFormat.ShortDate)

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
                               " displayEventEnd: true," & vbCrLf &
                                "eventLimit: false, " & vbCrLf &
                                  "defaultView: 'agendaWeek', " & vbCrLf &
     "contentHeight:  'auto'," & vbCrLf &
                                "eventClick: function(calEvent, jsEvent, view) {" & vbCrLf &
     "$(this).css('border-color', 'green'); " &
            "var modal = $('#modal-event'); changeview(calEvent.joinurl,calEvent.id,calEvent.maxposti,calEvent.start,calEvent.end,calEvent.iduser); " & vbCrLf

            scriptcalendar &= " $('#idsessione').val(calEvent.id);" & vbCrLf
            scriptcalendar &= " $('#profile').val(calEvent.idprofile);" & vbCrLf
            scriptcalendar &= " $('#maxposti').val(calEvent.maxposti);" & vbCrLf
            scriptcalendar &= " $('#nomecorso').val(calEvent.nomecorso);" & vbCrLf
            scriptcalendar &= " $('#notenew').val(calEvent.notenew);" & vbCrLf
            scriptcalendar &= " $('#notenewpublic').val(calEvent.notenewpublic);" & vbCrLf
            scriptcalendar &= " $('#joinurl').val(calEvent.joinurl);" & vbCrLf
            scriptcalendar &= "  $('#btnsessione').addClass('hide');" & vbCrLf
            scriptcalendar &= "  $('#btnmodifica').removeClass('hide');" & vbCrLf
            scriptcalendar &= " modal.find('.modal-title').html(calEvent.title); " & vbCrLf
            ' scriptcalendar &= " If (calEvent.flaginvia =='1') {   $('#btninvia').show();        } else {               $('#btninvia').hide();            }" & vbCrLf
            '  scriptcalendar &= "if(calEvent.iduser ==" & Session("iduser") & ") " & vbCrLf





            scriptcalendar &= " }, " & vbCrLf &
                                "events: [" & vbCrLf

            For d = 0 To dr.Length - 1
                Dim flaginvia As Boolean = dr(d)("flaginvia")
                Dim idprofile As String = dr(d)("idprofile").ToString
                Dim maxposti As String = dr(d)("maxposti").ToString
                Dim joinurl As String = dr(d)("joinurl").ToString
                Dim meetingid As String = dr(d)("meetingid").ToString
                datcorso = FormatDateTime(dr(d)("datastart"), DateFormat.ShortDate)
                Dim datastartora As DateTime = FormatDateTime(dr(d)("datastart"), DateFormat.GeneralDate)
                Dim nomesessione As String = EscapeMySql(dr(d)("Nomesessione").ToString)

                Dim dirigente As String = EscapeMySql(dr(d)("dirigente").ToString)


                scriptcalendar &= "      { " & vbCrLf &
                       "id: " & dr(d)("id") & ", " & vbCrLf &
        "title: '" & nomesessione & " - " & dirigente & "  ', " & vbCrLf &
        "start: '" & Convert.ToDateTime(dr(d)("datastart")).ToString("s") & "Z', " &
         "end: '" & Convert.ToDateTime(dr(d)("dataend")).ToString("s") & "Z', " &
            "flaginvia: '" & dr(d)("flaginvia") & "', " &
             "data: '" & Convert.ToDateTime(dr(d)("datastart")) & " - " & Convert.ToDateTime(dr(d)("dataend")) & "', " &
            "meetingurl: '" & LCase(dr(d)("joinurl")) & "', " &
                   "iduser: '" & dr(d)("iduser") & "', " &
          "idprofile: '" & dr(d)("idprofile") & "', " &
            "notenew: '" & dr(d)("notenew") & "', " &
              "notenewpublic: '" & dr(d)("notenewpublic") & "', " &
            "maxposti: '" & dr(d)("maxposti") & "', " &
                 "joinurl: '" & LCase(dr(d)("joinurl")) & "', " &
        "nomecorso: '" & nomesessione & "', " & vbCrLf




                If (CInt(dr(d)("attivo").ToString) = 0) Then
                    scriptcalendar &= "    attivo: 'False'," & vbCrLf
                Else
                    scriptcalendar &= "    attivo: 'True'," & vbCrLf
                End If


                Dim colorprenotazione As String = String.Empty
                Try
                    colorprenotazione = utility.hitsessionall(dr(d)("tipo").ToString, dr(d)("ifwrite").ToString, dr(d)("id").ToString)
                Catch e As Exception
                    colorprenotazione = " backgroundColor: 'red' "
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