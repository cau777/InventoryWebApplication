
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
