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
using Acr.UserDialogs;


namespace Biltegi2.Negocio.Adaptadores
{
    class ProductoExpandableAdapter : BaseExpandableListAdapter
    {

        readonly Activity Context;
        public ProductoExpandableAdapter(Activity newContext, List<Existencias> newList, List<Elemento> elementos) : base()
        {
            Context = newContext;
            DataList = newList;
            _cabeceraElementos = elementos;

        }

        protected List<Existencias> DataList { get; set; }
        protected List<Elemento> _cabeceraElementos { get; set; }
        private int _posicion;
        private int _posicionHijo;

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            if (header == null)
            {
                header = Context.LayoutInflater.Inflate(Resource.Layout.CabecerasProducto, null);
                header.FindViewById<ImageButton>(Resource.Id.AccionElementoCabecera).Click += AccionElemento_Click;
            }
            header.FindViewById<TextView>(Resource.Id.DataHeaderProducto).Text = _cabeceraElementos[groupPosition].Descripcion;
            header.FindViewById<TextView>(Resource.Id.DataHeaderProducto).Tag = _cabeceraElementos[groupPosition].IdProducto.ToString();
            _posicion = groupPosition;
            header.FindViewById<ImageButton>(Resource.Id.AccionElementoCabecera).Tag = groupPosition.ToString();
            return header;
        }

        private void AccionElemento_Click(object sender, EventArgs e)
        {
            ImageButton menu = (ImageButton)sender;
            Intent IntentDetalleElemento = new Intent(Context, typeof(ElementoActivity));
            IntentDetalleElemento.PutExtra(Negocio.Constantes.MENSAJE_IDELEMENTO, _cabeceraElementos[int.Parse(menu.Tag.ToString())].IdElemento);
            ((Activity)Context).StartActivityForResult(IntentDetalleElemento, 0);
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = Context.LayoutInflater.Inflate(Resource.Layout.ProductoRow, null);
                row.FindViewById<ImageButton>(Resource.Id.AccionExistenciaProducto).Click += DeleteExistencia;
                row.FindViewById<ImageButton>(Resource.Id.AccionRemove10).Click += Remove10;
                row.FindViewById<ImageButton>(Resource.Id.AccionRemove).Click += Remove;
                row.FindViewById<ImageButton>(Resource.Id.AccionAdd10).Click += Add10;
                row.FindViewById<ImageButton>(Resource.Id.AccionAdd).Click += Add;
;
            }
            string newId = "", newValue = "", newCantidad = "";
            GetChildViewHelper(groupPosition, childPosition, out newId, out newValue, out newCantidad);
            row.FindViewById<TextView>(Resource.Id.ValueProducto).Tag = newId;
            row.FindViewById<TextView>(Resource.Id.ValueProducto).Text = newValue;
            row.FindViewById<TextView>(Resource.Id.CantidadProducto).Text = newCantidad;
            row.FindViewById<ImageButton>(Resource.Id.AccionRemove10).Tag = newId;
            row.FindViewById<ImageButton>(Resource.Id.AccionRemove).Tag = newId;
            row.FindViewById<ImageButton>(Resource.Id.AccionAdd10).Tag = newId;
            row.FindViewById<ImageButton>(Resource.Id.AccionAdd).Tag = newId;
            row.FindViewById<ImageButton>(Resource.Id.AccionExistenciaProducto).Tag = newId;
            return row;
        }

        private void DeleteExistencia(object sender, EventArgs e)
        {
            ImageButton menu = (ImageButton)sender;
            var result = UserDialogs.Instance.ConfirmAsync(new Acr.UserDialogs.ConfirmConfig
            {

                Message = Context.GetString(Resource.String.MENSAJE_BORRAR_EXISTENCIA),
                OkText = Context.GetString(Resource.String.MENSAJE_BOTON_OK),
                CancelText = Context.GetString(Resource.String.MENSAJE_BOTON_CANCEL),
            }).ContinueWith(t => Context.RunOnUiThread(
                 () =>
                 {
                     if (t.Result)
                     {
                         Existencias existencia = new Existencias()
                         {
                             IdExistencias = int.Parse(menu.Tag.ToString())
                         };
                         existencia.GetExistencia();
                         DataList.Remove(existencia);
                         existencia.Delete();
                         NotifyDataSetChanged();
                         Context.Recreate();
                     }

                 }));
        }

        private void Remove10(object sender, EventArgs e)
        {
            ImageButton menu = (ImageButton)sender;
            Existencias existencia = new Existencias()
            {
                IdExistencias = int.Parse(menu.Tag.ToString())
            };
            existencia.GetExistencia();
            existencia.Remove(10);
            NotifyDataSetChanged();
            Context.Recreate();
        }

        private void Remove(object sender, EventArgs e)
        {
            ImageButton menu = (ImageButton)sender;
            Existencias existencia = new Existencias()
            {
                IdExistencias = int.Parse(menu.Tag.ToString())
            };
            existencia.GetExistencia();
            existencia.Remove(1);
            NotifyDataSetChanged();
            Context.Recreate();
        }

        private void Add(object sender, EventArgs e)
        {
            ImageButton menu = (ImageButton)sender;
            Existencias existencia = new Existencias()
            {
                IdExistencias = int.Parse(menu.Tag.ToString())
            };
            existencia.GetExistencia();
            existencia.Add(1);
            NotifyDataSetChanged();
            Context.Recreate();
        }

        private void Add10(object sender, EventArgs e)
        {
            ImageButton menu = (ImageButton)sender;
            Existencias existencia = new Existencias()
            {
                IdExistencias = int.Parse(menu.Tag.ToString())
            };
            existencia.GetExistencia();
            existencia.Add(10);
            NotifyDataSetChanged();
            Context.Recreate();
        }

        public override int GetChildrenCount(int groupPosition)
        {
            List<Existencias> results = DataList.FindAll((Existencias obj) => obj.IdElemento == _cabeceraElementos[groupPosition].IdElemento);
            return results.Count;
        }

        public override int GroupCount
        {
            get
            {
                return _cabeceraElementos.Count;
            }
        }

        private void GetChildViewHelper(int groupPosition, int childPosition, out string Id, out string Value, out string Ammount)
        {

            List<Existencias> results = DataList.FindAll((Existencias obj) => obj.IdElemento == _cabeceraElementos[groupPosition].IdElemento);
            Id = results[childPosition].IdExistencias.ToString();
            Value = results[childPosition].FechaEntrada.ToShortDateString();
            Ammount = results[childPosition].Cantidad.ToString();
        }
        #region implemented abstract members of BaseExpandableListAdapter

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public override bool HasStableIds
        {
            get
            {
                return true;
            }
        }

        private int GetElementoAbsolutePosition(Negocio.Existencias _existencia)
        {
            int resultado;
            resultado = 0;
            foreach (Existencias e in DataList)
            {
                if ((e.FechaEntrada == _existencia.FechaEntrada) && (e.FechaCaducidad == _existencia.FechaCaducidad))
                {
                    return resultado;

                }
                else
                {
                    resultado++;
                }
            }

            return resultado;
        }

    }
    #endregion
    class ProductoExpandableAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}