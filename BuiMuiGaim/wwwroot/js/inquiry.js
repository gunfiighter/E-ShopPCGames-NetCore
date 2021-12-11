﻿var dataTable

$(document).ready(function () {
    loadDataTable()
});

function loadDataTable(url) {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/inquiry/GetInquiryList"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "fullname", "width": "10%" },
            { "data": "phonenumber", "width": "10%" },
            { "data": "email", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Inquiry/Details/${data} class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-edit"></i>
                            </a>
                        </div>                   
                    `;
                },
                "width" : "5%"
            },
        ]
    })
}