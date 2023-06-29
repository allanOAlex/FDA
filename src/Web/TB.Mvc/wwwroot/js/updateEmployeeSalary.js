$(function () {
    var Id;
    var OldSalary;

    $(document).on('click', '.edit-employee', function () {
        Id = $(this).data('emp-id');
        $('#empID').val(Id);

        OldSalary = $(this).closest('tr').find('.salary-cell').text();
        $('#currentSalary').text(OldSalary);
        $('#currentSalary').val(OldSalary);
    });

    $('#updateEmpSalaryForm').on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: 'employee/UpdateEmployeeSalary',
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                if (response.successful) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Salary updated successfully from ' + response.oldSalary + ' to ' + response.newSalary,
                        icon: 'success'
                    }).then(function () {
                        resetForm('updateEmpSalaryForm');
                        closeModal('updateEmpSalaryModal');
                    });
                } else {
                    Swal.fire('Error!', 'Salary update failed. Please contact system admin.', 'error');
                }
            },
            error: function () {
                Swal.fire('Request Failed!', 'Please contact system admin.', 'error');
            }
        });
    });
});


function closeModal(modalId) {
    $('#' + modalId).modal('hide');
}



