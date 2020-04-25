<%@ Page Title="" Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFstudentsAssign.aspx.vb" Inherits="TrainingSchool.WFAssignstudents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="page-content">
        <div class="page-header">
            <h1>Assegnazione Classi-Studenti: <b><%=Request.QueryString("title") %>
                <small><i class="icon-double-angle-right"></i></small></h1>
        </div>
        <!-- /.page-header -->


        <!-- PAGE CONTENT BEGINS -->


        <div class="row">
            <div class="col-xs-12">


                <table id="jqgrid_studentsassign"></table>
                <div id="pager_studentsassign"></div>
            </div>
            <div id="toolbarstudents" class="clearfix form-actions">

                <div class="col-md-offset-4 col-md-9">
                    <button id="btnAssignUser" class="btn btn-info" type="button">
                        <i class="icon-ok bigger-110"></i>
                        Assegna studenti alla classe
                    </button>


                </div>
            </div>

            <div class="col-xs-12">
                <table id="jqgrid_select"></table>
                <div id="pager_select"></div>
            </div>
        </div>




    </div>

    <script src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
    <script src="assets/js/jquery.ui.touch-punch.min.js"></script>
    <script src="assets/js/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>


    <script>


        jQuery(function ($) {


            var grid_selector2 = "#jqgrid_studentsassign";
            var grid_select = "#jqgrid_select";

            var pager_select = "#pager_select";
            var pager_selector2 = "#pager_studentsassign";

            var idcategory = <%= Request.QueryString("idcategory")%> +"";
            var oper = "<%= Request.QueryString("op")%>";
            var checkavailable = false;
            var noth = true;
            var HeaderGrid;
            eaderGrid = "Studenti Assegnati"
            //if (oper == "studentsassigned") {
            //    HeaderGrid = "Studenti Assegnati"
            //    jQuery("#toolbarstudents").hide();
            //    checkavailable = true;
            //} else {
            //    HeaderGrid = "Studenti da assegnare"
            //    checkavailable = true;
            //}


            jQuery(grid_selector2).jqGrid({
                height: 'auto',
                url: ' AdminAjaxLMS.aspx?op=modstudents&oper=usersassigned&idcategory=' + idcategory,
                datatype: "json",
                colNames: ['COGNOME', 'ID', 'NOME', 'EMAIL', 'CODICEFISCALE', ''],

                colModel: [
                    { name: 'lastname', index: 'lastname', width: 90, searchoptions: { sopt: ['cn'] }, editable: false, editoptions: { size: "120", maxlength: "50" } },

                    { name: 'id', index: 'id', width: 60, hidden: true, keys: true, sorttype: "int", editable: false },
                    { name: 'firstname', index: 'firstname', width: 120, editable: false, editoptions: { size: "180", maxlength: "30" } }, { name: 'codicefiscale', index: 'codicefiscale', width: 120, editable: false, editoptions: { size: "180", maxlength: "30" } },
                    { name: 'email', index: 'email', width: 120, editable: false, editoptions: { size: "180", maxlength: "30" } },
                    { name: 'idcategory', index: 'idcategory', width: 180, hidden: false, editable: false, editoptions: { size: "180", maxlength: "120" } },


                ],
                rowNum: 20,
                rowList: [20, 50, 100],
                  rowList: [],        // disable page size dropdown
    pgbuttons: false,     // disable page control like next, back button
    pgtext: null,
                gridview: true,
                loadonce: true,
                sortable: true,
                pager: pager_selector2,
                pagerpos: "right",
                toppager: true,
                sortname: 'name',
                sortorder: "desc",
                autowidth: true,
                viewsortcols: [true, 'vertical', true],
                viewrecords: true,
                multiselect: checkavailable,

                loadComplete: function (data) {

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
                            if (rowData.idcategory == null) {
                                //  $("#" + rowData.id).find("td").css("background-color", "rgba(255,123,0,0.1)");
                            }//update this to have your own check
                            var checkbox = $("#jqg_jqgrid_studentsassign_" + rowData.id);
                            checkbox.css("visibility", "hidden");
                            checkbox.attr("disabled", true);

                        }
                    }


                },



                editurl: " AdminAjaxLMS.aspx?op=modstudents&oper=studentsdelete&idcategory=" + getParameterByName("idcategory"),
                caption: "Studenti assegnati alla classe  <%=Request.QueryString("name")%>"

            });



            function pickDate(cellvalue, options, cell) {
                setTimeout(function () {
                    $(cell).find('input[type=text]')
                        .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
                }, 0);
            }



            jQuery(grid_selector2).jqGrid('navGrid', pager_selector2,
                { 	//navbar options

                    edit: false,
                    editicon: 'icon-pencil blue',
                    del: true,
                    delicon: 'icon-trash red',
                    search: false,
                    searchicon: 'icon-search orange',

                },
                {
                    //edit record form
                    width: 330,
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                        style_edit_form(form);
                    },
                    afterComplete: function (response, postdata) {
                        if (response.responseText == "Aggiornamento completato") {
                            alert(response.responseText);
                            reloadTable(grid_select);

                        } else {
                            alert(response.responseText);

                        }

                    }
                },



                {
                    //delete record form
                    width: 300,
                    recreateForm: true,
                    beforeShowForm: function (e) {

                    }, afterComplete: function (response, postdata) {
                        alert(response.responseText);
                    }
                },
                {
                    //search form
                    width: 300,
                    recreateForm: true,
                    afterShowSearch: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                        style_search_form(form);
                        alert("search");
                    },
                    afterRedraw: function () {
                        style_search_filters($(this));
                    },

                },
                {
                    //view record form
                    width: 1200,
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                    }
                }
            )








            jQuery(grid_select).jqGrid({

                height: 'auto',
                datatype: "json",
                url: ' AdminAjaxLMS.aspx?op=modstudents&oper=usersavailable&idcategory=<%= Request.QueryString("idcategory")%>',

                colNames: ['COGNOME', 'ID', 'NOME', 'CODICEFISCALE', 'EMAIL', ''],

                colModel: [
                    { name: 'lastname', index: 'lastname', width: 90, searchoptions: { sopt: ['cn'] }, editable: false, editoptions: { size: "120", maxlength: "50" } },

                    { name: 'id', index: 'id', width: 60, hidden: true, keys: true, sorttype: "int", editable: false },
                    { name: 'firstname', index: 'firstname', width: 120, editable: false, editoptions: { size: "180", maxlength: "30" } },
                    { name: 'cf', index: 'cf', width: 120, editable: false, editoptions: { size: "180", maxlength: "30" } },
                    { name: 'email', index: 'email', width: 120, editable: false, editoptions: { size: "180", maxlength: "30" } },
                    { name: 'idcategory', index: 'idcategory', width: 180, hidden: false, editable: false, editoptions: { size: "180", maxlength: "120" } },


                ],
                rowNum: 20,
                rowList: [20, 50, 100],
                  rowList: [],        // disable page size dropdown
    pgbuttons: false,     // disable page control like next, back button
    pgtext: null,
                gridview: true,
                loadonce: true,
                sortable: true,
                pager: pager_select,
                pagerpos: "right",
                toppager: false,
                sortname: 'name',
                sortorder: "desc",
                autowidth: true,
                onSelectRow: function (id, status) {


                    var grid = jQuery(grid_select);
                    var rowKey = grid.getGridParam("selrow");
                    if (rowKey != null) {
                        var row = grid.jqGrid('getRowData', rowKey);

                         grid.delRowData(rowKey);


                        $(grid_selector2).addRowData(rowKey, row);

                        $(grid_select).jqGrid('restoreRow', rowKey, { keys: true });
                        $(grid_select).jqGrid('editRow', rowKey, true);
                        $(grid_select).jqGrid('saveRow', rowKey, {
                            succesfunc: function (response) {
                                alert(response);
                            },
                            url: " AdminAjaxLMS.aspx?op=modcategory",
                            mtype: "POST"

                        });
                        $(grid_select).jqGrid('editRow', rowKey, true);
                    }

                },
                viewsortcols: [true, 'vertical', true],
                viewrecords: true,
                caption: "Cerca Studenti da assegnare",
                editurl: " AdminAjaxLMS.aspx?op=modcategory",

            });





            jQuery(grid_select).jqGrid('navGrid', pager_select,
                { 	//navbar options

                    edit: true,
                    editicon: 'icon-pencil blue',
                    del: true,
                    delicon: 'icon-trash red',
                    search: true,
                    searchicon: 'icon-search orange',

                },
                {
                    //edit record form
                    width: 330,
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                        style_edit_form(form);
                    },
                    afterComplete: function (response, postdata) {
                        if (response.responseText == "Aggiornamento completato") {
                            alert(response.responseText);
                            reloadTable(grid_select);

                        } else {
                            alert(response.responseText);

                        }

                    }
                },
                {
                    //delete record form
                    width: 330,
                    recreateForm: true,
                    beforeShowForm: function (e) {
                    }, afterComplete: function (response, postdata) {
                        // alert(response.responseText);
                    }
                },
                {
                    //search form
                    width: 330,
                    recreateForm: true,
                    closeAfterSearch: true,
                    searchOnEnter: true,
                    beforeShowSearch: function (form) {
                        $(form).keydown(function (e) {
                            if (e.keyCode == 13) {
                                setTimeout(function () {
                                    $("#fbox_jqgrid_select_search").click();
                                }, 200);
                            }
                        });
                        return true;
                    },
                    onClose: function (form) {
                        $("#fbox_jqgrid_select_search").unbind('keydown');
                        $('input[id*="gs_"]').val("");
                    },
                    afterShowSearch: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                        style_search_form(form);
                    },
                    afterRedraw: function () {
                        style_search_filters($(this));
                    },


                    multipleSearch: true,
                    /**
                    multiplecategory:true,
                    showQuery: true
                    */
                },
                {
                    //view record form
                    width: 1200,
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                    }
                }
            )




            jQuery("#btnAssignUser").on("click", function (e) {
                e.preventDefault();


                var grid = jQuery(grid_selector2);
                var ids = grid.jqGrid('getDataIDs');
                var rowId;
                for (var i = 0; i < ids.length; i++) {
                    rowId += ids[i] + ',';
                }
                var urlsave = " AdminAjaxLMS.aspx?op=modstudents&oper=add&idcategory=<%=Request.QueryString("idcategory")%>&iduser=" + rowId;

                var request4 = $.ajax({
                    type: "GET",
                    url: urlsave,
                    dataType: "html"
                });

                request4.fail(function (data) {

                    alert("Assegnazione Fallita");

                });


                request4.success(function (data) {

                    alert(data);


                });
            });



        });




    </script>

</asp:Content>
