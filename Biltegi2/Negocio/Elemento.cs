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
    [Table("ELEMENTO")]
    public class Elemento
    {
        [PrimaryKey, AutoIncrement]
        public int IdElemento { get; set; }

        public string Descripcion { get; set; }
        public int IdProducto { get; set; }
        public string CodigoBarras { get; set; }

        [Ignore]
        public Negocio.Producto _producto { get; set; }
        [Ignore]
        public List<Negocio.Existencias> lExistencias { get; set; }

        public Elemento()
        {
            lExistencias = new List<Negocio.Existencias>();
            _producto = new Negocio.Producto();
        }

        /// <summary>
        /// Obtiene el elemento por descripcion </summary>       
        public void GetElemento()
        {
            Negocio.Elemento _elemento=GestorApp.myData.GetElemento(this);
            if (_elemento != null)
            {
                Descripcion = _elemento.Descripcion;
                IdElemento = _elemento.IdElemento;
                IdProducto = _elemento.IdProducto;
            }
        }


        /// <summary>
        /// Elimina el elemento  de la base de datos, elimina las existencias asociadas y sus precios</summary>
        public void Delete()
        {
            DeleteExistencias();
            GestorApp.myData.DeleteElemento(this);
        }

        /// <summary>
        /// Inserta el elemento  de la base de datos</summary>
        public void Insert()
        {
            GestorApp.myData.InsertElemento(this);
            GetElemento();
        }

        /// <summary>
        /// Elimina las existencias del elemento  de la base de datos</summary>
        public void DeleteExistencias()
        {
            GestorApp.myData.DeleteExistenciasElemento(this);        
        }


        /// <summary>
        /// Obtiene de base de datos el producto y lo almacena en producto</summary>
        public void GetProducto()
        {
            _producto =  GestorApp.myData.GetProductoElemento(this);
        }

        /// <summary>
        /// Obtiene de base de datos las existencias y lo almacena en lExistencias</summary>
        public void GetExistencias()
        {
            lExistencias = GestorApp.myData.GetExistencias(this);
        }

        /// <summary>
        /// Obtiene el elemento por id </summary>
        public void GetElementoById()
        {
            Negocio.Elemento _elemento = GestorApp.myData.GetElementoById(this);
            if (_elemento != null) { 
                Descripcion = _elemento.Descripcion;
                IdElemento = _elemento.IdElemento;
                IdProducto = _elemento.IdProducto;
                CodigoBarras = _elemento.CodigoBarras;
            }
        }

        public bool Exists()
        {
            Negocio.Elemento _elemento = GestorApp.myData.GetElemento(this);
            return (_elemento != null);
        }
        public Negocio.Grupo GetGrupo()
        {
            GetElemento();
            GetProducto();
            if (_producto != null) {
                return _producto.GetGrupo();
            }
            else { 
                Grupo g = new Grupo();
                return g;
            }
        }
        public void Update()
        {
            GestorApp.myData.UpdateElemento(this);
        }

        public void Scan()
        {
            Negocio.Elemento _elemento = GestorApp.myData.GetElementoByScan(this);
             if (_elemento != null) { 
                Descripcion = _elemento.Descripcion;
                IdElemento = _elemento.IdElemento;
                IdProducto = _elemento.IdProducto;
                CodigoBarras = _elemento.CodigoBarras;
            }
        }

    }
}