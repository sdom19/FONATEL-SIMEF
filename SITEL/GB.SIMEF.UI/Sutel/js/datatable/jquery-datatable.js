jQuery(function () {
    jQuery('.basic-example').DataTable();
    //Exportable table
    jQuery('.exportable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
});

jQuery(document).ready(function() {
    jQuery('#datatable').dataTable();
    jQuery('#datatable-keytable').DataTable({
        keys: true
    });
    jQuery('#datatable-responsive').DataTable();
    jQuery('#datatable-scroller').DataTable({
        ajax: "assets/js/datatables/json/scroller-demo.json",
        deferRender: true,
        scrollY: 380,
        scrollCollapse: true,
        scroller: true
    });
    var table = jQuery('#datatable-fixed-header').DataTable({
        fixedHeader: true
    });
});
// TableManageButtons.init();
