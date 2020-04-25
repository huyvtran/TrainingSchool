<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UCCalendar.ascx.vb" Inherits="TrainingSchool.UCCalendar" %>
<%@ Register TagPrefix="uc2" TagName="course" Src="~/UCViewCertificate.ascx" %>


<style>
    .fc-event {
        border: solid 1px black;
    }
</style>



<div class="page-content">
    <div class="page-header">
        <h1>Calendario Lezioni
              
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
        <a href="WFCalendar.aspx">Visualizza calendario Annuale</a>
    </div>
    <div id="dialog-docsession" class="hide">
        <div class="form-horizontal">

            <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right" for="UDoc">File da caricare </label>

                <div class="col-sm-9">
                    <span class="btn btn-success fileinput-button">
                        <i class="glyphicon glyphicon-plus"></i>
                        <span>Scegli file...</span>
                        <!-- The file input field used as target for the file upload widget -->
                        <input id="fileupload" type="file" name="files" />
                        <input id="iddoc" type="hidden" />

                        <input id="nomefile" type="hidden" />
                    </span>
                    <br>
                    <br>
                    <!-- The global progress bar -->
                    <div id="progress" class="progress">
                        <div class="progress-bar progress-bar-success"></div>
                    </div>
                    <!-- The container for the uploaded files -->

                    <br>
                </div>
            </div>



        </div>
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
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="hide" id="divmaterial">


                                    <table id="jqgrid_document"></table>


                                </div>


                                <div class="space-12"></div>

                                <div class="hide" id="divtest">

                                    <table id="jqgrid_teststudents"></table>

                                </div>

                            </div>
                        </div>




                    </div>

                </div>


                <!-- /.modal-body -->

                <!-- /.modal-footer -->
            </div>

            <div class="modal-footer">
                <div class="pull-right">
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

      


         $('#modal-webinar').on('hidden.bs.modal', function () {

              
             
        });

         $('#modal-webinar').on('show.bs.modal', function () {

             
          
        });

        $('#modal-event').on('shown.bs.modal', function () {
            widthmodal = $("#modal-event").width() - 30;

            resizeJqGridWidth("jqgrid_document", "modal-event .modal-dialog", widthmodal)
            resizeJqGridWidth("jqgrid_teststudents", "modal-event .modal-dialog", widthmodal)
            resizeJqGridWidth("jqgrid_coursestudents", "modal-event .modal-dialog", widthmodal)

        });

        $('#fileupload').fileupload({

            replaceFileInput: true,

            dataType: 'json',

            url: "HUpload.ashx?load=loaddocstudents",

            dataType: 'json',
            add: function (e, data) {
                $('#progress .progress-bar').css(
                    'width',
                    '0%'
                );
                $('.ui-dialog-buttonset').empty();
                data.context = $('<button/>').attr({ type: 'button' }).text('Invia').addClass('btn btn-primary ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only')
                    .appendTo(".ui-dialog-buttonset")
                    .click(function () {
                        data.context = $('<p/>').text('Caricando...').replaceAll($(this));

                        data.formData = { iddoc: $("#iddoc").val(), iduser: iduser, idsessione: $("#idsessione").val(), nomefile: $("#nomefile").val() };
                        data.submit();



                    });
            },
            error: function (e, data) {
                data.context = $('<p/>').text('Caricando terminato').replaceAll($(this));
                jQuery("#jqgrid_document").setGridParam({
                    fromserver: true,
                    datatype: 'json',
                    url: 'adminAjaxLMS.aspx?op=modprenotazioni&oper=getstudentdoc&idsessione=' + $("#idsessione").val(),
                }).trigger('reloadGrid', [{ page: 1 }]);

            },
            success: function (e, data) {

                data.context.text('Caricamento Terminato..');
                jQuery("#jqgrid_document").setGridParam({
                    fromserver: true,
                    datatype: 'json',
                    url: 'adminAjaxLMS.aspx?op=modprenotazioni&oper=getstudentdoc&idsessione=' + $("#idsessione").val(),
                }).trigger('reloadGrid', [{ page: 1 }]);
            },
            done: function (e, data) {

                data.context.text('Caricamento Terminato..');
                jQuery("#jqgrid_document").setGridParam({
                    fromserver: true,
                    datatype: 'json',
                    url: 'adminAjaxLMS.aspx?op=modprenotazioni&oper=getstudentdoc&idsessione=' + $("#idsessione").val(),
                }).trigger('reloadGrid', [{ page: 1 }]);
            },
            progressall: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('#progress .progress-bar').css(
                    'width',
                    progress + '%'
                );
            }
        });


        renderCalendar();
    });



    $("#showbuttonprenota").on("click", function (e) {
        e.preventDefault();


        var urlsave = "AdminAjaxLMS.aspx?op=modprenotazioni&oper=bookingstudent";

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

    function btloaddoc(nomefile, iddoc, idsessione) {

        $("#iddoc").val(iddoc);
        $("#idsessione").val(idsessione);
        $("#nomefile").val(nomefile);

        var dialog = $("#dialog-docsession").removeClass('hide').dialog({
            modal: true,
            title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='ace-icon fa fa-check'></i>  </h4></div>",
            height: 600,
            width: 700,

            title_html: true,

            buttons: [
                {
                    text: "Salva",
                    "class": "btn btn-minier",
                    click: function () {



                        $(this).dialog("close");
                        //   location.reload();


                    }
                },
                {

                    text: "Annulla",
                    "class": "btn btn-primary btn-minier",
                    click: function () {

                        $(this).dialog("close");

                    }
                }

            ]

        });
    }

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

    function getteststudents(idsessione) {


        $("#idsessione").val(idsessione);




        $("#jqgrid_teststudents").setGridParam({
            fromserver: true,
            datatype: 'json',
            url: 'AdminAjaxLMS.aspx?op=modsessioni&oper=getcoursestudentssingles&type=course&idsessione=' + idsessione,
        }).trigger('reloadGrid', [{ page: 1 }]);



        jQuery("#jqgrid_teststudents").jqGrid({
            height: 'auto',
            autowidth: true,
            shrinkToFit: true,
            url: 'AdminAjaxLMS.aspx?op=modsessioni&oper=getcoursestudentssingles&type=course&idsessione=' + idsessione,
            datatype: "json",
            emptyDataText: 'Nessun record da visualizzare, ripetere la ricerca',
            colNames: ['', 'ID', 'STATO ', 'DATA ISCRIZIONE', 'DATA COMPLETAMENTO', 'PUNTEGGIO'],
            colModel: [

                { name: 'act', index: 'act', exportcol: false, width: 240, sorttype: "int", editable: false },
                { name: 'id', index: 'id', key: true, hidden: true, width: 20, sorttype: "int", editable: false },
                { name: 'percentuale', index: 'percentuale', width: 90, sortable: true, editable: true, editoptions: { size: "100", maxlength: "200" } },
                { name: 'date_inscr', index: 'date_inscr', width: 100, sortable: true, editable: true, editoptions: { size: "100", maxlength: "200" } },
                { name: 'date_complete', index: 'date_complete', width: 100, sortable: true, editable: true, editoptions: { size: "100", maxlength: "200" } },
                { name: 'media', index: 'media', width: 80, sortable: true, editable: true, editoptions: { size: "100", maxlength: "200" } },


            ],
            rownumbers: true,
            rowList: [],        // disable page size dropdown
            pgbuttons: false,     // disable page control like next, back button
            pgtext: null,
            loadonce: true,
            navOptions: { reloadGridOptions: { fromServer: true } },
            sortname: 'date_complete',
            ignoreCase: true,
            sortorder: "desc",
            autowidth: true,
            viewsortcols: [true, 'vertical', true],
            viewrecords: true,
            loadComplete: function (data) {
                var noth = true;
                var table = this;
                setTimeout(function () {
                    styleCheckbox(table);
                    updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);

                }, 0);

                var obj;

                if (data.rows != undefined) {
                    obj = data.rows;
                } else if (data.length != undefined || data.length != 0) {

                    if (data.length > 0) {

                        obj = data;

                    } else {

                        noth = false;

                    }
                }

                if (noth) {

                    for (var i = 0; i < obj.length; i++) {

                        var rowData = obj[i];

                        link = "<a target='blank' class='btn btn-primary' href='WfcourseRoom.aspx?course=" + rowData.idcourse + "&name=" + rowData.nominativo + "'> <i class='ace icon-eye'></i>" + rowData.nominativo + "</a>";
                        jQuery("#jqgrid_teststudents").jqGrid('setRowData', rowData.id, { act: link });

                    }
                }
            },


            editurl: "GetJson.aspx?op=modtest",//nothing is saved
            caption: "DA COMPLETARE",

            //grouping: true,
            //groupingView: {
            //    groupField: ['category'],
            //    groupText: ['<b>{0} </b>'],
            //    groupCollapse: false
            //},
        });

    }


    function changeview(tipo, joinurl, id, write, start, end) {

        today = new Date();

        $("#joinurl").val(joinurl);
        $("#idsessione").val(id);
        if (check_date(start, today)) $("#modal-event").modal({ backdrop: 'static', keyboard: false }, 'show');
        $("#showbuttonprenota").addClass("hide")
        moment.locale('it');
        $('input[name="datastart"]').val(start.format('DD/MM/YYYY H:mm') + ' - ' + end.format('H:mm'));
        $("#webinar").empty();
        $('#divtest').addClass('hide'); $('#divcourse').addClass('hide'); $('#divmaterial').addClass('hide');

        if (tipo == 'LEZIONE') {
            $('#divmaterial').removeClass('hide'); getstudentdoc(id);

        } else if (tipo == 'CORSO') {
            $('#divtest').removeClass('hide');
            getteststudents(id);

        } else if (tipo == 'WEBINAR') {
            attivawebinar(joinurl); $('.didattica').removeClass('hide'); $('#divmaterial').removeClass('hide'); getstudentdoc(id);

        } else if (tipo == 'VERIFICA SCRITTA') {
            attivawebinar(joinurl); $('.didattica').removeClass('hide');

            $('input[name=write][value=1]').attr('checked', true);

            if (write == '1') {

                $('#divtest').addClass('hide'); $('#divmaterial').removeClass('hide'); getstudentdoc(id);

            } else {

                $('#divmaterial').addClass('hide'); $('#divtest').removeClass('hide'); getteststudents(id);
            }

        } else if (tipo == 'VERIFICA ORALE') {
            attivawebinar(joinurl);

        } else if (tipo == 'RICEVIMENTO') {
            $("#showbuttonprenota").removeClass("hide")
            attivawebinar(joinurl);
        }

    }


    function attivawebinar(joinurl) {
        var url = "window.open('api.html?idsessione=" + $("#idsessione").val() + "&iduser=<%=Session("iduser")%>&g=<%=Session("idprofile")%>&fullname=<%=Session("fullname")%>&joinurl=" + joinurl + "');"

        $('#webinar').empty(); $('#webinar').removeClass('hide');
        $('<a>', {
            text: 'Avvia webinar',
            title: 'Avvia  webinar',
            class: 'btn btn-success btn-sm',
            target: 'blank',
            onClick: url,
        }).appendTo('#webinar');


    }

    function getstudentdoc(idsessione) {



        jQuery("#jqgrid_document").setGridParam({
            datatype: 'json',
            formServer: true, url: 'adminAjaxLMS.aspx?op=modprenotazioni&oper=getstudentdoc&idsessione=' + idsessione,
        }).trigger('reloadGrid', [{ page: 1 }]);



        jQuery("#jqgrid_document").jqGrid({

            url: 'adminAjaxLMS.aspx?op=modprenotazioni&oper=getstudentdoc&idsessione=' + idsessione,
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
            editurl: "adminAjaxLMS.aspx?op=modsessioni",//nothing is saved
            caption: "MATERIALE DA SCARICARE"

        });







    }

    function openfile(pathfile, iddoc, idsessione, iduser) {

        var pathfile = root + '/' + idsessione + '/' + iddoc + '/' + pathfile;

        $.ajax({
            url: 'adminAjaxLMS.aspx?op=modsessioni&oper=hitdoc',
            type: 'POST',
            data: { iduser: iduser, iddoc: iddoc },
            success: function (data) {
                window.open(pathfile);
            }

        });


    }


   

</script>
