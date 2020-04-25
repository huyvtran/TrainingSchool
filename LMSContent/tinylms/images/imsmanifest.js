/*
 * @(#)imsmanifest.js 1.9 2010-02-10
 *
 * Copyright (c) 2003-2009 Werner Randelshofer
 * Hausmatt 10, Immensee, CH-6405, Switzerland
 * All rights reserved.
 *
 * This software is the confidential and proprietary information of
 * Werner Randelshofer. ("Confidential Information").  You shall not
 * disclose such Confidential Information and shall use it only in
 * accordance with the terms of the license agreement you entered into
 * with Werner Randelshofer.
 */
/**
 * This file represents the Java Script version of a Content Aggregation Model (CAM).
 *
 * Reference:
 * ADL (2001a). Advanced Distributed Learning.
 * Sharable Content Object Reference Model (SCORM) Version 1.2.
 * Internet (2003-01-20): http://www.adlnet.org
 *
 * ADL (2001b). Advanced Distributed Learning.
 * SCORM 1.2 Runtime Environment.
 * Internet (2003-01-20): http://www.adlnet.org
 *
 * IMPORTANT! This file MUST be plain ASCII.
 * All non-ASCII characters MUST be encoded using HEXADECIMAL HTML entities.
 * Other encodings are not supported and will lead into incorrect display of the
 * labels.
 *
 * @author Werner Randelshofer, Hausmatt 10, Immensee, CH-6405, Switzerland
 * @version 1.9 2010-02-10
 */

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
    new ResourceElement("2","index.html")
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
