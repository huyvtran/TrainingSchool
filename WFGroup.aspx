<%@ Page Title="" Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFGroup.aspx.vb" Inherits="TrainingSchool.WFGroup" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <!-- #page-content -->
    <div class="page-content">
        <div class="page-header">
            <h1><b>Le tue classi</b><small></small>
                <small><i class="icon-double-angle-right"></i></small>
            </h1>
        </div>


        <div class="row">
            <div id="tableresponsive" class="table-responsive">

                <div id="pager_category"></div>
                <table id="jqgrid_category"></table>

            </div>

        </div>

        <div id="modal-invite" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel4">Invito alla classe</h4>
                </div>
                <!-- /.modal-header -->
                <div class="modal-body">

                   
                    <input type="hidden" id="codice" />
                     <input type="hidden" id="idcategory" />
                    <div class="box-content">
                         <div class="form-horizontal">
                         <div class="form-group">
                                <label for="notesess" class="col-sm-3 control-label no-padding-right">Nome Studente</label>
                                <div class="col-sm-9">

                                    <input type="text" class="col-sm-6" name="firstname" id="firstname" />
                                </div>
                            </div>
                              <div class="form-group">
                                <label for="notesess" class="col-sm-3 control-label no-padding-right">Cognome Studente</label>
                                <div class="col-sm-9">

                                    <input type="text" class="col-sm-6" name="lastname" id="lastname" />
                                </div>
                            </div>
                         <div class="form-group">
                                <label for="notesess" class="col-sm-3 control-label no-padding-right">email</label>
                                <div class="col-sm-9">

                                    <input type="email" class="col-sm-6" name="email" id="email" />
                                </div>
                            </div>

                        </div>
                    </div>

                </div>


                <!-- /.modal-body -->

                <!-- /.modal-footer -->
            </div>

            <div class="modal-footer">
              
                <button type="button" title='Invia invito' id="btninvite" class='btn btn-success prenotazione' ><i class='ace-icon icon-envelope bigger-160 white'></i>Invia!</button>
          
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

    </div>



        <!-- PAGE CONTENT ENDS -->
    </div>


    <!-- page specific plugin scripts -->
    <script src="assets/js/jqGrid/src/jquery.jqGrid.js"></script>
    <script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>



    <script type="text/javascript">
        function openmodal(codice,idcategory) {
            $("#codice").val(codice);
            $("#idcategory").val(idcategory)
            $("#modal-invite").modal({ backdrop: 'static', keyboard: false }, 'show');
        }

        $("#btninvite").click(function () {

            $.ajax({
                url: "AdminAjaxLMS.aspx?op=modprenotazioni&oper=sendinvite",
                type: 'POST',
                data: { nome: $("#firstname").val(),cognome: $("#lastname").val(),  codice: $("#codice").val(), email: $("#email").val(),idcategory:$("#idcategory").val() },
                datatype: 'json',
                success: function (data) {
                    alert(data);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("Errore");

                }

            })
        });
        var admin ='<%=Session("idprofile")%>';
        resizeJqGridWidth("jqgrid_category", "tableresponsive", 300);

        jQuery("#jqgrid_category").jqGrid({
            subGrid: true,
            subGridOptions: {

                plusicon: "ace-icon glyphicon glyphicon-plus bigger-110 blue",
                minusicon: "ace-icon glyphicon glyphicon-minus bigger-110 blue",
                openicon: "ace-icon fa fa-chevron-right center orange"
            },

            subGridRowExpanded: function (subgridDivId, rowId) {
                var subgridTableId = subgridDivId + "_t";
                var subgridTableId1 = subgridDivId + "_p";
                $("#" + subgridDivId).html("<table id='" + subgridTableId + "'></table><div id='" + subgridTableId1 + "'></div>");
                $("#" + subgridTableId).jqGrid({
                    height: 'auto',
                    viewrecords: true,
                    ignoreCase: true,
                    gridview: true,
                    loadonce: true,
                    pager: "#" + subgridTableId1,
                    datatype: 'json',
                    url: 'AdminAjaxLMS.aspx?op=moduser&oper=getstudenti&id=' + rowId,
                    colNames: ['', '', 'NOME', 'COGNOME', 'DATA NASCITA', 'EMAIL', 'TELEFONO'],
                    colModel: [
                        { name: 'act', index: 'act', expoindex: 'id', width: 90 },
                        { name: 'id', keys: true, hidden: true, index: 'id', width: 90 },
                        { name: 'firstname', index: 'firstname', width: 140 },
                        { name: 'lastname', index: 'lastname', width: 140 },
                        { name: 'datanascita', index: 'datanascita', width: 350 },
                        { name: 'email', index: 'email', width: 350 },
                        { name: 'tel', index: 'tel', width: 350 },


                    ],
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


                            }
                        }


                    },
                }); },
            autowidth: true,
            height: "auto",
            url: 'AdminAjaxLMS.aspx?op=moduser&oper=getcategory',
            datatype: "json",
            colNames: ['', 'ID', 'DATA CREAZIONE', 'NOME CLASSE-GRUPPO', 'CODICE CLASSE'],
            colModel: [

                { name: 'act', index: 'act', width: 80, sortable: false, editable: false },
                { name: 'id', index: 'id', keys: true, width: 20, hidden: true, sorttype: "int", editable: true },
                { name: 'date_create', index: 'date_create', editable: false, width: 170, sortable: true, sortable: true },
                { name: 'description', index: 'description', width: 280, editable: true, editoptions: { size: "20", maxlength: "20" }, sortable: true },
                { name: 'codice', index: 'codice', sortable: true, width: 170, editable: false, },


            ],

            rowNum: 100,
            rowList: [100, 150, 200],
            gridview: true,
            loadonce: true,
                       sortname: 'description',
            ignoreCase: true,
            sortorder: "desc",
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
                        if (admin == 1) {

                           em1= " <a title='Assegna studenti' class='btn btn-success btn-sm'  href='WfStudentsassign.aspx?title=" + rowData.description + "&idcategory=" + rowData.id + "'><i class='ace icon-user bigger-130'></i>(" + rowData.iscrittistudenti + ")</a> ";
                           sm2= " <a title='Assegna docenti' class='btn btn-warning btn-sm'  href='WFteacherAssign.aspx?title=" + rowData.description + "&idcategory=" + rowData.id + "'><i class='icon-graduation-cap bigger-160'></i>(" + rowData.iscrittidocenti + ")</a> ";
                            jQuery("#jqgrid_category").jqGrid('setRowData', rowData.id, { act:  em1 + sm2 });
                        } else {


                       // re = "<div  style='float:left;margin-left:5px;'  class='ui-pg-div ui-inline-del'  onclick=\"openmodal('" + rowData.codice + "'," + rowData.id + ");\" ><span class='ace-icon icon-envelope bigger-160 green'> </span>Crea invito</div>";
                        // jQuery("#jqgrid_category").jqGrid('setRowData', rowData.id, { act: re });
                        }
                    }
                }

            },


            editurl: "AdminAjaxLMS.aspx?op=modcategoryuser",//nothing is saved
            caption: "CLASSI/GRUPPI",


        });

        jQuery("#jqgrid_category").jqGrid('navGrid', "#pager_category",
            {
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

                recreateForm: true,
                beforeShowForm: function (e) {
                    var form = $(e[0]);
                    form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                    style_edit_form(form);
                },
                onInitializeForm: function (e) {
                },
                afterComplete: function (response, postdata) {
                
                        alert(response.responseText);
                        location.reload()
                  

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



    </script>

</asp:Content>
