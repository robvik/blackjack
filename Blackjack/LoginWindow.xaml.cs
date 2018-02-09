///-----------------------------------------------------------------
///   Namespace:      Blackjack
///   Class:          LoginWindow
///   Description:    Interaction logic for LoginWindow.xaml
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

using System.Windows;
using DatabaseLib;
using GameCardLib2;

namespace Blackjack {

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {

        //Instances of classes
        Database db = new Database();
        public static Player currentPlayer;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
            currentPlayer = null;
        }

        /// <summary>
        /// Tries to fetch a player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!db.DoesPlayerExist(txtBoxUsername.Text.ToString()))
            {
                MessageBox.Show("The username you entered does not exist. Please try again.", "Invalid account details!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            currentPlayer = db.GetPlayer(txtBoxUsername.Text.ToString(), txtBoxPassword.Password.ToString());
            if(currentPlayer != null)
            {
                MessageBox.Show("You were successfully logged on.", "Login succeed.", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
                MessageBox.Show("The login-process failed. Please contact customer desk.", "Login failed.", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Closes the LoginWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
