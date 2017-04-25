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

namespace Biltegi2.Negocio
{
     static class  Constantes
    {
       public const int MENU_SALVAR= 1;
        public const int MENU_ANADIR = 2;
        public const int MENU_BORRAR = 3;
        public const int MENU_SCAN = 4;
        public const int MENU_EDIT = 5;
        public const int MENU_CANCELAR = 6;

        public const string MENSAJE_IDGRUPO = "IDGRUPO";
        public const string MENSAJE_ACCION = "ACCION";
        public const string MENSAJE_IDPRODUCTO = "IDPRODUCTO";
        public const string MENSAJE_IDELEMENTO = "IDELEMENTO";
        public const string MENSAJE_CODIGOBARRAS = "CODIGOBARRAS";
        public const string MENSAJE_MODO = "MODO";

        public enum Acciones
        {
            ACCIONES_NONE = 0,
            ACCIONES_EDIT = 1,
            ACCIONES_ADD =2
        }

    }
}