
$('#lockScreenForm').submit(function (e) {
    e.preventDefault();

    unlockScreen();

})

function unlockScreen() {
    $.ajax({
        url: 'auth/unlockscreen',
        type: 'GET',
        success: function (response) {
            if (response.successful) {
                removeOverlay();
                Swal.fire('Success!', 'Welcome back!.', 'success');
            } else {
                Swal.fire('Error!', 'We could not unlock your screen. Please confirm that the provided password is correct.', 'error');
            }
        },
        error: function () {
            alert('An error occurred during operation.');
        }
    });
}

function removeOverlay() {
    $('#lockScreenOverlay').hide();
}
