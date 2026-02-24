document.addEventListener("DOMContentLoaded", function () {
    let password = document.getElementById("password");
    let confirmPassword = document.getElementById("confirmPassword");
    let errorLabel = document.getElementById("passwordError");
    let submitButton = document.getElementById("submitButton");

    function validarPassword() {
        if (password.value !== confirmPassword.value) {
            errorLabel.classList.remove("d-none");
            submitButton.disabled = true;
        } else {
            errorLabel.classList.add("d-none");
            submitButton.disabled = false;
        }
    }

    password.addEventListener("input", validarPassword);
    confirmPassword.addEventListener("input", validarPassword);
});