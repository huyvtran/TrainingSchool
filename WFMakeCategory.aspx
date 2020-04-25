<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WFMakeCategory.aspx.vb" Inherits="TrainingSchool.WFMakeCategory" %>


<!DOCTYPE html>
<html lang="it">
<head>
    <meta charset="utf-8" />
    <title>Categorie Corsi</title>

    <meta name="description" content="Common form elements and layouts" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- basic styles -->

    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />

    <!--[if IE 7]>
		  <link rel="stylesheet" href="assets/css/font-awesome-ie7.min.css" />
		<![endif]-->

    <!-- page specific plugin styles -->

    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.custom.min.css" />
 

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
</head>

<body>



    <div class="page-content">
        <div class="page-header">
            <h1>Categorie Corsi: 
								<small>
                                    <i class="icon-double-angle-right"></i>
                                    <%=Request.QueryString("title") %>
                                </small>
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->

                <div style="height: 500px; overflow-y: scroll" class="col-xs-6">
                    <div class="form-group" id="InputsWrapper"></div>
                </div>

                <div style="height: 500px; overflow-y: scroll" class="col-xs-6">
                    <%-- <form id="form1" runat="server" class="form-horizontal" role="form">
                    </form>--%>
                    <div id="Categoria_list" runat="server" cssclass="panel-group accordion-style1 accordion-style2">
                    </div>

                </div>


                <div class="space-4"></div>


            </div>

        </div>
      
    </div>
    
    <!-- /.page-content -->
  
        




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

     <script src="assets/js/lmsrb.js"></script>
    <!-- LMSRB styles -->
     <link rel="stylesheet" href="assets/css/lmsrb.css" />


    <script type="text/javascript">
        if ("ontouchend" in document) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
    </script>
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/typeahead-bs2.min.js"></script>

    <!-- page specific plugin scripts -->

    <!--[if lte IE 8]>
		  <script src="assets/js/excanvas.min.js"></script>
		<![endif]-->



    <script type="text/javascript">



        $(document).on('click', '.updateobject', function () {

            dataobjupdate = $(this).attr('data-obj');

            var cat = $("#cat_" + dataobjupdate + "").text();
           
            $("#cat").val(cat);
          


        });



        $(document).on('click', '.deleteobject', function () {

            var dataobj = $(this).attr('data-obj');

            var arr = dataobj.split('|');
            var urllink = "WFMakeCategory.aspx?op=deletesource&id=" + dataobj
            var requestdelete = $.ajax({
                url: urllink,
                type: "GET",
                datatype: "html"
            });


            requestdelete.fail(function (data) {
                alert(data);

            });


            requestdelete.success(function (data) {

                location.reload()


            });

        });

        $(document).ready(function () {


            InitLmsRb();

            var InputsWrapper = $("#InputsWrapper"); //Input boxes wrapper ID


            var FieldCount = 1;


            $(InputsWrapper).append(' <label for="fieldcat"> Categoria </label><div class="input-group"><input style="width:365px; padding: 2px" type="text" name="mytext[]" id="cat" /> <span class="input-group-btn"></div><br>   <button  class="addclass btn btn-info" > <i class="icon-ok bigger-110"></i>Aggiungi Categoria</button>');



        });





        $("body").on("click", ".addclass", function (e) { //user click on remove text
            e.preventDefault();

            $cat = $(this).parent('div').find('input').val();
                 if ($cat == '' ) {
                alert("Impossibile salvare campi vuoti!")
            } else {

                var request4 = $.ajax({
                    type: "POST",
                    url: "AdminAjaxLMS.aspx?op=modcourse&oper=addcat",
                    data: { cat: $cat },
                    dataType: "html"
                });


                request4.success(function (data) {

                    alert(data);

                    location.reload();

                });


            }
        });



        function ordercat(id, sequence, offset) {


            sequence = sequence++;

            var request4 = $.ajax({
                type: "GET",
                url: "?op=modcourse&oper=updatesequence&idcategory=" + <%=request.querystring("idcategory") %> +"&sequence=" + sequence + "&offset=" + offset ,
                dataType: "html"
            });


            request4.success(function () {



                location.reload();

            });

        }




       

       

          








    </script>


</body>
</html>
