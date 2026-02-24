document.addEventListener("DOMContentLoaded", function () {
    let newPassword = document.getElementById("newPassword");
    let confirmPassword = document.getElementById("confirmPassword");
    let errorLabel = document.getElementById("passwordError");
    let submitButton = document.getElementById("submitButton");

    function validarPassword() {
        if (newPassword.value !== confirmPassword.value) {
            errorLabel.classList.remove("d-none");
            submitButton.disabled = true;
        } else {
            errorLabel.classList.add("d-none");
            submitButton.disabled = false;
        }
    }

    newPassword.addEventListener("input", validarPassword);
    confirmPassword.addEventListener("input", validarPassword);
});