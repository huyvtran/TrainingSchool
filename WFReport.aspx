<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFReport.aspx.vb" Inherits="TrainingSchool.WFReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-content">
        <div class="page-header">
            <h1>Report
                <small><i class="icon-double-angle-right"></i></small>
            </h1>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-xs-12">
                        Selezione l'intervallo temporale del report
        <select id="export" class="form-control" name="export">
            <option value=""></option>
            <option selected="selected" value="01">Gennaio</option>
            <option value="02">Febbraio</option>
            <option value="03">Marzo</option>
            <option value="04">Aprile</option>
            <option value="05">Maggio</option>
            <option value="06">Giugno</option>
            <option value="07">Luglio</option>
            <option value="08">Agosto</option>
            <option value="09">Settembre</option>
            <option value="10">Ottobre</option>
            <option value="11">Novembre</option>
            <option value="12">Dicembre</option>
            <option value="0">Annuale</option>
        </select>
                    </div>
                </div>
            </div>
        </div>
        <div class="space-12"></div>
        <div class="row">
            <div class="col-xs-12">
                <div id="pager_useravailable"></div>


                <table id="jqgrid_useravailable"></table>




            </div>


        </div>
    </div>

    <!-- PAGE CONTENT ENDS -->

    <!-- page specific plugin scripts -->



    <script src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
    <script src="assets/js/jquery.ui.touch-punch.min.js"></script>
    <script src="assets/js/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>
    <script src="assets/js/codeUpload/jquery.iframe-transport.js"></script>
    <script src="assets/js/codeUpload/jquery.fileupload.js"></script>

    <script>
        jQuery(function ($) {
            $("#export").val(getParameterByName("data"));

            grid_selector = "#jqgrid_useravailable";
            pager_selector = "#pager_useravailable";

            var grid = $(grid_selector);

            jQuery(grid_selector).jqGrid({
                url: 'AdminAjaxLMS.aspx?op=moduser&oper=get&sel=userall',
                datatype: "json",
                height: 445,
                colNames: ['', 'ID', 'NOME', 'COGNOME', 'EMAIL', 'CF', 'TEL', 'RUOLO', 'DATA REGISTRAZIONE', 'ISCRIZIONI'],
                colModel: [

                          { name: 'act', index: 'act', width: 50, sortable: false, resize: false, editable: false },
                        { name: 'id', index: 'id', width: 30, sorttype: "int", editable: true },
                         { name: 'firstname', index: 'firstname', width: 100, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },
                        { name: 'lastname', index: 'lastname', width: 100, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },
                        { name: 'email', index: 'email', width: 150, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },
                        { name: 'cf', index: 'cf', width: 100, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },
                         { name: 'tel', index: 'tel', width: 100, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },
                          { name: 'profile', index: 'profile', sortable: true, width: 90, editable: true, edittype: "select", editoptions: { dataUrl: 'AdminAjaxLMS.aspx?op=moduser&oper=profile' } },

                    { name: 'datestart', index: 'datestart', width: 100, hidden: false, editable: false, editoptions: { size: "70", maxlength: "30" } },
 { name: 'iscrizionit', index: 'iscrizionit', width: 100, hidden: false, editable: false, editoptions: { size: "70", maxlength: "30" } },


                ],

                rowNum: 20,
                rowList: [20, 50, 100],
                rowList: [20, 50, 100],
                rowList: [],        // disable page size dropdown
                pgbuttons: false,     // disable page control like next, back button
                pgtext: null,
                gridview: true,
                loadonce: true,
                sortable: true,
                pager: pager_selector,
                pagerpos: "right",
                toppager: true,
                sortname: 'register_date',
                sortorder: "desc",
                autowidth: true,
                viewrecords: true,
                multiselect: true,
                viewsortcols: [true, 'vertical', true],
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
                    for (var i = 0; i < obj.length; i++) {

                        var rowData = obj[i];

                        ce = "<a target='_blank' class='btn btn-primary' href='AdminAjaxLMS.aspx?op=modsessioni&oper=getreportzip&data=" + getParameterByName("data") + "&iduser=" + rowData.id + "'  title='Download Report'  > <span class='ui-icon icon-file white'></span></a>";

                        jQuery("#jqgrid_useravailable").jqGrid('setRowData', rowData.id, { act: ce });

                        fe = "<div  title='Iscrizioni ai corsi' style='float:left;margin-left:5px;' class='ui-pg-div ui-inline-search'   onclick=\"ViewModalObj('WfReportByuser.aspx?iduser=" + rowData.id + "&nominativo='" + rowData.firstname + " " + rowData.lastname + "'','Report');\"  > <i class='ui-icon icon-graduation-cap green'></i>(" + rowData.iscrizioni + ")</div>";

                        jQuery("#jqgrid_useravailable").jqGrid('setRowData', rowData.id, { iscrizionit: fe });


                    }


                },

                editurl: "AdminAjaxLMS.aspx?op=moduser",//nothing is saved
                caption: "Elenco Report"

            });



            //$(window).on('resize.jqGrid', function () {
            //    $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
            //})





            //navButtons
            jQuery(grid_selector).jqGrid('navGrid', pager_selector,
                { 	//navbar options

                    edit: true,
                    editicon: 'icon-pencil blue',
                    add: true,
                    addicon: 'icon-plus-sign purple',
                    del: true,
                    delicon: 'icon-trash red',
                    search: true,
                    searchicon: 'icon-search orange',
                    cloneToTop: true,

                },
                 {
                     //edit record form
                     //closeAfterEdit: true,
                     recreateForm: true,
                     beforeShowForm: function (e) {
                         var form = $(e[0]);
                         form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                         style_edit_form(form);
                     },
                     afterComplete: function (response, postdata) {
                         if (response.responseText == "Aggiornamento completato") {
                             alert(response.responseText);
                             reloadTable(grid_selector);
                             location.reload();
                         } else {
                             alert(response.responseText);
                             location.reload();
                         }

                     }
                 },
                 {
                     //new record form
                     closeAfterAdd: true,
                     width: 800,
                     recreateForm: true,
                     viewPagerButtons: false,
                     beforeShowForm: function (e) {
                         var form = $(e[0]);
                         form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                         style_edit_form(form);
                     }, afterComplete: function (response, postdata) {
                         if (response == "inserimento completato") {
                             alert(response)
                             reloadTable(grid_selector);
                             location.reload();
                         } else {
                             alert(response.responseText);
                             location.reload();
                         }

                     }
                 },
                {
                    //delete record form
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        if (form.data('styled')) return false;

                        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                        style_delete_form(form);
                        width: 500
                        form.data('styled', true);
                    } //},
                    //onClick: function (e) {
                    //    alert(1);
                    //}
                },
                {
                    //search form
                    recreateForm: true,
                    afterShowSearch: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                        style_search_form(form);
                    },
                    afterRedraw: function () {
                        style_search_filters($(this));

                    }
                    ,
                    multipleSearch: true,
                    /**
                    multipleGroup:true,
                    showQuery: true
                    */
                },
                {
                    //view record form
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                    }
                }
            ).jqGrid('navButtonAdd', '#' + grid[0].id + '_toppager_left', { // "#list_toppager_l
                caption: "Esporta in Excel",
                buttonicon: "icon-file green",
                onClickButton: function () {
                    grid.jqGrid("exportToExcel", {
                        title: 'Lista Utenti',
                        orientation: 'landscape',
                        pageSize: 'A4',
                        description: '',
                        customSettings: null,
                        download: 'download',
                        includeLabels: true,
                        includeGroupHeader: true,
                        includeFooter: true,
                        fileName: "ListaUtenti.xls"
                    })



                }, position: "last"
            });







        });







        $('#export').change(function () {

            top.location.href = "?data=" + $('#export option:selected').val()

        });





    </script>
</asp:Content>
