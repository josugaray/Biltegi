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
    class ElementoAdapter : BaseAdapter
    {

        private readonly List<Negocio.Existencias> _items;
        private readonly Activity _context;
        private Negocio.Existencias _existencias;

        public ElementoAdapter(Activity context, List<Negocio.Existencias> items)
        {
            this._context = context;
            _items = items;

        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            var item = _items[position];
            if (view == null)
            {
                var inflater = LayoutInflater.FromContext(_context);
                view = inflater.Inflate(Resource.Layout.ProductoRow, parent, false);
                view.FindViewById<ImageButton>(Resource.Id.AccionExistenciaProducto).Click += DeleteExistencia;
                view.FindViewById<ImageButton>(Resource.Id.AccionRemove10).Click += Remove10;
                view.FindViewById<ImageButton>(Resource.Id.AccionRemove).Click += Remove;
                view.FindViewById<ImageButton>(Resource.Id.AccionAdd10).Click += Add10;
                view.FindViewById<ImageButton>(Resource.Id.AccionAdd).Click += Add;
            }
            _existencias = item;
            string newId = _existencias.IdExistencias.ToString();
            string newValue = _existencias.FechaEntrada.ToShortDateString();
            string newCantidad = _existencias.Cantidad.ToString();

            view.FindViewById<TextView>(Resource.Id.ValueProducto).Tag = newId;
            view.FindViewById<TextView>(Resource.Id.ValueProducto).Text = newValue;
            view.FindViewById<TextView>(Resource.Id.CantidadProducto).Text = newCantidad;
            view.FindViewById<ImageButton>(Resource.Id.AccionRemove10).Tag = newId;
            view.FindViewById<ImageButton>(Resource.Id.AccionRemove).Tag = newId;
            view.FindViewById<ImageButton>(Resource.Id.AccionAdd10).Tag = newId;
            view.FindViewById<ImageButton>(Resource.Id.AccionAdd).Tag = newId;
            view.FindViewById<ImageButton>(Resource.Id.AccionExistenciaProducto).Tag = newId;


            return view;
        }


        private void DeleteExistencia(object sender, EventArgs e)
        {
            ImageButton menu = (ImageButton)sender;
            var result = UserDialogs.Instance.ConfirmAsync(new Acr.UserDialogs.ConfirmConfig
            {

                Message = _context.GetString(Resource.String.MENSAJE_BORRAR_EXISTENCIA),
                OkText = _context.GetString(Resource.String.MENSAJE_BOTON_OK),
                CancelText = _context.GetString(Resource.String.MENSAJE_BOTON_CANCEL),
            }).ContinueWith(t => _context.RunOnUiThread(
                 () =>
                 {
                     if (t.Result)
                     {
                         Existencias existencia = new Existencias()
                         {
                             IdExistencias = int.Parse(menu.Tag.ToString())
                         };
                         existencia.GetExistencia();
                         existencia.Delete();
                         NotifyDataSetChanged();
                         _context.Recreate();
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
            _context.Recreate();
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
            _context.Recreate();
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
            _context.Recreate();
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
            _context.Recreate();
        }

        public override int Count
        {
            get { return _items.Count; }
        }

    }

    class ElementoAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}