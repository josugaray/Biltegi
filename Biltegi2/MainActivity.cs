using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SatelliteMenu;
using System.Collections.Generic;
using Acr.UserDialogs;
using ZXing;
using ZXing.Mobile;

namespace Biltegi2
{
    [Activity(Label = "Biltegi", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Negocio.Grupo _grupo;
        private Negocio.Constantes.Acciones AccionEnCurso;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MobileBarcodeScanner.Initialize(Application);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Inicializar base de datos
            GestorApp.myData = new Biltegi2.Negocio.BD.BDManager();
            GestorApp.myData.SetContext(this);

            //inicializar dialogos
            UserDialogs.Init(Application);

            //Inicializar Menu
            var menu = FindViewById<FlyOutContainer>(Resource.Id.FlyOutContainer);
            var menuButton = FindViewById(Resource.Id.MenuButton);
            var button = FindViewById<SatelliteMenuButton>(Resource.Id.menuSatelite);
            menuButton.Click += (sender, e) =>
            {
                menu.AnimatedOpened = !menu.AnimatedOpened;

                if (menu.AnimatedOpened)
                {
                    button.Visibility = ViewStates.Visible;
                } else
                {
                    button.Visibility = ViewStates.Invisible;
                    
                }
            };

            button.AddItems(new[] {
               new SatelliteMenuButtonItem (Negocio.Constantes.MENU_SCAN, Resource.Drawable.ic_barcode_scan)
            });

            button.MenuItemClick += MenuItem_Click;

            //Añadir elementos al menú de forma dinamica
            List<Negocio.Grupo> ListaGrupos = GestorApp.myData.GetAllGrupos();

            View vBotonAdd = FindViewById<View>(Resource.Id.addGroupButton);
            vBotonAdd.Click += BotonAdd_Click;

            LinearLayout lvPadre = FindViewById<LinearLayout>(Resource.Id.layoutpadre);
            LinearLayout lvDummy = FindViewById<LinearLayout>(Resource.Id.layoutCategorias);
            
            TextView txtCategorias= FindViewById<TextView>(Resource.Id.txtTodasCategorias);
            txtCategorias.Click += TxtCategorias_Click;
      
            _grupo = new Negocio.Grupo();
            TextView txt = FindViewById<TextView>(Resource.Id.txtBarraMain);
            EditText txtEdit = FindViewById<EditText>(Resource.Id.txtEditMain);
            txt.Text = GetString(Resource.String.TODOS);
            txt.Visibility = ViewStates.Visible;
            txtEdit.Visibility = ViewStates.Gone;
            if (bundle != null) { 
               _grupo.IdGrupo = bundle.GetInt(Negocio.Constantes.MENSAJE_IDGRUPO);
                int accionTemp = bundle.GetInt(Negocio.Constantes.MENSAJE_ACCION);
                AccionEnCurso = (Negocio.Constantes.Acciones) accionTemp;
                if (_grupo.IdGrupo != 0) {
                    _grupo.GetGrupoById();
                    if (AccionEnCurso == Negocio.Constantes.Acciones.ACCIONES_NONE) {
                        txt.Text = _grupo.Descripcion;
                        button.AddItems(new[] {
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_ANADIR, Resource.Drawable.ic_add_circle_outline_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_BORRAR, Resource.Drawable.ic_delete_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_EDIT, Resource.Drawable.ic_create_black_24dp)
                       });
                     }
                    else if (AccionEnCurso == Negocio.Constantes.Acciones.ACCIONES_EDIT)
                    {
                        txtEdit.Text = _grupo.Descripcion;
                        txt.Visibility = ViewStates.Gone;
                        txtEdit.Visibility = ViewStates.Visible;
                        button.AddItems(new[] {
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_SALVAR, Resource.Drawable.ic_save_black_24dp),
                       new SatelliteMenuButtonItem (Negocio.Constantes.MENU_CANCELAR, Resource.Drawable.ic_clear_black_24dp),
                       });
                    }
                }
            }
            else
            {
                txtEdit.Visibility = ViewStates.Invisible;
            }

            foreach (Negocio.Grupo g in ListaGrupos)
            {
                LinearLayout lvHijo = new LinearLayout(this);
                TextView txtHijo = new TextView(this);            
                lvHijo.LayoutParameters = lvDummy.LayoutParameters;
                txtHijo.LayoutParameters = txtCategorias.LayoutParameters;
                txtHijo.Text = g.Descripcion;
                txtHijo.Tag = g.IdGrupo.ToString();
                txtHijo.Click += TxtHijo_Click;
                lvHijo.AddView(txtHijo);
                if (g.IdGrupo == _grupo.IdGrupo)
                {
                    lvHijo.SetBackgroundColor(Android.Graphics.Color.Black);
                }
                lvPadre.AddView(lvHijo);
            }
            List<Negocio.Producto> lProductos; 
            List<Negocio.Elemento> lElementos;
            if (_grupo.IdGrupo == 0)
            {
                lProductos = GestorApp.myData.GetAllProductos();
                lElementos = GestorApp.myData.GetAllElementos();
            } else
            {
                lProductos = _grupo.GetProductos();
                lElementos = _grupo.GetElementos();
            }
            

            var listView = FindViewById<ExpandableListView>(Resource.Id.lLista);
            listView.SetAdapter(new Negocio.Adaptadores.ExpandableDataAdapter(this, lElementos, lProductos));

        }

        private void TxtCategorias_Click(object sender, EventArgs e)
        {
            _grupo.IdGrupo = 0;
            Recreate();
        }

        private void BotonAdd_Click(object sender, EventArgs e)
        {
           AnadirGrupo();
        }

        private void MenuItem_Click(object sender, SatelliteMenuItemEventArgs e)
        {
           switch (e.MenuItem.Tag)
           {
                case Negocio.Constantes.MENU_SALVAR:
                    SalvarEdit();
                    break;
                case Negocio.Constantes.MENU_ANADIR:
                    AnadirPoducto();
                    break;
                case Negocio.Constantes.MENU_BORRAR:
                    BorrarGrupo();
                    break;
                case Negocio.Constantes.MENU_EDIT:
                    ModificarGrupo();
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
                       Intent IntentDetalleElemento = new Intent(this, typeof(ElementoActivity));                       
                       _elemento.CodigoBarras = result.Text;
                       IntentDetalleElemento.PutExtra(Negocio.Constantes.MENSAJE_CODIGOBARRAS, _elemento.CodigoBarras);
                       IntentDetalleElemento.PutExtra(Negocio.Constantes.MENSAJE_MODO, Negocio.Constantes.MENSAJE_MODO);
                       StartActivityForResult(IntentDetalleElemento, 0);
                   }
               }));
            }
            catch (ZXing.ReaderException ex)
            {

            };
            
        }

        private void SalvarEdit()
        {
            EditText txtEdit = FindViewById<EditText>(Resource.Id.txtEditMain);
            _grupo.Descripcion = txtEdit.Text;
            _grupo.Update();
            AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_NONE;
            Recreate();
        }

        private void CancelarEdit()
        {
            AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_NONE;
            Recreate();
        }

        private void ModificarGrupo()
        {
            AccionEnCurso = Negocio.Constantes.Acciones.ACCIONES_EDIT;
            Recreate();
        }

        private void BorrarGrupo()
        {
            string TextoMensaje = GetString(Resource.String.MENSAJE_BORRAR_GRUPO);
            TextoMensaje = String.Format(TextoMensaje, _grupo.Descripcion);
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
                         _grupo.Delete();
                         _grupo = new Negocio.Grupo();
                         this.Recreate();
                     }                   

                 }));
        }

        private void AnadirGrupo()
        {

            var result = UserDialogs.Instance.PromptAsync(new Acr.UserDialogs.PromptConfig
            {
                Message = GetString(Resource.String.MENSAJE_ANADIR_GRUPO),
                OkText = GetString(Resource.String.MENSAJE_BOTON_OK),
                CancelText = GetString(Resource.String.MENSAJE_BOTON_CANCEL),               
            }).ContinueWith(t => RunOnUiThread(
                 () =>
                 {

                     if (t.Result != null)
                     {
                         _grupo = new Negocio.Grupo()
                         {
                             Descripcion = t.Result.Text
                         };
                         _grupo.Insert();
                         this.Recreate();
                     }

                 }));
            
            
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(Negocio.Constantes.MENSAJE_IDGRUPO, _grupo.IdGrupo);
            if (AccionEnCurso == Negocio.Constantes.Acciones.ACCIONES_EDIT)
            {
                outState.PutInt(Negocio.Constantes.MENSAJE_ACCION, (int) Negocio.Constantes.Acciones.ACCIONES_EDIT);
            }
            else
            {
                outState.PutInt(Negocio.Constantes.MENSAJE_ACCION, (int)Negocio.Constantes.Acciones.ACCIONES_NONE);

            }
             base.OnSaveInstanceState(outState);
        }

        private void AnadirPoducto()
        {

            var result = UserDialogs.Instance.PromptAsync(new Acr.UserDialogs.PromptConfig
            {
                Message = GetString(Resource.String.MENSAJE_ANADIR_PRODUCTO),
                OkText = GetString(Resource.String.MENSAJE_BOTON_OK),
                CancelText = GetString(Resource.String.MENSAJE_BOTON_CANCEL),
            }).ContinueWith(t => RunOnUiThread(
                 () =>
                 {

                     if (t.Result != null)
                     {
                         Negocio.Producto _producto = new Negocio.Producto()
                         {
                             Descripcion = t.Result.Text,
                             IdGrupo = _grupo.IdGrupo
                         };

                         if (_producto.Exists())
                         {
                             CallProductExist();
                         } else { 
                             _producto.Insert();
                              this.Recreate();
                         }
                     }

                 }));

        }

        private void CallProductExist()
        {
            var result = UserDialogs.Instance.AlertAsync(new Acr.UserDialogs.AlertConfig
            {
                Message = GetString(Resource.String.MENSAJE_EXISTE_PRODUCTO)
            });

        }


        private void TxtHijo_Click(object sender, EventArgs e)
        {
            TextView texto = (TextView)sender;
            _grupo = new Negocio.Grupo();
            _grupo.IdGrupo = int.Parse(texto.Tag.ToString());
            _grupo.Descripcion = texto.Text;
            this.Recreate();

        }


        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Recreate();
        }
    }
}

