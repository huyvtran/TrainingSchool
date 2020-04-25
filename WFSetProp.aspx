<%@ Page Language="vb" ValidateRequest="false" AutoEventWireup="false" CodeBehind="WFSetProp.aspx.vb" Inherits="TrainingSchool.WfSetProp" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="description" content="Cerco offro lavoro nel settore assicurativo e finanziario" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

   <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    	<link rel="stylesheet" href="assets/css/datepicker.css" />
		<link rel="stylesheet" href="assets/css/ui.jqgrid.css" />




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
    <script src="assets/js/jquery-ui-1.10.3.custom.min.js"></script>


    <!-- ace scripts -->

    <script src="assets/js/ace-elements.min.js"></script>

     <!-- ace LMSRB -->
     <script src="assets/js/lmsrb.js"></script>

</head>
<body >
    <div id="pagebody">
    <form id="form1" runat="server">
        

    <div class="page-content">
        <div class="page-header">
            <h1>Gestione Oggetti: <small><i class="icon-double-angle-right"></i></small>  <b><asp:Label ID="titleobj" runat="server" ></asp:Label></b>
               
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->


                <div class="row">
                    <div class="col-xs-12">

                        <div class="col-xs-6">
                            <table id="jqgrid_propsavailable"></table>
                            <div id="pager_propsavailable"></div>
                        </div>

                           
                        <div class="col-xs-4">
                                <div class="form-group">
                                <label class="col-sm-3 control-label no-padding-right" for="txttitle">Titolo </label>

                                <div class="col-sm-9">
                                    <label>
                                        <input id="txttitle" name="txttitle" maxlength="255" class="col-xs-22 col-sm-22"  size="55" type="text" />
                                        <span class="lbl"></span>
                                    </label>
                                </div>
                            </div>
                               <div class="space-4"><
                                  </div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label no-padding-right" for="ckendcourse">Fine Corso </label>

                                <div class="col-sm-9">
                                    <label>
                                        <input name="ckendcourse" id="ckendcourse" class="ace ace-switch" type="checkbox" />
                                        <span class="lbl"></span>
                                    </label>
                                </div>
                            </div>
                            <div class="space-4"></div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label no-padding-right" for="ckvisible">Visibile </label>

                                <div class="col-sm-9">
                                    <label>
                                        <input id="ckvisible" name="ckvisible" class="ace ace-switch" type="checkbox" />
                                        <span class="lbl"></span>
                                    </label>
                                </div>
                            </div>
                       
                            <div class="space-4"></div>
                        </div>

                    </div>
                
                     

                </div>
                <div class="clearfix form-actions">
                    <div class="col-md-offset-4 col-md-9">
                        <button id="btnAssignProperty" class="btn btn-info btn-sm" type="button">
                            <i class="icon-ok bigger-110"></i>
                        Aggiorna Oggetto Didattico
                        </button>
                        
											
                    </div>
                </div>

                <!-- PAGE CONTENT ENDS -->
            </div>
        </div>
    </div>
   
 </form>
     
        
        
        <!-- page specific plugin scripts -->

    <script src="assets/js/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="assets/js/jqGrid/i18n/grid.locale-it.js"></script>
   

    <script>


        jQuery(function ($) {

            InitLmsRb();


            grid_selector = "#jqgrid_propsavailable";
            pager_selector = "#pager_propsavailable";
            isterminator = <%= Request.QueryString("ckendcourse")%> +"";
            isvisible =<%= Request.QueryString("ckvisible")%> +"";
            objecttype = '<%= Request.QueryString("objecttype")%>' + "";
            txttitle = '<%= Request.QueryString("title")%>' +"";


          

            if (isterminator=="1") {
            $('input[name=ckendcourse]').attr('checked', true);
            }

            $('input[name=txttitle]').val(txttitle);

            if (isvisible=="1") {
            $('input[name=ckvisible]').attr('checked', true);
            }


            jQuery(grid_selector).jqGrid({
                //direction: "rtl",
                //data: grid_data,
                //datatype: "local",

                height: 400,
                 url: "AdminAjaxLMS.aspx?op=modprops&oper=propsavailable&idCourse= <%= Request.QueryString("idCourse")%>&idOrg=<%= Request.QueryString("idOrg")%> ",
                datatype: "json",
                colNames: ['ID', 'TITLE','PREREQUISITES'],
                colModel: [
                        { name: 'idOrg', index: 'idOrg', width: 60, sorttype: "int", editable: false },
                        { name: 'title', index: 'title', width: 400, editable: false, editoptions: { size: "400", maxlength: "30" } },
                         { name: 'prerequisites', width: 120, editable: true, hidden: true, editable: true, editrules: { edithidden: true }, editoptions: { size: "120", maxlength: "30" } },

                ],

                rowNum: 2000,
                rowTotal: 2000,
                rowList: [20, 30, 50],
                loadonce: true,
                mtype: "GET",
                rownumbers: false,
                rownumWidth: 40,
                gridview: true,
                sortname: 'id',
                multiselect: true,
                loadComplete: function(data) {
                    for (var i = 0; i < data.length; i++) {
                        var rowData = data[i];
                        var idOrg= "" + <%= Request.QueryString("idOrg")%> +"";

                        var prerequisites;
                        if (rowData.id == idOrg) {//update this to have your own check
                            var checkbox = $("#jqg_jqgrid_propsavailable_" + rowData.idOrg);
                            prerequisites = rowData.prerequisites;
                            checkbox.css("visibility", "hidden");
                            checkbox.attr("disabled", true);
                            $("#" + rowData.id).find("td").css("background-color", "orange");
                           
                        }
                    }

                    var checkbox1 = $("#jqg_jqgrid_propsavailable_" + prerequisites);
                    checkbox1.attr("checked", true);
                },
                
                

                editurl: "AdminAjaxLMS.aspx",
                caption: "Propedeuticità",
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






            jQuery("#btnAssignProperty").on("click", function (e) {
                e.preventDefault();


                var s;
                var ckendcourse;
                var ckvisible;
                var txtvideo;
                s = jQuery(grid_selector).jqGrid('getGridParam', 'selarrrow');

                if (jQuery('input[name=ckendcourse]').is(':checked')) {
                    ckendcourse = true;
                }else{
                    ckendcourse = false;

                }


                if (jQuery('input[name=ckvisible]').is(':checked')) {
                    ckvisible = true;
                } else {
                    ckvisible = false;

                }

                var title = jQuery('input[name=txttitle]').val();
                var txtvideo = jQuery('input[name=txtvideo').val();
                var callurl = "AdminAjaxLMS.aspx?op=modprops&oper=update&ckvisible="+ ckvisible +"&ckendcourse=" + ckendcourse + "&idOrg=" + <%=Request.QueryString("idOrg")%> + "&idCourse=" + <%=Request.QueryString("idCourse")%> +"&title="+ title + "&prerequisites=" + s;
             
                var request4 = $.ajax({
                    url: callurl,
                    type: "GET",
                    dataType: "html"
                });


                request4.success(function (data) {

                    alert(data);


                });
            });



        });

    </script>
    </div>
      <div id="modal" ></div>
</body>
</html>
