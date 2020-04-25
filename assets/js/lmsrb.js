var winPop = false;

function convertDateTime(dateTime) {

    dateTime = dateTime.replace(".", ":");
    var date = dateTime.split(" ");
    var d = date[0].split("/");
    var yyyy = d[2];
    var mm = d[1] - 1;
    var dd = d[0];

    var time = date[1].split(":");
    var h = time[0];
    var m = time[1];
    var s = 00;


    return new Date(yyyy, mm, dd, h, m);


}

function ShowAlert(data){
    if (data != '') {
        message = data;
    } else {
        message = 'Operazione completata';
    }
    bootbox.alert({
        message: message,
        className: 'rubberBand animated'
    });

}

function get_estensione(path) {
    posizione_punto = path.lastIndexOf(".");
    lunghezza_stringa = path.length;
    estensione = path.substring(posizione_punto + 1, lunghezza_stringa);
    return estensione;
}

function controlla_estensione(path) {
    if (get_estensione(path) != "jpg" && get_estensione(path) != "tif" && get_estensione(path) != "tiff" && get_estensione(path) != "jpeg" && get_estensione(path) != "png" && get_estensione(path) != "pdf") {
        alert("Il file deve avere una di queste estensioni: jpg/png/tiff/tif/pdf/jpeg");
    }
}

function resizeJqGridWidth(grid_id, div_id, width) {
    $('#' + grid_id).setGridWidth(width, true); //Back to original width
    $(window).bind('resize', function () {
        $('#' + grid_id).setGridWidth(width, true); //Back to original width
        $('#' + grid_id).setGridWidth($('#' + div_id).width() - 100, true);
        
    }).trigger('resize');
}
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function reloadTable(grid_selector) {
    $(grid_selector).trigger( 'reloadGrid' );
    
}

var DataSourceTree = function (options) {
    this._data = options.data;
    this._delay = 400;
}

DataSourceTree.prototype.data = function (options, callback) {
    var self = this;
    var $data = null;

    if (!("name" in options) && !("type" in options)) {
        $data = this._data; //the root tree
        callback({ data: $data });
        return;
    }
    else if ("type" in options && options.type == "folder") {
        if ("additionalParameters" in options && "children" in options.additionalParameters)
            $data = options.additionalParameters.children;
        else $data = {}//no data
    }

    if ($data != null)//this setTimeout is only for mimicking some random delay
        setTimeout(function () { callback({ data: $data }); }, parseInt(Math.random() * 500) + 200);

    //we have used static data here
    //but you can retrieve your data dynamically from a server using ajax call
    //checkout examples/treeview.html and examples/treeview.js for more info
};

function abortTask() {
    var obj = Sys.WebForms.PageRequestManager.getInstance();
    if (obj.get_isInAsyncPostBack())
        obj.abortPostBack();
}

function abortTask() {
    var obj = Sys.WebForms.PageRequestManager.getInstance();
    if (obj.get_isInAsyncPostBack())
        obj.abortPostBack();
}


var start_count_from = ".$start_time.";
var step = 1;
var time_elapsed = 0;

var id_interval;
var id_timeout;

//if( window.document.getElementById == null ) {
//    window.document.getElementById = function( id ) {
//        return document.all[id];
//    }
//}

function counter() {

    time_elapsed += step;

    var display = start_count_from - time_elapsed;
    var elem = document.getElementById('time_left');

    if (display < 0) return;

    var value = display / 60;
    var minute = Math.floor(value).toString(10);
    if (minute.length <= 1) minute = '0' + minute;
    value = display % 60;
    var second = Math.floor(value).toString(10);
    if (second.length <= 1) second = '0' + second;
    elem.innerHTML = minute + 'm ' + second + ' s';
}

function whenTimeElapsed() {

    window.clearInterval(id_interval);
    window.clearTimeout(id_timeout);

    var submit_to_end = document.getElementById('test_play');
    var time_elapsed = document.getElementById('time_elapsed');
    time_elapsed.value = 1;

    submit_to_end.submit();
}

function activateCounter() {

    counter();
    id_interval = window.setInterval("counter()", step * 1000);
    id_timeout = window.setTimeout("whenTimeElapsed()", (start_count_from - 1) * 1000);
}
var selectTreeFolder = function ($treeEl, folder, $parentEl) {
    var $parentEl = $parentEl || $treeEl;
    if (folder.type == "folder") {
        var $folderEl = $parentEl.find("div.tree-folder-name").filter(function (_, treeFolder) {
            return $(treeFolder).text() == folder.name;
        }).parent();
        $treeEl.one("loaded", function () {
            $.each(folder.children, function (i, item) {
                selectTreeFolder($treeEl, item, $folderEl.parent());
            });
        });
        $treeEl.tree("selectFolder", $folderEl);
    }
    else {
        selectTreeItem($treeEl, folder, $parentEl);
    }
};

var selectTreeItem = function ($treeEl, item, $parentEl) {
    var $parentEl = $parentEl || $treeEl;
    if (item.type == "item") {
        var $itemEl = $parentEl.find("div.tree-item-name").filter(function (_, treeItem) {
            return $(treeItem).text() == item.name && !$(treeItem).parent().is(".tree-selected");
        }).parent();
        $treeEl.tree("selectItem", $itemEl);
    }
    else if (item.type == "folder") {
        selectTreeFolder($treeEl, item, $parentEl);
    }
};




function comunilms(regione, provincia, comune,v) {

    $.ajax({
        type: "POST",
        url: "comunilms.aspx?view=" + v,
        data: { regione: regione, comune: comune, provincia: provincia },
        success: function (response) {
            eval(response);


        },
        fail: function (response) {
            eval(response);


        }
    });
}









function filtri(_id_anni_esperienza,_id_orario_lavorativo,_id_contratto,_tipoistruzione,_tipoprofessione,v) {

    $.ajax({
        type: "POST",
        url: "WFIOFiltri.aspx?view="+ v,
        data: { id_anni_esperienza: _id_anni_esperienza, id_orario_lavorativo: _id_orario_lavorativo,id_contratto:_id_contratto, id_titolo_di_studio: _tipoistruzione, id_mansione_lavorativa: _tipoprofessione },
        success: function (response) {
            eval(response);

        },
        fail: function (response) {
            eval(response);


        }
    });
}

function PopupCenter(url, title, w, h) {
    // Fixes dual-screen position                         Most browsers      Firefox
    var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
    var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;

    width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
    height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

    var left = 0; ((width / 2) - (w / 2)) + dualScreenLeft;
    var top = 0; ((height / 2) - (h / 2)) + dualScreenTop;

    var newWindow = window.open(url, title, 'fullscreen = yes,scrollbars=yes, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left);

    // Puts focus on the newWindow
    if (window.focus) {
        newWindow.focus();
    }
}



function ViewModal(pdf_link, title, f) {

    /*
    * This is the plugin
    */
    (function (a) { a.createModal = function (b) { defaults = { title: "", message: "!", closeButton: true, scrollable: false }; var b = a.extend({}, defaults, b); var c = (b.scrollable === true) ? 'style="z-index:99999;max-height="' + $(window).height() - 200 + 'px";overflow-y: auto;"' : ""; html = '<div class="modal fade" id="myModal">'; html += '<div class="modal-dialog">'; html += '<div class="modal-content">'; html += '<div class="modal-header">'; if (b.title.length > 0) { html += '<h4 class="modal-title">' + b.title + "</h4>" } html += "</div>"; html += '<div class="modal-body" ' + c + ">"; html += b.message; html += "</div>"; html += '<div class="modal-footer addfield">'; if (b.closeButton === true) { html += '<button type="button" class="btn btn-primary" data-dismiss="modal">Chiudi</button>' } html += "</div>"; html += "</div>"; html += "</div>"; html += "</div>"; a("body").prepend(html); a("#myModal").modal().on("hidden.bs.modal", function () { a(this).remove() }) } })(jQuery);

    /*
    * Here is how you use it
    */


    params = pdf_link.split('?')[1];
    link = pdf_link.split('?')[0];

    link = link + '?' + params;

    var pdf_link = $(this).attr('href');
    var iframe = '<div class="iframe-container"><iframe frameborder=0 height="600px"  id="myframe" src="' + link + '"></iframe>' //<div id="loadingmessage" >Attendere prego...</div>
    $.createModal({
        title: title,
        modal: true,
        message: iframe,
        closeButton: true,
        scrollable: false
    });
    return false;

}


function ViewModalReport(pdf_link, title, obj) {

    h = (window.innerHeight - 150);


    (function (a) {
        a.createModal = function (b) {

            defaults = { title: "", message: "!", closeButton: true, scrollable: false };
            var b = a.extend({}, defaults, b);
            var c = (b.scrollable === false) ? 'style="z-index:99999;height:800;overflow-x:hidden;overflow-y: auto;"' : "";


            html = '<div class="modal fade" id="myModal1">'; html += '<div class="modal-dialog">'; html += '<div class="modal-content">'; html += '<div class="modal-header">';


            if (b.title.length > 0) {
                html += '<h4 class="modal-title">' + b.title + "</h4>"
            }

            html += "</div>";
            html += '<div class="modal-body" ' + c + ">";
            html += b.message; html += "</div>";
            html += '<div class="modalfooter">';
            if (b.closeButton == true) {
                if (obj == 'scorm') {
                    html += '';
                } else {
                    html += '<button type="button" class="btn btn-primary" data-dismiss="modal">Chiudi</button>';


                }
            }
            html += "</div>";
            html += "</div>";
            html += "</div>";
            html += "</div>";
            a("body").prepend(html);
            a("#myModal1").modal({ backdrop: 'static', keyboard: false }).on("hidden.bs.modal", function () { a(this).remove();  })
        }
    })(jQuery);


    params = pdf_link.split('?')[1];

    link = pdf_link.split('?')[0];

    link = link + '?' + params;

    var pdf_link = $(this).attr('href');

    var iframe = '<div class="iframe-container"><iframe style="min-height:calc(100vh - 200px)"  frameborder=0 width="800px" id="myframe" src="' + link + '"></iframe>'

    $.createModal({
        title: title,
        height: '600px',
        modal: true,
        message: iframe,
        closeButton: true,
        scrollable: false
    });
    return false;

}

function ViewModalObjclient(pdf_link, title, obj) {

    h = (window.innerHeight - 150);


    (function (a) {
        a.createModal = function (b) {

            defaults = { title: "", message: "!", closeButton: true, scrollable: false };
            var b = a.extend({}, defaults, b);
            var c = (b.scrollable === false) ? 'style="z-index:99999;height:800;overflow-x:hidden;overflow-y: auto;"' : "";


            html = '<div class="modal fade" id="myModal1">'; html += '<div class="modal-dialog">'; html += '<div class="modal-content">'; html += '<div class="modal-header">';


            if (b.title.length > 0) {
                html += '<h4 class="modal-title">' + b.title + "</h4>"
            }

            html += "</div>";
            html += '<div class="modal-body" ' + c + ">";
            html += b.message; html += "</div>";
            html += '<div class="modalfooter">';
            if (b.closeButton == true) {
                if (obj == 'scorm') {
                    html += '';
                } else {
                    html += '<button type="button" class="btn btn-primary" data-dismiss="modal">Chiudi</button>';


                }
            }
            html += "</div>";
            html += "</div>";
            html += "</div>";
            html += "</div>";
            a("body").prepend(html);
            a("#myModal1").modal({ backdrop: 'static', keyboard: false }).on("hidden.bs.modal", function () { a(this).remove();location.reload() })
        }
    })(jQuery);


    params = pdf_link.split('?')[1];

    link = pdf_link.split('?')[0];

    link = link + '?' + params;

    var pdf_link = $(this).attr('href');

    var iframe = '<div class="iframe-container"><iframe style="min-height:calc(100vh - 200px);width:100%"  frameborder=0  id="myframe" src="' + link + '"></iframe>'

    $.createModal({
        title: title,
        height: '400px',
        modal: true,
        message: iframe,
        closeButton: true,
        scrollable: false
    });
    return false;

}


function ViewModalObj(pdf_link, title,obj) {


    h = (window.innerHeight - 150);
    /*
    * This is the plugin
    */
    (function (a) { a.createModal = function (b) { defaults = { title: "", message: "!", closeButton: true, scrollable: false }; var b = a.extend({}, defaults, b); var c = (b.scrollable === true) ? 'style="z-index:99999;height="' + $(window).height() + 'px";overflow-x:hidden;overflow-y: auto;"' : ""; html = '<div class="modal fade" id="myModal1">'; html += '<div class="modal-dialog">'; html += '<div class="modal-content">'; html += '<div class="modal-header">'; if (b.title.length > 0) { html += '<h4 class="modal-title">' + b.title + "</h4>" } html += "</div>"; html += '<div class="modal-body" ' + c + ">"; html += b.message; html += "</div>"; html += '<div class="modalfooter">'; if (b.closeButton === true) { html += '<button type="button" class="btn btn-primary" data-dismiss="modal">Chiudi</button>' } html += "</div>"; html += "</div>"; html += "</div>"; html += "</div>"; a("body").prepend(html); a("#myModal1").modal().on("hidden.bs.modal", function () { a(this).remove() }) } })(jQuery);

    /*
    * Here is how you use it
    */


    params = pdf_link.split('?')[1];
    link = pdf_link.split('?')[0];

    link = link + '?' + params;

    var pdf_link = $(this).attr('href');
    var iframe = '<div class="iframe-container"><iframe height=' + h + '"  frameborder=0 width="100%" id="myframe" src="' + link + '"></iframe>' //<div id="loadingmessage" >Attendere prego...</div>

    //$("iframe").width = $("#myModal1").height - 10;

    $.createModal({
        title: title,
        modal: true,
        message: iframe,
        closeButton: true,
        scrollable: false
    });
    return false;

}


//function ViewModalObj(pdf_link, title,obj) {

//    h = (window.innerHeight - 150);
    
   
//    (function (a) {
//        a.createModal = function (b) {

//            defaults = { title: "", message: "!", closeButton: true, scrollable: false };
//            var b = a.extend({}, defaults, b);
//            //var c = (b.scrollable === false) ? 'style="z-index:1050;height:800;overflow-x:hidden;overflow-y: auto;"' : "";

                    
//            html = '<div style="z-index:1080 !important;height:800;overflow-x:hidden;overflow-y: auto;" class="modal fade" id="myModal1">'; html += '<div class="modal-dialog">'; html += '<div class="modal-content">'; html += '<div class="modal-header">';
           

//            if (b.title.length > 0) {
//                html += '<h4 class="modal-title">' + b.title + "</h4>"
//            }

//            html += "</div>";
//            html += '<div class="modal-body" >"';
//            html += b.message;
//            html += "</div>";
//            html += '<div class="modalfooter">';
//            if (b.closeButton == true) {
//                if (obj == 'scorm') {
//                    html += '';
//                } else {
//                    html += '<button type="button" class="btn btn-primary" data-dismiss="modal">Chiudi</button>';

                   
//                }
//            }
//            html += "</div>";
//            html += "</div>";
//            html += "</div>";
//            html += "</div>";
//            a("body").prepend(html);
//            a("#myModal1").modal({ backdrop: 'static', keyboard: false }).on("hidden.bs.modal", function () { a(this).remove();  })
//        }
//    })(jQuery);


//    params = pdf_link.split('?')[1];

//    link = pdf_link.split('?')[0];

//    link = link + '?' + params;

//    var pdf_link = $(this).attr('href');

//    var iframe = '<div class="iframe-container"><iframe style="min-height:calc(100vh - 200px);width:100%"  frameborder=0  id="myframe" src="' + link + '"></iframe>'

//    $.createModal({
//        title: title,
//        height:'400px',
//        modal: true,
//        message: iframe,
//        closeButton: true,
//        scrollable: false
//    });
//    return false;

//}



function ViewModalModifica(pdf_link, title) {


    h = (window.innerHeight - 150);
    /*
    * This is the plugin
    */
    (function (a) { a.createModal = function (b) { defaults = { title: "", message: "!", closeButton: true, scrollable: false }; var b = a.extend({}, defaults, b); var c = (b.scrollable === true) ? 'style="z-index:99999;height="' + $(window).height()  + 'px";overflow-x:hidden;overflow-y: auto;"' : ""; html = '<div class="modal fade" id="myModal1">'; html += '<div class="modal-dialog">'; html += '<div class="modal-content">'; html += '<div class="modal-header">'; if (b.title.length > 0) { html += '<h4 class="modal-title">' + b.title + "</h4>" } html += "</div>"; html += '<div class="modal-body" ' + c + ">"; html += b.message; html += "</div>"; html += '<div class="modalfooter">'; if (b.closeButton === true) { html += '<button type="button" class="btn btn-primary" data-dismiss="modal">Chiudi</button>' } html += "</div>"; html += "</div>"; html += "</div>"; html += "</div>"; a("body").prepend(html); a("#myModal1").modal().on("hidden.bs.modal", function () { a(this).remove() }) } })(jQuery);

    /*
    * Here is how you use it
    */


    params = pdf_link.split('?')[1];
    link = pdf_link.split('?')[0];

    link = link + '?' + params;

    var pdf_link = $(this).attr('href');
    var iframe = '<div class="iframe-container"><iframe height=' + h +'"  frameborder=0 width="800px" id="myframe" src="' + link + '"></iframe>' //<div id="loadingmessage" >Attendere prego...</div>

    //$("iframe").width = $("#myModal1").height - 10;

    $.createModal({
        title: title,
        modal: true,
        message: iframe,
        closeButton: true,
        scrollable: false
    });
    return false;

}


function openpage(url, idcourse, coursename, code) {

    link = url + "&idcourse=" + idcourse + "&name=" + coursename + "&code=" + code;
    setTimeout(function () { document.location.href = link }, 500);

}


function ModalAlert(msg, title, reload) {


    (function (a) { a.createModal = function (b) { defaults = { title: "", message: "!", closeButton: true, scrollable: false }; var b = a.extend({}, defaults, b); var c = (b.scrollable === true) ? 'style="max-height="' + $(window).height() - 200 + 'px";overflow-y: auto;"' : ""; html = '<div class="modal fade" id="myModal">'; html += '<div class="modal-dialog">'; html += '<div class="modal-content">'; html += '<div class="modal-header">'; if (b.title.length > 0) { html += '<h4 class="modal-title">' + b.title + "</h4>" } html += "</div>"; html += '<div class="modal-body" ' + c + ">"; html += b.message; html += "</div>"; html += '<div class="modal-footer">'; if (b.closeButton === true) { html += '<button type="button" class="btn btn-primary" data-dismiss="modal">Chiudi</button>' } html += "</div>"; html += "</div>"; html += "</div>"; html += "</div>"; a("body").prepend(html); a("#myModal").modal().on("hidden.bs.modal", function () { a(this).remove(); if (reload == true) { location.reload(); } }) } })(jQuery);


    $.createModal({
        title: title,
        message: msg,
        closeButton: true,
        scrollable: false,

    });
    return false;

}
function openpage_ads(url, parameter,f) {

    link = url + "?" + parameter;
    //setTimeout(function () { document.location.href = link }, 500);

    ViewModalModifica(link, f);
}

var openObject = function (uri, name, w, h) {
    var win = lmspopup(uri, name, w, h);
    var interval = window.setInterval(function () {
        try {
            if (win == null || win.closed) {
                window.clearInterval(interval);
                location.reload();
            }
        }
        catch (e) {

        }
    }, 1000);
    return win;
};


var percentheight = 90;
var percentwidth = 80;
function openwindowStat(obj, reference, title, iduser) {

    var w = 630, h = 440; // default sizes
    if (window.screen) {
        w = window.screen.availWidth * percentheight / 100;
        h = window.screen.availHeight * percentwidth / 100;
    }

    ViewModalObj("WfStatObj.aspx?obj=" + obj + "&reference=" + reference + "&iduser=" + iduser + "", title);



}

function openwindowreportpdf(iduser, idsessione,nomesessione, fullname) {
    var stile = "width=1200,height=700";
    window.open("Hreport.ashx?op=modreport&oper=getreportcourse&nomesessione=" + nomesessione + "&idsessione=" + idsessione + "&iduser=" + iduser + "&fullname=" + fullname + "");
}
function openwindowreport(iduser, idcourse, fullname) {
    var stile = "width=1200,height=700";
    ViewModalObj("WFChartUser.aspx?idCourse=" + idcourse + "&iduser=" + iduser + "&fullname=" + fullname + "");
}


function openwindow(obj, reference, title) {

    var w = 630, h = 440; // default sizes
    if (window.screen) {
        w = window.screen.availWidth * percentwidth / 100;
        h = window.screen.availHeight * percentheight / 100;
    }


    var stile = "width=" + w + ",height=" + h + ",scrollbars=1,titlebar=yes";


    if (obj == "scormorg") {
        ViewModalObjclient("LMSContent/RunTimePlayer.aspx?reference=" + reference, title, obj)
        // openObject("LMSContent/RunTimePlayer.aspx?reference=" + reference, "PlayerScorm", w,h);

        //  top.location.href = "WfViewScorm.aspx?reference=" + reference;
    } else {
        obj = obj.replace("'", "");
        ViewModalObjclient("WfViewObj.aspx?obj=" + obj + "&reference=" + reference, title, obj)
        //  openObject("WfViewObj.aspx?obj=" + obj + "&reference=" + reference + "","PlayerItem",w,h);

    }
}


function lmspopup(uri, name,w,h) {

    PopupCenter(uri, name, w,h)

}

function ExitSession()
{
    location.reload();
    
    

}


function style_edit_form(form) {
    //enable datepicker on "sdate" field and switches for "stock" field
    form.find('input[name=sdate]').datepicker({ format: 'yyyy-mm-dd', autoclose: true })
        .end().find('input[name=stock]')
              .addClass('ace ace-switch ace-switch-5').wrap('<label class="inline" />').after('<span class="lbl"></span>');

    //update buttons classes
    var buttons = form.next().find('.EditButton .fm-button');
    buttons.addClass('btn btn-sm').find('[class*="-icon"]').remove();//ui-icon, s-icon
    buttons.eq(0).addClass('btn-primary').prepend('<i class="icon-ok"></i>');
    buttons.eq(1).prepend('<i class="icon-remove"></i>')

    buttons = form.next().find('.navButton a');
    buttons.find('.ui-icon').remove();
    buttons.eq(0).append('<i class="icon-chevron-left"></i>');
    buttons.eq(1).append('<i class="icon-chevron-right"></i>');
}

function style_delete_form(form) {
    var buttons = form.next().find('.EditButton .fm-button');
    buttons.addClass('btn btn-sm').find('[class*="-icon"]').remove();//ui-icon, s-icon
    buttons.eq(0).addClass('btn-danger').prepend('<i class="icon-trash"></i>');
    buttons.eq(1).prepend('<i class="icon-remove"></i>')
}

function style_search_filters(form) {
    form.find('.delete-rule').val('X');
    form.find('.add-rule').addClass('btn btn-xs btn-primary');
    form.find('.add-group').addClass('btn btn-xs btn-success');
    form.find('.delete-group').addClass('btn btn-xs btn-danger');
}

function style_search_form(form) {
    var dialog = form.closest('.ui-jqdialog');
    var buttons = dialog.find('.EditTable')
    buttons.find('.EditButton a[id*="_reset"]').addClass('btn btn-sm btn-info').find('.ui-icon').attr('class', 'icon-retweet');
    buttons.find('.EditButton a[id*="_query"]').addClass('btn btn-sm btn-inverse').find('.ui-icon').attr('class', 'icon-comment-alt');
    buttons.find('.EditButton a[id*="_search"]').addClass('btn btn-sm btn-purple').find('.ui-icon').attr('class', 'icon-search');
}

function beforeDeleteCallback(e) {
    var form = $(e[0]);
    if (form.data('styled')) return false;

    form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
    style_delete_form(form);

    form.data('styled', true);
}

function beforeEditCallback(e) {
    var form = $(e[0]);
    form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
    style_edit_form(form);
}


function styleCheckbox(table) {
    
    $(table).find('input:checkbox').addClass('ace')
    .wrap('<label />')
    .after('<span class="lbl align-top" />')


    $('.ui-jqgrid-labels th[id*="_cb"]:first-child')
    .find('input.cbox[type=checkbox]').addClass('ace')
    .wrap('<label />').after('<span class="lbl align-top" />');
    
}


function updateActionIcons(table) {
    
    var replacement = 
    {
        'ui-icon-pencil' : 'icon-pencil blue',
        'ui-icon-trash' : 'icon-trash red',
        'ui-icon-disk' : 'icon-ok green',
        'ui-icon-cancel' : 'icon-remove red'
    };
    $(table).find('.ui-pg-div span.ui-icon').each(function(){
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));
        if($class in replacement) icon.attr('class', 'ui-icon '+replacement[$class]);
    })
   
}

//replace icons with FontAwesome icons like above
function updatePagerIcons(table) {
    var replacement =
    {
        'ui-icon-seek-first': 'icon-double-angle-left bigger-140',
        'ui-icon-seek-prev': 'icon-angle-left bigger-140',
        'ui-icon-seek-next': 'icon-angle-right bigger-140',
        'ui-icon-seek-end': 'icon-double-angle-right bigger-140'
    };
    $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

        if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
    })
}

function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}

//var selr = jQuery(grid_selector).jqGrid('getGridParam','selrow');


function aceSwitch(cellvalue, options, cell) {
    setTimeout(function () {
        $(cell).find('input[type=checkbox]')
                .wrap('<label class="inline" />')
            .addClass('ace ace-switch ace-switch-5')
            .after('<span class="lbl"></span>');
    }, 0);
}
//enable datepicker
function pickDate(cellvalue, options, cell) {
    setTimeout(function () {
        $(cell).find('input[type=text]')
                .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
    }, 0);
}





function InitLmsRb() {

    $.ajaxSetup({
        cache: false,
    });

    setTimeout(function () {
        $("#modal").fadeOut(function () {
            // Wait for #msg to fade out before fading in #pageBody
            $("#pagebody").animate({
                opacity: "1.0"
            }, 800);
        });
    }, 1500);



    $('.easy-pie-chart.percentage').each(function () {
        var $box = $(this).closest('.infobox');
        var barColor = $(this).data('color') || (!$box.hasClass('infobox-dark') ? $box.css('color') : 'rgba(255,255,255,0.95)');
        var trackColor = barColor == 'rgba(255,255,255,0.95)' ? 'rgba(255,255,255,0.25)' : '#E2E2E2';
        var size = parseInt($(this).data('size')) || 50;
        $(this).easyPieChart({
            barColor: barColor,
            trackColor: trackColor,
            scaleColor: false,
            lineCap: 'butt',
            lineWidth: parseInt(size / 10),
            animate: /msie\s*(8|7|6)/.test(navigator.userAgent.toLowerCase()) ? false : 1000,
            size: size
        });
    })



}