/*
 * Copyright (C) 2015 kcotugno
 * Distributed under the MIT software license, see the accompanying
 * LICENSE file or http://www.opensource.org/licenses/MIT
 *
 * Author: kcotugno
 * Date: 8/15/2015
 */

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


namespace OpenARLog
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        #region Menu Click Handlers
        
        private void newDBMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void openDBMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void importADIFMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void exportADIFMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void aboutMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }
        
        private void exitMenuItemClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
        

        private void logBtnClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void resetBtnBlick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void showTODOMessage()
        {
            MessageBox.Show("TODO", "DEBUG", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}