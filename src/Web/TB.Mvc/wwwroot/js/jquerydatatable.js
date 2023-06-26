$(document).ready(function () {
    $('.dataTable').DataTable({
        "scrollY": 300,
        "scrollX": false,
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "pagingType": "full",
        "processing": true,
        "autoWidth": true,
        fixedHeader: true
    });
});