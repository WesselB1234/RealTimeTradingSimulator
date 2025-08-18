
function OnMarketData(message) {
    console.log(message);
}

function init() {

    CurrentConnection = new signalR.HubConnectionBuilder().withUrl("/marketHub").build();

    CurrentConnection.start();
    CurrentConnection.on("ReceiveMarketData", OnMarketData);
    window.addEventListener("beforeunload", () => {
        CurrentSocket.close();
    });
}