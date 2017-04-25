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

namespace Biltegi2.Negocio.Adaptadores
{
    class ExpandableDataAdapter : BaseExpandableListAdapter
    {
        readonly Activity Context;
        public ExpandableDataAdapter(Activity newContext, List<Elemento> newList, List<Producto> producto ) : base()
        {
            Context = newContext;
            DataList = newList;
            _productos = producto;
            
        }

        protected List<Elemento> DataList { get; set; }
        protected List<Producto> _productos{ get; set; }
        protected int _posicion { get; set; }
        protected int _posicionHijo { get; set; }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            if (header == null)
            {
                header = Context.LayoutInflater.Inflate(Resource.Layout.MainListGroup, null);
                header.FindViewById<ImageButton>(Resource.Id.AccionGrupo).Click += AccionGrupo_Click;
            }
            header.FindViewById<TextView>(Resource.Id.DataHeader).Text = _productos[groupPosition].Descripcion;
            header.FindViewById<TextView>(Resource.Id.DataHeader).Tag = _productos[groupPosition].IdProducto.ToString();
            _posicion = groupPosition;
            header.FindViewById<ImageButton>(Resource.Id.AccionGrupo).Tag = groupPosition.ToString();
            
            return header;
        }

        private void AccionGrupo_Click(object sender, EventArgs e)
        {
            ImageButton menu = (ImageButton)sender;
            Intent IntentDetalleProducto = new Intent(Context, typeof(ProductoActivity));
            IntentDetalleProducto.PutExtra(Negocio.Constantes.MENSAJE_IDPRODUCTO, _productos[int.Parse(menu.Tag.ToString())].IdProducto);
            ((Activity)Context).StartActivityForResult(IntentDetalleProducto, 0);
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = Context.LayoutInflater.Inflate(Resource.Layout.MainDataListItem, null);
                row.FindViewById<ImageButton>(Resource.Id.AccionSubgrupo).Click += AccionElemento_Click;
            }
            string newId = "", newValue = "";
            GetChildViewHelper(groupPosition, childPosition, out newId, out newValue);
            row.FindViewById<TextView>(Resource.Id.DataValue).Tag = newId;
            row.FindViewById<TextView>(Resource.Id.DataValue).Text = newValue;
            row.FindViewById<ImageButton>(Resource.Id.AccionSubgrupo).Tag = childPosition.ToString();
            _posicionHijo = childPosition;
            return row;
        }

        private void AccionElemento_Click(object sender, EventArgs e)
        {
            ImageButton menu = (ImageButton)sender;
            Intent IntentDetalleElemento = new Intent(Context, typeof(ElementoActivity));
            IntentDetalleElemento.PutExtra(Negocio.Constantes.MENSAJE_IDELEMENTO, DataList[int.Parse(menu.Tag.ToString())].IdElemento);
            ((Activity)Context).StartActivityForResult(IntentDetalleElemento, 0);
        }

        public override int GetChildrenCount(int groupPosition)
        {
             List<Elemento> results = DataList.FindAll((Elemento obj) => obj.IdProducto == _productos[groupPosition].IdProducto);
            return results.Count;
        }

        public override int GroupCount
        {
            get
            {
                return _productos.Count;
            }
        }

        private void GetChildViewHelper(int groupPosition, int childPosition, out string Id, out string Value)
        {
       
            List<Elemento> results = DataList.FindAll((Elemento obj) => obj.IdProducto == _productos[groupPosition].IdProducto);
            Id = results[childPosition].IdProducto.ToString();
            Value = results[childPosition].Descripcion;
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

        private int GetElementoAbsolutePosition (Negocio.Elemento _elemento)
        {
            int resultado;
            resultado = 0;
            foreach (Elemento e in DataList)
            {
                if (e.Descripcion == _elemento.Descripcion)
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

        #endregion
    }
}
