<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WFStatObjUser.aspx.vb" Inherits="TrainingSchool.WFStatObjUser" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="description" content="Training School" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- basic styles -->

    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
     <script src="LMSContent/tinylms/lib/soapclient.js"></script>
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
  
    <script src="assets/js/jquery-ui-1.10.3.custom.min.js"></script>
      <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/typeahead-bs2.min.js"></script>

    <!-- ace scripts -->

    <script src="assets/js/ace-elements.min.js"></script>


</head>
      <body>
    <form id="form1" runat="server">
   

        <div id="viewstat" visible="false" runat="server">
        </div>

      

        <div  id="quizPage" runat="server">
 </div>

        
         
        





       

        <!-- page specific plugin scripts -->

        <script src="assets/js/jquery-ui-1.10.3.custom.min.js"></script>
        <script src="assets/js/jquery.ui.touch-punch.min.js"></script>
        <script src="assets/js/jquery.slimscroll.min.js"></script>



        <!-- inline scripts related to this page -->

        <script type="text/javascript">

           


            jQuery(function ($) {
                $('.accordion').on('hide', function (e) {
                    $(e.target).prev().children(0).addClass('collapsed');
                })
                $('.accordion').on('show', function (e) {
                    $(e.target).prev().children(0).removeClass('collapsed');
                })
            });
        </script>
    </form>
</body>
</html>
