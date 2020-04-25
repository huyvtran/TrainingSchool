<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfMakeTest.aspx.vb" Inherits="TrainingSchool.WfMakeTest" %>


<!DOCTYPE html>
<html lang="it">
<head>

    <meta charset="utf-8" />
    <title>Gestione Test</title>


    <meta name="description" content="Training School" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; iso-8859-1" />

    <!-- basic styles -->

    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />





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
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/typeahead-bs2.min.js"></script>




    <script src="assets/js/jquery-ui-1.10.3.full.min.js"></script>


    <!-- ace scripts -->

    <script src="assets/js/ace-elements.min.js"></script>
    <script src="assets/js/ace.min.js"></script>
    <!-- ace LMSRB -->
    <script src="assets/js/lmsrb.js"></script>
    <!-- LMSRB styles -->
    <link rel="stylesheet" href="assets/css/lmsrb.css" />

</head>

<body>
    <div id="pagebody">

        <div class="page-content">



            <div class="row">
                <div class="col-xs-12 ">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-3 control-label no-padding-right" for="txtnametest">Titolo *</label>

                            <div class="col-sm-9">
                                <input type="text" size="55" maxlength="255" id="txtnametest" value="Test Di Verifica" class="col-xs-20 col-sm-12" />
                            </div>
                        </div>


                        <div class="form-group">
                            <label for="materia" class="col-sm-3 control-label no-padding-right">Materia *</label>
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
                            <label for="anno" class="col-sm-3 control-label no-padding-right">Anno *</label>
                            <div class="col-sm-9">
                                <span class="input-icon  input-icon-right">

                                    <select class="form-control " id="anno" data-validation="length">
                                        <option value="">Seleziona Anno</option>
                                        <option value="1 anno">1 anno</option>
                                        <option value="2 anno">2 anno</option>
                                        <option value="3 anno">3 anno</option>
                                        <option value="4 anno">4 anno</option>
                                        <option value="5 anno">5 anno</option>
                                    </select>
                                </span>
                            </div>
                        </div>

                        <div class="tab-content">


                            <div id="objavailable" class="tabbable">
                                <ul class="nav nav-tabs" data-tabs="tabs" id="myTab">



                                    <li class="active"><a data-toggle="tab" class="tabsetting" href="#settings">MODALITA' DI FRUIZIONE
                                       
                                    </a></li>
                                    <li><a data-toggle="tab" class="tabquestionfree" href="#questionfree">INSERISCI DOMANDE 
                                       
                                    </a></li>
                                    <li><a data-toggle="tab" class="tabquestiongift" href="#questiongift">INSERISCI DA FILE GIFT
                                       
                                    </a></li>
                                    <li><a data-toggle="tab" class="tabquestionrepository" href="#questionrepository">INSERISCI DOMANDE DA REPOSITORY
                                       
                                    </a></li>

                                </ul>
                                <div class="tab-content">
                                    <div id="settings" class="tab-pane in active">
                                        <div class="form-horizontal">

                                            <div class="form-group">
                                                <label class="col-sm-3 control-label no-padding-right" for="txtsoglia">Soglia Superamento </label>

                                                <div class="col-sm-9">
                                                    <input type="text" id="txtsoglia" value="60" class="form-control" />
                                                    <br />
                                                    <span class="lbl">Percentuale (Ex. 60)</span>
                                                </div>

                                            </div>


                                            <div class="form-group">
                                                <label class="col-sm-3 control-label no-padding-right" for="txtsoglia">Numero max di tentativi </label>

                                                <div class="col-sm-9">
                                                    <input type="text" id="txttentativi" value="0" class="form-control" />
                                                    <span class="lbl">0 per tentativi illimitati </span>
                                                </div>



                                            </div>


                                            <div class="form-group">
                                                <label class="col-sm-3 control-label no-padding-right" for="txtrandom">Domande Random </label>

                                                <div class="col-sm-9">
                                                    <input type="text" id="txtrandom" value="0" class="form-control" />
                                                    <span class="lbl">Digitare il numero domande di che verranno estratte dal totale delle domande inserite </span>
                                                </div>
                                                <br />

                                            </div>
                                        </div>
                                        <button class="updatetest btn btn-success btn-sm"><i class="icon-ok bigger-110"></i>Salva Test</button>


                                    </div>
                                    <div id="questionfree" class="tab-pane">

                                        <div id="showtest" class="hide">

                                            <div class="row ">
                                                <div class="col-xs-12">
                                                    <!-- PAGE CONTENT BEGINS -->


                                                    <div class="form-group" id="QuestionWrapper"></div>
                                                    <div class="form-group" id="AnswerWrapper"></div>
                                                    <div class="form-group" id="SaveWrapper"></div>


                                                </div>
                                            </div>
                                            <div class="row ">


                                                <button class="addtest btn btn-danger btn-xs"><i class="icon-plus bigger-110"></i>Aggiungi al test</button>



                                            </div>
                                        </div>
                                    </div>
                                   
                               
                                    <div id="questiongift" class="tab-pane">

                                        <div class="form-group">

                                            <span class="btn btn-success btn-sm fileinput-button">
                                                <i class="glyphicon glyphicon-plus"></i>
                                                <span>Scegli file gift...</span>
                                                <!-- The file input field used as target for the file upload widget -->
                                                <input id="fileupload" type="file" name="files" />
                                                <input id="iddoc" type="hidden" />
                                            </span>
                                         

                                            <!-- The global progress bar -->
                                            <div id="progress" class="progress">
                                                <div class="progress-bar progress-bar-success"></div>
                                            </div>
                                            <!-- The container for the uploaded files -->

                                            <br>
                                               <div class="buttonset"></div>
                                        </div>
                                    </div>

                                         <div id="questionrepository" class="tab-pane">
                                             UNDER CONSTRUCTION
</div>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12">
                    <fieldset>
                        <legend>Domande</legend>


                        <div id="test_list" class="panel-group accordion-style1 accordion-style2">
                        </div>
                    </fieldset>


                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="idtest" />

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


    <script src="assets/js/codeUpload/jquery.iframe-transport.js"></script>
    <script src="assets/js/codeUpload/jquery.fileupload.js"></script>


    <script type="text/javascript">

        var dataobjupdate = 0;

        getquestion();

        var QuestionWrapper = $("#QuestionWrapper"); //Input boxes wrapper ID
        var SaveWrapper = $("#SaveWrapper"); //Input boxes wrapper ID
        var AnswerWrapper = $("#AnswerWrapper"); //Input boxes wrapper ID
        var domanda;
        var risposta;


        var FieldCount;
        var idquest = 0;
        var button = '  <button  class="addanswer btn btn-info btn-xs" > <i class="icon-plus bigger-80"></i>Aggiungi risposta</button> <button  class="removeAnswer btn btn-warning btn-xs" > <i class="icon-ok bigger-110"></i>Annulla</button>';
        var idtest;

        $(document).ready(function () {

            $("#txtnametest").val(getParameterByName("title"));
            $("#materia").val(getParameterByName("materia"));
            $("#anno").val(getParameterByName("anno"));
            $("#showtest").addClass("hide")


            if (getParameterByName("idtest") != 0) {
                $("#showtest").removeClass("hide");
                getquestion();
            }


            InitLmsRb();

            FieldCount = 1;


            domanda = '<label for="fieldquestion_' + FieldCount + '"> Domanda </label><div class="input-group"><textarea style="background-color:lightyellow" class="form-control"  cols="120" rows="2" type="text" name="mytext[]" id="fieldquestion_' + FieldCount + '" />';
            risposta = '  <br>Risposta ' + FieldCount + '  <input name="mycheck"  id="fieldcheck_' + FieldCount + '" type="radio" class="ace" /><span class="lbl">  Esatta</span> <br><textarea  class="form-control" name="mytext[]" id="fieldanswer_' + FieldCount + '"  cols="120" rows="2" /></textarea></div><br> ';


            $(QuestionWrapper).append(domanda);
            $(AnswerWrapper).append(risposta);
            $(SaveWrapper).append(button);


        });



        $(document).on('click', '.updateobject', function () {

            dataobjupdate = $(this).attr('data-obj');


            var urllink = "AdminAjaxLMS.aspx?op=modtest&oper=getquestion&idquest=" + dataobjupdate
            var requestupdate = $.ajax({
                url: urllink,
                type: "GET",
                datatype: "html"
            });


            requestupdate.fail(function (data) {
                alert(data);

            });


            requestupdate.success(function (data) {


                QuestionWrapper.html('');
                AnswerWrapper.html('');
                SaveWrapper.html('');


                QuestionWrapper.append(data);
                FieldCount = $("#nanswer").val();
                $(SaveWrapper).append(button);




            });





        });


        $(document).on('click', '.deleteobject', function () {

            var dataobj = $(this).attr('data-obj');

            var arr = dataobj.split('|');
            var urllink = "WFMakeTest.aspx?op=deletesource&id=" + dataobj
            var requestdelete = $.ajax({
                url: urllink,
                type: "GET",
                datatype: "html"
            });


            requestdelete.fail(function (data) {
                alert(data);
            });


            requestdelete.success(function (data) {
                alert(data);
                location.reload()
            });

        });

        $("body").on("click", ".removeAnswer", function (e) { //user click on remove text
            location.reload();
        });

        $("body").on("click", ".addanswer", function (e) { //user click on remove text
            var InputsWrapper = $("#InputAnswer"); //Input boxes wrapper ID
            FieldCount++;
            var risposta = ' <br> Risposta ' + FieldCount + ' <input name="mycheck"  id="fieldcheck_' + FieldCount + '" type="radio" class="ace" /><span class="lbl">  Esatta</span> <br><textarea  class="form-control"  name="mytext[]" id="fieldanswer_' + FieldCount + '"  cols="120" rows="2" /></textarea></div> ';

            $(AnswerWrapper).append(risposta);

        });

        $("body").on("click", ".updatetest", function (e) {


            if ($("txtnametest").val() != "" && $("#materia option:selected").val() != "" && $("#anno option:selected") != "") {


                var requesttest = $.ajax({
                    type: "POST",
                    url: "AdminAjaxLMS.aspx?op=" + getParameterByName("mod") +"&oper=createtest&idsessione=" + getParameterByName("idsessione") + "&idcategory=" + getParameterByName("idcategory") ,
                    dataType: "html",
                    data: {anno:$("#anno option:selected").val(),materia:$("#materia option:selected").val(), title: $('#txtnametest').val(), soglia: $('#txtsoglia').val(), tentativi: $('#txttentativi').val(), random: $('#txtrandom').val(), idtest: getParameterByName("idtest") }

                });
                requesttest.fail(function (msg) {
                    if (msg > 0) {
                        location.href = "?op=" + getParameterByName("mod") + "&idtest=" + msg + "&idsessione=" + getParameterByName("idsessione") + "&idcategory=" + getParameterByName("idcategory") + "&anno=" + $("#anno option:selected").val() + "&materia=" + $("#materia option:selected").val() + "&title=" + $('#txtnametest').val(),
                        $("#showtest").removeClass("hide")
                    }

                });

                requesttest.success(function (msg) {
                    if (msg > 0) {
                        location.href = "?op=" + getParameterByName("mod") + "&idtest=" + msg + "&idsessione=" + getParameterByName("idsessione") + "&idcategory=" + getParameterByName("idcategory") + "&anno=" + $("#anno option:selected").val() + "&materia=" + $("#materia option:selected").val() + "&title=" + $('#txtnametest').val(),
                        $("#showtest").removeClass("hide")
                    }
                });
            } else {

                alert("Riempire i campi obbligatori!")
            }
        });


        $("body").on("click", ".addtest", function (e) { //user click on remove text
            e.preventDefault();
            var answers = "";
            var checks = "";
            var idanswers = "";

            $question = $("#fieldquestion_1").val();


            if ($question == "") {
                alert("Impossibile salvare campi vuoti!");
            } else {
                if ($("#idquest").val() != undefined) {
                    FieldCount = $("#nanswer").val();
                }
                for (i = 1; i <= FieldCount; i++) {
                    if ($('#fieldanswer_' + i).val() != '') {
                        idanswers += "|" + $('#fieldanswer_' + i).attr('name');
                        answers += "|" + $('#fieldanswer_' + i).val();
                        checks += "|" + $('#fieldcheck_' + i)[0].checked;
                    }
                }



                if ($("#idquest").val() != undefined) {
                    idquest = $("#idquest").val();
                }

                var request4 = $.ajax({
                    type: "POST",
                    url: "AdminAjaxLMS.aspx?op=modtest&oper=addquestion&idquest=" + idquest + "&idtest=" + getParameterByName("idtest"),
                    data: { question: $question, answer: answers, check: checks, idanswer: idanswers },
                    dataType: "html"
                });



                request4.success(function (data) {


                    QuestionWrapper.html('');
                    AnswerWrapper.html('');
                    SaveWrapper.html('');
                    $(QuestionWrapper).append(domanda);
                    $(AnswerWrapper).append(risposta);
                    $(SaveWrapper).append(button);
                    FieldCount = 1;
                    getquestion();

                });


            }
        });

        $('#fileupload').fileupload({
            replaceFileInput: true,
            dataType: 'json',
            url: "HUpload.ashx?load=addquestgift",
            add: function (e, data) {
                data.context = $('<button/>').attr({ type: 'button' }).text('Invia').addClass('btn btn-primary ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only')
                     .appendTo(".buttonset")
                    .click(function () {
                        data.context = $('<p/>').text('Caricando..').replaceAll($(this));
                        data.formData = { idtest: getParameterByName("idtest")};
                        data.submit();
                    });
            },
            done: function (data) {
                data.context = $('<p/>').text('Caricando terminato').replaceAll($(this));
            
                getquestion();

            },
            progressall: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('#progresstest .progress-bar').css(
                    'width',
                    progress + '%'
                );
            },
            fail: function (data) {
                alert(data.responseText)
                getquestion();
            }
        });

        $("body").on("click", ".addsequence", function (e) { //user click on remove text
            e.preventDefault();


            var request = $.ajax({
                type: "GET",
                url: "AdminAjaxLMS.aspx?op=modtest&oper=addsequence&idtest=" + getParameterByName("idtest"),

                dataType: "html"
            });



            request.success(function (data) {

                getquestion();

            });



        });

        function getquestion() {


            var request4 = $.ajax({
                type: "GET",
                url: "AdminAjaxLMS.aspx?op=modtest&oper=get&idtest=" + getParameterByName("idtest"),
                dataType: "html"
            });



            request4.success(function (data) {

                $("#test_list").empty()
                $("#test_list").html(data);

            });
        }

        function ordertest(id, sequence, offset) {


            sequence = sequence++;

            var request4 = $.ajax({
                type: "GET",
                url: "?op=modtest&oper=updatesequence&idtest=" + getParameterByName("idtest") + "&idquest=" + id + "&sequence=" + sequence + "&offset=" + offset,
                dataType: "html"
            });


            request4.success(function () {

                location.reload();

            });

        }


    </script>

    <div id="modal"></div>

</body>
</html>
