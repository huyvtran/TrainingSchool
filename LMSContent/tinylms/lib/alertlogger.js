/*
 * @(#)alertlogger.js 1.0  2004-06-13
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
 */ 
 
/**
 * This file provides logging functionality.
 *
 * @author Werner Randelshofer
 * @version 1.0 2004-06-13 Created.
 */



/**
 * Opens the logger.
 */
function logger_open() {
	// nothing to do
}
/**
 * Closes the logger.
 */
function logger_close() {
	// nothing to do
}

/**
 * Logs a message.
 * 
 * @param type   Type of the message.
 * @param message String The message.
 */
function logger_write(type, message) {
	if (type <= this.level && type > this.INFO) {
		alert(message);
	}
}

/**
 * This is the constructor for a Logger object.
 */
function Logger() {
	// the title of the logger
	// Note: This is not used by the alertlogger, but the scriptloggger.js does.
	this.title = "Log";
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
}
/**
 * Create the debugLog object
 */
var logger = new Logger();