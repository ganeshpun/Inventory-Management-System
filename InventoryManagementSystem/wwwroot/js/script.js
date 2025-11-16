let customers = [];
let currentCustomerIndex = -1;

document.getElementById('price').addEventListener('input', function () {
    const quantity = document.getElementById('quantity').value;
    const price = this.value;
    document.getElementById('totalPrice').value = quantity * price;
});

document.getElementById('quantity').addEventListener('input', function () {
    const price = document.getElementById('price').value;
    const quantity = this.value;
    document.getElementById('totalPrice').value = quantity * price;
});

function submitForm() {

    const customerForm = document.getElementById('customerForm');
    const productForm = document.getElementById('productForm');

    const customer = {
        date: customerForm.date.value,
        customerName: customerForm.customerName.value,
        mobile: customerForm.mobile.value,
        email: customerForm.email.value,
        product: productForm.product.value,
        quantity: productForm.quantity.value,
        price: productForm.price.value,
        totalPrice: productForm.totalPrice.value
    };

    if (currentCustomerIndex >= 0) {
        customers[currentCustomerIndex] = customer;
        currentCustomerIndex = -1;
    } else {
        customers.push(customer);
    }

    updateDashboard();
    // customerForm.reset();
    productForm.reset();
}

function updateDashboard() {
    const dashboardBody = document.getElementById('dashboardBody');
    dashboardBody.innerHTML = '';

    let totalAmount = 0;
    customers.forEach((customer, index) => {
        totalAmount += parseFloat(customer.totalPrice);
        const row = `
            <tr>
                <td >${index + 1}</td>
                <td id="productField-${index}">${customer.product}</td>
                <td id="quantityField-${index}">${customer.quantity}</td>
                <td id="priceField-${index}">Rs. ${customer.price}</td>
                <td id="totalPriceField-${index}">Rs. ${customer.totalPrice}</td>
                <td class="actions">
                    <button onclick="editCustomer(${index})">Edit</button>
                    <button onclick="deleteCustomer(${index})">Delete</button>
                </td>
            </tr>
        `;
        dashboardBody.innerHTML += row;
    });

    document.getElementById('totalAmount').textContent = `Rs. ${totalAmount.toFixed(2)}`;
    calculateGrandTotal();
}

function calculateGrandTotal() {
    const totalAmount = parseFloat(document.getElementById('totalAmount').textContent.replace('Rs. ', '')) || 0;
    const vatPercentage = parseFloat(document.getElementById('vat').value) || 0;
    const vatAmount = totalAmount * (vatPercentage / 100);
    const discountPercentage = parseFloat(document.getElementById('discount').value) || 0;
    const discountAmount = totalAmount * (discountPercentage / 100);
    const grandTotal = totalAmount + vatAmount - discountAmount;
    document.getElementById('grandTotal').textContent = `Rs. ${grandTotal.toFixed(2)}`;
    $('#grandTotal').val(grandTotal.toFixed(2))
}
function getProductDataFromTable() {
    debugger;
    const products = [];
    const rows = dashboardBody.querySelectorAll('tr');

    rows.forEach((row, index) => {
        const productCell = row.querySelector(`#productField-${index}`);
        const quantityCell = row.querySelector(`#quantityField-${index}`);
        const priceCell = row.querySelector(`#priceField-${index}`);
        const totalPriceCell = row.querySelector(`#totalPriceField-${index}`);
        debugger;
        if (productCell && quantityCell && priceCell && totalPriceCell) {
            const productId = parseInt(index + 1);
            const product = productCell.textContent;
            const quantity = parseInt(quantityCell.textContent);
            const price = parseFloat(priceCell.textContent.replace('Rs. ', ''));
            //const totalPrice = parseFloat(totalPriceCell.textContent.replace('Rs. ', ''));

            products.push({
                ProductName: product,
                Quantity: quantity,
                Price: price
                //TotalPrice: totalPrice
            });
        }
    });
    return products;
}
function saveData() {
    debugger;
    getProductDataFromTable();
    var inputData = {
        SalesDate: $("#date").val(),
        CustomerName: $("#customerName").val(),
        TotalPrice: Number($("#grandTotal").val()),
        Discount: Number($("#discount").val()),
        Vat: Number($("#vat").val()),
        ProductsSold: getProductDataFromTable()
    };


    $.ajax({
        type: 'POST',
        url: '/Sales/ProductsSoldAdd',
        data: JSON.stringify(inputData),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (response) {
            // Redirect to the Index action of License controller
            debugger;
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error(error);
        }
    });

    const popupContent = document.getElementById('popupContent');
    popupContent.innerHTML = '';

    customers.forEach((customer) => {
        const row = `
            <tr>
                <td>${customer.date}</td>
                <td>${customer.customerName}</td>
                <td>${customer.mobile}</td>
                <td>${customer.email}</td>
                <td>${customer.product}</td>
                <td>${customer.quantity}</td>
                <td>Rs. ${customer.price}</td>
                <td>Rs. ${customer.totalPrice}</td>
            </tr>
        `;
        popupContent.innerHTML += row;
    });

    document.getElementById('popup').style.display = 'block';
    $
}

function closePopup() {
    document.getElementById('popup').style.display = 'none';
    debugger;
    window.location.href = '/Sales/SalesList';// redirect to sales controller's saleslist after popup

}

function editCustomer(index) {
    currentCustomerIndex = index;
    const customer = customers[index];
    const customerForm = document.getElementById('customerForm');
    const productForm = document.getElementById('productForm');

    customerForm.date.value = customer.date;
    customerForm.customerName.value = customer.customerName;
    customerForm.mobile.value = customer.mobile;
    customerForm.email.value = customer.email;
    productForm.product.value = customer.product;
    productForm.quantity.value = customer.quantity;
    productForm.price.value = customer.price;
    productForm.totalPrice.value = customer.totalPrice;
}

function deleteCustomer(index) {
    customers.splice(index, 1);
    updateDashboard();
}


