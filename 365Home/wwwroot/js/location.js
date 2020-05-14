var dataTable;

$(document).ready(function () {
    loadDataTable();

});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/location/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "name",
                "render": function (data) {
                    return data.length > 50 ? data.substr(0, 20) + '...' : data;
                }, "width": "10%"
            },
            { "data": "locationStatus", "width": "10%" },
            { "data": "locationType.name", "width": "10%" },
            { "data": "address", "width": "10%" },
            { "data": "price", "width": "10%" },
            { "data": "applicationUser.name", "width": "10%" },
            {
                "data": "description",
                "render": function (data) {
                    return data.length > 20 ? data.substr(0, 20) + '...' : data;
                },"width" : "20%"

            },
            {
                "data": "id",
                "render": function (data) {

                    return `
                            <div class="col-9">
                                <div class="row"> 
                                    <a href="/Admin/location/Upsert/${data}" class='btn btn-success btn-block text-white' style='cursor:pointer;' >
                                        <i class='far fa-edit'></i> Edit
                                    </a>
                                    <a class='btn btn-danger text-white btn-block' style='cursor:pointer;' onclick=Delete('/admin/location/Delete/${data}')>
                                       <i class='far fa-trash-alt'></i> Delete
                                    </a>
                                </div>
                            </div>
                        `;
                }, "width": "20%"
            }


        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure want to Delete?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();

                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}