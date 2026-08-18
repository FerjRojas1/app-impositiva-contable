using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Interfaces.Managers
{
    /// <summary>
    /// Interfaz genérica para todas las clases Manager. 
    /// Funciona como intermediario entre los Controller y el DAO. 
    /// Realiza la lógica y las validaciones necesarias.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public interface IManager<T>
    {

        /// <summary>
        /// Realiza las operaciones previas necesarias y después inserta un registro <typeparamref name="T"/> en la base de datos
        /// </summary>
        /// <param name="entidad"></param>
        //public void Insertar(T entidad);
        Task Insertar(T entidad, CancellationToken cancellationToken);


        /// <summary>
        /// Devuelve todos los registros <typeparamref name="T"/> de la base de datos
        /// </summary>
        /// <returns>Lista de <typeparamref name="T"/></returns>
        Task<IEnumerable<T>> ObtenerTodos(CancellationToken cancellationToken);

        /// <summary>
        /// Devuelve el registro <typeparamref name="T"/> con el id especificado
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto <typeparamref name="T"/></returns>
        Task<T> ObtenerPorId(int id, CancellationToken cancellationToken);



        /// <summary>
        /// Permite editar un registro <typeparamref name="T"/> en la base de datos
        /// </summary>
        /// <param name="entidad"></param>
        Task Editar(T entidad, CancellationToken cancellationToken);

        /// <summary>
        /// Realiza las operaciones previas necesarias y después elimina un registro <typeparamref name="T"/> en la base de datos en base a su id.
        /// </summary>
        /// <param name="id"></param>
        Task Borrar(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Devuelve la cantidad de registros.
        /// </summary>
        Task<int> ObtenerCantidad(CancellationToken cancellationToken);




    }
}
