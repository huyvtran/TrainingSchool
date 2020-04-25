/*
 * @(#)fonts.js 0.1  2003-03-26
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
 *
 * 
 * The following conditions apply only, if this software is distributed 
 * as part of TinyLMS:
 *
 *      This program is free software; you can redistribute it and/or modify it 
 *      under the terms of the GNU General Public License as published by the 
 *      Free Software  Foundation; either version 2 of the License, or (at your
 *      option) any later version. 
 *
 *      This program is distributed in the hope that it will be useful, but 
 *      WITHOUT ANY WARRANTY; without even the implied warranty of 
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General
 *      Public License for more details. You should have received a copy of the
 *      GNU General Public License along with this program; if not, write to the
 *      Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
 *      02111-1307 USA
 */

/**
 * This file contains utility functions related to fonts.
 */
var debug = false;

function verdanaWidth(aChar) {
  switch (aChar) {
     case 'c':
     case 's':
     case 'z':
       return 10 / 12;
     case 'j':
       return 10 / 18;
     case 'm':
       return 10 / 6;
     case ' ':
     case 'w':
       return 10 / 7.25;
     case 'W':
       return 10 / 6;
     case 'M':
       return 10 / 7;
     case 'A':
     case 'B':
     case 'C':
     case 'K':
     case 'R':
     case 'S':
     case 'V':
     case 'X':
       return 10 / 9;
     case 'D':
     case 'G':
     case 'H':
     case 'N':
     case 'O':
     case 'Q':
     case 'U':
        return 10 / 8;
     case 'F':
     case 'L':
     case 'T':
       return 10 / 11;
     case 'J':
     case 'r':
       return 10 / 13;
     case 'I':
     case 't':
     case 'f':
       return 10 / 16;
     case 'l':
     case 'i':
       return 10 / 24;
     default:
       return 1;
  }
}


/**
 * Crops the string to the specified maximal length.
 * The length is actually a character width unit. The character width
 * is determined by function verdanaWidth.
 * This function deals properly with HTML meta characters (i.e. ü).
 * @param s The String to be cropped.
 * @param maxLength the length in character width units. 
 */
function cropString(s, maxLength) {
  //if (s.length < maxLength) return s;
  
  var cropped = "";
  var count = 0;
  var i;
  for (i = 0; i < s.length && count < maxLength - 3; i++) {
    if (s.charAt(i) == '&') {
      for (; i < s.length && s.charAt(i) != ';'; i++) {
	    cropped += s.charAt(i);
	  }
	  if (i < s.length) cropped += s.charAt(i);
  	} else {
      cropped += s.charAt(i);
	  }
	  count += verdanaWidth(s.charAt(i));
  }
  if (s.length - i < 3) cropped += s.substring(i); 
  else cropped += '...';
  return cropped;
}
