function addToCart(id, name, count) {
    var cart = [];
    var cartStr = localStorage.getItem("cart");
    if (cartStr) {
        cart = JSON.parse(cartStr);
    }
    var bookAlreadyInCart = cart.filter(a => a.id === id).length;
    if (bookAlreadyInCart) {
        var bookIndex = cart.findIndex(a => a.id === id);
        cart[bookIndex].count += count;
    } else {
        cart.push({ id, name, count });
    }
    localStorage.setItem("cart", JSON.stringify(cart));
    updateCartBookCountElements();
    console.log(cart);

}
function removeFromCart(id, count) {
    if (!count) {
        count = 9999999;
    }

    var cart = [];
    var cartStr = localStorage.getItem("cart");
    if (cartStr) {
        cart = JSON.parse(cartStr);
    } else {
        return;
    }

    var bookInCart = cart.filter(a => a.id === id).length;
    if (!bookInCart) {
        return;
    }
    
    var bookIndex = cart.findIndex(a => a.id === id);
    if (cart[bookIndex].count <= count) {
        cart = cart.filter(a => a !== cart[bookIndex]);
    } else {
        cart[bookIndex].count = cart[bookIndex].count - count;
    }

    localStorage.setItem("cart", JSON.stringify(cart));
    updateCartBookCountElements();
    console.log(cart);
}

function getCart() {
    if (localStorage.getItem("cart"))
        return JSON.parse(localStorage.getItem("cart"));
    else
        return [];
}

function clearCart() {
    localStorage.setItem("cart", JSON.stringify([]));
    updateCartBookCountElements();

}

function updateCartBookCountElements() {
    var elementsToUpdate = $("[name='cart-item-count']");
    var booksCount = 0;
    if (getCart().length > 1) {
        booksCount = getCart().reduce((a, b) => a.count + b.count);
    } else if (getCart().length == 1) {
        booksCount = getCart()[0].count;
    }
    elementsToUpdate.text(booksCount);
    
}
async function getCartPrice() {
    const response = fetch("/Transaction/GetCartPrice", {
        method: "POST",
        headers: {
            'Accept': '*/*',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(getCart())
    }).then(a => a.text());

    const result = parseFloat(await response);
    console.log("Cart price is " + result);
    return result;

}
