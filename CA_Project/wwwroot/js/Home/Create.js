window.onload = function () {
    let element = document.getElementById("id");
    if (!element) {
        return;
    }
    element.onblur = function () {
        checkUserId(element.value);
    }

    const cartImage = document.getElementById("shopping-cart-img");
    const displayname = document.getElementById("displayname");
    const logout = document.getElementById("logout");

    cartImage.style.display = "none";
    displayname.style.display = "none";
    logout.style.display = "none";

    let form = document.getElementById("form");
    let elem = document.getElementById("id");
    let ele = document.getElementById("password");
    let con = document.getElementById("confirm");
    let ema = document.getElementById("email");
    form.onsubmit = function () {
        let userid = elem.value;
        let password = ele.value;
        let confirm = con.value;
        let email = ema.value;
        if (password != "" && userid !="") {
            if (password != confirm) {
                alert("Please Confirm your password!");
                return false;
            }
            else if (email == "") {
                alert("Please enter your Email!");
                return false;
            }
            else if (password === confirm && email != "") {
                return true;
            }
            return false;
            
        }
        else {
            alert("Please enter your userId and password!");
            return false;
        }
       // window.location.href = "/User/Index";
    }
    
}
function checkUserId(id) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/home/CheckUser");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status != 200)
                return;

            let data = JSON.parse(this.responseText);
            if (data.isOkay === false) {
                alert("UserID is unavailable");
                window.location.href = "/home/register";
            }
        }
    };
    let data = {
        "username": id
    };
    xhr.send(JSON.stringify(data));
}