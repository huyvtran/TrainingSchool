<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFCourseStructure.aspx.vb" Inherits="TrainingSchool.WFCourseStructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="pagebody">

        <div class="page-content">
            <div class="page-header">
                <h1>Corso Selezionato: 
				<small><i class="icon-double-angle-right"></i></small><b>modalità studente: <a href="WFCourseRoom.aspx?course=<%=Request.QueryString("idcourse")%>&name=<%=Request.QueryString("name") %>"><%=Request.QueryString("name")%></a></b>
                </h1>
            </div>
            <!-- /.page-header -->

            <div class="row">
                <div class="col-xs-12">
                    <!-- PAGE CONTENT BEGINS -->


                    <div id="dialog_folder" class="hide">
                        <div class="row">
                            <div class="col-xs-12 ">

                                <div class="form-group">
                                    <label class="col-sm-3 control-label no-padding-right" for="txtname_html">Titolo </label>

                                    <div class="col-sm-9">
                                        <input type="text" maxlength="255" size="55" maxlength="100" id="txtname_folder" placeholder="Titolo oggetto" class="col-xs-20 col-sm-12" />
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


                   

                    <!-- The container for the uploaded files -->
                    <div id="files" class="files"></div>

                    <menu id="nestable-menu">

                        <%--    <button class="btn btn-warning btn-sm" type="button" data-action="expand-all">Espandi</button>
					<button class="btn btn-warning btn-sm" type="button" data-action="collapse-all">Collassa</button>
					<button id="update" class="btn btn-cog btn-sm">Aggiorna</button>--%>
                    </menu>

                    <div class="col-sm-12">
                        <div class="col-xs-6">
                            <%--  <h3 class="header smaller lighter red">Organizzazione Corso</h3>--%>

                            <div class="space-4"></div>
                            Rilascia qui gli oggetti
                <div style="background-color: #428bca1f" class="dd" runat="server" id="nestable">
                </div>
                            <div class="space-4"></div>
                            <hr />
                            <a href="#" id="btnfolder" class="btn btn-purple btn-sm">Crea Cartella</a>
                            <asp:Button ID="btndeleteall" CssClass="btn btn-danger btn-sm" runat="server" Text="Elimina tutti gli oggetti" />
                            <button id="btnapplyprerequisites" class="btn btn-success btn-sm">Applica propedeuticità agli oggetti</button>
                        </div>
                        <div class="col-xs-6">



                            <div class="tab-content">
                                <div id="new" class="tab-pane in active">

                                    <h3 class="header smaller lighter red">i miei oggetti didattici</h3>

                                    <div id="objavailable" runat="server" class="tabbable">
                                        <ul class="nav nav-tabs" data-tabs="tabs" id="myTab">
                                            <li class="active"><a data-toggle="tab" class="tabscorm" href="#ctl00_ContentPlaceHolder1_scorm"><i class="green icon-home bigger-110"></i>VIDEO/SCORM
                                        <asp:Label ID="scormcount" runat="server"></asp:Label>
                                            </a></li>
                                            <li><a data-toggle="tab" class="tabtest" href="#ctl00_ContentPlaceHolder1_test">TEST 
                                        <asp:Label ID="testcount" runat="server"></asp:Label>
                                            </a></li>
                                            <li><a data-toggle="tab" class="tabitem" href="#ctl00_ContentPlaceHolder1_item">FILE
                                        <asp:Label ID="itemcount" runat="server"></asp:Label>
                                            </a></li>
                                            <li><a data-toggle="tab" class="tabpoll" href="#ctl00_ContentPlaceHolder1_poll">SONDAGGIO
                                        <asp:Label ID="pollcount" runat="server"></asp:Label>
                                            </a></li>
                                            <li><a data-toggle="tab" class="tabfaq" href="#ctl00_ContentPlaceHolder1_faq">FAQ
                                        <asp:Label ID="faqcount" runat="server"></asp:Label>
                                            </a></li>
                                            <li><a data-toggle="tab" class="tabhtml" href="#ctl00_ContentPlaceHolder1_html">HTML
                                        <asp:Label ID="htmlcount" runat="server"></asp:Label>
                                            </a></li>
                                        </ul>
                                        <div class="tab-content">
                                            <div id="scorm" runat="server" class="tab-pane in active">

                                                <a href="#" onclick="ViewModalObjTest('WFmakescorm.aspx?idscorm_package=0', 'Crea VideoLezione', '')" id="btnscorm" class="btn btn-warning btn-sm">Crea Scorm</a>
                                                <img id="loadscorm" src="assets/images/loading.gif" />
                                                <ol class="dd" runat="server" id="scorm_list">
                                                    <li class="dd-item 0" data-id="0"></li>
                                                </ol>
                                            </div>
                                            <div id="test" runat="server" class="tab-pane">

                                                <a href="#" onclick="ViewModalObjTest('WFmaketest.aspx?idtest=0', 'Crea test', '')" id="btntest" class="btn btn-danger btn-sm">Crea Test</a>
                                                <img id="loadtest" src="assets/images/loading.gif" />
                                                <ol class="dd" runat="server" id="test_list">
                                                    <li class="dd-item 0" data-id="0"></li>
                                                </ol>
                                            </div>
                                            <div id="item" runat="server" class="tab-pane">

                                              <a href="#" onclick="ViewModalObjTest('WFmakeItem.aspx?id_item=0', 'Crea Sondaggio', '')" id="btnitem" class="btn btn-danger btn-sm">Crea File</a>

                                                <img id="loaditem" src="assets/images/loading.gif" />
                                                <ol class="dd" runat="server" id="item_list">
                                                    <li class="dd-item 0" data-id="0"></li>
                                                </ol>
                                            </div>
                                            <div id="poll" runat="server" class="tab-pane">

                                                <a href="#" onclick="ViewModalObjTest('WFmakepoll.aspx?id_poll=0', 'Crea Sondaggio', '')" id="btnpoll" class="btn btn-danger btn-sm">Crea Sondaggio</a>

                                                <img id="loadpoll" src="assets/images/loading.gif" />
                                                <ol class="dd" runat="server" id="poll_list">
                                                    <li class="dd-item 0" data-id="0"></li>
                                                </ol>
                                            </div>
                                            <div id="faq" runat="server" class="tab-pane">

                                                <a href="#" onclick="ViewModalObjTest('WFmakefaq.aspx?idcategory=0', 'Crea Faq', '')" id="btnfaq" class="btn btn-danger btn-sm">Crea Faq</a>
                                                <img id="loadfaq" src="assets/images/loading.gif" />

                                                <ol class="dd" runat="server" id="faq_list">
                                                    <li class="dd-item 0" data-id="0"></li>
                                                </ol>
                                            </div>
                                            <div id="html" runat="server" class="tab-pane">

                                                <a href="#" onclick="ViewModalObjTest('WfMakehtml.aspx?idPage=0', 'Crea HTML', '')" id="btnhtml" class="btn btn-danger btn-sm">Crea HTML</a>

                                                <img id="loadhtml" src="assets/images/loading.gif" />

                                                <ol class="dd" runat="server" id="html_list">
                                                    <li class="dd-item 0" data-id="0"></li>
                                                </ol>
                                            </div>
                                        </div>
                                    </div>

                                </div>


                                <div id="available" class="tab-pane">

                                    <label for="form-field-select-1">Seleziona i Corsi</label>
                                    <asp:DropDownList ID="courselist" runat="server" CssClass="form-control"></asp:DropDownList><br />
                                    <button id="btncopy" class="btn btn-success btn-sm"><i class="ace-icon icon-paste"></i>Copia/incolla</button>
                                </div>

                                <hr />
                                <div class="dd" runat="server" id="nestableavailable">
                                </div>


                            </div>





                        </div>



                        <!-- PAGE CONTENT ENDS -->

                    </div>
                </div>
            </div>

        </div>
    </div>


    <!-- basic scripts -->

    <link rel="stylesheet" type="text/css" href="//cdnjs.cloudflare.com/ajax/libs/codemirror/3.20.0/codemirror.min.css" />
    <link rel="stylesheet" type="text/css" href="//cdnjs.cloudflare.com/ajax/libs/codemirror/3.20.0/theme/monokai.min.css" />
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/codemirror/3.20.0/codemirror.min.js"></script>
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/codemirror/3.20.0/mode/xml/xml.min.js"></script>
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/codemirror/2.36.0/formatting.min.js"></script>
     <script type="text/javascript" src=" https://cdnjs.cloudflare.com/ajax/libs/Nestable/2012-10-15/jquery.nestable.js"></script>
     <script src="assets/js/codeUpload/jquery.iframe-transport.js"></script>
    <script src="assets/js/codeUpload/jquery.fileupload.js"></script>




    <script type="text/javascript">


        $("#btnapplyprerequisites").on("click", function (e) {
            $.ajax({
                type: "POST",
                url: "WFCourseStructure.aspx?op=prerequisites&idcourse=<%=Request.QueryString("idcourse")%>",
                dataType: "json",
                success: function (data) {
                    alert("Operazione riuscita");
                    location.reload();
                },
                error: function (data) {

                    alert("Operazione riuscita");
                },
            });

        });


        jQuery(function ($) {



            var request = $.ajax({
                url: "WfCourseStructure.aspx",
                type: "GET",
                async: false,
                cache: false,
                data: { ThisCourse: "<%= Request.QueryString("idCourse")%>" },
                dataType: "html"
            });


            request.done(function (data) {

                jQuery('#<%= nestable.ClientID%>').append(data);

            });


             $("body").on("click", ".tabscorm", function (e) { //user click on remove text
            jQuery('#<%= scorm_list.ClientID%>').load("?loadobj=scormorg", function () {
                    $("#loadscorm").hide();
                });
            });


             $("body").on("click", ".tabtest", function (e) { //user click on remove text
            jQuery('#<%= test_list.ClientID%>').load("?loadobj=test", function () {
                    $("#loadtest").hide();
                });
            });

            $("body").on("click", ".tabitem", function (e) { //user click on remove text


                jQuery('#<%= item_list.ClientID%>').load("?loadobj=item", function () {
                    $("#loaditem").hide();
                });
            });
            $("body").on("click", ".tabtest", function (e) { //user click on remove text

                jQuery('#<%= test_list.ClientID %>').load("?loadobj=test", function () {
                    $("#loadtest").hide();
                });
            });


            $("body").on("click", ".tabhtml", function (e) {

                jQuery('#<%= html_list.ClientID%>').load("?loadobj=htmlpage", function () {
                    $("#loadhtml").hide();
                });
            });


            $("body").on("click", ".tabpoll", function (e) { 

                jQuery('#<%= poll_list.ClientID%>').load("?loadobj=poll", function () {
                    $("#loadpoll").hide();
                });
            });


            $("body").on("click", ".tabfaq", function (e) { //user click on remove text

                jQuery('#<%= faq_list.ClientID%>').load("?loadobj=faq", function () {
                    $("#loadfaq").hide();
                });

            });


       jQuery('#<%= scorm_list.ClientID%>').load("?loadobj=scorm", function () {
                $("#loadscorm").hide();
            });
        });

       



            $("#btncopy").on("click", function (e) {

                var update1 = window.JSON.stringify(jQuery('#<%= nestableavailable.ClientID%>').nestable('serialize'));

                var requestupdate1 = $.ajax({
                    type: "POST",
                    url: "WFCourseStructure.aspx?op=update&idCourse=<%= Request.QueryString("idCourse")%>",
                    data: update1,
                    dataType: "json"
                });


                requestupdate1.beforeSend(function () {
                    $('#loading').show();
                });

                requestupdate1.complete(function () {
                    $('#loading').hide();
                });

                requestupdate1.success(function (data) {
                    jQuery('#<%= nestable.ClientID%>').html('');
                    jQuery('#<%= nestable.ClientID%>').append(data.responseText);
                });
                requestupdate1.fail(function (data) {
                    jQuery('#<%= nestable.ClientID%>').html('');
                    jQuery('#<%= nestable.ClientID%>').append(data.responseText);
                });
                return false;
            });




            $("body").on("click", ".availablecourse", function (e) { //user click on remove text

                var request1 = $.ajax({
                    type: "POST",
                    url: "WfCourseStructure.aspx/LoadCorsi",
                    data: '',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                });


                request1.success(function (data) {
                    $('#<%=courselist.ClientID%>').empty().append($("<option></option>").val("[-]").html("Seleziona il corso"));

                    $.each(data.d, function () {
                        $('#<%=courselist.ClientID%>').append($("<option></option>").val(this['Value']).html(this['Text']));
                    });


                });


                request1.error(function (data) {
                    alert(data);
                });
            });




            $("#myTab").tabs({
                beforeLoad: function (event, ui) {
                    if (ui.tab.data("loaded")) {
                        event.preventDefault();
                        return false;
                    }
                    ui.jqXHR.error(function () {

                    });
                    ui.jqXHR.success(function () {
                        ui.tab.data("loaded", true);
                    });
                }
            });


            $('.dd').nestable({
            }).on('change', function (e) {
                e.stopPropagation();

            });


            $('#<%= nestable.ClientID%>').nestable({
            }).on('change', function (e) {


                var update1 = window.JSON.stringify($("#<%= nestable.ClientID%>").nestable('serialize'));

                var requestupdate1 = $.ajax({
                    type: "POST",
                    url: "WFCourseStructure.aspx?op=update&idCourse=<%= Request.QueryString("idCourse")%>",
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
                    url: "WFCourseStructure.aspx?op=update&idCourse=<%= Request.QueryString("idCourse")%>",
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
                var urllink = "WFCourseStructure.aspx?op=deletesource&id=" + arr[1] + "&objecttype=" + arr[3];



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

                var classObj = $(this).attr('data-obj');



                var requestupdate2 = $.ajax({
                    type: "POST",
                    url: "WFCourseStructure.aspx?op=deleteobjectsource&id=" + classObj,
                    dataType: "json"
                });


                requestupdate2.fail(function (data) {
                    location.reload()
                });


                requestupdate2.success(function (data) {

                    location.reload()

                });




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
                    title: "<div class='widget-header widget-header-small'><h4 style='color:red'  class='smaller'><i class='icon-ok'></i> Cartella</h4></div>",
                    title_html: true,
                    buttons: [
                        {
                            text: "Annulla",
                            "class": "btn btn-xs",
                            click: function () {
                                $(this).dialog("close");
                            }
                        },
                        {
                            text: "Salva",
                            "class": "btn btn-primary btn-xs",
                            click: function () {

                                var content = " <li class='dd-item dd-item2' data-id='new|0|" + jQuery('#txtname_folder').val() + "||'><div class='dd-handle dd2-handle'>" + jQuery('#txtname_folder').val() + "<div class='pull-right action-buttons'><a class='red deleteobject' data-obj='new|0|" + jQuery('#txtname_folder').val() + "||'  href='#'><i class='icon-trash bigger-130'></i></a></div></div>";


                                jQuery('#<%= nestable.ClientID%> > ol').append(content);


                                var updateFolder = window.JSON.stringify(jQuery("#<%= nestable.ClientID%>").nestable('serialize'));

                                var requestupdate3 = $.ajax({
                                    type: "POST",
                                    url: "WFCourseStructure.aspx?op=update&idCourse=<%= Request.QueryString("idCourse")%>",
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

            });






                function getPageName(url) {
                    var index = url.lastIndexOf(".") + 1;
                    var filenameWithExtension = url.substr(index);
                    var filename = filenameWithExtension.split(".")[0]; // <-- added this line
                    return filename;                                    // <-- added this line
                }

                function htmlEncode(value) {
                    return $('<div/>').text(value).html();
                }
                function htmlDecode(value) {
                    return $('<div/>').html(value).text();
                }
                function Viewer(obj, reference) {


                    if (obj == "scormorg") {
                        ViewModalObj("LMSContent/RunTimePlayer.aspx?reference=" + reference);
                        // top.location.href = "LMSCONTENT/RunTimePlayer.aspx?reference=" + reference;
                        // top.location.href = "WFViewScorm.aspx?reference=" + reference;
                    } else {
                        obj = obj.replace("'", "");
                        ViewModalObj("WfViewObj.aspx?obj=" + obj + "&reference=" + reference + "")

                    }


                }

                function openwindow(objecttype, reference, idcourse, isvisible, isterminator, title) {
                    ViewModalObj("/WFSetProp.aspx?objecttype=" + objecttype + "&idCourse=" + idcourse + "&ckvisible=" + isvisible + "&ckendcourse=" + isterminator + "&idOrg=" + reference + "&title=" + +escape(title) + "");
                }

                function openwindowEdit(objecttype, reference, title,anno,materia) {


                    switch (objecttype) {
                        case "test":
                            ViewModalObj("WFMakeTest.aspx?mod=modtest&idtest=" + reference + "&title=" + escape(title) + "&anno=" + escape(anno) + "&materia=" + escape(materia) + "");

                            break;

                        case "scormorg":
                            ViewModalObj("WFMakeScorm.aspx?idscorm_package=" + reference + "&title=" + escape(title) + "&anno=" + escape(anno) + "&materia=" + escape(materia) + "", 'Modifica Scorm');
                            break;
                        case "faq":
                            ViewModalObj("WFMakeFaq.aspx?idcategory=" + reference + "&title=" + escape(title) + "&anno=" + escape(anno) + "&materia=" + escape(materia) + "", 'Modifica Faq');
                            break;
                        case "htmlpage":
                            ViewModalObj("WFMakeHtml.aspx?idPage=" + reference + "&title=" + escape(title) + "&anno=" + escape(anno) + "&materia=" + escape(materia) + "", 'Modifica Html');
                            break;
                        case "poll":
                            ViewModalObj("WFMakePoll.aspx?id_poll=" + reference + "&title=" + escape(title) + "&anno=" + escape(anno) + "&materia=" + escape(materia) + "", 'Modifica Sondaggio');
                            break;
                        
                        case "item":
                            ViewModalObj("WFMakeItem.aspx?iditem=" + reference + "&title=" + escape(title) + "&anno=" + escape(anno) + "&materia=" + escape(materia) + "",'Modifica File');
                            break;
                        default:

                    }

                }



                function ViewModalObjTest(pdf_link, title, obj) {

                    h = (window.innerHeight - 150);


                    (function (a) {
                        a.createModal = function (b) {

                            defaults = { title: "", message: "!", closeButton: true, scrollable: false };
                            var b = a.extend({}, defaults, b);
                            //var c = (b.scrollable === false) ? 'style="z-index:1050;height:800;overflow-x:hidden;overflow-y: auto;"' : "";


                            html = '<div style="z-index:1080 !important;height:800;overflow-x:hidden;overflow-y: auto;" class="modal fade" id="myModal1">'; html += '<div class="modal-dialog">'; html += '<div class="modal-content">'; html += '<div class="modal-header">';


                            if (b.title.length > 0) {
                                html += '<h4 class="modal-title">' + b.title + "</h4>"
                            }

                            html += "</div>";
                            html += '<div class="modal-body" >';
                            html += b.message; html += "</div>";
                            html += '<div class="modalfooter">';
                            if (b.closeButton == true) {
                                if (obj == 'scorm') {
                                    html += '';
                                } else {
                                    html += '<button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">Chiudi</button>';


                                }
                            }
                            html += "</div>";
                            html += "</div>";
                            html += "</div>";
                            html += "</div>";
                            a("body").prepend(html);
                            a("#myModal1").modal({ backdrop: 'static', keyboard: false }).on("hidden.bs.modal", function () { a(this).remove(); callbacktest(); })
                        }
                    })(jQuery);


                    params = pdf_link.split('?')[1];

                    link = pdf_link.split('?')[0];

                    link = link + '?' + params;

                    var pdf_link = $(this).attr('href');

                    var iframe = '<div class="iframe-container"><iframe style="min-height:calc(100vh - 200px);width:100%"  frameborder=0  id="myframe" src="' + link + '"></iframe>'

                    $.createModal({
                        title: title,
                        height: '400px',
                        modal: true,
                        message: iframe,
                        closeButton: true,
                        scrollable: false
                    });
                    return false;

                }


                function callbacktest() {

                    jQuery('#<%= test_list.ClientID %>').load("?loadobj=test", function () {
            $("#loadtest").hide();
        });

        jQuery('#<%= poll_list.ClientID %>').load("?loadobj=poll", function () {
            $("#loadpoll").hide();
        });

        jQuery('#<%= faq_list.ClientID %>').load("?loadobj=faq", function () {
            $("#loadfaq").hide();
        });

      

        jQuery('#<%= html_list.ClientID%>').load("?loadobj=html", function () {
             $("#loadhtml").hide();
        })

                      $("#loadscorm").show();

     jQuery('#<%= scorm_list.ClientID%>').load("?loadobj=scorm", function () {
            $("#loadscorm").hide();
        })
     }

   

    </script>



</asp:Content>
