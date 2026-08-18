let txtSaldoAnt
let txtSaldoDispAnt
let txtRetPerc

const calcularSaldos = e => {
    let saldoAntStr = txtSaldoAnt.value;
    let saldoDispAntStr = txtSaldoDispAnt.value;
    let retPercStr = txtRetPerc.value;

    //validaciones para no permitir valores no numericos
    if (isNaN(e.target.value)) {
        e.target.value = "";
    }
    if (saldoAntStr == "") saldoAntStr = 0
    if (saldoDispAntStr == "") saldoDispAntStr = 0
    if (retPercStr == "") retPercStr = 0

    //convierto a float los valores de los inputsss
    const saldoAnt = parseFloat(saldoAntStr)
    const saldoDispAnt = parseFloat(saldoDispAntStr)
    const retPerc = parseFloat(retPercStr)

    //otros valores a sumar
    const totalDeb = document.querySelector("#totalDeb").dataset.value
    const totalCred = document.querySelector("#totalCred").dataset.value

    //sumas
    const saldoTec = parseFloat(totalDeb) - parseFloat(totalCred) - saldoAnt
    const saldoDisp = saldoTec - saldoDispAnt - retPerc

    const nuevoSaldoTec = saldoTec.toLocaleString("es-AR", {
        style: "currency",
        currency: "ARS",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
    document.querySelector("#saldoTec").textContent = nuevoSaldoTec
    document.querySelector("#saldoTec2").textContent = nuevoSaldoTec

    const nuevoSaldoDisp = saldoDisp.toLocaleString("es-AR", {
        style: "currency",
        currency: "ARS",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
    document.querySelector("#saldoDisp").textContent = nuevoSaldoDisp

    //console.log("valor",valor, totalDeb, totalCred)
}




document.addEventListener("DOMContentLoaded", () => {

    txtSaldoAnt = document.querySelector("input[name='SaldoTecnicoAnterior']")

    txtSaldoDispAnt = document.querySelector("input[name='SaldoLibreDisponibilidad']")
    txtRetPerc = document.querySelector("input[name='RetencionesPercepciones']")

    txtSaldoAnt.addEventListener("input", calcularSaldos)
    txtSaldoDispAnt.addEventListener("input", calcularSaldos)
    txtRetPerc.addEventListener("input", calcularSaldos)




    document.getElementById("form-libroIva").addEventListener("submit", e => {
        e.preventDefault();

        const form = e.target;
        const formData = new FormData(form);
        const url = form.action;
        const options = {
            method: "POST",
            body: formData,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            }
        };
        fetch(url, options)
            .then(response =>  response.json())
            .then(data => {
                console.log("ok", data);
                document.getElementById("modalIvaTexto").textContent = "Libro Iva guardado correctamente.";
            })
            .catch(error => {
                console.error("Error:", error);
                document.getElementById("modalIvaTexto").textContent = "Libro Iva guardado correctamente.";
            });
    })

})