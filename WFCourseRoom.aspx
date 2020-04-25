<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSsw.Master"
    CodeBehind="WFCourseRoom.aspx.vb" Inherits="TrainingSchool.WFCourseRoom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        

        $(document).ready(function () {

                     


            jQuery('#tree2').ace_tree({
                dataSource: treeDataSource2,
                loadingHTML: '<div class="tree-loading"><i class="icon-refresh icon-spin blue"></i></div>',
                'open-icon': 'icon-folder-open',
                'close-icon': 'icon-folder-close',
                'selectable': false,
                'selected-icon': 'icon-ok',
                'unselected-icon': 'null'
            });

                  
            StartObj();
          



        });


        



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <!-- #page-content -->
    <div class="page-content">
        <div class="page-header">
           
          <h1>  <asp:Label ID="parentpage" Font-Size="Larger" runat="server">
                    
            </asp:Label></h1>
        </div>
        <!-- /.page-header -->
        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <div id="content" class="row" runat="server">
                   


                    <div id="materials" runat="server" visible="false" class="col-sm-12">
                        <div class="widget-box">
                            <div class="widget-header header-color-blue">
                                <h4 class="lighter smaller" runat="server" id="nomecorso">Materiali </h4>
                                
                            </div>
                            <div class="widget-body">
                                <div class="widget-main padding-8">
                                  <%--  <input type="button" id="treeselect" class="btn btn-cog btn-sm" value="Selezione" />--%>


                                    <div id="tree2" class="tree">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
</div>
              
                <div class="row">

                    <div id="view" runat="server" visible="false" class="col-sm-12">

                            <div class="widget-box">
                                <div class="widget-header header-color-blue">
                                    <h4 class="lighter smaller" runat="server" id="h2">Stato di Avanzamento</h4>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main padding-8">



                                        <div class="clearfix">


                                            <div class="col-sm-7 infobox-container">



                                                <div style="width: 350px" class="infobox infobox-orange  ">
                                                    <div class="infobox-icon">
                                                        <i class="icon-cog"></i>
                                                    </div>

                                                    <div class="infobox-data">
                                                        Ultimo oggetto didattico visualizzato
                                                    </div>
                                                    <span class="infobox-data-text">
                                                        <asp:Label ID="LastObjectView" runat="server"></asp:Label></span>
                                                </div>





                                                <div style="width: 350px" class="infobox infobox-orange  ">
                                                    <div class="infobox-icon">
                                                        <i class="icon-cog"></i>
                                                    </div>

                                                    <div class="infobox-data">

                                                        <div class="infobox-content">Completati</div>
                                                        <span class="infobox-data-number">
                                                            <asp:Label ID="readobj" runat="server"></asp:Label></span>

                                                    </div>
                                                    <div>
                                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                                    </div>
                                                </div>






                                                <div style="width: 350px" class="infobox infobox-green  ">
                                                    <div class="infobox-icon">
                                                        <div class="easy-pie-chart percentage easyPieChart" data-percent="<%=Session("percent")%>" data-size="39"
                                                            style="width: 39px; height: 39px; line-height: 39px;">
                                                            <span class="percent">
                                                                <asp:Label ID="percentUser" runat="server"></asp:Label></span>%
                                                <canvas width="42" height="42" style="width: 39px; height: 39px;"></canvas>
                                                        </div>
                                                    </div>

                                                    <div class="infobox-data">

                                                        <div class="infobox-content">% di Completamento</div>
                                                        <span class="infobox-data-number">
                                                            <asp:Label ID="Label2" runat="server"></asp:Label></span>

                                                    </div>
                                                    <div>
                                                        <asp:Label ID="Label5" runat="server"></asp:Label>
                                                    </div>
                                                </div>



                                                <div style="width: 350px" class="infobox infobox-green  ">
                                                    <div class="infobox-icon">
                                                        <i class="icon-book"></i>
                                                    </div>

                                                    <div class="infobox-data">

                                                        <div class="infobox-content">Totale Materiali</div>
                                                        <span class="infobox-data-number">
                                                            <asp:Label ID="totobj" runat="server"></asp:Label></span>

                                                    </div>
                                                    <div>
                                                        <asp:Label ID="Label4" runat="server"></asp:Label>
                                                    </div>
                                                </div>


                                                <div style="width: 350px" class="infobox infobox-green  ">
                                                    <div class="infobox-icon">
                                                        <i class="icon-film"></i>
                                                    </div>

                                                    <div class="infobox-data">

                                                        <div class="infobox-content">Tempo VideoLezioni</div>
                                                        <span class="infobox-data-number">
                                                            <asp:Label ID="lbTimeViewVideo" runat="server"></asp:Label></span>

                                                    </div>
                                                    <div>
                                                        <asp:Label ID="lbpercentVideoCompleted" runat="server"></asp:Label>
                                                    </div>
                                                </div>

                                                <div style="width: 350px" class="infobox infobox-red  ">
                                                    <div class="infobox-icon">
                                                        <i class="icon-time"></i>
                                                    </div>

                                                    <div class="infobox-data">

                                                        <div class="infobox-content">Tempo Totale</div>
                                                        <span class="infobox-data-number">

                                                            <asp:Label ID="lbTotalTimeSession" runat="server"></asp:Label></span>

                                                    </div>

                                                    <div>
                                                        <asp:Label ID="lbMaXTotalSession" runat="server"></asp:Label>

                                                    </div>
                                                </div>

                                            </div>

                                            <div class="panel-info ">

                                                <i class="icon-ok red bigger-130"></i>Test Fallito    
                                                    <div class="space-4"></div>
                                                <i class="icon-ok green bigger-130"></i>Completato    
                                                    <div class="space-4"></div>
                                                <i class="normal-icon icon-eye-open green  bigger-130"></i>Visibile    
                                                    <div class="space-4"></div>
                                                <i style="color: #CCC000" class="normal-icon icon-eye-open  bigger-130"></i>Incompleto    
                                                    <div class="space-4"></div>
                                                <i class="icon-lock red  bigger-130"></i>Oggetto bloccato<br />

                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>


                </div>
            </div>
            <!-- PAGE CONTENT ENDS -->
        </div>
        <!-- /.col -->
   
	
    



</asp:Content>
