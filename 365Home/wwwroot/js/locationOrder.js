var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDataTable("GetAllLocationsApprovedOrders");
    }
    else {
        if (url.includes("pending")) {
            loadDataTable("GetAllLocationsPendingOrders");
        }
        else {
            loadDataTable("GetAllLocationsOrders");
        }
    }
});

function loadDataTable(url) {
    var status; 
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/order/" + url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "location.name", "width": "10%" },
            { "data": "price", "width": "10%" },
            {
                "data": "rentTimeStartDate",
                "render": function (data) {
                    return moment(data).format("MM-DD-YYYY");
                }, "width": "10%"
            },
            {
                "data": "rentTimeEndDate",
                "render": function (data) {
                    return moment(data).format("MM-DD-YYYY");
                }, "width": "10%"
            },
            {
                "data": "status",
                "render": function (data) {
                    status = data;
                    return data;
                },"width": "15%"
            },
            { "data": "orderHeader.comments", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    if (status == "Submitted") {
                        return `<div class="text-center">
                                <a onclick=Approve('/admin/order/ApproveOrderDetails/'+${data}) class='btn btn-success text-white' style='cursor:pointer;'>
                                    <i class='far fa-edit'></i> Approve 
                                </a>
                                <a onclick=Reject('/admin/order/RejectOrderDetails/'+${data}) class='btn btn-danger text-white' style='cursor:pointer;'>
                                    <i class='far fa-edit'></i> Reject 
                                </a>
                            </div>
                            
                            `;
                    } else {
                        return '';
                    }
                    
                }, "width":"25%"
            }
        ],
        "language": {
            "emptyTable":"No records found."
        },
        "width":"100%"
    })
}

function Approve(url) {
    swal({
        title: "Approve this booking?",
        text: "This location status will be change to booked.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#32a836",
        confirmButtonText: "Approve",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'POST',
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

function Reject(url) {
    swal({
        title: "Reject this booking?",
        text: "This location status will be change to vacant.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Reject",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'POST',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.error(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}