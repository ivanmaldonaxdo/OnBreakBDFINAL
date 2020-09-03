using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OnBreak.Negocio;
using MahApps.Metro.Controls.Dialogs;


namespace OnBreakWPF
{
    /// <summary>
    /// Lógica de interacción para AdminClientes.xaml
    /// </summary>
    public partial class AdminClientes : MetroWindow
    {
        public string RutListado;
        public bool estadodark;
        public AdminClientes()
        {
            InitializeComponent();
            CargarActividad();
            CargarEmpresa();
            BuscarPorListado();
        }

        private void CargarEmpresa()
        {
            TipoEmpresa empresa = new TipoEmpresa();
            cboTipoemp.DisplayMemberPath = "Descripcion";
            cboTipoemp.SelectedValuePath = "IdTipoEmpresa";
            cboTipoemp.ItemsSource = empresa.ReadAll();
            cboTipoemp.SelectedIndex = -1;
        }

        public void CargarActividad()
        {
            ActividadEmpresa actividad = new ActividadEmpresa();
            cboActividad.ItemsSource = actividad.ReadAll();
            cboActividad.DisplayMemberPath = "Descripcion";
            cboActividad.SelectedValuePath = "IdActividadEmpresa";
            cboActividad.SelectedIndex = -1;

        }


        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtDireccion_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal vp = new VentanaPrincipal();
            if (estadodark)
            {
                vp.ThemeDark();
            }
            this.Close();
            vp.Show();
        }


        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarControls();
        }

        private void btnListarcli_Click(object sender, RoutedEventArgs e)
        {
            ListadoClientes listcli = new ListadoClientes();
            if (estadodark)
            {
                listcli.ThemeDark();
            }
            this.Close();

            listcli.Show();
            listcli.btnSelect.Visibility = Visibility.Visible;
            listcli.btnBack.Visibility = Visibility.Visible;
            listcli.cambiowindow = true;
        }

        private void swDarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            ThemeDark();
        }

        private void swDarkTheme_IsCheckedChanged(object sender, EventArgs e)
        {
            ThemeLight();
        }



        #region BOTON REGISTRO
        private async void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            
            //Array que se le almacena el nombre contacto separado
            string[] separenombre = txtNombre.Text.Split(' ');

            if (string.IsNullOrEmpty(txtRut.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Escriba bien el campo rut y respete el formato"));
            }
            else if (separenombre.Length != 2)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Por favor respete el orden de 'Nombre Apellido' !!!", txtRut.Text));
            }
            else if (string.IsNullOrEmpty(txtRazonSocial.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo 'Razón Social'!!!"));
            }
            else if (string.IsNullOrEmpty(txtMail.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo 'Email Contacto'!!!"));
            }  

            else if (string.IsNullOrEmpty(txtTelefono.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo 'telefono'!!!"));
            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo 'Dirección'!!!"));
            }
                  
            else if (cboActividad.SelectedIndex < 0 || cboTipoemp.SelectedIndex < 0)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Seleccione los algun item de cada 'Actividad' o 'Tipo Empresa'!!!"));
            }
            else
            {
                
                //Extraccion id (combobox.selectedValuePath )conviertiendo el valor del combobox a entero
                int valueIDemp = (int)cboTipoemp.SelectedValue;
                int valueIDactiv = (int)cboActividad.SelectedValue;
                Cliente cliente = new Cliente()
                {
                    RutCliente = txtRut.Text,
                    RazonSocial = txtRazonSocial.Text,
                    NombreContacto = txtNombre.Text,
                    MailContacto = txtMail.Text,
                    Direccion = txtDireccion.Text,
                    Telefono = txtTelefono.Text,
                    IdActividadEmpresa = valueIDactiv,
                    IdTipoEmpresa = valueIDemp
                };
                if (cliente.Create())
                {                    
                    await this.ShowMessageAsync("Alerta:", String.Format("El cliente con rut {0}, Ha sido registrado con exito!!!! ", txtRut.Text));
                    LimpiarControls();
                }
                else
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("El rut: {0}, no se ha podido registrar , ya que pertenece a otro cliente !!!", txtRut.Text));
                }
            }
        }


        #endregion

        #region BOTON BUSCAR
        private async void btnBuscarcli_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente()
            {
                RutCliente = txtRut.Text
            };
            if (cliente.Read())
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Exito en la búsqueda cliente por el rut: {0} ", txtRut.Text));
                txtRut.Text = cliente.RutCliente;
                txtTelefono.Text = cliente.Telefono;
                txtDireccion.Text = cliente.Direccion;
                txtRazonSocial.Text = cliente.RazonSocial;
                txtNombre.Text = cliente.NombreContacto;
                txtMail.Text = cliente.MailContacto;
                cboActividad.SelectedValue = cliente.IdActividadEmpresa;
                cboTipoemp.SelectedValue = cliente.IdTipoEmpresa;
            }
            else
            {
                await this.ShowMessageAsync("Alerta:", String.Format("El cliente con rut {0}, no se ha encontrado ", txtRut.Text));

            }

        } 
        #endregion

        #region BOTON ELIMINAR
        private async void btnEliminarcli_Click(object sender, RoutedEventArgs e)
        {
            if (txtRut.Text!=string.Empty)
            {
                Cliente cliente = new Cliente()
                {
                    RutCliente = txtRut.Text

                };
                Contrato contrato = new Contrato()
                {
                    RutCliente= txtRut.Text
                };
                if (cliente.Read())
                {
                    //contrato.ReadRut();
                    if (!contrato.ReadRut())
                    {
                        #region estilos del BoxMessage               
                        var settingButton = new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "SI",
                            NegativeButtonText = "NO"
                        };
                        #endregion

                        if ((await this.ShowMessageAsync("Alerta", "¿Está Seguro de Eliminar el Cliente?", MessageDialogStyle.AffirmativeAndNegative, settingButton)) == MessageDialogResult.Affirmative)
                        {                           
                                                       
                            if (cliente.Delete() )
                            {
                                await this.ShowMessageAsync("Alerta:", String.Format("El cliente con rut ,ha sido eliminado con exito!!! "));
                                this.LimpiarControls(); 
                            }                       
                        }
                    }
                    else
                    {
                        await this.ShowMessageAsync("Alerta:", String.Format("El cliente con rut {0}, no se ha podido eliminar ya que todavía sigue asociado a contrato(s)!!! ", txtRut.Text));
                    }
                }
                else
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("El cliente con rut {0}, no se ha encontrado para eliminarlo", txtRut.Text));
                }
            }
            else
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacío el campo rut!!!"));

            }
        }
        #endregion

        #region BOTON ACTUALIZAR                
        private async void btnActualizarcli_Click(object sender, RoutedEventArgs e)
        {
            //Array que se le almacena el nombre contacto separado
            string[] separenombre = txtNombre.Text.Split(' ');

            if (string.IsNullOrEmpty(txtRut.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Escriba bien el campo rut y respete el formato"));
            }
            else if (separenombre.Length != 2)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Por favor respete el orden de 'Nombre Apellido' !!!", txtRut.Text));
            }
            else if (string.IsNullOrEmpty(txtRazonSocial.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo 'Razón Social'!!!"));
            }
            else if (string.IsNullOrEmpty(txtMail.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo 'Email Contacto'!!!"));
            }

            else if (string.IsNullOrEmpty(txtTelefono.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo 'telefono'!!!"));
            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo 'Dirección'!!!"));
            }

            else if (cboActividad.SelectedIndex < 0 || cboTipoemp.SelectedIndex < 0)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Seleccione los algun item de cada 'Actividad' o 'Tipo Empresa'!!!"));
            }
            else
            {
                int valueIDemp = (int)cboTipoemp.SelectedValue;
                int valueIDactiv = (int)cboActividad.SelectedValue;

                Cliente cliente = new Cliente()
                {
                    RutCliente = txtRut.Text,
                    RazonSocial = txtRazonSocial.Text,
                    NombreContacto = txtNombre.Text,
                    MailContacto = txtMail.Text,
                    Direccion = txtDireccion.Text,
                    Telefono = txtTelefono.Text,
                    IdActividadEmpresa = valueIDactiv,
                    IdTipoEmpresa = valueIDemp
                };

                if (cliente.Update())
                {

                    await this.ShowMessageAsync("Alerta:", String.Format("El cliente con rut {0}, Ha sido modificado con exito!!!! ", txtRut.Text));
                }
                else
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("No se pudo modificar el cliente,el rut´: {0} no existe  !!!", txtRut.Text));
                } 
            }
           
           
        }
        #endregion

        #region Metodos Customers


        public void LimpiarControls()
        {
            this.txtDireccion.Text = String.Empty;
            this.txtMail.Text = String.Empty;
            this.txtNombre.Text = String.Empty;
            this.txtRazonSocial.Text = String.Empty;
            this.txtRut.Text = String.Empty;
            this.txtTelefono.Text = String.Empty;
            this.cboActividad.SelectedIndex =-1;
            this.cboTipoemp.SelectedIndex =-1;

        }
        private void ThemeLight()
        {
            estadodark = false;
            this.Background = Brushes.White;
            swDarkTheme.Foreground = Brushes.Black;
            swDarkTheme.ThumbIndicatorBrush = Brushes.Black;
            this.btnLimpiar.BorderBrush = Brushes.Black;
            #region Cambio Color labels
            this.lblActividad.Foreground = Brushes.Black;
            this.lblDireccion.Foreground = Brushes.Black;
            this.lblMailContacto.Foreground = Brushes.Black;
            this.lblNombre.Foreground = Brushes.Black;
            this.lblRazonSocial.Foreground = Brushes.Black;
            this.lblRut.Foreground = Brushes.Black;
            this.lblSwitch.Foreground = Brushes.Black;
            this.lblTelef.Foreground = Brushes.Black;
            this.lblTitulo.Foreground = Brushes.Black;
            this.lblTIpoemp.Foreground = Brushes.Black;
            #endregion
            #region Cambio  color borde textboxes
            txtDireccion.BorderBrush = Brushes.Black;
            txtMail.BorderBrush = Brushes.Black;
            txtNombre.BorderBrush = Brushes.Black;
            txtRazonSocial.BorderBrush = Brushes.Black;
            txtRut.BorderBrush = Brushes.Black;
            txtTelefono.BorderBrush = Brushes.Black;
            #endregion

        }

        public void ThemeDark()
        {
            estadodark = true;
            var bc = new BrushConverter();

            this.Background = Brushes.Black;
            swDarkTheme.Foreground = Brushes.White;
            swDarkTheme.ThumbIndicatorBrush = Brushes.White;
            this.btnLimpiar.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            #region Cambio Color labels
            this.lblActividad.Foreground = (Brush)bc.ConvertFrom("#085394");
            this.lblDireccion.Foreground = (Brush)bc.ConvertFrom("#085394");
            this.lblMailContacto.Foreground = (Brush)bc.ConvertFrom("#085394");
            this.lblNombre.Foreground = (Brush)bc.ConvertFrom("#085394");
            this.lblRazonSocial.Foreground = (Brush)bc.ConvertFrom("#085394");
            this.lblRut.Foreground = (Brush)bc.ConvertFrom("#085394");
            this.lblSwitch.Foreground = (Brush)bc.ConvertFrom("#085394");
            this.lblTelef.Foreground = (Brush)bc.ConvertFrom("#085394");
            this.lblTitulo.Foreground = (Brush)bc.ConvertFrom("#085394");
            this.lblTIpoemp.Foreground = (Brush)bc.ConvertFrom("#085394");
            #endregion
            #region Cambio color borde textboxes
            txtDireccion.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtMail.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtNombre.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtRazonSocial.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtRut.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtTelefono.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            #endregion
            swDarkTheme.IsChecked = true;
        }
        #endregion

        private void txtRut_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public async void BuscarPorListado()
        {
            ListadoClientes listcli = new ListadoClientes();
            Cliente cliente = new Cliente()
            {
                RutCliente = listcli.Rutcli
            };
            if (cliente.Read())
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Exito en la búsqueda cliente por el rut: {0} ", txtRut.Text));
                txtRut.Text = cliente.RutCliente;
                txtTelefono.Text = cliente.Telefono;
                txtDireccion.Text = cliente.Direccion;
                txtRazonSocial.Text = cliente.RazonSocial;
                txtNombre.Text = cliente.NombreContacto;
                txtMail.Text = cliente.MailContacto;
                cboActividad.SelectedValue = cliente.IdActividadEmpresa;
                cboTipoemp.SelectedValue = cliente.IdTipoEmpresa;
            }
            
        }


        
    }
   
  

}
