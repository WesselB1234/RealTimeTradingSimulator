function onAmountChanged(amountInput, totalValueLbl, newAmount)
{
    if (newAmount > 0)
    {
        amountInput.dataset.amountLabelValue = newAmount;
        totalValueLbl.textContent = formatPrice(totalValueLbl.dataset.price * newAmount);
    }
}

function initBuySellPriceSetter()
{
    let amountInput = document.getElementById("amount");
    let totalValueLbl = document.getElementById("totalValueLbl");

    amountInput.addEventListener("input", (event) => {
        onAmountChanged(amountInput, totalValueLbl, Number(event.target.value));
    });
}

initBuySellPriceSetter();