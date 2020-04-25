/**
* These functions handle the communication between the TinyLMS API adapter and 
* the DNN LMSLite interface. This code is derived from the TinyLMS lmsapi.js file
* created by Kent Ogletree Kent@scormkit.com
*
*/


/**
* Loads the database of the LMS from persistent storage.
* The data is loaded into the resource objects of the this.cam data model.
*
*/
function LMSAPI_loadDatabaseFromLMSlite() {
    logger.write(logger.INTERNAL, "lms.LoadDatabaseFromLMSLite");
    this.resetDatabase();

    var cookie = this.WS.LoadDatabasefromWS(API.UserID, API.Reference);
    //alert(temp)
    //var cookie_name ="TinyLMS."+this.cam.identifier+"."+this.getValue("cmi.core.student_id");
    //var cookie = getCookie(cookie_name);

    var logmsg = "lms.LoadDatabaseFromLWS()\n<br>&nbsp; EventID=" + this.EventID + "\n<br>&nbsp; lesson_status=" + cookie;
    var map = new Map();
    if (cookie != null) map.importFromStringDelim(cookie, ".");

    var node = this.cam.resources.getFirstLeaf();
    while (node != null && node != this.cam.resources) {
        if (node.isResourceElement) {
            switch (map.get(API.Reference)) {
                case "p": node.cmi_core_lesson_status = "passed"; break;
                case "c": node.cmi_core_lesson_status = "completed"; break;
                case "f": node.cmi_core_lesson_status = "failed"; break;
                case "i": node.cmi_core_lesson_status = "incomplete"; break;
                case "b": node.cmi_core_lesson_status = "browsed"; break;
                case "n":
                default: node.cmi_core_lesson_status = "not attempted"; break;
            }
        }
        node = node.getNextNode();
    }

    logger.write(logger.INTERNAL_SUCCESS, logmsg);
}



function LMSAPI_readCMIdataModelLMSLite(itemElement) {
    // We read the CMI data model from the CAM resource object.
    var resource = itemElement.getResource();
    resource.identifier = API.Reference;
    var itemCount = 0;
    if (resource != null) {


        if (resource.isCached == false) {  //The data is not cached so we need to get from server

            var SCOData = this.WS.GetSCOData(API.UserID, resource.identifier);


            if (SCOData["Credit"] == null) { //error occured or no data, so default
                this.setValueToDefault("cmi.core.lesson_location");
                this.setValueToDefault("cmi.core.credit");
                this.setValueToDefault("cmi.core.score.raw");
                this.setValueToDefault("cmi.core.total_time");
                this.setValueToDefault("cmi.core.exit");
                this.setValueToDefault("cmi.core.session_time");
                this.setValueToDefault("cmi.suspend_data");
                this.setValueToDefault("cmi.core.lesson_status");
                this.setValueToDefault("cmi.core.entry", "ab-initio");
                this.setValue("cmi.launch_data", itemElement.dataFromLMS);
            } else { //We have Data

                if (SCOData["LessonLocation"] == null) {
                    SCOData["LessonLocation"] = "";
                }

                this.setValue("cmi.core.lesson_status", SCOData["LessonStatus"]);
                this.setValue("cmi.core.exit", SCOData["SCOExit"]);
                this.setValue("cmi.core.entry", SCOData["SCOEntry"]);
                this.setValue("cmi.core.score.raw", SCOData["RawScore"]);
                this.setValue("cmi.suspend_data", SCOData["SuspendData"]);
                this.setValue("cmi.core.total_time", SCOData["TotalTime"]);
                this.setValue("cmi.core.lesson_location", SCOData["LessonLocation"]);
                this.setValue("cmi.core.credit", SCOData["Credit"]);
                this.setValue("cmi.launch_data", SCOData["LaunchData"]);

            }
            //Update the Cache
            resource.cmi_core_lesson_location = this.getValue("cmi.core.lesson_location");
            resource.cmi_core_credit = this.getValue("cmi.core.credit");
            resource.cmi_core_lesson_status = this.getValue("cmi.core.lesson_status");
            resource.cmi_core_entry = this.getValue("cmi.core.entry");
            resource.cmi_core_sraw = this.getValue("cmi.core.score.raw");
            resource.cmi_core_total_time = this.getValue("cmi.core.total_time");
            resource.cmi_core_exit = this.getValue("cmi.core.exit");
            resource.cmi_core_session_time = this.getValue("cmi.core.session_time");
            resource.cmi_suspend_data = this.getValue("cmi.suspend_data");
            resource.isChached = true;
        }

    } else {  //Data is Chached, so get it from the Resource Element
        this.setValue("cmi.core.lesson_location", resource.cmi_core_lesson_location);
        this.setValue("cmi.core.credit", resource.cmi_core_credit);
        this.setValue("cmi.core.score.raw", resource.cmi_core_sraw);
        this.setValue("cmi.core.total_time", resource.cmi_core_total_time);
        this.setValue("cmi.core.exit", resource.cmi_core_exit);
        this.setValue("cmi.core.session_time", resource.cmi_core_session_time);
        this.setValue("cmi.suspend_data", resource.cmi_suspend_data);
    }

}




function LMSAPI_saveDatabaseToLMSlite() {
    logger.write(logger.INTERNAL, "lms.saveDatabaseToLMSlite");
    var map = new Map();

    var node = this.cam.resources.getFirstLeaf();
    while (node != null && node != this.cam.resources) {
        if (node.isResourceElement) {
            switch (node.cmi_core_lesson_status) {
                case "passed": map.put(node.identifier, "p"); break;
                case "completed": map.put(node.identifier, "c"); break;
                case "failed": map.put(node.identifier, "f"); break;
                case "incomplete": map.put(node.identifier, "i"); break;
                case "browsed": map.put(node.identifier, "b"); break;
                case "not attempted":
                default: break;
            }
        }
        node = node.getNextNode();
    }

    var cookie_value = map.exportToStringDelim(".");

    //this.WS.SaveDatabaseToWS(API.UserID, cookie_value)
    //setCookie(cookie_name, cookie_value, expires);
    var logmsg = "lms.saveDatabaseToWS\n<br>&nbsp; EventID" + this.EventID + "\n<br>&nbsp; Lesson_Status=" + cookie_value;

    logger.write(logger.INTERNAL_SUCCESS, logmsg);
}



/**
 * Writes the CMI data model into the specified CAM resource.
 * 
 * @param itemElement A CAM item from the this.cam data model.
 */
function LMSAPI_writeCMIdataModelLMSLite(itemElement) {
    if (itemElement.getResource() != null) {
        var resource = itemElement.getResource();
        resource.identifier = API.Reference;
        //Update the Resource Cache
        resource.cmi_core_lesson_location = this.getValue("cmi.core.lesson_location");
        resource.cmi_core_credit = this.getValue("cmi.core.credit");
        resource.cmi_core_lesson_status = this.getValue("cmi.core.lesson_status");
        resource.cmi_core_entry = this.getValue("cmi.core.entry");
        resource.cmi_core_sraw = this.getValue("cmi.core.score.raw");
        resource.cmi_core_total_time = this.getValue("cmi.core.total_time");
        resource.cmi_core_exit = this.getValue("cmi.core.exit");
        resource.cmi_core_session_time = this.getValue("cmi.core.session_time");
        resource.cmi_suspend_data = this.getValue("cmi.suspend_data");
        //Update the server data
        var paramStr = ""
        paramStr = "LessonStatus=" + this.getValue("cmi.core.lesson_status");
        paramStr = paramStr + "&LessonLocation=" + this.getValue("cmi.core.lesson_location");
        paramStr = paramStr + "&SCOExit=" + this.getValue("cmi.core.exit");
        paramStr = paramStr + "&SCOEntry=" + this.getValue("cmi.core.entry");
        paramStr = paramStr + "&RawScore=" + this.getValue("cmi.core.score.raw");
        paramStr = paramStr + "&SuspendData=" + this.getValue("cmi.suspend_data");
        paramStr = paramStr + "&TotalTime=" + this.getValue("cmi.core.total_time");
        paramStr = paramStr + "&SessionTime=" + this.getValue("cmi.core.session_time");

        if (API.Userid == "") {
            alert("Sessione Scaduta")
        }

        API.WS.SaveSCOData(API.UserID, resource.identifier, paramStr);
    }
}


/**
 * Reads the CMI data model from the specified CAM resource.
 * 
 * @param itemElement A CAM item from the this.cam data model.
 */



function WebService() {
    var Method = "";


    function getParam(param) {
        var i = 0;
        var res = param.split("&");
        var pl = new SOAPClientParameters();
        for (i = 0; i < res.length ; i++) {
        var key=res[i].split("=")[0];
        var value=res[i].split("=")[1];
            pl.add(key,value);
        }
        return pl;
    }


    /* This method is call upon when data is requested from the SCO */
    function O_GetData(param) {
      
        var pl = getParam(param);

        switch (Method) {

            case "LoadDatabaseFromWS":
               return  SOAPClient.invoke(API.WS.URL, Method, pl,false, LoadDatabasefromWS_callBack);
                break;

            case "LMSFinish":

                return SOAPClient.invoke(API.WS.URL, Method, pl, false, Finish_callBack);
             
                break;

            case "LMSCommit":
                return SOAPClient.invoke(API.WS.URL, Method, pl, false, LMSCommit_callBack);
                break;
                 case "LMSGetLastError":
                     return SOAPClient.invoke(API.WS.URL, Method, pl, false, LMSGetLastError_callBack);
                break;
                 case "LMSGetErrorString":
                     return SOAPClient.invoke(API.WS.URL, Method, pl, false, LMSGetErrorString_callBack);
                break;
                 case "LMSGetDiagnostic":
                     return SOAPClient.invoke(API.WS.URL, Method, pl, false, LMSGetDiagnostic_callBack);
                break;
                     
             
                
            default:
            
        }


        
    }


  
    function LMSCommit_callBack(r, soapResponse) {  }
    function LMSGetLastError_callBack(r, soapResponse) { }
    function LMSGetErrorString_callBack(r, soapResponse) { }
    function LMSGetDiagnostic_callBack(r, soapResponse) {  }
    function SaveSCODataToWS_callBack(r) { }
    function O_Finish_callBack(r) {  window.close(); }
    function O_SetData_callBack(r) { }
    function callback_GetScoData(soapResponse, r) { }
   function LoadDatabasefromWS_callBack(r, soapResponse) { }


    /* This method is call upon when data is sent from the SCO */
    function O_SetData(param, value) {
        pl = getParam(param);
        pl.add("Statereference", value);
        SOAPClient.invoke(API.WS.URL, Method, pl, true,O_SetData_callBack);
                  
    }

    
    function O_GetSCOData(UserId, Identifier) {
              
               var _command = "UserId=" + UserId + "&Identifier=" + Identifier;
               var pl = getParam(_command );
                          

               return SOAPClient.invoke(API.WS.URL, "GetSCODataFromWS", pl, false, callback_GetScoData);

              
                   }

    function O_SaveSCOData(UserID,Reference, param) {

      
        var _command = "UserID=" + UserID +"&Reference=" + Reference+ "&" + param;
        var pl = getParam(_command);

        SOAPClient.invoke(API.WS.URL, "SaveSCODataToWS", pl, true, SaveSCODataToWS_callBack);
        
        
    }

    function O_Finish(UserID, Reference,IDEnter,CourseID, param) {
        var _command = "UserID=" + UserID + "&Reference=" + Reference + "&IDEnter=" + IDEnter + "&CourseID=" + CourseID + "&" + param;
        var pl = getParam(_command);
        SOAPClient.invoke(API.WS.URL, "LMSFinish", pl, false, O_Finish_callBack);

    }


    function O_Initialize(param) {
        Method = "LMSInitialize";
        return O_GetData(param);
    }

    function O_GetValue(param) {
        Method = "LMSGetValue";
        return O_GetData(param);
    }

    function O_SetValue(param, value) {
        Method = "LMSSetValue";
        return O_SetData(param, value);
    }
    
    

    
    function O_Commit(param) {
        Method = "LMSCommit";
        return O_GetData(param);
    }

    function O_GetLastError(param) {
        Method = "LMSGetLastError";
        return O_GetData(param);
    }

    function O_GetErrorString(param) {
        Method = "LMSGetErrorString";
        return O_GetData(param);
    }

    function O_GetDiagnostic(param) {
        Method = "LMSGetDiagnostic";
        return O_GetData(param);
    }
    function O_LoadDataBaseFromWS(Userid,Identifier) {
        Method = "LoadDatabaseFromWS"
        var _command = "UserID=" + Userid + "&Identifier=" + Identifier;
        return O_GetData(_command)
    }
    function O_SaveDatabaseToWS(evt, value) {
        Method = "SaveDatabaseToWS"
        return O_SetData("UserID=" + evt, value)
    }

    //Public methods
    this.Initialize = O_Initialize;
    this.GetValue = O_GetValue;
    this.SetValue = O_SetValue;
       this.Commit = O_Commit;
    this.GetLastError = O_GetLastError;
    this.GetErrorString = O_GetErrorString;
    this.GetDiagnostic = O_GetDiagnostic;
    this.LoadDatabasefromWS = O_LoadDataBaseFromWS;
    //this.SaveDatabaseToWS = O_SaveDatabaseToWS;
    this.GetSCOData = O_GetSCOData;
    this.SaveSCOData = O_SaveSCOData;
    this.Finish = O_Finish;
    //Public Variables
    //this.URL = "lmslitepost.asmx/";
}



