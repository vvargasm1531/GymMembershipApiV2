```javascript
// ======================
// POWERFIT GYM APP
// ======================

// Cuando la página termine de cargar
document.addEventListener("DOMContentLoaded", function () {

    console.log("✅ PowerFit Gym iniciado");

    // Mensaje de bienvenida
    setTimeout(function () {
        mostrarNotificacion("🔥 Bienvenido a PowerFit Gym");
    }, 1000);


    // ======================
    // ANIMACIÓN DE ESTADÍSTICAS
    // ======================

    const stats = document.querySelectorAll(".stat h3");

    stats.forEach(function (stat) {

        const texto = stat.innerText;
        const valorFinal = parseInt(texto.replace("+", ""));

        if (isNaN(valorFinal)) {
            return;
        }

        let contador = 0;

        const intervalo = setInterval(function () {

            contador += Math.ceil(valorFinal / 50);

            if (contador >= valorFinal) {

                // Mantiene el + solamente si el original lo tenía
                if (texto.includes("+")) {
                    stat.innerText = valorFinal + "+";
                } else {
                    stat.innerText = valorFinal;
                }

                clearInterval(intervalo);

            } else {

                stat.innerText = contador;

            }

        }, 40);

    });


    // ======================
    // CUENTA REGRESIVA
    // ======================

    let tiempo = 3600;

    function actualizarTimer() {

        const timer = document.getElementById("timer");

        // Si no existe el contador, no hacemos nada
        if (!timer) {
            return;
        }

        const horas = Math.floor(tiempo / 3600);

        const minutos =
            Math.floor((tiempo % 3600) / 60);

        const segundos =
            tiempo % 60;

        timer.innerText =
            String(horas).padStart(2, "0") + ":" +
            String(minutos).padStart(2, "0") + ":" +
            String(segundos).padStart(2, "0");

        if (tiempo > 0) {
            tiempo--;
        }

    }

    actualizarTimer();

    setInterval(actualizarTimer, 1000);


    // ======================
    // HORA DEL DÍA
    // ======================

    const hora = new Date().getHours();

    if (hora < 12) {

        console.log("☀️ Buenos días");

    } else if (hora < 18) {

        console.log("🌤️ Buenas tardes");

    } else {

        console.log("🌙 Buenas noches");

    }

});


// ======================
// ELEGIR PLAN
// ======================

function elegirPlan(plan) {

    localStorage.setItem("planSeleccionado", plan);

    mostrarNotificacion(
        "✅ Has seleccionado el Plan " + plan
    );

    const contacto = document.getElementById("contacto");

    if (contacto) {

        contacto.scrollIntoView({
            behavior: "smooth"
        });

    }

}


// ======================
// FORMULARIO
// ======================

function enviarFormulario(event) {

    event.preventDefault();

    const nombreInput =
        document.querySelector('input[type="text"]');

    let nombre = "cliente";

    if (nombreInput) {
        nombre = nombreInput.value;
    }

    mostrarNotificacion(
        "🏋️ Gracias " + nombre + ", pronto te contactaremos"
    );

}


// ======================
// NOTIFICACIONES
// ======================

function mostrarNotificacion(mensaje) {

    const div = document.createElement("div");

    div.classList.add("notificacion");

    div.innerText = mensaje;

    document.body.appendChild(div);

    setTimeout(function () {

        div.remove();

    }, 3000);

}
```
