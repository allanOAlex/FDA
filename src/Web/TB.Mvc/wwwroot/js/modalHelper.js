function resetForm() {
    document.getElementById("createUserForm").reset();
}


function resetForgotPasswordForm() {
    document.getElementById("forgotPasswordForm").reset();
}

function resetPasswordResetForm() {
    document.getElementById("passwordResetForm").reset();
}


function resetAndCloseModal() {
    resetForm();
    new bootstrap.Modal(document.getElementById("createUserModal")).hide();
}

function openModal() {
    new bootstrap.Modal(document.getElementById('createUserModal')).show();
}