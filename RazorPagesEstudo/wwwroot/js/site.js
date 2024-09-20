// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// venda-validation.js
// venda-validation.js


$(document).ready(function () {
    var errors = $('#error-message').find('div').length;
    if (errors > 0) {
        $('#error-message').show();
    }
});

document.querySelectorAll('input[name="selectedProducts"]').forEach((checkbox) => {
    checkbox.addEventListener('change', function () {
        const quantityInput = document.querySelector(`input[data-produto-id="${this.value}"]`);
        quantityInput.disabled = !this.checked;  // Disable quantity if product is not selected
    });
});


function validateVendaForm() {
  /*  const selectedProducts = document.querySelectorAll('input[name="selectedProducts"]:checked');
    if (selectedProducts.length === 0) {
        document.getElementById('error-message').innerText = 'Selecione ao menos um produto para finalizar a venda.';
        return false;
    }

    const formaPagamento = document.querySelector('select[name="formaPagamento"]').value;
    if (!formaPagamento) {
        document.getElementById('error-message').innerText = 'Informe a forma de pagamento.';
        return false;
    }
*/

    return true;
}