/* global $,window */


function SaveTest_callBack(){
    
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

var quizMaster = (function () {
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
                window.alert("Attenzione rispondere alla domanda!");
                return;
            } else {
                status.answers[status.question] = checked.val();
            }
        }
        status.question++;
        storeUserStatus(status);
        displayQuiz(successCbAlias);
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
        displayQuiz(successCbAlias);
    }


    function displayQuiz(successCb) {

        //We copy this out so our event can use it later. This feels wrong
        successCbAlias = successCb;
        var current = getQuiz();
        var html;

        if (current.state === "introduction") {
            html = "<h2>" + current.introduction + "</h2>" + nextButton();
            displayDom.html(html).trigger('create');
        } else if (current.state === "inprogress") {

            var _question = current.question.question.split("|")[1];
            var _numberquestion = current.question.question.split("|")[0].split(".")[0];
            html = "<h3>" + _numberquestion  + '/' + data.questions.length +' ' +  _question + "</h3><form><div data-role='fieldcontain'><fieldset data-role='controlgroup'>";
            for (var i = 0; i < current.question.answers.length; i++) {
                var _answer;
                _answer = current.question.answers[i].split("|")[1]
                html += "<input type='radio' name='quizMasterAnswer' id='quizMasterAnswer_" + i + "' value='" + i + "'/><label for='quizMasterAnswer_" + i + "'>" + _answer + "</label>";
            }
            html += "</fieldset></div></form><div data-role='controlgroup' data-type='horizontal' data-mini='true'>" + prevButton() + nextButton() + "</div>";
            displayDom.html(html).trigger('create');
        } else if (current.state === "complete") {
           
            var total = data.questions.length * 10;
            var point_required = Math.round((total * data.soglia) / 100);
            var correct = current.correct * 10;
           
           

            html = "<div style='text-align:center;background-color:white'> Tempo impiegato: " +  $('.timer2').text() ;
           if (point_required <= correct) {
              
               html += "<h2 style='color:green'><i class='icon-ok green bigger-200'></i>Hai Superato il Test!</h2><p> Hai totalizzato " + correct + " su " + total + ".</p> ";
                 } else {
          
               html += "<h2 style='color:red'><i class='icon-ok red bigger-200'></i>Non Hai Superato il Test!</h2><p> Hai totalizzato " + correct + " su " + total + ".</p> <br><br>il punteggio minimo da superare e\': " + point_required;
               
               html += "<br><br><a href='#' onclick='location.reload()' class='btn btn-success' >Riprova</a></div>";
            }
           $('.timer2').hide();
      

            //var paramStr = ""

            //paramStr = "total=" + total;
            //paramStr = paramStr + "&correct=" + current.correct;
            //paramStr = paramStr + "&Reference=" + current.correct;
            //paramStr = paramStr + "&total=" + current.correct;
            //paramStr = paramStr + "&answers=" + answers;

            //var pl = getParam(paramStr);


            //SOAPClient.invoke(temppath.substring(0, pathend) + "LMSPostBack.asmx", "SaveTest", pl, false, SaveTest_callBack);



            displayDom.html(html).trigger('create');
            removeUserStatus();
            successCb(current);
        }


        //Remove previous if there...
        //Note - used click since folks will be demoing in the browser, use touchend instead
        displayDom.off("click", ".quizMasterNext", nextHandler);
        //Then restore it
        displayDom.on("click", ".quizMasterNext", nextHandler);

        displayDom.off("click", ".quizMasterPrev", prevHandler);
        //Then restore it
        displayDom.on("click", ".quizMasterPrev", prevHandler);
    }

    function getKey() {
        return "quizMaster_" + name;
    }

    function getQuestion(x) {
        return data.questions[x];
    }

    function getQuiz() {
        //Were we taking the quiz already?
        var status = getUserStatus();
        if (!status) {
            status = { question: -1, answers: [] };
            storeUserStatus(status);
        }
        //If a quiz doesn't have an intro, just go right to the question
        if (status.question === -1 && !data.introduction) {
            status.question = 0;
            storeUserStatus(status);
        }

        var result = {
            currentQuestionNumber: status.question,
            soglia: data.soglia,
            idlog: data.idlog,
            idtest:data.idtest,
			 num:data.questions.length
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
                

               
                var idquest = data.questions[i].question.split("|")[0];
                idquest = idquest.split(".")[1];

              
                var idanswers = data.questions[i].answers;

                if (data.questions[i].correct == status.answers[i]) {
                    result.correct= result.correct +1;
                }


                for (var j = 0; j <= idanswers.length - 1; j++) {
                    var paramStr = ""
                    paramStr = "idlog=" + data.idlog;
                    paramStr = paramStr + "&idtest" + data.idtest;
                    paramStr = paramStr + "&idquest=" + idquest;
                    paramStr = paramStr + "&idanswer=" + idanswers[j].split("|")[0];

                    
                    if(j==status.answers[i]){
                    paramStr = paramStr + "&score=1";
                }else{
                    paramStr = paramStr + "&score=0";
                    
                }
                  
                    
                     var pl = getParam(paramStr);

                     var temppath = unescape(location.href)
                     var pathend = temppath.lastIndexOf("/") + 1
                     SOAPClient.invoke(temppath.substring(0, pathend) + "LMSContent/LMSPostBack.asmx", "SaveQuestAnswer", pl, false, SaveTest_callBack);
                 }


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
        return "<a href='' class='quizMasterNext'  data-theme='b' data-role='button'>Prossima</a>";
    }


    function prevButton() {
        return "";
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
                    //console.dir(res);
                    loaded = true;
                    displayQuiz(cb);
                }
            });


            
        }
    };
} ());