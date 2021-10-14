window.onload = function () {
    //for the modal display
    const closeBtn = document.getElementById("close");
    const closeBtn2 = document.getElementById("close2");
    const modalContainer = document.getElementById("modal");
    
    //const modalContainer2 = document.getElementById("modal2");



    const loginBtn = document.getElementById("login");
    const signupBtn = document.getElementById("signup");
    const displayname = document.getElementById('displayname');
    const logout = document.getElementById("logout");
    const shoppingCartImg = document.getElementById("shopping-cart-img");
    const purchases = document.getElementById("purchases");
    const account = document.getElementById("account");
    

    //hidding displays
    displayname.style.display = 'none';
    logout.style.display = "none";
    shoppingCartImg.style.display = "none";
    purchases.style.display = "none";
    account.style.display = 'none';



    const usernameLabel = document.getElementById("label-username");
    const emailLabel = document.getElementById("label-email");

    closeBtn.addEventListener('click', close);
    closeBtn2.addEventListener('click', close2);
    loginBtn.addEventListener('click', open);
    signupBtn.addEventListener('click', open2);


    function close() {
        modalContainer.style.display = "none";
    }

    function close2() {
        modalContainer2.style.display = "none";
    }
    function open() {
        modalContainer.style.display = "flex";
    }
    function open2() {
        modalContainer2.style.display = "flex";
    }

    let username = document.getElementById("username");
    let email = document.getElementById("email");

    email.onblur = function () {
        checkIsEmailUsed(email.value);
    }

    username.onblur = function () {
        checkIsUsernameUsed(username.value);
    }

    function checkIsEmailUsed(email) {
        const xhr = new XMLHttpRequest();
        xhr.open("POST", "Home/CheckEmailDuplicate");
        xhr.setRequestHeader("Content-Type", "application/json");

        xhr.onreadystatechange = function () {
            if (this.readyState === XMLHttpRequest.DONE) {
                if (this.status !== 200) {
                    return;
                }

                let data = JSON.parse(this.responseText);
                if (emailLabel.innerHTML !== "") {
                    emailLabel.innerHTML = "";
                }
                if (data.unique === false) {
                    emailLabel.innerHTML = "Email is used";
                }
            }
        }

        let data = {
            "email": email
        };
        xhr.send(JSON.stringify(data));
    }

    function checkIsUsernameUsed(username) {
        let xhr = new XMLHttpRequest();

        xhr.open("POST", "Home/CheckUserDuplicate");
        xhr.setRequestHeader("Content-Type", "application/json", "charset=utf8");

        xhr.onreadystatechange = function () {
            if (this.readyState === XMLHttpRequest.DONE) {
                //response from server is complete
                if (this.status !== 200) {
                    return;
                }
                let data = JSON.parse(this.responseText);
                if (usernameLabel.innerHTML !== "") {
                    usernameLabel.innerHTML = "";
                }
                if (data.unique === false) {
                    usernameLabel.innerHTML = "Username is used"
                }
            }
        }
        let data = {
            "username": username
        };
        xhr.send(JSON.stringify(data));
    }

}

function close() {
    modalContainer.style.display = "none";
}

function close2() {
    modalContainer2.style.display = "none";
}
function open() {
    modalContainer.style.display = "flex";
}
function open2() {
    modalContainer2.style.display = "flex";
}