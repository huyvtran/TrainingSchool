<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfMakeScorm.aspx.vb" Inherits="TrainingSchool.WfMakeScorm" %>


<!DOCTYPE html>
<html lang="it">
<head>
    <meta charset="utf-8" />
    <title>Gestione Scorm</title>

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


    <script>
        var startTime = 0;
        var myVar = 0;
        function handleDataAvailable(event) {
            if (event.data && event.data.size > 0) {
                recordedBlobs.push(event.data);
            }
        }

        function myTimer() {
            endTime = performance.now();
            passato = Math.floor((endTime - startTime));

            var date = new Date(passato);
            // Hours part from the timestamp
            var hours = date.getHours() - 1;
            // Minutes part from the timestamp
            var minutes = "0" + date.getMinutes();
            // Seconds part from the timestamp
            var seconds = "0" + date.getSeconds();

            // Will display time in 10:30:23 format
            var formattedTime = hours + ':' + minutes.substr(-2) + ':' + seconds.substr(-2);
            document.getElementById('ttempo').innerHTML = ' (' + formattedTime + ')';
        }



    </script>

</head>

<body>

    <div id="pagebody">



        <div class="page-content">
            <div class="row">
                <div class="form-horizontal">
                <div class="form-group">
                    <label for="nomecorso" class="col-sm-3 control-label no-padding-right">Titolo </label>
                    <div class="col-sm-9">
                        <span class="input-icon  input-icon-right">&nbsp;
                                            <input type="text" width="400" name="title_scorm" id="title_scorm" /></span>
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
                                    <label for="anno" class="col-sm-3 control-label no-padding-right">Materia *</label>
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
                    </div>
            </div>
            <div class="row">
                <!-- /.page-header -->
                <div class="tab-content">
                    <div id="new" class="tab-pane in active">


                        <div id="objavailable" class="tabbable">
                            <ul class="nav nav-tabs" data-tabs="tabs" id="myTab">



                                <li class="active"><a data-toggle="tab" href="#youtube">VIDEO YOUTUBE
                                       
                                </a></li>
                                <li><a data-toggle="tab" class="tabscorm" href="#scorm">SCORM 1.2 
                                       
                                </a></li>
                                <li><a data-toggle="tab" class="tabrecord" href="#videorecord">REGISTRAZIONE CON WEBCAM
                                       
                                </a></li>
                                <li><a data-toggle="tab" class="tabvideo" href="#video">VIDEO MP4
                                       
                                </a></li>

                            </ul>
                            <div class="tab-content">
                                <div id="youtube" class="tab-pane in active">
                                    <div class="form-horizontal">


                                        <div class="form-group">
                                            <label for="addressyoutube" class="col-sm-3 control-label no-padding-right">Indirizzo video Youtube </label>
                                            <div class="col-sm-9">
                                                <span class="input-icon  input-icon-right">&nbsp;
                                            <input type="text" width="400" name="addressyoutube" id="addressyoutube" /></span>
                                                <small>Esempio:https://www.youtube.com/watch?v=FRWhRi56NYU</small>

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="nomecorso" class="col-sm-3 control-label no-padding-right"></label>
                                            <div class="col-sm-9">
                                                <span class="input-icon  input-icon-right">&nbsp;
                                           
                                   <button id="btnvideoyoutube" class="btn btn-danger btn-sm">Invia</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="scorm" class="tab-pane">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label for="nomecorso" class="col-sm-3 control-label no-padding-right">File compresso Scorm</label>
                                            <div class="col-sm-9">
                                                <span class="input-icon  input-icon-right">&nbsp;
                                            <input type="file" id="singlefilescorm" />
                                                </span>
                                                <div id="progressscorm" class="progress">
                                                    <div class="progress-bar progress-bar-success"></div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="videorecord" class="tab-pane ">
                                    <script src="assets/js/videorecorder/adapter-latest.js"></script>
                                    <script src="assets/js/videorecorder/MultiStreamsMixer.js"></script>
                                    <div class="row">
                                        <div class="col-sm-6" style="padding-bottom: 18px;">
                                            <div id="guma">
                                                <video id="gum" autoplay="" muted="" style="margin-bottom: 12px; width: 98%; min-height: 100px; border: 1px solid #666666;"></video>


                                            </div>
                                            <div id="recordedVideoa" style="display: none;">
                                                <video id="recordedVideo" style="margin-bottom: 12px; width: 98%; min-height: 100px; border: 1px solid #666666;" controls=""></video>
                                                <br>
                                                <span class="glyphicon glyphicon-facetime-video" aria-hidden="true"></span>&nbsp;&nbsp;REGISTRA VIDEO
                                            </div>
                                        </div>
                                        <div class="col-sm-6" style="padding-bottom: 18px;">
                                            <label style="min-width: 80px;" for="audioSource">AUDIO:&nbsp;&nbsp;</label><select id="audioSource" style="width: 100%; font-size: 12pt; padding: 5px; margin-bottom: 4px"><option value="">microphone 1</option>
                                            </select><br>
                                            <label style="min-width: 80px;" for="ridecho">Riduzione echo:&nbsp;&nbsp;</label><input type="checkbox" id="echoCancellation" name="echoCancellation" value="0"><hr>
                                            <label style="min-width: 80px;" for="videoSource">VIDEO:&nbsp;&nbsp;</label><select id="videoSource" style="width: 100%; font-size: 12pt; padding: 5px;"><option value="">camera 1</option>
                                                <option value="camera-screen">Camera + Schermo</option>
                                                <option value="only-screen">Solo schermo e audio</option>
                                            </select><hr>
                                            <button id="record" class="btn btn-success" style="border-radius: 5px; float: left; margin-right: 8px;">Start Rec</button>
                                            <button id="newrecord" class="btn btn-success" style="border-radius: 5px; display: none; float: left; margin-right: 8px;">Nuova regustrazione</button>
                                            <button id="stoprecord" class="btn btn-danger" style="border-radius: 5px; display: none; float: left; margin-right: 8px;">Ferma registrazione<span id="ttempo"></span></button>

                                            <button id="playrec" class="btn btn-success" style="border-radius: 5px; display: none; float: left; margin-right: 8px;">Riproduci video</button>

                                            <button id="downloadrec" class="btn btn-warning" style="border-radius: 5px; display: none; float: left; margin-right: 8px;">Salva video</button>

                                            <script src="assets/js/videorecorder/devices-rec.js"></script>
                                        </div>

                                    </div>
                                </div>
                                <div id="video" class="tab-pane ">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label for="nomecorso" class="col-sm-3 control-label no-padding-right">File Video Mp4</label>
                                            <div class="col-sm-9">
                                                <span class="input-icon  input-icon-right">&nbsp;
                                            <input type="file" id="singlefilesvideo" />
                                                </span>

                                                <div id="progressvideo" class="progress">
                                                    <div class="progress-bar progress-bar-success"></div>
                                                </div>

                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <label for="nomecorso" class="col-sm-3 control-label no-padding-right"></label>
                                            <div class="col-sm-9">
                                                <span class="input-icon  input-icon-right">&nbsp;
                                          <div class="appendvideo"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /.page-content -->
    <!-- /.main-container -->

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

    <!-- page specific plugin scripts -->

    <!--[if lte IE 8]>
		  <script src="assets/js/excanvas.min.js"></script>
		<![endif]-->

    <script src="assets/js/ace-elements.min.js"></script>
    <script src="assets/js/ace.min.js"></script>

    <script src="assets/js/codeUpload/jquery.iframe-transport.js"></script>
    <script src="assets/js/codeUpload/jquery.fileupload.js"></script>



    <script type="text/javascript">



        $(document).ready(function () {

            InitLmsRb();


            $("#txtname_titlescorm").val("<%=Request.QueryString("title")%>");




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



            $(".tabrecord").on("click", function () {

                // Prefer camera resolution nearest to 1280x720.
                var constraints = { audio: true, video: { width: 1280, height: 720 } };
                navigator.mediaDevices.getUserMedia(constraints)
                .then(function (mediaStream) {
                    //var video = document.querySelector('video');
                    var video = document.getElementById("gum")
                    video.srcObject = mediaStream;
                    video.onloadedmetadata = function (e) {
                        var sel = document.getElementById('videoSource').value;
                        if (sel === 'camera-screen' || sel === 'only-screen') {
                            if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1)
                                var mediaRecorder = new MediaRecorder(gum.mozCaptureStream());
                            else
                                var mediaRecorder = new MediaRecorder(gum.captureStream());
                        }
                        else
                            var mediaRecorder = new MediaRecorder(mediaStream);

                        video.play();



                        record.onclick = function () {
                            startTime = performance.now();
                            myVar = setInterval(myTimer, 1000);

                            recordedVideo.pause();
                            recordedBlobs = [];
                            mediaRecorder.start();
                            mediaRecorder.ondataavailable = handleDataAvailable;

                            console.log(mediaRecorder.state);
                            console.log("recorder started");
                            record.style.display = "none";
                            stoprecord.style.display = "block";

                            playrec.style.display = "none";
                            downloadrec.style.display = "none";
                            recordedVideoa.style.display = "none";
                            guma.style.display = "block";


                        }
                        stoprecord.onclick = function () {
                            clearInterval(myVar);
                            mediaRecorder.stop();
                            console.log(mediaRecorder.state);
                            console.log("recorder stopped");
                            record.style.display = "none";
                            newrecord.style.display = "block";
                            stoprecord.style.display = "none";

                            playrec.style.display = "block";
                            downloadrec.style.display = "block";

                        }

                        playrec.onclick = function () {
                            const superBuffer = new Blob(recordedBlobs, { type: 'video/webm' });
                            recordedVideo.src = null;
                            recordedVideo.srcObject = null;
                            recordedVideo.src = window.URL.createObjectURL(superBuffer);
                            recordedVideo.controls = true;
                            recordedVideo.play();

                            recordedVideoa.style.display = "block";
                            playrec.style.display = "none";
                            guma.style.display = "none";
                        }

                        newrecord.onclick = function () {
                            recordedVideoa.style.display = "none";
                            recordedVideo.pause();
                            record.style.display = "block";
                            newrecord.style.display = "none";
                            guma.style.display = "block";
                            downloadrec.style.display = "none";
                            playrec.style.display = "none";
                        }






                        downloadrec.onclick = function () {
                            const blob = new Blob(recordedBlobs, { type: 'video/webm' });

                            var formData = new FormData()
                            formData.append('title', $("#title_scorm").val());
                            formData.append('materia', $("#materia option:selected").val())
                            formData.append('anno', $("#anno option:selected").val())
                            formData.append('source', blob)

                            $.ajax({
                                url: "HUpload.ashx?load=createrecordvideo",
                                type: 'POST',
                                data: formData,
                                processData: false,
                                contentType: false,
                                success: function (data) {
                                    alert("scorm creato")
                                }
                            });



                            const url = window.URL.createObjectURL(blob);
                            const a = document.createElement('a');
                            a.style.display = 'none';
                            a.href = url;
                            a.download = 'registrazione.webm';
                            document.body.appendChild(a);
                            a.click();
                            setTimeout(() => {
                                document.body.removeChild(a);
                                window.URL.revokeObjectURL(url);
                            }, 100);
                        }
                    };
                })
.catch(function (err) { console.log(err.name + ": " + err.message); }); // always check for errors at the end.


            });

            $('#singlefilescorm').fileupload({
                replaceFileInput: true,
                dataType: 'json',
                url: "HUpload.ashx?load=createscormzip",
                add: function (e, data) {
                    $('.ui-dialog-buttonset').empty();
                    data.context = $('<button/>').attr({ type: 'button' }).text('Invia').addClass('btn btn-primary ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only')
                        .appendTo(".appendscorm")
                        .click(function () {

                            data.formData = {anno:$("#anno option:selected").val(),materia:$("option:selected").val(), title: $("#title_scorm").val() }
                            data.context = $('<p/>').text('Caricando...').replaceAll($(this));
                            data.submit();



                        });



                },
                done: function (e, data) {
                    data.context.text('Caricamento Terminato..');

                    alert("Scorm creato");

                },
                progressall: function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    $('#progressscorm').css(
                        'width',
                        progress + '%'
                    );
                },
                fail: function (data) {
                    alert("Scorm caricato");
                    location.reload()
                }
            });

            $('#singlefilesvideo').fileupload({
                replaceFileInput: true,
                dataType: 'json',
                url: "HUpload.ashx?load=createvideozip",
                add: function (e, data) {
                    $('.ui-dialog-buttonset').empty();
                    data.context = $('<button/>').attr({ type: 'button' }).text('Invia').addClass('btn btn-primary ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only')
                        .appendTo(".appendvideo")
                        .click(function () {

                            data.formData = { anno: $("#anno option:selected").val(), materia: $("option:selected").val(), title: $("#title_scorm").val() }

                            data.context = $('<p/>').text('Caricando...').replaceAll($(this));
                            data.submit();



                        });



                },
                done: function (e, data) {
                    data.context.text('Caricamento Terminato..');

                    alert("Scorm creato");

                },
                progressall: function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    $('#progressvideo').css(
                        'width',
                        progress + '%'
                    );
                },
                fail: function (data) {
                    alert("Scorm caricato");
                    location.reload()
                }
            });

            $("#btnvideoyoutube").on("click", function (e) {


                $.ajax({
                    type: "POST",
                    url: "HUpload.ashx?load=createyoutubevideo",
                    data: {  title: $("#title_scorm").val(), anno:$("#anno option:selected").val(),materia:$("option:selected").val(), addressyoutube: $("#addressyoutube").val() },
                    dataType: "json",
                    success: function (data) {
                        alert("Scorm creato");

                    },
                    error: function (data) {
                        alert("Scorm creato");

                    },
                });



                return false;
            });
        });
    </script>

    <div id="modal"></div>

</body>
</html>
