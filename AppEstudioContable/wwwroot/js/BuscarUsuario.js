const input = document.getElementById("buscarUsuario")

input.addEventListener("input", async e => {
    const filtro = e.target.value.toLowerCase();

    const res = await fetch(`/Usuarios/Buscar?filtro=${filtro}`)
    const json = await res.json()

    const trs = [...json].map(usuario => `
        <tr>
            <td class="text-center">${usuario.idPersona}</td>
            <td>${usuario.nombreUsuario}</td>
            <td>${usuario.email}</td>
            <td>${usuario.rolId == 2 ? "Admin" : "Usuario"}</td>
            <td class="text-center">
                <div class="btn-group btn-group-sm">
                    <a href="/Usuarios/Edit/${usuario.idPersona}" class="btn btn-outline-secondary" title="Editar">
                        <i class="fas fa-edit"></i>
                    </a>
                    <a href="/Usuarios/Details/${usuario.idPersona}" class="btn btn-outline-info" title="Detalles">
                        <i class="fas fa-info-circle"></i>
                    </a>
                    <a href="/Usuarios/Delete/${usuario.idPersona}" class="btn btn-outline-danger" title="Eliminar">
                        <i class="fas fa-trash-alt"></i>
                    </a>
                </div>
            </td>
        </tr>
    `)

    document.getElementById("lista-usuarios").innerHTML = trs.join("\n")

    

})