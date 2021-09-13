
function clamp(value, minValue, maxValue) {
    let numValue = Number.parseFloat(value);
    if(isNaN(numValue)) return value;
    
    return Math.min(maxValue, Math.max(minValue, numValue));
}

function getProductRequest(id) {
    let request = new XMLHttpRequest();
    request.responseType = 'json';
    request.open('GET', '/products/' + id);
    request.send();
    return request;
}

function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min)) + min;
}

function fadeAlerts() {
    const intervalFadeError = setInterval(function() {
        $('.fade-alert').fadeOut(300, 'swing', function() {});
        clearInterval(intervalFadeError);
    }, 5000);
}

function sendDeleteRequest(path) {
    let request = new XMLHttpRequest();
    request.open('DELETE', path, false);
    request.send();
}

function redirectTo(url) {
    window.location.assign(url);
}

function roundTo(num, decimalPlaces) {
    let factor = Math.pow(10, decimalPlaces);
    return Math.round(num * factor) / factor;
}

let touchtime = 0;
// https://stackoverflow.com/questions/27560653/jquery-on-double-click-event-dblclick-for-mobile
function executeOnDoubleClick(doubleClickFunction) {
    if (touchtime === 0) {
        // set first click
        touchtime = new Date().getTime();
    } else {
        // compare first click to this click and see if they occurred within double click threshold
        if (((new Date().getTime()) - touchtime) < 800) {
            // double click occurred
            doubleClickFunction();
            touchtime = 0;
        } else {
            // not a double click so set as a new first click
            touchtime = new Date().getTime();
        }
    }
}

function clearValue(element) {
    element.value = undefined;
}
