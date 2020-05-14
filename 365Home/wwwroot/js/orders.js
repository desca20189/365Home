var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    //if (url.includes("approved")) {
    //    loadDataTable("GetAllApprovedOrders");
    //}
    //else {
    //    if (url.includes("pending")) {
    //        loadDataTable("GetAllPendingOrders");
    //    }
    //    else {
    //        loadDataTable("GetAllOrders");
    //    }
    //}
    loadDataTable("GetAllOrders");
});

function loadDataTable(url) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/order/" + url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "20%" },
            {
                "data": "orderDate",
                "render": function (data) {
                    return moment(data).format("MM-DD-YYYY");
                }, "width": "20%"
            },
            { "data": "comments", "width": "20%" },
            { "data": "applicationUser.name", "width": "20%" },

            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/admin/order/Details/${data}" class='btn btn-success text-white' style='cursor:pointer, width:100px;'>
                                    <i class='far fa-edit'></i> Details 
                                </a>
                                
                            </div>
                            
                            `;
                }, "width":"20%"
            }
        ],
        "language": {
            "emptyTable":"No records found."
        },
        "width":"100%"
    })
}
