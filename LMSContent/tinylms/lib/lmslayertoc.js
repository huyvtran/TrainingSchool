/*
 * @(#)lmslayertoc.js 1.2.1 2004-06-14
 *
 * Copyright (c) 2003 Werner Randelshofer
 * Staldenmattweg 2, Immensee, CH-6405, Switzerland
 * All rights reserved.
 *
 * This software is the confidential and proprietary information of 
 * Werner Randelshofer. ("Confidential Information").  You shall not
 * disclose such Confidential Information and shall use it only in
 * accordance with the terms of the license agreement you entered into
 * with Werner Randelshofer.
 */ 
/**
 * This scripts inserts a table of contents (TOC) at the current location
 * into a HTML document.
 *
 * This file is intended to be included in the body of a HTML document. The
 * HTML document must be in a child frame of the Learning Management System
 * generated by TinyLMS.
 *
 * Example:
 * <html>
 *   <head>
 *     <title>Table Of Contens</title>
 *     <script language="JavaScript" src="lib/lmscollections.js" type="text/JavaScript"></script>
 *     <script language="JavaScript" src="lib/lmsfonts.js" type="text/JavaScript"></script>
 *     <script language="JavaScript" src="lib/lmsstub.js" type="text/JavaScript"></script>
 *   </head>
 *   <body>
 *     <script language="JavaScript" src="lib/lmslayertoc.js" type="text/JavaScript"></script>
 *   </body>
 * </html>
 *
 * @version 1.2.1 2004-06-14 We must not select an item when TinyLMS is not in course mode.
 * 1.2 2004-06-13 Support for debugging improved.
 * <br> 1.1.7 2003-11-06 Support for debugging added. 
 * <br>1.1 2003-11-04 Adapted to changes in lmsapi.js and lmscam.js.
 * <br>1.0.1 2003-10-30 Retrieve version number from API.
 * <br>1.0 2003-09-12  Locale specific labels are now read from the API object.
 * <br>0.21 2003-06-30 Function showDebugInfo() added.
 * <br>0.20.1 2003-04-07 The Safari browser can not handle return statements
 * within switch statements.
 * <br>0.19.4 2003-04-04 Revised.
 * <br>0.19.3 2003-04-01 Revised.
 * <br>0.19 2003-03-26 Revised.
 * <br>0.18 2003-03-26 Created.
 */

/**
 * Writes the title of the table contents.
 * The title is the name of the current CAM organization.
 *
 * @param selectedItem the current item within the current CAM organization.
 */
function writeOrganization(organizationElement) {
	document.writeln('<tr><td valign="top" class="organization" height="52">');
  document.writeln('<a href="#" onclick="stub.getAPI().gotoMenu()">'+organizationElement.title+'</a>'); 
	document.writeln('</td></tr>');
}

function writeIcon(lessonStatus) {
		switch (lessonStatus) {
			case "passed" :
			case "completed" :
				document.writeln('<img src="images/symbols/sym_toc_passed.gif" width="12" height="9" border="0">');
				break;
			case "failed" :
				document.writeln('<img src="images/symbols/sym_toc_failed.gif" width="12" height="9" border="0">');
			  break;
			case "incomplete" :
			case "browsed" :
				document.writeln('<img src="images/symbols/sym_toc_browsed.gif" width="12" height="9" border="0">');
			  break;
			case "not attempted" :
			case "" :
			case null :
				document.writeln('<img src="images/symbols/sym_toc_spacer.gif" width="12" height="9" border="0">');
			  break;
			case "current" :
				document.writeln('<img src="images/symbols/sym_toc_current.gif" width="12" height="9" border="0">');
			  break;
			default :
			  document.writeln("lessonStatus:"+lessonStatus);
				break;
		}
}
function getCSSClass(lessonStatus) {
  /*
  if (lessonStatus == "passed"
	|| lessonStatus == "completed") 
		return "Passed";
  else if (lessonStatus == "failed"
	|| lessonStatus == "incomplete"
	|| lessonStatus == "browsed") 
		return "Visited";
  else if (lessonStatus == "not attempted"
	|| lessonStatus == "")
		return "";
  else if (lessonStatus == "current")
		return "Current";
  else
	  return "";*/
  result = "";
	switch (lessonStatus) {
		case "passed" :
		case "completed" :
			result = "Passed";
			break;
		case "failed" :
		case "incomplete" :
		case "browsed" :
			result = "Visited";
			break;
		case "not attempted" :
		case "" :
			result = "";
			break;
		case "current" :
			result = "Current";
			break;
		default :
			result = "";
			break;
	}
	return result;
}


/**
 * Writes the toc starting from depth 0 for the
 * subtree starting from the specified item.
 *
 * @param currentOrganization the current organization.
 * @param node The root of the subtree. node must be an instance of lmscam.js:ItemElement
 * @param selectedItem the selected item. selectedItem must be an instance of lmscam.js:ItemElement
 * @param depth The depth of the node in the TOC tree (must be 0).
 */
function TOC_writeTOC1(currentOrganization, node, selectedItem, depth) {
	var title = cropString(node.title,27);

	var currentColumnName = stub.getAPI().getCurrentColumnName();
  var refItem = node;
  var columnItem = null;
	if (currentColumnName != null) {
 	  for (var i=0; i < node.getChildCount(); i++) {
      columnItem = node.getChildAt(i);
  	  if (columnItem.title == currentColumnName) {
				refItem = columnItem;
				break;			
			}
	  }	
	}
	
	var resource = refItem.getResource();
  while (resource == null && refItem.getChildCount() != 0) {
    refItem = refItem.getChildAt(0);
 		resource = refItem.getResource();
		columnItem = currentOrganization.getColumnOfItem(refItem);
  }
	
	
	var lessonStatus = (resource == null) ? "" : resource.cmi_core_lesson_status;
	if (node.isNodeDescendant(selectedItem)) lessonStatus = "current"; 
  var iconStatus = lessonStatus;
	var cssClass = getCSSClass(lessonStatus);
	document.writeln('<tr><td valign="top" background="images/bg_toc_item.gif" class="toc1'+cssClass+'" height="18">');
	if (resource == null) {
	  document.write('<span class="toc1Disabled">');
		document.write(title); 
	  document.write('</span>');
	} else if (columnItem == null || columnItem.title != currentColumnName) {
		if (resource != null) document.write('<a class="toc1Disabled" href="#" onClick="stub.getAPI().gotoItemWithID(\''+refItem.identifier+'\')">');
		// writeIcon(iconStatus);
		document.write(title); 
		if (resource != null) document.write('</a>'); 
	} else {
		if (resource != null) document.write('<a class="toc1'+cssClass+'" href="#" onClick="stub.getAPI().gotoItemWithID(\''+refItem.identifier+'\')">');
		// writeIcon(iconStatus);
		//document.write('toc1'+cssClass);
		document.write(title); 
		if (resource != null) document.write('</a>'); 
	}
	document.writeln('</td></tr>');
}

/**
 * Writes the table of contents.
 * The TOC entries are retrieved from the LMS using 
 * protected operations.
 */
function TOC_writeTOC() {
	var api = stub.getAPI();
	if (api.isLoggedIn()) {
		//var selectedItem = api.getAnticipatedItem();
		var selectedItem = null;
		if (api.mode == api.MODE_COURSE) {
			if (selectedItem == null) selectedItem = api.getCurrentItem();
		}
 		var currentOrganization = api.getCurrentOrganization();
		/*
	  // Count refreshes of the toc	
	  if (api.tocCounter == null) api.tocCounter = 1;
		else api.tocCounter++;
	  document.writeln(api.tocCounter+"<br>");
		*/
		document.writeln('<table width="100%" border="0" cellpadding="2" cellspacing="0" bordercolor="#FFFFFF">');
		
		writeOrganization(currentOrganization);
	  
		
		for (var i=0; i < currentOrganization.getChildCount(); i++) {
			var child = currentOrganization.getChildAt(i);
				//document.writeln('<tr><td valign="top" background="images/bg_nav.gif">');
				this.writeTOC1(currentOrganization, child, selectedItem, 0);
		}
		/*
		//document.writeln('<tr><td valign="top" class="tocButton" background="images/bg_toc_item.gif">');
		document.writeln('<tr height="100%"><td valign="bottom" class="tocButton">');
		document.writeln('<br>�<br>');
		document.writeln('<a class="tocButton" href="#" onclick="stub.getAPI().gotoLogin()">'+api.labels.get('toc.logoff')+'</a><br>');
		document.writeln('<a class="tocButton" href="#" onclick="stub.getAPI().gotoMenu()">'+api.labels.get('toc.menu')+'</a><br>');
		document.writeln('<a class="tocButton" href="#" onclick="stub.getAPI().gotoPreviousItem()">'+api.labels.get('toc.previous')+'</a><br>');
		document.writeln('<a class="tocButton" href="#" onclick="stub.getAPI().gotoNextItem()">'+api.labels.get('toc.next')+'</a><br>');
		document.writeln('</td></tr>');
		*/
		document.writeln("</table>");
	}
}

function TOC() {
	this.writeTOC = TOC_writeTOC;
	this.writeTOC1 = TOC_writeTOC1;
}

var t = new TOC();
t.writeTOC();