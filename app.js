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


var app = builder.Build();

app.UseCors("dev");

// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();



async function editarSocio() {

const socio = {
socioId: 1,
nombre: "Victor Morales",
correo: "victor@gmail.com"
};

await fetch(
'https://localhost:7000/api/Socios/1',
{
method: 'PUT',
headers: {
'Content-Type': 'application/json'
},
body: JSON.stringify(socio)
}
);
}


async function actualizarSocio() {

const socio = {
socioId: 1,
nombre: "Victor",
telefono: "88888888"
};

await fetch(
"https://localhost:7001/api/Socios/1",
{
method: "PUT",
headers: {
"Content-Type": "application/json"
},
body: JSON.stringify(socio)
}
);

alert("Socio actualizado");
}
















async function actualizarSocio() {

const socio = {
socioId: 1,
nombre: "Victor",
telefono: "88888888"
};

await fetch(
"https://localhost:7001/api/Socios/1",
{
method: "PUT",
headers: {
"Content-Type": "application/json"
},
body: JSON.stringify(socio)
}
);

alert("Socio actualizado");
}









async function guardarSocio() {

const socio = {
nombres: document.getElementById("nombres").value,
apellidos: document.getElementById("apellidos").value,
documento: document.getElementById("documento").value,
telefono: null,
correo: null,
fechaNacimiento: null
};

try {

const response = await fetch(
"https://localhost:5001/api/socios",
{
method: "POST",
headers: {
"Content-Type": "application/json"
},
body: JSON.stringify(socio)
}
);

if (response.ok) {
alert("Socio guardado correctamente");
} else {
alert("Error al guardar");
}

} catch (error) {
console.error(error);
alert("No se pudo conectar con la API");
}
}


fetch("http://localhost:5096/api/Socios", { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ nombres: "Prueba", apellidos: "Test", documento: "123" }) }).then(r => r.text()).then(console.log)



async function guardarContacto(event) {
    event.preventDefault();

    const contacto = {
        nombre: valor("contactoNombre"),
        correo: valor("contactoCorreo"),
        mensaje: valor("contactoMensaje") || null
    };

    try {
        const res = await fetch("http://localhost:5096/api/Contactos", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(contacto)
        });

        if (!res.ok) throw new Error(await res.text());

        alert("Mensaje enviado");
        event.target.reset();
    } catch (e) {
        console.error(e);
        alert("No se pudo enviar: " + e.message);
    }
}











async function guardarContacto(event) {
    event.preventDefault();

    const contacto = {
        nombre: valor("contactoNombre"),
        correo: valor("contactoCorreo"),
        mensaje: valor("contactoMensaje") || null
    };

    try {
        const res = await fetch("http://localhost:5096/api/Contactos", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(contacto)
        });

       
        if (!res.ok) throw new Error("Código " + res.status + " " + (await res.text()));

        alert("Mensaje enviado");
        event.target.reset();
    } catch (e) {
        console.error(e);
        alert("No se pudo enviar: " + e.message);
    }
}







if (!socio.nombres.trim() || !socio.apellidos.trim() || !socio.documento.trim()) {
  alert("Nombres, apellidos y documento son obligatorios");
  return;
}








async function guardarSocio() {
  const socio = {
    nombres: document.getElementById("nombres").value,
    apellidos: document.getElementById("apellidos").value,
    documento: document.getElementById("documento").value,
    telefono: document.getElementById("telefono").value || null,
    correo: document.getElementById("correo").value || null
  };

  if (!socio.nombres.trim() || !socio.apellidos.trim() || !socio.documento.trim()) {
    alert("Nombres, apellidos y documento son obligatorios");
    return;
  }

  try {
    const res = await fetch("http://localhost:5096/api/Socios", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(socio)
    });
    if (!res.ok) throw new Error(await res.text());
    alert("Socio guardado");
  } catch (e) {
    console.error(e);
    alert("Error: " + e.message);
  }
}





fetch("http://localhost:5096/api/socios").then(r => r.json()).then(console.log).catch(console.error)