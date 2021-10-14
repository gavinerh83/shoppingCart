//js file for shopping cart

const shoppingCartImg = document.getElementById("shopping-cart-img");
const login = document.getElementById("login");
const register = document.getElementById("signup");
const cartTotal = document.getElementById("cart-total");
const qtyBox = document.getElementsByClassName("qtybox"); //input box

//update subtotal and cart total
UpdateDisplay();

for (let i = 0; i < qtyBox.length; i++) {
    qtyBox[i].onblur = CheckInput;
}

function UpdateDisplay() {
    let carttotal = 0;
    for (let i = 0; i < qtyBox.length; i++) {
        let quantityEle = qtyBox[i].value;
        let priceEle = qtyBox[i].parentNode.previousElementSibling.children[1].innerHTML;
        let subtotal = quantityEle * parseFloat(priceEle);
        //update subtotal
        let subtotalEle = qtyBox[i].parentNode.nextElementSibling.children[1];
        subtotalEle.innerHTML = subtotal;

        //update to cart total
        carttotal += subtotal;
    }
    cartTotal.innerHTML = carttotal;
}


//hiding display from navbar
shoppingCartImg.style.display = "none";
login.style.display = "none";
register.style.display = "none";


function CheckInput(event) {
    //check the input value
    let target = event.currentTarget;
    if (target.value <= 0) {
        target.value = 1;
        alert("Quantity cannot be lower than 1");
    }
    //send to ajax method to update db
    UpdateQuantity(target);

    //update subtotal and cart total
}




function UpdateQuantity(target) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/cart/UpdateQuantity");
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status !== 200) {
                return;
            }
            let data = JSON.parse(this.responseText);
            if (data !== null) {
                //retrieve the quantity and price
                if (data.success === true) {
                    UpdateDisplay();
                }
            } else {
                alert("did not update");
            }
        }
    }

    let data = {};
    data.Id = target.id;
    data.Quantity = parseInt(target.value);

    xhr.send(JSON.stringify(data));
}
