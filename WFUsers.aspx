<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFUsers.aspx.vb" Inherits="TrainingSchool.WFUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
    <div class="page-content">
        <div class="page-header">
            <h1>Gestione Utenti
<small> <i class="icon-double-angle-right"></i></small>
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
                <label class="col-sm-3 control-label no-padding-right" for="UDoc">Import File Txt </label>
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
             <br><p>Il file deve avere i dati anagrafici degli utenti separati dal punto e virgola nel seguente format:
                 Cognome;Nome;email;codicefiscale;telefono per ogni riga</p><br />
           
            </div></div>


    
    <script src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
   <%-- <script src="assets/js/jquery.ui.touch-punch.min.js"></script>--%>
    <script src="assets/js/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>
    <script src="assets/js/codeUpload/jquery.iframe-transport.js"></script>
	<script src="assets/js/codeUpload/jquery.fileupload.js"></script>
   
    <script>
        jQuery(function ($) {


            grid_selector = "#jqgrid_useravailable";
            pager_selector = "#pager_useravailable";

            var grid = $(grid_selector);

            jQuery(grid_selector).jqGrid({
                url: 'AdminAjaxLMS.aspx?op=moduser&oper=get&sel=userall',
                datatype: "json",
                height: 445,
                colNames: [ '', 'ID', 'NOME', 'COGNOME','EMAIL','CF','TEL','SCUOLA','DATA DI NASCITA','RUOLO', 'USERNAME', 'PASS',  'DATA REGISTRAZIONE','ISCRIZIONI'],
                colModel: [
                    
                          { name: 'act', index: 'act', width: 50, sortable: false, resize: false, editable: false },
                        { name: 'id', index: 'id', width: 30, sorttype: "int", editable: true },
                         { name: 'firstname', index: 'firstname', width: 100, editable: true, editrules: { hidden: true}, editoptions: { size: "70", maxlength: "50" } },
                        { name: 'lastname', index: 'lastname', width: 100, editable: true, editrules: { hidden: true},editoptions: { size: "70", maxlength: "50" } },
                        { name: 'email', index: 'email', width: 150, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },
                        { name: 'cf', index: 'cf', width: 100, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },
                         { name: 'tel', index: 'tel', width: 100, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },
                         { name: 'scuola', index: 'scuola', width: 100, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },
{ name: 'datanascita', index: 'datanascita', width: 100, editable: true, editrules: { hidden: true }, editoptions: { size: "70", maxlength: "50" } },

                        { name: 'profile', index: 'profile', sortable: true, width: 90, editable: true, edittype: "select", editoptions: { dataUrl: 'AdminAjaxLMS.aspx?op=moduser&oper=profile' } },
                           { name: 'userid', index: 'userid', width: 100, editable: false, editoptions: { size: "70", maxlength: "30" } },
                             { name: 'clearpass', index: 'clearpass', width: 100, editable: false, editoptions: { size: "70", maxlength: "30" } },
                         
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

                        //ce = "<div  title='Visualizza scheda' style='float:left;margin-left:5px;' class='ui-pg-div ui-inline-search'   onclick=\"ViewModalObj('WfIOViewProfile.aspx?iduser=" + rowData.id + "','" + rowData.firstname + ' '  + rowData.lastname + "');\"  > <span class='ui-icon icon-credit-card blue'></span></div>";
                                             
                        //jQuery("#jqgrid_useravailable").jqGrid('setRowData', rowData.id, { act: ce });

                        fe = "<div  title='Iscrizioni ai corsi' style='float:left;margin-left:5px;' class='ui-pg-div ui-inline-search'   onclick=\"ViewModalObj('WfReportByuser.aspx?iduser=" + rowData.id + "&nominativo='" + rowData.firstname + " " + rowData.lastname + "'','Report');\"  > <i class='ui-icon icon-graduation-cap green'></i>("+ rowData.iscrizioni +")</div>";
                        
                        jQuery("#jqgrid_useravailable").jqGrid('setRowData', rowData.id, { iscrizionit: fe  });
                      

                    }


                },

                editurl: "AdminAjaxLMS.aspx?op=moduser",//nothing is saved
                caption: "Elenco Utenti"

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

          


                  grid.jqGrid('navButtonAdd', '#' + grid[0].id + '_toppager_left', { // "#list_toppager_left"
                      caption: "Importa Studenti",
                      buttonicon: "ace icon-user blue",
                      onClickButton: function () {

                         opendialogupload(3,'Studenti')
                      }, position: "last"
                  });

             grid.jqGrid('navButtonAdd', '#' + grid[0].id + '_toppager_left', { // "#list_toppager_left"
                      caption: "Importa Docenti",
                      buttonicon: "icon-graduation-cap blue",
                      onClickButton: function () {

                         opendialogupload(2,'Docenti')
                      }, position: "last"
                  });


                  $('#fileupload').fileupload({

                      replaceFileInput: true,

                      dataType: 'json',

                      url: "HUpload.ashx?load=loadexcel&tipo=" + $("#tipo").val(),

                      dataType: 'json',
                      add: function (e, data) {
                          $('.ui-dialog-buttonset').empty();
                          data.context = $('<button/>').attr({ type: 'button' }).text('Invia').addClass('btn btn-primary ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only')
                              .appendTo(".ui-dialog-buttonset")
                              .click(function () {

                               
                                      data.context = $('<p/>').text('Caricando...').replaceAll($(this));
                                      data.submit();
                                 


                              });
                      },
                      done: function (e, data) {
                         
                        data.context = $('<p/>').text('Caricando terminato').replaceAll($(this));
                          grid1.setGridParam({
                              fromserver:true,
                              datatype: 'json',
                              url: 'adminAjaxLMS.aspx?op=moduser&oper=get',
                          }).trigger('reloadGrid', [{ page: 1 }]);
                      },
                       derrorne: function (e, data) {
                           alert(data);
                        data.context = $('<p/>').text('Caricando terminato').replaceAll($(this));
                          grid1.setGridParam({
                              fromserver:true,
                              datatype: 'json',
                              url: 'adminAjaxLMS.aspx?op=moduser&oper=get',
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

        function opendialogupload(tipo,nome) {

            $("#tipo").val(tipo);

            var dialog = $("#dialog-docsession").removeClass('hide').dialog({
                modal: true,
                title: "<div class='widget-header widget-header-small'><h4 style='color:black' ><i class='ace-icon fa fa-file'></i> " + nome + " </h4></div>",
                height: 400,
                width: 700,

                title_html: true,

                buttons: [
                    {
                        text: "Salva",
                        "class": "btn btn-minier",
                        click: function () {

                            grid1.setGridParam({

                                datatype: 'json',
                                url: 'adminAjaxLMS.aspx?op=moduser&oper=get',
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
