function openModal() {
    var modal = document.getElementById(modalId);
    if (modal) {
        new bootstrap.Modal(modal).show();
    }
}

function resetForm(formId) {
    var form = document.getElementById(formId);
    if (form) {
        form.reset();
    }
}
function closeModal(modalId) {
    
    new bootstrap.Modal(document.getElementById(modalId)).hide();
}




function resetAndCloseModal(modalId, formId) {
    resetForm(formId);
    var openModal = document.getElementById(modalId);
    var modal = new bootstrap.Modal(openModal)
    if (modal) {
        modal.hide();
        
        

    }
}





