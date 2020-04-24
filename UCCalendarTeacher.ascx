<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UCCalendarTeacher.ascx.vb" Inherits="TrainingSchool.UCCalendarTeacher" %>
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