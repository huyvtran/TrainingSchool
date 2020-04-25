/* global $,window */


function Savepoll_callBack(){
    
}

function getParam(param) {
    var i = 0;
    var res = param.split("&");
    var pl = new SOAPClientParameters();
    for (i = 0; i < res.length ; i++) {
        var key = res[i].split("=")[0];
        var value = res[i].split("=")[1];
        pl.add(key, value);
    }
    return pl;
}

var pollMaster = (function () {
    var name;
    var data;
    var loaded = false;
    var displayDom;
    var successCbAlias;

    function nextHandler(e) {
        e.preventDefault();

        var status = getUserStatus();

        //if we aren't on the intro, then we need to ensure you picked something
        if (status.question >= 0) {
            var checked = $("input[type=radio]:checked", displayDom);
            if (checked.length === 0) {
                //for now, an ugly alert
                window.alert("Attenzione! Rispondere alla domanda!");
                return;
            } else {
                status.answers[status.question] = checked.val();
            }
        }
        status.question++;
        storeUserStatus(status);
        displaypoll(successCbAlias);
    }


    function prevHandler(e) {
        e.preventDefault();

        var status = getUserStatus();

        //if we aren't on the intro, then we need to ensure you picked something
        if (status.question >= 0) {
          
            } else {
                status.answers[status.question] = checked.val();
            }
        
        status.question--;
        storeUserStatus(status);
        displaypoll(successCbAlias);
    }


    function displaypoll(successCb) {

        //We copy this out so our event can use it later. This feels wrong
        successCbAlias = successCb;
        var current = getpoll();
        var html;

        if (current.state === "introduction") {
            html = "<h2>" + current.introduction + "</h2>" + nextButton();
            displayDom.html(html).trigger('create');
        } else if (current.state === "inprogress") {

            var _question = current.question.question.split("|")[2]
            
            html = "<h2>" +_question + "</h2><form><div data-role='fieldcontain'><fieldset data-role='controlgroup'>";
            for (var i = 0; i < current.question.answers.length; i++) {
                var _answer;
                _answer = current.question.answers[i].split("|")[1];
                html += "<input type='radio' name='pollMasterAnswer' id='pollMasterAnswer_" + i + "' value='" + i + "'/><label for='pollMasterAnswer_" + i + "'><img height='20' src='assets/images/emoji/" + _answer + "'></label>";
            }
            html += "</fieldset></div></form><div data-role='controlgroup' data-type='horizontal' data-mini='true'>" + prevButton() + nextButton() + "</div>";
            displayDom.html(html).trigger('create');
        } else if (current.state === "complete") {
            html = "La ringraziamo per il suo contributo!";

              


          
           
            //var paramStr = ""

            //paramStr = "total=" + total;
            //paramStr = paramStr + "&correct=" + current.correct;
            //paramStr = paramStr + "&Reference=" + current.correct;
            //paramStr = paramStr + "&total=" + current.correct;
            //paramStr = paramStr + "&answers=" + answers;

            //var pl = getParam(paramStr);


            //SOAPClient.invoke(temppath.substring(0, pathend) + "LMSPostBack.asmx", "Savepoll", pl, false, Savepoll_callBack);



            displayDom.html(html).trigger('create');
            removeUserStatus();
            successCb(current);
        }


        //Remove previous if there...
        //Note - used click since folks will be demoing in the browser, use touchend instead
        displayDom.off("click", ".pollMasterNext", nextHandler);
        //Then restore it
        displayDom.on("click", ".pollMasterNext", nextHandler);

        displayDom.off("click", ".pollMasterPrev", prevHandler);
        //Then restore it
        displayDom.on("click", ".pollMasterPrev", prevHandler);
    }

    function getKey() {
        return "pollMaster_" + name;
    }

    function getQuestion(x) {
        return data.questions[x];
    }

    function getpoll() {
        //Were we taking the poll already?
        var status = getUserStatus();
        if (!status) {
            status = { question: -1, answers: [] };
            storeUserStatus(status);
        }
        //If a poll doesn't have an intro, just go right to the question
        if (status.question === -1 && !data.introduction) {
            status.question = 0;
            storeUserStatus(status);
        }

        var result = {
            currentQuestionNumber: status.question,
            idlog: data.idlog,
            idpoll: data.idpoll
        };

        if (status.question == -1) {
            result.state = "introduction";
            result.introduction = data.introduction;
        } else if (status.question < data.questions.length) {
            result.state = "inprogress";
            result.question = getQuestion(status.question);
          
        } else {

          
            result.state = "complete";
            result.correct = 0;


            for (var i = 0; i < data.questions.length; i++) {
                

                 var paramStr = ""

                 paramStr = "idlog=" + data.idlog;
                 paramStr = paramStr + "&idquest=" + data.questions[i].question.split("|")[1];  
                 paramStr = paramStr + "&idanswer=" + data.questions[i].answers[status.answers[i]].split("|")[0];
                
               
                 var pl = getParam(paramStr);

                 var temppath = unescape(location.href)
                 var pathend = temppath.lastIndexOf("/") + 1
                 SOAPClient.invoke(temppath.substring(0, pathend) + "LMSContent/LMSPostBack.asmx", "SavePollquestanswer", pl, false, Savepoll_callBack);


            }
        }
        return result;
    }

    function getUserStatus() {
        var existing = window.sessionStorage.getItem(getKey());
        if (existing) {
            return JSON.parse(existing);
        } else {
            return null;
        }
    }

    function nextButton() {
        return "<a href='' class='pollMasterNext'  data-theme='b' data-role='button'>Prossima</a>";
    }


    function prevButton() {
        return "<a href='' class='pollMasterPrev'  data-theme='b' data-role='button'>Precedente</a>";
    }

    function removeUserStatus(s) {
        window.sessionStorage.removeItem(getKey());
    }

    function storeUserStatus(s) {
        window.sessionStorage.setItem(getKey(), JSON.stringify(s));
    }

    return {
        execute: function (url, dom, cb) {
            //We cache the ajax load so we can do it only once 

            $.ajax({
                type: 'GET',
                url: url,
                cache:false,
                dataType: 'json',
                success: function (res) {
                    name = url;
                    data = res;
                    displayDom = $(dom);
                    console.dir(res);
                    loaded = true;
                    displaypoll(cb);
                }
            });


            
        }
    };
} ());