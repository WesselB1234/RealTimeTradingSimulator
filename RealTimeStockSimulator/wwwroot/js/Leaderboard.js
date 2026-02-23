function sortOwnershipList(ownershipList)
{
    return ownershipList.sort((a, b) => b.price - a.price);
}

function onMultiOwnershipLabelsUpdate() {

    let tableRows = document.querySelectorAll(".leaderboard-table .TotalOwnershipValue");
    let ownershipList= [];

    tableRows.forEach((tableRow) => {
        ownershipList.push({
            "userId": tableRow.dataset.userId,
            "price": tableRow.dataset.price
        });
    })

    const ownershipListOrdered = sortOwnershipList(ownershipList);
    
    ownershipListOrdered.forEach((ownership, index) => {

        const totalOwnershipValueTr = document.querySelector(
            `.TotalOwnershipValue[data-user-id="${ownership.userId}"]`
        ).parentElement;
        const currentPlaceTd = totalOwnershipValueTr.getElementsByClassName("CurrentPlace")[0];

        totalOwnershipValueTr.style.order = index;
        currentPlaceTd.textContent = `#${index + 1}`;
    })
}