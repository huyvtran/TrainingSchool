<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/LMSSw.Master"   %>

 
<%@ Import Namespace="LMS" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.SessionState" %>


<script  runat="server">
	
   Dim scr As New Scorm
	

	Sub Page_Load(sender as Object, e as EventArgs) Handles Me.Load
        If Request.QueryString("reference") <> "" Then
            Try
            Page.Title=scr.title
                scr.GetScormByReference(Request.QueryString("reference"),Session("idcourse"))
                
            Catch ex As Exception
                Response.Redirect("~/s500.aspx")
            End Try
        End If
        
        'Set Session Vars
     

    End Sub

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
		<script language="JavaScript" src="LMSCONTENT/tinylms/lib/listlogger.js" type="text/JavaScript"></script>
		<script language="JavaScript" src="LMSCONTENT/tinylms/lib/collections.js" type="text/JavaScript"></script>
		<script language="JavaScript" src="LMSCONTENT/tinylms/lib/cookies.js" type="text/JavaScript"></script>
		<script language="JavaScript" src="LMSCONTENT/tinylms/lib/sha1.js" type="text/JavaScript"></script>
		<script language="JavaScript" src="LMSCONTENT/tinylms/lib/lmscam.js" type="text/JavaScript"></script>
        <script language="JavaScript" src="LMSCONTENT/tinylms/lib/soapclient.js" type="text/JavaScript"></script>
		<script language="JavaScript" src="LMSCONTENT/tinylms/lib/lmsrb.js" type="text/JavaScript"></script>
		<script language="JavaScript" src="LMSCONTENT/tinylms/lib/lmsapi.js" type="text/JavaScript"></script>
		<script language="JavaScript" src="LMSCONTENT/tinylms/lib/lmslabels_en.js" type="text/JavaScript"></script>
		<script language="JavaScript" src="<%=Session("lmscontentpathrel") %>/<% =scr.CoursePath %>imsmanifest.js?ver=1" type="text/JavaScript"></script>
		<script language="JavaScript" type="text/JavaScript">




		    // LMS Configuration
		    // -----------------

		    // Users

		    var userArray = [
                ['<% =scr.UserName %>', new User('<% =scr.UserName %>', null, '<% =scr.FullName %>')]
		    ];
		    API.userMap = new Map();
		    API.userMap.importFromArray(userArray);

		    //  learning_organization  Structure
		    API. learning_organization Structure = API.STRUCTURE_HIERARCHICAL;

		    // Column Headers
		    API.camColumnNames = null;

		    // Sequencing
		    API.isAutomaticSequencing = true;

		    // Version info
		    API.version = '1.3.2 2004-06-20';

		    // Use of TinyLMS as a SCORM to SCORM Adapter
		    API.setSCORMAdapter(false);

		    // Debugging options
		    API.loglevel = logger.INTERNAL;
		    logger.level = API.loglevel;
		    API.showDebugButtons = true;
		    API.showBugInfoButton = true;
		    API.Reference = "<% =scr.Reference%>";
	API.CourseID = "<% =scr.CourseId %>";
		    API.UserID = "<% =scr.UserID  %>";
		    API.IDEnter = "<% = HttpContext.Current.Session("id_enter_course") %>";
		    API.CoursewarePath = "<% =scr.CoursePath %>";
		    //API.WS.URL = "http://"+location.host+"/DotNetNuke/DesktopModules/SCORMKit.lmslite/LMSPostBack.asmx"
		    var temppath = unescape(location.href)
		    var pathend = temppath.lastIndexOf("/") + 1
		    API.WS.URL =  temppath.substring(0, pathend) + "LMSCONTENT/LMSPostBack.asmx"




		    var origCols;
		    function toggleFrame(elem) {
		        if (origCols) {
		            elem.firstChild.nodeValue = "<<Hide Navbar";
		            setCookie("frameHidden", "false", getExpDate(180, 0, 0));
		            showFrame();
		        } else {
		            elem.firstChild.nodeValue = "Show Navbar>>";
		            setCookie("frameHidden", "true", getExpDate(180, 0, 0));
		            hideFrame();
		        }
		    }
		    function hideFrame() {
		        var frameset = document.getElementById("masterFrameset");
		        origCols = frameset.cols;
		        frameset.cols = "0, *";
		    }

		    function showFrame() {
		        document.getElementById("masterFrameset").cols = origCols;
		        origCols = null;
		    }

		    // set frame visibility based on previous cookie setting
		    function setFrameVis() {
		        if (document.getElementById) {
		            if (getCookie("frameHidden") == "true") {
		                hideFrame()
		            }
		        }
		    }

		    API.start();
		    API.login('<% =scr.UserName %>', '');
		</script>
		
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 

	<div id="framescorm" style="width:400px;height=400px"></div>
<%--<frameset id="masterFrameset" cols="5,*" frameborder="0" border="0" scrolling="NO" framespacing="0" onload="">
		<frame src="tinylms/lmstreetoc.html"  height=0 width=0 name="lmsTOCFrame" scrolling="NO">
		<frame scrolling="no" src="tinylms/lmsempty.html"  name="lmsContentFrame">
	</frameset>--%>
   

</asp:Content>
