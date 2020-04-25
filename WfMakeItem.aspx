<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfMakeItem.aspx.vb" Inherits="TrainingSchool.WfMakeItem" %>


<!DOCTYPE html>
<html lang="it">
<head>
    <meta charset="utf-8" />
    <title>Gestione File</title>


    <meta name="description" content="Make file" />
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
          
           
            <!-- /.page-header -->

            <div class="row">
                <div class="col-xm-12 ">
                    <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right" for="txtname_item">Titolo </label>

                        <div class="col-sm-9">
                            <input type="text" size="55" maxlength="255" id="txtname_item" placeholder="Titolo oggetto" class="col-xs-20 col-sm-12" />
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
                       </div>
                    <div class="space-4"></div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <input type="file" id="singlefileitem" />

                            <label>
                                <%--  <input type="checkbox" name="file-format" id="id-file-format" class="ace" />
                                <span class="lbl">Solo formati Pdf,Jpg,Png</span>--%>
                            </label>
                            <div class="col-sm-9">
                            </div>
                        </div>
                    </div>
                    <div class="space-4"></div>
                    <div class="appenditem"></div>
                     <div id="progress1" class="progress">
                                                    <div class="progress-bar progress-bar-success"></div>
                                                </div>
                   <%-- <button class="reset btn btn-warning"><i class="icon-ok bigger-110"></i>Annulla</button>--%>

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

       
                $("#txtname_item").val(getParameterByName("title"));
                $("#materia").val(getParameterByName("materia"));
                $("#anno").val(getParameterByName("anno"));
             
                
                
                $('#singlefileitem').ace_file_input({
                    no_file: 'Nessun File ...',
                    btn_choose: 'Scegli',
                    btn_change: 'Cambia',
                    droppable: false,
                    thumbnail: false, //| true | large
                    whitelist: 'gif|png|jpg|jpeg|pdf|txt',
                    blacklist: 'exe|php'
                    //onchange:''
                    //
                });



                var request5 = $('#singlefileitem').fileupload({
                    replaceFileInput: true,
                    dataType: 'json',
                    autoUpload: false,
                    url: "HUpload.ashx?load=createitem",
                    add: function (e, data) {



                        if (!controlla_estensione(data.files[0].name)) {
                            return false;
                        }


                        if (!controlla_dimensione(data.files[0].size)) {
                            return false;
                        }

                        if ($("#txtname_item").val() != "" && $("#materia option:selected").val() != "" && $("#anno option:selected") != "") {


                            data.context = $('<p/>').text('Caricando..').replaceAll($(this));
                            data.formData = { anno: $("#anno option:selected").val(), materia: $("#materia option:selected").val(), title: $('#txtname_item').val(), id:<%=Request.QueryString("iditem") %> +'' };
                            $('#progress').show();
                            data.submit();
                        }else{
                        alert("Riempire i campi obbligatori!")
                        }
                    },
                    success: function (response) {
                        bootbox.alert(data);
                    },
                    done: function (e, data) {

                        $('#progress').text("Caricamento terminato");
                   
                    },
                    fail: function (e, data) {
                        $('#progress1').text("Caricamento terminato");

                      },
                    progressall: function (e, data) {
                        var progress = parseInt(data.loaded / data.total * 100, 10);
                        $('#progress .progress-bar').css(
                            'width',
                            progress + '%'
                        );
                    }
                });


            });


            function get_estensione(path) {
                posizione_punto = path.lastIndexOf(".");
                lunghezza_stringa = path.length;
                estensione = path.substring(posizione_punto + 1, lunghezza_stringa);
                return estensione;
            }

            function controlla_estensione(path) {
                if (get_estensione(path) != "BMP" && get_estensione(path) != "bmp" && get_estensione(path) != "PDF" && get_estensione(path) != "GIF" && get_estensione(path) != "JPG" && get_estensione(path) != "TIF" && get_estensione(path) != "jpg" && get_estensione(path) != "tif" && get_estensione(path) != "tiff" && get_estensione(path) != "jpeg" && get_estensione(path) != "png" && get_estensione(path) != "pdf") {
                    bootbox.alert("Il file deve avere una di queste estensioni: bmp/gif/jpg/png/tiff/tif/pdf/jpeg");
                    return false;
                }

                return true;
            }

            function controlla_dimensione(Size) {
                if (Size > 2048000) {
                    bootbox.alert("Il file allegato non può essere di dimensioni superiori a 2 MB.");
                    return false;
                }
                return true;
            }
        </script>
    </div>
    <div id="modal"></div>

</body>
</html>
