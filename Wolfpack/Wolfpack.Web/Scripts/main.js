let isCollapsed = false;

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
    isCollapsed = true;
}

document.querySelector("#toggle-collapse").addEventListener('click', function () {
    if (isCollapsed) {
        document.querySelector(".sidebar").classList.remove("collapsed");
        isCollapsed = false;

    }
    else {
        document.querySelector(".sidebar").classList.add("collapsed");
        isCollapsed = true;
    }
});