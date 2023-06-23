// Attach an event handler to the 'submit' event of all forms
$('form').submit(function (e) {
    // Find the '__RequestVerificationToken' input element within the form
    var tokenInput = $(this).find('input[name="__RequestVerificationToken"]');

    // Check if the token input exists
    if (tokenInput.length > 0) {
        // Get the token value
        var tokenValue = tokenInput.val();

        // Append the token as a data parameter to the form submission
        $(this).append('<input type="hidden" name="__RequestVerificationToken" value="' + tokenValue + '">');
    } else {
        // Generate a new token dynamically
        var newToken = generateNewToken(); // Implement your logic to generate a new token

        // Append the new token as a hidden input to the form
        $(this).append('<input type="hidden" name="__RequestVerificationToken" value="' + newToken + '">');
    }
});

function generateNewToken() {
    // Generate a random string for the token
    var token = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var length = 64;

    for (var i = 0; i < length; i++) {
        token += characters.charAt(Math.floor(Math.random() * characters.length));
    }

    return token;
}
