<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UCCalendarBoss.ascx.vb" Inherits="TrainingSchool.UCalendarBoss" %>

<style>


</style>
<div class="page-content">
    <div class="page-header">
        <h1>Calendario Incontri </h1>
    </div>


    <div class="row">
        <div class="col-xs-12">


            <button id="btnnewevent" type="button" class="btn btn-primary btn-sm"><i class="ace icon glyphicon-plus">Nuovo Evento</i></button>


            <span style="color: red" class="msgerror"></span>
            <div id="lcalendar" runat="server">
            </div>



        </div>
    </div>
</div>



<script type="text/javascript">


    renderCalendar();

    var widthmodal;

    $(document).ready(function () {


    });


</script>

<input type="hidden" id="idsessione" />



<div id="modal-event" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="myModalLabel2"></h4>
            </div>
            <!-- /.modal-header -->
            <div class="modal-body">

                <div class="box-content">
                    <div class="form-horizontal">

                        <fieldset class="scheduler-border ">
                            <legend class="scheduler-border">Evento</legend>

                            <div id="fieldsetevento">
                                <div class="form-group">
                                    <label for="selsedi" class="col-sm-3 control-label no-padding-right">Data Evento *</label>
                                    <div class="col-sm-9">

                                        <input type="text" autocomplete="off" class="col-sm-6" name="datastart" value="" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="nomecorso" class="col-sm-3 control-label no-padding-right">Nome evento *</label>
                                    <div class="col-sm-9">

                                        <input type="text" class="col-sm-6" name="nomecorso" id="nomecorso" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="prfile" class="col-sm-3 control-label no-padding-right">Destinatari *</label>
                                    <div class="col-sm-9">
                                        <span class="input-icon  input-icon-right">
                                            <select name="profile" id="profile">

                                                <option selected="selected" value="2">Personale docente</option>
                                                <option value="5">Segreteria</option>
                                                <option value="3">Studenti/genitori</option>

                                            </select></span>
                                    </div>
                                </div>



                                <div class="form-group">
                                    <label for="maxposti" class="col-sm-3 control-label no-padding-right">Posti disponibili *</label>
                                    <div class="col-sm-9">

                                        <input type="number" class="col-sm-6" name="maxposti" id="maxposti" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="notesess" class="col-sm-3 control-label no-padding-right">Note Personali</label>
                                <div class="col-sm-9">

                                    <input type="text" class="col-sm-6" name="notemod" id="notenew" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="notesess" class="col-sm-3 control-label no-padding-right">Note Pubbliche</label>
                                <div class="col-sm-9">

                                    <input type="text" class="col-sm-6" name="notemodpublic" id="notenewpublic" />
                                </div>
                            </div>

                        </fieldset>


                        <div class="space-12"></div>


                        <div id="divprenotati" class="hide">

                            <table id="jqgrid_student"></table>


                        </div>
                        <div class="space-12"></div>

                        <div class=" pull-left">
                        </div>






                    </div>
                </div>

            </div>
            <!-- /.modal-body -->
            <div class="modal-footer">
                <div class="pull-left">
                    <button type="button" id="btndeletesession" class="btn btn-danger btn-sm"><i class="ace icon-hand"></i>Cancella evento</button>
                    &nbsp;
                    <button type="button" id="btninviacomunicazione" class="btn btn-warning  btn-sm"><i class="ace icon-envelope"></i>Invia Avviso Email</button>
                    &nbsp;
                </div>
                <div id="webinar" class="pull-left">
                </div>
                <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Chiudi</button>
                &nbsp;
                <input type="button" id="btnsessione" class="btn btn-primary hide btn-sm" value="Salva" />
                &nbsp;
                <input type="button" id="btnmodifica" class="btn btn-primary hide btn-sm" value="Modifica" />
            </div>
            <!-- /.modal-footer -->
        </div>
        <!-- /.modal-content -->
    </div>
</div>










<script src="assets/js/jqGrid/src/jquery.jqGrid.js"></script>
<script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>

<script type="text/javascript" src="//cdn.jsdelivr.net/momentjs/latest/moment-with-locales.min.js"></script>
<script type="text/javascript" src="//cdn.jsdelivr.net/bootstrap.daterangepicker/2/daterangepicker.js"></script>
<link rel="stylesheet" type="text/css" href="//cdn.jsdelivr.net/bootstrap.daterangepicker/2/daterangepicker.css" />

<script src="assets/js/codeUpload/jquery.iframe-transport.js"></script>
<script src="assets/js/codeUpload/jquery.fileupload.js"></script>


<script type="text/javascript" language="javascript" src="//cdn.rawgit.com/bpampuch/pdfmake/0.1.26/build/pdfmake.min.js">	</script>
<script type="text/javascript" language="javascript" src="//cdn.rawgit.com/bpampuch/pdfmake/0.1.26/build/vfs_fonts.js"></script>
<script type="text/javascript" language="javascript" src="//cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>

<script>


    var root;
    var thisiduser;


    jQuery(function ($) {

        root ='<%=Session("lmscontentpathrel")%>';
        thisiduser =<%=Session("iduser")%>;
       
       



        $('#btnnewevent').click(function () {
            moment.locale('it');

            $('input[name="datastart"]').daterangepicker({
                timePicker: true,
                timePicker24Hour: true,
                timePickerIncrement: 30,
                locale: {

                    "separator": "/",
                    "applyLabel": "Applica",
                    "cancelLabel": "Cancella",
                    "fromLabel": "DE",
                    "toLabel": "HASTA",
                    "customRangeLabel": "Custom",
                    "daysOfWeek": [
                        "Dom",
                        "Lun",
                        "Mar",
                        "Mer",
                        "Gio",
                        "Ven",
                        "Sab"
                    ],
                    monthNames: [
                        "Gen",
                        "Feb",
                        "Marzo",
                        "Apr",
                        "Mag",
                        "Giu",
                        "Lug",
                        "Ago",
                        "Set",
                        "Ott",
                        "Nov",
                        "Dic"
                    ],
                    "firstDay": 1
                }
            });

            $('input[name="datastart"]').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('DD/MM/YYYY H:mm') + ' - ' + picker.endDate.format('DD/MM/YYYY H:mm'));
            });

            $('input[name="datastart"]').on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
            });


            
            $(".modal-title").text("");
            $('#modal-event').find('input:text').val('');
            $("#modal-event").modal({ backdrop: 'static', keyboard: false }, 'show');
            $("#idsessione").val(0);
            $(".didattica").addClass("hide");
            $("#btnsessione").removeClass("hide");
            $("#btnmodifica").addClass("hide");
        });


        $('#btnsessione').click(function () {


            if ($('#nomecorso').val() != '' ) {


                $.ajax({
                    url: 'adminAjaxLMS.aspx?op=modsessioniboss',
                    type: 'POST',
                    data: { idsessione: $("#idsessione").val(), datastart: $('input[name="datastart"]').val(),idprofile:$("#profile option:selected").val(),  maxposti: $("#maxposti").val(), nomecorso: $('#nomecorso').val(), notenew: $('#notenew').val(), notenewpublic: $('#notenewpublic').val(), oper: 'add2' },
                    datatype: 'json',
                    success: function (data) {
                        joinurl = data.split(";")[1];
                        data = data.split(";")[0];

                        if (data <= 0) {
                            alert("Errore inserimento evento! Controllare le date!");
                            return false;
                        }

                        $('#fieldsetevento *').prop('disabled', false);
                        $("#idsessione").val(data);
                        
                        $('#divprenotati').removeClass('hide'); getbooking(data); attivawebinar(joinurl);
                       
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        ShowAlert(jqXHR.responseText);
                        location.reload();

                    }
                });
            } else {
                alert("Inserire i campi obbligatori!")
            }

        });


        $("#btndeletesession").on("click", function (e) {
            e.preventDefault();
            var b;

            bootbox.confirm({
                message: "Desideri cancellare l'evento?",
                buttons: {
                    confirm: {
                        label: 'Si',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'No',
                        className: 'btn-danger'
                    }
                },
                callback: function (result) {
                    if (result) {
                        var urlsave = "AdminAjaxLMS.aspx?op=modsessioniboss";

                        var request4 = $.ajax({
                            type: "POST",
                            url: urlsave,
                            data: { id: $('#idsessione').val(), oper: 'del' },
                            dataType: "json"
                        });

                        request4.fail(function (data) {



                            location.reload();
                        });


                        request4.success(function (data) {


                            location.reload();
                        });

                    }
                }
            });


        });

        $('#btninviacomunicazione').click(function () {
            $('#fieldsetevento *').prop('disabled', false);
            $.ajax({
                url: 'adminAjaxLMS.aspx?op=modprenotazioniboss&oper=inviacomunicazioneboss',
                type: 'POST',
                data: { idsessione: $("#idsessione").val(), idcategory: $("#jclasse option:selected").val(), format: 'mailformat' },
                datatype: 'json',
                success: function (data) {
                    ShowAlert(data);

                    getbooking($("#idsessione").val());

                    $('#fieldsetevento *').prop('disabled', true);
                },
                error: function (data, textStatus, errorThrown) {
                    ShowAlert(data);

                }
            });
        });
        $('#btnmodifica').click(function () {

            $.ajax({
                url: 'adminAjaxLMS.aspx?op=modsessioniboss',
                type: 'POST',
                data: { idsessione: $('#idsessione').val(), notenew: $("#notenew").val(), notenewpublic: $("#notenewpublic").val(), oper: 'edit' },
                datatype: 'json',
                success: function (data) {
                    ShowAlert(data);

                },
                error: function (data, textStatus, errorThrown) {
                    ShowAlert(data);

                }
            });



        });
       

       

        $('#modal-event').on('hidden.bs.modal', function () {
            document.location.reload()
        })


        $('#modal-event').on('shown.bs.modal', function () {


            resizeJqGridWidth("jqgrid_student", "modal-event .modal-dialog", widthmodal)
           
            resizeJqGridWidth("jqgrid_prenota", "modal-event .modal-dialog", widthmodal);
             

            widthmodal = $("#modal-event").width() - 30;


        })

        


    });

    function booking() {


        s = jQuery("#jqgrid_prenota").jqGrid('getGridParam', 'selarrrow');

        $.ajax({
            url: 'adminAjaxLMS.aspx?op=modprenotazioniboss&idsessione=' + $("#idsessione").val() + '&tipo=' + $("#typetable").val() + '&oper=prenota&ids=' + s,
            type: 'GET',
            datype: 'json',
            success: function (data) {

                $('#modal-prenota').modal('hide');

                if ($("#typetable").val() == 2) {
                    getteststudents($("#idsessione").val());
                } else {
                    getdocumenti($("#idsessione").val());
                }


                ShowAlert(data);


            },
            error: function (jqXHR, textStatus, errorThrown) {

                $('#modal-prenota').modal('hide');
                ShowAlert(jqXHR.responseText)
            }
        });

    }



    function changeview( joinurl, id, write, start, end, iduser) {

        $("#btnattendanceall").hide();
        $("#idsessione").val(id);
       

        moment.locale('it');


        $('input[name="datastart"]').val(start.format('DD/MM/YYYY H:mm') + ' - ' + end.format('H:mm'));


        $("#webinar").empty();
        
        attivawebinar(joinurl);  $('#divprenotati').removeClass('hide'); getbooking(id);
         

        $('#fieldsetevento *').prop('disabled', true);

        $("#modal-event").modal({ backdrop: 'static', keyboard: false }, 'show');

        getbooking(id);

    }


    function attivawebinar(joinurl) {
        var url = "window.open('apiboss.html?idsessione=" + $("#idsessione").val() + "&iduser=<%=Session("iduser")%>&g=<%=Session("idprofile")%>&cfg=0&fullname=<%=Session("fullname")%>&joinurl=" + joinurl + "');"

        $('#webinar').empty(); $('#webinar').removeClass('hide');
        $('<a>', {
            text: 'Avvia webinar',
            title: 'Avvia  webinar',
            class: 'btn btn-success btn-sm',
            target: 'blank',
            onClick: url,
        }).appendTo('#webinar');




        $('<a>', {
            text: 'Apri registrazione',
            title: 'Apri  registrazione',
            class: 'btn btn-danger btn-sm',
            target: 'blank',
            href: 'http://51.107.87.53/recorder/' + joinurl + '.mp4',
        }).appendTo('#webinar');
    }

    function getbooking(idsessione) {

        $("#idsessione").val(idsessione);


        $("#jqgrid_student").setGridParam({
            fromserver: true,
            datatype: 'json',
            url: 'adminAjaxLMS.aspx?op=modprenotazioniboss&oper=getbooking&idsessione=' + idsessione,
        }).trigger('reloadGrid', [{ page: 1 }]);


        jQuery("#jqgrid_student").jqGrid({
            url: 'adminAjaxLMS.aspx?op=modprenotazioniboss&oper=getbooking&idsessione=' + idsessione,
            datatype: "json",
            height: 'auto',
            autowidth: true,
            shrinkToFit: true,
            width: widthmodal,
            colNames: ['', '', '', 'NOME', 'COGNOME', 'DATA PRENOTAZIONE'],
            colModel: [

                { name: 'act', index: 'act', width: 30, hidden: false, sortable: false, resize: false },
                { name: 'id', index: 'id', width: 20, hidden: true, editable: false },
                { name: 'token', index: 'token', width: 20, editable: false },
                { name: 'firstname', index: 'firstname', width: 120, editable: false, editoptions: { size: "120", maxlength: "50" } },
                { name: 'lastname', index: 'lastname', width: 120, editable: false, editoptions: { size: "120", maxlength: "50" } },
                { name: 'data_prenotazione', width: 120, index: 'date_prenotazione' },


            ],
            gridview: true,
            loadonce: true,
            sortable: true,
            viewrecords: true,
            loadComplete: function (data) {

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


                        //$("#" + rowData.id).find("td").css("background-color", "rgba(255,123,0,0.1)");

                        de = "<div  style='float:left;margin-left:5px;' class='ui-pg-div ui-inline-del' id='jDeleteButton_'" + rowData.id + "' onclick=\"CancellazionePrenotazione(" + rowData.id + "," + rowData.iduser + ");\"  ><span class='ui-icon ui-icon-trash bigger-160'></span></div>";
                        if (rowData.token) {
                            re = "<span class='label label-success arrowed-in arrowed-in-right'> confermato</span>";


                        } else {
                            re = '';
                        }

                        jQuery("#jqgrid_student").jqGrid('setRowData', rowData.id, { token: re });

                        if (rowData.flagmail) {
                            em1 = "<div  style='float:left;margin-left:5px;'  class='ui-pg-div ui-inline-del'  onclick=\"InviaComunicazioneStudente('" + rowData.iduser + "'," + rowData.id + ",0);\" ><span class='ace-icon icon-envelope bigger-160 green'> </span></div>";
                        } else {
                            em1 = "<div style='float:left;margin-left:5px;'  class='ui-pg-div ui-inline-del'  onclick=\"InviaComunicazioneStudente('" + rowData.iduser + "'," + rowData.id + ",1);\" ><span class='ace-icon icon-envelope bigger-160 red'> </span></div>";

                        }


                        if (rowData.presente) {
                            sm2 = "<div  style='float:left;margin-left:5px;'  class='ui-pg-div ui-inline-del'  onclick=\"InviaPresente('" + rowData.iduser + "'," + rowData.id + ",0);\" ><span class='ace-icon icon-graduation-cap bigger-160 green'> </span></div>";

                        } else {
                            sm2 = "<div  style='float:left;margin-left:5px;'  class='ui-pg-div ui-inline-del'  onclick=\"InviaPresente('" + rowData.iduser + "'," + rowData.id + ",1);\" ><span class='ace-icon icon-graduation-cap bigger-160 red'> </span></div>";

                        }



                        jQuery("#jqgrid_student").jqGrid('setRowData', rowData.id, { act: de + em1 + sm2 });

                    }


            },
            editurl: "adminAjaxLMS.aspx?op=modsessioniboss&oper=prenotati",//nothing is saved
            caption: "ELENCO PARTECIPANTI"

        });


    }

 

    function addbutton(grid) {

        jQuery("#jqgrid_" + grid).jqGrid('navButtonAdd', '#pager_' + grid, { // "#list_toppager_left"
            caption: " Pdf",
            buttonicon: "icon-file red",
            onClickButton: function () {

                jQuery("#jqgrid_" + grid).jqGrid("exportToPdf", {
                    title: 'Report Evento ' + $("#myModalLabel2").text(),
                    orientation: 'landscape',
                    pageSize: 'A4',
                    description: $("#myModalLabel2").text(),
                    customSettings: null,
                    download: 'download',
                    includeLabels: true,
                    includeGroupHeader: true,

                    fileName: $("#myModalLabel2").text() + ".pdf"
                })



            }, position: "last"
        });

        jQuery("#jqgrid_" + grid).jqGrid('navButtonAdd', '#pager_' + grid, { // "#list_toppager_left"
            caption: "Excel",
            buttonicon: "icon-file green",
            onClickButton: function () {

                jQuery("#jqgrid_" + grid).jqGrid("exportToExcel", {
                    title: 'Report : ' + $("#myModalLabel2").text(),
                    orientation: 'landscape',
                    pageSize: 'A4',
                    description: $("#myModalLabel2").text(),
                    customSettings: null,
                    download: 'download',
                    includeLabels: true,
                    includeGroupHeader: true,
                    includeFooter: true,
                    fileName: $("#myModalLabel2").text() + ".xls"
                })



            }, position: "last"
        });
    }

    

    function CancellazionePrenotazione(idprenotazione, iduser) {
        $('#fieldsetevento *').prop('disabled', false);
        $.ajax({
            url: 'adminAjaxLMS.aspx?op=modprenotazioniboss&oper=deletebooking',
            type: 'POST',
            data: { iduser: iduser, idprenotazione: idprenotazione, idsessione: $("#idsessione").val(), tipo: $("#tipo option:selected").val(), write: $("input[name='write']:checked").val() },
            datatype: 'json',
            success: function (data) {
                alert("Cancellazione avvenuta");
                getbooking($("#idsessione").val());
                $('#fieldsetevento *').prop('disabled', true);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Errore");

            }
        });
    }

    function InviaComunicazioneStudente(iduser, idprenotazione, flag) {
        $.ajax({
            url: 'adminAjaxLMS.aspx?op=modprenotazioniboss&oper=inviacomunicazionestudente',
            type: 'POST',
            data: { iduser: iduser, idprenotazione: idprenotazione, flagmail: flag, idsessione: $("#idsessione").val() },
            datatype: 'json',
            success: function (data) {
                ShowAlert(data);
                getbooking($("#idsessione").val());
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Errore");

            }
        });
    }


  

   
</script>

