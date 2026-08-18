using Moq;
using ServiciosEC.Interfaces.Managers;
using ServiciosEC.Models;
using ServiciosEC.Utilidades.ModelosDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class IVAControllerTests
    {
        [Fact]
        public async Task ObtenerIVAPorClienteYPeriodo_DeberiaRetornarCorrecto()
        {
            var mock = new Mock<ICompraManager>(); // pasamos null si no usás esos deps en este test

            mock.Setup(m => m.ObtenerIVAPorClienteYPeriodo(
                    It.IsAny<Cliente>(), It.IsAny<int>(), It.IsAny<int>())
                ).ReturnsAsync((new TotalesIVA { Neto21 = 1000 }, new TotalesIVA { Neto21 = 200 }));

            var (crd, restitucionCred) = await mock.Object.
                ObtenerIVAPorClienteYPeriodo(new Cliente(), 5, 2025);

            Assert.NotNull(crd);
            Assert.NotNull(restitucionCred);

            Assert.Equal(1000, crd.NetoGravado);

        }
    }
}
