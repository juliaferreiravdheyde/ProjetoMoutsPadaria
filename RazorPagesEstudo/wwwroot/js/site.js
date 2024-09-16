// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function validateVendaForm() {
    // Check if any product is selected
    const selectedProducts = document.querySelectorAll('input[name="selectedProducts"]:checked');
    if (selectedProducts.length === 0) {
        document.getElementById('error-message').innerText = 'Selecione ao menos um produto para finalizar a venda.';
        return false;  // Prevent form submission
    }

    // Check if a payment method is selected
    const formaPagamento = document.querySelector('select[name="formaPagamento"]').value;
    if (!formaPagamento) {
        document.getElementById('error-message').innerText = 'Informe a forma de pagamento.';
        return false;  // Prevent form submission
    }

    return true;  // Allow form submission
}
