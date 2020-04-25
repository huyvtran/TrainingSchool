<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSsw.Master" CodeBehind="WfReportSurvey.aspx.vb" Inherits="TrainingSchool.WfReportSurvey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="page-content">
         <div class="page-header">
            <h1>Report Sondaggi
                <small><i class="icon-double-angle-right"></i></small>
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-xs-12">


                <div class="widget-box transparent">
                    <div class="widget-header widget-header-flat">
                       
                        <div class="widget-toolbar">
                            <a href="#" data-action="collapse">
                                <i class="icon-chevron-up"></i>
                            </a>
                        </div>
                    </div>

                    <div class="widget-body">
                        <div class="widget-main no-padding">
                            <label for="form-field-select-1">Seleziona il corso</label>
                            <asp:DropDownList ID="courselist" runat="server" CssClass="form-control"></asp:DropDownList>

                            <div id="divquest" runat="server"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



    </div>




    <script type="text/javascript">


        jQuery(function ($) {



            var request1 = $.ajax({
                type: "POST",
                url: "WfReportSurvey.aspx/LoadCorsi",
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            });


            request1.success(function (msg) {
                $('#<%=courselist.ClientID%>').empty().append($("<option></option>").val("[-]").html("Seleziona il corso"));
                $.each(msg.d, function () {
                    $('#<%=courselist.ClientID%>').append($("<option></option>").val(this['Value']).html(this['Text']));
                });



            });


            request1.error(function (msg) {
                alert(msg);
            });

        });



        jQuery('#<%=courselist.ClientID%>').change(function () {

            jQuery('#<%= divquest.ClientID%>').html("");

            var request3 = $.ajax({
                url: "WfReportSurvey.aspx?idcourse=" + $(this).val() + "",
                type: "GET",
                async: false,
                cache: false,
                data: { AvailableCourse: +$(this).val() },
                dataType: "html"
            });

            request3.done(function (data) {
                jQuery('#<%= divquest.ClientID%>').append(data);
                });

        });
    </script>
</asp:Content>
