var dataTable;
$(document).ready(function () {
    loadDataTable();

})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Laptop/GetAll"
        },
        "columns": [
                { "data": "laptopCompany.name", "width": "12%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
                { "data": "modelName", "width": "12%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
                { "data": "processor.name", "width": "12%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
                { "data": "graphicsCard.name", "width": "12%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
                { "data": "ramMemory", "width": "12%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
                { "data": "storageSpace", "width": "12%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
                { "data": "price", "width": "12%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            {
                "data": "id", "render": function (data) {
                    return `
                    <div class="text-center">
                    <a href="/Admin/Laptop/Upsert/${data}"class="btn btn-warning">
                    <i class="fas fa-edit"></i>
                    </a>
                    <a class="btn btn-danger"onclick=Delete("/Admin/Laptop/Delete/${data}")>
                    <i class="fas fa-trash-alt"></i>
                    </a>
                    </div>
                    `;
                }
            }
        ]
    })
}
function Delete(url) {
    //alert(url);
    swal({
        title: "Want To Delete Data?",
        text: "All The Information Will Be Deleted!!!",
        buttons: true,
        icon: "warning",
        dangerModel: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "delete",
                success: function (data) {
                    if (data.success) {
                        /* toastr.success(data.message);*/
                        swal({
                            title: "Data Deleted Successfully!!!",
                            icon: "success",
                            dangerModel: true
                        })
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}