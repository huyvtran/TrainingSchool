<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSsw.Master" CodeBehind="WFDashAdmin.aspx.vb" Inherits="TrainingSchool.WFDashAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="titoloSezione text-center">
        <h1>Calendario attività &nbsp;
				<button class="btn btn-primary">
                    <a href="WFnewActivity.aspx">Crea nuova attività&nbsp;&nbsp;<span class="oi oi-plus"></span>
                    </a>
                </button>
        </h1>
    </div>
    <div class="container">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">


                <div id='calendar-container'>
                    <div id='calendar'></div>
                </div>

            </div>
        </div>
    </div>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');

            var calendar = new FullCalendar.Calendar(calendarEl, {

                lang:'it',
                plugins: ['interaction', 'dayGrid', 'timeGrid', 'list'],
                height: 'parent',
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
                },
                defaultView: 'dayGridMonth',
                defaultDate: new Date(),
                navLinks: true, // can click day/week names to navigate views
                editable: true,
                eventLimit: true, // allow "more" link when too many events
                    events: {
                        url: 'AdminAjaxLMS.aspx?op=modsessioni&oper=getcalendar',
                        extraParams: function () { // a function that returns an object
                            return {
                                dynamic_value: Math.random()
                            };
                        }
                },
                dayClick: function (date, jsEvent, view) {

                    alert('Clicked on: ' + date.format());

                    alert('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);

                    alert('Current view: ' + view.name);

                    // change the day's background color just for fun
                    $(this).css('background-color', 'red');

                },
                eventClick: function (calEvent) {

                    
                   

                    // change the day's background color just for fun
                    $(this).css('background-color', 'red');

                }
            });

            calendar.render();
        });
    </script>
</asp:Content>
