$(document).ready(function () {
    $('.dataTable').DataTable({
        "scrollY": 400,
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "processing": true,
        "autoWidth": true,
        "fixedHeader": true
    });
});