// Attach an event handler to the 'submit' event of all forms
$("form").submit(function (e) {
    if (isTokenExpired()) {
        handleTokenExpiration();
    }
});

// Set up default AJAX settings
$.ajaxSetup({
    beforeSend: function (xhr, settings) {
        if (!settings.crossDomain) {
            // Get the CSRF token value
            var token = $('input[name="__RequestVerificationToken"]').val();

            // Add the CSRF token header to the AJAX request
            xhr.setRequestHeader("X-CSRF-TOKEN", token);
        }
    },
});

function isTokenExpired() {
    var token = localStorage.getItem("authToken");
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
    var base64Url = token.split(".")[1];
    var base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    var jsonPayload = decodeURIComponent(
        atob(base64)
            .split("")
            .map(function (c) {
                return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
            })
            .join("")
    );

    return JSON.parse(jsonPayload);
}

function handleTokenExpiration() {
    $.ajax({
        url: "auth/HandleTokenExpired",
        type: "GET",
        success: function (response) {
            if (response.successful) {
                $.ajax({
                    url: "auth/CreateJwtToken",
                    type: "GET",
                    success: function (response) {
                        if (response.successful) {
                            localStorage.removeItem("authToken");
                            localStorage.setItem("authToken", response.jwtAuthToken);

                            // Perform the original action after obtaining a new token
                            if (window.originalAction) {
                                window.originalAction();
                            }
                        } else {
                            Swal.fire({
                                icon: "error",
                                title: "New JwtToken generation failed",
                                text: "An error occurred while generating a new token.",
                                confirmButtonText: "Okay",
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    console.log("New Jwt Confirmed!");
                                }
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        Swal.fire({
                            icon: "error",
                            title: "Request Failed",
                            text: "An error occurred while communicating with the server.",
                        });
                    },
                });
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Something went wrong",
                    text: "An error occurred during registration.",
                });
            }
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: "error",
                title: "AJAX request failed",
                text: "An error occurred while communicating with the server.",
            });
        },
    });
}
