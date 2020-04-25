

// This function will start the debug process, if the window is not open 
// it will open a window to log debug calls to.
function debug_Start(sco_info)
{
		 var NOW = new Date();
		 var date = (NOW.getMonth()+1) + "/" + (NOW.getDay()+1) + "/" + NOW.getFullYear() + "&nbsp;&nbsp;" + NOW.getHours() + ":" + NOW.getMinutes();
		  
		var w = 400;
		var h = 600;

		if ((!this.wndDebug) || (this.wndDebug.closed)) {
			window.status = "Opening popup log window";
			var features = "width=" + w + ",height=" + h + ",scrollbars,resizable,dependent";
			this.wndDebug = lmspopup("","WndPopupLog",features);
			window.focus();

	        navTempText = "<html>\n";
			navTempText += "   <head>\n";
			navTempText += "      <meta http-equiv=\"expires\" content=\"Tue, 20 Aug 1999 01:00:00 GMT\">\n";
			navTempText += "      <meta http-equiv=\"Pragma\" content=\"no-cache\">\n\n";
			navTempText += "      <title>Test Log</title>\n";
			navTempText += "      <style>\n";
			navTempText += "         table.logTitle\n";
			navTempText += "         {\n";
			navTempText += "            font-size: 12px;\n";
			navTempText += "            font-family :  Arial, Helvetica, sans-serif;\n";
			navTempText += "            color: Purple;\n";
			navTempText += "            text-align: center;\n";
			navTempText += "         }\n";
			navTempText += "      </style>\n";
			navTempText += "   </head>\n\n";
			navTempText += "   <body>";
			navTempText += "      <table border=\"0\" width=\"100%\">";
			navTempText += "         <tr><td>";
			navTempText += "               <div id=\"LogTitleLayer\" name=\"LogTitleLayer\" align=\"center\"><b>LMS API Debug Log started"
			navTempText += "               " + date + "<br>" + sco_info + "</b></center></div>";
			navTempText += "         </td></tr>";
			navTempText += "      </table>";
			navTempText += "      <br />";
			navTempText += "      <div id=\"logText\" name=\"logText\"> </div>";
			navTempText += "   </body></html>";

			this.wndDebug.document.write(navTempText);
		
		}
	else  // Log is already open, so we start a new log entry
		{

			this.writeLogEntry( 3, "<center><b>LMS API Debug Log started " + date + "<br>" + sco_info + "</b></center>");
		}

}

// This function will stop the debug log and print summary info
// allowing us to log data for several SCO's
function debug_End()
{
	var NOW = new Date();
	var date = (NOW.getMonth()+1) + "/" + (NOW.getDay()+1) + "/" + NOW.getFullYear() + "&nbsp;&nbsp;" + NOW.getHours() + ":" + NOW.getMinutes();

	this.writeLogEntry( 3, "LMS API Log stopped at: " + date);

}


// This will close the debug window completely. It should be called during the 
// onunload event for the frameset to ensure no orphaned windows are left
function debug_Close()
{

		if (this.wndDebug) {
			this.wndDebug.close();
			this.wndDebug = null;
		}

}

//************************************************************************
// Function: writeLogEntry
//
// Description:  This function is responsible for formatting and writing out
//  the log text to the Log.htm page.
//************************************************************************
function writeLogEntry(type, msg)
{
	// the supported types are:
	//      0 = informational (diagnostic, trace, etc.)
	//      1 = warning
	//      2 = display no icon and use default font.

	var displayMsg = "";
	if (type == 0)
	{
		displayMsg += "     <img src=\"images/smallinfo.gif\">" +
                    "     <font style=\"font-size:15px;\" color=\"green\">";
	}
	else if (type == 1)
	{
		displayMsg += "     <img src=\"images/smallwarning.gif\">" +
					"     <font style=\"font-size:15px;\" color=\"red\"> WARNING:";
	}
	else if (type == 2)
	{
		displayMsg += "     <img src=\"images/spacer.gif\">" +
						"     <font style=\"font-size:15px;\" color=\"black\">";
	}
	else
	{
		displayMsg += "     <font style=\"font-size:15px;\" color=\"black\">";
	}

	displayMsg += "&nbsp;" + msg + "</font>";
	displayMsg += "<br>";
	this.wndDebug.document.write(displayMsg);
}


// Writes a normal message to the log.
function debug_Write(msg)
{
	if (this.wndDebug) {
		this.writeLogEntry(2, msg)
	}
}


// Writes a failure message to the log.
function debug_WriteFailure(msg)
{
	if (this.wndDebug) {
		this.writeLogEntry(1, msg)
	}

}

// Writes an API Result message to the log
function debug_WriteResults(msg)
{
	if (this.wndDebug) {
		this.writeLogEntry(0, msg)
	}
}


/**
 * This is the constructor for a Debugging object.
 */
function objDebug() {

// the Window Handle to the Debug Window
this.wndDebug = null;

this.Start = debug_Start;
this.End = debug_End;
this.Close = debug_Close;
this.Write = debug_Write;
this.WriteFailure = debug_WriteFailure;
this.WriteResults = debug_WriteResults;
this.writeLogEntry = writeLogEntry;

}
/**
 * Create the debugLog object
 */
var debugLog = new objDebug();
