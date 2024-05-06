function goBack() {
    window.history.back()
}

function clearText(field) {
    if (field.defaultValue == field.value) field.value = '';
    else if (field.value == '') field.value = field.defaultValue;
}

function disableSelection(target) {
    if (typeof target.onselectstart != "undefined") //IE route
        target.onselectstart = function() { return false }
    else if (typeof target.style.MozUserSelect != "undefined") //Firefox route
        target.style.MozUserSelect = "none"
    else //All other route (ie: Opera)
        target.onmousedown = function() { return false }
    target.style.cursor = "default"
}

function killCopy(e) {
    return false
}
function reEnable() {
    return true
}
document.onselectstart = new Function("return false")
if (window.sidebar) {
    document.onmousedown = killCopy
    document.onclick = reEnable
}


//window.onload = function() { disableSelection(document.body) }  //disable text selection on entire body of page