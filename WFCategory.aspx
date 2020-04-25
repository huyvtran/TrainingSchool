<%@ Page Title="" Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFCategory.aspx.vb" Inherits="TrainingSchool.WFCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <div class="page-content">
        <div class="page-header">
            <h1>Categorie Corsi 
            </h1>
        </div>
        <!-- /.page-header -->
        
                <div id="dialog_folder" class="hide">
                    <div class="row">
                        <div class="col-xs-12 ">

                            <div class="form-group">
                                <label class="col-sm-3 control-label no-padding-right" for="txtname_html">Titolo </label>

                                <div class="col-sm-9">
                                    <input type="text" maxlength="255" size="55" maxlength="255" id="txtname_folder" placeholder="Titolo oggetto" class="col-xs-20 col-sm-12" />
                                </div>
                            </div>
                            <br />
                            <div class="space-4"></div>
                            <div class="space-4"></div>
                            <div class="form-group">
                                <label class="col-sm-3 control-label no-padding-right" for="txtname_html"></label>

                               
                            </div>


                            <div class="space-4"></div>
                            <br />
                            <hr />

                        </div>
                    </div>
                </div>
        <div class="row">
            

                <menu id="nestable-menu">
                    <a href="#" id="btnfolder" class="btn btn-purple btn-sm">Crea Categoria</a>
                 
                </menu>

                <div class="col-sm-12">
                    <div class="col-xs-12">
                      <%--  <h3 class="header smaller lighter red">Organizzazione Corso</h3>--%>


                        <div class="dd" runat="server" id="nestable">
                        </div>
                    </div>
                  



                    <!-- PAGE CONTENT ENDS -->

                </div>
            </div>
 </div>

   
    <!-- basic scripts -->

    <link rel="stylesheet" type="text/css" href="//cdnjs.cloudflare.com/ajax/libs/codemirror/3.20.0/codemirror.min.css" />
    <link rel="stylesheet" type="text/css" href="//cdnjs.cloudflare.com/ajax/libs/codemirror/3.20.0/theme/monokai.min.css" />
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/codemirror/3.20.0/codemirror.min.js"></script>
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/codemirror/3.20.0/mode/xml/xml.min.js"></script>
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/codemirror/2.36.0/formatting.min.js"></script>

    <!-- include summernote css/js-->
    <link rel="stylesheet" href="assets/summernote/summernote.css" />
    <script src="assets/js/summernote/summernote.min.js"></script>

    <script src="assets/js/jquery.nestable.min.js"></script>


    <script src="assets/js/codeUpload/jquery.iframe-transport.js"></script>
    <script src="assets/js/codeUpload/jquery.fileupload.js"></script>




    <script type="text/javascript">



     

        jQuery(function ($) {




        $('.dd').nestable({
            }).on('change', function (e) {
                e.stopPropagation();

            });


            $('#<%= nestable.ClientID%>').nestable({
            }).on('change', function (e) {


                var update1 = window.JSON.stringify($("#<%= nestable.ClientID%>").nestable('serialize'));

                var requestupdate1 = $.ajax({
                    type: "POST",
                    url: "WFCategory.aspx?op=update",
                    data: update1,
                    dataType: "json"
                });

                requestupdate1.fail(function (data) {
                    jQuery('#<%= nestable.ClientID%>').html('');
                    jQuery('#<%= nestable.ClientID%>').append(data.responseText);
                });


                requestupdate1.done(function (data) {

                    jQuery('#<%= nestable.ClientID%>').html('');
                    jQuery('#<%= nestable.ClientID%>').append(data.responseText);
                });

            });





            $(document).on('click', '.deleteobject', function () {

                var classObj = $(this).attr('data-obj');

                var datiobjdelete = "delete" + $('.' + classObj).attr('data-id');
                var datiobj = $('.' + classObj).attr('data-id');

                datiobj = datiobj.replace("\\'", "\\\\'");
                datiobjdelete = datiobjdelete.replace("\\'", "\\\\'");

                var update2 = window.JSON.stringify($("#<%= nestable.ClientID%>").nestable('serialize'));

                update2 = update2.replace(datiobj, datiobjdelete)

                var requestupdate2 = $.ajax({
                    type: "POST",
                    url: "WFCategory.aspx?op=update",
                    data: update2,
                    dataType: "json"
                });


                requestupdate2.fail(function (data) {

                    jQuery('#<%= nestable.ClientID%>').html('');
                    jQuery('#<%= nestable.ClientID%>').append(data.responseText);
                });


                requestupdate2.success(function (data) {

                    requestupdate1.fail(function (data) {
                        jQuery('#<%= nestable.ClientID%>').html('');
                        jQuery('#<%= nestable.ClientID%>').append(data.responseText);
                    });


                });

            });

                        $('#btnDelteYes').click(function () {
                            var id = $('#myModal').data('id');
                           
                        
                            var arr = id.split('|');
                            var urllink = "WFCategory.aspx?op=deletesource&id=" + arr[1] + "&objecttype=" + arr[3];



                            var requestdelete = $.ajax({
                                url: urllink,
                                type: "GET",
                                datatype: "html"
                            });


                            requestdelete.fail(function (data) {
                                alert(data);

                            });


                            requestdelete.success(function (data) {
                                var replaced = id;
                                replaced = replaced.replace("/\\/g", "\\\\");
                                $("li[data-id='" + replaced + "']").remove();
                                $('#myModal').modal('hide');
                            });
                           
                        });

            $(document).on('click', '.deleteobjectsource', function () {

                var dataobj = $(this).attr('data-obj');


                $('#myModal').data('id', dataobj).modal('show');

               

               
            });



            $('.dd-handle a').on('mousedown', function (e) {
                e.stopPropagation();
            });



          
            $("#btnfolder").on('click', function (e) {
                e.preventDefault();

                var dialog = $("#dialog_folder").removeClass('hide').dialog({
                    modal: true,
                    height: 200,
                    width: 500,
                    title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='icon-ok'></i> Cartella</h4></div>",
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

                                var content = " <li class='dd-item dd-item2' data-id='new|0|" + jQuery('#txtname_folder').val() + "||'><div class='dd-handle dd2-handle'>" + jQuery('#txtname_folder').val() + "<div class='pull-right action-buttons'><a class='red deleteobject' data-obj='new|0|" + jQuery('#txtname_folder').val() + "||'  href='#'><i class='icon-trash bigger-130'></i></a></div></div>";


                                jQuery('#<%= nestable.ClientID%> > ol').append(content);


                                var updateFolder = window.JSON.stringify(jQuery("#<%= nestable.ClientID%>").nestable('serialize'));

                                var requestupdate3 = $.ajax({
                                    type: "POST",
                                    url: "WFCategory.aspx?op=update&idCourse=<%= Request.QueryString("idCourse")%>",
                                    data: updateFolder,
                                    dataType: "json"
                                });

                                requestupdate3.fail(function (data) {
                                    jQuery('#<%= nestable.ClientID%>').html('');
                                    jQuery('#<%= nestable.ClientID%>').append(data.responseText);
                                });


                                requestupdate3.done(function (data) {

                                    jQuery('#<%= nestable.ClientID%>').html('');
                                    jQuery('#<%= nestable.ClientID%>').append(data.responseText);
                                });


                                $(this).dialog("close");

                            }
                        }
                    ]
                });
                /**
        dialog.data( "uiDialog" )._title = function(title) {
            title.html( this.options.title );
        };
        **/
            });



        

            $('#nestable-menu').on('click', function (e) {
                var target = $(e.target),
                    action = target.data('action');
                if (action === 'expand-all') {
                    $('.dd').nestable('expandAll');
                }
                if (action === 'collapse-all') {
                    $('.dd').nestable('collapseAll');
                }
            });


               var request = $.ajax({
                url: "WfCategory.aspx",
                type: "GET",
                async: false,
                cache: false,
                data: { ThisCat:1 },
                dataType: "html"
            });


            request.done(function (data) {

                jQuery('#<%= nestable.ClientID%>').append(data);
                return false;
            });

        });


                   

                    function openwindow(objecttype, reference, idcourse, isvisible, isterminator, title) {
                        ViewModalObj("/WFSetProp.aspx?objecttype=" + objecttype + "&idCourse=" + idcourse + "&ckvisible=" + isvisible + "&ckendcourse=" + isterminator + "&idOrg=" + reference + "&title=" + title + "");
                    }







    </script>



</asp:Content>
