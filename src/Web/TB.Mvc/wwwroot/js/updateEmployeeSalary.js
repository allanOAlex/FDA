$(function () {
    var Id;
    var empName;
    var oldSalary;
    var updatedProperties = {};

    $(document).on('click', '.edit-employee', function () {
        Id = $(this).data('emp-id');
        $('#empID').val(Id);

        oldSalary = $(this).closest('tr').find('.salary').text();
        empName = $(this).closest('tr').find('.name').text();
        $('#newSalary').text(oldSalary);
        $('#newSalary').val(oldSalary);

        

    });

    $('#updateEmpSalaryForm').on('submit', function (e) {
        e.preventDefault();

        var formData = $(this).serializeArray();

        $.ajax({
            url: 'employee/UpdateEmployeeSalary',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.successful) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Salary updated successfully from ' + response.oldSalary + ' to ' + response.newSalary,
                        icon: 'success'
                    }).then(function () {
                        resetForm('updateEmpSalaryForm');
                        closeModal('updateEmpSalaryModal');

                        updatedProperties = {
                            'name': empName,
                            'salary': response.newSalary
                        };

                        // Iterate over each cell in the current row and store the property values
                        $(this).closest('tr').find('td').each(function () {
                            var propertyName = $(this).attr('class');
                            var propertyValue = $(this).text();
                            updatedProperties[propertyName] = propertyValue;
                        });
                        
                        // Update the corresponding table row with the updated properties
                        var row = $('tr[data-emp-id="' + Id + '"]');
                        for (var prop in updatedProperties) {
                            if (updatedProperties.hasOwnProperty(prop)) {
                                row.find('.' + prop).text(updatedProperties[prop]);
                            }
                        }

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



