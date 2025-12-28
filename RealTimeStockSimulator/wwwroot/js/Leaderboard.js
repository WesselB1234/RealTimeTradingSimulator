

function onMultiOwnershipLabelsUpdate() {

    let tableRows = document.querySelectorAll(".leaderboard-table .TotalOwnershipValue");
    let ownershipListOrdered = {};

    tableRows.forEach((tableRow) => {
        console.log(tableRow.dataset.userId);
        console.log(tableRow.dataset.price);
    })
}