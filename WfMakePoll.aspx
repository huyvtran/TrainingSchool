<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfMakePoll.aspx.vb" Inherits="TrainingSchool.WfMakePoll" %>



<!DOCTYPE html>
<html lang="it">
<head>
    <meta charset="utf-8" />
    <title>Gestione Sondaggi</title>

    <meta name="description" content="Common form elements and layouts" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- basic styles -->

    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />

    <!--[if IE 7]>
		  <link rel="stylesheet" href="assets/css/font-awesome-ie7.min.css" />
		<![endif]-->

    <!-- page specific plugin styles -->

    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.custom.min.css" />


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
    <script src="assets/js/lmsrb.js"></script>
    <!-- LMSRB styles -->
    <link rel="stylesheet" href="assets/css/lmsrb.css" />

</head>

<body>





    <div class="page-content">

        <!-- /.page-header -->
        <div class="row">
            <div class="col-xs-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right" for="txtnametest">Titolo </label>

                        <div class="col-sm-9">
                            <input type="text" size="55" maxlength="255" id="txtname_poll" value="Sondaggio" class="col-xs-20 col-sm-12" />
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

                                            <select class="form-control " id="anno" data-validation="length" >
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

                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right" for="txtnametest"></label>

                        <div class="col-sm-9">
                            <button class="updatepoll btn btn-success btn-sm"><i class="icon-ok bigger-110"></i>Salva</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-xs-12">
                <div id="showpoll" class="hide">


                    <div class="col-sm-4 well">
                        <div class="form-group" id="QuestionWrapper"></div>

                        <div class="form-group" id="SaveWrapper"></div>

                        <button class="addpoll btn btn-warning"><i class="icon-ok bigger-110"></i>Aggiungi Domanda</button>


                    </div>
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-6">
                        <%-- <form id="form1" runat="server" class="form-horizontal" role="form">
                    </form>--%>
                        <div id="poll_list" class="form-horizontal">
                        </div>

                    </div>

                </div>

            </div>
        </div>
        <div class="space-4"></div>



    </div>










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



    <script type="text/javascript">

        var dataobjupdate = 0;


        var QuestionWrapper = $("#QuestionWrapper"); //Input boxes wrapper ID
        var SaveWrapper = $("#SaveWrapper"); //Input boxes wrapper ID
        var AnswerWrapper = $("#AnswerWrapper"); //Input boxes wrapper ID
                  


        var FieldCount = 1;
        var idquest = 0;

        var domanda = '<label for="fieldquestion_' + FieldCount + '"> Domanda </label><div class="input-group"><textarea style="background-color:lightyellow" cols="40" rows="5" type="text" name="mytext[]" id="fieldquestion_' + FieldCount + '" />';
        //  var button = '  <button  class="addanswer btn btn-info" > <i class="icon-ok bigger-110"></i>Aggiungi risposta</button> <button  class="removeAnswer btn btn-warning" > <i class="icon-ok bigger-110"></i>Annulla</button>';
        var button = '';
        var risposta = '  <br>Risposta ' + FieldCount + ' <br><textarea  name="mytext[]" id="fieldanswer_' + FieldCount + '"  cols="60" rows="7" /></textarea></div><br> ';

        $(document).ready(function () {

            $("#txtname_poll").val(getParameterByName("title"));
            $("#materia").val(getParameterByName("materia"));
            $("#anno").val(getParameterByName("anno"));



            if (getParameterByName("id_poll") != 0) {
                $("#showpoll").removeClass("hide");

                $(QuestionWrapper).append(domanda);
                $(AnswerWrapper).append(risposta);


                getpoll();
            }


            InitLmsRb();



        });

        $("body").on("click", ".removeAnswer", function (e) { //user click on remove text
            location.reload();
        });

        $("body").on("click", ".addanswer", function (e) { //user click on remove text
            var InputsWrapper = $("#InputAnswer"); //Input boxes wrapper ID
            FieldCount++;
            var risposta = '  <br>Risposta ' + FieldCount + ' <br><textarea  name="mytext[]" id="fieldanswer_' + FieldCount + '"  cols="60" rows="7" /></textarea></div></div><br> ';

            $(AnswerWrapper).append(risposta);

        });


        $("body").on('click', '.deleteobject', function () {

            var dataobj = $(this).attr('data-obj');

            var arr = dataobj.split('|');
            var urllink = "WFMakePoll.aspx?op=deletesource&id=" + dataobj
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


        $("body").on("click", ".addpoll", function (e) { //user click on remove text
            e.preventDefault();
            var answers = "";

            var idanswers = "";

            $question = $("#fieldquestion_1").val();

            if ($("#idquest").val() != undefined) {
                FieldCount = $("#nanswer").val();
            }
            for (i = 1; i <= FieldCount; i++) {
                idanswers += "|" + $('#fieldanswer_' + i).attr('name');
                answers += "|" + $('#fieldanswer_' + i).val();
            }

            if ($question == "") {
                alert("Impossibile salvare campi vuoti!");
            } else {

                if ($("#idquest").val() != undefined) {
                    idquest = $("#idquest").val();
                }

                var request4 = $.ajax({
                    type: "POST",
                    url: "AdminAjaxLMS.aspx?op=modpoll&oper=addquestion&idquest=" + idquest + "&idpoll=" + getParameterByName("id_poll"),
                    data: { question: $question, answer: answers, idanswer: idanswers },
                    dataType: "html"
                });



                request4.success(function (data) {
                    $(QuestionWrapper).empty();
                    $(AnswerWrapper).empty();

                    $(QuestionWrapper).append(domanda);
                    $(AnswerWrapper).append(risposta);
                    getpoll();


                });


            }
        });


        $("body").on("click", ".updatepoll", function (e) { //user click on remove text

            if ($("#txtname_poll").val() != "" && $("#materia option:selected").val() != "" && $("#anno option:selected") != "") {

                var requesttest = $.ajax({
                    type: "POST",
                    url: "AdminAjaxLMS.aspx?op=modpoll&oper=createpoll",
                    dataType: "html",
                    data: { anno: $("#anno option:selected").val(), materia: $("#materia option:selected").val(), title: $('#txtname_poll').val(), description: '', id_poll: getParameterByName("id_poll") }

                });


                requesttest.success(function (data) {
                    if (data > 0) {
                        $("#showpoll").removeClass("hide");
                        location.href = "?id_poll=" + data;
                    }


                });
            } else {
                alert("Riempire i campi obbligatori!")
            }

        });


        function getpoll() {


            var request4 = $.ajax({
                type: "GET",
                url: "AdminAjaxLMS.aspx?op=modpoll&oper=get&id_poll=" + getParameterByName("id_poll"),
                dataType: "html"
            });



            request4.success(function (data) {

                $(QuestionWrapper).empty();
                $(AnswerWrapper).empty();

                $(QuestionWrapper).append(domanda);
                $(AnswerWrapper).append(risposta);

                $("#poll_list").empty(),
                    $("#poll_list").html(data);



            });
            return false;
        }

        function orderpoll(id, sequence, offset) {


            sequence = sequence++;

            var request4 = $.ajax({
                type: "GET",
                url: "?op=modpoll&oper=updatesequence&idpoll=" + getParameterByName("id_poll") + "&idquest=" + id + "&sequence=" + sequence + "&offset=" + offset,
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
