﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/customer/useraddress/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "addressName", "width": "20%" },
            { "data": "phoneNumber", "width": "20%" },
            { "data": "address", "width": "20%" },
            { "data": "isDefaultAddress", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/customer/useraddress/Upsert/${data}" class='btn btn-success text-white' style='cursor:pointer, width:100px;'>
                                    <i class='far fa-edit'></i>Edit 
                                </a>
                                &nbsp;
                                <a onclick="Delete('/customer/useraddress/Delete/${data}')" class='btn btn-danger text-white' style='cursor:pointer, width:100px;'>
                                    <i class='far fa-trash-alt'></i>Delete
                                </a>
                            </div>
                            
                            `;
                }, "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "No records found."
        },
        "width": "100%"
    })
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "Confirm?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD66B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: "DELETE",
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

function ShowMessage(msg) {
    toastr.success(msg);
}