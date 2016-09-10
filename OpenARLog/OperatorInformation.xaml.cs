/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
 *
 * Author: kcotugno
 * Date: 4/27/2016
 */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OpenARLog
{
    /// <summary>
    /// Interaction logic for OperatorInformation.xaml
    /// </summary>
    public partial class OperatorInformation : Window
    {
        public string Callsign { get { return callsignTxt.Text; } set { callsignTxt.Text = value; } }
        
        // Name here hides the FrameworkElement.Name. Decide to change the property name or supress the warning.
        new public string Name { get { return nameTxt.Text; } set { nameTxt.Text = value; } }
        public string Country { get { return countryTxt.Text; } set { countryTxt.Text = value; } }
        public string State { get { return stateTxt.Text; } set { stateTxt.Text = value; } }
        public string County { get { return countryTxt.Text; } set { countyTxt.Text = value; } }
        public string City { get { return cityTxt.Text; } set { cityTxt.Text = value; } }
        public string GridSquare { get { return gridsquareTxt.Text; } set { gridsquareTxt.Text = value; } }

        public OperatorInformation()
        {
            InitializeComponent();

            callsignTxt.Focus();
        }

        public OperatorInformation(string callsign, string name, string country, string state, string county, string city, string gridsquare)
        {
            InitializeComponent();

            callsignTxt.Focus();

            Callsign = callsign;
            Name = name;
            Country = country;
            State = state;
            County = county;
            City = city;
            GridSquare = gridsquare;
        }

        public void returnBtnClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            this.DialogResult = button == okBtn ? true : false;
        }

        private void InfoKeyDown(object sender, RoutedEventArgs e)
        {
            KeyEventArgs key = (KeyEventArgs)e;
            if (key.Key == Key.Enter)
                returnBtnClick(okBtn, null);

            if (key.Key == Key.Escape)
                returnBtnClick(cancelBtn, null);
        }
    }
}
