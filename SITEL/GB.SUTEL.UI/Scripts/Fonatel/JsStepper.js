﻿$(document).on("click", ".step_navigation_indicador div", function (e) {
    $(".step_navigation_indicador div").removeClass('active');
    $(this).addClass('active');
    let selected = $(this);

    $(this).siblings().each(function () {
        if ($(this).attr("data-step") < selected.attr("data-step")) {
            $(this).addClass('active');
        }
        else {
            $(this).removeClass('active');
        }
    });

    e.preventDefault();
    let div = $(this).children("a").attr("href");
    $(".stepwizard-content-container").addClass('hidden');
    $(div).removeClass('hidden');
});


$(document).ready(function () {
    $(".stepwizard-step[data-step='0']").trigger('click');
});