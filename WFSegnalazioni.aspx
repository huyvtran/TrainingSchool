<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFSegnalazioni.aspx.vb" Inherits="TrainingSchool.WFSegnalazioni" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <!-- #page-content -->
    <div class="page-content">
        <div class="page-header">
            <h1>Segnalazioni
                <small><i class="icon-double-angle-right"></i></small>
            </h1>
        </div>


        <div class="row">
           <div class="col-lg-12">
            <div class="table-responsive">
                <div id="pager_segnalazioni"></div>

                <table id="jqgrid_segnalazioni"></table>

            </div>
            </div>
        </div>





        <!-- PAGE CONTENT ENDS -->
    </div>
    <!-- /.col -->



    <!-- page specific plugin scripts -->
    <script src="assets/js/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>



    <script type="text/javascript">


        var grid_selector = "#jqgrid_segnalazioni";

        var pager_selector = "#pager_segnalazioni";



        jQuery(grid_selector).jqGrid({
            autowidth: true,
            height: "auto",
            url: 'AdminAjaxLMS.aspx?op=modsegnalazioni&oper=get',
            datatype: "json",
            colNames: ['', 'ID', 'DATA', 'OGGETTO', 'MESSAGGIO', 'LETTO DAL DIRIGENTE'],
            colModel: [

                { name: 'act', index: 'act', width: 80, hidden: true, sortable: false, editable: false },
                { name: 'id', index: 'id', keys: true, width: 20,hidden:true, sorttype: "int", editable: false },
                { name: 'date_insert', index: 'date_insert', sortable: true, editable: true, width: 70 },
                {
                    name: 'subject', index: 'subject', width: 180, editable: true, editoptions:
                    {
                        size: 55,

                    }
                },
                {
                    name: 'message', index: 'message', width: 270, edittype: 'textarea', editrules: { required: true }, editable: true, editoptions:
                    {
                        cols: 80,
                        rows: 5,
                        wrap: "off",
                        style: 'overflow-x: hidden',
                    }
                },
                { name: 'flagread', index: 'flagread', width: 40, sortable: false, editable: false },


            ],


            rowNum: 100,
            rowList: [100, 150, 200],
            gridview: true,
            loadonce: true,
            sortable: true,
            toppager: true,
            pager: pager_selector,
            sortname: 'percorso',
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
                } else if (data.length != undefined) {

                    if (data.length > 0) {

                        obj = data;

                    } else {

                        noth = false;

                    }
                }


                if (noth) {

                    for (var i = 0; i < obj.length; i++) {




                    }
                }
            },


            editurl: "AdminAjaxLMS.aspx?op=modsegnalazioni",//nothing is saved
            caption: "Segnalazioni",

        });


        jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                add: true,
                addicon: 'icon-plus-sign purple',
                edit: true,
                editicon: 'icon-pencil blue',
                del: true,
                delicon: 'icon-trash red',
                search: false,
                searchicon: 'icon-search orange',
                cloneToTop: true,
            },
            {
               width: 800,
                recreateForm: true,
                beforeShowForm: function (e) {
                    var form = $(e[0]);
                    form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                    style_edit_form(form);
                },
                afterComplete: function (response, postdata) {
                    if (response.responseText == "Update completed") {
                        alert(response.responseText);

                    } else {
                        alert(response.responseText);

                    }

                }
            },
            {
                width: 800,
                recreateForm: true,
                beforeShowForm: function (e) {
                    var form = $(e[0]);
                    form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                    style_edit_form(form);
                },
                onInitializeForm: function (e) {


                    var form = $(e[0]);

                },
                afterComplete: function (response, postdata) {
                    if (response.responseText == "Insert completed") {
                        alert(response.responseText);
                        reloadTable(grid_selector);
                    } else {
                        alert(response.responseText);

                    }

                }
            },


            {

                recreateForm: true,
                beforeShowForm: function (e) {
                    var form = $(e[0]);
                    if (form.data('styled')) return false;

                    form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                    style_delete_form(form);

                    form.data('styled', true);
                }
            },
            {


                recreateForm: true,
                afterShowSearch: function (e) {
                    var form = $(e[0]);
                    form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                    style_search_form(form);
                },
                afterRedraw: function () {
                    style_search_filters($(this));
                },


                multipleSearch: true,

            },
            {


                recreateForm: true,
                beforeShowForm: function (e) {
                    var form = $(e[0]);
                    form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                }
            }
        )







    </script>

</asp:Content>
