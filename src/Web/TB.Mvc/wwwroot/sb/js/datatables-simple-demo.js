window.addEventListener('DOMContentLoaded', event => {
    // Simple-DataTables
    // https://github.com/fiduswriter/Simple-DataTables/wiki

    //const datatablesSimple = document.getElementById('datatablesSimple');

    //if (datatablesSimple) {
    //    new simpleDatatables.DataTable(datatablesSimple);
    //}

    const datatablesSimple = document.getElementsByClassName('datatablesSimple');

    if (datatablesSimple.length > 0) {
        for (let i = 0; i < datatablesSimple.length; i++) {
            new simpleDatatables.DataTable(datatablesSimple[i]);
        }
    }


});
