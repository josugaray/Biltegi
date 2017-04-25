using SQLite;
using System.Collections.Generic;

namespace Biltegi2.Negocio
{
    [Table("GRUPO")]
    public class Grupo
    {
        [PrimaryKey, AutoIncrement]
        public int IdGrupo { get; set; }
        public string Descripcion { get; set; }


        /// <summary>
        /// Obtiene la Categoria, la busqueda se realiza por Descrion</summary>

        public void GetGrupo()
        {
            Negocio.Grupo _grupo = GestorApp.myData.GetGrupo(this);
            if (_grupo != null) { 
                IdGrupo = _grupo.IdGrupo;
                Descripcion = _grupo.Descripcion;
            }

        }

        public List<Producto> GetProductos()
        {
            return GestorApp.myData.GetProductosGrupo(this);
        }

        public List<Elemento> GetElementos()
        {
            return GestorApp.myData.GetElementosGrupo(this);
        }

        /// <summary>
        /// Obtiene la Categoria, la busqueda se realiza por Descrion</summary>

        public void GetGrupoById()
        {
            Negocio.Grupo _grupo = GestorApp.myData.GetGrupoById(this);
            if (_grupo != null)
            {
                IdGrupo = _grupo.IdGrupo;
                Descripcion = _grupo.Descripcion;
            }

        }
        /// <summary>
        /// Actualiza en base de datos</summary>
        public void Update()
        {
            GestorApp.myData.UpdateGrupo(this);
        }
        /// <summary>
        /// Elimina de base de datos</summary>
        public void Delete()
        {
            List<Producto> _productos = GetProductos();
            foreach (Producto p in _productos) {
                p.Delete();
            }
            GestorApp.myData.DeleteGrupo(this);
        }

        /// <summary>
        /// Inserta en base de datos</summary>
        public void Insert()
        {
            GestorApp.myData.InsertGrupo(this);
            GetGrupo();
        }

    }
}