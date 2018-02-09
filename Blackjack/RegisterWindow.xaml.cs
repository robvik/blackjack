///-----------------------------------------------------------------
///   Namespace:      Blackjack
///   Class:          RegisterWindow
///   Description:    Interaction logic for RegisterWindow.xaml
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using DatabaseLib;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace Blackjack {

    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window {

        //Creating an instance of Database
        Database db = new Database();

        /// <summary>
        /// Constructor for RegisterWindow
        /// </summary>
        public RegisterWindow()
        {
            InitializeComponent();
            PopulateCountryComboBox();
        }

        /// <summary>
        /// The Click-event method for the Register-button. 
        /// The method will register a user if all required fields are filled in properly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox tb in FindVisualChildren<TextBox>(this))
            {
                if ((string.IsNullOrEmpty(tb.Text)) || (string.IsNullOrWhiteSpace(tb.Text)))
                {
                    MessageBox.Show("All fields need to be filled in.", "Fill in all fields.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }                
            }

            if (cboCountry.SelectedIndex == -1)
            {
                MessageBox.Show("All fields need to be filled in.", "Fill in all fields.", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!IsValidEmailAddress(txtEmail.Text.ToString()))
            {
                MessageBox.Show("This is an invalid email address. Please try again.", "Invalid email address.", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            List<string> userInfo = new List<string>();
            userInfo.Add(txtFirstname.Text.ToString());
            userInfo.Add(txtLastname.Text.ToString());
            userInfo.Add(txtStreetname.Text.ToString());
            userInfo.Add(txtCity.Text.ToString());
            userInfo.Add(txtZipcode.Text.ToString());
            userInfo.Add(txtState.Text.ToString());
            userInfo.Add(cboCountry.Text.ToString());
            userInfo.Add(txtEmail.Text.ToString());
            userInfo.Add(txtUsername.Text.ToString());
            userInfo.Add(txtPassword.Password.ToString());

            string message = "";
            if (db.registerPlayer(userInfo, out message))
            {
                MessageBox.Show("You succesfully registered a user.", "Registration succeeded.", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            } else
                MessageBox.Show(message, "Registration failed.", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        /// <summary>
        /// Controls if the email-adress is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(string email)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email);
        }

        /// <summary>
        /// Populates the country combobox with all available countries
        /// </summary>
        private void PopulateCountryComboBox()
        {
            RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
            List<string> countryNames = new List<string>();
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                countryNames.Add(country.DisplayName.ToString());
            }

            IEnumerable<string> nameAdded = countryNames.OrderBy(names => names).Distinct();

            foreach (string item in nameAdded)
            {
                cboCountry.Items.Add(item);

            }
        }

        /// <summary>
        /// Finds children of a specific type in the XAML-code (buttons, textboxes, labels, and so on).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
