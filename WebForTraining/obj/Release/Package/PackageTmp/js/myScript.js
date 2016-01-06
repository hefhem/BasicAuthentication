

loading = '<div align="center" style="margin-top: 3%;"><i class="fa fa-cog green fa-spin fa-3x fa-pulse"></i></div>';

function ld_fm2(url) {
    location.hash = url //url.match(/(^.*)\./)[1]
    return false
}
var originalTitle = document.title;
function hashChange() {
    var page = location.hash.slice(1);
    if (page != "") {
        $('.pageContent').html(loading);
        //console.log(page+".php");
        $('.pageContent').load(page, function (response, status, xhr) {
            if (status == "error") {
                var msg = "Sorry there was an error: ";
                alertMsg('Notification', msg, 'error');

            }
            //alertMsg('Successfully processed!','success');
            document.title = originalTitle + ' – ' + page;
        })
    }
}
// part 3
if ("onhashchange" in window) { // cool browser
    $(window).on('hashchange', hashChange).trigger('hashchange');
} else { // lame browser
    var lastHash = '';
    setInterval(function () {
        if (lastHash != location.hash)
            hashChange();
        lastHash = location.hash;
    }, 100)
}

function alertMsg(ttl, msg, cls) {
    new PNotify({
        title: ttl,
        text: msg,
        type: cls
    });
};

function ld_fm(url, container) {
    //alert(url + " " + container);
    var link = url;
    var cnt = container;
    $("." + cnt).html(loading);
    $("." + cnt).load(link, function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error: " + xhr.status + " " + xhr.statusText;
            //$("." + cnt).html(msg + xhr.status + " " + xhr.statusText);
            alertMsg('Notification', msg, 'error');

        }
        //alertMsg('Notification', 'Successfully processed!', 'success');
    }
    );
    return false;
}
function ld_fm_with_id(ids, url, container) {
    //alert(url + " " + container);
    var link = url;
    var cnt = container;
    $("." + cnt).html(loading);
    $("." + cnt).load(link, { id: ids }, function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error: " + xhr.status + " " + xhr.statusText;
            //$("." + cnt).html(msg + xhr.status + " " + xhr.statusText);
            alertMsg('Notification', msg, 'error');

        }
        //alertMsg('Notification', 'Successfully processed!', 'success');
    }
    );
    return false;
}
function ld_fm_no_id(url, container) {
    //alert(url + " " + container);
    var link = url;
    var cnt = container;
    $("." + cnt).html(loading);
    $("." + cnt).load(link, function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error: " + xhr.status + " " + xhr.statusText;
            //$("." + cnt).html(msg + xhr.status + " " + xhr.statusText);
            alertMsg('Notification', msg, 'error');
        }
    }
    );
    return false;
}

function ld_modal_fm(url, container, id) {
    //alert(url + " " + container);
    var link = url;
    var cnt = container;
    var ids = id;
    $('#modalContent').modal('show');
    $("#" + cnt).html(loading);
    $("#" + cnt).load(link,{id:ids}, function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error: " + xhr.status + " " + xhr.statusText;
            //$("." + cnt).html(msg + xhr.status + " " + xhr.statusText);
            alertMsg('Notification', msg, 'error');
            $("#" + cnt).html('Notification', msg, 'error');
        }
        //alertMsg('Notification', 'Successfully processed!', 'success');
        
    }
    );
    return false;
}
function validate_form(fm_id, url) {
    //alert(fm_id);

    $.validate({
        form: "#"+fm_id,
        //modules: 'security',
        onError: function () {
            alertMsg('Notification', 'Please check your entries.', 'error');
            return false; // Will stop the submission of the form
        },
        onSuccess: function ($form) {
            //alert('The form is valid!');
            //alertMsg('Notification', 'This form is valid', 'success');
            setForm($form,url);
            return false; // Will stop the submission of the form
        }
    });
    //alert("I got called finally");
}

function setForm($form,url) {

    $.ajax({
        type: "POST",
        url: url,
        dataType: 'json',
        data: $form.serialize()
    }).done(function (dt) {
        //alert(dt);
        //console.log(dt);
        if (dt.isSuccess == 1) {
            alertMsg('Notification', dt.msg, 'success');
            if ($(".btn-primary", $form).text() == 'Save') {
                $form[0].reset();
            }
            $('.refresh').trigger('click');
        }
        else {
            alertMsg('Notification', dt.msg, 'error');
            //alert(dt.msg);
        }
    }).fail(function (jqXHR, textStatus) {
        alertMsg('Notification', textStatus, 'error');
        //alert(textStatus);
    });

    //alert('called finally');
    return false;
}

function delRecord(id, url) {
    //if (!confirm('Are you sure you want to delete this record?')) {
    //    return;
    //}
    $.ajax({
        type: "POST",
        url: url,
        dataType: 'json',
        data: {ids:id}
    }).done(function (dt) {
        //alert(dt);
        //console.log(dt);
        if (dt.isSuccess == 1) {
            alertMsg('Notification', dt.msg, 'success');
            $('.refresh').trigger('click');
        }
        else {
            alertMsg('Notification', dt.msg, 'error');
            //alert(dt.msg);
        }
    }).fail(function (jqXHR, textStatus) {
        alertMsg('Notification', textStatus, 'error');
        //alert(textStatus);
    });

    //alert('called finally');
    return false;
}
function del_dir_Record(id, url) {
    if (!confirm('Are you sure you want to cancel this record?')) {
        return;
    }
    $.ajax({
        type: "POST",
        url: url,
        dataType: 'json',
        data: { ids: id }
    }).done(function (dt) {
        //alert(dt);
        //console.log(dt);
        if (dt.isSuccess == 1) {
            alertMsg('Notification', dt.msg, 'success');
            $('.refresh').trigger('click');
        }
        else {
            alertMsg('Notification', dt.msg, 'error');
            //alert(dt.msg);
        }
    }).fail(function (jqXHR, textStatus) {
        alertMsg('Notification', textStatus, 'error');
        //alert(textStatus);
    });

    //alert('called finally');
    return false;
}

    
    
function validate_Register_form(fm_id, url) {
    
    $.validate({
        form: "#" + fm_id,
        modules: 'date',
        onError: function () {
            alertMsg('Notification', 'Please check your entries.', 'error');
            return false; // Will stop the submission of the form
        },
        onSuccess: function ($form) {
            //alert('The form is valid!');
            //alertMsg('Notification', 'This form is valid', 'success');
            setRegisterForm($form, url);
            return false; // Will stop the submission of the form
        }
    });
    //alert("I got called finally");
}

function validate_Dispatch_form(fm_id, url) {
    //alert(fm_id);
    form = "#" + fm_id;
    $form = $(form);
    st = $("#status").val();
    ldDate = $.trim($("#loadingDate").val());
    ldTime = $.trim($("#loadingTime").val());
    dpDate = $.trim($("#dispatchDate").val());
    dpTime = $.trim($("#dispatchTime").val());
    var rs = false;
    if (st == "Loaded" &&  ldDate == "") {
        alertMsg("Notification", "Loading date is required.", "error");
        $("#loadingDate").focus();
        return;
    }
    if (st == "Loaded" && ldTime == "") {
        alertMsg("Notification", "Loading time is required.", "error");
        $("#loadingTime").focus();
        return;
    }
    if (st == "Dispatched" && dpDate == "") {
        alertMsg("Notification", "Dispatch date is required.", "error");
        $("#dispatchDate").focus();
        return;
    }
    if (st == "Dispatched" && dpTime == "") {
        alertMsg("Notification", "Dispatch time is required.", "error");
        $("#dispatchTime").focus();
        return;
    }
    if (st == "Loaded" && ldTime != "") {
        rs = validatetime("loadingTime");
    }
    if (st == "Dispatched" && dpTime != "" && ldTime != "") {
        rs = validatetime("dispatchTime");
    }

    if (rs) {
        //alert("I got called");
        setRegisterForm($form, url);
    }
    
    //setRegisterForm($form, url);
    //return false;

}

function setRegisterForm($form, url) {
    
    $.ajax({
        type: "POST",
        url: url,
        dataType: 'json',
        data: $form.serialize()
    }).done(function (dt) {
        //alert(dt);
        //console.log(dt);
        if (dt.isSuccess == 1) {
            alertMsg('Notification', dt.msg, 'success');
            if ($("#tdoRegisterID").val() == "0") {
                //alert("Am here");
                $form[0].reset();
                //ld_fm2('/Modules/Register');
                //$('.refresh').trigger('click');
            }
        }
        else {
            alertMsg('Notification', dt.msg, 'error');
            //alert(dt.msg);
        }
    }).fail(function (jqXHR, textStatus) {
        alertMsg('Notification', textStatus, 'error');
        //alert(textStatus);
    });

    //alert('called finally');
    return false;
}

function styleTable(tb_id) {

        $('input.tableflat').iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
        });

    var asInitVals = new Array();
    var oTable = $('#'+tb_id).dataTable({
        "oLanguage": {
            "sSearch": "Search all columns:"
        },
        "aoColumnDefs": [
            {
                'bSortable': false,
                'aTargets': [0]
            } //disables sorting for column one
        ],
        
        'iDisplayLength': 12,
        "sPaginationType": "full_numbers",
        "dom": 'T<"clear">lfrtip'
        ,
        "tableTools": {
            "sSwfPath": "../js/datatables/tools/swf/copy_csv_xls_pdf.swf"
        }
    });
    $("tfoot input").keyup(function () {
        /* Filter on the column based on the index of this element's parent <th> */
        oTable.fnFilter(this.value, $("tfoot th").index($(this).parent()));
    });
    $("tfoot input").each(function (i) {
        asInitVals[i] = this.value;
    });
    $("tfoot input").focus(function () {
        if (this.className == "search_init") {
            this.className = "";
            this.value = "";
        }
    });
    $("tfoot input").blur(function (i) {
        if (this.value == "") {
            this.className = "search_init";
            this.value = asInitVals[$("tfoot input").index(this)];
        }
    });
}

function checkAll(ele, cls) {
    //var checkboxes = document.getElementsByTagName('input');
    //alert(cls);
    var checkboxes = document.getElementsByClassName(cls);
    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = true;
            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            console.log(i)
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = false;
            }
        }
    }
}

function countCheckBox(tbody_id, btn_id) {
    if ($('#' + tbody_id + ' input:checkbox:checked').length > 0) {
        $('#'+ btn_id).fadeIn();
    }
    else {
        //$('#chkAllTaxFormula').checked = false;
        $('#' + btn_id).fadeOut();
    }
}


function splitCheckboxIdDelete(tb_cls,body_id, url) {
    if ($('#' + body_id + ' input:checkbox:checked').length < 1) {
        alertMsg('Notification', 'please check the item to delete...', 'error');
        return;
    }
    if (!confirm('Do you want to delete ' + $('#' + body_id + ' input:checkbox:checked').length + ' record(s)')) {
        return
    }
      
    var sel_IDs = "";
    $('.' + tb_cls).each(function () {
        if (this.checked) {
            sel_IDs += $(this).val() + ",";
        }
    });

    delRecord(sel_IDs, url);

}

function checkAllTableBox(elem) {
    //alert("I got called");
    elem = $(elem);
    if (elem.prop("checked")) {
        //alert("I got called");
        elem.parents('table').find('tr').each(function () {
           // alert("I got called");
            var row = $(this);
            row.find('input[type="checkbox"]').each(function () {
            //row.find('input[type="checkbox"]:not(.firstcheck)').each(function () {
                //this is the current checkbox
                //alert("I got called");
                this.checked = true;
                //if(this.classList.contains("cscs"))
                this.value = 'true';
            });
        });
    } else {
        elem.parents('table').find('tr').each(function () {
            var row = $(this);
            row.find('input[type="checkbox"]').each(function () {
                //this is the current checkbox
                this.checked = false;
                this.value = 'false';
            });
        });
    }
}

function checkAllRow(elem) {
    //alert("I got called");
    //$(this).parents('tr').find(':checkbox').prop('checked', this.checked);
    elem = $(elem);
    if (elem.prop("checked")) {
        elem.parents('tr').find('input[type="checkbox"]').each(function () {
            //this is the current checkbox
            //alert("I got called");
            this.checked = true;
            this.value = 'true';
        });
    } else {
       elem.parents('tr').find('input[type="checkbox"]').each(function () {
            //this is the current checkbox
            //alert("I got called");
            this.checked = false;
            this.value = 'false';
        });
    }
    
}

function tog(elem) {
    if (elem.checked) {
        elem.value = 'true';
    } else {
        elem.value = 'false';
    }
}

function setAccessLevel() {
    var accessLevelID = '';
    var userGroupID = $('#userGroupID').val();
    var formID = '';
    var canAdd = '';
    var canView = '';
    var canEdit = '';
    var canDelete = '';
    var canApprove = '';

    $('#tblAccessLevel').find("tr").find("td").each(function () {

        $(this).children('#accessLevelID').each(function () {

            accessLevelID += $(this).val().toString() + ',';
        });
        $(this).children('#formID').each(function () {

            formID += $(this).val().toString() + ',';
        });
        $(this).children('.canAddCls').each(function () {
            canAdd += $(this).val().toString() + ',';
        });
        $(this).children('.canViewCls').each(function () {
            canView += $(this).val().toString() + ',';
        });
        $(this).children('.canEditCls').each(function () {
            canEdit += $(this).val().toString() + ',';
        });
        $(this).children('.canDeleteCls').each(function () {
            canDelete += $(this).val().toString() + ',';
        });
        $(this).children('.canApproveCls').each(function () {
            canApprove += $(this).val().toString() + ',';
        });
    });

    $.ajax({
        type: "POST",
        url: "/Administration/setAccessLevel",
        dataType: 'json',
        data: { accessLevelID: accessLevelID, userGroupID: userGroupID, formID: formID, canAdd: canAdd, canView: canView, canEdit: canEdit, canDelete: canDelete, canApprove: canApprove }
    }).done(function (dt) {
        //alert(dt);
        //console.log(dt);
        if (dt.isSuccess == 1) {
            alertMsg('Notification', 'Record saved successfully.', 'success');
        }
        else {
            alertMsg('Notification', 'Oops! Record could not be saved.', 'error');
            //alert(dt.msg);
        }
    }).fail(function (jqXHR, textStatus) {
        alertMsg('Notification', textStatus, 'error');
        //alert(textStatus);
    });

    //alert('called finally');
    return false;
    
}

function getDropDown(id, url, container) {
   
        //alert(url + " " + container);
        var link = url;
        var cnt = container;
        $("#" + cnt).html(loading);
        $("#" + cnt).load(link,{stateID: id}, function (response, status, xhr) {
            if (status == "error") {
                var msg = "Sorry but there was an error: " + xhr.status + " " + xhr.statusText;
                alertMsg('Notification', msg, 'error');
            }
        }
        );
        return false;
}
function IsNumeric(strString) {
    var strValidChars = "0123456789";
    var strChar;
    var blnResult = true;
    //var strSequence = document.frmQuestionDetail.txtSequence.value; 
    //test strString consists of valid characters listed above 
    if (strString.length == 0)
        return false;
    for (i = 0; i < strString.length && blnResult == true; i++) {
        strChar = strString.charAt(i);
        if (strValidChars.indexOf(strChar) == -1) {
            blnResult = false;
        }
    }
    return blnResult;
}
function trimString(str) {
    var str1 = '';
    var i = 0;
    while (i != str.length) {
        if (str.charAt(i) != ' ') str1 = str1 + str.charAt(i); i++;
    }
    var retval = IsNumeric(str1);
    if (retval == false)
        return -100;
    else
        return str1;
}
function trimAllSpace(str) {
    var str1 = '';
    var i = 0;
    while (i != str.length) {
        if (str.charAt(i) != ' ')
            str1 = str1 + str.charAt(i); i++;
    }
    return str1;
}
function  validatetime(elem)
{
    var strval = $("#"+elem).val();
    var strval1;
    
    //minimum lenght is 6. example 1:2 AM
    if(strval.length < 6)
    {
        alertMsg("Notification","Invalid time. Time format should be HH:MM AM/PM.", "error");
        return false;
    }
  
    //Maximum length is 8. example 10:45 AM
    if(strval.lenght > 8)
    {
        alertMsg("Notification","invalid time. Time format should be HH:MM AM/PM.", "error");
        return false;
    }
  
    //Removing all space
    strval = trimAllSpace(strval); 
  
    //Checking AM/PM
    if(strval.charAt(strval.length - 1) != "M" && strval.charAt(
        strval.length - 1) != "m")
    {
        alertMsg("Notification","Invalid time. Time shoule be end with AM or PM.", "error");
        return false;
   
    }
    else if(strval.charAt(strval.length - 2) != 'A' && strval.charAt(
        strval.length - 2) != 'a' && strval.charAt(
        strval.length - 2) != 'p' && strval.charAt(strval.length - 2) != 'P')
    {
        alertMsg("Notification","Invalid time. Time shoule be end with AM or PM.", "error");
        return false;
   
    }
  
    //Give one space before AM/PM
  
    strval1 =  strval.substring(0,strval.length - 2);
    strval1 = strval1 + ' ' + strval.substring(strval.length - 2,strval.length)
  
    strval = strval1;
      
    var pos1 = strval.indexOf(':');
    $("#" + elem).val(strval);
  
    if(pos1 < 0 )
    {
        alertMsg("Notification","invlalid time. A color(:) is missing between hour and minute.", "error");
        return false;
    }
    else if(pos1 > 2 || pos1 < 1)
    {
        alertMsg("Notification","invalid time. Time format should be HH:MM AM/PM.", "error");
        return false;
    }
  
    //Checking hours
    var horval =  trimString(strval.substring(0,pos1));
   
    if(horval == -100)
    {
        alertMsg("Notification","Invalid time. Hour should contain only integer value (0-11).", "error");
        return false;
    }
      
    if(horval > 12)
    {
        alertMsg("Notification","invalid time. Hour can not be greater that 12.", "error");
        return false;
    }
    else if(horval < 0)
    {
        alertMsg("Notification","Invalid time. Hour can not be hours less than 0.", "error");
        return false;
    }
    //Completes checking hours.
  
    //Checking minutes.
    var minval =  trimString(strval.substring(pos1+1,pos1 + 3));
  
    if(minval == -100)
    {
        alertMsg("Notification","Invalid time. Minute should have only integer value (0-59).", "error");
        return false;
    }
    
    if(minval > 59)
    {
        alertMsg("Notification","Invalid time. Minute can not be more than 59.", "error");
        return false;
    }   
    else if(minval < 0)
    {
        alertMsg("Notification","Invalid time. Minute can not be less than 0.", "error");
        return false;
    }
   
    //Checking minutes completed.
  
    //Checking one space after the mintues 
    minpos = pos1 + minval.length + 1;
    if(strval.charAt(minpos) != ' ')
    {
        alertMsg("Notification","Invalid time. Space missing after minute. Time should have HH:MM AM/PM format.", "error");
        return false;
    }
 
    return true;
}