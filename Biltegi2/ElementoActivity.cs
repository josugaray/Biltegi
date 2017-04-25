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
    public class ElementoActivity : Activity
    {
        private Negocio.Elemento _elemento;
        private Negocio.Constantes.Acciones AccionEnCurso;
 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            MobileBarcodeScanner.Initialize(Application);
            // Create your application here
            SetContentView(Resource.Layout.DetalleElemento);
            //inicializar dialogos
            UserDialogs.Init(Application);

            _elemento = new Negocio.Elemento();
            
             if (savedInstanceState != null)
            {
                AccionEnCurso = (Negocio.Constantes.Acciones)savedInstanceState.GetInt(Negocio.Constantes.MENSAJE_ACCION);
                _elemento.IdElemento = savedInstanceState.GetInt(Negocio.Constantes.MENSAJE_IDELEMENTO);
                _elemento.GetElementoById();
            }
            else {
                string Modo = Intent.GetStringExtra(Negocio.Constantes.MENSAJE_MODO);
                if (Modo == Negocio.Constantes.MENSAJE_MODO)
                {
                    AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_NONE;
                    _elemento.CodigoBarras = Intent.GetStringExtra(Negocio.Constantes.MENSAJE_CODIGOBARRAS);
                    _elemento.Scan();
                    if (_elemento.IdElemento == 0)
                    {
                        //Mensaje indicando que no existe

                        string TextoMensaje = GetString(Resource.String.MENSAJE_NO_SCAN);
                        var result = UserDialogs.Instance.ConfirmAsync(new Acr.UserDialogs.ConfirmConfig
                        {

                            Message = TextoMensaje,
                            OkText = GetString(Resource.String.MENSAJE_BOTON_OK),
                            CancelText = GetString(Resource.String.MENSAJE_BOTON_CANCEL),
                        }).ContinueWith(t => RunOnUiThread(
                             () =>
                             {
                                 if (!t.Result)
                                 {
                                     this.Finish();
                                 }
                                 else
                                 {
                                     AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_ADD;
                                 }

                             }));
                    }
                                    
                } else
                {                
                    _elemento.IdElemento = Intent.GetIntExtra(Negocio.Constantes.MENSAJE_IDELEMENTO, 0);                
                    if (_elemento.IdElemento != 0)                {
                        AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_NONE;
                    } else
                    {
                        AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_ADD;
                    }
                    _elemento.GetElementoById();
                }
            }

            
            View vBotonBack = FindViewById<View>(Resource.Id.BackButtonElemento);
            vBotonBack.Click += BotonBack_Click;

            var button = FindViewById<SatelliteMenu.SatelliteMenuButton>(Resource.Id.menuSateliteElemento);

           button.MenuItemClick += MenuItem_Click;

            LinearLayout linearLayoutProducto = FindViewById<LinearLayout>(Resource.Id.linearLayoutProducto);
            LinearLayout linearLayoutCategoria = FindViewById<LinearLayout>(Resource.Id.linearLayoutCategoria);
            Spinner spinnerGrupo = FindViewById<Spinner>(Resource.Id.spinnerGrupoElemento);
            EditText txtCodigo = FindViewById<EditText>(Resource.Id.txtCodigoBarras);
            TextView txt = FindViewById<TextView>(Resource.Id.txtBarraElemento);
            EditText txtEdit = FindViewById<EditText>(Resource.Id.txtEditElemento);
            //  Spinner spinnerProducto = FindViewById<Spinner>(Resource.Id.spinnerProductoElemento);
            txt.Text = _elemento.Descripcion;
            txtEdit.Text = _elemento.Descripcion;
                    
            switch (AccionEnCurso){
                case Negocio.Constantes.Acciones.ACCIONES_NONE:
                    button.AddItems(new[] {
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_ANADIR, Resource.Drawable.ic_add_circle_outline_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_BORRAR, Resource.Drawable.ic_delete_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_EDIT, Resource.Drawable.ic_create_black_24dp)
                       });
                    linearLayoutCategoria.Visibility = ViewStates.Gone;
                    linearLayoutProducto.Visibility = ViewStates.Gone;
                    txt.Visibility = ViewStates.Visible;
                    txtEdit.Visibility = ViewStates.Gone;
                    txtCodigo.InputType = 0;
                    if ((_elemento.CodigoBarras == "") || (_elemento.CodigoBarras == null))
                    {
                        txtCodigo.Text = GetString(Resource.String.NOCODIGOBARRAS);
                    }
                    else
                    {
                        txtCodigo.Text = _elemento.CodigoBarras;
                    }
                    break;
                case Negocio.Constantes.Acciones.ACCIONES_ADD:
                case Negocio.Constantes.Acciones.ACCIONES_EDIT:
                    button.AddItems(new[] {
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_SALVAR, Resource.Drawable.ic_save_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_CANCELAR, Resource.Drawable.ic_clear_black_24dp),
                       });
                    linearLayoutCategoria.Visibility = ViewStates.Visible;
                    linearLayoutProducto.Visibility = ViewStates.Visible;
                    List<Negocio.Grupo> ListaGrupo = GestorApp.myData.GetAllGrupos();
                    ListaGrupo.OrderBy(g => g.Descripcion);
                    List<string> ListaGrupoString = GetDescripcionesGrupos(ListaGrupo);
                    ArrayAdapter SpinnerAdapter = new ArrayAdapter(this, Resource.Layout.ProductLayoutSpinner, ListaGrupoString);
                    spinnerGrupo.Adapter = SpinnerAdapter;
                    Negocio.Grupo _grupo = _elemento.GetGrupo();
                    spinnerGrupo.ItemSelected += SpinnerGrupo_Changed;
                    int posicion = ListaGrupoString.IndexOf(_grupo.Descripcion);
                    spinnerGrupo.SetSelection(posicion);
                    if ((_elemento.CodigoBarras == "") || (_elemento.CodigoBarras == null))
                    {
                        txtCodigo.Text = GetString(Resource.String.NOCODIGOBARRAS);
                    }
                    else
                    {
                        txtCodigo.Text = _elemento.CodigoBarras;
                    }

                    txtCodigo.Click += TxtCodigo_Click;
                    txt.Visibility = ViewStates.Gone;
                    txtEdit.Visibility = ViewStates.Visible;
                    break;
            }
            _elemento.GetExistencias();


            var listView = FindViewById<ListView>(Resource.Id.lListaElemento);
            listView.Adapter = new Negocio.Adaptadores.ElementoAdapter(this, _elemento.lExistencias);


        }

        private void TxtCodigo_Click(object sender, EventArgs e)
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
                       EditText txtCodigo = FindViewById<EditText>(Resource.Id.txtCodigoBarras);
                       txtCodigo.Text = _elemento.CodigoBarras;

                   }
               }));
            }
            catch (ZXing.ReaderException ex)
            {

            };


        }

        private void SpinnerGrupo_Changed(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinnerGrupo = (Spinner)sender;
            Negocio.Grupo _grupo = new Negocio.Grupo()
            {
                Descripcion = spinnerGrupo.SelectedItem.ToString()
            };
            _grupo.GetGrupo();
            List<string> ListaProductosString = GetDescripcionesProductos(_grupo.GetProductos());
            Spinner spinnerProducto = FindViewById<Spinner>(Resource.Id.spinnerProductoElemento);
            ArrayAdapter SpinnerAdapter = new ArrayAdapter(this, Resource.Layout.ProductLayoutSpinner, ListaProductosString);
            spinnerProducto.Adapter = SpinnerAdapter;
            if (_elemento.IdElemento != 0) {
                _elemento.GetProducto();
               int posicion = ListaProductosString.IndexOf(_elemento._producto.Descripcion);
                spinnerProducto.SetSelection(posicion);
             }
        }

        private void BotonBack_Click(object sender, EventArgs e)
        {
            SetResult(Android.App.Result.Ok);
            Finish();
        }

        private void MenuItem_Click(object sender, SatelliteMenuItemEventArgs e)
        {
            switch (e.MenuItem.Tag)
            {
                case Negocio.Constantes.MENU_SALVAR:
                    SalvarEdit();
                    break;
                case Negocio.Constantes.MENU_ANADIR:
                    AnadirExistencia();
                    break;
                case Negocio.Constantes.MENU_BORRAR:
                    BorrarElemento();
                    break;
                case Negocio.Constantes.MENU_EDIT:
                    ModificarExistencia();
                    break;
                case Negocio.Constantes.MENU_CANCELAR:
                    CancelarEdit();
                    break;
            }
        }
        
        private void CancelarEdit()
        {
            AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_NONE;
            Recreate();
        }

        private void ModificarExistencia()
        {
            AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_EDIT;
            Recreate();
        }

        private void BorrarElemento()
        {
            string TextoMensaje = GetString(Resource.String.MENSAJE_BORRAR_ELEMENTO);
            TextoMensaje = String.Format(TextoMensaje, _elemento.Descripcion);
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
                         _elemento.Delete();
                         _elemento = new Negocio.Elemento();
                         Finish();
                     }

                 }));
        }

        private void AnadirExistencia()
        {



            var result = UserDialogs.Instance.DatePromptAsync(new Acr.UserDialogs.DatePromptConfig
            {
                OkText = GetString(Resource.String.MENSAJE_BOTON_OK),
            }).ContinueWith(t => RunOnUiThread(
                 () =>
                 {

                     if (t.Result != null)
                     {
                         Negocio.Existencias existencia = new Negocio.Existencias()
                         {
                             FechaEntrada = t.Result.SelectedDate,
                             IdElemento = _elemento.IdElemento,
                             Cantidad = 0                             
                         };
                         existencia.Insert();
                         Recreate();
                     }

                 }));

        }

        private void SalvarEdit()
        {
            EditText txtEdit = FindViewById<EditText>(Resource.Id.txtEditElemento);
            if (txtEdit.Text=="")
            {
                var result = UserDialogs.Instance.AlertAsync(new Acr.UserDialogs.AlertConfig
                {
                    Message = GetString(Resource.String.ALERTA_DESCRIPCION_VACIA),
                    OkText = GetString(Resource.String.MENSAJE_BOTON_OK),
                });


            } else
            {
                Spinner spinner1 = FindViewById<Spinner>(Resource.Id.spinnerProductoElemento);
                EditText txtCodigo = FindViewById<EditText>(Resource.Id.txtCodigoBarras);
                Negocio.Producto _producto = new Negocio.Producto()
                {
                    Descripcion = spinner1.SelectedItem.ToString()
                };
                _producto.GetProducto();
                _elemento.IdProducto = _producto.IdProducto;
                _elemento.CodigoBarras = txtCodigo.Text;
                _elemento.Update();
                AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_NONE;
                Recreate();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(Negocio.Constantes.MENSAJE_IDELEMENTO, _elemento.IdElemento);
            if (AccionEnCurso == Negocio.Constantes.Acciones.ACCIONES_EDIT)
            {
                outState.PutInt(Negocio.Constantes.MENSAJE_ACCION, (int)Negocio.Constantes.Acciones.ACCIONES_EDIT);
            }
            else if (AccionEnCurso == Negocio.Constantes.Acciones.ACCIONES_NONE)
            {
                outState.PutInt(Negocio.Constantes.MENSAJE_ACCION, (int)Negocio.Constantes.Acciones.ACCIONES_NONE);
            }
            else
            {
                outState.PutInt(Negocio.Constantes.MENSAJE_ACCION, (int)Negocio.Constantes.Acciones.ACCIONES_ADD);
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

        private List<string> GetDescripcionesProductos(List<Negocio.Producto> _producto)
        {
            List<string> lista = new List<string>();
            foreach (Negocio.Producto p in _producto)
            {
                lista.Add(p.Descripcion);
            }
            return lista;
        }
    }
}