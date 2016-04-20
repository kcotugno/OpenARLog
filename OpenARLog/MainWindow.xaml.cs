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
using System.Windows;
using System.Windows.Input;

using OpenARLog.Data;

namespace OpenARLog
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private QSOLog _qsoLog;

        private bool uiVisible = false;

        // Types
        private TypeDataDb _typeDataDb;

        private BandsManager _bandsManager;
        private ModesManager _modesManager;
        private CountriesManager _countriesManager;

        #region Initialization Methods

        public MainWindow()
        {
            InitializeComponent();

            // Restore window settings
            Width = Properties.Settings.Default.WindowWidth;
            Height = Properties.Settings.Default.WindowHeigth;
            WindowState = Properties.Settings.Default.IsWindowMaximized == true ? WindowState.Maximized : WindowState.Normal;

            // Make UX better
            callsignTxt.Focus();

            // Hide extra entry fields.
            moreContGroup.Visibility = Visibility.Collapsed;
            extraQSLGroup.Visibility = Visibility.Collapsed;

            InitializeDataBinding();
        }

        private void InitializeDataBinding()
        {
            // Log data
            _qsoLog = new QSOLog();
            
            _qsoLog.OpenConnection(Properties.Settings.Default.LogPath);

            qsoGrid.DataContext = _qsoLog;
            qsoGrid.ItemsSource = _qsoLog.QSOs;

            // Type data
            _typeDataDb = new TypeDataDb();

            _bandsManager = new BandsManager(_typeDataDb);
            _modesManager = new ModesManager(_typeDataDb);
            _countriesManager = new CountriesManager(_typeDataDb);

            _bandsManager.LoadAndUpdate();
            _modesManager.LoadAndUpdate();
            _countriesManager.LoadAndUpdate();

            bandTxt.ItemsSource = _bandsManager.Bands;
            modeTxt.ItemsSource = _modesManager.Modes;
            countryTxt.ItemsSource = _countriesManager.Countries;
        }

        #endregion

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.WindowWidth = Width;
            Properties.Settings.Default.WindowHeigth = Height;
            Properties.Settings.Default.IsWindowMaximized = WindowState == WindowState.Maximized ? true : false;

            Properties.Settings.Default.LogPath = _qsoLog.Path;

            Properties.Settings.Default.Save();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            _qsoLog.Close();
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

        private void InfoKeyDown(object sender, RoutedEventArgs e)
        {
            KeyEventArgs key = (KeyEventArgs)e;
            if (key.Key == Key.Enter)
                LogBtnClick(sender, e);
        }

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
            qsoGrid.Items.Refresh();
            ClearUIFields();
        }

        private void ClearBtnBlick(object sender, RoutedEventArgs e)
        {
            ClearUIFields();
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

        #region UI

        private void ClearUIFields()
        {
            callsignTxt.Text = string.Empty;
            nameTxt.Text = string.Empty;
            countryTxt.SelectedIndex = -1;
            stateTxt.SelectedIndex = -1;
            cityTxt.Text = string.Empty;
            gridSquareTxt.Text = string.Empty;

            ageTxt.Text = string.Empty;
            countryTxt.Text = string.Empty;
            distTxt.Text = string.Empty;
            emailTxt.Text = string.Empty;

            bandTxt.SelectedIndex = -1;
            frequencyTxt.Text = string.Empty;
            modeTxt.SelectedIndex = -1;

            timeOnTxt.Text = string.Empty;
            dateOnTxt.Text = string.Empty;
            timeOffTxt.Text = string.Empty;
            dateOffTxt.Text = string.Empty;

            qslRecTxt.SelectedIndex = -1;
            qslSentTxt.SelectedIndex = -1;

            callsignTxt.Focus();
        }

        #endregion

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
