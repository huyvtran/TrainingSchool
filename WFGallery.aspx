<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master" CodeBehind="WFGallery.aspx.vb" Inherits="TrainingSchool.WFGallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-content">

        <div class="page-header">
            <h3>Video tutorial</h3>
        </div>
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12 col-xs-12">

                <div class="col-sm-2">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a class="linkSource">
                                <img src="//img.youtube.com/vi/gh38Vos8Y58/0.jpg" alt="https://youtu.be/gh38Vos8Y58" class="img-responsive">
                            </a>
                        </div>
                        <div class="panel-footer text-center">Video introduttivo delle funzionalità generali</div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a class="linkSource">
                                <img src="//img.youtube.com/vi/ZSSrk_z5ev4/0.jpg" alt="https://www.youtube.com/vi/ZSSrk_z5ev4" class="img-responsive">
                            </a>
                        </div>
                        <div class="panel-footer text-center">Creazione di una lezione on demand</div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a class="linkSource">
                                <img src="//img.youtube.com/vi/Hqe6NmqvyQk/0.jpg" alt="https://www.youtube.com/vi/Hqe6NmqvyQk" class="img-responsive">
                            </a>
                        </div>
                        <div class="panel-footer text-center">Creazione di una lezione in diretta webinar</div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a class="linkSource">
                                <img src="//img.youtube.com/vi/8n7z0CGwxAQ/0.jpg" alt="https://www.youtube.com/vi/8n7z0CGwxAQ" class="img-responsive">
                            </a>
                        </div>
                        <div class="panel-footer text-center">Creazione di un corso on demand</div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a class="linkSource">
                                <img src="//img.youtube.com/vi/gqoVjJgRBu0/0.jpg" alt="https://www.youtube.com/vi/gqoVjJgRBu0" class="img-responsive">
                            </a>
                        </div>
                        <div class="panel-footer text-center">Organizzazione oggetti didattici nel corso on demand</div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a class="linkSource">
                                <img src="//img.youtube.com/vi/eIi6X1vFPFs/0.jpg" alt="https://www.youtube.com/vi/eIi6X1vFPFs" class="img-responsive">
                            </a>
                        </div>
                        <div class="panel-footer text-center">Consultazione report corso on demand</div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Video Tutorial</h4>
                </div>
                <div class="modal-body">
                    <div id="carouselgallery" class="carousel slide" data-ride="carousel">

                        <!-- Wrapper for slides -->
                        <div class="carousel-inner">
                            <div class="item active">
                                <div class="embed-responsive embed-responsive-4by3">
                                    <iframe id="player" class="embed-responsive-item" src="https://www.youtube.com/embed/gh38Vos8Y58?enablejsapi=1"></iframe>
                                </div>
                            </div>
                            <div class="item">
                                <div class="embed-responsive embed-responsive-4by3">
                                    <iframe class="embed-responsive-item" src="https://www.youtube.com/embed/ZSSrk_z5ev4?enablejsapi=1"></iframe>
                                </div>
                            </div>
                            <div class="item">
                                <div class="embed-responsive embed-responsive-4by3">
                                    <iframe class="embed-responsive-item" src="https://www.youtube.com/embed/Hqe6NmqvyQk?enablejsapi=1"></iframe>
                                </div>

                            </div>
                            <div class="item ">
                                <div class="embed-responsive embed-responsive-4by3">
                                    <iframe class="embed-responsive-item" src="https://www.youtube.com/embed/8n7z0CGwxAQ?enablejsapi=1"></iframe>
                                </div>
                            </div>
                            <div class="item">
                                <div class="embed-responsive embed-responsive-4by3">
                                    <iframe class="embed-responsive-item" src="https://www.youtube.com/embed/gqoVjJgRBu0?enablejsapi=1"></iframe>
                                </div>
                            </div>
                            <div class="item">
                                <div class="embed-responsive embed-responsive-4by3">
                                    <iframe class="embed-responsive-item" src="https://www.youtube.com/embed/eIi6X1vFPFs?enablejsapi=1"></iframe>
                                </div>

                            </div>
                        </div>
                        <!-- Controls -->
                        <a class="left carousel-control" href="#carouselgallery" role="button" data-slide="prev">
                            <span class="glyphicon glyphicon-chevron-left"></span>
                        </a>
                        <a class="right carousel-control" href="#carouselgallery" role="button" data-slide="next">
                            <span class="glyphicon glyphicon-chevron-right"></span>
                        </a>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <script>



        $(".linkSource").click(function () {
            $(".carousel").carousel({ interval: 1000000 });
            $("#myModal").modal("show");
        });


        $('#myModal').on('hidden.bs.modal', function () {
            location.reload();
        })
    </script>

</asp:Content>
