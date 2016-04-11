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
        private QSOLog _qsoLog;
        private List<QSO> _qsos;

        private bool uiVisible = false;

        // Types
        private TypeDataDb _dataTypesDb;

        private BandsManager _bandsdb;
        private CountriesManager _countriesMng;

        public List<BandModel> Bands { get { return _bands; } }
        public List<BandModel> _bands;

        public List<CountryModel> Countries { get { return _countries; } }
        private List<CountryModel> _countries;

        public MainWindow()
        {

            _qsoLog = new QSOLog();
            // TODO Add support for a log from other locations, loaded from preferences.
            _qsoLog.OpenLog(Properties.Settings.Default.LogPath);

            _qsos = new List<QSO>();

            _dataTypesDb = new TypeDataDb();

            _bandsdb = new BandsManager(_dataTypesDb);
            _bandsdb.LoadAndUpdate();
            _bands = _bandsdb.HamBands;

            _countriesMng = new CountriesManager(_dataTypesDb);
            _countriesMng.LoadAndUpdate();
            _countries = _countriesMng.Countries;

            DataContext = this;

            InitializeComponent();

            // Hide extra entry fields.
            moreContGroup.Visibility = Visibility.Collapsed;
            extraQSLGroup.Visibility = Visibility.Collapsed;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            _qsoLog.CloseLog();
            _qsoLog.Dispose();
        }
        
        #region Menu Click Handlers
        
        private void NewDBMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void OpenDBMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void ImportADIFMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void ExportADIFMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void AboutMenuClick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }
        
        private void ExitMenuItemClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Main Ui Events

        private void ShowBtnClick(object sender, RoutedEventArgs e)
        {
            if (uiVisible == true)
            {
                uiVisible = false;
                showBtn.Content = "More";

                moreContGroup.Visibility = Visibility.Collapsed;
                extraQSLGroup.Visibility = Visibility.Collapsed;
            }
            else
            {
                uiVisible = true;
                showBtn.Content = "Less";

                moreContGroup.Visibility = Visibility.Visible;
                extraQSLGroup.Visibility = Visibility.Visible;
            }
        }

        private void LogBtnClick(object sender, RoutedEventArgs e)
        {
            if (IsValidTime(timeOnTxt.Text) == false || IsValidDate(dateOnTxt.Text) == false)
                return;

            if (IsValidTime(timeOffTxt.Text) == false || IsValidDate(dateOffTxt.Text) == false)
                return;

            QSO contact = new QSO()
            {
                Callsign = callsignTxt.Text,
                Name = nameTxt.Text,
                Country = countryTxt.Text,
                State = stateTxt.Text,
                County = countyTxt.Text,
                City = cityTxt.Text,
                GridSquare = gridSquareTxt.Text,
                Band = bandTxt.Text,
                Mode = modeTxt.Text,
                Frequency = frequencyTxt.Text,
                DateTimeOn = GetDateTime(timeOnTxt.Text, dateOnTxt.Text),
                DateTimeOff = GetDateTime(timeOffTxt.Text, dateOffTxt.Text)
            };

            _qsoLog.InsertQSO(contact);
        }

        private void ResetBtnBlick(object sender, RoutedEventArgs e)
        {
            showTODOMessage();
        }

        private void CallsignTxtChanged(object sender, RoutedEventArgs e)
        {
            if (callsignTxt.Text == string.Empty)
                logBtn.IsEnabled = false;
            else
                logBtn.IsEnabled = true;
        }

        private void TimeOnGotFocus(object sender, RoutedEventArgs e)
        {
            if (timeOnTxt.Text == string.Empty)
                timeOnTxt.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void DateOnGotFocus(object sender, RoutedEventArgs e)
        {
            if (dateOnTxt.Text == string.Empty)
                dateOnTxt.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }

        private void TimeOffGotFocus(object sender, RoutedEventArgs e)
        {
            if (timeOffTxt.Text == string.Empty)
                timeOffTxt.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void DateOffGotFocus(object sender, RoutedEventArgs e)
        {
            if (dateOffTxt.Text == string.Empty)
                dateOffTxt.Text = DateTime.Now.ToString("MM/dd/yyyy");
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
