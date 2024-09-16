// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// venda-validation.js
// venda-validation.js

function validateVendaForm() {
    const selectedProducts = document.querySelectorAll('input[name="selectedProducts"]:checked');
    if (selectedProducts.length === 0) {
        document.getElementById('error-message').innerText = 'Selecione ao menos um produto para finalizar a venda.';
        return false;
    }

    const formaPagamento = document.querySelector('select[name="formaPagamento"]').value;
    if (!formaPagamento) {
        document.getElementById('error-message').innerText = 'Informe a forma de pagamento.';
        return false;
    }


    return true;
}


/*
function validateVendaForm() {
    const selectedProducts = document.querySelectorAll('input[name="selectedProducts"]:checked');
    if (selectedProducts.length === 0) {
        document.getElementById('error-message').innerText = 'Selecione ao menos um produto para finalizar a venda.';
        return false;
    }

    const formaPagamento = document.querySelector('select[name="formaPagamento"]').value;
    if (!formaPagamento) {
        document.getElementById('error-message').innerText = 'Informe a forma de pagamento.';
        return false;
    }

    const nomeCliente = document.querySelector('input[name="nomeCliente"]').value.trim();

    const customerExists = document.getElementById('customerExists').value === 'true';
 

    if (nomeCliente && customerExists == 'true') {
        const userConfirmed = confirm("Cliente não encontrado. Deseja cadastrar este cliente?");
        if (userConfirmed) {
     
           // window.location.href = '/Clientes/Create?nome=' + encodeURIComponent(nomeCliente);
            window.location.href = '/Clientes/Create?returnUrl=' + encodeURIComponent(window.location.href);
            return false; 
        } else {
             return true; 
        }
    }

    return true; 
}


*/