<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WFViewObj.aspx.vb" Inherits="TrainingSchool.WFViewObjLMS" %>

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
      <script type="text/javascript" src="assets/js/quiz.js?ver=5"></script>
       <script type="text/javascript" src="assets/js/Poll.js"></script>

       <script type="text/javascript"   src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-AMS-MML_HTMLorMML"> </script>

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
     <script src="assets/js/lmsrb.js"></script>

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

        <div id="faq_list" visible="false" runat="server" cssclass="panel-group accordion-style1 accordion-style2">
        </div>
        
        <div id="htmlpage" visible="false"  runat="server" >
        </div>

        <div data-role="page" visible="false" id="quizPage" runat="server">

            <div data-role="header">
                
            </div>

            <div data-role="content">
             
                <h3 class="timer2 element" style="font-size: 50px;"></h3>
                <div class="quizdisplay"></div>
            </div>

            <div data-role="footer">
              
            </div>

        </div>

        
         <div data-role="page" visible="false" id="pollPage" runat="server">

            <div data-role="header">
                
            </div>

            <div data-role="content">
                <div class="polldisplay"></div>
            </div>

            <div data-role="footer">
              
            </div>

        </div>


      

        <!-- page specific plugin scripts -->

       
         <script src="assets/js/timer/countid.js"></script>
   


        <!-- inline scripts related to this page -->

        <script type="text/javascript">
            Date.prototype.addSeconds = function (h) {
                this.setSeconds(this.getSeconds() + h);
                return this;
            }
            var date = new Date().addSeconds(5) / 1000

           
     window.onunload = refreshParent;
            function refreshParent() {
               
              location.reload();
            }



            jQuery(function ($) {

                $('.timer2').countid({
                    clock: true,
                    dateTplRemaining: "%H:%M:%S",
                    dateTplElapsed: "%H:%M:%S",
                    complete: function (el) {
                        el.animate({ 'font-size': '50px' })
                    }
                })
                $("body").on("contextmenu", function (e) {
                    return false;
                });
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
