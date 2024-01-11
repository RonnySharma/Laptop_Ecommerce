var dataTable;
$(document).ready(function () {
    loadDataTable();

})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "14%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "streetAddress", "width": "14%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "city", "width": "14%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "state", "width": "14%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "phoneNumber", "width": "14%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            {
                "data": "isAuthorizedCompany", "width": "14", "render": function (data) {
                    if (data) {
                        return `
                        <p class="text-center">
                        <input type="checkbox" checked disabled/>
                        </p>
                        `;
                    }
                    else {
                        return `
                        <p class="text-center">
                        <input type="checkbox" disabled/>
                        </p>
                        `;
                    }
                }
            },
            {
                "data": "id", "render": function (data) {
                    return `
                    <div class="text-center">
                    <a href="/Admin/Company/Upsert/${data}"class="btn btn-warning">
                    <i class="fas fa-edit"></i>
                    </a>
                    <a class="btn btn-danger"onclick=Delete("/Admin/Company/Delete/${data}")>
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