function isNumber(value) {
    return !isNaN(parseFloat(value)) && isFinite(value);
}

function setPriceLabelUpdateClass(priceLabel, isUp) {

    priceLabel.classList.remove("down-price-update", "up-price-update");

    // Recalculate styles
    void priceLabel.offsetWidth;

    if (isUp) {
        priceLabel.classList.add("up-price-update");
    }
    else {
        priceLabel.classList.add("down-price-update");
    }
}

function setPriceLabelUpdatePrice(priceLabel, newPrice) {

    const currentPrice = priceLabel.dataset.price;

    if (newPrice > currentPrice) {
        setPriceLabelUpdateClass(priceLabel, true);
    }
    else if (newPrice < currentPrice) {
        setPriceLabelUpdateClass(priceLabel, false);
    }

    priceLabel.textContent = formatPrice(newPrice);
    priceLabel.dataset.price = newPrice;
}

function updatePriceLabels(updatedSymbol, newPrice) {

    const priceLabels = document.querySelectorAll(
        `[data-price-symbol="${updatedSymbol}"]`
    );

    for (const priceLabel of priceLabels) {

        if (isNumber(priceLabel.dataset.amountLabelNumber) != false) {

            const amountLabel = document.querySelector(
                `[data-amount-label-symbol="${updatedSymbol}"][data-amount-label-number="${priceLabel.dataset.amountLabelNumber}"]`
            );

            const amount = parseInt(amountLabel.dataset.amountLabelValue);
            newPrice *= amount;
        }

        setPriceLabelUpdatePrice(priceLabel, newPrice);
    }
}

function updateOwnershipLabels(updatedSymbol, newPrice) {

    let totalPriceOfOwnership = 0;

    ownershipJson.forEach((entry) => {

        if (updatedSymbol === entry.Symbol) {
            entry.TradablePriceInfos.Price = newPrice;
        }

        const totalPrice = entry.TradablePriceInfos.Price * entry.Amount;
        totalPriceOfOwnership += totalPrice;
    })

    const TotalOwnershipValueLabels = document.getElementsByClassName("TotalOwnershipValue");

    for (let totalOwnershipValueLabel of TotalOwnershipValueLabels) {
        setPriceLabelUpdatePrice(totalOwnershipValueLabel, totalPriceOfOwnership);
    }
}

function onMarketData(message) {

    const tradableUpdatePayload = JSON.parse(message);
    const tradablePriceInfos = tradableUpdatePayload["TradablePriceInfos"];
    const updatedSymbol = tradableUpdatePayload["Symbol"];
    const newPrice = tradablePriceInfos["Price"];

    updatePriceLabels(updatedSymbol, newPrice);

    if (ownershipJson !== null) {
        updateOwnershipLabels(updatedSymbol, newPrice);
    }
}

function initMarketConnection() {

    CurrentConnection = new signalR.HubConnectionBuilder().withUrl("/marketHub").build();

    CurrentConnection.start();
    CurrentConnection.on("ReceiveMarketData", onMarketData);
    window.addEventListener("beforeunload", () => {
        CurrentSocket.close();
    });
}

initMarketConnection();