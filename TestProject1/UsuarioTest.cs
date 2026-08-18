using AppEstudioContable.Controllers;
using AppEstudioContable.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiciosEC.Interfaces.Managers;
using ServiciosEC.Managers;
using ServiciosEC.Models;

namespace TestProject1
{
    public class UsuarioTest
    {
        [Fact]
        public async Task UsuariosIndex_DevuelveVistaConLista()
        {
            // Arrange
            var falsos = new List<Usuario>
            {
                new Usuario{NombreUsuario = "u1", IdPersona = 1, RolId = 1},
                new Usuario{NombreUsuario = "u2", IdPersona = 2, RolId = 1},
                new Usuario{NombreUsuario = "u3", IdPersona = 3, RolId = 1},

            };

            var mockService = new Mock<IUsuariosManager>();
            mockService.Setup(s => s.ObtenerTodos(default)).ReturnsAsync(falsos);

            var controller = new UsuariosController(mockService.Object);


            // Act
            var result = await controller.Index(default);


            // Assert

            //debe devolver una vista con un modelo de tipo IEnumerable<Usuario>
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.NotNull(viewResult?.Model);
            var model = Assert.IsType<List<Usuario>>(viewResult.Model);


            Assert.Equal(3, model.Count);
            Assert.Equal("u1", model[0].NombreUsuario);
        }



        [Fact]
        public async Task UsuariosDetails_DevuelveVistaConUsuarioModel()
        {
            // Arrange
            var falsos = new List<Usuario>
            {
                new Usuario{NombreUsuario = "u1", IdPersona = 1, RolId = 1},
                new Usuario{NombreUsuario = "u2", IdPersona = 2, RolId = 1},
                new Usuario{NombreUsuario = "u3", IdPersona = 3, RolId = 1},

            };

            var id = 1;
            var mockService = new Mock<IUsuariosManager>();
            mockService.Setup(s => s.ObtenerPorId(id, default)).ReturnsAsync(falsos[id-1]);

            var controller = new UsuariosController(mockService.Object);


            // Act
            var result = await controller.Details(id, default);


            // Assert

            //debe devolver una vista con un modelo de tipo UsuarioModel
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.NotNull(viewResult?.Model);
            var model = Assert.IsType<UsuarioModel>(viewResult.Model);


            Assert.Equal("u1", model.nombre_usuario);
        }
    }
}