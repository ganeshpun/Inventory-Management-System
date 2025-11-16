document.getElementById('supplierForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const fromDate = document.getElementById('from').value;
    const toDate = document.getElementById('to').value;
    const supplier = document.getElementById('supplier').value;

    const listBody = document.getElementById('listBody');

    const newRow = document.createElement('tr');
    newRow.innerHTML = `
        <td>${fromDate} - ${toDate}</td>
        <td>${supplier}</td>
        <td>Item</td>
    `;

    listBody.appendChild(newRow);
});


function FetchPurchaseList() {
    debugger;

    var inputData = {
        From: $("#from").val(),
        To: $("#to").val(),
        SupplierId: $("#supplier").val()
    };

    $.ajax({
        type: 'POST',
        url: '/Report/PurchaseReport',
        data: JSON.stringify(inputData),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            // Redirect to the Index action of License controller

            debugger;
            console.log(response);
            $("#listBody").empty();

            // Iterate over the response data and add rows to the table
            response.forEach(function (item) {
                var row = "<tr>" +
                    "<td>" + new Date(item.date).toLocaleDateString() + "</td>" +
                    "<td>" + item.supplierName + "</td>" +
                    "<td>" + item.productName + "</td>" +
                    "<td>" + item.count + "</td>" +
                    "<td>" + item.totalCost + "</td>" +
                    "</tr>";
                $("#listBody").append(row);
            });
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error(error);
        }
    });
}
function ResetForm() {
    //debugger;
    //$("#listSalesBody").empty();
    //$("#listBody").empty();
    //supplierForm.reset();
    //supplier.reset();

    window.location.reload(true);

}
function FetchSalesList() {
    debugger;

    var inputData = {
        From: $("#fromDate").val(),
        To: $("#toDate").val()
    };

    $.ajax({
        type: 'POST',
        url: '/Report/SalesReport',
        data: JSON.stringify(inputData),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            // Redirect to the Index action of License controller

            debugger;
            console.log(response);
            $("#listSalesBody").empty();

            // Iterate over the response data and add rows to the table
            response.forEach(function (item) {
                var row = "<tr>" +
                    "<td>" + new Date(item.salesDate).toLocaleDateString() + "</td>" +
                    "<td>" + item.customerName + "</td>" +
                    "<td>" + item.productName + "</td>" +
                    "<td>" + item.quantity + "</td>" +
                    "<td>" + item.perPrice + "</td>" +
                    "<td>" + item.totalPrice + "</td>" +
                    "</tr>";
                $("#listSalesBody").append(row);
            });
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error(error);
        }
    });
}
function FetchPLList() {
    debugger;

    var inputData = {
        From: $("#PLfromDate").val(),
        To: $("#PLtoDate").val(),
        SupplierId: $("#PLproducts").val(),
    };
    if (inputData.SupplierId===null) {
        inputData.SupplierId = 0;
    }
    var grandTotal = 0;
    $.ajax({
        type: 'POST',
        url: '/Report/ProfitLossReport',
        data: JSON.stringify(inputData),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            // Redirect to the Index action of License controller

            debugger;
            console.log(response);
            $("#listSalesBody").empty();

            // Iterate over the response data and add rows to the table
            response.forEach(function (item) {
                grandTotal = grandTotal + item.totalCost;
                var row = "<tr>" +
                    "<td>" + new Date(item.date).toLocaleDateString() + "</td>" +
                    "<td>" + item.productName + "</td>" +
                    "<td>" + item.count + "</td>" +
                    "<td>" + item.totalCost + "</td>" +
                    "</tr>";
                $("#listPLBody").append(row);
                document.getElementById('grandPLTotal').textContent = `Rs. ${grandTotal.toFixed(2)}`;
                $('#grandPLTotal').val(grandTotal.toFixed(2))
            });
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error(error);
        }
    });
}
