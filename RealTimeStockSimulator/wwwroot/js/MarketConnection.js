function IsNumber(value) {
    return !isNaN(parseFloat(value)) && isFinite(value);
}

function UpdatePriceLabel(updatedSymbol, newPrice) {

    const priceLabels = document.querySelectorAll(
        `[data-price-symbol="${updatedSymbol}"]`
    );

    for (let priceLabel of priceLabels) {

        if (IsNumber(priceLabel.dataset.amountLabelNumber) != false) {

            const amountLabel = document.querySelector(
                `[data-amount-label-symbol="${updatedSymbol}"][data-amount-label-number="${priceLabel.dataset.amountLabelNumber}"]`
            );

            const amount = parseInt(amountLabel.dataset.amountLabelValue);

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

function InitMarketConnection() {

    CurrentConnection = new signalR.HubConnectionBuilder().withUrl("/marketHub").build();

    CurrentConnection.start();
    CurrentConnection.on("ReceiveMarketData", OnMarketData);
    window.addEventListener("beforeunload", () => {
        CurrentSocket.close();
    });
}

InitMarketConnection();