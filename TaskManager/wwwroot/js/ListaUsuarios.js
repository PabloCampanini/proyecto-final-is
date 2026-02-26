document.addEventListener("DOMContentLoaded", function () {
    let buscador = document.getElementById("buscador");
    let tablaAdmin = document.getElementById("tablaAdministradores");
    let tablaOp = document.getElementById("tablaOperadores");
    let tablaUnificada = document.getElementById("tablaUnificada");
    let tablaUnificadaBody = document.getElementById("tablaUnificadaBody");

    buscador.addEventListener("keyup", function () {
        let filtro = buscador.value.toLowerCase();
        let hayResultados = false;

        // Limpiar la tabla unificada antes de filtrar
        tablaUnificadaBody.innerHTML = "";

        // Si hay texto en el buscador, unificar datos y ocultar las tablas separadas
        if (filtro.length > 0) {
            tablaAdmin.style.display = "none";
            tablaOp.style.display = "none";
            tablaUnificada.style.display = "table";

            // Obtener todas las filas de ambas tablas
            let filas = [...document.querySelectorAll("#tablaAdministradores tbody tr, #tablaOperadores tbody tr")];

            filas.forEach(fila => {
                let textoFila = fila.innerText.toLowerCase();
                if (textoFila.includes(filtro)) {
                    // Clonar la fila y agregarla a la tabla unificada
                    let nuevaFila = fila.cloneNode(true);
                    tablaUnificadaBody.appendChild(nuevaFila);
                    hayResultados = true;
                }
            });

            // Si no hay resultados, ocultar la tabla unificada
            tablaUnificada.style.display = hayResultados ? "table" : "none";
        } 
        // Si el buscador está vacío, volver a mostrar las tablas originales y ocultar la unificada
        else {
            tablaAdmin.style.display = "table";
            tablaOp.style.display = "table";
            tablaUnificada.style.display = "none";
        }
    });
});