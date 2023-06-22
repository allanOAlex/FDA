function resetForm() {
    document.getElementById("createUserForm").reset();
    document.getElementById("createEmployeeForm").reset();
    document.getElementById("updateEmpSalaryForm").reset();
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
    new bootstrap.Modal(document.getElementById("createEmployeeModal")).hide();
    new bootstrap.Modal(document.getElementById("updateEmpSalaryModal")).hide();

}

function openModal() {
    new bootstrap.Modal(document.getElementById('createUserModal')).show();
    new bootstrap.Modal(document.getElementById('createEmployeeModal')).show();
    new bootstrap.Modal(document.getElementById('updateEmpSalaryModal')).show();
}