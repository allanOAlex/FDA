function resetForm() {
    document.getElementById("createUserForm").reset();
}

function resetCreateEmployeeForm() {
    document.getElementById("createEmployeeForm").reset();
}

function resetUpdateEmployeeSalaryForm() {
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
}

function resetAndCloseCreateEmployeeModal() {
    resetCreateEmployeeForm();
    new bootstrap.Modal(document.getElementById("createEmployeeModal")).hide();
}

function resetAndCloseUpdateEmployeeSalaryModal() {
    resetUpdateEmployeeSalaryForm();
    new bootstrap.Modal(document.getElementById("updateEmpSalaryModal")).hide();
}

function openModal() {
    new bootstrap.Modal(document.getElementById('createUserModal')).show();
}

function openCreateEmployeeModal() {
    new bootstrap.Modal(document.getElementById('createEmployeeModal')).show();
}

function openUpdateEmpSalaryModal() {
    new bootstrap.Modal(document.getElementById('updateEmpSalaryModal')).show();
}