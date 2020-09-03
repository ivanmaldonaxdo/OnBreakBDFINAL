using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using OnBreak.Negocio;
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

namespace OnBreakWPF
{
    /// <summary>
    /// Lógica de interacción para ListadoContratos.xaml
    /// </summary>
    public partial class ListadoContratos : MetroWindow
    {
        public bool estadodark;
        public ListadoContratos()
        {
            InitializeComponent();
            LlenarDG();
            CargarTipoEvento();
        }

        public void LimpiarControls()
        {
            txtNumcontrato.Text = string.Empty;
            txtRut.Text = string.Empty;
            cboTipoevent.SelectedIndex = -1;

        }
        public void LlenarDG()
        {
            Contrato contrato = new Contrato();
            dtgContrato.ItemsSource = contrato.ReadAll();
        }
        public void CargarTipoEvento()
        {
            TipoEvento evento = new TipoEvento();
            cboTipoevent.DisplayMemberPath = "Descripcion";
            cboTipoevent.SelectedValuePath = "IdTipoEvento";
            cboTipoevent.ItemsSource = evento.ReadAll();
            cboTipoevent.SelectedIndex = -1;
        }
        private void cboTipoevent_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            AdminContratos adcon = new AdminContratos();
            if (estadodark)
            {
                adcon.ThemeDark();
            }
            this.Close();
            adcon.Show();
            
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
            lblNumcontrato.Foreground = Brushes.Black;
            lblRut.Foreground = Brushes.Black;
            lblSwitch.Foreground = Brushes.Black;
            lblTitulo.Foreground = Brushes.Black;
            lblTpoevent.Foreground = Brushes.Black;

            #endregion

            #region  Cambio de color Textboxes,cbo y botones
            txtNumcontrato.BorderBrush = Brushes.Black;
            txtRut.BorderBrush = Brushes.Black;
            cboTipoevent.BorderBrush = Brushes.Black;
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
            lblNumcontrato.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblRut.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblSwitch.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblTitulo.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblTpoevent.Foreground = (Brush)bc.ConvertFrom("#085394");

            #endregion

            #region  Cambio de color Textboxes,cbo y botones

            txtNumcontrato.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtRut.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            cboTipoevent.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            btnBuscar.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            btnLimpiar.BorderBrush = (Brush)bc.ConvertFrom("#085394");            
            #endregion       
        }

        #region BOTON SELECCIONAR CONTRATO
        private async void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            Contrato contrato = dtgContrato.SelectedItem as Contrato;
            AdminContratos adcon = new AdminContratos();
            if (dtgContrato.SelectedIndex>-1)
            {
                if (estadodark)
                {
                    adcon.ThemeDark();
                }
                this.Close();
                adcon.Show();

                #region LLENADO CONTROLES HACIA ADMIN CONTRATO
                adcon.txtAsistentes.Text = contrato.Asistentes.ToString();
                adcon.txtNumcont.Text = contrato.Numero;
                adcon.txtObservacion.Text = contrato.Observaciones;
                adcon.txtRut.Text = contrato.RutCliente;
                adcon.txtPersonal.Text = contrato.PersonalAdicional.ToString();
                adcon.txtAsistentes.Text = contrato.Asistentes.ToString();
                adcon.txtValorBase.Text = contrato.ValorBaseTipo.ToString();
                adcon.txtTotalfinal.Text = contrato.ValorTotalContrato.ToString();
                adcon.dtpFechaInic.SelectedDate = contrato.FechaHoraInicio;
                adcon.dtpFechaTerm.SelectedDate = contrato.FechaHoraTermino;
                adcon.txtCreacion.Text = contrato.Creacion.ToString();
                adcon.txtTermino.Text = contrato.Termino.ToString();

                if (contrato.Realizado == true)
                {
                    adcon.rdbActiva.IsChecked = true;
                }
                else
                {
                    adcon.rdbInactiva.IsChecked = true;
                }

                adcon.cboTIpo.Text = contrato.DescTipoEvento;
                adcon.cboModalidad.Text = contrato.DescModalidad;

                //adcon.cboModalidad.Text.Trim();
                #endregion
                if (contrato.DescTipoEvento == "Coffee Break")
                {
                    CoffeBreak coffe = new CoffeBreak()
                    {
                        Numero = contrato.Numero
                    };
                    if (coffe.Read())
                    {
                        if (coffe.Vegetariana == true)
                        {
                            adcon.chkVegetarian.IsChecked = true;
                        }
                    }
                }
                else if (contrato.DescTipoEvento == "Cocktail")
                {
                    Cocktail cocktail = new Cocktail()
                    {
                        Numero = contrato.Numero
                    };
                    if (cocktail.Read())
                    {
                        if (cocktail.Ambientacion == true)
                        {
                            adcon.chkAmbientacion.IsChecked = true;
                        }
                        if (cocktail.IdTipoAmbientacion == 10)
                        {
                            adcon.cboAmbientacion.SelectedIndex = 0;
                        }
                        else if (cocktail.IdTipoAmbientacion == 20)
                        {
                            adcon.cboAmbientacion.SelectedIndex = 1;
                        }
                        if (cocktail.MusicaAmbiental == true)
                        {
                            adcon.rdbAmbiental.IsChecked = true;
                        }
                        else if (cocktail.MusicaCliente == true)
                        {
                            adcon.rdbCliente.IsChecked = true;
                        }

                    }
                }
                else if (contrato.DescTipoEvento == "Cenas")
                {
                    Cenas cenas = new Cenas()
                    {
                        Numero = contrato.Numero
                    };
                    if (cenas.Read())
                    {
                        if (cenas.LocalOnBreak == true)
                        {
                            adcon.rdbLocalOnbreak.IsChecked = true;
                        }
                        else if (cenas.OtroLocalOnBreak == true)
                        {
                            adcon.rdbOtroLocal.IsChecked = true;
                            adcon.txtArriendo.Text = cenas.ValorArriendo.ToString();
                        }
                        if (cenas.MusicaAmbiental == true)
                        {
                            adcon.chkMusicAmb.IsChecked = true;
                        }
                    }
                }

            }
            else
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Seleccione bien el contrato"));
            }
        }
        #endregion


        #region BUSCAR CONTRATO
        private  void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            Contrato contrato = new Contrato();
            if (txtNumcontrato.Text != string.Empty )
            {
                dtgContrato.ItemsSource = contrato.ReadAllByNumero(txtNumcontrato.Text);                  
            }            
           
            else if (txtRut.Text != string.Empty)
            {
                dtgContrato.ItemsSource = contrato.ReadAllByRut(txtRut.Text);           
                     
            }           
            else if ( cboTipoevent.SelectedIndex >-1)
            {
                int idtipo = (int)cboTipoevent.SelectedValue;
                dtgContrato.ItemsSource = contrato.ReadAllByTipoEvento(idtipo);                   
            }               
            //LimpiarControls();
        }
        #endregion

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarControls();
            LlenarDG();
        }
    }
}
