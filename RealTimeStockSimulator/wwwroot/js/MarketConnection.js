function FormatPrice(price) {
    return price.toLocaleString('en-US', {
        style: 'currency',
        currency: 'USD',
    });
}

function IsNumber(value) {
    return !isNaN(parseFloat(value)) && isFinite(value);
}

function UpdatePriceLabel(updatedSymbol, newPrice) {

    const priceLabels = document.getElementsByClassName("TradablePrice_" + updatedSymbol);

    for (let priceLabel of priceLabels) {

        if (IsNumber(priceLabel.dataset.labelNumber) != false) {

            const amountLabel = document.getElementById("TradableAmount_" + priceLabel.dataset.labelNumber + "_" + updatedSymbol);
            const amount = parseInt(amountLabel.textContent);

            newPrice *= amount;
        }

        priceLabel.textContent = FormatPrice(newPrice);
    }
}

function UpdateOwnershipLabels(updatedSymbol, newPrice) {

    let totalPriceOfOwnership = 0

    ownershipJson.forEach((entry) => {

        if (updatedSymbol === entry.Symbol) {
            entry.TradablePriceInfos.Price = newPrice;
        }

        const totalPrice = entry.TradablePriceInfos.Price * entry.Amount;
        totalPriceOfOwnership += totalPrice;
    });

    const TotalOwnershipValueLabels = document.getElementsByClassName("TotalOwnershipValue");

    for (let totalOwnershipValueLabel of TotalOwnershipValueLabels) {
        totalOwnershipValueLabel.textContent = FormatPrice(totalPriceOfOwnership);
    }
}

function OnMarketData(message) {

    const tradableUpdatePayload = JSON.parse(message);
    const tradablePriceInfos = tradableUpdatePayload["TradablePriceInfos"];
    const updatedSymbol = tradableUpdatePayload["Symbol"];
    const newPrice = tradablePriceInfos["Price"];

    UpdatePriceLabel(updatedSymbol, newPrice);

    if (ownershipJson !== null) {
        UpdateOwnershipLabels(updatedSymbol, newPrice);
    }
}

function init() {

    CurrentConnection = new signalR.HubConnectionBuilder().withUrl("/marketHub").build();

    CurrentConnection.start();
    CurrentConnection.on("ReceiveMarketData", OnMarketData);
    window.addEventListener("beforeunload", () => {
        CurrentSocket.close();
    });
}

init();