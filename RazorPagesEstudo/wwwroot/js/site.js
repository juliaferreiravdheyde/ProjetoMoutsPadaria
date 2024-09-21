// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// venda-validation.js
// venda-validation.js


document.querySelectorAll('input[name="selectedProducts"]').forEach((checkbox) => {
    checkbox.addEventListener('change', function () {
        const quantityInput = document.querySelector(`input[data-produto-id="${this.value}"]`);
        quantityInput.disabled = !this.checked;  
    });
});

$(document).ready(function () {
    var totalQuantity = 0;
    var totalPrice = 0.0;

    $('#produtoSelect').on('change', function () {
        var selectedProductId = $(this).val();
        var selectedProductName = $("#produtoSelect option:selected").text();
        var selectedProductPrice = parseFloat($("#produtoSelect option:selected").data("preco"));

        var existingProduct = $('#produtoList').find('[data-product-id="' + selectedProductId + '"]');
        if (existingProduct.length > 0) {
            var quantityInput = existingProduct.find('.quantity');
            var newQuantity = parseInt(quantityInput.val()) + 1;
            quantityInput.val(newQuantity);
            totalQuantity += 1;
            totalPrice += selectedProductPrice;
        } else {
            $('#produtoList').append(`
                <div class="form-check d-flex align-items-center mb-3" data-product-id="${selectedProductId}">
                    <span class="me-2">${selectedProductName}</span>
                    <input type="number" name="quantidades" class="form-control w-25 quantity" value="1" readonly />
                    <input type="hidden" name="selectedProducts" value="${selectedProductId}" />
                </div>
            `);
            totalQuantity += 1;
            totalPrice += selectedProductPrice;
        }

        $('#totalQuantity').text(totalQuantity);
        $('#totalPrice').text(totalPrice.toFixed(2));
    });

    $('#openModal').on('click', function () {
        $('#paymentModal').modal('show');
    });


    $('#confirmPayment').on('click', function () {

        var formaPagamento = $('#formaPagamento').val();
        var cpfCnpj = $('#cpfCnpj').val();

        $('<input>').attr({
            type: 'hidden',
            name: 'formaPagamento',
            value: formaPagamento
        }).appendTo('#vendaForm');

        $('<input>').attr({
            type: 'hidden',
            name: 'cpfCnpj',
            value: cpfCnpj
        }).appendTo('#vendaForm');

        $('#vendaForm').submit();
    });
});