<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFPlatform.aspx.vb" Inherits="TrainingSchool.WFPlatform" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <div class="page-content">
        <div class="page-header">
            <h1>Gestione Piattaforme
                <small><i class="icon-double-angle-right"></i></small>
            </h1>
        </div>
        <!-- /.page-header -->


        <!-- PAGE CONTENT BEGINS -->


        <div class="row">
            <div class="col-xs-12">
                <div id="pager_useravailable"></div>


                <table id="jqgrid_useravailable"></table>




            </div>


        </div>


        <!-- PAGE CONTENT ENDS -->
    </div>
    <!-- page specific plugin scripts -->

    <div id="dialog-docsession" class="hide">
        <div class="form-horizontal">


            <div class="form-group">

                <div class="col-sm-9">
                    <span class="btn btn-success fileinput-button">
                        <i class="glyphicon glyphicon-plus"></i>
                        <span>Scegli file...</span>
                        <!-- The file input field used as target for the file upload widget -->
                        <input id="fileupload" type="file" name="files" />

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



    <script src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
    <script src="assets/js/jquery.ui.touch-punch.min.js"></script>
    <script src="assets/js/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>

    <script src="assets/js/codeUpload/jquery.iframe-transport.js"></script>
    <script src="assets/js/codeUpload/jquery.fileupload.js"></script>

    <script type="text/javascript">



        jQuery(function ($) {



            grid_selector = "#jqgrid_useravailable";
            pager_selector = "#pager_useravailable";

            var grid = $(grid_selector);

            jQuery(grid_selector).jqGrid({
                url: 'AdminAjaxLMS.aspx?op=modplatform&oper=get&sel=userall',
                datatype: "json",
                height: 445,
                colNames: ['', 'ID', 'NOME', 'COGNOME', 'INDIRIZZO WEB', 'EMAIL', 'PASSWORD', 'DATA CREAZIONE', 'DATA FINE'],
                colModel: [
                    { name: 'act', index: 'act', width: 50, sortable: false, resize: false },
                    { name: 'id', index: 'id', width: 30, sorttype: "int", editable: true },
                    { name: 'nome', index: 'nome', width: 100, editable: true, editoptions: { size: "30", maxlength: "50" } },
                    { name: 'cognome', index: 'cognome', width: 100, editable: true, editoptions: { size: "30", maxlength: "50" } },
                    { name: 'indirizzoweb', index: 'indirizzoweb', width: 100, editable: true, editoptions: { size: "30", maxlength: "11" } },
                    { name: 'email', index: 'email', width: 100, editable: true, editoptions: { size: "30", maxlength: "50" } },
                    { name: 'userpassword', index: 'userpassword', width: 100, editable: true, editoptions: { size: "30", maxlength: "50" } },

                    { name: 'datacreazione', index: 'datacreazione', width: 100, editable: false, editoptions: { size: "30", maxlength: "50" } },
                    { name: 'datafine', index: 'datafine', width: 100, editable: false, editoptions: { size: "30", maxlength: "50" } },


                ],

                rowNum: 20,
                rowList: [20, 50, 100],
                gridview: true,
                loadonce: true,
                sortable: true,
                pager: pager_selector,
                pagerpos: "right",
                toppager: true,
                sortname: 'register_date',
                sortorder: "nome",
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

                    if (!(typeof obj === 'undefined'))
                        for (var i = 0; i < obj.length; i++) {
                            var rowData = obj[i];

                            // jQuery("#jqgrid_useravailable").jqGrid('setRowData', rowData.id, { act: ce });

                        }


                },

                editurl: "AdminAjaxLMS.aspx?op=modplatform",//nothing is saved
                caption: "Elenco Piattaforme"

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
                    onclickSubmit: function (response, postdata) {
                        $('#loading').show();
                    },
                    afterComplete: function (response, postdata) {


                        alert(response.responseText);
                        location.reload();

                        $('#loading').hide();

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
                    },
                    onclickSubmit: function (response, postdata) {
                        $('#loading').show();
                   
                }, afterComplete: function (response, postdata) {

                   
                        alert(response.responseText);
                        location.reload();

                        $('#loading').hide();

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
        )


        //  grid.jqGrid('navButtonAdd', '#' + grid[0].id + '_toppager_left', { // "#list_toppager_left"
        //caption: " Campi aggiuntivi",
        //buttonicon: "icon-credit-card green",
        //onClickButton: function () {

        //    lmspopup("WFAddField.aspx?op=get");

        //}, position: "last"
        // });





        $('#fileupload').fileupload({

            replaceFileInput: true,

            dataType: 'json',

            url: "HUpload.ashx?load=loadlogo",

            dataType: 'json',
            add: function (e, data) {
                $('.ui-dialog-buttonset').empty();
                data.context = $('<button/>').attr({ type: 'button' }).text('Invia').addClass('btn btn-primary ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only')
                    .appendTo(".ui-dialog-buttonset")
                    .click(function () {

                        if ($('#iddoc').val() != '') {
                            data.context = $('<p/>').text('Caricando...').replaceAll($(this));
                            data.formData = { id: $('#iddoc').val() };
                            data.submit();
                        } else {
                            alert("Errore !");
                        }


                    });
            },
            done: function (e, data) {
                alert("Caricamento Completato!")
                data.context.text('Caricamento Terminato..');
                grid1.setGridParam({

                    datatype: 'json',
                    url: 'adminAjaxLMS.aspx?op=modplatform&oper=get',
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


        });

        function opendialogupload() {



            var dialog = $("#dialog-docsession").removeClass('hide').dialog({
                modal: true,
                title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='ace-icon fa fa-check'></i> Import Excel </h4></div>",
                height: 250,
                width: 700,

                title_html: true,

                buttons: [
                    {
                        text: "Salva",
                        "class": "btn btn-minier",
                        click: function () {

                            grid1.setGridParam({

                                datatype: 'json',
                                url: 'adminAjaxLMS.aspx?op=modplatform&oper=get',
                            }).trigger('reloadGrid', [{ page: 1 }]);

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




    </script>

</asp:Content>
