﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
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
    /// Interaction logic for GRuserControlForAddOrder.xaml
    /// </summary>
    public partial class GRuserControlForAddOrder : UserControl
    {
        BackgroundWorker bg;
        NetworkCredential credentials;
        SmtpClient client;
        BE.Order order;
        long key2;
        long hostkey1;
        BE.HostingUnit hu;
        BE.GuestRequest gr;
        IBL bl = FactoryBL.getIBL();
        BE.BEEnum.Status mailsent1 = BE.BEEnum.Status.mailSent;
        public GRuserControlForAddOrder(BE.GuestRequest guesty, long key1,long hostkey)
        {
            InitializeComponent();
            if (hebEnglish.hebrew)
                hebChange();
            key2 = key1;
            hostkey1 = hostkey;
            gr = guesty;
            hu = bl.GetHostingUnitByKey(key2);
            grid1.DataContext = guesty;
            order = new BE.Order();


            client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = credentials
            };
        }
        private void hebChange()
        {
            rdate.Content = "תאריך הרשמה";
            pname.Content = "שם פרטי";
            sub.Content = "תת איזור";
            grkey.Content = "מספר לבקשת אירוח";
            status.Content = "סטטוס";
            GSpick.Content = "תבחר אותי!";
            childA.Content = "אטרקציות לילדים";
            jac.Content = "גקוזי";
            fname.Content = "שם משפחה";
            edate.Content = "תאריך כניסה";
            mail.Content = "מייל";
            pool.Content = "בריכה";
            type.Content = "סוג";
            adults.Content = "מבוגרים";
            area.Content = "איזור";
            children.Content = "ילדים";
            regdate.Content = "תאריך הרשמה";
        }
        private void GSpick_Click(object sender, RoutedEventArgs e)
        {
            //we need to make the occupied dates set as occupied in the diary of the hosting unit
            int error = 0;
            for (DateTime date = gr.EntryDate1; date <= gr.ReleaseDate1; date = date.AddDays(1))
            {
               if( hu.Diary1[date.Month - 1, date.Day - 1] == true)
                {
                    MessageBox.Show("cannot preform order, the dates are allready occupied");
                    error++;
                }
            }
            if (error == 0)
            {
                for (DateTime date = gr.EntryDate1; date <= gr.ReleaseDate1; date = date.AddDays(1))
                {
                    hu.Diary1[date.Month - 1, date.Day - 1] = true;
                }
                bl.UpdateHostingUnit(hu);
                order.HostingUnitKey1 = key2;
                order.hostKey1 = hostkey1;
                order.GuestRequestKey1 = Convert.ToInt64(guestRequestKey1Label.Content);
                bl.AddOrder(order);
                MessageBox.Show("order added, order key:" + order.OrderKey1);
                //sending mail
                bg = new BackgroundWorker();
                bg.DoWork += Bg_DoWork;
                bg.RunWorkerCompleted += Bg_RunWorkerCompleted;
                bg.RunWorkerAsync(new List<Object> { client, "moshesspam@gmail.com" });

            }
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            const string message = "Email sccessfully sent";
            MessageBox.Show(message);
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(gr.MailAddress1);//the guest email
            mail.From = new MailAddress(hu.Owner1.MailAddress1);//the HU owners email
            mail.Subject = "order added";
            mail.Body = "your guest request has been added to an order, hosting unit name:" + hu.HostingUnitName1 + "\n" +
                "hosting unit info:" + "\n" +
                "hostin unit owner's name:" + hu.Owner1.PrivateName1 + " " + hu.Owner1.FamilyName1 + "\n" +
                "location:" + hu.AreaOfHostingUnit.ToString() + "\n" +
                "has pool:" + hu.hasPool1.ToString() + "\n" +
                "has Garden:" + hu.hasGarden1.ToString() + "\n" +
                "has Jaccuzzi:" + hu.hasJaccuzzi1.ToString() + "\n" +
                "has Childrens Attractions:" + hu.hasChildrensAttractions1.ToString() + "\n" +
                "commission:" + hu.Commission1 + "\n" +
                "have a good day!";
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("moshesspam@gmail.com", "ihatespam");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
                //changing the gr status to mail sent
                gr.status1 = mailsent1;
                bl.UpdateGuestRequest(gr);
                order.Status1 = BE.BEEnum.Status.mailSent;
                bl.UpdateOrder(order);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
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
