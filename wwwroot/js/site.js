// Script de utilidades cliente para BiblioCampus (ASP.NET Core MVC)
document.addEventListener("DOMContentLoaded", function () {
    // 1. Menú responsive móvil
    const btnMenu = document.getElementById("btnMenuMovil");
    const menuMovil = document.getElementById("menuMovil");
    if (btnMenu && menuMovil) {
        btnMenu.addEventListener("click", function () {
            menuMovil.classList.toggle("hidden");
        });
    }

    // 2. Filtro de búsqueda en vivo sobre tablas CRUD (atributo data-filtro-tabla)
    const inputsFiltro = document.querySelectorAll("[data-filtro-tabla]");
    inputsFiltro.forEach(function (input) {
        const tablaId = input.getAttribute("data-filtro-tabla");
        const tabla = document.getElementById(tablaId);
        if (!tabla) return;

        input.addEventListener("input", function () {
            const termino = input.value.trim().toLowerCase();
            const filas = tabla.querySelectorAll("tbody tr[data-fila-registro]");
            let visibles = 0;

            filas.forEach(function (fila) {
                const texto = fila.textContent.toLowerCase();
                const coincide = texto.includes(termino);
                fila.classList.toggle("hidden", !coincide);
                if (coincide) visibles++;
            });

            const filaVacia = tabla.querySelector("tbody tr[data-fila-sin-resultados]");
            if (filaVacia) {
                filaVacia.classList.toggle("hidden", visibles > 0);
            }
        });
    });
});
