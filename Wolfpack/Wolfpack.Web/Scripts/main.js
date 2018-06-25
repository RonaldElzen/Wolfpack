let isCollapsed = false;
let modalOpen = false;

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

function getPartial(url,data) {
    //Ajax request
    let httpRequest = new XMLHttpRequest();
    httpRequest.open('POST', url);
    httpRequest.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    httpRequest.send(data);

    //Handle result
    httpRequest.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            document.body.innerHTML += httpRequest.response;

            try {
                //Key up function to check if changes are made in input
                let input = document.querySelector("#NewSkillName")
                input.onkeyup = function (e) {
                    getSkillSuggestions("/Skill/GetSkills", e.target.value)
                }
            }
            catch (err) {

                }
           
        }
    }
}


/**
 * Function to open/close a info modal
 */
function showModal(heading, text,addCloseButton) {
    //Create modal background
    let modalBackground = document.createElement("div");
    modalBackground.setAttribute("class", 'modal-background');
    //Create modal body
    let modalBody = document.createElement("div");
    modalBody.setAttribute("class", 'modal');
    //Create Heading
    let modalHeading = document.createElement("div");
    modalHeading.setAttribute("class", 'modal-heading');
    let headingText = document.createElement("h2")
    headingText.appendChild(document.createTextNode(heading));
    modalHeading.append(headingText);

    //Create modal
    document.body.append(modalBackground);
    modalBody.append(modalHeading);
    modalBackground.append(modalBody);

    //Create modal text element
    if (text != null) {
        let modalTextDiv = document.createElement("div");
        modalTextDiv.setAttribute("class", 'modal-text');
        let modalText = document.createElement("p");
        modalText.appendChild(document.createTextNode(text));
        modalTextDiv.append(modalText);
        modalBody.append(modalTextDiv);
    }
 
    if (addCloseButton) {
        //Create Close button
        let closeButton = document.createElement("button");
        closeButton.setAttribute("class", 'modal-close');
        closeButton.setAttribute("id", "modal-close");
        let icon = document.createElement("i");
        icon.setAttribute("class", "fas fa-times fa-3x icon-only");
        closeButton.append(icon);
        modalHeading.append(closeButton);
        //Click listener for close button
        closeButton.addEventListener("click", function () {
            modalBackground.remove();
        });
    }
}

function createConfirmModal(actionLink) {

    showModal("Are you sure?", "You are about to delete a group, are you sure?", false);
    let cancelButton = document.createElement("button");
    cancelButton.setAttribute("class", 'action-button');
    cancelButton.setAttribute("id", 'cancel');
    cancelButton.innerText = "No";

    let confirmButton = document.createElement("button");
    confirmButton.setAttribute("class", 'action-button');
    confirmButton.setAttribute("id", 'confirm');
    confirmButton.innerText = "yes";

    document.querySelector(".modal-text").append(confirmButton);
    document.querySelector(".modal-text").append(cancelButton)

    document.querySelector("#cancel").addEventListener('click', function () {
        document.querySelector(".modal-background").remove();

    })
    document.querySelector("#confirm").addEventListener('click', function () {
        document.querySelector(".modal-background").remove();
        window.location = actionLink;
    });
}

//Handle ratings 
let stars = document.querySelectorAll(".star");
for (let i = 0; i < stars.length; i++) {
    stars[i].addEventListener('click', function () {
        let starsInParent = this.parentElement.children;
        for (let i = 0; i < starsInParent.length; i++) {
            starsInParent[i].children[0].style.color = "#d3d3d3";
        }
        for (let i = 0; i < this.dataset.value; i++) {
            this.parentElement.children[i].children[0].style.color = "#455B65";
        }
        document.querySelector("#Rating").value = this.dataset.value;
    });
}

//Display user suggestions in UI
let suggestions = document.querySelectorAll(".userNameSuggestion");
for (let i = 0; i < suggestions.length; i++) {
    suggestions[i].addEventListener('click', function () {
        UserName.value = this.innerText;
    });
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

document.querySelector(".modal-close").addEventListener("click", function () {
    document.querySelector(".modal-background").remove();
})