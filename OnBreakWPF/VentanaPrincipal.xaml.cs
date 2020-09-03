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

namespace OnBreakWPF
{
    /// <summary>
    /// Lógica de interacción para VentanaPrincipal.xaml
    /// </summary>
    public partial class VentanaPrincipal : MetroWindow
    {

        public bool estadodark;
        public VentanaPrincipal()
        {
            InitializeComponent();
            

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
           
        }

        

        private void imgDark_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void btnAdmincli_Click(object sender, RoutedEventArgs e)
        {
            AdminClientes ac = new AdminClientes();
            if (estadodark)
            {                
                ac.ThemeDark();           
            }
            this.Close();
            ac.Show();

        }

        private void btnAdmincont_Click(object sender, RoutedEventArgs e)
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
            this.Background = Brushes.White;
            swDarkTheme.Foreground = Brushes.Black;
            swDarkTheme.ThumbIndicatorBrush = Brushes.Black;
            lblSwitch.Foreground = Brushes.Black;
            estadodark = false;
            btnImgLight.Visibility = Visibility.Visible;
        }

        public void ThemeDark()
        {
            var bc = new BrushConverter();
            this.Background = Brushes.Black;
            swDarkTheme.Foreground = Brushes.White;
            swDarkTheme.ThumbIndicatorBrush = Brushes.White;
            lblSwitch.Foreground = (Brush)bc.ConvertFrom("#085394");
            swDarkTheme.IsChecked = true;
            btnImgLight.Visibility = Visibility.Collapsed;
            estadodark = true;
        }

        private void btnListarcont_Click(object sender, RoutedEventArgs e)
        {
            ListadoContratos listco = new ListadoContratos();
            if (estadodark)
            {
                listco.ThemeDark();
            }
            this.Close();
            listco.Show();
        }

               
    }
}
