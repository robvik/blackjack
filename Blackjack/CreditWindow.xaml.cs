///-----------------------------------------------------------------
///   Namespace:      Blackjack
///   Class:          CreditWindow
///   Description:    Interaction logic for CreditWindow.xaml
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------
///
using System;
using System.Windows;
using DatabaseLib;

namespace Blackjack {

    /// <summary>
    /// Interaction logic for CreditWindow.xaml
    /// </summary>
    public partial class CreditWindow : Window {

        //An instance of Database
        Database db = new Database();

        /// <summary>
        /// Constructor
        /// </summary>
        public CreditWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds credit to a users account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCredit_Click(object sender, RoutedEventArgs e)
        {
            //Lets only check username for now since we're not actually gonna add money from a real card.
            if (!db.DoesPlayerExist(txtUsername.Text.ToString()))
            {
                MessageBox.Show("The username you entered does not exist. Please try again.", "Invalid account details!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            } else
            {
                if(db.AddCredit(txtUsername.Text.ToString(), Int32.Parse(cboCreditToAdd.Text)))
                {
                    MessageBox.Show("Credit was succesfully added to the account", "Credit was succesfully added!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                    MessageBox.Show("Credit could not be added to the account. Contact customer service.", "Credit was not added to the account.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
                
        }

        /// <summary>
        /// Closes the CreditWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
