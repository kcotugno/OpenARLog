/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
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

using OpenARLog.Data;

namespace OpenARLog
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QSOLog _qsoLog;
        List<QSO> _qsos;

        public MainWindow()
        {
            InitializeComponent();

            _qsoLog = new QSOLog();
            _qsoLog.OpenLog(Properties.Settings.Default.LogPath);

            _qsos = new List<QSO>();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            _qsoLog.CloseLog();
            _qsoLog.Dispose();
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

        #region Main Ui Events

        private void logBtnClick(object sender, RoutedEventArgs e)
        {
            if (IsValidTime(TimeOnTxt.Text) == false || IsValidDate(DateOnTxt.Text) == false)
                return;

            if (IsValidTime(TimeOffTxt.Text) == false || IsValidDate(DateOffTxt.Text) == false)
                return;

            QSO contact = new QSO()
            {
                Callsign = CallsignTxt.Text,
                Name = NameTxt.Text,
                Country = CountryTxt.Text,
                State = StateTxt.Text,
                County = CountyTxt.Text,
                City = CityTxt.Text,
                GridSquare = GridSquareTxt.Text,
                Band = BandTxt.Text,
                Mode = ModeTxt.Text,
                Frequency = FrequencyTxt.Text,
                DateTimeOn = GetDateTime(TimeOnTxt.Text, DateOnTxt.Text),
                DateTimeOff = GetDateTime(TimeOffTxt.Text, DateOffTxt.Text)
            };

            _qsoLog.InsertQSO(contact);
        }

        private void resetBtnBlick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void CallsignTxtChanged(object sender, RoutedEventArgs e)
        {
            if (CallsignTxt.Text == string.Empty)
                LogBtn.IsEnabled = false;
            else
                LogBtn.IsEnabled = true;
        }

        private void TimeOnGotFocus(object sender, RoutedEventArgs e)
        {
            if (TimeOnTxt.Text == string.Empty)
                TimeOnTxt.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void DateOnGotFocus(object sender, RoutedEventArgs e)
        {
            if (DateOnTxt.Text == string.Empty)
                DateOnTxt.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }

        private void TimeOffGotFocus(object sender, RoutedEventArgs e)
        {
            if (TimeOffTxt.Text == string.Empty)
                TimeOffTxt.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void DateOffGotFocus(object sender, RoutedEventArgs e)
        {
            if (DateOffTxt.Text == string.Empty)
                DateOffTxt.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }

        #endregion

        #region User Messages

        private void showTODOMessage()
        {
            MessageBox.Show("TODO", "DEBUG", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region Helper Functions

        private bool IsValidDateOrTime(string check, char delimiter)
        {
            if (check == string.Empty)
                return true;

            if (check.Length == 2 || check.Length == 3)
            {
                for (int i = 0; i < check.Length; i++)
                {
                    try
                    {
                        Convert.ToInt32(check[i]);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsValidTime(string time)
        {
            return IsValidDateOrTime(time, ':');
        }

        private bool IsValidDate(string date)
        {
            return IsValidDateOrTime(date, '/');
        }

        private DateTime? GetDateTime(string time, string date)
        {
            if (time == string.Empty && date == string.Empty)
                return null;

            int year = 1500;
            int month = 1;
            int day = 1;
            int hour = 0;
            int minutes = 0;
            int seconds = 0;

            string[] _date = date.Split('/');
            string[] _time = time.Split(':');

            DateTime datetime;
            
            try
            {
                month = Convert.ToInt32(_date[0]);
                day = Convert.ToInt32(_date[1]);
                year = _date.Length == 3 ? Convert.ToInt32(_date[2]) : 2016;

                // If the year is in 2 digit form, we will just assume it is a 2000 year.
                if (year < 100)
                    year += 2000;

                hour = Convert.ToInt32(_time[0]);
                minutes = Convert.ToInt32(_time[1]);
                seconds = _time.Length == 3 ? Convert.ToInt32(_time[2]) : 0;

                datetime = new DateTime(year, month, day, hour, minutes, seconds);
            }
            catch
            {
                return null;
            }


            return datetime;
        }

        #endregion
    }
}
