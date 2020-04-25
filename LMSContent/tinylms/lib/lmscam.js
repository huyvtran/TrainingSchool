/*
 * @(#)lmscam.js 1.1  2003-11-04
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
 * This file contains Java Script prototypes that are used by lmsapi.js
 * to handle the metadata of a Content Aggregation Model (CAM).
 *
 * This file is intended to be included in the top level frameset of an 
 * eLearning course generated by TinyLMS.
 *
 * Example:
 * <html>
 *   <head>
 *     <title>Learning Management System</title>
 *     <script language="JavaScript" src="tinylms/lib/collections.js" type="text/JavaScript"></script>
 *     <script language="JavaScript" src="tinylms/lib/lms/lib/lmscam.js" type="text/JavaScript"></script>
 *   </head>
 * </html>
 *
 * Reference:
 * ADL (2001a). Advanced Distributed Learning.
 * Sharable Content Object Reference Model (SCORM�) Version 1.2. 
 * Internet (2003-01-20): http://www.adlnet.org
 *
 * ADL (2001b). Advanced Distributed Learning.
 * SCORM 1.2 Runtime Environment. 
 * Internet (2003-01-20): http://www.adlnet.org
 *
 * @author Werner Randelshofer, Staldenmattweg 2, Immensee, CH-6405, Switzerland
 * @version 1.1 2003-10-01 Constructor for User objects added. Constructor for ItemElement
 * extended.
 * 0.19 2003-03-28 Property isOrganizationElementItem renamed to isItemElement and
 * property isResource renamed to isResourceElement, property isOrganizationElement renamed
 * to isOrganizationElementElement.
 * 0.18 2003-03-26 Support for layered organization structures added.
 * 0.17 2003-03-16 Naming conventions for CAM elements streamlined with Java implementation.
 * 0.12 2003-03-05 Revised.
 * 0.1 2003-01-26 Created.
 */
 
var debug = false;
var debugResults = false;

/**
 * Searches this subtree for a node with the specified identifier.
 */
function CAMElement_findByIdentifier(identifier) {
  if (debug) alert("TreeNode.findByIdentifier("+identifier+")");
  if (identifier == this.identifier) return this;
  var result = null;
	for (var i=0; i < this.children.length; i++) {
	  result = this.children[i].findByIdentifier(identifier);
		if (result != null) break;
	}
  return result;
}
/**
 * Searches this subtree for a node with the specified identifierref.
 */
function CAMElement_findByIdentifierref(identifierref) {
  if (debug) alert("CAMElement.findByIdentifierref("+identifierref+")");
  if (identifierref == this.identifierref) return this;
  var result = null;
	for (var i=0; i < this.children.length; i++) {
	  result = this.children[i].findByIdentifierref(identifierref);
		if (result != null) break;
	}
  return result;
}
/**
 * Searches this subtree for a node with the specified href.
 */
function CAMElement_findByHRef(href) {
  if (debug) alert("CAMElement.findByHRef("+href+")");
  if (href == this.href) return this;
  var result = null;
	for (var i=0; i < this.children.length; i++) {
	  result = this.children[i].findByHRef(href);
		if (result != null) break;
	}
  return result;
}

/**
 * This is the constructor for an element of the Content Aggregation Model (CAM).
 */
function CAMElement() {
	this.identifier = null;
	this.identifierref = null;
	this.href = null;
	
	this.isOrganizationElement = false; // this attribute is set to true by OrganizationElement only
	this.isItemElement = false; // this attribute is set to true by ItemElement only
	this.isResourceElement = false; // this attribute is set to true by ResourceElement only
	
	this.findByIdentifier = CAMElement_findByIdentifier;
	this.findByIdentifierref = CAMElement_findByIdentifierref;
	this.findByHRef = CAMElement_findByHRef;
}
CAMElement.prototype = new TreeNode();

/**
 * This is the constructor for a CAM manifest element.
 */
function ManifestElement(identifier,version,organizations,resources) {
  this.identifier = identifier;
	this.version = version;
	
	this.organizations = (organizations != null) ? organizations : new OrganizationsElement(null);
	this.resources = (resources != null) ? resources : new ResourcesElement(null);
	
	this.add(this.organizations);
	this.add(this.resources);
}
ManifestElement.prototype = new CAMElement();

/**
 * This is the constructor for a CAM organizations node.
 */
function OrganizationsElement(children) {
  if (children != null) {
		for (var i=0; i < children.length; i++) {
			this.add(children[i]);
		}  
	}
}
OrganizationsElement.prototype = new CAMElement();
/**
 * Returns the column item of the specified item.
 * This method is used only for layered organization structures.
 * Returns null if the item is not in a column of this organization.
 */
function OrganizationElement_getColumnOfItem(itemElement) {
  var column = null;
	var row = itemElement;
	while (row != null && row.getParent() != this) {
	  column = row;
		row = row.getParent();
	}
	// this should never happen
	if (row == null) {
	  //alert("OrganizationElement.getColumnOfItem()\nItem is not part of this organization\nItem:"+itemElement+" title="+itemElement.title+" id="+itemElement.identifier);
		return null;
	}
	
	return column;
}
/**
 * Returns the row item of the specified item.
 * This method is used only for layered organization structures.
 * Returns null if the item is not in a row of this organization.
 */
function OrganizationElement_getRowOfItem(itemElement) {
	var row = itemElement;
	while (row != null && row.getParent() != this) {
		row = row.getParent();
	}
	// this should never happen
	if (row == null) {
	  //alert("OrganizationElement.getRowOfItem()\nItem is not part of this organization\nItem:"+itemElement+" title="+itemElement.title+" id="+itemElement.identifier);
		return null;
	}
	return row;
}
/**
 * This is the constructor for a CAM organization element.
 */
function OrganizationElement(identifier,title,children) {
  this.identifier = identifier;
	this.title = title;
	this.isOrganizationElement = true; // this attribute is set to true by OrganizationElement only
  for (var i=0; i < children.length; i++) {
	  this.add(children[i]);
	}
	
	// Support for layered organizations
	this.getColumnOfItem = OrganizationElement_getColumnOfItem;
	this.getRowOfItem = OrganizationElement_getRowOfItem;
}
OrganizationElement.prototype = new CAMElement();

function ItemElement_getResource() {
  if (this.identifierref == null) return null;
	
	if (this.resource == null) {
		// find the root node. The root node must be of type manifest
		var manifest = this.getRoot();
		this.resource = manifest.resources.findByIdentifier(this.identifierref);
	}
	return this.resource;
}
function ItemElement_getHRef() {
  var r = this.getResource();
	return (r == null) ? null : r.href;
}
/**
 * This is the constructor for a CAM organization item element.
 */
function ItemElement(identifier,title,identifierref,parameters,dataFromLMS,children) {
  this.isItemElement = true;
  this.identifier = identifier;
	this.title = title;
	this.identifierref = identifierref;
	this.parameters = parameters;
	this.dataFromLMS = dataFromLMS;
  if (children != null) {
		for (var i=0; i < children.length; i++) {
			this.add(children[i]);
		}  
	}
	
	this.resource = null; // this attribute is used by getResource()
	this.isExpanded = false; // this attribute is used to expand and collapse tree nodes in lmstoc.js
	
	
	this.getResource = ItemElement_getResource;
	this.getHRef = ItemElement_getHRef;
}
ItemElement.prototype = new CAMElement();
 
/**
 * This is the constructor for a CAM resources node.
 */
function ResourcesElement(children) {
  if (children != null) {
		for (var i=0; i < children.length; i++) {
			this.add(children[i]);
		}  
	}
}
ResourcesElement.prototype = new CAMElement();

/**
 * This is the constructor for a CAM resource element.
 */
function ResourceElement(identifier,href) {
  this.identifier = identifier;
	this.href = href;
	this.isResourceElement = true;
	
	// Please note: If you add or remove entries here, you have to
	// change lmsapi.js as well.
	this.cmi_core_lesson_location = ""; // "cmi.core.lesson_location" is used by lmsapi.js
	this.cmi_core_credit = "credit"; // "cmi.core.credit" is used by lmsapi.js
	this.cmi_core_lesson_status = "not attempted"; // "cmi.core.lesson_status" is used by lmsapi.js
	this.cmi_core_entry = ""; // "cmi.core.entry" is used by lmsapi.js
	this.cmi_core_sraw = ""; // "cmi.core.score.raw" is used by lmsapi.js
	this.cmi_core_total_time = "00:00.00"; // "cmi.core.total_time" is used by lmsapi.js
	this.cmi_core_exit = ""; // "cmi.core.exit" is used by lmsapi.js
	this.cmi_core_session_time = ""; // "cmi.core.session_time" is used by lmsapi.js
	this.cmi_suspend_data = ""; // "cmi.suspend_data" is used by lmsapi.js
	this.isCached = false; //Used by the server based WebService
}
ResourceElement.prototype = new CAMElement();

var LMSCAM = new ManifestElement("identifier","1.0",null,null);


/**
 * Verifies a password.
 */
function User_isPasswordValid(password) {
	if (this.passwordDigest == null) return true;
	if (password == null) return false;
	
	var pos = this.passwordDigest.indexOf('.');
	var digest = hex_sha1(this.passwordDigest.substring(0, pos)+password);
	return digest == this.passwordDigest.substring(pos + 1);
}
/**
 * This is the constructor for a user object.
 */
function User(identifier,passwordDigest,name) {
  this.identifier = identifier;
	this.passwordDigest = passwordDigest;
	this.name = name;

	this.isPasswordValid = User_isPasswordValid;
}
