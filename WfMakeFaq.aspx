<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WfMakeFaq.aspx.vb" Inherits="TrainingSchool.WfMakeFaq" %>


<!DOCTYPE html>
<html lang="it">
<head>
    <meta charset="utf-8" />
    <title>Gestione FAQ's</title>

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

       <script type="text/javascript"   src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-AMS-MML_HTMLorMML"> </script>

</head>

<body>

    <div id="pagebody">



        <div class="page-content">
           
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-3 control-label no-padding-right" for="txtnametest">Titolo </label>

                            <div class="col-sm-9">
                                <input type="text" size="55" maxlength="255" id="txtname_faq" value="Faq" class="col-xs-20 col-sm-12" />
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
                        <div class="form-group">
                            <label class="col-sm-3 control-label no-padding-right" for="txtnametest"></label>

                            <div class="col-sm-9">
                                <button class="updatefaq btn btn-success btn-sm"><i class="icon-ok bigger-110"></i>Salva</button>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row">
                <div class="col-sm-12">
                    <!-- PAGE CONTENT BEGINS -->
                    <div id="showfaq" class="hide">
                        <div class="col-sm-6">
                            <label for="fieldquestion">Domanda </label>
                            <div class="input-group">
                                <textarea style="background-color: lightyellow" name="mytext[]" cols="80" rows="5" id="fieldquestion"></textarea>
                                <br>
                                Risposta
                                <br>

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

                                <br>
                                <button class="addclass btn btn-info"><i class="icon-ok bigger-110"></i>Aggiungi FAQ</button>
                            </div>
                            </div>
                            <div class="col-sm-6">

                                <div id="faq_list" class="panel-group accordion-style1 accordion-style2">
                                </div>

                            </div>

                      

                    </div>

                </div>
            </div>




        </div>
     </div>
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

        <link rel="stylesheet" href="assets/js/summernote/summernote.css" />
        <script src="assets/js/summernote/summernote.min.js"></script>


        <script type="text/javascript">

            var dataobjupdate = 0;

            $(document).ready(function () {


                if (getParameterByName("idcategory") > 0) {

                    $("#showfaq").removeClass("hide");
                    getfaq();
                }


                InitLmsRb();



                var FieldCount = 1;






            });



            $("body").on('click', '.updateobject', function () {

                dataobjupdate = $(this).attr('data-obj');
                var answer = $("#answer_" + dataobjupdate + "").html();
                var question = $("#question_" + dataobjupdate + "").text()

               CKEDITOR.instances['editor1'].setData(answer);
                $("#fieldquestion").val(question);



            });



            $("body").on('click', '.deleteobject', function () {

                var dataobj = $(this).attr('data-obj');

                var arr = dataobj.split('|');
                var urllink = "WFMakeFaq.aspx?op=deletesource&id=" + dataobj
                var requestdelete = $.ajax({
                    url: urllink,
                    type: "GET",
                    datatype: "html"
                });


                requestdelete.fail(function (data) {
                    alert(data);

                });


                requestdelete.success(function (data) {

                    location.reload()


                });

            });



            $("body").on("click", ".addclass", function (e) { //user click on remove text
                e.preventDefault();

                $question = $("#fieldquestion").val();
               
                $answer =CKEDITOR.instances['editor1'].getData()

                if ($question == '' && $answer == '') {
                    alert("Impossibile salvare campi vuoti!")
                } else {

                    var request4 = $.ajax({
                        type: "POST",
                        url: "AdminAjaxLMS.aspx?op=modfaq&oper=addfield&idfaq=" + dataobjupdate + "&idcategory=" + getParameterByName("idcategory") + "",
                        data: { question: $question, answer: $answer },
                        dataType: "html"
                    });


                    request4.success(function (data) {
                        getfaq();
                        $("#fieldanswer").code('');
                        $("#fieldquestion").val('');;

                    });


                }
            });


            $("body").on("click", ".updatefaq", function (e) { //user click on remove text

                if($("title_faq").val()!=  "" && $("#materia option:selected").val() != "" &&  $("#anno option:selected")!="" ) {

                    $.ajax({
                        type: "POST",
                        url: "AdminAjaxLMS.aspx?op=modfaq&oper=createfaq",
                        dataType: "html",
                        data: { anno: $("#anno option:selected").val(), materia: $("#materia option:selected").val(), title: $('#txtname_faq').val(), description: '', idcategory: getParameterByName("idcategory") },
                        success: function (data) {
                            if (data > 0) {
                                $("#showfaq").removeClass("hide");
                                location.href = "?idcategory=" + data;
                            }
                        }


                    });


                }else{
                
                alert("Attenzione riempire i campi obbligatori!")
                }

            });

            function getfaq() {


                var request4 = $.ajax({
                    type: "GET",
                    url: "AdminAjaxLMS.aspx?op=modfaq&oper=get&idfaq=" + getParameterByName("idcategory"),
                    dataType: "html"
                });



                request4.success(function (data) {

                   CKEDITOR.instances['editor1'].setData('');
                    $("#fieldquestion").val('');;

                    $("#faq_list").empty(),
                        $("#faq_list").html(data);



                });
                return false;
            }

            function orderfaq(id, sequence, offset) {


                sequence = sequence++;

                var request4 = $.ajax({
                    type: "GET",
                    url: "?op=modfaq&oper=updatesequence&idcategory=" + getParameterByName("idcategory") + "&sequence=" + sequence + "&offset=" + offset + "&idfaq=" + id,
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
