/*
 * @(#)listlogger.js 1.1  2004-06-16
 *
 * Copyright (c) 2004 Werner Randelshofer
 * Staldenmattweg 2, Immensee, CH-6405, Switzerland
 * All rights reserved.
 * 
 * This software is the confidential and proprietary information of 
 * Werner Randelshofer. ("Confidential Information").  You shall not
 * disclose such Confidential Information and shall use it only in
 * accordance with the terms of the license agreement you entered into
 * with Werner Randelshofer.
 *
 *
 * This software has been derived from lmsdebug.js 
 * Copyright (c) 2004 Kent Ogletreee
 * All rights reserved.
 * Code of lmsdebug.js used by permission.
 */ 

/**
 * This file provides logging functionality.
 *
 * @author Werner Randelshofer, Kent Ogletree
 * @version 1.1 2004-06-16 Uses now a style sheet.
 * 1.0 2004-06-13 Created.
 */



// This function will open a window to log debug calls to.
function logger_open() {
	if ((!this.logWindow) || (this.logWindow.closed)) {
		var w = 400;
		var h = 600;

		var now = new Date();
		var date = now.getFullYear()+"-"+(now.getMonth()+1)+"-"+(now.getDay()+1) + " "
		+ ((now.getHours() < 10) ? "0" : "") + now.getHours() + ":" + ((now.getMinutes() < 10) ? "0" : "") + now.getMinutes();

		
		window.status = "Opening popup log window";
		var features = "left=0,screenX=0,width=" + w + ",height=" + h + ",scrollbars,resizable,dependent";
		//this.logWindow = lmspopup("","logwindow",features);
		//window.focus();
		
		navTempText = "<html>\n";
		navTempText += "   <head>\n";
		navTempText += "      <meta http-equiv=\"expires\" content=\"Tue, 20 Aug 1999 01:00:00 GMT\"></meta>\n";
		navTempText += "      <meta http-equiv=\"Pragma\" content=\"no-cache\"></meta>\n\n";
		navTempText += "      <link href=\""+this.topURL+"tinylms/listlogger.css\" rel=\"stylesheet\" type=\"text/css\">\n";
		navTempText += "      <title>"+this.title+"</title>\n";
		navTempText += "   </head>\n\n";
		navTempText += "   <body>";
		navTempText += "      <p class=\"log_milestone\">LMS API Debug Log started at "+ date+"</p>";
		navTempText += "   </body></html>";
		
		//this.logWindow.document.write(navTempText);
	}
}
	
// This will close the debug window completely. It should be called during the 
// onunload event for the frameset to ensure no orphaned windows are left
function logger_close() {
	if (this.logWindow) {
	    if (this.scrollDownTimeoutID != null) {
		   this.logWindow.cancelTimeout(this.scrollDownTimeoutID);
		   this.scrollDownTimeoutID = null;
		}
		this.logWindow.close();
		this.logWindow = null;
	}
}


// This function is responsible for formatting and writing out
//  the log text to the Log.htm page.
function logger_write(type, message) {
	if (type <= this.level) {
    	this.open();
		
		var displayMsg = "";
		var now = new Date();
		var date = now.getFullYear()+"-"+(now.getMonth()+1)+"-"+(now.getDay()+1) + " "
		+ ((now.getHours() < 10) ? "0" : "") + now.getHours() + ":" + ((now.getMinutes() < 10) ? "0" : "") + now.getMinutes();
		
		switch (type) 	{
			case this.MILESTONE :
				displayMsg = "<p class=\"log_milestone\">"+date+" "+message+"</p>"
				break;
			case this.INFO :
				displayMsg = "<p class=\"log_info\">"+date+" "+message+"</p>"
				break;
			case this.API_FAILURE :
				displayMsg = "<p class=\"log_api_failure\"><img src=\""+this.topURL+"tinylms/images/symbols/log_api_failure.gif\">"+message+ "</p>"
				break;
			case this.API_SUCCESS :
				displayMsg = "<p class=\"log_api_success\"><img src=\""+this.topURL+"tinylms/images/symbols/log_api_success.gif\">"+message+ "</p>"
				break;
			case this.API :
				displayMsg = "<p class=\"log_api\"><img src=\""+this.topURL+"tinylms/images/symbols/log_api.gif\">"+message+ "</p>"
				break;
			case this.INTERNAL_FAILURE :
				displayMsg = "<p class=\"log_internal_failure\"><img src=\""+this.topURL+"tinylms/images/symbols/log_internal_failure.gif\">"+message+ "</p>"
				break;
			case this.INTERNAL_SUCCESS :
				displayMsg = "<p class=\"log_internal_success\"><img src=\""+this.topURL+"tinylms/images/symbols/log_internal_success.gif\">"+message+ "</p>"
				break;
			case this.INTERNAL :
				displayMsg = "<p class=\"log_internal\"><img src=\""+this.topURL+"tinylms/images/symbols/log_internal.gif\">"+message+ "</p>"
				break;
		}

		//this.logWindow.document.writeln(displayMsg);
		
		// Scroll down.
		// Scrolling is a very expensive operation. We use a timeout here, to prevent us
		// from scrolling more than 10 times per second.
		if (this.scrollDownTimeoutID == null) {
		   this.scrollDownTimeoutID = window.setTimeout("logger.scrollDown()", 100);
		}		
		this.lastSecond = now;
	}
}

/**
 * This function scroll the window down.
 * It is called from within function logger_write().
 */
function logger_scrollDown() {
//This should scroll to the exact position, but its not quite right for Netscape 7.1 for Mac OS X.
   //if (this.logWindow.innerHeight < this.logWindow.document.height) {
   //	  this.logWindow.scrollBy(0, this.logWindow.document.height - this.logWindow.pageYOffset - this.logWindow.innerHeight);
   //}
 
   // this.logWindow.scrollBy(0, this.logWindow.document.height - this.logWindow.pageYOffset);
   // this.scrollDownTimeoutID = null;   
}


/**
 * This is the constructor for a Logger object.
 */
function Logger() {
	// the title of the log window
	this.title = "Log Window";

	// the Window Handle to the Debug Window
	this.logWindow = null;

    // We need this variable to control the timeout for scrolling down
	this.scrollDownTimeoutID = null;

	/**
	 * Message types.
	 */
	this.MILESTONE = 1;
	this.INFO = 2;
	this.API_FAILURE = 3;
	this.API_SUCCESS = 4;
	this.API = 5;
	this.INTERNAL_FAILURE = 6;
	this.INTERNAL_SUCCESS = 7;
	this.INTERNAL = 8;

	/**
	 * Enables log output for specific types.
	 * Number between 0 and 8. This correspondes more or less with
	 * the message types. In general, The higher the level, 
	 * the more messages are logged.
	 */
	this.level = 0;

	this.open = logger_open;
	this.close = logger_close;
	this.write = logger_write;
	this.scrollDown = logger_scrollDown;
	
	// Determine the top level URL 
	// Remove the filename of the frameset at the end, but preserve
	// the final slash.
	this.topURL = window.location.href;
	this.topURL = this.topURL.substring(0, this.topURL.lastIndexOf("/")+1);	
}
/**
 * Create the logger object
 * Note: If you change this variable name, you also have to change the setTimeout call in
 * function loger_write.
 */
var logger = new Logger();