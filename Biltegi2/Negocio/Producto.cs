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

namespace Biltegi2.Negocio
{
    [Table("PRODUCTO")]
    public class Producto
    {
        [PrimaryKey, AutoIncrement]
        public int IdProducto { get; set; }
        public string Descripcion { get; set; }

        public int IdGrupo { get; set; }

        public void GetProducto()
        {
            Negocio.Producto _producto = GestorApp.myData.GetProducto(this);
            if (_producto != null)
            {
                IdProducto = _producto.IdProducto;
                Descripcion = _producto.Descripcion;
                IdGrupo = _producto.IdGrupo;
            }

        }

        public Negocio.Grupo GetGrupo()
        {
            Grupo _grupo = new Negocio.Grupo();
            _grupo.IdGrupo = IdGrupo;
            _grupo.GetGrupoById();
            return _grupo;
        }

        public void GetProductoById()
        {
            Negocio.Producto _producto = GestorApp.myData.GetProductobById(this);
            if (_producto != null)
            {
                IdProducto = _producto.IdProducto;
                Descripcion = _producto.Descripcion;
                IdGrupo = _producto.IdGrupo;
            }

        }

        public List<Elemento> GetElementos()
        {
            return GestorApp.myData.GetElementosProducto(this);
        }

        public List<Existencias> GetExistencias()
        {
            List<Negocio.Existencias> ListaRetorno = new List<Existencias>();
            List<Negocio.Elemento> ListaElementos = GetElementos();
            foreach (Negocio.Elemento e in ListaElementos)
            {
                e.GetExistencias();
                ListaRetorno.AddRange(e.lExistencias);
            }
            return ListaRetorno;
        }

        /// <summary>
        /// Actualiza en base de datos</summary>
        public void Update()
        {
            GestorApp.myData.UpdateProducto(this);
        }
        /// <summary>
        /// Elimina de base de datos</summary>
        public void Delete()
        {
            List<Elemento> _elementos = GetElementos();
            foreach (Elemento e in _elementos) {
                e.Delete();
            }
            GestorApp.myData.DeleteProducto(this);
        }

        /// <summary>
        /// Inserta en base de datos</summary>
        public void Insert()
        {
            GestorApp.myData.InsertProducto(this);
            GetProducto();
        }

        /// <summary>
        /// Comprueba si ya existe en base de datos</summary>
        public bool Exists()
        {
            Negocio.Producto _producto = GestorApp.myData.GetProducto(this);
            return (_producto != null);
        }
    }
}