const passwordInput = document.getElementById("password");
const repeatPasswordInput = document.getElementById("repeat_password");

function onRegister() {

    if (passwordInput.value !== repeatPasswordInput.value) {

        alert("Password is not equal to repeated password.");

        return false;
    }

    return true;
}