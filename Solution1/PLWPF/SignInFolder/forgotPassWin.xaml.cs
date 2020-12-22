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
using System.Net.Mail;
using System.Threading;
using BL;
using System.Xml.Linq;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for forgotPassWin.xaml
    /// </summary>
    public partial class forgotPassWin : Window
    {
        string newPass;
        IBL bl = FactoryBL.getIBL();
        String hostPath = "@HostXml.xml";
        XElement hostRoot = new XElement("hostsInfo");
        XElement guestRoot = new XElement("guestsInfo");
        string guestPath = "@GuestXml.xml";


        public forgotPassWin()
        {
            InitializeComponent();
            if (hebEnglish.hebrew)
                hebChange();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                hostRoot = XElement.Load("@HostXml.xml");

                guestRoot = XElement.Load("@GuestXml.xml");
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (usernametxtbx.Text == "")
            {
                MessageBox.Show("please enter your user name", "error", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                usernametxtbx.Focus();
            }
            else
            {


                newPass = putrand();

                try
                {
                    BE.Host host = bl.getHostByUser(usernametxtbx.Text);
                    Thread passmail = new Thread(() => SendMail(host.MailAddress1));
                    passmail.Start();
                    updatePasswordHost();

                    var win = new SignInPage();
                    win.Show();
                    this.Close();
                }
                catch
                {
                    try
                    {
                        string mal = GetGuestByuser();
                        Thread passmail = new Thread(() => SendMail(mal));
                        passmail.Start();
                        updatePasswordGuest();
                    }
                    catch
                    {
                        MessageBox.Show("this user is not in the system");
                        usernametxtbx.Clear();
                        usernametxtbx.Focus();
                    }
                }               
            }

        }
        private string GetGuestByuser()
        {
            XElement m = (from g in guestRoot.Elements()
                          where g.Element("username").Value == usernametxtbx.Text
                          select g).Single();
            string ma = m.Element("mail").Value;
            return ma;
        }
        private void SendMail(string mailing)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(mailing);//the guest email
            mail.From = new MailAddress("funstay.mini@proj");//the HU owners email
            mail.Subject = "order added";
            mail.Body = "your new password is:\n" + newPass + "\n" +
                "have a good day!";
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("moshesspam@gmail.com", "ihatespam");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
                MessageBox.Show("please wait to receive a mail.");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void updatePasswordGuest()
        {
            XElement guest;
            guest = (from g in guestRoot.Elements()
                     where g.Element("username").Value == usernametxtbx.Text
                     select g).Single();
            guest.Element("password").Value = newPass;
            guestRoot.Save(guestPath);
        }

        private void updatePasswordHost()
        {
            XElement host = (from ho in hostRoot.Elements()
                             where ho.Element("username").Value == usernametxtbx.Text
                             select ho).FirstOrDefault();
            host.Element("password").Value = newPass;
            hostRoot.Save(hostPath);

        }
        private string putrand()
        {
            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = " ";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }

            return pwd;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var win = new SignInPage();
            win.Show();
            this.Close();
        }
        private void hebChange()
        {
            lable1.Content=" !שכחת הסיסמה? כבר לא בעיה";
            backb.Content = "חזור";
            lable2.Content = "הכנס שם משתמש";
            sendb.Content = "שלח מייל";
            usernametxtbx.Watermark = "השם משתמש שלך";
        }
    }
}
