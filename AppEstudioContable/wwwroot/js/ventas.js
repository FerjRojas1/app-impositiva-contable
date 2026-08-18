// AppEstudioContable/wwwroot/js/ventas.js

document.addEventListener('DOMContentLoaded', function () {




    const getCuitCliente = () => {

        const urlParams = new URLSearchParams(window.location.search);
        if (urlParams.has('cuit')) {
            return urlParams.get('cuit');
        }

        const cuitHiddenInput = document.getElementById('clienteCuit');
        if (cuitHiddenInput) {
            return cuitHiddenInput.value;
        }
        return '';
    };

    const clienteCuit = getCuitCliente();


    const mesInput = document.getElementById('mesInput');
    const anoInput = document.getElementById('anoInput');

    if (!mesInput || !anoInput)
        return

    
    const buildUrl = (baseUrl, mes, ano, cuit) => {
        console.log(baseUrl)
        const params = []; 

        if (cuit) {
            params.push(cuit); 
        }
        if (ano) { 
            params.push(ano);
        }
        if (mes) { 
            params.push(mes);
        }

        let url = baseUrl
        if (params.length > 0) {
            url += '/' + params.join('/');
        }
        return url;
    };
   




    const filtrarListadoBtn = document.getElementById('filtrarListadoBtn');
    if (filtrarListadoBtn) {
        filtrarListadoBtn.addEventListener('click', function (e) {
            e.preventDefault();
            const mes = mesInput.value;
            const ano = anoInput.value;
            const url = buildUrl('/Ventas/lista', mes, ano, clienteCuit);
            window.location.href = url;
        });
    }


    const verTotalNetoBtn = document.getElementById('verTotalNetoBtn');
    if (verTotalNetoBtn) {
        verTotalNetoBtn.addEventListener('click', function (e) {
            e.preventDefault();
            const mes = mesInput.value;
            const ano = anoInput.value;

            const url = buildUrl('/Ventas/VerNeto', mes, ano, clienteCuit);
            window.location.href = url;
        });
    }


    const verTotalesComprobanteBtn = document.getElementById('verTotalesComprobanteBtn');
    if (verTotalesComprobanteBtn) {
        verTotalesComprobanteBtn.addEventListener('click', function (e) {
            e.preventDefault();
            const mes = mesInput.value;
            const ano = anoInput.value;

            const url = buildUrl('/Ventas/VerTotales', mes, ano, clienteCuit);
            window.location.href = url;
        });
    }


    const limpiarFiltrosBtn = document.getElementById('limpiarFiltrosBtn');
    if (limpiarFiltrosBtn) {
        limpiarFiltrosBtn.addEventListener('click', function (e) {
            e.preventDefault();
            mesInput.value = '';
            anoInput.value = ''; 
            const url = buildUrl('/Ventas/lista', '', '', clienteCuit); 
            window.location.href = url;
        });
    }


    mesInput.addEventListener('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '').substring(0, 2); 
        if (parseInt(this.value) > 12) {
            this.value = '12'; 
        } else if (parseInt(this.value) < 1 && this.value.length === 2) {
            this.value = '01'; 
        }
    });

    anoInput.addEventListener('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '').substring(0, 4); 
    });

});


document.body.addEventListener("click", function (e) {
    const btn = e.target.closest(".btn-guardar-modal");
    if (btn) {
        console.log("Clic en botón Guardar detectado", btn);

        const idVenta = btn.getAttribute('data-id');
        const modal = document.getElementById('modalEdit_' + idVenta);

        const getInputValue = (name) => parseFloat(modal.querySelector(`input[name="${name}"]`)?.value.replace(',', '.') || '0');

        const total = getInputValue('Total');
        const netoGravado = getInputValue('NetoGravado');
        const noGravado = getInputValue('NoGravado');
        const exento = getInputValue('Exento');
        const iva = getInputValue('Iva');
        const iva0 = getInputValue('Iva0');
        const iva25 = getInputValue('Iva25');
        const iva5 = getInputValue('Iva5');
        const iva105 = getInputValue('Iva105');
        const iva21 = getInputValue('Iva21');
        const iva27 = getInputValue('Iva27');
        const grav0 = getInputValue('Grav0');
        const grav25 = getInputValue('Grav25');
        const grav5 = getInputValue('Grav5');
        const grav105 = getInputValue('Grav105');
        const grav21 = getInputValue('Grav21');
        const grav27 = getInputValue('Grav27');

        const tolerancia = 0.1;

        const totalCalculado = netoGravado + iva + exento + noGravado;
        const totalValido = Math.abs(total - totalCalculado) < tolerancia;

        const ivaDesglose = iva0 + iva25 + iva5 + iva105 + iva21 + iva27;
        const ivaValido = Math.abs(iva - ivaDesglose) < tolerancia;

        const netoDesglose = grav0 + grav25 + grav5 + grav105 + grav21 + grav27;
        const netoValido = Math.abs(netoGravado - netoDesglose) < tolerancia;

        if (!totalValido) {
            alert("El Total no coincide con la suma de Neto Gravado + IVA + Exento + No Gravado.");
            return;
        }

        if (!ivaValido || !netoValido) {
            alert("La suma de IVA o Neto Gravado desglosado no coincide con los valores totales.");
            return;
        }

        const formData = new FormData(modal.querySelector('form'));
        const data = new URLSearchParams();
        for (const pair of formData) {
            data.append(pair[0], pair[1]);
        }

        fetch('/Ventas/Edit2', {
            method: 'POST',
            body: data,
        })
            .then(response => {
                if (!response.ok) throw new Error('Error al guardar en el servidor');
                return response.json();
            })
            .then(result => {
                if (result.success) {
                    alert("Venta actualizada correctamente.");
                    $('#modalEdit_' + idVenta).modal('hide');
                    btn.closest('tr').remove();
                } else {
                    alert("Error: " + (result.message || "No se pudo actualizar."));
                }
            })
            .catch(error => {
                console.error(error);
                alert("Error al guardar la venta." + error.message);
            });
    }
});