using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.Threading.Tasks;



namespace Biltegi2.Negocio.BD
{
    public class BDManager
    {

        private BDHelper _helper;

        public void SetContext(Context context)
        {
            if (context != null)
            {
                _helper = new BDHelper(context);
            }
        }

        /// <summary>
        /// Obtiene la lista de todas las categorias</summary>
        /// <returns> Lista de tipo Negocio.Grupo</returns>
        public List<Negocio.Grupo> GetAllGrupos()
        {
            List<Negocio.Grupo> _grupos;
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {

                    _grupos = database.Table<Negocio.Grupo>().ToList();
                    _grupos = _grupos.OrderBy(_grupo => _grupo.Descripcion).ToList();
                    return _grupos;

                }
                catch (Exception ex)
                {
                    return null;
                    //exception handling code to go here
                }
            }
        }

        /// <summary>
        /// Obtiene la lista de todas llos productos</summary>
        /// <returns> Lista de tipo Negocio.Producto</returns>
        public List<Negocio.Producto> GetAllProductos()
        {
            List<Negocio.Producto> _productos;
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {

                    _productos = database.Table<Negocio.Producto>().ToList();
                    _productos = _productos.OrderBy(_producto => _producto.Descripcion).ToList();
                    return _productos;

                }
                catch (Exception ex)
                {
                    return null;
                    //exception handling code to go here
                }
            }
        }
        
        /// <summary>
        /// Obtiene la lista de todos los productos de un grupo</summary>
        /// <returns> Lista de tipo Negocio.Producto</returns>
        public List<Negocio.Producto> GetProductosGrupo(Negocio.Grupo grupo)
        {
            List<Negocio.Producto> _productos;
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {

                    _productos = database.Table<Negocio.Producto>().Where(u => u.IdGrupo == grupo.IdGrupo).ToList();
                    _productos = _productos.OrderBy(_producto => _producto.Descripcion).ToList();
                    return _productos;

                }
                catch (Exception ex)
                {
                    return null;
                    //exception handling code to go here
                }
            }
        }

        /// <summary>
        /// Obtiene la lista de todos los productos de un grupo</summary>
        /// <returns> Lista de tipo Negocio.Producto</returns>
        public List<Negocio.Elemento> GetElementosGrupo(Negocio.Grupo grupo)
        {
            List<Negocio.Producto> _productos;
            List<Negocio.Elemento> _elementos = new List<Elemento>();
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {

                    _productos = database.Table<Negocio.Producto>().Where(u => u.IdGrupo == grupo.IdGrupo).ToList();
                    _productos = _productos.OrderBy(_producto => _producto.Descripcion).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                    //exception handling code to go here
                }
            }

            foreach (Negocio.Producto p in _productos)
            {
                _elementos.AddRange(p.GetElementos());
            }

            return _elementos;

        }

        /// <summary>
        /// Obtiene la lista de todos los productos de un grupo</summary>
        /// <returns> Lista de tipo Negocio.Producto</returns>
        public List<Negocio.Elemento> GetElementosProducto(Negocio.Producto producto)
        {
           List<Negocio.Elemento> _elementos = new List<Elemento>();
           
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {

                    _elementos = database.Table<Negocio.Elemento>().Where(u => u.IdProducto == producto.IdProducto).ToList();
                    _elementos = _elementos.OrderBy(_e => _e.Descripcion).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                    //exception handling code to go here
                }
            }

            return _elementos;

        }

        /// <summary>
        /// Obtiene la lista de todos los elementos</summary>
        /// <returns> Lista de tipo Negocio.Elemento</returns>
        public List<Negocio.Elemento> GetAllElementos()
        {
            List<Negocio.Elemento> _elementos;
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    _elementos = database.Table<Negocio.Elemento>().ToList();
                    return _elementos;

                }
                catch (Exception ex)
                {
                    return null;
                    //exception handling code to go here
                }
            }
        }

        /// <summary>
        /// Obtiene la lista de todos los elementos de una categoria</summary>
        /// <param name="_grupo">Grupo de la que se desea obtener los elementos</param>
        /// <returns> Lista de tipo Negocio.Elemento</returns>
        public List<Negocio.Elemento> GetElementosCategoria(Negocio.Producto producto)
        {
            List<Negocio.Elemento> _elementos;
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    _elementos = database.Table<Negocio.Elemento>().Where(u => u.IdProducto== producto.IdGrupo).ToList();
                    return _elementos;

                }
                catch (Exception ex)
                {
                    return null;
                    //exception handling code to go here
                }
            }
        }
        /// <summary>
        /// Obtiene el identificador de la categoria</summary>
        /// <param name="grupo">grupo de la que se desea obtener el id</param>
        /// <returns> Id de la categoria</returns>
        public int GetIdGrupo(string grupo)
        {
            Negocio.Grupo _grupo;
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    _grupo =  database.Table<Negocio.Grupo>().FirstOrDefault(u => u.Descripcion == grupo);
                    return _grupo.IdGrupo;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// Obtiene el identificador de la categoria</summary>
        /// <param name="grupo">grupo de la que se desea obtener el id</param>
        /// <returns> Id de la categoria</returns>
        public Negocio.Grupo GetGrupoById(Negocio.Grupo grupo)
        {
            
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    return  database.Table<Negocio.Grupo>().FirstOrDefault(u => u.IdGrupo == grupo.IdGrupo);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// Obtiene el identificador de la categoria</summary>
        /// <param name="producto">Producto de la que se desea obtener el id</param>
        /// <returns> Id de la categoria</returns>
        public int GetIdProducto(string producto)
        {
            Negocio.Producto _producto;
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    _producto = database.Table<Negocio.Producto>().FirstOrDefault(u => u.Descripcion == producto);
                    return _producto.IdGrupo;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

        }
        /// <summary>
        /// Obtiene la lista con las descripciones de la categoria</summary>
        /// <param name="grupo">Descripcion Categoria de la que se desea obtener los elemntos, </param>
        /// <returns> Lista de descripciones de los elementos </returns>
        public List<string> GetElementos(string producto)
        {
            int IdProducto;
            List<Negocio.Elemento> _elementos;
            List<string> _lista;
            IdProducto = GetIdProducto(producto);
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    _lista = new List<string>();
                    _elementos = database.Table<Negocio.Elemento>().Where(u => u.IdProducto == IdProducto).ToList();
                    foreach (Negocio.Elemento _elemento in _elementos)
                    {
                        _lista.Add(_elemento.Descripcion);
                    }
                    return _lista;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Obtiene el id del elemento</summary>
        /// <param name="elemento">Descripcion del elemento del que se desea obtener el id</param>
        /// <returns> Id del elemento</returns>
        public int GetIdElemento(string elemento)
        {
            Negocio.Elemento _elemento;
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    _elemento = database.Table<Negocio.Elemento>().FirstOrDefault(u => u.Descripcion == elemento);
                    return _elemento.IdElemento;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

        }
        /// <summary>
        /// Obtiene los elementos que tienen la cadena filtro en su descripcion</summary>
        /// <param name="filtro">filtro que se desea aplicar</param>
        /// <returns> Lista de los elementos que cumplen el filtro, lista vacia si no hay ninguno</returns>
        public List<Negocio.Elemento> GetElementosFiltro(string filtro)
        {
         
            List<Negocio.Elemento> _elementos;
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                   _elementos = database.Table<Negocio.Elemento>().Where(u => u.Descripcion.Contains(filtro)).ToList();
                   return _elementos;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Inserta en base de datos la categoria </summary>
        /// <param name="descripcion">Producto </param>
        public void InsertProducto(Negocio.Producto producto)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    db.Insert(producto);

                }
                catch (Exception ex)
                {

                }
            }
        }



        /// <summary>
        /// Inserta en base de datos la categoria </summary>
        /// <param name="descripcion">categoria </param>
        public void InsertGrupo(Negocio.Grupo grupo)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    db.Insert(grupo);

                }
                catch (Exception ex)
                {

                }
            }
        }
        /// <summary>
        /// Inserta en base de datos el elemento </summary>
        /// <param name="elemento">categoria </param>
        public void InsertElemento(Negocio.Elemento elemento)
        {

            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    db.Insert(elemento);
                  }
                catch (Exception ex)
                {
   
                }
            }
        }

        /// <summary>
        /// Inserta en base de datos la existencia </summary>
        /// <param name="_existencia">existencia </param>
        /// <returns> Existencias</returns>
        public Negocio.Existencias InsertExistencias(Negocio.Existencias _existencia)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    db.Insert(_existencia);
                    return _existencia;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

              /// <summary>
        /// Obtiene todas las existencias de _elemento </summary>
        /// <param name="elemento">elemento del que se desean obtener las existencias </param>
        /// <returns> Lista de tipo Negocio.Existencias</returns>
        public List<Negocio.Existencias> GetExistencias(Negocio.Elemento elemento)
        {
            List<Negocio.Existencias> _listaExistencias = new List<Negocio.Existencias>();
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    _listaExistencias=database.Table<Existencias>().Where(u => u.IdElemento == elemento.IdElemento).ToList();
                    _listaExistencias = _listaExistencias.OrderBy(_ex => _ex.FechaEntrada).ToList();
                    return _listaExistencias;
                }
                catch (Exception e)
                {
                    return _listaExistencias;
                }
            }

        }
        /// <summary>
        /// Obtiene el elemento </summary>
        /// <param name="elemento">elemento del que se desea obtener la informacion, la busquedad se realiza por descripcion </param>
        ///<returns>Lista del tipo Negocio.Elemento</returns>
        public Negocio.Elemento GetElemento(Negocio.Elemento elemento)
        {

            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    return database.Table<Elemento>().FirstOrDefault(u => u.Descripcion == elemento.Descripcion);
                  
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }

        public Negocio.Elemento GetElementoByScan(Negocio.Elemento elemento)
        {

            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    return database.Table<Elemento>().FirstOrDefault(u => u.CodigoBarras == elemento.CodigoBarras);

                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }
        /// <summary>
        /// Obtiene el elemento </summary>
        /// <param name="elemento">elemento del que se desea obtener la informacion, la busquedad se realiza por id </param>
        /// <returns> Negocio.Elemento con el id indicado</returns>
        public Negocio.Elemento GetElementoById(Negocio.Elemento elemento)
        {
            Negocio.Elemento _elem = new Negocio.Elemento();
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    _elem = database.Table<Negocio.Elemento>().FirstOrDefault(u => u.IdElemento== elemento.IdElemento);
                    return _elem;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }
        /// <summary>
        /// Obtiene la categoria </summary>
        /// <ref
        /// <param name="Grupo">Grupo de la que se desea obtener la informacion, la busquedad se realiza por descripcion </param>
        /// <returns> Negocio.Grupo con la descripcion indicada</returns>
        public Negocio.Grupo GetGrupo(Negocio.Grupo  grupo)
        {
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    return database.Table<Negocio.Grupo>().FirstOrDefault(u => u.Descripcion == grupo.Descripcion);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// Obtiene el producto </summary>
        /// <ref
        /// <param name="Producto">Grupo de la que se desea obtener la informacion, la busquedad se realiza por descripcion </param>
        /// <returns> Negocio.Producto con la descripcion indicada</returns>
        public Negocio.Producto GetProducto(Negocio.Producto producto)
        {
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    return database.Table<Negocio.Producto>().FirstOrDefault(u => u.Descripcion == producto.Descripcion);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }
        /// <summary>
        /// Obtiene el producto </summary>
        /// <ref
        /// <param name="Producto">Grupo de la que se desea obtener la informacion, la busquedad se realiza por Id </param>
        /// <returns> Negocio.Producto con la descripcion indicada</returns>
        public Negocio.Producto GetProductobById(Negocio.Producto producto)
        {
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    return database.Table<Negocio.Producto>().FirstOrDefault(u => u.IdProducto== producto.IdProducto);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }


        /// <summary>
        /// Obtiene la existencia </summary>
        /// <param name="existencia">Existencia de la que se desea obtener la informacion, la busquedad se realiza por id </param>
        /// <returns> Negocio.Existencias con el id indicado</returns>

        public Negocio.Existencias GetExistencia(Negocio.Existencias existencia)
        {
            Negocio.Existencias _existencia = new Negocio.Existencias();
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    return database.Table<Negocio.Existencias>().FirstOrDefault(u => u.IdExistencias == existencia.IdExistencias);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// Obtiene la categoria del elemento </summary>
        /// <param name="elemento">elemento del que se desea obtener la categoria </param>
        /// <returns> Negocio.Categoria con el id del elemento indicada</returns>

        public Negocio.Producto GetProductoElemento(Negocio.Elemento elemento)
        {
            Negocio.Producto _producto = new Negocio.Producto();
            using (var database = new SQLiteConnection(_helper.ReadableDatabase.Path))
            {
                try
                {
                    _producto = database.Table<Negocio.Producto>().FirstOrDefault(u => u.IdProducto == elemento.IdProducto);
                    return _producto;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }
        /// <summary>
        /// elimina el elemento </summary>
        /// <param name="elemento">Negocio.Elemento a borrar de base de datos </param>
        /// <returns>- 1 en caso de error</returns> 
        public long DeleteElemento(Negocio.Elemento elemento)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    return db.Delete(elemento);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// elimina el elemento </summary>
        /// <param name="producto">Negocio.Elemento a borrar de base de datos </param>
        /// <returns>- 1 en caso de error</returns> 
        public long DeleteProducto(Negocio.Producto producto)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    return db.Delete(producto);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }


        /// <summary>
        /// elimina la categoria </summary>
        /// <param name="GRUPO">Negocio.Categoria a borrar de base de datos </param>
        /// <returns>- 1 en caso de error</returns> 
        public long DeleteGrupo(Negocio.Grupo grupo)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    return db.Delete(grupo);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// elimina la existencia </summary>
        /// <param name="existencia">Negocio.Existencia a borrar de base de datos </param>
        /// <returns>- 1 en caso de error</returns> 
        public long DeleteExistencia(Negocio.Existencias _existencia)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    return db.Delete(_existencia);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// elimina todas las existencias de un elemento </summary>
        /// <param name="elemento">Negocio.Elemento del que se desean borrar las existencias </param>
        /// <returns>- 1 en caso de error</returns> 
        public long DeleteExistenciasElemento(Negocio.Elemento elemento)
        {
            List<Negocio.Existencias> _lista = new List<Negocio.Existencias>();
            _lista = GetExistencias(elemento);
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    foreach (Negocio.Existencias e in _lista)
                    {
                        db.Delete(e);
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

      
        /// <summary>
        /// Actualiza la informacion en base de datos de la categoria </summary>
        /// <param name="grupo">Negocio.Grupo del que se desea actualziar la informacion </param>
        /// <returns>- 1 en caso de error</returns> 
        public long UpdateGrupo(Negocio.Grupo grupo)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    return db.Update(grupo);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Actualiza la informacion en base de datos de la existencia </summary>
        /// <param name="existencia">Negocio.Existencias del que se desea actualziar la informacion </param>
        /// <returns>- 1 en caso de error</returns> 
        public long UpdateExistencias(Negocio.Existencias existencia)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    return db.Update(existencia);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Actualiza la informacion en base de datos de la existencia </summary>
        /// <param name="existencia">Negocio.Existencias del que se desea actualziar la informacion </param>
        /// <returns>- 1 en caso de error</returns> 
        public long UpdateProducto(Negocio.Producto producto)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    return db.Update(producto);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Actualiza la informacion en base de datos de la existencia </summary>
        /// <param name="Elemento">Negocio.Elemento del que se desea actualziar la informacion </param>
        /// <returns>- 1 en caso de error</returns> 
        public long UpdateElemento(Negocio.Elemento elemento)
        {
            using (var db = new SQLiteConnection(_helper.WritableDatabase.Path))
            {
                try
                {
                    return db.Update(elemento);
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
    }
}