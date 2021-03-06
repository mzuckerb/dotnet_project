﻿using BL;
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
using System.Xml.Linq;
using PLWPF.HostFolder;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostingUnit.xaml
    /// </summary>
    public partial class HostWindow : Window
    {
        string user;
        BE.Host host;
        XElement HostRoot = new XElement("hostsInfo");
        IBL bl = FactoryBL.getIBL();
        //public HostWindow()
        //{
        //    InitializeComponent();
        //    LoadData();
        //    host = new BE.Host();
        //    try
        //    {
        //        host = getOldestHostKey();

        //    }
        //    catch
        //    {


        //    }
        //}
        public HostWindow(string username)
        {
            InitializeComponent();
            if(hebEnglish.hebrew)
                hebChange();
            LoadData();
            user = username;
            host = new BE.Host();
            host = bl.getHostByUser(username);

        }
        private void hebChange()
        {
            Title = "מארח";
            BackButton.Content = "התנתק";
            ShowAll_Button.Content = "הראה כל היחידות אירוח";
            OrderUnitButton.Content = "הזמנה";
            UpdateUnitButton.Content = "עדכן/מחק יחידה";
            AddUnitButton.Content = "הוסף יחידה";
            AddOrder.Content = "הוסף הזמנה";
            UpdateOrder.Content = "עדכן הזמנה";
            showallorders.Content = "הראה כל ההזמנות";
                }
        private BE.Host getOldestHostKey()
        {
            BE.HostingUnit temp = new BE.HostingUnit();
            temp.Owner1 = new BE.Host();
            foreach (var item in bl.GetAllHostingUnits())
            {
                if (item.HostingUnitKey1 > temp.HostingUnitKey1)
                    temp = item;
            }
            foreach (var item in bl.GetAllHosts())
            {
                if (item.HostKey1 == temp.Owner1.HostKey1)
                    return item;
            }
            return null;
        }
        private void addUnitButton_Click(object sender, RoutedEventArgs e)
        {
            Window addUnitWindow = new AddUnit(host,user);
            addUnitWindow.Show();
            this.Close();

        }
        private void updateUnitButton_Click(object sender, RoutedEventArgs e)
        {
            Window updateUnitWindow = new updeletebyunit(user);
            updateUnitWindow.Show();
            this.Close();

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Window mWindow = new SignInPage();
            mWindow.Show();
            this.Close();

        }
        
        private long getHostKey(BE.Host host)
        {

            foreach (var item in bl.GetAllHosts())
            {
                if (host.FamilyName1 == item.FamilyName1 && host.PrivateName1 == item.PrivateName1 && host.MailAddress1 == item.MailAddress1)
                    return item.HostKey1;
            }
            return 0;
        }
        private void LoadData()
        {
            try
            {
                HostRoot = XElement.Load("@HostXml.xml");
            }
            catch
            {

                throw new Exception("File upload problem");
            }
        }



        private void OrderUnitButton_Click(object sender, RoutedEventArgs e)
        {
            this.OrderUnitButton.Visibility = Visibility.Hidden;
            this.AddOrder.Visibility = Visibility.Visible;
            this.UpdateOrder.Visibility = Visibility.Visible;
            this.showallorders.Visibility = Visibility.Visible;
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window addWindow = new AddOrder(Convert.ToInt64(host.HostKey1),user);
                addWindow.Show();
                this.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc + " please add requests or wait for a guest to add");
            }
        }

        private void UpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window updateOrderwindow = new HostFolder.updateOrderWindow(Convert.ToInt64(host.HostKey1),user);
                updateOrderwindow.Show();
                this.Close();
            }
            catch (Exception)
            {
            }
           
        }

        private void ShowAll_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window showallwindow = new showAllHUWindow(Convert.ToInt64(host.HostKey1),user);
                showallwindow.Show();
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("you have no hosting units!");
            }
        }

        private void showallorders_Click(object sender, RoutedEventArgs e)
        {
            var showallorderswindow = new hostShowAllOrders(Convert.ToInt64(host.HostKey1), user);
            showallorderswindow.Show();
            this.Close();
        }
    }
}
