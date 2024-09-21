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
                        <input type="number" name="quantidades" class="form-control quantity" value="1" min="1" />
                        <button type="button" class="btn btn-danger btn-sm ms-2 remove-product">
                            <i class="fas fa-trash"></i> <!-- Font Awesome trash icon -->
                        </button>
                        <input type="hidden" name="selectedProducts" value="${selectedProductId}" />
                    </div>
                `);

    totalQuantity += 1;
    totalPrice += selectedProductPrice;
            }

    $('#totalQuantity').text(totalQuantity);
    $('#totalPrice').text(totalPrice.toFixed(2));
        });

    $('#produtoList').on('change', '.quantity', function () {
            var quantityChange = parseInt($(this).val());
    var productId = $(this).closest('[data-product-id]').data('product-id');
    var productPrice = parseFloat($("#produtoSelect option[value='" + productId + "']").data("preco"));

    var currentTotal = totalPrice - (parseInt($(this).attr('data-old-quantity')) * productPrice);
    totalPrice = currentTotal + (quantityChange * productPrice);

    $(this).attr('data-old-quantity', quantityChange);

            totalQuantity = $('#produtoList').find('.quantity').toArray().reduce((total, input) => total + parseInt($(input).val()), 0);
                            $('#totalQuantity').text(totalQuantity);
                            $('#totalPrice').text(totalPrice.toFixed(2));
                            });

    $('#produtoList').on('click', '.remove-product', function () {
            var quantityInput = $(this).siblings('.quantity');
            var quantityToRemove = parseInt(quantityInput.val());
            var productPrice = parseFloat($("#produtoSelect option[value='" + $(this).closest('[data-product-id]').data('product-id') + "']").data("preco"));

    totalQuantity -= quantityToRemove;
    totalPrice -= (quantityToRemove * productPrice);

    $(this).closest('[data-product-id]').remove();

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
