<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSsw.Master" CodeBehind="WFNewActivity.aspx.vb" Inherits="TrainingSchool.WFNewActivity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    
		<div class="titoloSezione text-center">
			<h1>
				Crea nuova attività &nbsp;
				<button class="btn btn-primary">
					<a href="javascript:history.back()">
						Torna al calendario&nbsp;&nbsp;<span class="oi oi-arrow-left"></span>
					</a>
				</button>
			</h1> 
		</div>

		<div class="container">
			<div class="row">
				<div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
					<h3>Gestione attività</h3>
					<div class="container">
						<div class="row">
							<div class="col-md-12">
								<div id="myCarousel" class="carousel slide" data-ride="carousel" data-interval="0">
									<!-- Wrapper for carousel items -->
									<div class="carousel-inner">
										<div class="item carousel-item active">
											<h4>Step 1: evento</h4>
											<div class="form-row">
		<div class="form-group col-sm-12 col-md-6 col-lg-6 col-xl-6">
			<div class="input-group mb-3">
				<div class="input-group-prepend">
					<span class="input-group-text" required>Nome attività</span>
				</div>
				<input type="text" class="form-control" >
			</div>
		</div>
		<div class="form-group col-sm-12 col-md-6 col-lg-6 col-xl-6">
			<div class="input-group mb-3">
				<div class="input-group-prepend">
					<label class="input-group-text" for="inputGroupSelect00" required>
						Tipo attività
					</label>
				</div>
				<select class="custom-select" id="inputGroupSelect00" required>
					<option selected>Seleziona:</option>
					<option value="1">Lezione semplice</option>
					<option value="2">Lezione mediante strumenti didattici</option>
					<option value="3">Lezione in diretta/streaming</option>
					<option value="4">Verifica esame scritto</option>
					<option value="5">Verifica esame orale</option>
					<option value="6">Ricevimento genitori</option>
				</select>
			</div>
		</div>
		<div class="form-group col-sm-12 col-md-6 col-lg-6 col-xl-6">
			<div class="input-group mb-3">
				<div class="input-group-prepend">
					<label class="input-group-text" for="inputGroupSelect01" required>
						Seleziona la materia:
					</label>
				</div>
				<select class="custom-select" id="inputGroupSelect01" required>
					<option selected>Seleziona:</option>
					<option value="1">Geografia</option>
					<option value="2">Algebra</option>
					<option value="3">Filosofia</option>
					<option value="4">Matematica</option>
					<option value="5">Fisica</option>
					<option value="6">Latino</option>
					<option value="7">Inglese</option>
				</select>
			</div>
		</div>
		<div class="form-group col-sm-12 col-md-6 col-lg-6 col-xl-6">
			<div class="input-group mb-3">
				<div class="input-group-prepend">
					<label class="input-group-text" for="inputGroupSelect02">
						Seleziona la classe:
					</label>
				</div>
				<select class="custom-select" id="inputGroupSelect02">
					<option selected>Seleziona:</option>
					<option value="1">Matematica 1A</option>
					<option value="2">Geografia 2F</option>
					<option value="3">Inglese 2B</option>

				</select>
			</div>
		</div>
	</div>	
	
	<div class="form-row">
		<div class="form-group col-md-6">
			<div class="input-group mb-3">
				<div class="input-group-prepend">
					<span class="input-group-text">Note personali</span>
				</div>
				<input type="text" class="form-control">
			</div>
		</div>
		<div class="form-group col-md-6">
			<div class="input-group mb-3">
				<div class="input-group-prepend">
					<span class="input-group-text">Note pubbliche</span>
				</div>
				<input type="text" class="form-control">
			</div>
		</div>
	</div>
	<div class="form-row">
		<div class="form-group col-sm-12 col-md-8 col-lg-6 col-xl-6">
			<div class="input-group mb-3">
				<div class="input-group date" id="datetimepicker1" data-target-input="nearest">
					<div class="input-group-append" data-target="#datetimepicker1" data-toggle="datetimepicker">
						<div class="input-group-text">
						Data Attività&nbsp;&nbsp;<i class="fa fa-calendar"></i></div>
					</div>
					<input type="text" class="form-control datetimepicker-input" data-target="#datetimepicker1"/>
				</div>

				<script type="text/javascript">
					$(function () {
						$('#datetimepicker1').datetimepicker();
						moment.locale('it');
					});
				</script>
			</div>
		</div>
		<div class="form-group col-sm-12 col-md-4 col-lg-6 col-xl-6">
			<div class="slidecontainer">
				<input type="range" min="15" max="480" value="50" step="15" class="slider" id="myRange">
				<p>L'attività durerà circa: <span id="demo"></span> minuti</p>
			</div>
			<script>
				var slider = document.getElementById("myRange");
				var output = document.getElementById("demo");
				output.innerHTML = slider.value;

				slider.oninput = function() {
				  output.innerHTML = this.value;
				}
			</script>
		</div>
	</div>

<div class="row bottoni">
	<div class="col-xs-4 col-sm-4 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-primary btn-sm btn-block">Invia avviso</button>
	</div>
	<div class="col-xs-4 col-sm-4 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-warning btn-sm btn-block">Segnala</button>
	</div>
	<div class="col-xs-4 col-sm-4 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-dark btn-sm btn-block">Azzera</button>
	</div>
	<div class="col-xs-0 col-sm-0 col-md-2 col-lg-2 col-xl-2"></div>
	<div class="col-xs-6 col-sm-6 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-success btn-sm btn-block" onclick="javascript:mostra()">Salva</button>
	</div>
	<div class="col-xs-6 col-sm-6 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-danger btn-sm btn-block">Cancella</button>
	</div>
</div>
										</div>	
										<div class="item carousel-item">
											<h4>Step 2: didattica</h4>
											 <a href="#carica" class="btn btn-primary" data-toggle="collapse" style="margin-bottom: 20px;">Carica nuovo materiale &nbsp;&nbsp; <span class="oi oi-data-transfer-upload"></span></a>

<div id="carica" class="collapse">
	<div class="form-row">
		<div class="form-group col-sm-12 col-md-6 col-lg-6 col-xl-6">
			<div class="custom-file">
				<input type="file" class="custom-file-input" id="customFile" value="sfoglia" name="Your label here.">
				<label class="custom-file-label" for="customFile">Scegli il file da caricare</label>
			</div>
			<script>
                // Per far apparire il nome del file nella label
                $(".custom-file-input").on("change", function () {
                    var fileName = $(this).val().split("\\").pop();
                    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
                });
			</script> 
		</div>
		<div class="form-group col-sm-12 col-md-6 col-lg-6 col-xl-6">
			<div class="custom-control custom-switch">
				<input type="checkbox" class="custom-control-input" id="switch1">
				<label class="custom-control-label" for="switch1">E' necessario che lo studente carichi un documento?</label>
			</div>
		</div>
	</div>
	<div class="form-row">
		<div class="form-group col-sm-12 col-md-6 col-lg-6 col-xl-6">
			<p>Associa l'attività didattica ai seguenti studenti:</p>
			<div class="contenitoreConvocati">
				<div class="custom-control custom-checkbox selezionaTutti">
					<input type="checkbox" class="custom-control-input check" id="checkStudenti" checked>
					<label class="custom-control-label" for="checkAll">Seleziona tutti</label>
				</div>
				<div class="custom-control custom-checkbox elenco">
					<input type="checkbox" class="custom-control-input check" id="defaultChecked1" checked>
					<label class="custom-control-label" for="defaultChecked1">Ermenegildo Chiorbolini</label>
				</div>
				<div class="custom-control custom-checkbox elenco">
					<input type="checkbox" class="custom-control-input check" id="defaultChecked2" checked>
					<label class="custom-control-label" for="defaultChecked2">Ludovica Visconti</label>
				</div>
				<div class="custom-control custom-checkbox elenco">
					<input type="checkbox" class="custom-control-input check" id="defaultChecked3" checked>
					<label class="custom-control-label" for="defaultChecked3">Ermenegildo Chiorbolini</label>
				</div>
				<div class="custom-control custom-checkbox elenco">
					<input type="checkbox" class="custom-control-input check" id="defaultChecked4" checked>
					<label class="custom-control-label" for="defaultChecked4">Ludovica Visconti</label>
				</div>
				<div class="custom-control custom-checkbox elenco">
					<input type="checkbox" class="custom-control-input check" id="defaultChecked5" checked>
					<label class="custom-control-label" for="defaultChecked5">Ermenegildo Chiorbolini</label>
				</div>
				<div class="custom-control custom-checkbox elenco">
					<input type="checkbox" class="custom-control-input check" id="defaultChecked6" checked>
					<label class="custom-control-label" for="defaultChecked6">Ludovica Visconti</label>
				</div>
				<div class="custom-control custom-checkbox elenco">
					<input type="checkbox" class="custom-control-input check" id="defaultChecked6" checked>
					<label class="custom-control-label" for="defaultChecked6">Ludovica Visconti</label>
				</div>
				<div class="custom-control custom-checkbox elenco">
					<input type="checkbox" class="custom-control-input check" id="defaultChecked6" checked>
					<label class="custom-control-label" for="defaultChecked6">Ludovica Visconti</label>
				</div>
				<div class="custom-control custom-checkbox elenco">
					<input type="checkbox" class="custom-control-input check" id="defaultChecked6" checked>
					<label class="custom-control-label" for="defaultChecked6">Ludovica Visconti</label>
				</div>
			</div>
			<script type="text/javascript">
                $("#checkStudenti").click(function () {
                    $(".check").prop('checked', $(this).prop('checked'));
                });
			</script>
		</div>
		<div class="form-group col-sm-12 col-md-6 col-lg-6 col-xl-6">
		<p>L'attività didattica include video?</p>
			<div class="input-group mb-3">
				<div class="input-group-prepend">
					<span class="input-group-text">Indirizzo Youtube</span>
				</div>
				<input type="text" class="form-control" >
			</div>
			<div class="input-group mb-3">
				<textarea class="form-control" rows="5" id="descrizione" placeholder=" Inserisci una descrizione del video"></textarea>
			</div>
		</div>
	</div>
	<div class="row bottoni">
		<div class="col-xs-6 col-sm-6 col-md-6 col-lg-4 col-xl-4">
			<button type="button" class="btn btn-dark btn-md btn-block">Azzera</button>
		</div>
		<div class="col-xs-6 col-sm-6 col-md-6 col-lg-4 col-xl-4">
			<button type="button" class="btn btn-success btn-md btn-block">Carica</button>
		</div>
	</div>
</div> 
<div class="materiale table-responsive">
	<table class="table table-striped" cellpadding="0" cellspacing="0">
		<thead>
			<tr>
				<th colspan="8">Materiale caricato</th>
			</tr>
			<tr class="titolo">
				
				<th><span class="oi oi-wrench"></span></th>
				<th><span class="oi oi-eye"></span></th>
				<th><span class="oi oi-data-transfer-download"></span></th>
				<th><i class="fa fa-info-circle" aria-hidden="true"></th>
				<th><span class="oi oi-paperclip"></span></th>
				<th><span class=""><img src="img/student.png" width="30" /></span></th>
				<th><span class="oi oi-clipboard"></span></th>
				<th><span class="oi oi-circle-x"></span></th>
			</tr>
		</thead>
		<tbody>
			<tr>
				<td><a href="#" title="Modifica"><span class="oi oi-wrench"></span></a></td>
				<td><a href="#" title="anteprima"><span class="oi oi-eye"></span></a></td>
				<td><a href="#" title="scarica documento">nome documento</a></td>
				<td>descrizione del documento</td>
				<td class="faicone">
					<a href="https://www.youtube.it" target="_blank" title="visualizza il video su youtube">
						<i class="fa fa-youtube" aria-hidden="true"></i>
					</a>
				</td>
				<td>
					<div class="dropdown">
					  <button class="btn btn-secondary dropdown-toggle btn-sm" type="button" id="dropdownMenuButton2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
					  </button>
					  <div class="dropdown-menu student" aria-labelledby="dropdownMenuButton2">
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download status"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
					  </div>
						</div>
				</td>
				<td class="faicone">
					<a href="#" title="scarica report PDF">
						<i class="fa fa-file-pdf-o" aria-hidden="true"></i>
					</a>
					<a href="#" title="scarica report Excel">
						<i class="fa fa-file-excel-o" aria-hidden="true"></i>
					</a>
				</td>
				<td class="faicone cancella"><a href="#" title="cancella documento"><span class="oi oi-circle-x"></span></a></td>
			</tr>
			<tr>
				<td><a href="#" title="Modifica"><span class="oi oi-wrench"></span></a></td>
				<td><a href="#" title="anteprima"><span class="oi oi-eye"></span></a></td>
				<td><a href="#"title="scarica documento">Metodologia della ricerca sociale comparata</a></td>
				<td>descrizione del documento</td>
				<td class="faicone">
					<a href="https://www.youtube.it" target="_blank" title="visualizza il video su youtube">
						<i class="fa fa-youtube" aria-hidden="true"></i>
					</a>
				</td>
				<td>
					<div class="dropdown">
					  <button class="btn btn-secondary dropdown-toggle btn-sm" type="button" id="dropdownMenuButton2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
					  </button>
					  <div class="dropdown-menu student" aria-labelledby="dropdownMenuButton2">
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download status"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div><div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
					  </div>
				</td>
				<td class="faicone">
					<a href="#" title="scarica report PDF">
						<i class="fa fa-file-pdf-o" aria-hidden="true"></i>
					</a>
					<a href="#" title="scarica report Excel">
						<i class="fa fa-file-excel-o" aria-hidden="true"></i>
					</a>
				</td>
				<td class="faicone cancella"><a href="#" title="cancella documento"><span class="oi oi-circle-x"></span></a></td>
			</tr>
			<tr>
				<td><a href="#" title="Modifica"><span class="oi oi-wrench"></span></a></td>
				<td><a href="#" title="anteprima"><span class="oi oi-eye"></span></a></td>
				<td><a href="#"title="scarica documento">Dimostrazione teorema di Pitagora</a></td>
				<td>Video descrizione sui metodi per dimostrare il teorema di Pitagora</td>
				<td class="faicone">
					<a href="https://www.youtube.it" target="_blank" title="visualizza il video su youtube">
						<i class="fa fa-youtube" aria-hidden="true"></i>
					</a>
				</td>
				<td>
					<div class="dropdown">
					  <button class="btn btn-secondary dropdown-toggle btn-sm" type="button" id="dropdownMenuButton2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
					  </button>
					  <div class="dropdown-menu student" aria-labelledby="dropdownMenuButton2">
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download status"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download"></span>
							</a>
						</div>
						<div class="dropdown-item">
							<span class="oi oi-check scaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div><div class="dropdown-item">
							<span class="oi oi-check noscaricato"></span>
							<span class="nome">Armandini Filippo</span>
							<a href="#" title="scarica documento">
								<span class="oi oi-data-transfer-download none"></span>
							</a>
						</div>
					  </div>
					</div>
				</td>
				<td class="faicone">
					<a href="#" title="scarica report PDF">
						<i class="fa fa-file-pdf-o" aria-hidden="true"></i>
					</a>
					<a href="#" title="scarica report Excel">
						<i class="fa fa-file-excel-o" aria-hidden="true"></i>
					</a>
				</td>
				<td class="cancella faicone"><a href="#" title="cancella documento"><span class="oi oi-circle-x"></span></a></td>
			</tr>
		</tbody>
	</table>
</div>

 <a href="#docenti" class="btn btn-primary" data-toggle="collapse" style="margin-bottom: 20px;">Aggiungi personale docente &nbsp;&nbsp; <span class="oi oi-plus"></span></a>

<div id="docenti" class="collapse">
	<p>Seleziona il personale docente da inserire all'interno dell'attività</p>
	<div class="contenitoreConvocati">
		<div class="custom-control custom-checkbox selezionaTutti">
			<input type="checkbox" class="custom-control-input check" id="checkAll" checked>
			<label class="custom-control-label" for="checkAll">Seleziona tutti</label>
		</div>
		<div class="custom-control custom-checkbox elenco">
			<input type="checkbox" class="custom-control-input check" id="defaultChecked1" checked>
			<label class="custom-control-label" for="defaultChecked1">Ermenegildo Chiorbolini</label>
		</div>
		<div class="custom-control custom-checkbox elenco">
			<input type="checkbox" class="custom-control-input check" id="defaultChecked2" checked>
			<label class="custom-control-label" for="defaultChecked2">Ludovica Visconti</label>
		</div>
		<div class="custom-control custom-checkbox elenco">
			<input type="checkbox" class="custom-control-input check" id="defaultChecked3" checked>
			<label class="custom-control-label" for="defaultChecked3">Ermenegildo Chiorbolini</label>
		</div>
		<div class="custom-control custom-checkbox elenco">
			<input type="checkbox" class="custom-control-input check" id="defaultChecked4" checked>
			<label class="custom-control-label" for="defaultChecked4">Ludovica Visconti</label>
		</div>
		<div class="custom-control custom-checkbox elenco">
			<input type="checkbox" class="custom-control-input check" id="defaultChecked5" checked>
			<label class="custom-control-label" for="defaultChecked5">Ermenegildo Chiorbolini</label>
		</div>
		<div class="custom-control custom-checkbox elenco">
			<input type="checkbox" class="custom-control-input check" id="defaultChecked6" checked>
			<label class="custom-control-label" for="defaultChecked6">Ludovica Visconti</label>
		</div>
	</div>
	<script type="text/javascript">
        $("#checkAll").click(function () {
            $(".check").prop('checked', $(this).prop('checked'));
        });
	</script>
</div>


	<ul class="nav nav-pills nav-justified">
		<li data-target="#myCarousel" data-slide-to="0" class="nav-item active" id=""><a href="#" class="nav-link"><strong>Torna indietro</strong></a></li>
		<li class="nav-item" id="salvaChiudi"><a href="javascript:history:back()" class="nav-link"><strong>Salva e chiudi</strong></a></li>
	</ul>

	<!--
<div class="row bottoni">
	<div class="col-xs-4 col-sm-4 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-primary btn-sm btn-block">Invia avviso</button>
	</div>
	<div class="col-xs-4 col-sm-4 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-warning btn-sm btn-block">Segnala</button>
	</div>
	<div class="col-xs-4 col-sm-4 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-dark btn-sm btn-block">Azzera</button>
	</div>
	<div class="col-xs-0 col-sm-0 col-md-2 col-lg-2 col-xl-2"></div>
	<div class="col-xs-6 col-sm-6 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-success btn-sm btn-block" onclick="javascript:mostra()">Salva</button>
	</div>
	<div class="col-xs-6 col-sm-6 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-danger btn-sm btn-block">Cancella</button>
	</div>
</div>
-->
										</div>
										<div class="item carousel-item">
											<img src="/examples/images/slides/workstation.jpg" alt="">
											<div class="carousel-caption">
												STEP 3
											</div>
										</div>
										<div class="item carousel-item">
											STEP4 
										</div>
									</div>
									<!-- End Carousel Inner -->
									<ul class="nav nav-pills nav-justified">
										<li data-target="#myCarousel" data-slide-to="1" class="nav-item active" id="prossimoStep" style="display: none"><a href="#" class="nav-link" onclick="javascript:nascondi()"><strong>Vai al prossimo step</strong></a></li>
									</ul>
								</div>
							</div>
						</div>
					</div>
								
				</div>
			</div>
							
		</div>
	

 
	

	<!--
<div class="row bottoni">
	<div class="col-xs-4 col-sm-4 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-primary btn-sm btn-block">Invia avviso</button>
	</div>
	<div class="col-xs-4 col-sm-4 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-warning btn-sm btn-block">Segnala</button>
	</div>
	<div class="col-xs-4 col-sm-4 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-dark btn-sm btn-block">Azzera</button>
	</div>
	<div class="col-xs-0 col-sm-0 col-md-2 col-lg-2 col-xl-2"></div>
	<div class="col-xs-6 col-sm-6 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-success btn-sm btn-block" onclick="javascript:mostra()">Salva</button>
	</div>
	<div class="col-xs-6 col-sm-6 col-md-2 col-lg-2 col-xl-2">
		<button type="button" class="btn btn-danger btn-sm btn-block">Cancella</button>
	</div>
</div>
-->
</asp:Content>
