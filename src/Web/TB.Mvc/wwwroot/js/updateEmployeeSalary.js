
$('#updateEmpSalaryForm').submit(function (e) {
    e.preventDefault();

    if (isTokenExpired()) {
        handleTokenExpiration();
    } else {
        unlockScreen();
    }

})

function isTokenExpired() {
    var token = localStorage.getItem('authToken');
    var expirationDate = getTokenExpirationDate(token);
    var currentDateTime = new Date();
    return expirationDate < currentDateTime;
}

function getTokenExpirationDate(token) {
    var tokenData = parseJwt(token);
    var expirationDate = new Date(tokenData.exp * 1000); // Convert to milliseconds
    return expirationDate;
}

function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

function handleTokenExpiration() {
    $.ajax({
        url: 'auth/HandleTokenExpired',
        type: 'GET',
        success: function (response) {
            if (response.successful) {
                $.ajax({
                    url: 'auth/CreateJwtToken',
                    type: 'GET',
                    success: function (response) {
                        console.log(response)
                        if (response.successful) {
                            localStorage.removeItem('authToken');
                            localStorage.setItem('authToken', response.jwtAuthToken);
                            updateEmployeeSalary();
                            Swal.fire({
                                icon: 'success',
                                title: 'New Jwt Success!',
                                showCancelButton: true,
                                confirmButtonText: 'Okay',
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    console.log("New Jwt Confirmed!")
                                }
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'New JwtToken generation failed',
                                text: 'An error occurred while generating a new token.',
                                confirmButtonText: 'Okay',
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    console.log("New Jwt Confirmed!")
                                }
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        Swal.fire({
                            icon: 'error',
                            title: 'AJAX request failed',
                            text: 'An error occurred while communicating with the server.',
                        });
                    }
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Somewthing went wrong',
                    text: 'An error occurred during registration.',
                });
            }
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: 'error',
                title: 'AJAX request failed',
                text: 'An error occurred while communicating with the server.',
            });
        }
    });

}

function updateEmployeeSalary() {
    $.ajax({
        url: 'employee/updateemployeeSalary',
        type: 'POST',
        data: $(this).serialize(),
        success: function (response) {
            if (response.successful) {
                Swal.fire('Success!', 'Salary updated successfully!.', 'success');
            } else {
                Swal.fire('Error!', 'Salary update failes. Please contact system admin.', 'error');
            }
        },
        error: function () {
            alert('An error occurred during operation.');
        }
    });
}



$("#updateEmpSalaryForm").submit(function (e) {
    e.preventDefault();

    $.ajax({
        url: '@Url.Action("UpdateEmployeeSalary", "Employee")',
        type: 'POST',
        data: $(this).serialize(),
        success: function (response, textStatus, xhr) {
            if (xhr.getResponseHeader('TokenExpired') === 'true') {
                // Token has expired, initiate token refresh process or handle as needed
                // Call your refresh token endpoint or perform any other necessary actions
            } else if (response.success) {
                resetForm();
                Swal.fire({
                    icon: 'success',
                    title: 'Registration successful!',
                    showCancelButton: true,
                    confirmButtonText: 'Okay',
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = '@Url.Action("LoginView", "Auth")';
                    }
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Registration failed',
                    text: 'An error occurred during registration.',
                });
            }
        },
        error: function (xhr, status, error) {
            if (xhr.getResponseHeader('TokenExpired') === 'true') {
                // Token has expired, initiate token refresh process or handle as needed
                // Call your refresh token endpoint or perform any other necessary actions
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'AJAX request failed',
                    text: 'An error occurred while communicating with the server.',
                });
            }
        }
    });
});