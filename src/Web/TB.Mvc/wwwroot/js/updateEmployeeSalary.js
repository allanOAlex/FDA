$(document).ready(function () {

    var Id;

    $('[data-target="#updateEmpSalaryModal"]').click(function () {
        Id = $(this).data('emp-id');
    });


    $('#updateEmpSalaryForm').submit(function (e) {
        e.preventDefault();

        $('#empID').val(Id);

        var employeeID = $('#empID').val();
        var Salary = $('#newSalary').val();
        
        $.ajax({
            url: 'employee/UpdateEmployeeSalary',
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                if (response.successful) {
                    resetAndCloseUpdateEmployeeSalaryModal()

                    Swal.fire('Success!', 'Salary updated successfully from ' + response.oldSalary + ' to ' + response.newSalary, 'success');

                } else {
                    Swal.fire('Error!', 'Salary update failes. Please contact system admin.', 'error');
                }
            },
            error: function () {
                Swal.fire('Request Failed!', 'Please contact system admin.', 'error');
            }
        });

    })


});
    
