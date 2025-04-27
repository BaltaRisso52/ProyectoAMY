// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', () => {
    const images = document.querySelectorAll('.carousel-inner img');
    if (images.length === 0) return;

    let index = 0;

    function showImage(i) {
        images.forEach(img => img.classList.remove('active'));
        images[i].classList.add('active');
    }

    document.querySelector('.next')?.addEventListener('click', () => {
        index = (index + 1) % images.length;
        showImage(index);
    });

    document.querySelector('.prev')?.addEventListener('click', () => {
        index = (index - 1 + images.length) % images.length;
        showImage(index);
    });

    // Auto Slide
    setInterval(() => {
        index = (index + 1) % images.length;
        showImage(index);
    }, 4000);
});

// //////////////////////////////////////////////////////////////////////////
// logica para sumar y restar productos del carrito

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".btn-mas, .btn-menos").forEach(btn => {
        btn.addEventListener("click", function () {
            const row = btn.closest("section");
            const productoId = row.getAttribute("data-producto-id");
            const cantidadEl = row.querySelector(".cantidad");
            let cantidad = parseInt(cantidadEl.textContent);

            if (btn.classList.contains("btn-mas")) cantidad++;
            if (btn.classList.contains("btn-menos") && cantidad > 1) cantidad--;

            fetch("/Carrito/ActualizarCantidad", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ productoId, cantidad })
            })
                .then(res => res.json())
                .then(data => {
                    cantidadEl.textContent = data.nuevaCantidad;
                    row.querySelector(".subtotal").textContent = `$${data.subtotal}`;
                    document.getElementById("total").textContent = data.total;
                })
                .catch(error => console.error("Error al actualizar cantidad:", error));
        });
    });
});

///////////////////////////////////////////////////////////////////////////
// logica para agregar al carrito

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.btn-agregar-carrito').forEach(button => {
        button.addEventListener('click', function () {
            const productoId = this.getAttribute('data-id');

            fetch('/Carrito/Agregar', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ productoId: parseInt(productoId) })
            })
                .then(res => res.json())
                .then(data => {
                    console.log("Producto agregado al carrito:", data);
                    if (data.ok) {
                        alert("¡Producto agregado al carrito!");
                    } else {
                        alert("Hubo un problema al agregar al carrito.");
                    }
                })
                .catch(err => console.error("Error al agregar al carrito:", err));
        });
    });
});

/////////////////////////////////////////////////////////////////////////////
// logica para borrar producto y vaciar carrito

$(document).ready(function () {
    // Eliminar un producto
    $(".btn-eliminar-producto").click(function () {
        var productoId = $(this).data("id");

        $.ajax({
            url: '/Carrito/EliminarProductoAjax',
            type: 'POST',
            data: { productoId: productoId },
            success: function (response) {
                if (response.success) {
                    // Eliminamos visualmente la fila
                    $('section[data-producto-id="' + productoId + '"]').remove();
                    location.reload();
                } else {
                    alert(response.message || "Error al eliminar el producto.");
                }
            }
        });
    });

    // Vaciar todo el carrito
    $(".btn-vaciar-carrito").click(function () {
        $.ajax({
            url: '/Carrito/VaciarCarritoAjax',
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    // Podés recargar o limpiar la tabla
                    location.reload();
                }
            }
        });
    });
});

/////////////////////////////////////////////////////
// logica para el enter de la descripcion

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.descripcion-producto').forEach(function (element) {
        element.innerHTML = element.innerHTML.replace(/\n/g, '<br>');
    });
});