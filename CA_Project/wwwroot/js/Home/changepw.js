window.onload = function () {
    let form = document.getElementById("form2");
    let ele = document.getElementById("cpassword");
    let pw = document.getElementById("newpassword");
    let con = document.getElementById("confirmp");
    form.onsubmit = function () {
        let password = ele.value;
        let newpassword = pw.value;
        let confirm = con.value;
        if (password != "") {
            if (newpassword == "") {
                alert("Please enter your new Password");
                return false;
            }
            if (newpassword != confirm) {
                alert("Please Confirm your password!");
                return false;
            }
            else if (newpassword === confirm) {
                return true;
            }
            return false;
        }
        else {
            alert("Please enter your password!");
            return false;
        }
        // window.location.href = "/User/Index";
    }
}