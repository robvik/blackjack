///-----------------------------------------------------------------
///   Namespace:      Blackjack
///   Class:          MainWindow
///   Description:    Interaction logic for MainWindow.xaml
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------
///
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using GameCardLib2;
using DatabaseLib;

namespace Blackjack {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {


        //An Instance of class BlackJackTable
        BlackJackTable table = new BlackJackTable();

        /// <summary>
        /// Constructor for MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeGUI();
        }

        /// <summary>
        /// Initializing the GUI
        /// The method hides all buttons, labels and other components that should not show at startup.
        /// </summary>
        private void InitializeGUI()
        {

            //Hide all leave buttons
            btnP1Leave.Visibility = Visibility.Collapsed;
            btnP2Leave.Visibility = Visibility.Collapsed;
            btnP3Leave.Visibility = Visibility.Collapsed;
            btnP4Leave.Visibility = Visibility.Collapsed;
            btnP5Leave.Visibility = Visibility.Collapsed;

            //Hide all buttons except join
            btnStartGame.Visibility = Visibility.Collapsed;
            btnHit.Visibility = Visibility.Collapsed;
            btnStand.Visibility = Visibility.Collapsed;
            btnDeck.Visibility = Visibility.Collapsed;
            btnSplit.Visibility = Visibility.Collapsed;
            btnDouble.Visibility = Visibility.Collapsed;

            //Hide all the rectangles
            HideAllRectangles();

            //Reset all images on table
            ResetImagesOnTable();
        }

        /// <summary>
        /// Will attempt to join a player to the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            //Nothing will happen if table is full
            if (table.IsMaxPlayersReached)
                Console.WriteLine("The table is full. Please try again later.");
            else
            {

                //Open the loginform
                LoginWindow login = new LoginWindow();
                login.ShowDialog();

                if (LoginWindow.currentPlayer == null)
                    return;
                else if (LoginWindow.currentPlayer.Bank <= 0)
                {
                    MessageBox.Show("The username does not have any credit. Please add credit before joining.", "Insufficient amount of credit!", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                    
                //Lets genererate a player thats gonna join the game
                int position = table.GetFreePosition(); //Should never return 0 tho.
                Player player = new Player(position, LoginWindow.currentPlayer.Id, LoginWindow.currentPlayer.Username, LoginWindow.currentPlayer.Bank);
                table.JoinRound(player, position);

                //Change GUI-settings
                ShowPlayerName(position, LoginWindow.currentPlayer.Username);

                //Show the leave-button of the player who just joined
                ShowLeaveButton(position);

                //If there is atleast one player joined it should be possible to start the game
                if (!table.IsRoundPlaying)
                    ShowStartButton();

                //Hide the join-button if table reached its max-amount of players
                if (table.IsMaxPlayersReached)
                    btnJoin.Visibility = Visibility.Collapsed;  //Hide the join-button and show the startbutton
            }
        }

        /// <summary>
        /// Will reset the table and start the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            //A list to store the cards to visualize
            List<Tuple<Card, int[]>> cardsToVisualize = new List<Tuple<Card, int[]>>();

            //Reset the table if it might not be the first round ever
            table.ResetTable();
            ResetImagesOnTable();

            //Hide the join button if a round has started
            if (table.IsMaxPlayersReached)
                HideJoinButton();

            //Start the round
            cardsToVisualize = table.StartRound();

            //Call the method which visualizes these cards
            VisualizeCards(cardsToVisualize);

            //Hide the start-button
            HideStartButton();

            //Show Hit/Stand-button   
            btnHit.Visibility = Visibility.Visible;
            btnStand.Visibility = Visibility.Visible;
            btnDeck.Visibility = Visibility.Visible;

            //Updates scores
            UpdateScores(table.getPlayers);

            //Lets check whose turn it is
            WhoseTurnIsIt();

            if(table.IsRoundPlaying == false)
            {
                btnHit.Visibility = Visibility.Collapsed;
                btnStand.Visibility = Visibility.Collapsed;
                btnDeck.Visibility = Visibility.Collapsed;
                btnStartGame.Visibility = Visibility.Visible;
                btnJoin.Visibility = Visibility.Visible;
            }
               
        }

        /// <summary>
        /// The current player will fetch another card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            //A list to store the cards to visualize
            List<Tuple<Card, int[]>> cardsToVisualize = new List<Tuple<Card, int[]>>();

            //Call the method which adds a card to the players hand
            cardsToVisualize = table.Hit();

            //Call the method which visualizes these cards
            VisualizeCards(cardsToVisualize);

            //Update all the scores on the table
            UpdateScores(table.getPlayers);

            //Lets check if the round is finished. If so hide/show proper buttons
            IsRoundFinished();
        }

        /// <summary>
        /// The current player will stand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStand_Click(object sender, RoutedEventArgs e)
        {

            //A list to store the cards to visualize
            List<Tuple<Card, int[]>> cardsToVisualize = new List<Tuple<Card, int[]>>();

            //Call the method which makes the player stand
            cardsToVisualize = table.Stand();

            //Call the method which visualizes these cards
            VisualizeCards(cardsToVisualize);

            //Update all the scores on the table
            UpdateScores(table.getPlayers);

            //Lets check if the round is finished. If so hide/show proper buttons
            IsRoundFinished();
        }

        /// <summary>
        /// Shuffles the deck
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShuffleDeck_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("You want to shuffle the deck?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                table.ShuffleDeck();
                MessageBox.Show("The deck was shuffled!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        /// <summary>
        /// Will be implemented later.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Will be implemented later.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDouble_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Visualizes all cards on the table.
        /// </summary>
        /// <param name="cards">The cards to visualize</param>
        private void VisualizeCards(List<Tuple<Card, int[]>> cards)
        {
            //Theres no point in trying visualizing cards that does not exist.
            if (cards.Any())
            {
                //Variables to store information
                string imgName;
                string visualizePos;
                BitmapImage img;
                Image imageToFind;

                //Lets iterate through the list in case there are more cards to be dealt
                foreach (Tuple<Card, int[]> item in cards)
                {
                    imgName = item.Item1.ImageName;

                    if (item.Item2[0] == -1)
                        visualizePos = "imgDealer_Card" + item.Item2[1].ToString();
                    else
                        visualizePos = "imgPlayer" + item.Item2[0].ToString() + "_Card" + item.Item2[1].ToString();

                    //Generating the image
                    img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri("pack://application:,,,/Images/Cards/" + imgName);
                    img.EndInit();

                    //Lets decide where to place the image
                    imageToFind = this.FindName(visualizePos) as Image;
                        imageToFind.Source = img;
                }
            }
            else
                Console.WriteLine("The list-tuple has no content!");
        }

        /// <summary>
        /// Clears all the image-sources
        /// </summary>
        private void ResetImagesOnTable()
        {
            foreach (Image img in grid.Children.OfType<Image>())
                img.Source = null;
        }

        /// <summary>
        /// Adds the players name to the position he/she just joined
        /// Removes: Position x (free)
        /// </summary>
        /// <param name="position">An integer containing the position which label should be changed.</param>
        private void ShowPlayerName(int position, string playername)
        {
            switch (position)
            {
                case 1:
                    lblPlayer1.Content = playername;
                    break;
                case 2:
                    lblPlayer2.Content = playername;
                    break;
                case 3:
                    lblPlayer3.Content = playername;
                    break;
                case 4:
                    lblPlayer4.Content = playername;
                    break;
                case 5:
                    lblPlayer5.Content = playername;
                    break;
                default:
                    Console.WriteLine("ShowLeaveButton triggers default");
                    break;
            }
        }

        /// <summary>
        /// Makes the leave-button of a specific position visible
        /// </summary>
        /// <param name="position">An integer containing the position which should have an visible leave-button</param>
        private void ShowLeaveButton(int position)
        {
            switch (position)
            {
                case 1:
                    btnP1Leave.Visibility = Visibility.Visible;
                    break;
                case 2:
                    btnP2Leave.Visibility = Visibility.Visible;
                    break;
                case 3:
                    btnP3Leave.Visibility = Visibility.Visible;
                    break;
                case 4:
                    btnP4Leave.Visibility = Visibility.Visible;
                    break;
                case 5:
                    btnP5Leave.Visibility = Visibility.Visible;
                    break;
                default:
                    Console.WriteLine("ShowLeaveButton() triggers default");
                    break;
            }
        }

        /// <summary>
        /// Fetched the current player and shows the rectangle around his/her cards
        /// </summary>
        public void WhoseTurnIsIt()
        {

            //First lets hide all of them
            HideAllRectangles();

            int currentPlayer = -1;
            if (table.CurrentPlayer != null)
                currentPlayer = table.CurrentPlayer.Position;

            switch (currentPlayer)
            {
                case 1:
                    RectPlayer1.Visibility = Visibility.Visible;
                    break;
                case 2:
                    RectPlayer2.Visibility = Visibility.Visible;
                    break;
                case 3:
                    RectPlayer3.Visibility = Visibility.Visible;
                    break;
                case 4:
                    RectPlayer4.Visibility = Visibility.Visible;
                    break;
                case 5:
                    RectPlayer5.Visibility = Visibility.Visible;
                    break;
                default:
                    Console.WriteLine("Noones turn? Maybe a new round?");
                    break;
            }
        }

        /// <summary>
        /// Updates the dealer's and players labels on the table to make sure the information shown is up-to-date
        /// </summary>
        /// <param name="players">A list containing all players</param>
        public void UpdateScores(List<Player> players) //This method must be fixed. 
        {
            //Lets update the dealers label
            lblDealer.Content = "Dealer (Count: " + table.DealerScore + ")";

            //Updating all the busted players
            foreach (Player player in table.getBustedPlayers)
            {
                switch (player.Position)
                {
                    case 1:
                        lblPlayer1.Content = player.Username + " (Busted)";
                        lblPlayer1Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer1Bet.Content = "";
                        lblPlayer1Count.Content = "Score: " + player.Score;
                        break;
                    case 2:
                        lblPlayer2.Content = player.Username + " (Busted)";
                        lblPlayer2Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer2Bet.Content = "";
                        lblPlayer2Count.Content = "Score: " + player.Score;
                        break;
                    case 3:
                        lblPlayer3.Content = player.Username + " (Busted)";
                        lblPlayer3Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer3Bet.Content = "";
                        lblPlayer3Count.Content = "Score: " + player.Score;
                        break;
                    case 4:
                        lblPlayer4.Content = player.Username + " (Busted)";
                        lblPlayer4Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer4Bet.Content = "";
                        lblPlayer4Count.Content = "Score: " + player.Score;
                        break;
                    case 5:
                        lblPlayer5.Content = player.Username + " (Busted)";
                        lblPlayer5Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer5Bet.Content = "";
                        lblPlayer5Count.Content = "Score: " + player.Score;
                        break;
                    default:
                        Console.WriteLine("This should now have shown!");
                        break;
                }
            }

            //Updating all the players which are still in the game
            foreach (Player player in players)
            {
                switch (player.Position)
                {
                    case 1:
                        lblPlayer1.Content = player.Username;
                        lblPlayer1Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer1Bet.Content = "Bet: " + player.Bet + " $";
                        lblPlayer1Count.Content = "Score: " + player.Score;
                        break;
                    case 2:
                        lblPlayer2.Content = player.Username;
                        lblPlayer2Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer2Bet.Content = "Bet: " + player.Bet + " $";
                        lblPlayer2Count.Content = "Score: " + player.Score;
                        break;
                    case 3:
                        lblPlayer3.Content = player.Username;
                        lblPlayer3Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer3Bet.Content = "Bet: " + player.Bet + " $";
                        lblPlayer3Count.Content = "Score: " + player.Score;
                        break;
                    case 4:
                        lblPlayer4.Content = player.Username;
                        lblPlayer4Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer4Bet.Content = "Bet: " + player.Bet + " $";
                        lblPlayer4Count.Content = "Score: " + player.Score;
                        break;
                    case 5:
                        lblPlayer5.Content = player.Username;
                        lblPlayer5Pot.Content = "Bank: " + player.Bank + " $";
                        lblPlayer5Bet.Content = "Bet: " + player.Bet + " $";
                        lblPlayer5Count.Content = "Score: " + player.Score;
                        break;
                    default:
                        Console.WriteLine("This should now have shown!");
                        break;
                }
            }

            //Next lets see whose turn it is
            WhoseTurnIsIt();
        }

        /// <summary>
        /// Button allows Player 1 to leave the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnP1Leave_Click(object sender, RoutedEventArgs e)
        {

            //Hide the leave-button
            btnP1Leave.Visibility = Visibility.Collapsed;

            //What happens after a player leaves the round?
            PlayerLeavesRound(1);

            //Remove the player from the table
            lblPlayer1.Content = "Position 1 (free)";
            lblPlayer1Pot.Content = "";
            lblPlayer1Bet.Content = "";
            lblPlayer1Count.Content = "";
        }

        /// <summary>
        /// Button allows Player 3 to leave the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnP2Leave_Click(object sender, RoutedEventArgs e)
        {

            //Hide the leave-button
            btnP2Leave.Visibility = Visibility.Collapsed;

            //What happens after a player leaves the round?
            PlayerLeavesRound(2);

            //Remove the player from the table
            lblPlayer2.Content = "Position 2 (free)";
            lblPlayer2Pot.Content = "";
            lblPlayer2Bet.Content = "";
            lblPlayer2Count.Content = "";
        }

        /// <summary>
        /// Button allows Player 3 to leave the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnP3Leave_Click(object sender, RoutedEventArgs e)
        {

            //Hide the leave-button
            btnP3Leave.Visibility = Visibility.Collapsed;

            //What happens after a player leaves the round?
            PlayerLeavesRound(3);

            //Remove the player from the table
            lblPlayer3.Content = "Position 3 (free)";
            lblPlayer3Pot.Content = "";
            lblPlayer3Bet.Content = "";
            lblPlayer3Count.Content = "";
        }

        /// <summary>
        /// Button allows Player 4 to leave the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnP4Leave_Click(object sender, RoutedEventArgs e)
        {

            //Hide the leave-button
            btnP4Leave.Visibility = Visibility.Collapsed;

            //What happens after a player leaves the round?
            PlayerLeavesRound(4);

            //Remove the player from the table
            lblPlayer4.Content = "Position 4 (free)";
            lblPlayer4Pot.Content = "";
            lblPlayer4Bet.Content = "";
            lblPlayer4Count.Content = "";
        }

        /// <summary>
        /// Button allows Player 5 to leave the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnP5Leave_Click(object sender, RoutedEventArgs e)
        {

            //Hide the leave-button
            btnP5Leave.Visibility = Visibility.Collapsed;

            //What happens after a player leaves the round?
            PlayerLeavesRound(5);

            //Remove the player from the table
            lblPlayer5.Content = "Position 5 (free)";
            lblPlayer5Pot.Content = "";
            lblPlayer5Bet.Content = "";
            lblPlayer5Count.Content = "";
        }

        /// <summary>
        /// Remove a player from the table
        /// </summary>
        /// <param name="id"></param>
        private void PlayerLeavesRound(int position)
        {
            List<Tuple<Card, int[]>> cardsToVisualize = new List<Tuple<Card, int[]>>();

            //Show the join button for other players that might want to join
            ShowJoinButton();

            //Remove the player from the table
            cardsToVisualize = table.LeaveGame(position);

            Console.WriteLine("Amount of players: " + table.GetAmountOfPlayers());

            //Hide the start-button if theres no players left
            if (table.GetAmountOfPlayers() == 0)
                HideStartButton();

            if (table.GetAmountOfPlayers() == 0 && !table.IsRoundPlaying)
            {
                HideStartButton();
                HideHitButton();
                HideStandButton();
                btnDeck.Visibility = Visibility.Collapsed;
            }

            //Visualize the cards if theres any to visualize
            if (cardsToVisualize.Any())
                VisualizeCards(cardsToVisualize);

            //Update all the scores on the table
            UpdateScores(table.getPlayers);

            //Check whose next
            WhoseTurnIsIt();

            //Is round finished?
            IsRoundFinished();
        }

        /// <summary>
        /// Controls if round if finished. If so it resets the GUI
        /// </summary>
        private void IsRoundFinished()
        {

            if (!table.IsRoundPlaying) //The round IS finished
            {
                table.ResetTable();
                ShowStartButton();
                HideHitButton();
                HideStandButton();
                btnDeck.Visibility = Visibility.Collapsed;
            }
                
        }

        /// <summary>
        /// Hides all rectangles
        /// </summary>
        private void HideAllRectangles()
        {
            RectDealer.Visibility = Visibility.Collapsed;
            RectPlayer1.Visibility = Visibility.Collapsed;
            RectPlayer2.Visibility = Visibility.Collapsed;
            RectPlayer3.Visibility = Visibility.Collapsed;
            RectPlayer4.Visibility = Visibility.Collapsed;
            RectPlayer5.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Make the join-button visible
        /// </summary>
        private void ShowJoinButton()
        {
            btnJoin.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Make the start-button visible
        /// </summary>
        private void ShowStartButton()
        {
            btnStartGame.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hide the hit-button
        /// </summary>
        private void HideHitButton()
        {
            btnHit.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Hide the stand-button
        /// </summary>
        private void HideStandButton()
        {
            btnStand.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Hide the start-button
        /// </summary>
        private void HideStartButton()
        {
            btnStartGame.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Hide the join-button
        /// </summary>
        private void HideJoinButton()
        {
            btnJoin.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Creates an instance of, and opens a new RegisterWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            //Open the loginform
            RegisterWindow register = new RegisterWindow();
            register.ShowDialog();
        }

        /// <summary>
        /// Creates an instance of, and opens a new CreditWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCredit_Click(object sender, RoutedEventArgs e)
        {
            CreditWindow credit = new CreditWindow();
            credit.ShowDialog();
        }

        /// <summary>
        /// Event is running whenever user closes the application
        /// This method makes sure to save the current bankbalance so he wont "save" lost money
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            //Lets make this one local since we will only use it here.
            Database db = new Database(); 

            //Iterate through all players (busted and non-busted)
            foreach (Player player in table.getPlayers.Concat(table.getBustedPlayers))
            {
                db.UpdateBankBalance(player.Id, player.Bank);
            }
        }

    }
}