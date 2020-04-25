<%@ Page Title="Format Html" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSsw.Master" CodeBehind="WFSettingTemplate.aspx.vb" Inherits="TrainingSchool.WFImpostazioni" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   
    <script>

         CKEDITOR.editorConfig = function (config) {
                config.toolbarGroups = [
                    { name: 'clipboard', groups: ['clipboard', 'undo'] },
                    { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
                    { name: 'links', groups: ['links'] },
                    { name: 'insert', groups: ['insert'] },
                    { name: 'forms', groups: ['forms'] },
                    { name: 'tools', groups: ['tools'] },
                    { name: 'document', groups: ['mode', 'document', 'doctools'] },
                    { name: 'others', groups: ['others'] },
                    '/',
                    { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
                    { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
                    { name: 'styles', groups: ['styles'] },
                    { name: 'colors', groups: ['colors'] },
                    { name: 'about', groups: ['about'] }
                ];
                config.stylesSet = 'my_styles';
             config.extraPlugins = 'imagebrowser';

                config.height = '800px';
            };

    </script>

    <div class="page-content">
        <div class="page-header">
            <h1>Gestione Template
                <small><i class="icon-double-angle-right"></i></small>
            </h1>
        </div>
        <!-- /.page-header -->
        <div class="row">
            <div class="col-xs-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right" for="form-field-1">Seleziona il template </label>



                        <div class="col-md-5">
                            <select id="selformat" class=" form-control" data-style="btn-primary" name="selformat"></select>
                        </div>
                    </div>
                    <%--       <div class="form-group">
                              <label class="col-sm-3 control-label no-padding-right" for="form-field-1">Seleziona il template </label>

                        
                          <div class="col-md-5">
                                <button type="button" id="btnnewformat" class="btn-success" >Inserisci</button>
                    </div>
                   </div>--%>


                    <textarea name="editor1" id="editor1" rows="100" cols="20">
               
            </textarea>
                    <hr />
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right" for="form-field-1">Orientamento </label>

                        <div class="col-sm-9">
                            <select id="orientation" name="orientation">

                                <option value="0">Verticale</option>
                                <option value="1">Orizzontale</option>

                            </select>
                        </div>
                    </div>



                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right" for="form-field-1">Intestazione </label>

                        <div class="col-sm-9">
                            <select id="intestazione" name="intestazione">
                                <option value="1">Si</option>
                               
                                <option value="0">No</option>


                            </select>

                        </div>
                    </div>
                    <div id="headerdiv"></div>
                </div>


                <script>
                   
                    CKEDITOR.replace('editor1', {
                      filebrowserImageUploadUrl:  'HUpload.ashx?load=image',
                        width: '95%',
                        height: 400
                    });
                </script>
                <input type="hidden" name="wysiwyg-value" />

                <div class="form-actions align-right clearfix">
                    <button type="reset" class="pull-left btn">
                        <i class="ace-icon fa fa-retweet bigger-110"></i>
                        Annulla
                    </button>



                    <button type="button" id="btnpreview" class="btn btn-primary pull-right">
                        <i class="ace-icon fa fa-check bigger-110"></i>
                        Anteprima
                    </button> - 
                     <button type="button" id="btnsave" class="btn btn-success pull-right">
                        <i class="ace-icon fa fa-check bigger-110"></i>
                        Salva
                    </button>
                </div>


            </div>
        </div>

        <input type="hidden" id="mailf" />
    </div>




    <script src="assets/js/bootstrap-select.js"></script>

    <script type="text/javascript">


        function callformat() {

            $.ajax({
                type: "POST",
                url: "loadselect.aspx?op=format",
                data: { selformat: $('#selformat option:selected').val() },
                 beforeSend: function () {
                        $("#wait").css("display", "block");
                    },

                    oncomplete: function (data, status) {
                        $("#wait").css("display", "none");

                    },
                success: function (response) {
                    eval(response);
                    
                     $("#wait").css("display", "none");
                },
                fail: function (response) {
                    eval(response);

                        $("#wait").css("display", "none");
                    
                }
            });
        }


        jQuery(function ($) {

            
            callformat()

          

            $('#selformat').on('change', function () {


                $.ajax({
                    url: 'adminajaxlms.aspx?op=getmailformat&format=' + $('#selformat option:selected').val(),
                    type: 'POST',
                    data: new FormData(),
                    processData: false,
                    contentType: false,
                     beforeSend: function () {
                        $("#wait").css("display", "block");
                    },

                    oncomplete: function (data, status) {
                        $("#wait").css("display", "none");

                    },
                    success: function (data) {
                        CKEDITOR.instances['editor1'].setData(data);
                        $("#wait").css("display", "none");
                    },
                    fail: function (data) {
                        CKEDITOR.instances['editor1'].setData(data);
                        $("#wait").css("display", "none");
                    }
        
                });




                $('#btnpreview').on("click", function (e) {
                    e.preventDefault();
                     e.stopImmediatePropagation();
                            window.open('adminajaxlms.aspx?op=preview&intestazione=' + $('#intestazione option:selected').val() + '&orientation=' + $('#orientation option:selected').val() + '&format=' + $('#selformat option:selected').val());
                });


                $('#btnsave').on("click", function (e) {
                    e.preventDefault();
                     e.stopImmediatePropagation();
                    $.ajax({
                        url: 'adminajaxlms.aspx?op=updateformatmail&format=' + $('#selformat option:selected').val(),
                        datatype: 'html',
                        type: 'POST',
                         beforeSend: function () {
                        $("#wait").css("display", "block");
                    },

                    oncomplete: function (data, status) {
                        $("#wait").css("display", "none");

                    },
                        data: {orientation: $('#orientation option:selected').val(),header: $('#intestazione option:selected').val(), mailformat: CKEDITOR.instances['editor1'].getData() },
                        success: function (data) {
                              $("#wait").css("display", "none");
                            alert("Salvataggio avvenuto con successo");
                            return false;
                        },
                        fail: function (data)
                        {  $("#wait").css("display", "none");
                            return false;
                        }
                    });

                });

                $('#btnnewformat').on("click", function (e) {
                    e.preventDefault();
                     e.stopImmediatePropagation();
                    $.ajax({
                        url: 'adminajaxlms.aspx?op=insertformatmail&formatnew=' + $("formatnew").val(),
                        datatype: 'json',
                        type: 'POST',
                      
                        success: function (data) {
                            window.open('adminajaxlms.aspx?op=preview&intestazione=' + $('#intestazione option:selected').val() + '&orientation=' + $('#orientation option:selected').val() + '&format=' + $('#selformat option:selected').val());
                        },
                        fail: function (data) {

                        }
                    });


                });



            });


        

           

        });


    </script>




</asp:Content>