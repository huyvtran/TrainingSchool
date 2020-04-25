<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UCCalendarProfile.ascx.vb" Inherits="TrainingSchool.UCCalendarProfile" %>



<style>
    .fc-event {
        border: solid 1px black;
    }
</style>



<div class="page-content">
    <div class="page-header">
        <h1>Calendario incontri dirigente
              
        </h1>

    </div>

    <div class="row">
        <span style="color: red" class="msgerror"></span>
        <div id="lcalendar" runat="server">
        </div>
        <div id="divday" style="display: none" runat="server">
            <div class="fc-toolbar">
                <div class="fc-left"></div>

                <div class="fc-right">
                    <div class="fc-button-group">
                        <button type="button" id="btncalendar" class="fc-month-button ui-button ui-state-default ui-corner-left ui-state-active">Calendario</button>
                    </div>
                </div>

            </div>
        </div>
        <br />
      
    </div>
    

    <div id="modal-event" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel">Evento</h4>
                </div>
                <!-- /.modal-header -->
                <div class="modal-body">



                    <div class="box-content">
                        <div class="row">

                            <div class="col-xs-12">


                                <div id="divcalendar" class="well lg">
                                </div>

                            </div>


                        </div>
                      


                    </div>

                </div>


                <!-- /.modal-body -->

                <!-- /.modal-footer -->
            </div>

            <div class="modal-footer">
                <div class="prenotati pull-right">
                    <button id="showbuttonprenota" class="btn btn-primary btn-sm">Prenotati!</button>
                </div>
                <div id="webinar" class="pull-left"></div>
            </div>

        </div>

    </div>

</div>



<input id="idsessione" type="hidden" />

<script src="assets/js/jqGrid/jquery.jqGrid.min.js"></script>
<script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>

<script type="text/javascript" src="//cdn.jsdelivr.net/momentjs/latest/moment-with-locales.min.js"></script>
<script type="text/javascript" src="//cdn.jsdelivr.net/bootstrap.daterangepicker/2/daterangepicker.js"></script>
<link rel="stylesheet" type="text/css" href="//cdn.jsdelivr.net/bootstrap.daterangepicker/2/daterangepicker.css" />


<script src="assets/js/codeupload/jquery.iframe-transport.js"></script>
<script src="assets/js/codeupload/jquery.fileupload.js"></script>

<script type="text/javascript">






    var widthmodal;


    var root ='<%=Session("lmscontentpathrel")%>';
    var iduser =<%=Session("iduser")%>;


    jQuery(function ($) {

        renderCalendar();
    });



    $("#showbuttonprenota").on("click", function (e) {
        e.preventDefault();


        var urlsave = "AdminAjaxLMS.aspx?op=modprenotazioniboss&oper=bookingstudent";

        var request4 = $.ajax({
            type: "GET",
            url: urlsave,
            data: { iduser:<%=Session("iduser")%>, idsessione: $('#idsessione').val() },
            dataType: "json"
        });

        request4.fail(function (data) {

            ShowAlert(data.resposeText);


        });


        request4.success(function (data) {


            ShowAlert(data);


        });
    });

   
    function convert(str) {
        var date = new Date(str),
            mnth = ("0" + (date.getMonth() + 1)).slice(-2),
            day = ("0" + date.getDate()).slice(-2);
        return [day, mnth, date.getFullYear()].join("/");
    }

    function check_date(data_iniziale, data_finale) {

        var datainiziale = convert(data_iniziale);
        var datafinale = convert(data_finale);
        var arr1 = datainiziale.split("/");
        var arr2 = datafinale.split("/");

        var d1 = new Date(arr1[2], arr1[1] - 1, arr1[0]);
        var d2 = new Date(arr2[2], arr2[1] - 1, arr2[0]);

        var r1 = d1.getTime();
        var r2 = d2.getTime();

        if (r1 <= r2) return true;
        else
            return true;

    }

   

    function changeview( joinurl, id, start, end,token) {

        today = new Date();

        $("#joinurl").val(joinurl);
        $("#idsessione").val(id);

        if(token){
            $("#showbuttonprenota").addClass("hide");
            $(".prenotati").html("L'evento è stato confermato, collegati all'orario previsto alla videconferenza")
        }else{
            $("#showbuttonprenota").removeClass("hide");
        }


        if (check_date(start, today)) $("#modal-event").modal({ backdrop: 'static', keyboard: false }, 'show');

        moment.locale('it');

        $('input[name="datastart"]').val(start.format('DD/MM/YYYY H:mm') + ' - ' + end.format('H:mm'));

        $("#webinar").empty();
       
        attivawebinar(joinurl); $('.didattica').removeClass('hide'); $('#divmaterial').removeClass('hide'); getdocumenti(id);

    }


    function attivawebinar(joinurl) {
        var url = "window.open('apiboss.html?idsessione=" + $("#idsessione").val() + "&iduser=<%=Session("iduser")%>&g=<%=Session("idprofile")%>&fullname=<%=Session("fullname")%>&joinurl=" + joinurl + "');"

        $('#webinar').empty(); $('#webinar').removeClass('hide');
        $('<a>', {
            text: 'Avvia webinar',
            title: 'Avvia  webinar',
            class: 'btn btn-success btn-sm',
            target: 'blank',
            onClick: url,
        }).appendTo('#webinar');


    }

    function getdocumenti(idsessione) {



        jQuery("#jqgrid_document").setGridParam({
            datatype: 'json',
            formServer: true, url: 'adminAjaxLMS.aspx?op=modprenotazioneboss&oper=getdocumenti&idsessione=' + idsessione,
        }).trigger('reloadGrid', [{ page: 1 }]);



        jQuery("#jqgrid_document").jqGrid({

            url: 'adminAjaxLMS.aspx?op=modprenotazioneboss&oper=getdocumentistudent&idsessione=' + idsessione,
            datatype: "json",
            height: "auto",
            colNames: ['', '', 'FILE DA SCARICARE', 'DESCRIZIONE', 'LINK', '', ''],
            colModel: [

                { name: 'act', index: 'act', width: 30, hidden: true, sortable: false, resize: false },
                { name: 'id', index: 'id', width: 20, hidden: true, key: true, sortable: false, resize: false },

                { name: 'documento', index: 'documento', width: 320, editable: false, editoptions: { size: "120", maxlength: "50" } },
                { name: 'descrizione', index: 'descrizione', width: 270, editable: false, editoptions: { size: "120", maxlength: "50" } },
                { name: 'internetaddress', index: 'internetaddress', width: 270, editable: false, editoptions: { size: "120", maxlength: "50" } },
                { name: 'uploaddocstudent1', index: 'uploaddocstudent1', width: 30, editable: false, editoptions: { size: "120", maxlength: "50" } },
                { name: 'nomedocupload', index: 'nomedocupload', width: 270, editable: false, editoptions: { size: "120", maxlength: "50" } },


            ],

            rowNum: 300,
            gridview: true,
            loadonce: false,
            sortable: true,
            sortname: 'data_scadenza',
            sortorder: "desc",
            viewrecords: true,
            viewsortcols: [true, 'vertical', true],
            loadComplete: function (data) {
                doccaricati = 0;
                var table = this;

                setTimeout(function () {
                    styleCheckbox(table);

                    updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);

                var obj;

                if (data.length > 0) {
                    obj = data;
                } else {
                    obj = data.rows;
                }

                if (!(typeof obj === 'undefined'))

                    for (var i = 0; i < obj.length; i++) {
                        var rowData = obj[i];


                        var nomedocumento = rowData.nomedoc;


                        var path = root + '/' + rowData.idsessione + '/' + rowData.id;
                        ft = "<a target='blank'onClick=\'openfile(\"" + nomedocumento + "\"," + rowData.id + "," + rowData.idsessione + "," + iduser + ");\' ><i class='icon-file bigger-160' ></i> " + nomedocumento + "</a>";
                        jQuery("#jqgrid_document").jqGrid('setRowData', rowData.id, { documento: ft });


                        if (rowData.uploaddocstudent) {


                            ft = "<button  onclick='btloaddoc(\"" + nomedocumento + "\"," + rowData.id + "," + rowData.idsessione + ");' type='button' class='btn btn-primary btn-sm'>Carica</button>";

                            jQuery("#jqgrid_document").jqGrid('setRowData', rowData.id, { uploaddocstudent1: ft });

                        }



                        if (!(rowData.nomedocupload == null)) {

                            fi = "<a target='blank' href='"+ path + "/" + rowData.nomedocupload + "' ><i class='icon-file bigger-160' ></i> " + rowData.nomedocupload + "</a>";
                            jQuery("#jqgrid_document").jqGrid('setRowData', rowData.id, { nomedocupload: fi });
                        }





                        if (rowData.internetaddress != '') {
                            fi = "<a target='blank' href='" + rowData.internetaddress + "' ><i class='icon-html bigger-160' ></i> " + rowData.internetaddress + "</a>";
                            jQuery("#jqgrid_document").jqGrid('setRowData', rowData.id, { internetaddress: fi });
                        }


                    }



            },
            editurl: "adminAjaxLMS.aspx?op=modsessioniboss",//nothing is saved
            caption: "MATERIALE DA SCARICARE"

        });







    }

    function openfile(pathfile, iddoc, idsessione, iduser) {

        var pathfile = root + '/' + idsessione + '/' + iddoc + '/' + pathfile;

        $.ajax({
            url: 'adminAjaxLMS.aspx?op=modsessioniboss&oper=hitdoc',
            type: 'POST',
            data: { iduser: iduser, iddoc: iddoc },
            success: function (data) {
                window.open(pathfile);
            }

        });


    }


   

</script>
