<%@ Page Language="VB" %>

<%@ Import Namespace="LMS" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.SessionState" %>

<script  runat="server">

    Dim scr As Scorm


    Sub Page_Load(sender As Object, e As EventArgs)

        scr=New Scorm()
        If Request.QueryString("reference") <> "" Then
            Try

                scr.GetScormByReference(Request.QueryString("reference"), Session("idcourse"))

            Catch ex As Exception
                Response.Redirect("~/s500.aspx")
            End Try
        End If

        'Set Session Vars


    End Sub

</script>
<html>
	<head runat="server">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
         <meta name="viewport" content="width=device-width, initial-scale=1.0" />
		<title><% =scr.title %></title>
         <script  type="text/javascript" src="https://code.jquery.com/jquery-latest.js"></script>
   
		<script  src="tinylms/lib/listlogger.js" type="text/JavaScript"></script>
		<script  src="tinylms/lib/collections.js" type="text/JavaScript"></script>
		<script  src="tinylms/lib/cookies.js" type="text/JavaScript"></script>
		<script  src="tinylms/lib/sha1.js" type="text/JavaScript"></script>
		<script  src="tinylms/lib/lmscam.js" type="text/JavaScript"></script>
        <script  src="tinylms/lib/soapclient.js" type="text/JavaScript"></script>
		<script  src="tinylms/lib/lmsrb.js" type="text/JavaScript"></script>
		<script  src="tinylms/lib/lmsapi.js?ver=5" type="text/JavaScript"></script>
		<script  src="tinylms/lib/lmslabels_en.js" type="text/JavaScript"></script>
       
		<%--<script language="JavaScript" src="<% =scr.Coursepath %>imsmanifest.js" type="text/JavaScript"></script>
	--%>
        <script>
            // Content Aggregation Model
// -------------------------
API.cam = new ManifestElement("VideoCorso","1.2",
  new OrganizationsElement([
    new OrganizationElement("0","VideoCorso",[
      new ItemElement("1","A2013","2",null,null,[])
    ])
  ])
  ,
  new ResourcesElement([
    new ResourceElement("2","<% =scr.ref %>")
  ])
);

// LMS Configuration
// -----------------

// Users

var userArray = [
['guest', new User('guest',null,'Guest, Guest')]
];
API.userMap = new Map();
API.userMap.importFromArray(userArray);

// Organization Structure
API.organizationStructure = API.STRUCTURE_HIERARCHICAL;

// Column Headers
API.camColumnNames = null;

// Quiz mode
API.isQuiz = false;

// Sequencing
API.isAutomaticSequencing = true;

// Version info
API.version = '1.9 2010-02-10';

// Use of TinyLMS as a SCORM to SCORM Adapter
API.setSCORMAdapter(false);

// Help out poor IE6 to do its job
API.tocWidth =192;
API.tocHeight = 580;

// Debugging options
API.showBugInfoButton = false;

        </script>
            

         <script language="JavaScript" type="text/JavaScript">

            

		    window.onunload = function () {
		       
		        if (window.top.opener && !window.top.opener.closed) {
		            window.top.opener.popUpClosed();
		        }
		    };

	// LMS Configuration
	// -----------------

	// Users

	var userArray = [
		['<% =scr.UserName %>', new User('<% =scr.UserName %>',null,'<% =scr.FullName %>')]
		];
	API.userMap = new Map();
	API.userMap.importFromArray(userArray);

	//  learning_organization  Structure
	API.organizationstructure = API.STRUCTURE_HIERARCHICAL;

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
	var pathend = temppath.lastIndexOf("/")+1
	API.WS.URL = temppath.substring(0,pathend)+"LMSPostBack.asmx"




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
		</script>
		<script language="JavaScript" type="text/JavaScript">
API.start();
		</script>
	</head>
	
<frameset id="masterFrameset" cols="5,*" frameborder="0" border="0" scrolling="NO" framespacing="0" onload=" hideFrame();API.login('<% =scr.UserName %>','');">
		<frame src="tinylms/lmstreetoc.html"  height=0 width=0 name="lmsTOCFrame" scrolling="NO">
		<frame scrolling="no" src="tinylms/lmsempty.html"  name="lmsContentFrame">
	</frameset>

    
    <body>
    </body>
</html>
