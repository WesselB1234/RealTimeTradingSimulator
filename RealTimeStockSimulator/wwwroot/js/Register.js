const passwordInput = document.getElementById("password");
const repeatPasswordInput = document.getElementById("repeat_password");

function OnRegister() {

    if (passwordInput.value !== repeatPasswordInput.value) {

        alert("Password is not equal to the repeated password.");

        return false;
    }

    return true;
}