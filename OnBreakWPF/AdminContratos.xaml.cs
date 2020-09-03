using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using OnBreak.Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
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
using System.Windows.Threading;
using System.Xml.Serialization;

namespace OnBreakWPF
{
    /// <summary>
    /// Lógica de interacción para AdminContratos.xaml
    /// </summary>
    public partial class AdminContratos : MetroWindow
    {
        Caretaker caretaker = new Caretaker();
        public bool estadodark;
        public AdminContratos()
        {
            InitializeComponent();
            CargarTipoEvento();
            Respaldar();
            Recuperar();
        }
        #region RESPALDO
        public void Respaldar()
        {
            DispatcherTimer contador = new DispatcherTimer();
            contador.Interval = TimeSpan.FromSeconds(1);
            contador.Tick += CuentaAtraz;
            contador.Start();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 30, 0);
            timer.Tick += (a, b) =>
            {

                #region ACCION EJECUTADA CADA 30 SEGS
                Contrato contrato = new Contrato()
                {
                    IdTipoEvento = (int)(cboTIpo.SelectedValue ?? 10),//int)cboTIpo.SelectedValue,
                    IdModalidad = (string)(cboModalidad.SelectedValue ?? "CB001"),// (string)cboModalidad.SelectedValue,
                    Realizado = true,
                    RutCliente = txtRut.Text,
                    Asistentes = 0,
                    PersonalAdicional = 0,// int.Parse(txtPersonal.Text),
                    Observaciones = this.txtObservacion.Text,
                    FechaHoraInicio = dtpFechaInic.SelectedDate ?? DateTime.Now,
                    FechaHoraTermino = dtpFechaTerm.SelectedDate ?? DateTime.Now,
                    ValorTotalContrato = 0
                    
                };

                //crea formateador que pasa el objeto de clase a un archivo
                //BinaryFormatter formateador = new BinaryFormatter();
                XmlSerializer formateador = new XmlSerializer(typeof(Contrato));
                //creamos el archivo
                Stream miStream = new FileStream("RespaldoContratoXML.txt", FileMode.Create, FileAccess.Write, FileShare.None);
                formateador.Serialize(miStream, contrato);
                miStream.Close();
                #endregion
            };
            timer.Start();
        }
        #endregion
        #region RECUPERAR
        public void Recuperar()
        {
            //crea formateador que pasa el objeto de clase a un archivo
            XmlSerializer formateador = new XmlSerializer(typeof(Contrato));
            //creamos el archivo
            Stream miStream = new FileStream("RespaldoContratoXML.txt", FileMode.Open, FileAccess.Read, FileShare.None);
            if (miStream != null)
            {
                Contrato contrato = (Contrato)formateador.Deserialize(miStream);
                miStream.Close();
                caretaker.Memento = contrato.CreateMemento();
                contrato.SetMemento(caretaker.Memento);
                #region LLENADO DE CONTROLES
                txtAsistentes.Text = contrato.Asistentes.ToString();
                txtObservacion.Text = contrato.Observaciones;
                txtRut.Text = contrato.RutCliente;
                txtPersonal.Text = contrato.PersonalAdicional.ToString();
                txtAsistentes.Text = contrato.Asistentes.ToString();
                txtTotalfinal.Text = contrato.ValorTotalContrato.ToString();
                dtpFechaInic.SelectedDate = contrato.FechaHoraInicio;
                dtpFechaTerm.SelectedDate = contrato.FechaHoraTermino;
                txtCreacion.Text = contrato.Creacion.ToString();
                txtTermino.Text = contrato.Termino.ToString();
                cboTIpo.SelectedValue = contrato.IdTipoEvento;
                if (contrato.Realizado)
                {
                    rdbActiva.IsChecked = true;
                }
                else
                {
                    rdbInactiva.IsChecked = false;
                }
                #endregion
            }
            //deszerializar

            //cboModalidad.SelectedValue = contrato.IdModalidad;
        } 
        #endregion
        private int descontar = 30;
        #region CUENTA REGRESIVA
        private void CuentaAtraz(object sender, EventArgs e)
        {
            descontar--;
            lblTimer.Content = "          Quedan " + descontar + " segundos " + "\r\npara otro guardado automático";
            if (descontar == 0)
            {
                lblTimer.Content = "Se há realizado otro guardado automático";
                descontar = 30;
            }
        }
        #endregion

        public void CargarTipoEvento()
        {
            cboTIpo.ItemsSource = new TipoEvento().ReadAll();
            cboTIpo.DisplayMemberPath = "Descripcion";
            cboTIpo.SelectedValuePath = "IdTipoEvento";

            cboTIpo.SelectedIndex = -1;
        }

        private void txtObservacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        #region LISTAR CONTRATO
        private void btnListarcont_Click(object sender, RoutedEventArgs e)
        {
            ListadoContratos listcon = new ListadoContratos();

            if (estadodark)
            {
                listcon.ThemeDark();
            }
            this.Close();
            listcon.Show();
            listcon.btnMenu.Visibility = Visibility.Collapsed;
            listcon.btnSelect.Visibility = Visibility.Visible;
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
        #endregion

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarControls();
        }

        #region LIMPIA CONTROLES
        private void LimpiarControls()
        {
            this.txtRazonSocial.Text = String.Empty;
            this.txtNumcont.Text = String.Empty;
            this.txtObservacion.Text = String.Empty;
            this.txtRut.Text = String.Empty;
            dtpFechaInic.SelectedDate = null;
            dtpFechaTerm.SelectedDate = null;
            this.rdbActiva.IsChecked = false;
            this.rdbInactiva.IsChecked = false;
            this.txtTotalfinal.Text = string.Empty;
            txtPersonal.Text = string.Empty;
            txtValorBase.Text = string.Empty;
            txtAsistentes.Text = string.Empty;
            txtCreacion.Text = string.Empty;
            txtTermino.Text = string.Empty;
            cboModalidad.SelectedIndex = -1;
            cboTIpo.SelectedIndex = -1;
        }
        #endregion

        private void txtNumcont_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #region LISTAR CLIENTE
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
            listcli.cambiowindow = false;
        }

        #endregion
        private void swDarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            ThemeDark();
        }

        private void swDarkTheme_IsCheckedChanged(object sender, EventArgs e)
        {
            ThemeLight();
        }

        #region METODO BAJO CONTRASTE
        private void ThemeLight()
        {
            estadodark = false;
            this.Background = Brushes.White;
            swDarkTheme.Foreground = Brushes.Black;
            swDarkTheme.ThumbIndicatorBrush = Brushes.Black;
            lblSwitch.Foreground = Brushes.Black;

            #region Cambio color Labels

            lblAsistentes.Foreground = Brushes.Black;
            lblCalculo.Foreground = Brushes.Black;
            lblCreacion.Foreground = Brushes.Black;
            lblFechaInicio.Foreground = Brushes.Black;
            lblFechaTermino.Foreground = Brushes.Black;
            lblInfoCliente.Foreground = Brushes.Black;
            lblNombre.Foreground = Brushes.Black;
            lblnumcont.Foreground = Brushes.Black;
            lblObservacion.Foreground = Brushes.Black;
            lblPersonal.Foreground = Brushes.Black;
            lblRut.Foreground = Brushes.Black;
            lblSwitch.Foreground = Brushes.Black;
            lblTermino.Foreground = Brushes.Black;
            lblTipo.Foreground = Brushes.Black;
            lblTitulo.Foreground = Brushes.Black;
            lblTotalfinal.Foreground = Brushes.Black;
            lblUFtotalfinal.Foreground = Brushes.Black;
            lblModalidad.Foreground = Brushes.Black;
            lblUFvalorbase.Foreground = Brushes.Black;
            lblValorbase.Foreground = Brushes.Black;
            lblVigencia.Foreground = Brushes.Black;
            rdbActiva.BorderBrush = Brushes.Black;
            rdbActiva.Foreground = Brushes.Black;
            rdbInactiva.BorderBrush = Brushes.Black;
            rdbInactiva.Foreground = Brushes.Black;
            btnLimpiar.BorderBrush = Brushes.Black;
            btnCalculartotal.BorderBrush = Brushes.Black;
            brFechaInici.BorderBrush = Brushes.Black;
            brFechaTerm.BorderBrush = Brushes.Black;

            #endregion           

            #region Cambio de color en Textboxes ,datepickers y combobox
            txtAsistentes.BorderBrush = Brushes.Black;

            txtNumcont.BorderBrush = Brushes.Black;
            txtObservacion.BorderBrush = Brushes.Black;
            txtPersonal.BorderBrush = Brushes.Black;
            txtRut.BorderBrush = Brushes.Black;
            txtCreacion.BorderBrush = Brushes.Black;
            txtTermino.BorderBrush = Brushes.Black;
            cboTIpo.BorderBrush = Brushes.Black;
            #endregion
        }
        #endregion

        #region METODO ALTO CONTRASTE
        public void ThemeDark()
        {
            estadodark = true;
            var bc = new BrushConverter();
            this.Background = Brushes.Black;
            swDarkTheme.Foreground = Brushes.White;
            swDarkTheme.ThumbIndicatorBrush = Brushes.White;
            lblSwitch.Foreground = (Brush)bc.ConvertFrom("#085394");

            #region Cambio color Labels            
            lblAsistentes.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblCalculo.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblCreacion.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblModalidad.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblFechaInicio.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblFechaTermino.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblInfoCliente.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblNombre.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblnumcont.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblObservacion.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblPersonal.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblRut.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblSwitch.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblTermino.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblTipo.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblTitulo.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblTotalfinal.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblUFtotalfinal.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblUFvalorbase.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblValorbase.Foreground = (Brush)bc.ConvertFrom("#085394");
            lblVigencia.Foreground = (Brush)bc.ConvertFrom("#085394");
            brFechaInici.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            brFechaTerm.BorderBrush = (Brush)bc.ConvertFrom("#085394");

            #endregion

            #region Cambio de color en Textboxes ,datepickers y cbo
            txtAsistentes.BorderBrush = (Brush)bc.ConvertFrom("#085394");

            txtNumcont.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtObservacion.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtPersonal.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtRut.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtCreacion.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            txtTermino.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            cboTIpo.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            rdbActiva.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            rdbActiva.Foreground = (Brush)bc.ConvertFrom("#085394");
            rdbInactiva.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            rdbInactiva.Foreground = (Brush)bc.ConvertFrom("#085394");
            btnLimpiar.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            btnCalculartotal.BorderBrush = (Brush)bc.ConvertFrom("#085394");
            #endregion

            swDarkTheme.IsChecked = true;
        }
        #endregion

        private void cboTIpo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CargarModalidad();
            if (cboTIpo.SelectedValue!=null)
            {
                cboModalidad.ItemsSource = new ModalidadServicio().ReadbyTipoEvento((int)cboTIpo.SelectedValue);
                cboModalidad.DisplayMemberPath = "Nombre";
                cboModalidad.SelectedValuePath = "IdModalidad";
                MostrarOpEvent();


            }
        }


        #region REG CONTRATOS
        private async void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            #region validaciones de ingreso controles
            if (cboModalidad.SelectedIndex < 0 || cboTIpo.SelectedIndex < 0 || txtObservacion.Text == string.Empty)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Debe seleccionar algun item de 'tipo de evento', 'modalidad de evento' " +
                                                                    "y no dejar espacio en 'Observación'!!!"));
            }
            else if (txtRut.Text == string.Empty)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo rut !!"));
            }
            //else if (!Enumerable.Range(1, 200).Contains(int.Parse(txtAsistentes.Text)) 
            //    || !Enumerable.Range(1, 200).Contains(int.Parse(txtPersonal.Text))) //txtAsistentes.Text == string.Empty || txtPersonal.Text == string.Empty ||txtTotalfinal.Text == string.Empty     
            //{
            //    await this.ShowMessageAsync("Alerta:", String.Format("No dejar con 0  los campos de asistentes o personal adicional!!!"));
            //}
            //else if (txtAsistentes.Text!=string.Empty||txtPersonal.Text != string.Empty)//txtAsistentes.Text == string.Empty || txtPersonal.Text == string.Empty ||txtTotalfinal.Text == string.Empty     
            //{
            //    await this.ShowMessageAsync("Alerta:", String.Format("No dejar en blanco los campos de asistentes o personal adicional!!!"));
            //}
            else if (dtpFechaInic.SelectedDate == null || dtpFechaTerm.SelectedDate == null)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No dejar vacio los Datepickers(Calendarios) de Fecha,Hora Inicio y" +
                                               " de Fecha, Hora término de evento!!!"));
            }

            else
            {
                #region validaciones de fechas
                int hours = (int)(dtpFechaTerm.SelectedDate.Value - dtpFechaInic.SelectedDate.Value).TotalHours;//caluclo de horas entre 2 fechas
                TimeSpan fecha = (dtpFechaTerm.SelectedDate.Value - dtpFechaInic.SelectedDate.Value);//diferencia de fechas en mes..año..fecha..dias
                int meses = Math.Abs((DateTime.Now.Month - dtpFechaInic.SelectedDate.Value.Month) + 12 * (DateTime.Now.Year - dtpFechaInic.SelectedDate.Value.Year));
                //String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                //fecha.Days, fecha.Hours, fecha.Minutes, fecha.Seconds);
                if (hours > 24)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora evento no puede durar mas de 24 horas,el evento sobrepasa con {0} hrs y {1} minutos ", hours, fecha.Minutes));
                }
                else if (dtpFechaInic.SelectedDate.Value < DateTime.Now)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser menor a la fecha actual"));
                }
                else if (meses > 10)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser mayor a la fecha actual por 10 meses"));
                }
                else if (dtpFechaInic.SelectedDate.Value > dtpFechaTerm.SelectedDate.Value)//valida que  fecha hora-termino no sea menor a fecha inc
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser mayor a la fecha-hora termino del evento"));
                }
                #endregion
                else
                {
                    Contrato contrato = new Contrato()
                    {
                        Numero = DateTime.Now.ToString("yyyyMMddHHmm"),
                        Creacion = DateTime.Now,
                        Termino = DateTime.Parse("12-12-3000"),
                        IdTipoEvento = (int)cboTIpo.SelectedValue,
                        IdModalidad = (string)cboModalidad.SelectedValue,
                        Realizado = true,
                        RutCliente = txtRut.Text,

                        Asistentes = 0,
                        //PersonalAdicional = int.Parse(txtPersonal.Text),
                        PersonalAdicional = 0,
                        Observaciones = this.txtObservacion.Text,
                        //ValorTotalContrato = double.Parse(txtTotalfinal.Text),
                        ValorTotalContrato = 0,
                        FechaHoraInicio = dtpFechaInic.SelectedDate.Value,
                        FechaHoraTermino = dtpFechaTerm.SelectedDate.Value
                    };
                    if (contrato.Create())
                    {
                        if (contrato.IdTipoEvento == 10)
                        {
                            bool vegcheck = Vegetariana();
                            CoffeBreak coffeBreak = new CoffeBreak()
                            {
                                Numero = contrato.Numero,
                                Vegetariana = vegcheck

                            };
                            coffeBreak.Create();
                        }
                        else if (contrato.IdTipoEvento == 20)
                        {
                            bool muscli = musclisi();
                            bool musamb = musambsi();
                            bool ambsi = Ambientacion();
                            Cocktail cocktail = new Cocktail()
                            {
                                Numero = contrato.Numero,
                                IdTipoAmbientacion = (int)cboAmbientacion.SelectedValue,
                                Ambientacion = ambsi,
                                MusicaCliente = muscli,
                                MusicaAmbiental = musamb

                            };
                            cocktail.Create();
                        } else
                        {
                            bool localOB = Local();
                            bool otrolocal = OtroLocal();
                            bool mamb = CheckAmbiental();
                            double arriendo = arriendoLocal();
                            Cenas cenas = new Cenas()
                            {
                                Numero = contrato.Numero,
                                IdTipoAmbientacion = (int)cboAmbientacion.SelectedValue,
                                LocalOnBreak = localOB,
                                MusicaAmbiental = mamb,
                                OtroLocalOnBreak = otrolocal,
                                ValorArriendo = arriendo
                            };
                        }
                        LimpiarControls();
                        await this.ShowMessageAsync("Alerta:", String.Format("Contrato creado con exito!!!"));
                    }
                    else
                    {
                        await this.ShowMessageAsync("Alerta:", String.Format("El contrato no se pudo registrar!!!"));
                    }
                }
            }
            #endregion
        }
        #endregion

        #region BUSCAR CONTRATO
        private async void btnBuscarcont_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumcont.Text != string.Empty)
            {
                Contrato contrato = new Contrato()
                {
                    Numero = txtNumcont.Text
                };
                if (contrato.Read())
                {

                    txtAsistentes.Text = contrato.Asistentes.ToString();
                    txtNumcont.Text = contrato.Numero;
                    txtObservacion.Text = contrato.Observaciones;
                    txtRut.Text = contrato.RutCliente;
                    txtPersonal.Text = contrato.PersonalAdicional.ToString();
                    txtAsistentes.Text = contrato.Asistentes.ToString();
                    txtValorBase.Text = contrato.ValorBaseTipo.ToString();
                    txtTotalfinal.Text = contrato.ValorTotalContrato.ToString();
                    dtpFechaInic.SelectedDate = contrato.FechaHoraInicio;
                    dtpFechaTerm.SelectedDate = contrato.FechaHoraTermino;
                    txtCreacion.Text = contrato.Creacion.ToString();
                    txtTermino.Text = contrato.Termino.ToString();
                    cboTIpo.SelectedValue = contrato.IdTipoEvento;
                    cboModalidad.SelectedValue = contrato.IdModalidad;

                    if (contrato.Realizado == true)
                    {
                        rdbActiva.IsChecked = true;
                    }
                    else
                    {
                        rdbInactiva.IsChecked = true;
                    }
                    //cboModalidad.SelectedValue = contrato.IdModalidad;
                }
                else
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("El número de contrato no se ha encontrado "));
                }
            }
            else
            {
                await this.ShowMessageAsync("Alerta:", String.Format("´No deje vacío el campo número de contrato "));

            }
        }
        #endregion

        #region BOTON BUSCAR CLIENTE
        private async void btnBuscarcli_Click(object sender, RoutedEventArgs e)
        {
            if (txtRut.Text != string.Empty)
            {
                Cliente cliente = new Cliente()
                {
                    RutCliente = txtRut.Text
                };
                if (cliente.Read())
                {
                    //string[] separaNombre = cliente.NombreContacto.Split(' ');
                    await this.ShowMessageAsync("Alerta:", String.Format("Exito en la búsqueda cliente por el rut: {0} ", txtRut.Text));
                    txtRut.Text = cliente.RutCliente;
                    txtRazonSocial.Text = cliente.RazonSocial;
                    //txtNombre.Text = separaNombre[0];
                    //txtApel.Text = separaNombre[1];
                }
                else
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("El cliente con rut {0}, no se ha encontrado ", txtRut.Text));

                }
            }
            else
            {
                await this.ShowMessageAsync("Alerta:", String.Format("El campo Rut no debe estar vacío!!!", txtRut.Text));
            }
        }

        #endregion
        private void txtValorBase_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cboModalidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboModalidad.SelectedIndex > -1)
            {
                ModalidadServicio ms = new ModalidadServicio()
                {
                    IdModalidad = cboModalidad.SelectedValue.ToString()
                };
                if (ms.Read())
                {
                    txtValorBase.Text = ms.ValorBase.ToString();
                }
            }
        }
        #region METODO CALCULO DE CONTRATO
        public async void CalcularContrato()
        {
            double var;
            if (Enumerable.Range(1, 200).Contains(int.Parse(txtAsistentes.Text)) && Enumerable.Range(1, 200).Contains(int.Parse(txtPersonal.Text))
                && txtValorBase.Text != string.Empty)
            {
                Contrato contrato = new Contrato()
                {
                    IdTipoEvento = (int)cboTIpo.SelectedValue,
                    PersonalAdicional = int.Parse(txtPersonal.Text),
                    Asistentes = int.Parse(txtAsistentes.Text),
                    IdModalidad = cboModalidad.SelectedValue.ToString(),
                    Numero = txtNumcont.Text                  
                };
                
                    Valorizar val = Creador.CreadorTipos(contrato.IdTipoEvento);
                    var = val.CalcularContrato(contrato);
                    txtTotalfinal.Text = var.ToString();


            }
            else
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Porfavor ingrese todos los datos antes de calcular "));
            }
        }
        #endregion

        private void btnCalculartotal_Click(object sender, RoutedEventArgs e)
        {
            CalcularContrato();
        }


        #region BOTON ACTUALIZAR CONTRATO
        private async void btnActualizarcont_Click(object sender, RoutedEventArgs e)
        {
            #region validaciones de ingreso controles
            if (cboModalidad.SelectedIndex < 0 || cboTIpo.SelectedIndex < 0 || txtObservacion.Text == string.Empty)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Debe seleccionar algun item de 'tipo de evento', 'modalidad de evento' " +
                                                                    "y no dejar espacio en 'Observación'!!!"));
            }
            else if (txtRut.Text == string.Empty)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo rut !!"));
            }
            //else if (!Enumerable.Range(1, 200).Contains(int.Parse(txtAsistentes.Text))
            //    || !Enumerable.Range(1, 200).Contains(int.Parse(txtPersonal.Text))) //txtAsistentes.Text == string.Empty || txtPersonal.Text == string.Empty ||txtTotalfinal.Text == string.Empty     
            //{
            //    await this.ShowMessageAsync("Alerta:", String.Format("No dejar con 0  los campos de asistentes o personal adicional!!!"));
            //}
            else if (txtAsistentes.Text == string.Empty || txtPersonal.Text == string.Empty)//txtAsistentes.Text == string.Empty || txtPersonal.Text == string.Empty ||txtTotalfinal.Text == string.Empty     
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No dejar en blanco los campos de asistentes o personal adicional!!!"));
            }
            else if (dtpFechaInic.SelectedDate == null || dtpFechaTerm.SelectedDate == null)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No dejar vacio los Datepickers(Calendarios) de Fecha,Hora Inicio y" +
                                               " de Fecha, Hora término de evento!!!"));
            }
            else
            {
                #region validaciones de fechas
                int hours = (int)(dtpFechaTerm.SelectedDate.Value - dtpFechaInic.SelectedDate.Value).TotalHours;//caluclo de horas entre 2 fechas
                TimeSpan fecha = (dtpFechaTerm.SelectedDate.Value - dtpFechaInic.SelectedDate.Value);//diferencia de fechas en mes..año..fecha..dias
                int meses = Math.Abs((DateTime.Now.Month - dtpFechaInic.SelectedDate.Value.Month) + 12 * (DateTime.Now.Year - dtpFechaInic.SelectedDate.Value.Year));
                //String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                //fecha.Days, fecha.Hours, fecha.Minutes, fecha.Seconds);
                if (hours > 24)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora evento no puede durar mas de 24 horas,el evento sobrepasa con {0} hrs y {1} minutos ", hours, fecha.Minutes));
                }
                else if (dtpFechaInic.SelectedDate.Value < DateTime.Now)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser menor a la fecha actual"));
                }
                else if (meses > 10)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser mayor a la fecha actual por 10 meses"));
                }
                else if (dtpFechaInic.SelectedDate.Value > dtpFechaTerm.SelectedDate.Value)//valida que  fecha hora-termino no sea menor a fecha inc
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser mayor a la fecha-hora termino del evento"));
                }
                #endregion
                else
                {
                    string numeroCont = txtNumcont.Text;
                    DateTime creacionNow = DateTime.Parse(txtCreacion.Text); DateTime terminoNow = DateTime.Parse(txtTermino.Text);
                    #region estilos del BoxMessage
                    var settingOption = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "ACTUALIZAR VALOR",
                        NegativeButtonText = "TERMINAR CONTRATO"
                    };
                    var settingYesNo = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "SI",
                        NegativeButtonText = "NO"
                    };
                    #endregion
                    var dialogOpcion = await this.ShowMessageAsync("Alerta", "¿Actualizar valor del contrato o término a este contrato ?", MessageDialogStyle.AffirmativeAndNegative, settingOption);


                    #region ACTUALIZAR VALOR CONTRATO
                    if (dialogOpcion == MessageDialogResult.Affirmative)
                    {
                        Contrato contrato = new Contrato()
                        {
                            Numero = numeroCont,
                            Creacion = creacionNow,
                            Termino = DateTime.Parse("12-12-3000"),
                            IdTipoEvento = (int)cboTIpo.SelectedValue,
                            IdModalidad = (string)cboModalidad.SelectedValue,
                            Realizado = true,
                            RutCliente = txtRut.Text,
                            Asistentes = int.Parse(txtAsistentes.Text),
                            PersonalAdicional = int.Parse(txtPersonal.Text),
                            Observaciones = txtObservacion.Text,
                            ValorTotalContrato = double.Parse(txtTotalfinal.Text),
                            FechaHoraInicio = dtpFechaInic.SelectedDate.Value,
                            FechaHoraTermino = dtpFechaTerm.SelectedDate.Value
                        };
                        if (contrato.Update())
                        {
                            await this.ShowMessageAsync("Alerta:", String.Format("Valor total del Contrato actualizado con exito!!!"));
                            if ((int)cboTIpo.SelectedValue == 10)
                            {
                                bool isvegan = Vegetariana();
                                CoffeBreak coffe = new CoffeBreak()
                                {
                                    Numero = contrato.Numero,
                                    Vegetariana = isvegan
                                };
                                if (coffe.Update())
                                {
                                    await this.ShowMessageAsync("Alerta:", String.Format("Contrato y/o Valor Total actualizado con exito!!!"));
                                    LimpiarControls();
                                }
                            }
                            else if ((int)cboTIpo.SelectedValue == 20)
                            {
                                bool muscli = musclisi();
                                bool musamb = musambsi();
                                bool ambsi = Ambientacion();
                                Cocktail cocktail = new Cocktail()
                                {
                                    Numero = contrato.Numero,
                                    IdTipoAmbientacion = (int)cboAmbientacion.SelectedValue,
                                    Ambientacion = ambsi,
                                    MusicaCliente = muscli,
                                    MusicaAmbiental = musamb

                                };
                                if (cocktail.Update())
                                {
                                    await this.ShowMessageAsync("Alerta:", String.Format("Contrato y/o Valor Total actualizado con exito!!!"));
                                    LimpiarControls();
                                };
                            }
                            else if ((int)cboTIpo.SelectedValue == 30)
                            {
                                bool localOB = Local();
                                bool otrolocal = OtroLocal();
                                bool mamb = CheckAmbiental();
                                double arriendo = arriendoLocal();
                                Cenas cenas = new Cenas()
                                {
                                    Numero = contrato.Numero,
                                    IdTipoAmbientacion = (int)cboAmbientacion.SelectedValue,
                                    LocalOnBreak = localOB,
                                    MusicaAmbiental = mamb,
                                    OtroLocalOnBreak = otrolocal,
                                    ValorArriendo = arriendo
                                };
                                if (cenas.Update())
                                {
                                    await this.ShowMessageAsync("Alerta:", String.Format("Contrato y/o Valor Total actualizado con exito!!!"));
                                    LimpiarControls();
                                }
                            }
                        }
                        else
                        {
                            await this.ShowMessageAsync("Alerta:", String.Format("El valor del contrato no se pudo actualizar!!!"));

                        }
                    }
                    #endregion
                    #region OPCION TERMINO CONTRATO
                    else
                    {
                        var dialogTermino = await this.ShowMessageAsync("Alerta", "¿Está seguro de eliminar el contrato?", MessageDialogStyle.AffirmativeAndNegative, settingYesNo);
                        if (dialogTermino == MessageDialogResult.Affirmative)
                        {
                            if (rdbActiva.IsChecked == true)
                            {
                                Contrato contrato = new Contrato()
                                {
                                    Numero = numeroCont,
                                    Creacion = creacionNow,
                                    Termino = DateTime.Now,
                                    IdTipoEvento = (int)cboTIpo.SelectedValue,
                                    IdModalidad = (string)cboModalidad.SelectedValue,
                                    Realizado = false,
                                    RutCliente = txtRut.Text,
                                    Asistentes = int.Parse(txtAsistentes.Text),
                                    PersonalAdicional = int.Parse(txtPersonal.Text),
                                    Observaciones = txtObservacion.Text,
                                    ValorTotalContrato = double.Parse(txtTotalfinal.Text),
                                    FechaHoraInicio = dtpFechaInic.SelectedDate.Value,
                                    FechaHoraTermino = dtpFechaTerm.SelectedDate.Value
                                };
                                if (contrato.Update())
                                {
                                    await this.ShowMessageAsync("Alerta:", String.Format("Contrato terminado con exito!!!"));
                                    rdbInactiva.IsChecked = true;
                                }
                                else
                                {
                                    await this.ShowMessageAsync("Alerta:", String.Format("El contrato no se pudo actualizar!!!"));

                                }
                            }
                            else
                            {
                                await this.ShowMessageAsync("Alerta:", String.Format("El contrato debe estar vigente para realizar término!!!"));

                            }
                        }
                        else if (dialogTermino == MessageDialogResult.Negative)
                        {
                            await this.ShowMessageAsync("Alerta:", String.Format("Termino de contrato denegado!!!"));
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }
        #endregion

        private void cboTIpo_SourceUpdated(object sender, DataTransferEventArgs e)
        {


        }

        private void chkAmbietacion_Checked(object sender, RoutedEventArgs e)
        {
            cboAmbientacion.Visibility = Visibility.Visible;
        }

        private void txtArriendo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void MostrarOpEvent()
        {
            TipoAmbientacion();
            if (cboTIpo.SelectedIndex == 0)
            {
                brOpcionEventSmall.Visibility = Visibility.Visible;
                lblOpcionesEvent.Visibility = Visibility.Visible;
                lblCoffee.Visibility = Visibility.Visible;
                chkVegetarian.Visibility = Visibility.Visible;

                #region CONTROLES COLLAPSED
                brOpcionEvent.Visibility = Visibility.Collapsed;
                brOpcionEventCT.Visibility = Visibility.Collapsed;
                chkAmbientacion.Visibility = Visibility.Collapsed;
                chkAmbientacion.IsChecked = false;
                chkMusicAmb.IsChecked = false;
                chkMusicAmb.Visibility = Visibility.Collapsed;
                lblAmbientacion.Visibility = Visibility.Collapsed;
                lblLocal.Visibility = Visibility.Collapsed;
                lblMusica.Visibility = Visibility.Collapsed;
                lblCocktail.Visibility = Visibility.Collapsed;
                lblCenas.Visibility = Visibility.Collapsed;
                lblArriendo.Visibility = Visibility.Collapsed;
                cboAmbientacion.Visibility = Visibility.Collapsed;
                rdbAmbiental.Visibility = Visibility.Collapsed;
                rdbCliente.Visibility = Visibility.Collapsed;
                rdbLocalOnbreak.Visibility = Visibility.Collapsed;
                rdbOtroLocal.Visibility = Visibility.Collapsed;
                txtArriendo.Visibility = Visibility.Collapsed;

                #endregion
            }
            else if (cboTIpo.SelectedIndex == 1)
            {

                #region CONTROLES VISIBLES
                chkAmbientacion.IsChecked = false;
                chkMusicAmb.IsChecked = false;
                brOpcionEventCT.Visibility = Visibility.Visible;
                brOpcionEvent.Visibility = Visibility.Visible;
                lblOpcionesEvent.Visibility = Visibility.Visible;
                lblCocktail.Visibility = Visibility.Visible;
                lblAmbientacion.Visibility = Visibility.Visible;
                chkAmbientacion.Visibility = Visibility.Visible;
                lblMusica.Visibility = Visibility.Visible;
                rdbAmbiental.Visibility = Visibility.Visible;
                rdbCliente.Visibility = Visibility.Visible;

                #endregion
                #region CONTROLES COLLAPSED
                brOpcionEventSmall.Visibility = Visibility.Collapsed;
                brOpcionEvent.Visibility = Visibility.Collapsed;
                lblCoffee.Visibility = Visibility.Collapsed;
                chkVegetarian.Visibility = Visibility.Collapsed;
                chkMusicAmb.Visibility = Visibility.Collapsed;
                lblCenas.Visibility = Visibility.Collapsed;
                lblLocal.Visibility = Visibility.Collapsed;
                rdbLocalOnbreak.Visibility = Visibility.Collapsed;
                rdbOtroLocal.Visibility = Visibility.Collapsed;
                lblArriendo.Visibility = Visibility.Collapsed;
                txtArriendo.Visibility = Visibility.Collapsed;
                cboAmbientacion.Visibility = Visibility.Collapsed;
                #endregion
            }
            else if (cboTIpo.SelectedIndex == 2)
            {
                #region CONTROLES VISIBLES
                chkAmbientacion.IsChecked = false;
                chkMusicAmb.IsChecked = false;
                brOpcionEvent.Visibility = Visibility.Visible;
                lblOpcionesEvent.Visibility = Visibility.Visible;
                lblCenas.Visibility = Visibility.Visible;
                lblAmbientacion.Visibility = Visibility.Visible;
                lblLocal.Visibility = Visibility.Visible;
                rdbLocalOnbreak.Visibility = Visibility.Visible;
                rdbOtroLocal.Visibility = Visibility.Visible;
                cboAmbientacion.Visibility = Visibility.Visible;
                chkMusicAmb.Visibility = Visibility.Visible;


                #endregion

                #region CONTROLES COLLAPSED
                txtArriendo.Visibility = Visibility.Collapsed;
                lblCocktail.Visibility = Visibility.Collapsed;
                lblCoffee.Visibility = Visibility.Collapsed;
                lblArriendo.Visibility = Visibility.Collapsed;
                lblMusica.Visibility = Visibility.Collapsed;
                chkAmbientacion.Visibility = Visibility.Collapsed;
                brOpcionEventSmall.Visibility = Visibility.Collapsed;
                brOpcionEventCT.Visibility = Visibility.Collapsed;
                chkVegetarian.Visibility = Visibility.Collapsed;
                rdbAmbiental.Visibility = Visibility.Collapsed;
                rdbCliente.Visibility = Visibility.Collapsed;

                #endregion
            }
        }

        private void chkAmbientacion_Checked(object sender, RoutedEventArgs e)
        {
            cboAmbientacion.Visibility = Visibility.Visible;
        }

        private void chkAmbientacion_Unchecked(object sender, RoutedEventArgs e)
        {
            cboAmbientacion.Visibility = Visibility.Collapsed;
        }

        public void TipoAmbientacion()
        {
            TipoAmbientacion amb = new TipoAmbientacion();
            cboAmbientacion.DisplayMemberPath = "Descripcion";
            cboAmbientacion.SelectedValuePath = "IdTipoAmbientacion";
            cboAmbientacion.ItemsSource = amb.ReadAll();
            cboAmbientacion.SelectedIndex = -1;
        }

        private void rdbLocalOnbreak_Checked(object sender, RoutedEventArgs e)
        {
            txtArriendo.Visibility = Visibility.Visible;
            lblArriendo.Visibility = Visibility.Visible;
        }

        private void rdbLocalOnbreak_Unchecked(object sender, RoutedEventArgs e)
        {
            txtArriendo.Visibility = Visibility.Collapsed;
            lblArriendo.Visibility = Visibility.Collapsed;
        }

        public void OcultarAllControls()
        {
            #region CONTROLES COLLAPSED
            lblOpcionesEvent.Visibility = Visibility.Collapsed;
            lblCocktail.Visibility = Visibility.Collapsed;
            lblCoffee.Visibility = Visibility.Collapsed;
            lblCenas.Visibility = Visibility.Collapsed;
            lblLocal.Visibility = Visibility.Collapsed;
            lblArriendo.Visibility = Visibility.Collapsed;
            lblAmbientacion.Visibility = Visibility.Collapsed;
            lblMusica.Visibility = Visibility.Collapsed;
            chkVegetarian.Visibility = Visibility.Collapsed;
            chkMusicAmb.Visibility = Visibility.Collapsed;
            chkAmbientacion.Visibility = Visibility.Collapsed;
            rdbAmbiental.Visibility = Visibility.Collapsed;
            rdbCliente.Visibility = Visibility.Collapsed;
            rdbLocalOnbreak.Visibility = Visibility.Collapsed;
            rdbOtroLocal.Visibility = Visibility.Collapsed;
            brOpcionEventCT.Visibility = Visibility.Collapsed;
            brOpcionEvent.Visibility = Visibility.Collapsed;
            brOpcionEventSmall.Visibility = Visibility.Collapsed;
            txtArriendo.Visibility = Visibility.Collapsed;
            cboAmbientacion.Visibility = Visibility.Collapsed;
            #endregion
        }

        public async void UpdateContrato()
        {
            string numeroCont = txtNumcont.Text;
            #region validaciones de ingreso controles
            if (cboModalidad.SelectedIndex < 0 || cboTIpo.SelectedIndex < 0 || txtObservacion.Text == string.Empty)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Debe seleccionar algun item de 'tipo de evento', 'modalidad de evento' " +
                                                                    "y no dejar espacio en 'Observación'!!!"));
            }
            else if (txtRut.Text == string.Empty)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo rut !!"));
            }
            else if (!Enumerable.Range(1, 200).Contains(int.Parse(txtAsistentes.Text))
                || !Enumerable.Range(1, 200).Contains(int.Parse(txtPersonal.Text))) //txtAsistentes.Text == string.Empty || txtPersonal.Text == string.Empty ||txtTotalfinal.Text == string.Empty     
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No dejar con 0  los campos de asistentes o personal adicional!!!"));
            }
            else if (txtAsistentes.Text != string.Empty || txtPersonal.Text != string.Empty)//txtAsistentes.Text == string.Empty || txtPersonal.Text == string.Empty ||txtTotalfinal.Text == string.Empty     
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No dejar en blanco los campos de asistentes o personal adicional!!!"));
            }
            else if (dtpFechaInic.SelectedDate == null || dtpFechaTerm.SelectedDate == null)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No dejar vacio los Datepickers(Calendarios) de Fecha,Hora Inicio y" +
                                               " de Fecha, Hora término de evento!!!"));
            }
            else if (txtCreacion.Text != string.Empty)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No dejar vacio el campo de creación"));
            }
            else
            {
                #region validaciones de fechas
                int hours = (int)(dtpFechaTerm.SelectedDate.Value - dtpFechaInic.SelectedDate.Value).TotalHours;//caluclo de horas entre 2 fechas
                TimeSpan fecha = (dtpFechaTerm.SelectedDate.Value - dtpFechaInic.SelectedDate.Value);//diferencia de fechas en mes..año..fecha..dias
                int meses = Math.Abs((DateTime.Now.Month - dtpFechaInic.SelectedDate.Value.Month) + 12 * (DateTime.Now.Year - dtpFechaInic.SelectedDate.Value.Year));
                //String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                //fecha.Days, fecha.Hours, fecha.Minutes, fecha.Seconds);
                if (hours > 24)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora evento no puede durar mas de 24 horas,el evento sobrepasa con {0} hrs y {1} minutos ", hours, fecha.Minutes));
                }
                else if (dtpFechaInic.SelectedDate.Value < DateTime.Now)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser menor a la fecha actual"));
                }
                else if (meses > 10)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser mayor a la fecha actual por 10 meses"));
                }
                else if (dtpFechaInic.SelectedDate.Value > dtpFechaTerm.SelectedDate.Value)//valida que  fecha hora-termino no sea menor a fecha inc
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser mayor a la fecha-hora termino del evento"));
                }
                #endregion
                else
                {
                    Contrato contrato = new Contrato()
                    {
                        Numero = numeroCont,
                        Creacion = DateTime.Now,
                        Termino = DateTime.Now,
                        IdTipoEvento = (int)cboTIpo.SelectedValue,
                        IdModalidad = (string)cboModalidad.SelectedValue,
                        Realizado = true,
                        RutCliente = txtRut.Text,
                        Asistentes = int.Parse(txtAsistentes.Text),
                        //Asistentes = 0,
                        PersonalAdicional = int.Parse(txtPersonal.Text),
                        //PersonalAdicional = 0,
                        Observaciones = this.txtObservacion.Text,
                        //ValorTotalContrato = double.Parse(txtTotalfinal.Text),
                        ValorTotalContrato = int.Parse(txtTotalfinal.Text),
                        FechaHoraInicio = dtpFechaInic.SelectedDate.Value,
                        FechaHoraTermino = dtpFechaTerm.SelectedDate.Value
                    };
                    if (contrato.Create())
                    {
                        LimpiarControls();
                        await this.ShowMessageAsync("Alerta:", String.Format("Contrato creado con exito!!!"));
                    }
                    else
                    {
                        await this.ShowMessageAsync("Alerta:", String.Format("El contrato no se pudo registrar!!!"));
                    }
                }
            }
            #endregion
        }
        public bool Vegetariana()
        {
            bool veg = false;
            if (chkVegetarian.IsChecked == false)
            {
                veg = false;
            } else if (chkVegetarian.IsChecked == true)
            {
                veg = true;
            } return veg;
        }
        public bool Ambientacion()
        {
            bool amb = false;
            if (cboAmbientacion.SelectedIndex == 0)
            {
                amb = false;
            } else if (cboAmbientacion.SelectedIndex == 1)
            {
                amb = true;
            } return amb;
        }
        public int IdAmbientacion()
        {
            int IdAmbientacion = 0;
            if(Ambientacion()== true)
            {
                IdAmbientacion = 20;
            }else if (Ambientacion() == false)
            {
                IdAmbientacion = 10;
            }return IdAmbientacion;
        }
        public bool musclisi()
        {
            bool musica = false;
            if (rdbCliente.IsChecked == false)
            {
                musica = false;
            }else if (rdbCliente.IsChecked == true)
            {
                musica = true;
            }return musica;
        }

        public bool musambsi()
        {
            bool musamb = false;
            if (rdbAmbiental.IsChecked == false)
            {
                musamb = false;
            }else if (rdbAmbiental.IsChecked == true)
            {
                musamb = true;
            }return musamb;
        }
        public bool Local()
        {
            bool loc = false;
            if (rdbLocalOnbreak.IsChecked == false)
            {
                loc = false;
            }else if (rdbLocalOnbreak.IsChecked == true)
            {
                loc = true;
            }return loc;
        }
        public bool CheckAmbiental()
        {
            bool check = false;
            if (chkMusicAmb.IsChecked == false)
            {
                check = false;
            }else if (chkMusicAmb.IsChecked == true)
            {
                check = true;
            }return check;
        }
        public bool OtroLocal()
        {
            bool oloc = false;
            if(rdbOtroLocal.IsChecked == false)
            {
                oloc = false;
            }else if (rdbOtroLocal.IsChecked == true)
            {
                oloc = true;
            }return oloc;
        }
        public double arriendoLocal()
        {
            int arriendo = 0;
            if(Local()== true)
            {
                arriendo = int.Parse(txtArriendo.Text);
            }
            else if (Local() == false)
            {
                arriendo = 0;
            }return arriendo;
        }

        private async void btnCambios_Click(object sender, RoutedEventArgs e)
        {
            #region validaciones de ingreso controles
            if (cboModalidad.SelectedIndex < 0 || cboTIpo.SelectedIndex < 0 || txtObservacion.Text == string.Empty)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("Debe seleccionar algun item de 'tipo de evento', 'modalidad de evento' " +
                                                                    "y no dejar espacio en 'Observación'!!!"));
            }
            else if (txtRut.Text == string.Empty)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No deje vacio el campo rut !!"));
            }
            //else if (!Enumerable.Range(1, 200).Contains(int.Parse(txtAsistentes.Text)) 
            //    || !Enumerable.Range(1, 200).Contains(int.Parse(txtPersonal.Text))) //txtAsistentes.Text == string.Empty || txtPersonal.Text == string.Empty ||txtTotalfinal.Text == string.Empty     
            //{
            //    await this.ShowMessageAsync("Alerta:", String.Format("No dejar con 0  los campos de asistentes o personal adicional!!!"));
            //}
            else if (txtAsistentes.Text == string.Empty || txtPersonal.Text== string.Empty)//txtAsistentes.Text == string.Empty || txtPersonal.Text == string.Empty ||txtTotalfinal.Text == string.Empty     
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No dejar en blanco los campos de asistentes o personal adicional!!!"));
            }
            else if (dtpFechaInic.SelectedDate == null || dtpFechaTerm.SelectedDate == null)
            {
                await this.ShowMessageAsync("Alerta:", String.Format("No dejar vacio los Datepickers(Calendarios) de Fecha,Hora Inicio y" +
                                               " de Fecha, Hora término de evento!!!"));
            }

            else
            {
                #region validaciones de fechas
                int hours = (int)(dtpFechaTerm.SelectedDate.Value - dtpFechaInic.SelectedDate.Value).TotalHours;//caluclo de horas entre 2 fechas
                TimeSpan fecha = (dtpFechaTerm.SelectedDate.Value - dtpFechaInic.SelectedDate.Value);//diferencia de fechas en mes..año..fecha..dias
                int meses = Math.Abs((DateTime.Now.Month - dtpFechaInic.SelectedDate.Value.Month) + 12 * (DateTime.Now.Year - dtpFechaInic.SelectedDate.Value.Year));
                //String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                //fecha.Days, fecha.Hours, fecha.Minutes, fecha.Seconds);
                if (hours > 24)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora evento no puede durar mas de 24 horas,el evento sobrepasa con {0} hrs y {1} minutos ", hours, fecha.Minutes));
                }
                else if (dtpFechaInic.SelectedDate.Value < DateTime.Now)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser menor a la fecha actual"));
                }
                else if (meses > 10)
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser mayor a la fecha actual por 10 meses"));
                }
                else if (dtpFechaInic.SelectedDate.Value > dtpFechaTerm.SelectedDate.Value)//valida que  fecha hora-termino no sea menor a fecha inc
                {
                    await this.ShowMessageAsync("Alerta:", String.Format("La fecha-hora inicio del evento, no puede ser mayor a la fecha-hora termino del evento"));
                }
                #endregion
                else
                {
                    Contrato contrato = new Contrato()
                    {
                        Numero = DateTime.Now.ToString("yyyyMMddHHmm"),
                        Creacion = DateTime.Now,
                        Termino = DateTime.Parse("12-12-3000"),
                        IdTipoEvento = (int)cboTIpo.SelectedValue,
                        IdModalidad = (string)cboModalidad.SelectedValue,
                        Realizado = true,
                        RutCliente = txtRut.Text,
                        Asistentes = 0,
                        //PersonalAdicional = int.Parse(txtPersonal.Text),
                        PersonalAdicional = 0,
                        Observaciones = this.txtObservacion.Text,
                        //ValorTotalContrato = double.Parse(txtTotalfinal.Text),
                        ValorTotalContrato = 0,
                        FechaHoraInicio = dtpFechaInic.SelectedDate.Value,
                        FechaHoraTermino = dtpFechaTerm.SelectedDate.Value
                    };
                    caretaker.Memento = contrato.CreateMemento();
                    await this.ShowMessageAsync("Alerta:", String.Format("Cambios realizados!!"));

                }
            }
            #endregion
        }

        private void btnRecuperar_Click(object sender, RoutedEventArgs e)
        {
            Contrato contrato = new Contrato();
            contrato.SetMemento(caretaker.Memento);
            txtAsistentes.Text = contrato.Asistentes.ToString();
            txtObservacion.Text = contrato.Observaciones;
            txtRut.Text = contrato.RutCliente;
            txtPersonal.Text = contrato.PersonalAdicional.ToString();
            txtAsistentes.Text = contrato.Asistentes.ToString();
            txtTotalfinal.Text = contrato.ValorTotalContrato.ToString();
            dtpFechaInic.SelectedDate = contrato.FechaHoraInicio;
            dtpFechaTerm.SelectedDate = contrato.FechaHoraTermino;
            txtCreacion.Text = contrato.Creacion.ToString();
            txtTermino.Text = contrato.Termino.ToString();
            cboTIpo.SelectedValue = contrato.IdTipoEvento;
            if (contrato.Realizado)
            {
                rdbActiva.IsChecked = true;
            }
            else
            {
                rdbInactiva.IsChecked = false;
            }
        }
    }
}

        
    
