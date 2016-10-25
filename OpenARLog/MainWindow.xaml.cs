/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
 *
 * Author: kcotugno
 * Date: 8/15/2015
 */

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

using OpenARLog.Data;
using OpenARLog.ADIF;

namespace OpenARLog
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private QSOLog _qsoLog;

        // Holds the logging stations information
        private QSO _operator = new QSO();

        private bool uiVisible = false;

        // Types
        private TypeDataDb _typeDataDb;

        private BandsManager _bandsManager;
        private ModesManager _modesManager;
        private CountriesManager _countriesManager;
        private StatesManager _statesManager;

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

            // Reset extra entry fields visibility.
            ShowExtraFileds(Properties.Settings.Default.ShowExtraFields);

            LoadOperatorInfo();

            InitializeDataBinding();
        }

        private void InitializeDataBinding()
        {
            // Log data
            _qsoLog = new QSOLog(Properties.Settings.Default.LogPath);

            qsoGrid.ItemsSource = _qsoLog.QSOs;

            // Type data
            _typeDataDb = new TypeDataDb();

            _bandsManager = new BandsManager(_typeDataDb);
            _modesManager = new ModesManager(_typeDataDb);
            _countriesManager = new CountriesManager(_typeDataDb);
            _statesManager = new StatesManager(_typeDataDb);

            _bandsManager.LoadAndUpdate();
            _modesManager.LoadAndUpdate();
            _countriesManager.LoadAndUpdate();
            _statesManager.LoadAndUpdate();

            bandTxt.ItemsSource = _bandsManager.Bands;
            modeTxt.ItemsSource = _modesManager.Modes;
            countryTxt.ItemsSource = _countriesManager.Countries;
            stateTxt.ItemsSource = _statesManager.States;
        }

        private void LoadOperatorInfo()
        {
            _operator.Operator = Properties.Settings.Default.Operator;
            _operator.MyName = Properties.Settings.Default.MyName;
            _operator.MyCountry = Properties.Settings.Default.MyCountry;
            _operator.MyState = Properties.Settings.Default.MyState;
            _operator.MyCounty = Properties.Settings.Default.MyCounty;
            _operator.MyCity = Properties.Settings.Default.MyCity;
            _operator.MyGridSquare = Properties.Settings.Default.MyGridSquare;
        }

        #endregion

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.WindowWidth = Width;
            Properties.Settings.Default.WindowHeigth = Height;
            Properties.Settings.Default.IsWindowMaximized = WindowState == WindowState.Maximized ? true : false;
            Properties.Settings.Default.ShowExtraFields = uiVisible;

            Properties.Settings.Default.LogPath = _qsoLog.Path;

            Properties.Settings.Default.Operator = _operator.Operator;
            Properties.Settings.Default.MyName= _operator.MyName;
            Properties.Settings.Default.MyCountry= _operator.MyCountry;
            Properties.Settings.Default.MyState = _operator.MyState;
            Properties.Settings.Default.MyCounty = _operator.MyCounty;
            Properties.Settings.Default.MyCity = _operator.MyCity;
            Properties.Settings.Default.MyGridSquare = _operator.MyGridSquare;

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
            SaveFileDialog saveNewLogDialog = new SaveFileDialog();

            saveNewLogDialog.FileName = Constants.NEW_LOG_FILE_NAME;
            saveNewLogDialog.Filter = Constants.LOG_FILE_EXTENSION;
            saveNewLogDialog.InitialDirectory = new FileInfo(_qsoLog.Path).DirectoryName;

            if (saveNewLogDialog.ShowDialog() == true)
                SaveNewLogFile(saveNewLogDialog.FileName);
        }

        private void OpenDBMenuClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openLogDialog = new OpenFileDialog();

            openLogDialog.Filter = Constants.LOG_FILE_EXTENSION;
            openLogDialog.InitialDirectory = new FileInfo(_qsoLog.Path).DirectoryName;

            if (openLogDialog.ShowDialog() == true)
                OpenLogFile(openLogDialog.FileName);
        }

        private void ImportADIFMenuClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog importADIFDialog = new OpenFileDialog();

            importADIFDialog.Filter = Constants.ADIF_FILE_EXTENSION;
            importADIFDialog.InitialDirectory = new FileInfo(_qsoLog.Path).DirectoryName;

            if (importADIFDialog.ShowDialog() == true)
                ImportADIFile(importADIFDialog.FileName);
        }

        private void ExportADIFMenuClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog exportADIFDialog = new SaveFileDialog();

            exportADIFDialog.FileName = Constants.NEW_ADIF_FILE_NAME;
            exportADIFDialog.Filter = Constants.ADIF_FILE_EXTENSION;
            exportADIFDialog.InitialDirectory = new FileInfo(_qsoLog.Path).DirectoryName;

            if (exportADIFDialog.ShowDialog() == true)
                ExportADIFile(exportADIFDialog.FileName);
        }

        private void OperatorInformationClick(object sender, RoutedEventArgs e)
        {
            OperatorInformation info = new OperatorInformation(_operator.Operator, _operator.MyName, _operator.MyCountry, _operator.MyState,
                                                                _operator.MyCounty, _operator.MyCity, _operator.MyGridSquare);


            if(info.ShowDialog() == true)
            {
                _operator.Operator= info.Callsign;
                _operator.MyName = info.Name;
                _operator.MyCountry = info.Country;
                _operator.MyState = info.State;
                _operator.MyCounty = info.County;
                _operator.MyCity = info.City;
                _operator.MyGridSquare = info.GridSquare;

                Properties.Settings.Default.Operator = _operator.Operator;
                Properties.Settings.Default.MyName = _operator.MyName;
                Properties.Settings.Default.MyCountry = _operator.MyCountry;
                Properties.Settings.Default.MyState = _operator.MyState;
                Properties.Settings.Default.MyCounty = _operator.MyCounty;
                Properties.Settings.Default.MyCity = _operator.MyCity;
                Properties.Settings.Default.MyGridSquare = _operator.MyGridSquare;

                Properties.Settings.Default.Save();
            }
        }

        private void AboutMenuClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Constants.ABOUT_MESSAGE, Constants.APPLICATION_NAME, MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
                ShowExtraFileds(false);
            }
            else
            {
                ShowExtraFileds(true);
            }
        }

        private void LogBtnClick(object sender, RoutedEventArgs e)
        {
            if(callsignTxt.Text == string.Empty)
            {
                MessageBox.Show("Please enter a callsign", "Enter Callsign", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

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
                DateTimeOff = GetDateTime(timeOffTxt.Text, dateOffTxt.Text),

                Operator = _operator.Operator,
                MyName = _operator.MyName,
                MyCountry = _operator.MyCountry,
                MyState = _operator.MyState,
                MyCounty = _operator.MyCounty,
                MyCity = _operator.MyCity,
                MyGridSquare = _operator.MyGridSquare
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

        private void countryChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CountryModel country = (CountryModel)countryTxt.SelectedItem;

            // If country is null, no country is selected so reset to -1.
            _statesManager.CurrentCountry = country == null ? StatesManager.NO_COUNTRY : country.Code;

            stateTxt.ItemsSource = _statesManager.States;
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

        private void ShowExtraFileds(bool visible)
        {
            uiVisible = visible;

            if (visible == false)
            {

                showBtn.Content = "More";

                moreContGroup.Visibility = Visibility.Collapsed;
                extraQSLGroup.Visibility = Visibility.Collapsed;
            }
            else
            {
                showBtn.Content = "Less";

                moreContGroup.Visibility = Visibility.Visible;
                extraQSLGroup.Visibility = Visibility.Visible;
            }
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

        private DateTime GetDateTime(string time, string date)
        {
            if (time == string.Empty && date == string.Empty)
                return new DateTime();

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
                return new DateTime();
            }

            return datetime;
        }

        private void SaveNewLogFile(string path)
        {
            if (File.Exists(path))
                File.WriteAllBytes(path, new byte[0]);

            _qsoLog.Close();
            _qsoLog.Dispose();

            _qsoLog = new QSOLog(path);
            qsoGrid.ItemsSource = _qsoLog.QSOs;

            Properties.Settings.Default.LogPath = path;

            Properties.Settings.Default.Save();
        }

        private void OpenLogFile(string path)
        {
            _qsoLog.Close();
            _qsoLog.Dispose();

            _qsoLog = new QSOLog(path);
            qsoGrid.ItemsSource = _qsoLog.QSOs;

            Properties.Settings.Default.LogPath = path;

            Properties.Settings.Default.Save();
        }

        private void ImportADIFile(string path)
        {
            ADIReader reader = new ADIReader(path);

            List<QSO> imported = reader.Read();

            // Record the number of records imported
            int num = 0;

            foreach(QSO x in imported)
            {
                _qsoLog.InsertQSO(x);
                num++;
            }

            reader.Close();

            qsoGrid.Items.Refresh();

            MessageBox.Show(string.Format("Records imported: {0}", num), "ADIF Import", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportADIFile(string path)
        {
            ADIWriter writer = new ADIWriter(path, false);
            ADIFHeader header = new ADIFHeader();

            // This should be counted by the writer.
            int num = _qsoLog.QSOs.Count;

            header.InitialComment = Constants.ADIF_HEADER_COMMENT;
            header.ProgramId = Constants.APPLICATION_NAME;
            header.TimeStamp = DateTime.Now;

            writer.SetHeader(header);

            writer.WriteHeader();

            writer.WriteQSOLinkedList(_qsoLog.QSOs);

            writer.Close();

            MessageBox.Show(string.Format("Records exported: {0}", num), "ADIF Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}
