var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax":
        {
                "url":"/Admin/Company/GetAll"
        },
        "columns": [
            {"data":"name","widht":"15%"},
            {"data":"streetAddress","widht":"15%"},
            {"data":"city","widht":"15%"},
            {"data":"state","widht":"15%"},
            { "data": "phoneNumber", "widht": "15%" },
            {
                "data": "isAuthorizedCompany",
                "render": function (data) {
                    if (data)
                        return `<input type="checkbox" checked disabled/>`;
                    else
                      return  `<input type="checkbox" disabled/>`;
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
<div class="text-center">
<a href="/Admin/Company/Upsert/${data}" class="btn btn-info">
<i class="fas fa-edit"></i>
</a>
<a class="btn btn-danger" onclick=Delete("/Admin/Company/Delete/${data}")>
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
    swal({
        title: "WantToDeleteData?",
        text: "Delete Information",
        buttons: true,
        icon: "success",
        dangerModel: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.success(data.message);
                    }
                }
            })
        }
    })
}