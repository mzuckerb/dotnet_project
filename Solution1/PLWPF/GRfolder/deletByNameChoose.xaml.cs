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
using BL;
using BE;
namespace PLWPF.GRfolder
{
    /// <summary>
    /// Interaction logic for deletByNameChoose.xaml
    /// </summary>
    public partial class deletByNameChoose : Window
    {
        public deletByNameChoose()
        {
            InitializeComponent();
        }
        IBL bl = FactoryBL.getIBL();
        List<BE.GuestRequest> guestList = new List<BE.GuestRequest>();
        public deletByNameChoose(string pname, string fname)
        {
            InitializeComponent();
            //if (hebEnglish.hebrew)
            //    hebChange();
            guestList = bl.GetallGuestRequestByName(pname, fname);

            scrollview1 = new ScrollViewer();

            foreach (BE.GuestRequest item in guestList)
            {
                if (item.status1 == BE.BEEnum.Status.pending)
                {
                    deleteByNameUC gruc = new deleteByNameUC(item);
                    b.Children.Add(gruc);
                }
            }
            if (b.Children.Count != 0)
            {
                scrollview1.Content = b;
            }
            else
            {
                MessageBox.Show("there are no guest requests you can delete1", "error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void backB_Click(object sender, RoutedEventArgs e)
        {
            Window GRMain = new GuestRequest();
            GRMain.Show();
            this.Close();
        }
    }
}
