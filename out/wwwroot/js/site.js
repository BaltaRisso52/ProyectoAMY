// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const images = document.querySelectorAll('.carousel-inner img');
let index = 0;

function showImage(i) {
    images.forEach(img => img.classList.remove('active'));
    images[i].classList.add('active');
}

document.querySelector('.next').addEventListener('click', () => {
    index = (index + 1) % images.length;
    showImage(index);
});

document.querySelector('.prev').addEventListener('click', () => {
    index = (index - 1 + images.length) % images.length;
    showImage(index);
});

// Auto Slide
setInterval(() => {
    index = (index + 1) % images.length;
    showImage(index);
}, 4000); // cada 4 segundos