<%@ Page Language="vb" ValidateRequest="false" AutoEventWireup="false" CodeBehind="WFChartUser.aspx.vb" Inherits="TrainingSchool.WFChartUser" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="description" content="Training School" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link rel="stylesheet" href="assets/css/datepicker.css" />
    <link rel="stylesheet" href="assets/css/ui.jqgrid.css" />

    <link class="include" rel="stylesheet" type="text/css" href="assets/jquery.jqplot.min.css" />
    <link rel="stylesheet" type="text/css" href="assets/examples.min.css" />
    <link type="text/css" rel="stylesheet" href="assets/syntaxhighlighter/styles/shCoreDefault.min.css" />
    <link type="text/css" rel="stylesheet" href="assets/syntaxhighlighter/styles/shThemejqPlot.min.css" />
  



    <!--[if IE 7]>
		  <link rel="stylesheet" href="assets/css/font-awesome-ie7.min.css" />
		<![endif]-->

    <!-- page specific plugin styles -->

    <!-- fonts -->

    <link rel="stylesheet" href="assets/css/ace-fonts.css" />

    <!-- ace styles -->

    <link rel="stylesheet" href="assets/css/ace.min.css" />
    <link rel="stylesheet" href="assets/css/ace-rtl.min.css" />
    <link rel="stylesheet" href="assets/css/ace-skins.min.css" />

    <!--[if lte IE 8]>
		  <link rel="stylesheet" href="assets/css/ace-ie.min.css" />
		<![endif]-->

    <!-- inline styles related to this page -->

    <!-- ace settings handler -->

    <script src="assets/js/ace-extra.min.js"></script>

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->

    <!--[if lt IE 9]>
		<script src="assets/js/html5shiv.js"></script>
		<script src="assets/js/respond.min.js"></script>
		<![endif]-->
    <!-- basic scripts -->

    <!--[if !IE]> -->

    <script type="text/javascript">
        window.jQuery || document.write("<script src='assets/js/jquery-2.0.3.min.js'>" + "<" + "/script>");
    </script>

    <!-- <![endif]-->

    <!--[if IE]>
<script type="text/javascript">
 window.jQuery || document.write("<script src='assets/js/jquery-1.10.2.min.js'>"+"<"+"/script>");
</script>
<![endif]-->

    <script type="text/javascript">
        if ("ontouchend" in document) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
    </script>
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/typeahead-bs2.min.js"></script>
   

    <!-- ace scripts -->

    <script src="assets/js/ace-elements.min.js"></script>
    <script src="assets/js/lmsrb.js"></script>


    <style>
        #modal-1 .modal-dialog {
            width: 85%;
            z-index: 10000;
        }
        .modal-body {
    min-height: calc(100vh - 200px);
    overflow-y: auto;
}
        #myModal1 .modal-dialog {
     width: 75%;
            z-index: 10000;
        }



        #modal-2 .modal-dialog {
            width: 75%;
            z-index: 10000;
        }

        #modal-3 .modal-dialog {
            width: 75%;
            z-index: 10000;
        }

        iframe {
            width: 99%;
            max-height: 400px;
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0 0 0 0 !important;
            margin: 0 0 70px 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            font-size: 1.2em !important;
            font-weight: bold !important;
            text-align: left !important;
        }
    </style>


</head>
<body>
    <form id="form1" runat="server">
        <div class="page-header">
			<h3>    
           <%=Request.QueryString("fullname") %> 
              
			</h3><br />
              Totale Tempo nel Corso <asp:Label Font-Bold="true" ID="lbtotaltime" runat="server"></asp:Label>
               Totale Tempo VideoCorsi <asp:Label Font-Bold="true" ID="Lbtotaltimevideocorsi" runat="server"></asp:Label>
       
		
                           </div>
        <div class="page-content">
         <div class="row">
                         <div class="col-sm-12">
            <div class="col-sm-6">
           
                             
        <div id="chart1" style="height:300px; "></div>

                           
                        

               </div>

               <div class="table-responsive">
                  <table id="jqgrid_statuser"></table>
                   <div id="pager_statuser"></div>
               
                <!-- /.widget-box -->
            </div>
       </div>  <div class="table-responsive">
         
                 <table id="jqgrid_valutazioni"></table>
                <button type="button" class="btn btn-primary green" id="btnexport"><i class="ace-icon gicon file btn-yellow "></i> Esporta Excel</button>
         </div>
             </div>
        </div> 
        
    </form>

    <!-- page specific	<!-- basic scripts -->

    <!--[if !IE]> -->
    <script type="text/javascript">
        window.jQuery || document.write("<script src='assets/js/jquery.min.js'>" + "<" + "/script>");
    </script>

    <!-- <![endif]-->

    <!--[if IE]>
<script type="text/javascript">
 window.jQuery || document.write("<script src='../assets/js/jquery1x.min.js'>"+"<"+"/script>");
</script>
<![endif]-->
    <script type="text/javascript">
        if ('ontouchstart' in document.documentElement) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
    </script>
    <script src="assets/js/bootstrap.min.js"></script>

    <!-- page specific plugin scripts -->

    <!--[if lte IE 8]>
		  <script src="../assets/js/excanvas.min.js"></script>
		<![endif]-->
   

    <!-- ace scripts -->
    <script src="assets/js/ace-elements.min.js"></script>
    <script src="assets/js/ace.min.js"></script>


    <script src="assets/js/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>


    <!-- Don't touch this! -->


    <script class="include" type="text/javascript" src="assets/jquery.jqplot.min.js"></script>
    <script type="text/javascript" src="assets/syntaxhighlighter/scripts/shCore.min.js"></script>
    <script type="text/javascript" src="assets/syntaxhighlighter/scripts/shBrushJScript.min.js"></script>
    <script type="text/javascript" src="assets/syntaxhighlighter/scripts/shBrushXml.min.js"></script>
<!-- End Don't touch this! -->

        <script language="javascript" type="text/javascript" src="assets/plugins/jqplot.canvasTextRenderer.min.js"></script>
    <script language="javascript" type="text/javascript" src="assets//plugins/jqplot.canvasAxisTickRenderer.min.js"></script>
    <script language="javascript" type="text/javascript" src="assets/plugins/jqplot.dateAxisRenderer.min.js"></script>
    <script language="javascript" type="text/javascript" src="assets/plugins/jqplot.cursor.min.js"></script>


    <script>


        jQuery(function ($) {


          

            var line1 = [];
            


            grid_selector = "#jqgrid_statuser";
            pager_selector = "#pager_statuser";
            idcourse = <%= Request.QueryString("idCourse")%> +"";
            iduser =<%= Request.QueryString("idUser")%> +"";
            grid_selector1 = "#jqgrid_valutazioni";

            //$(window).on('resize.jqGrid', function () {
            //    $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
            //})
            //resize on sidebar collapse/expand
            //var parent_column = $(grid_selector).closest('[class*="col-"]');
            //$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
            //    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //        $(grid_selector).jqGrid('setGridWidth', parent_column.width());
            //    }
            //})



            jQuery(grid_selector).jqGrid({
             
               
                height:345,
                url: "AdminAjaxLMS.aspx?op=modstats&oper=getStatUserCourse&idCourse=" + idcourse + "&iduser=" + iduser + "",
                datatype: "json",
                colNames: ['INIZIO SESSIONE', 'FINE SESSIONE', 'DURATA SESSIONE', 'N.OPERAZIONI'],
                colModel: [
                             { name: 'enterTime', index: 'enterTime', width: 140, editable: false, editoptions: { size: "400", maxlength: "30" } },
                         { name: 'lastTime', index: 'lastTime', width: 140, editable: false, editrules: { edithidden: true }, editoptions: { size: "120", maxlength: "30" } },
                          { name: 'duration', index: 'duration', width:140, editable: false, editrules: { edithidden: true }, editoptions: { size: "120", maxlength: "30" } },
                            { name: 'numop', index: 'numop', width: 140, editable: false, editrules: { edithidden: true }, editoptions: { size: "120", maxlength: "30" } },

                ],

                rowNum: 20,
                rowTotal: 2000,
                rowList: [20, 30, 50],
                loadonce: true,
                mtype: "GET",
                rownumbers: true,
                rownumWidth: 40,
                gridview: true,
                sortname: 'id',
                loadComplete: function (data) {

                    for (var i = 1; i <= data.length; i++) {
                        var rowData = data[i-1];
                      
                        var daystart = rowData.enterTime.split('/')[0];
                        var dayend = rowData.lastTime.split('/')[0];
                        var dateParts = rowData.enterTime.split("/");
                        var mydate= dateParts[2].substr(0,4) + '-' + dateParts[1] + '-' + dateParts[0];
                        line1.push([mydate, i]);

                                           }
                    

                    //var line1 = [['2008-08-12 4:00PM', 1], ['2008-09-12 4:00PM', 6.5], ['2008-10-12 4:00PM', 5.7], ['2008-11-12 4:00PM', 9], ['2008-12-12 4:00PM', 31]];


                    //var plot1 = $.jqplot('chart1', [line1], {
                    //    title: 'Statistiche utente nel corso',
                    //    axes: {
                    //        xaxis: {
                    //            renderer: $.jqplot.DateAxisRenderer
                    //        }
                    //    },
                    //    series: [{ lineWidth:1, markerOptions: { style: 'square' } }]
                    //});

                    $.jqplot.config.enablePlugins = true;

                
                    if (data.length > 1) {
                        var plot1 = $.jqplot('chart1', [line1], {
                            title: 'Grafico Accessi al corso',
                            axes: {
                                xaxis: {
                                    renderer: $.jqplot.DateAxisRenderer,
                                    rendererOptions: {
                                        tickRenderer: $.jqplot.CanvasAxisTickRenderer
                                    },
                                    tickOptions: {
                                        fontSize: '10pt',
                                        fontFamily: 'Tahoma',
                                        angle: -40
                                    }
                                },
                                yaxis: {
                                    rendererOptions: {
                                        tickRenderer: $.jqplot.CanvasAxisTickRenderer
                                    },
                                    tickOptions: {
                                        fontSize: '10pt',
                                        fontFamily: 'Tahoma',
                                        angle: 30
                                    }
                                }
                            },
                            series: [{ lineWidth: 4, markerOptions: { style: 'circle' } }],

                        });
                    }
                },



                editurl: "AdminAjaxLMS.aspx",
                caption: "Statistiche di accesso",
                autowidth: false

            });

            //navButtons
            jQuery(grid_selector).jqGrid('navGrid', pager_selector,
                { 	//navbar options
                    search: true,
                    searchicon: 'icon-search orange',
                    refresh: true,
                    refreshicon: 'icon-refresh green',
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
                    }                    ,
                    multipleSearch: true,
                    
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




            jQuery(grid_selector1).jqGrid({
             
                subGrid: true,
                subGridOptions: {
                    
                    plusicon: "ace-icon glyphicon glyphicon-plus bigger-110 blue",
                    minusicon: "ace-icon glyphicon glyphicon-minus bigger-110 blue",
                    openicon: "ace-icon fa fa-chevron-right center orange"
                },
                //for this example we are using local data
                subGridRowExpanded: function (subgridDivId, rowId) {
                    var subgridTableId = subgridDivId + "_t";
                    $("#" + subgridDivId).html("<table id='" + subgridTableId + "'></table>");
                    $("#" + subgridTableId).jqGrid({
                        height: 645,
                        datatype: 'json',
                        url: 'adminAjaxLMS.aspx?op=modcourse&oper=getreportvalutazioni&idcourse=' + idcourse + '&iduser=' + rowId,
                        colNames: ['','Nome', 'Punteggio', 'Tentativi', 'Stato', 'Data Inizio'],
                        colModel: [
                            { name: 'act',  width: 40 },
                            { name: 'test', index: 'test', width: 600 },
                            { name: 'score', index: 'score', width: 200 },
                            { name: 'number_of_attempt', index: 'number_of_attempt', width: 200 },
                            { name: 'score_status', index: 'score_status', width: 200 },
                            { name: 'date_attempt', index: ' date_attempt', width: 200 }
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

                                  
                                   var  iduser=<%=request.querystring("iduser") %>
                                  
                                            state = "<a href='#' onclick=\"openwindowStat('test'," + rowData.id + ",\'" + rowData.test + "\'," + iduser + ");\" ><i class='icon-file bigger-160' ></i></a>";
                                         $("#" + subgridTableId).jqGrid('setRowData', rowData.id, { act: state});
                                

                                }
                            }


                        },
                    });
                },

                url: 'adminAjaxLms.aspx?op=modcourse&oper=getvalutazionibyiduser&iduser='+ iduser + '&idcourse=' + idcourse,
                datatype: "json",
                colNames: [ 'Id',  'Media Tentativi','Media Punteggio'],
                colModel: [

                   
                    { name: 'id', index: 'id', key: true, hidden: true, width: 20, sorttype: "int", editable: false },
                    { name: 'mediatentativi', index: 'mediatentativi', width: 130, sortable: true, editable: true, editoptions: { size: "100", maxlength: "200" } },


                     { name: 'media', index: 'media', width: 80, sortable: true, editable: true, editoptions: { size: "100", maxlength: "200" } },
               
                ],

                height: 445,
                rowNum: 100,
                rowList: [100, 150, 200],
                gridview: true,
                loadonce: true,
             
                sortname: 'nominativo',
                ignoreCase: true,
                sortorder: "asc",
                autowidth: true,
                viewsortcols: [true, 'vertical', true],
                viewrecords: true,
                loadComplete: function (data) {

                    var rowIds = $(grid_selector1).getDataIDs();
                    $.each(rowIds, function (index, rowId) {
                        $(grid_selector1).expandSubGridRow(rowId);
                    });
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


                editurl: "GetJson.aspx?op=modcourse",//nothing is saved
                caption: "Dettaglio test di verifica",

                //grouping: true,
                //groupingView: {
                //    groupField: ['category'],
                //    groupText: ['<b>{0} </b>'],
                //    groupCollapse: false
                //},
            });


            $('#btnexport').click(function () {


                $.ajax({
                    url: 'AdminAjaxLMS.aspx?op=modcourse&oper=downloadexcel&idCourse=' + idcourse + '&iduser=' + iduser,
                    type: 'GET',
                    datatype: 'json',
                    success: function (data) {
                        top.location.href = data;
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert("Errore");

                    }
                });

            });

           
            





        });



        function openwindowStat(obj, reference, title, iduser) {

           

            ViewModalObj("WfStatObj.aspx?obj=" + obj + "&reference=" + reference + "&iduser=" + iduser + "", title);



        }


    </script>

</body>
</html>
