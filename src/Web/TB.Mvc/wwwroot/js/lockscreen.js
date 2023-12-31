﻿var idleTime = 0;
var overlayTimer;

$(document).ready(function () {
    $(document).on("mousemove keydown", function () {
        idleTime = 0;
    });

    function showOverlay() {
        overlayTimer = setTimeout(function () {
            $("#lockScreenOverlay").addClass("show-overlay");
        }, 60000);
    }

    function hideOverlay() {
        clearTimeout(overlayTimer);
        $("#lockScreenOverlay").removeClass("show-overlay");
        idleTime = 0;
    }

    setInterval(function () {
        idleTime++;
        if (idleTime >= 60) {
            showOverlay();
        }
    }, 1000);
});
