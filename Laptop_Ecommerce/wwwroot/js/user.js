var dataTable;
$(document).ready(function () {
    loadDataTable();

})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "17%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "email", "width": "17%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "phoneNumber", "width": "17%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "company.name", "width": "17%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "role", "width": "17%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            {
                "data": {
                    "data": "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockOut = new Date(data.lockoutEnd).getTime();
                    if (lockOut > today) {
                        //user is locked
                        return `
                        <div class="text-center">
                        <a class="btn btn-danger"onclick=LockUnlock('${data.id}')>
                       <i class="fas fa-unlock"></i>
                        </a>
                        </div>
                        `;
                    } else {
                        //user is unlocked
                        return `
                        <div class="text-center">
                        <a class="btn btn-success"onclick=LockUnlock('${data.id}')>
                        <i class="fas fa-user-lock"></i>
                        </a>
                        </div>
                        `;
                    }
                }
            }
        ]
    })
}
function LockUnlock(id) {
    //alert(id);
    $.ajax({
        url: "/Admin/User/LockUnlock",
        type: "Post",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
}