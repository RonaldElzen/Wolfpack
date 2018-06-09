﻿let isCollapsed = false;

/**
 * Function to toggle the menu 
 **/
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

//Make sidebar collapse on tablet
if (window.innerWidth <= 1024 && window.innerWidth >= 600) {
    document.querySelector(".sidebar").classList.add("collapsed");
    document.querySelector(".sidebar-wrapper").classList.add("sidebar-small");
    isCollapsed = true;
}

//handle click event for collapse of sidebar
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

//Display user suggestions in UI
let suggestions = document.querySelectorAll(".userNameSuggestion");
for (let i = 0; i < suggestions.length; i++) {
    suggestions[i].addEventListener('click', function () {
        UserName.value = this.innerText;
    });
}

//Key up function to check if changes are made in input
let input = document.querySelector("#NewSkillName")
input.onkeyup = function (e) {
    getSkillSuggestions("/Skill/GetSkills", e.target.value)
}

/**
 * Function to autocomplete skills
 * @param {any} url
 * @param {any} prefix
 */
function getSkillSuggestions(url, prefix) {

    //Ajax request
    let httpRequest = new XMLHttpRequest();
    httpRequest.open('POST', url);
    httpRequest.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    httpRequest.send(JSON.stringify({
        prefix: prefix
    }));

    //Handle result
    httpRequest.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            //Get autocomplete element
            let autocomplete = document.querySelector("#autocomplete");
            //Reset changed form
            document.querySelector("#skillDescriptionItem").style.display = "block"
            document.querySelector("#NewSkillDescription").required = true;
            //Make sure it's empty
            while (autocomplete.firstChild) {
                autocomplete.removeChild(autocomplete.firstChild);
            }
            let skills = JSON.parse(httpRequest.response)
            //Loop through suggestions and display
            for (let i = 0; i < skills.length; i++) {
                let li = document.createElement('li');
                li.appendChild(document.createTextNode(skills[i]));
                //Add click function to elements
                li.addEventListener('click', function () {
                    //On click, fill the selected skill in the form item
                    document.querySelector("#NewSkillName").value = this.innerText;
                    //Hide description field
                    document.querySelector("#NewSkillDescription").required = false;
                    document.querySelector("#skillDescriptionItem").style.display = "none"
                    let autocomplete = document.querySelector("#autocomplete");
                    while (autocomplete.firstChild) {
                        autocomplete.removeChild(autocomplete.firstChild);
                    }
                });
                autocomplete.appendChild(li);
            }
        }
    };
}

