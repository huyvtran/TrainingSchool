<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFMarketPlace.aspx.vb" Inherits="TrainingSchool.WFMarketplace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-content">
        <div class="page-header">
            <h1>MarketPlace
                <small><i class="icon-double-angle-right"></i></small>
            </h1>
        </div>
        <!-- /.page-header -->

        <p>In quest'area sarà possibile ordinare libri di utilità per i docenti, libri per piani studio degli studenti, nonchè materiali di cancelleria per studenti e docenti</p><div class="row">
           
            <div class="col-sm-8">
            
            <div class="widget-box widget-color-pink ui-sortable-handle" id="widget-box-9">
												<div class="widget-header">
													<h5 class="widget-title">Modulo di richiesta</h5>

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
                                    <label for="selsedi" class="col-sm-3 control-label no-padding-right">Categoria *</label>
                                    <div class="col-sm-9">
                                        <span class="input-icon  input-icon-right">
                                            <select name="tipo" id="tipo">

                                                <option selected="selected" value="libri di testo">Libri di testo</option>
                                                <option value="cancelleria">Cancelleria</option>
                                                <option value="computer">Computer/tablet</option>
                                                
                                            </select></span>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label for="selsedi" class="col-sm-3 control-label no-padding-right">Materia *</label>
                                    <div class="col-sm-9">
                                        <span class="input-icon  input-icon-right">

                                            <select class="form-control " id="materia" data-validation="length" data-validation-length="1-50">
                                                <option value="">Seleziona materia</option>
                                                <option value="arte">Arte</option>
                                                <option value="biologia">Biologia</option>
                                                <option value="chimica">Chimica</option>
                                                <option value="disegno">Disegno</option>
                                                <option value="economia">Economia</option>
                                                <option value="educazione fisica">Educazione fisica</option>
                                                <option value="filosofia">Filosofia</option>
                                                <option value="fisica">Fisica</option>
                                                <option value="geografia">Geografia</option>
                                                <option value="geologia">Geologia</option>
                                                <option value="informatica">Informatica</option>
                                                <option value="ingegneria">Ingegneria</option>
                                                <option value="insegnante di sostegno">Insegnante di sostegno</option>
                                                <option value="italiano">Italiano</option>
                                                <option value="latino e greco">Latino e Greco</option>
                                                <option value="letteratura">Letteratura</option>
                                                <option value="lingua straniera">Lingua straniera</option>
                                                <option value="matematica">Matematica</option>
                                                <option value="musica">Musica</option>
                                                <option value="psicologia e pedagogia">Psicologia e Pedagogia</option>
                                                <option value="religioni">Religioni</option>
                                                <option value="scienze">Scienze</option>
                                                <option value="scienze dell'antichità">Scienze dell'Antichità</option>
                                                <option value="scienze giuridiche">Scienze giuridiche</option>
                                                <option value="scienze politiche e sociali">Scienze politiche e sociali</option>
                                                <option value="storia">Storia</option>
                                                <option value="storia dell'arte">Storia dell'Arte</option>
                                                <option value="separator" disabled="">–––––––––––––––––––––––––––</option>
                                                <option value="altro">Altro</option>
                                            </select>
                                        </span>
                                    </div>
                                </div>
                             
                                 <div class="form-group">
                                <label for="notesess" class="col-sm-3 control-label no-padding-right">Classe</label>
                                <div class="col-sm-9">

                                    <input type="text" class="col-sm-6" name="classe" id="classe" />
                                </div>
                            </div>

                    <div class="form-group">
                                <label for="notesess" class="col-sm-3 control-label no-padding-right">Messaggio</label>
                                <div class="col-sm-9">
                                                                        <input type="text" class="col-sm-6" name="messaggio" id="messaggio" />
                                </div>
                            </div>


                  
                            </div>
													</div>

													<div class="widget-toolbox padding-8 clearfix">
														
														
															   <button type="button" id="btnsend" class="btn btn-success btn-sm pull-right"><i class="ace icon-envelope"></i> Invia richiesta</button>

														
													</div>
												</div>
											</div>
            
            </div>
        </div>
      </div>  
        <script>


                   $('#btnsend').click(function () {

        $.ajax({
            url: 'adminAjaxLMS.aspx?op=sendmailmarket',
            type: 'POST',
            data: {  messaggio: $("#messaggio").val(), classe: $("#classe").val(),materia:$("#materia").val(),categoria:$("#categoria option:selected").val() },
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
