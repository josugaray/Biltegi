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
    [Table("EXISTENCIAS")]
    public class Existencias
    {
        [PrimaryKey, AutoIncrement]
        public int IdExistencias { get; set; }
        public int IdElemento { get; set; }

        public DateTime FechaEntrada { get; set; }
        public int Cantidad { get; set; }
        
        public DateTime FechaCaducidad { get; set; }
        /// <summary>
        /// Obtiene la existencia por id</summary>
        public void GetExistencia()
        {
            Negocio.Existencias _existencias = GestorApp.myData.GetExistencia(this);
            if (_existencias !=null) { 
                IdExistencias = _existencias.IdExistencias;
                IdElemento = _existencias.IdElemento;
                FechaEntrada = _existencias.FechaEntrada;
                FechaCaducidad = _existencias.FechaCaducidad;
                Cantidad = _existencias.Cantidad;
            }
        }

        /// <summary>
        /// Elimina la existencia</summary>
        public void Delete()
        {
            GestorApp.myData.DeleteExistencia(this);
        }

        /// <summary>
        /// Inserta en base de datos la existencia </summary>
        public void Insert()
        {
            GestorApp.myData.InsertExistencias(this);
        }
        
        /// <summary>
        /// Actualiza en base de datos la existencia </summary>
        public void Update()
        {
            GestorApp.myData.UpdateExistencias(this);
        }

        public void Add(int c) {
            Cantidad += c;
            Update();
        }

        public void Remove(int c)
        {
            Cantidad -= c;
            if (Cantidad < 0) {
                Cantidad = 0;
            }
            Update();
        }
    }
}