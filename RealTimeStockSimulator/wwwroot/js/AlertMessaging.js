function displayErrorAlert(messageStr) {

    let errorAlert = document.getElementById("errorAlert");

    if (errorAlert === null) {
        throw new Error("Error alert container doesn't exist yet.");
    }

    let errorMessageHolder = document.getElementById("errorMessageHolder");

    if (errorMessageHolder === null) {
        throw new Error("error message holder doesn't exist yet.");
    }

    errorAlert.classList.remove("d-none");
    errorMessageHolder.innerText = messageStr;
}

function displaySuccessAlert(messageStr) {

    successAlert = document.getElementById("successAlert");

    if (successAlert === null) {
        throw new Error("Success alert container doesn't exist yet.");
    }

    successAlert.classList.remove("d-none");
    successAlert.innerText = messageStr;
}