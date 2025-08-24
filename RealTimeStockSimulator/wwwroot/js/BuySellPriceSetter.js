function OnAmountChanged(newAmount) {

    if (newAmount > 0) {
        console.log(newAmount);
    }
}

function InitBuySellPriceSetter() {

    let amountInput = document.getElementById("amount");

    amountInput.addEventListener("input", (event) => {
        OnAmountChanged(Number(event.target.value));
    });
}

InitBuySellPriceSetter();