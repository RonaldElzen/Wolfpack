let isCollapsed = false;
let modalOpen = false;

/**
 * Function to show loading
 */
function showLoading() {
    let loading = document.createElement("div");
    loading.setAttribute("class", "loading");
    let image = document.createElement("img");
    image.setAttribute("src", "/Content/images/logo_animated.svg");
    let text = document.createElement("h2");
    text.appendChild(document.createTextNode("Please Wait"));
    loading.append(image);
    loading.append(text);
    document.body.append(loading);
}

/**
 * Function to hide loading
 */
function hideLoading() {
    document.querySelector(".loading").remove();
}

/**
 * Function to toggle the menu 
 **/
function setMenuListener() {
    document.querySelector(".toggle-menu").addEventListener('click', function () {
        document.querySelector(".sidebar").style.marginLeft = "0";
        let overlay = document.createElement('div');
        overlay.classList.add("sidebar-overlay");
        overlay.addEventListener('click', function () {

            document.querySelector(".sidebar").style.marginLeft = "-200px";
            overlay.remove();
        });
        document.querySelector(".wrapper").appendChild(overlay);
    });
}

setMenuListener();


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

/**
 * Function to get partial
 * @param {String} url 
 * @param {JSON} data
 */
function getPartial(url, data) {
    showLoading();
    //Ajax request
    let httpRequest = new XMLHttpRequest();
    httpRequest.open('POST', url);
    httpRequest.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    httpRequest.send(data);
    //Handle result
    httpRequest.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            document.body.innerHTML += httpRequest.response;
            //Key up function to check if changes are made in input
            let input = document.querySelector("#NewSkillName");
            if (input !== null) {
                input.onkeyup = function (e) {
                    getSkillSuggestions("/Skill/GetSkills", e.target.value);
                };
            }
            if (document.querySelector(".submit-button") !== null) {
                document.querySelector(".submit-button").addEventListener("click", function () {
                    showLoading();
                })
              
            }

            if (document.querySelector(".modal-close") !== null) {
                document.querySelector(".modal-close").addEventListener("click", function () {
                    let throwAway = document.querySelector(".modal-background");
                    document.querySelector("#" + throwAway.id).remove();
                })
                addListeners();
                setMenuListener();
            };
            hideLoading();
        }
    };
}

/**
 * Function to create ratings
 */
function createRatings() {
    //Handle ratings 
    let ratings = document.querySelectorAll(".rating");

    for (let i = 0; i < ratings.length; i++) {
        stars = ratings[i].getElementsByClassName("star");
        for (let j = 0; j < stars.length; j++)
            stars[j].addEventListener('click', function () {
                let starsInParent = this.parentElement.children;
                for (let i = 0; i < starsInParent.length; i++) {
                    starsInParent[i].children[0].style.color = "#d3d3d3";
                }
                for (let i = 0; i < this.dataset.value; i++) {
                    this.parentElement.children[i].children[0].style.color = "#455B65";
                }
                this.parentElement.dataset.rate = this.dataset.value;
            });
    }
}


/**
 * Function to start the team rating
 * @param {String} url
 * @param {JSON} data
 */
function getTeamRating(url, data) {
    showLoading();
    //Ajax request
    let httpRequest = new XMLHttpRequest();
    httpRequest.open('POST', url);
    httpRequest.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    httpRequest.send(data);
    //Handle result
    httpRequest.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            document.querySelector(".main").innerHTML += httpRequest.response;

            createRatings();
            document.querySelector(".submit-button").addEventListener("click", function () {

                let ratingsToSend = []
                let ratings = document.querySelectorAll(".rating");
                for (let i = 0; i < ratings.length; i++) {
                    ratingsToSend.push({
                        "Id": ratings[i].dataset.ratingid,
                        "Rating": ratings[i].dataset.rate,
                        "Comment": ratings[i].parentElement.querySelector(".text-box").value
                    });
                }
                sendRatings("/Event/HandleRating", ratingsToSend);

            });
            hideLoading();

        }
    };
}

/**
 * Function to send the rating
 * @param {String} url
 * @param {JSON} data
 */
function sendRatings(url, data) {
    showLoading();
    //Ajax request
    let httpRequest = new XMLHttpRequest();
    httpRequest.open('POST', url);
    httpRequest.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    httpRequest.send(JSON.stringify({
        eventId: document.querySelector("#rating").dataset.eventid,
        userId: document.querySelector("#rating").dataset.userid,
        skillId: document.querySelector(".rating").dataset.ratingid,
        ratings: data
    }));

    httpRequest.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            let eventId = document.querySelector("#rating").dataset.eventid
                document.querySelector("#rating").remove();
            users.shift();
            getTeamRating('/Event/RatePartial', JSON.stringify({
                userId: users[0].id,
                eventId: eventId
            }));
        }
        hideLoading();
    };
}

/**
 * Function to count the notifications and display in sidebar
 * @param {any} url
 */
function getNotificationCount(url) {
    //Ajax request
    let httpRequest = new XMLHttpRequest();
    httpRequest.open('POST', url);
    httpRequest.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    httpRequest.send();
    if (document.querySelector(".notification-count") !== null) {
        document.query(".notification-count").remove();
    }
    //Handle result
    httpRequest.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {

            if (httpRequest.response > 0) {
                let notificationcount = document.createElement("p");
                notificationcount.setAttribute("class", "notification-count");
                notificationcount.appendChild(document.createTextNode(httpRequest.response));
                document.querySelector("#notification-box").append(notificationcount);
            }
        }
    };
}

/**
 * Function to show modal
 * @param {String} heading
 * @param {String} text
 * @param {Boolean} addCloseButton
 */
function showModal(heading, text, addCloseButton) {
    //Create modal background
    let modalBackground = document.createElement("div");
    modalBackground.setAttribute("class", 'modal-background');
    //Create modal body
    let modalBody = document.createElement("div");
    modalBody.setAttribute("class", 'modal');
    //Create Heading
    let modalHeading = document.createElement("div");
    modalHeading.setAttribute("class", 'modal-heading');
    let headingText = document.createElement("h2");
    headingText.appendChild(document.createTextNode(heading));
    modalHeading.append(headingText);

    //Create modal
    document.body.append(modalBackground);
    modalBody.append(modalHeading);
    modalBackground.append(modalBody);

    //Create modal text element
    if (text !== null) {
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
        document.querySelector(".modal-close").addEventListener("click", function () {
            alert()
            document.querySelector(".modal-background").remove();
        });
    }
}

/**
 * Function to create confirm modal
 * @param {String} actionLink
 */
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
    document.querySelector(".modal-text").append(cancelButton);

    document.querySelector("#cancel").addEventListener('click', function () {
        document.querySelector(".modal-background").remove();

    });
    document.querySelector("#confirm").addEventListener('click', function () {
        document.querySelector(".modal-background").remove();
        window.location = actionLink;
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
            document.querySelector("#skillDescriptionItem").style.display = "block";
            document.querySelector("#NewSkillDescription").required = true;
            //Make sure it's empty
            while (autocomplete.firstChild) {
                autocomplete.removeChild(autocomplete.firstChild);
            }
            let skills = JSON.parse(httpRequest.response);
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
                    document.querySelector("#skillDescriptionItem").style.display = "none";
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

//Display user suggestions in UI
let suggestions = document.querySelectorAll(".userNameSuggestion");
for (let i = 0; i < suggestions.length; i++) {
    suggestions[i].addEventListener('click', function () {
        UserName.value = this.innerText;
        if (document.querySelector("#SearchProfile") !== null){
            document.querySelector("#SearchProfile").submit();
        }
        if (document.querySelector("#AddUserForm") !== null) {
            document.querySelector("#AddUserForm").submit();
        }

    });
}
