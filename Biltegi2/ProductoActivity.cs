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
using SatelliteMenu;
using System.Collections.Generic;
using Acr.UserDialogs;
using ZXing;
using ZXing.Mobile;

namespace Biltegi2
{
    [Activity(Label = "")]
    public class ProductoActivity : Activity
    {
        private Negocio.Producto _producto;
        private Negocio.Constantes.Acciones AccionEnCurso;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            //inicializar dialogos
            UserDialogs.Init(Application);
            MobileBarcodeScanner.Initialize(Application);
            // Create your application here
            SetContentView(Resource.Layout.DetalleProducto);
            _producto = new Negocio.Producto()
            {
                IdProducto = Intent.GetIntExtra(Negocio.Constantes.MENSAJE_IDPRODUCTO, 0)
            };
            _producto.GetProductoById();

            TextView txt = FindViewById<TextView>(Resource.Id.txtBarraProducto);
            EditText txtEdit = FindViewById<EditText>(Resource.Id.txtEditProducto);
            var button = FindViewById<SatelliteMenu.SatelliteMenuButton>(Resource.Id.menuSateliteProducto);

            button.AddItems(new[] {
               new SatelliteMenuButtonItem (Negocio.Constantes.MENU_SCAN, Resource.Drawable.ic_barcode_scan)
            });
            button.MenuItemClick += MenuItem_Click;

            View vBotonBack = FindViewById<View>(Resource.Id.BackButtonProducto);
            vBotonBack.Click += BotonBack_Click;

            Spinner spinner1 = FindViewById<Spinner>(Resource.Id.spinner1);


            txt.Visibility = ViewStates.Visible;
            txtEdit.Visibility = ViewStates.Gone;
            spinner1.Visibility = ViewStates.Gone;

            txt.Text = _producto.Descripcion;
            txtEdit.Text = _producto.Descripcion;
            if (savedInstanceState != null)
            {
                int accionTemp = savedInstanceState.GetInt(Negocio.Constantes.MENSAJE_ACCION);
                AccionEnCurso = (Negocio.Constantes.Acciones)accionTemp;
                _producto.IdProducto = savedInstanceState.GetInt(Negocio.Constantes.MENSAJE_IDPRODUCTO);
                if (_producto.IdProducto != 0)
                {
                    if (AccionEnCurso == Negocio.Constantes.Acciones.ACCIONES_NONE)
                    {
                        button.AddItems(new[] {
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_ANADIR, Resource.Drawable.ic_add_circle_outline_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_BORRAR, Resource.Drawable.ic_delete_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_EDIT, Resource.Drawable.ic_create_black_24dp)
                       });
                    }
                    else if (AccionEnCurso == Negocio.Constantes.Acciones.ACCIONES_EDIT)
                    {
                        txt.Visibility = ViewStates.Gone;
                        txtEdit.Visibility = ViewStates.Visible;
                        spinner1.Visibility = ViewStates.Visible;
                        List<Negocio.Grupo> ListaGrupo = GestorApp.myData.GetAllGrupos();
                        ListaGrupo.OrderBy(g => g.Descripcion);
                        List<string> ListaGrupoString = GetDescripcionesGrupos(ListaGrupo);
                        ArrayAdapter SpinnerAdapter = new ArrayAdapter(this, Resource.Layout.ProductLayoutSpinner, ListaGrupoString);
                        spinner1.Adapter = SpinnerAdapter;
                        Negocio.Grupo _grupo = _producto.GetGrupo();
                        int posicion = ListaGrupoString.IndexOf(_grupo.Descripcion);
                        spinner1.SetSelection(posicion);
                        button.AddItems(new[] {
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_SALVAR, Resource.Drawable.ic_save_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_CANCELAR, Resource.Drawable.ic_clear_black_24dp),
                       });
                    }
                }

            }
            else
            {
                button.AddItems(new[] {
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_ANADIR, Resource.Drawable.ic_add_circle_outline_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_BORRAR, Resource.Drawable.ic_delete_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_EDIT, Resource.Drawable.ic_create_black_24dp)
                       });
            }

            List<Negocio.Existencias> lExistencias = _producto.GetExistencias();
            List<Negocio.Elemento> lElementos = _producto.GetElementos();

            var listView = FindViewById<ExpandableListView>(Resource.Id.lListaProducto);
            listView.SetAdapter(new Negocio.Adaptadores.ProductoExpandableAdapter(this, lExistencias, lElementos));


        }

        private void MenuItem_Click(object sender, SatelliteMenuItemEventArgs e)
        {
            switch (e.MenuItem.Tag)
            {
                case Negocio.Constantes.MENU_SALVAR:
                    SalvarEdit();
                    break;
                case Negocio.Constantes.MENU_ANADIR:
                    AnadirElemento();
                    break;
                case Negocio.Constantes.MENU_BORRAR:
                    BorrarProducto();
                    break;
                case Negocio.Constantes.MENU_EDIT:
                    ModificarProducto();
                    break;
                case Negocio.Constantes.MENU_CANCELAR:
                    CancelarEdit();
                    break;
                case Negocio.Constantes.MENU_SCAN:
                    Scan();
                    break;
            }
        }
        private void Scan()
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner()
            {
                UseCustomOverlay = false,
                BottomText = GetString(Resource.String.MENSAJE_SCAN)
            };

            try
            {
                scanner.Scan().ContinueWith(t =>
                RunOnUiThread(
               () =>
               {
                   if (t.Result != null)
                   {
                       var result = t.Result;
                       Negocio.Elemento _elemento = new Negocio.Elemento();
                       _elemento.CodigoBarras = result.Text;
                       _elemento.Scan();
                       Intent IntentDetalleElemento = new Intent(this, typeof(ElementoActivity));
                       IntentDetalleElemento.PutExtra(Negocio.Constantes.MENSAJE_IDELEMENTO, _elemento.IdElemento);
                       IntentDetalleElemento.PutExtra(Negocio.Constantes.MENSAJE_CODIGOBARRAS, _elemento.IdElemento);
                       IntentDetalleElemento.PutExtra(Negocio.Constantes.MENSAJE_MODO, Negocio.Constantes.MENSAJE_MODO);
                       StartActivityForResult(IntentDetalleElemento, 0);
                   }
               }));
            }
            catch (ZXing.ReaderException ex)
            {

            };
        }



            private void CancelarEdit()
        {
            AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_NONE;
            Recreate();
        }

        private void ModificarProducto()
        {
            AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_EDIT;
            Recreate();
        }

        private void BorrarProducto()
        {
            string TextoMensaje = GetString(Resource.String.MENSAJE_BORRAR_PRODUCTO);
            TextoMensaje = String.Format(TextoMensaje, _producto.Descripcion);
            var result = UserDialogs.Instance.ConfirmAsync(new Acr.UserDialogs.ConfirmConfig
            {

                Message = TextoMensaje,
                OkText = GetString(Resource.String.MENSAJE_BOTON_OK),
                CancelText = GetString(Resource.String.MENSAJE_BOTON_CANCEL),
            }).ContinueWith(t => RunOnUiThread(
                 () =>
                 {
                     if (t.Result)
                     {
                         _producto.Delete();
                         _producto = new Negocio.Producto();
                         Finish();
                     }

                 }));
        }

        private void AnadirElemento()
        {
            var result = UserDialogs.Instance.PromptAsync(new Acr.UserDialogs.PromptConfig
            {
                Message = GetString(Resource.String.MENSAJE_ANADIR_ELEMENTO),
                OkText = GetString(Resource.String.MENSAJE_BOTON_OK),
                CancelText = GetString(Resource.String.MENSAJE_BOTON_CANCEL),
            }).ContinueWith(t => RunOnUiThread(
                 () =>
                 {

                     if (t.Result != null)
                     {
                         Negocio.Elemento _elemento = new Negocio.Elemento()
                         {
                             Descripcion = t.Result.Text,
                             IdProducto = _producto.IdProducto
                         };

                         if (_elemento.Exists())
                         {
                             CallElementExist();
                         }
                         else
                         {
                             _elemento.Insert();
                             this.Recreate();
                         }
                     }

                 }));

        }

        private void CallElementExist()
        {
            var result = UserDialogs.Instance.AlertAsync(new Acr.UserDialogs.AlertConfig
            {
                Message = GetString(Resource.String.MENSAJE_EXISTE_ELEMENTO)
            });
        }

        private void SalvarEdit()
        {
            EditText txtEdit = FindViewById<EditText>(Resource.Id.txtEditProducto);
            if (txtEdit.Text == "")
            {
                var result = UserDialogs.Instance.AlertAsync(new Acr.UserDialogs.AlertConfig
                {
                    Message = GetString(Resource.String.ALERTA_DESCRIPCION_VACIA),
                    OkText = GetString(Resource.String.MENSAJE_BOTON_OK),
                });


            }
            else { 
                _producto.Descripcion = txtEdit.Text;
            Spinner spinner1 = FindViewById<Spinner>(Resource.Id.spinner1);
            Negocio.Grupo _grupo = new Negocio.Grupo()
            {
                Descripcion = spinner1.SelectedItem.ToString()
            };
            _grupo.GetGrupo();
            _producto.IdGrupo = _grupo.IdGrupo;
            _producto.Update();
            AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_NONE;
            Recreate();
            }
        }

        private void BotonBack_Click(object sender, EventArgs e)
        {
            SetResult(Android.App.Result.Ok);
            Finish();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(Negocio.Constantes.MENSAJE_IDPRODUCTO, _producto.IdProducto);
            if (AccionEnCurso == Negocio.Constantes.Acciones.ACCIONES_EDIT)
            {
                outState.PutInt(Negocio.Constantes.MENSAJE_ACCION, (int)Negocio.Constantes.Acciones.ACCIONES_EDIT);
            }
            else
            {
                outState.PutInt(Negocio.Constantes.MENSAJE_ACCION, (int)Negocio.Constantes.Acciones.ACCIONES_NONE);

            }
            base.OnSaveInstanceState(outState);
        }

        private List<string> GetDescripcionesGrupos(List<Negocio.Grupo> _grupo)
        {
            List<string> lista = new List<string>();
            foreach (Negocio.Grupo g in _grupo)
            {
                lista.Add(g.Descripcion);
            }
            return lista;
        }

        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            Recreate();
        }
    }
}