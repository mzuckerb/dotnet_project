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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        IBL bl = FactoryBL.getIBL();
        List<BE.HostingUnit> hostingList = new List<BE.HostingUnit>();
        string username;
        private HUuserCuntrol makeReadOnly(HUuserCuntrol gruc)
        {
            gruc.hostingUnitName1TextBox.IsReadOnly = true;
            gruc.areatxtbx.IsReadOnly = true;
            gruc.commission1TextBox.IsReadOnly = true;
            gruc.hasChildrensAttractions1CheckBox.IsEnabled = false;
            gruc.hasPool1CheckBox.IsEnabled = false;
            gruc.hasJaccuzzi1CheckBox.IsEnabled = false;
            gruc.hasGarden1CheckBox.IsEnabled = false;


            return gruc;
        }
        public AddOrder(long hostkey,string user)
        {
            InitializeComponent();
            if (hebEnglish.hebrew)
                hebChange();
            hostingList = bl.GetAllHostingUnitsByHostKey(hostkey);
            username = user;
            scrollview1 = new ScrollViewer();

            foreach (BE.HostingUnit item in hostingList)
            {
                HUuserCuntrol gruc = new HUuserCuntrol(item,hostkey);
                gruc=makeReadOnly(gruc);
                b.Children.Add(gruc);
            }

            scrollview1.Content = b;
        }
        private void hebChange()
        {
            Back.Content = "חזור";
            Title = "הוסףהזמנה";
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window hostwin = new HostWindow(username);
            hostwin.Show();
            this.Close();
        }
    }
}
