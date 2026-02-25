document.addEventListener("DOMContentLoaded", function () {
    let tipoIdea = document.getElementById("tipoIdea");
    let tipoTarea = document.getElementById("tipoTarea");
    let importanciaSection = document.getElementById("importanciaSection");
    let urgente = document.getElementById("urgente");
    let noTanUrgente = document.getElementById("noTanUrgente");
    let estadoInput = document.getElementById("estadoTarea");
    let colorInput = document.getElementById("colorTarea");

    // Configuración inicial: Idea por defecto
    estadoInput.value = 1; // Estado 1 = Idea
    colorInput.value = 1;  // Color 1 = Blanco

    function actualizarValores() {
        if (tipoIdea.checked) {
            estadoInput.value = 1;
            colorInput.value = 1;
            importanciaSection.classList.add("d-none");
        } else {
            estadoInput.value = 2; // Estado 2 = Tarea
            importanciaSection.classList.remove("d-none");
            
            // Por defecto, si no se seleccionó, toma "No tan urgente"
            if (urgente.checked) {
                colorInput.value = 2;
            } else {
                colorInput.value = 3;
            }
        }
    }

    // Eventos para detectar cambios
    tipoIdea.addEventListener("change", actualizarValores);
    tipoTarea.addEventListener("change", actualizarValores);
    urgente.addEventListener("change", () => { colorInput.value = 2; });
    noTanUrgente.addEventListener("change", () => { colorInput.value = 3; });

});