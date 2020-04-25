<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WFChat.aspx.vb" Inherits="TrainingSchool.WFChat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Aula Virtuale</title>


    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />

    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />

    <!-- bootstrap & fontawesome -->
    <link rel="stylesheet" href="assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />

    <!-- page specific plugin styles -->

    <!-- text fonts -->
    <link rel="stylesheet" href="assets/css/ace-fonts.css" />

    <!-- ace styles -->
    <link rel="stylesheet" href="assets/css/ace.min.css" />

    <!--[if lte IE 9]>
			<link rel="stylesheet" href="assets/css/ace-part2.min.css" />
		<![endif]-->
    <link rel="stylesheet" href="assets/css/ace-skins.min.css" />
    <link rel="stylesheet" href="assets/css/ace-rtl.min.css" />

    <!--[if lte IE 9]>
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


    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/typeahead-bs2.min.js"></script>
    <script src="assets/js/jquery-ui-1.10.3.custom.min.js"></script>


    <!-- ace scripts -->

    <script src="assets/js/ace-elements.min.js"></script>

    <style>
        .wordwrap {
            white-space: pre-wrap; /* CSS3 */
            white-space: -moz-pre-wrap; /* Firefox */
            white-space: -pre-wrap; /* Opera <7 */
            white-space: -o-pre-wrap; /* Opera 7 */
            word-wrap: break-word; /* IE */
        }
    </style>

</head>


<body class="no-skin">
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-sm-12">
                <div class="col-sm-8">

                    <div class="widget-box transparent" id="recent-box">
                        <div class="widget-header">
                            <h4 class="widget-title lighter smaller">
                                <i class="ace-icon fa fa-rss orange"></i>Chat
                            </h4>

                            <div class="widget-toolbar no-border">
                                <ul class="nav nav-tabs" id="recent-tab">
                                    <li class="active">
                                        <a data-toggle="tab" href="#task-tab">Stanze</a>
                                    </li>

                                </ul>
                            </div>
                        </div>
                    </div>

                    <div class="tab-content padding-8">
                        <div id="room-tab" class="tab-pane active">
                            <div class="widget-box">
                                <div class="widget-header">
                                    <h4 class="widget-title lighter smaller">
                                        <i class="ace-icon fa fa-comment blue"></i>
                                        Stanza: 	<%=Session("Coursename")%>
                                    </h4>
                                </div>

                                <div class="widget-body">
                                    <div class="widget-main no-padding">

                                        <div id="contentdialogs" runat="server" class="dialogs">
                                        </div>

                                        <!-- /section:pages/dashboard.conversations -->

                                        <div class="form-actions">
                                            <div class="input-group">
                                                <textarea placeholder="scrivi il tuo messaggio" cols="2" rows="5" class="form-control" id="sendmsg" name="message"></textarea>
                                                <span class="input-group-btn">
                                                    <button id="sendchat" class="btn btn-sm btn-info no-radius" type="button">
                                                        <i class="ace-icon fa fa-share"></i>
                                                        Invia
                                                    </button>
                                                </span>
                                            </div>
                                        </div>

                                    </div>
                                    <!-- /.widget-main -->
                                </div>
                                <!-- /.widget-body -->
                            </div>
                            <!-- /.widget-box -->
                        </div>


                    </div>
                </div>
                <div class="col-sm-4">

                   

                    <div class="widget-box">
                        <div class="widget-header">
                            <h4 class="widget-title lighter smaller">
                                <i class="ace-icon fa fa-comment blue"></i>
                                Corsisti Collegati:
                            </h4>
                        </div>

                        <div class="widget-body">
                            <div class="widget-main no-padding">
                                <ul id="UserList" class="item-list">
                                </ul>

                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

    </form>


    <a href="#" id="btn-scroll-up" class="btn-scroll-up btn btn-sm btn-inverse">
        <i class="icon-double-angle-up icon-only bigger-110"></i>
    </a>

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

    <!-- page specific plugin scripts -->

    <!--[if lte IE 8]>
		  <script src="assets/js/excanvas.min.js"></script>
		<![endif]-->

    <script src="assets/js/jquery-ui-1.10.3.custom.min.js"></script>
    <script src="assets/js/jquery.ui.touch-punch.min.js"></script>
    <script src="assets/js/jquery.slimscroll.min.js"></script>

    <!-- ace scripts -->

    <script src="assets/js/ace-elements.min.js"></script>
    <script src="assets/js/ace.min.js"></script>

    <!-- inline scripts related to this page -->


    <script type="text/javascript">

     
                 jQuery(function ($) {

                     $('.dialogs').slimScroll({
                         height: '380px',
                         start: 'bottom'

                     });


                     $('.item-list').slimScroll({
                         height: '170px',
                         start: 'top'

                     });

                     setInterval("LoadChat()", 2000);

                 })



                 jQuery("#sendchat").on("click", function (e) {

                     InsertInChat();

                 });


                 function LoadChat() {

                     var requestupdate1 = $.ajax({
                         type: "POST",
                         url: "?op=load",
                         datatype: "json"
                     });




                     requestupdate1.done(function (data) {

                         jQuery('#contentdialogs').empty();
                         jQuery('#contentdialogs').append(data).trigger('create');

                         $('#contentdialogs').animate({ scrollTop: $('#contentdialogs').prop("scrollHeight") }, 400);


                     });



                     var requestupdate2 = $.ajax({
                         type: "POST",
                         url: "?op=loaduserlist",
                         datatype: "json"
                     });




                     requestupdate2.done(function (data) {

                         jQuery('#UserList').empty();
                         jQuery('#UserList').append(data).trigger('create');



                     });



                 }

                 function InsertInChat() {

                     var content = $("#sendmsg").val();
                     $("#sendmsg").empty();
                     var requestupdate3 = $.ajax({
                         type: "POST",
                         url: "?op=insert",
                         data: { message: content },
                         dataType: "json"
                     });


                     $('#recent-box [data-rel="tooltip"]').tooltip({ placement: tooltip_placement });
                     function tooltip_placement(context, source) {
                         var $source = $(source);
                         var $parent = $source.closest('.tab-content')
                         var off1 = $parent.offset();
                         var w1 = $parent.width();

                         var off2 = $source.offset();
                         //var w2 = $source.width();

                         if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) return 'right';
                         return 'left';
                     }


                     requestupdate3.done(function (data) {
                         jQuery('#contentdialogs').empty();
                         jQuery('#contentdialogs').append(data).trigger('create');
                         $('#contentdialogs').animate({ scrollTop: $('#contentdialogs').prop("scrollHeight") }, 400);

                     });



                 }


                 </script>


</body>
</html>
