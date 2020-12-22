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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using BE;
namespace PLWPF.GRfolder
{
    /// <summary>
    /// Interaction logic for deleteByNameUC.xaml
    /// </summary>
    public partial class deleteByNameUC : UserControl
    {
        BE.GuestRequest guest;
        IBL bl = FactoryBL.getIBL();
        public deleteByNameUC()
        {
            InitializeComponent();
        }
        public deleteByNameUC(BE.GuestRequest gruc)
        {
            InitializeComponent();
            grid1.DataContext = gruc;
        }

        private void GSpick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                guest = bl.GetGuestRequestByKey(Convert.ToInt64(guestRequestKey1Label.Content));
                long tempkey = guest.GuestRequestKey1;
                bl.DeleteGuestRequest(guest);
                MessageBox.Show("Guest Request deleted, Key: " + tempkey);
                Window GuestRequestWindow = new GuestRequest();
                GuestRequestWindow.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Guest Request does not exist");
                Window GuestRequestWindow = new GuestRequest();
                GuestRequestWindow.Show();
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // Do not load your data at design time.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

    }
}
