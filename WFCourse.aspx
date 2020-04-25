<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFCourse.aspx.vb" Inherits="TrainingSchool.WFCourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


 


    <div class="page-content">
        <div class="page-header">
            <h1>Gestione Corsi
                <small><i class="icon-double-angle-right"></i></small>
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->


                <menu id="nestable-menu">
                    <%--  <a href="#" onclick="openCat();" id="btnCat" class="btn btn-purple btn-sm">Aggiungi Categoria</a>
                    <a href="#" onclick="openLogo();" id="btnlogo" class="btn btn-blues btn-sm">Aggiungi Logo</a>--%>

                    <%--    <button class="btn btn-warning btn-sm" type="button" data-action="expand-all">Espandi</button>
                    <button class="btn btn-warning btn-sm" type="button" data-action="collapse-all">Collassa</button>
                    <button id="update" class="btn btn-cog btn-sm">Aggiorna</button>--%>
                </menu>

                <div id="listcourse" runat="server">
                    <div class="table-responsive">
                        <table id="jqgrid_course"></table>

                        <div id="pager_course"></div>
                    </div>
                    <script type="text/javascript">
                        var $path_base = "/";//this will be used in gritter alerts containing images
                    </script>

                </div>


                <!-- PAGE CONTENT ENDS -->
            </div>
        </div>
    </div>
    <!-- page specific plugin scripts -->


    <script src="assets/js/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>




    <script type="text/javascript">
        jQuery.jgrid.no_legacy_api = true;


        jQuery(function ($) {

            var category;
            var images = { '1': 'oam.jpg' };

            $.ajax({
                type: "GET",
                url: "AdminAjaxLMS.aspx?op=modcourse&oper=cat",
                async: false,
                dataType: "json",
                success: function (data) {
                    category = data;

                },
                fail: function (data) {
                    alert(data)
                }

            });







            function getAllImage() {
                var image;
                var request4 = $.ajax({
                    type: "GET",
                    url: "AdminAjaxLMS.aspx?op=modcourse&oper=image",
                    dataType: "json"
                });


                request4.success(function (data) {


                });

                return image;

            }

            var grid_selector = "#jqgrid_course";
            var pager_selector = "#pager_course";
            var jgImageFormatter = "*.jpg";

            var grid = $(grid_selector);



            jQuery(grid_selector).jqGrid({
                height: 445,
                url: 'AdminAjaxLMS.aspx?op=modcourse&oper=get',
                datatype: "json",
                colNames: ['', 'ID', 'CATEGORIA', 'CODICE', 'NOME', 'SOGLIA ORE', 'ISCRITTI'],
                colModel: [

                    { name: 'act', index: 'act', width: 60, sortable: false, editable: false },
                    { name: 'id', index: 'id', width: 20, hidden: true, sorttype: "int", editable: true },
                    { name: 'description', index: 'description', sortable: true, width: 40, editable: true, edittype: "select", editoptions: { dataUrl: 'AdminAjaxLMS.aspx?op=modcourse&oper=cat' } },
                    { name: 'code', index: 'code', width: 40, editable: true, editoptions: { size: "20", maxlength: "20" } },
                    {
                        name: 'name', index: 'name', width: 120, sortable: true, sortable: true, searchoptions: { sopt: ['cn'] }, editable: true, editoptions: { size: "40", maxlength: "120" }
                    },
                    { name: 'credits', index: 'credits', width: 40, editable: true, hidden: true, editrules: { edithidden: true }, editoptions: { size: "5", maxlength: "20" } },
                    { name: 'students', index: 'students', width: 35, sorttype: "int", editable: false },

                ],


                rowNum: 100,
                rowList: [100, 150, 200],
                rowList: [],        // disable page size dropdown
                pgbuttons: false,     // disable page control like next, back button
                pgtext: null,
                gridview: true,
                loadonce: true,
                sortable: true,
                pager: pager_selector,
                toppager: true,
                pagerpos: "right",
                sortname: 'name',
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

                            var rowData = obj[i];
                            var coursename = escape(rowData.name);
                            var code = rowData.code;


                                 st = "<button type='button' class='btn btn-warning'   onclick=\"openpage('WfCourseStructure.aspx?q=1','" + rowData.id + "','" + rowData.name + "','" + rowData.code + "' );\"  ><i class='ui-icon icon-pencil white '></i>Aggiungi contenuti</button>";
        
                            jQuery(grid_selector).jqGrid('setRowData', rowData.id, { act: st });

                        }
                    }
                },


                editurl: "AdminAjaxLMS.aspx?op=modcourse",//nothing is saved
                caption: "Elenco Corsi",

                //grouping: true,
                //groupingView: {
                //    groupField: ['category'],
                //    groupText: ['<b>{0} </b>'],
                //    groupCollapse: false
                //},
            });



            //navButtons
            jQuery(grid_selector).jqGrid('navGrid', pager_selector,
                { 	//navbar options
                    add: true,
                    addicon: 'icon-plus-sign purple',
                    edit: true,
                    editicon: 'icon-pencil blue',
                    del: true,
                    delicon: 'icon-trash red',
                    search: true,
                    searchicon: 'icon-search orange',
                    cloneToTop: true,

                },
                {
                    //edit record form

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

                        }

                    }
                },
                {

                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                        style_edit_form(form);
                    },
                    afterComplete: function (response, postdata) {
                        if (response.responseText == "Inserimento completato") {
                            alert(response.responseText);
                            reloadTable(grid_selector);
                            location.reload();
                        } else {
                            alert(response.responseText);

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

                        form.data('styled', true);
                    } //},
                    //onClick: function (e) {
                    //    alert(1);
                    //}
                },
                {
                    //search form

                    recreateForm: true,
                    closeAfterSearch: true,
                    searchOnEnter: true,
                    beforeShowSearch: function (form) {
                        $(form).keydown(function (e) {
                            if (e.keyCode == 13) {
                                setTimeout(function () {
                                    $("#fbox_jqgrid_course_search").click();
                                }, 200);
                            }
                        });
                        return true;
                    },
                    onClose: function (form) {
                        $("#fbox_jqgrid_course_search").unbind('keydown');
                    },
                    afterShowSearch: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                        style_search_form(form);
                    },
                    afterRedraw: function () {
                        style_search_filters($(this));
                    },
                    afterComplete: function (response, postdata) {
                        $(this).dialog("close");
                    },

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

            //grid.jqGrid('navButtonAdd', '#' + grid[0].id + '_toppager_left', { // "#list_toppager_left"
            //    caption: " Gestione Categoria",
            //    buttonicon: "icon-inbox",
            //    onClickButton: function () {

            //        openCat();

            //    },
            //    position: "last"
            //});


            //    grid.jqGrid('navButtonAdd', '#' + grid[0].id + '_toppager_left', { // "#list_toppager_left"
            //        caption: " Gestione Loghi",
            //        buttonicon: "icon-pencil",
            //        onClickButton: function () {

            //            openLogo();

            //        },
            //        position: "last"
            //    });

        });


        function flogo(sid) {

            openLogo();
        }

        function fcat(sid) {

            openCat();

        }



        function openCat() {


            ViewModalObj("WFMakeCategory.aspx?op=get");

        }

        function getuser(idcourse) {

            var nusers;
            var request4 = $.ajax({
                type: "GET",
                url: "AdminAjaxLMS.aspx?op=modcourse&oper=getuser",
                dataType: "json",
                data: { id: idcourse }
            });


            request4.success(function (data) {
                nusers = data;
            });

            return nusers;

        }






        function openLogo() {
            var dialog = $("#dialog_logo").removeClass('hide').dialog({
                modal: true,
                title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='icon-ok'></i> Gestione Loghi Corsi</h4></div>",
                height: 500,
                width: 800,
                title_html: true,
                buttons: [
                    {
                        text: "Cancel",
                        "class": "btn btn-xs",
                        click: function () {
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: "OK",
                        "class": "btn btn-primary btn-xs",
                        click: function () {

                            $(this).dialog("close");
                            location.reload();
                        }
                    }
                ]
            });
        }


        function openwindow(url, idcourse, coursename, code) {
            var stile = "width=1200,height=700";
            ViewModalObj(url + "&idcourse=" + idcourse + "&course=" + coursename + "&code=" + code + "");
        }





    </script>
</asp:Content>
