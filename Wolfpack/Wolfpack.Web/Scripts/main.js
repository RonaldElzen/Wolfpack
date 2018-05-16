﻿let isCollapsed = false;

document.querySelector(".toggle-menu").addEventListener('click', function () {
    document.querySelector(".sidebar").style.marginLeft = "0"
    let overlay = document.createElement('div');
    overlay.classList.add("sidebar-overlay");
    overlay.addEventListener('click', function () {

        document.querySelector(".sidebar").style.marginLeft = "-200px";
        overlay.remove();
    });
    document.querySelector(".wrapper").appendChild(overlay);
});



if (window.innerWidth <= 1024 && window.innerWidth >= 600) {
    document.querySelector(".sidebar").classList.add("collapsed");
    document.querySelector(".sidebar-wrapper").classList.add("sidebar-small");
    isCollapsed = true;
}

document.querySelector("#toggle-collapse").addEventListener('click', function () {
    if (isCollapsed) {
        document.querySelector(".sidebar").classList.remove("collapsed");
        document.querySelector(".sidebar-wrapper").classList.remove("sidebar-small");
        isCollapsed = false;

    }
    else {
        document.querySelector(".sidebar").classList.add("collapsed");
        document.querySelector(".sidebar-wrapper").classList.add("sidebar-small");
        isCollapsed = true;
    }
});

let suggestions = document.querySelectorAll(".userNameSuggestion");
for (let i = 0; i < suggestions.length; i++) {
    suggestions[i].addEventListener('click', function () {
        UserName.value = this.innerText;
    });
}