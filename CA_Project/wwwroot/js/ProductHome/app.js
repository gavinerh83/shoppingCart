window.onload = function () {
    const loginSpan = document.getElementById("login");
    const signupSpan = document.getElementById("signup");
    const shoppingCartImg = document.getElementById("shopping-cart-img");

    


    loginSpan.style.display = "none";
    signupSpan.style.display = "none";

    const addCartBtns = document.getElementsByClassName("add-cart-btn");
    const numberOfItems = document.getElementById("number-of-items-cart");

    const alignmentContainer = document.getElementById("alignment-container");
    alignmentContainer.classList.add("center-align");

    for (let i = 0; i < addCartBtns.length; i++) {
        addCartBtns[i].addEventListener("click", AddToCart);
    }



    function AddToCart(event) {
        let target = event.currentTarget;
        AddToCartServer(target.id);
    }

    function AddToCartServer(id) {
        let xhr = new XMLHttpRequest();
        xhr.open("POST", "/producthome/AddToCart");
        xhr.setRequestHeader("Content-Type", "application/json");

        xhr.onreadystatechange = function () {
            if (this.readyState == XMLHttpRequest.DONE) {
                if (this.status !== 200) {
                    return;
                }
                let data = JSON.parse(this.responseText);
                if (data.updated == true) {
                    //get the cart icon and increase the number
                    let num;
                    if (numberOfItems === null) {
                        window.location.href = "/producthome/index";
                        return;
                    } else {
                        num = parseInt(numberOfItems.innerHTML);
                    }
                    num += 1;
                    numberOfItems.innerHTML = num;
                }
            }
        }

        let data = {
            "id": id
        };

        xhr.send(JSON.stringify(data));
    }
}

