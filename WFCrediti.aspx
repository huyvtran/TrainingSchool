<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFCrediti.aspx.vb" Inherits="TrainingSchool.WFCrediti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <div class="page-content">
        <div class="page-header">
            <h1>Percorso Crediti Professionali
                <small><i class="icon-double-angle-right"></i></small>
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->

                <div class="col-sm-8">

                    <div class="widget-box widget-color-pink ui-sortable-handle" id="widget-box-9">
                        <div class="widget-header">
                            <h5 class="widget-title">Modulo di richiesta - a quale corso di perfezionamento sei interessato?</h5>

                            <div class="widget-toolbar">
                                <a href="#" data-action="collapse">
                                    <i class="1 ace-icon fa fa-chevron-up bigger-125"></i>
                                </a>
                            </div>

                            <div class="widget-toolbar no-border">
                            </div>
                        </div>

                        <div class="widget-body">
                            <div class="widget-main">
                                <div class="form-horizontal">

                                    

                                    <div class="form-group">
                                        <label for="notesess" class="col-sm-3 control-label no-padding-right">Messaggio</label>
                                        <div class="col-sm-9">
                                            <textarea cols="80" rows="10" class="col-sm-6" name="messaggio" id="messaggio" ></textarea>
                                        </div>
                                    </div>



                                </div>
                            </div>

                            <div class="widget-toolbox padding-8 clearfix">


                                <button type="button" id="btnsend" class="btn btn-success btn-sm pull-right"><i class="ace icon-envelope"></i>Invia richiesta</button>


                            </div>
                        </div>
                    </div>

                </div>
                
            </div>
        </div>
    </div>
    <!-- page specific plugin scripts -->


   


    <script type="text/javascript">
       

            $('#btnsend').click(function () {

                $.ajax({
                    url: 'adminAjaxLMS.aspx?op=sendmailcrediti',
                    type: 'POST',
                    data: { messaggio: $("#messaggio").val(), crediti: $("#crediti").val()},
                    datatype: 'json',
                    success: function (data) {
                        ShowAlert(data);

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert("Errore");

                    }
                });
            });


    </script>
</asp:Content>
