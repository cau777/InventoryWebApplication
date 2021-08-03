
function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min)) + min;
}

function fadeAlert() {
    const intervalFadeError = setInterval(function() {
        $("#failed-alert").fadeOut(300, "swing", function() {});
        clearInterval(intervalFadeError);
    }, 5000);
}

function sendDeleteRequest(path) {
    let request = new XMLHttpRequest();
    request.open("DELETE", path, false);
    request.send();
}

function redirectTo(url) {
    window.location.assign(url);
}

function roundTo(num, decimalPlaces) {
    let factor = Math.pow(10, decimalPlaces);
    return Math.round(num * factor) / factor;
}