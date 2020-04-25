<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfMakeHtml.aspx.vb" Inherits="TrainingSchool.WfMakeHtml" %>


<!DOCTYPE html>
<html lang="it">
<head>
    <meta charset="utf-8" />
    <title>HTML</title>


    <meta name="description" content="Common form elements and layouts" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />


    <meta charset="utf-8" />

    <meta name="description" content="Training School" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- basic styles -->
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/font-awesome.min.css" />
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link rel="stylesheet" media="screen" href="assets/css/ui.jqgrid.min.css" />

    <link rel="stylesheet" href="assets/css/fullcalendar/fullcalendar.css?ver=1" />
    <link rel="stylesheet" href="assets/css/fullcalendar/fullcalendar.print.css" media="print" />
    <link rel="stylesheet" href="assets/js/codeUpload/jquery.fileupload.css" />
    <link rel="stylesheet" href="assets/css/lmsrb.css" />

    <link rel="stylesheet" href="assets/css/ace.css?ver=2" />
    <link rel="stylesheet" href="assets/css/ace-fonts.css" />
    <link rel="stylesheet" href="assets/css/ace-rtl.min.css" />
    <link rel="stylesheet" href="assets/css/ace-skins.min.css" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.5.1/animate.min.css" />
    <!--[if IE 7]>
		  <link rel="stylesheet" href="assets/css/font-awesome-ie7.min.css" />
		<![endif]-->

    <!--[if lte IE 8]>
		  <link rel="stylesheet" href="assets/css/ace-ie.min.css" />
		<![endif]-->

    <!-- inline styles related to this page -->

    <!-- ace settings handler -->


    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->

    <!--[if lt IE 9]>
		<script src="assets/js/html5shiv.js"></script>
		<script src="assets/js/respond.min.js"></script>
		<![endif]-->
    <!-- basic scripts -->

    <!--[if !IE]> -->
    <!-- <![endif]-->


    <script type="text/javascript">
        window.jQuery || document.write("<script src='assets/js/jquery-2.0.3.min.js'>" + "<" + "/script>");
    </script>
    <script src="assets/js/ace-extra.min.js"></script>

    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/bootbox.js?ver=2"></script>
    <script src="assets/js/lmsrb.js?ver=20"></script>

    <!--[if IE]>
<script type="text/javascript">
 window.jQuery || document.write("<script src='assets/js/jquery-1.10.2.min.js'>"+"<"+"/script>");
</script>
<![endif]-->

    <script src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
    <script src="assets/js/jquery-ui-1.10.3.custom.min.js"></script>




</head>

<body>





    <div class="page-content">

        <div class="row">
            <div class="col-xs-12 ">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right" for="txtname_html">Titolo </label>

                        <div class="col-sm-9">
                            <input type="text" size="55" maxlength="255" id="txtname_html" placeholder="Titolo oggetto" class="col-xs-20 col-sm-12" />
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
              </div></div></div>
        <div class="row">
            <div class="space-4"></div>
            <script src="https://cdn.ckeditor.com/4.14.0/standard-all/ckeditor.js"></script>
            <textarea cols="10" id="editor1" name="editor1" rows="10"></textarea>
            <script>
                (function () {
                    var mathElements = [
                        'math',
                        'maction',
                        'maligngroup',
                        'malignmark',
                        'menclose',
                        'merror',
                        'mfenced',
                        'mfrac',
                        'mglyph',
                        'mi',
                        'mlabeledtr',
                        'mlongdiv',
                        'mmultiscripts',
                        'mn',
                        'mo',
                        'mover',
                        'mpadded',
                        'mphantom',
                        'mroot',
                        'mrow',
                        'ms',
                        'mscarries',
                        'mscarry',
                        'msgroup',
                        'msline',
                        'mspace',
                        'msqrt',
                        'msrow',
                        'mstack',
                        'mstyle',
                        'msub',
                        'msup',
                        'msubsup',
                        'mtable',
                        'mtd',
                        'mtext',
                        'mtr',
                        'munder',
                        'munderover',
                        'semantics',
                        'annotation',
                        'annotation-xml'
                    ];

                    CKEDITOR.plugins.addExternal('ckeditor_wiris', 'https://ckeditor.com/docs/ckeditor4/4.14.0/examples/assets/plugins/ckeditor_wiris/', 'plugin.js');

                    CKEDITOR.replace('editor1', {
                        extraPlugins: 'ckeditor_wiris',
                        // For now, MathType is incompatible with CKEditor file upload plugins.
                        removePlugins: 'uploadimage,uploadwidget,uploadfile,filetools,filebrowser',
                        height: 320,
                        // Update the ACF configuration with MathML syntax.
                        extraAllowedContent: mathElements.join(' ') + '(*)[*]{*};img[data-mathml,data-custom-editor,role](Wirisformula)'
                    });
                }());
            </script>
            <div class="space-4"></div>
            <div class="space-4"></div>
            <button class="savehtml btn btn-info"><i class="icon-ok bigger-110"></i>Salva</button>
            <button class="resethtml btn btn-warning"><i class="icon-ok bigger-110"></i>Annulla</button>
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



    <script type="text/javascript">



        $(document).ready(function () {



            InitLmsRb();




            var requesthtml = $.ajax({
                type: "GET",
                url: "AdminAjaxLMS.aspx?op=modhtml&oper=gethtml&idPage=" + getParameterByName("idPage"),
                datatype: "html"

            });
         

            requesthtml.success(function (data) {

                $("#txtname_html").val(getParameterByName("title"));
                $("#materia").val(getParameterByName("materia"));
                $("#anno").val(getParameterByName("anno"));
                CKEDITOR.instances['editor1'].setData(data);

            });


            requesthtml.error(function (data) {


            });

        });


        $("body").on("click", ".savehtml", function (e) { //user click on remove text
            e.preventDefault();


            if ($("txtname_html").val() != "" && $("#materia option:selected").val() != "" && $("#anno option:selected") != "") {


                var formdata = { anno: $("#anno option:selected").val(), materia: $("#materia option:selected").val(), title: $('#txtname_html').val(), content: CKEDITOR.instances['editor1'].getData() };
                var requesthtml = $.ajax({
                    type: "POST",
                    url: "AdminAjaxLMS.aspx?op=modhtml&oper=savehtml&idPage=" + getParameterByName("idPage"),
                    data: formdata

                });


                requesthtml.success(function (data) {
                    if (data > 0) {
                        ShowAlert("Operazione completata")
                        location.href = "?idPage=" + data;
                    } else {
                        ShowAlert("Errore!")
                    }
                });


                requesthtml.error(function (data) {
                    alert(data);

                });

            } else {

                alert("Attenzione riempire i campi obbligatori!")
            }

        });



        $("body").on("click", ".resethtml", function (e) { //user click on remove text
            e.preventDefault();

            location.reload()

        });

    </script>

    <div id="modal"></div>

</body>
</html>
