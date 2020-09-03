using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using OnBreak.Negocio;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OnBreakWPF
{
    /// <summary>
    /// Lógica de interacción para ListadoClientes.xaml
    /// </summary>
    public partial class ListadoClientes : MetroWindow
    {
        public string Rutcli;
        public bool estadodark;
        //cambiowindow -verifica que si ListarCli se abrió desde admin clientes 
        public bool cambiowindow;
        public ListadoClientes()
        {
            InitializeComponent();
            CargarEmpresa();
            CargarActividad();
            cargarGrid();
            LimpiarControls();
   
        }
        private void CargarEmpresa()
        {
            TipoEmpresa empresa = new TipoEmpresa();
            cboTipoemp.DisplayMemberPath = "Descripcion";
            cboTipoemp.SelectedValuePath = "IdTipoEmpresa";
            cboTipoemp.ItemsSource = empresa.ReadAll();
            cboTipoemp.SelectedIndex = 0;
        }

        public void CargarActividad()
        {
            ActividadEmpresa actividad = new ActividadEmpresa();

            cboActividad.ItemsSource = actividad.ReadAll();
            cboActividad.DisplayMemberPath = "Descripcion";
            cboActividad.SelectedValuePath = "IdActividadEmpresa";
            cboActividad.SelectedIndex = 0;
        }
        public void LimpiarControls()
        {
            txtRut.Text = string.Empty;
            cboActividad.SelectedIndex = -1;
            cboTipoemp.SelectedIndex = -1;
            cargarGrid();

        }

        private void cboActividad_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminClientes adcli = new AdminClientes();
            AdminContratos adcon = new AdminContratos();
            if (cambiowindow)
            {
                if (estadodark)
                {
                    adcli.ThemeDark();
                }
                this.Close();
                adcli.Show();
                //adcli.RutListado = this.Rutcli;
            }
            else
            {
                if (estadodark)
                {
                    adcon.ThemeDark();
                }
                this.Close();
                adcon.Show();
            }
            
        }

        private void swDarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            ThemeDark();
        }

        private void swDarkTheme_IsCheckedChanged(object sender, EventArgs e)
        {
            ThemeLight();
        }

        private void ThemeLight()
        {
            estadodark = false;
            this.Background = Brushes.White;
            swDarkTheme.Foreground = Brushes.Black;
            swDarkTheme.ThumbIndicatorBrush = Brushes.Black;
            lblSwitch.Foreground = Brushes.Black;
            brDark.Visibility = Visibility.Collapsed;

            #region Cambio Color de Labels
            lblSwitch.Foreground = Brushes.Black;
            lblTitulo.Foreground = Brushes.Black;
            lblRut.Foreground = Brushes.Black;
            lblActividad.Foreground = Brushes.Black;
            lblTipoemp.Foreground = Brushes.Black;

            #endregion

            #region  Cambio de color Textboxes,cbo y botones

            txtRut.BorderBrush = Brushes.Black;
            cboActividad.BorderBrush = Brushes.Black;
            cboTipoemp.BorderBrush = Brushes.Black;
            btnBuscar.BorderBrush = Brushes.Black;
            btnLimpiar.BorderBrush = Brushes.Black;
            #endregion

        }

        public void ThemeDark()
        {
            estadodark = true;
            var bc = new BrushConverter();
            this.Background = Brushes.Black;
            swDarkTheme.Foreground = Brushes.White;
            swDarkTheme.ThumbIndicatorBrush = Brushes.White;
            lblSwitch.Foreground = (Brush)bc.ConvertFrom("#085394");
            swDarkTheme.IsChecked = true;
            brDark.Visibility = Visibility.Visible;

            #region Cambio Color de Labels
            lblSwitch.Foreground = (Brush)bc.ConvertFrom("#085394");
            
            lblTitulo.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblRut.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblActividad.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblTipoemp.Foreground = (Brush)bc.ConvertFrom("#085394");
     
            #endregion

            #region  Cambio de color Textboxes,cbo y botones
            
            txtRut.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            cboActividad.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            cboTipoemp.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            btnBuscar.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            btnLimpiar.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            #endregion
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarControls();
        }

        #region BUSCAR CLIENTE
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();

            if (this.cboActividad.SelectedIndex > -1 )
            {

                int valueIDactiv = (int)cboActividad.SelectedValue;
                dtgClientes.ItemsSource = cliente.ReadAllByActividad(valueIDactiv);

            }                   
            else if (cboTipoemp.SelectedIndex > -1 )
            {
                int valueIDemp = (int)cboTipoemp.SelectedValue;
                dtgClientes.ItemsSource = cliente.ReadAllByTipoEmpresa(valueIDemp);
            }              
            else if ( txtRut.Text != string.Empty)
            {                    
                dtgClientes.ItemsSource = cliente.ReadAllByRut(txtRut.Text);
            }             
        } 
        #endregion

        public void cargarGrid()
        {
            Cliente cliente = new Cliente();
            dtgClientes.ItemsSource = cliente.ReadAll();
            
        }

      
        private  async void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = dtgClientes.SelectedItem as Cliente;
            AdminClientes adcli = new AdminClientes();
            AdminContratos adcon = new AdminContratos();
            if (dtgClientes.SelectedIndex>-1)
            {
                if (cambiowindow)
                {
                    if (estadodark)
                    {
                        adcli.ThemeDark();
                    }
                    this.Close();
                    #region LLENADO CONTROLES DE ADMIN CLIENTES                    
                    adcli.Show();
                    adcli.txtRut.Text = cliente.RutCliente;
                    adcli.txtNombre.Text = cliente.NombreContacto;
                    adcli.txtRazonSocial.Text = cliente.RazonSocial;
                    adcli.txtDireccion.Text = cliente.Direccion;
                    adcli.txtMail.Text = cliente.MailContacto;
                    adcli.txtTelefono.Text = cliente.Telefono;
                    adcli.cboActividad.SelectedValue = cliente.IdActividadEmpresa;
                    adcli.cboTipoemp.SelectedValue = cliente.IdTipoEmpresa;
                    #endregion

                }
                else
                {
                    if (estadodark)
                    {
                        adcon.ThemeDark();
                    }
                    this.Close();
                    adcon.Show();
                    #region LLENADO CONTROLES HACIA ADMIN CONTRATO
                    //string[] separaNombre = cliente.NombreContacto.Split(' ');
                    adcon.txtRut.Text = cliente.RutCliente;
                    //adcon.txtNombre.Text = separaNombre[0];//Nombre
                    //adcon.txtApel.Text = separaNombre[1];//Apellido  
                    adcon.txtRazonSocial.Text = cliente.RazonSocial;
                    #endregion
                }

            }
            else
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Seleccione bien el cliente"));
            }
        }


        private void dtgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        
        

        private void dtgClientes_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
