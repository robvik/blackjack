///-----------------------------------------------------------------
///   Namespace:      Blackjack
///   Class:          BlackJackTable
///   Description:    Contains logic and data related to the Blackjack-table and everything it includes
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

using GameCardLib2;
using DatabaseLib;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Blackjack {

    /// <summary>
    /// BlackJackTable-class handles all data and logic related to the blackjack-table such as which players are currently joining the table, if a round is currently playing and whenever its the dealers turn among many other functions.
    /// </summary>
    public class BlackJackTable {

        //Constants
        private const int MAX_PLAYERS = 5;

        //Variables
        private int betLimit;
        private bool isRoundPlaying;

        //Instances of classes and lists
        private Dealer dealer;
        private Deck deck;
        private List<int> spots;
        private List<Player> players;
        private List<Player> playersToJoin;
        private List<Player> bustedPlayers;

        /// <summary>
        /// Constructor of the BlackJackTable-class
        /// </summary>
        public BlackJackTable()
        {
            betLimit = 50;
            isRoundPlaying = false;
            dealer = new Dealer();
            spots = new List<int>(new int[] { 1, 2, 3, 4, 5 });
            players = new List<Player>();
            bustedPlayers = new List<Player>();
            playersToJoin = new List<Player>();
        }

        /// <summary>
        /// Starts a new round by assigning variables and instances of classes
        /// </summary>
        public List<Tuple<Card, int[]>> StartRound()
        {

            //Initializing variables
            isRoundPlaying = true;
            deck = new Deck();

            //Lets store the cards and who it belongs to here
            List<Tuple<Card, int[]>> cardsToReturn = new List<Tuple<Card, int[]>>();

            //Sort all the players
            players = players.OrderBy(p => p.Position).ToList();

            //First deal one card to the dealer...
            Card card = new Card();
            card = deck.FetchCard();
            dealer.AddCard(card);
            cardsToReturn.Add(new Tuple<Card, int[]>(card, new int[] { -1, dealer.AmountOfCards }));

            //...then two cards to all players who has joined
            foreach (Player player in players)
            {
                //Lets bet
                player.Bank -= betLimit;
                player.Bet = betLimit;

                //First player card      
                card = deck.FetchCard();
                player.AddCard(card);
                cardsToReturn.Add(new Tuple<Card, int[]>(card, new int[] { player.Position, player.AmountOfCards }));

                //Second player card
                card = deck.FetchCard();
                player.AddCard(card);
                cardsToReturn.Add(new Tuple<Card, int[]>(card, new int[] { player.Position, player.AmountOfCards }));

                //Check if players score is over 21, then they are out. 
                if (player.Score > 21)
                {
                    Console.WriteLine(player.Username + " got busted with " + player.Score);
                    player.Joined = false; //Make sure Joined is set to false

                    //Remove the player from players-list
                    int index = players.FindIndex(x => x.Position == player.Position);
                    bustedPlayers.Add(players[index]); //should be enough to add the player
                }

            }

            //Remove the players who did not make it.
            players.RemoveAll(x => x.Joined == false);
            players.TrimExcess();

            //Lets decide who's gonna start
            if (players.Count > 0)
                players[0].MyTurn = true;

            if (players.Count == 0)
                ResetTable();
                
            return cardsToReturn;
        }

        /// <summary>
        /// Triggers when user hits the "Hit"-button. It adds another card to the players hand. 
        /// </summary>
        /// <returns></returns>
        public List<Tuple<Card, int[]>> Hit()
        {
            //List to store all cards to be returned
            List<Tuple<Card, int[]>> cardsToReturn = new List<Tuple<Card, int[]>>();

            //Iterate through all players
            int currentPlayer = 0;
            foreach (Player player in players)
            {
                if (player.MyTurn == true)
                    break;
                else
                    currentPlayer++;
            }

            //Generate and add card for the player
            Card playerCard = new Card();
            playerCard = deck.FetchCard();
            players[currentPlayer].AddCard(playerCard);
            cardsToReturn.Add(new Tuple<Card, int[]>(playerCard, new int[] { players[currentPlayer].Position, players[currentPlayer].AmountOfCards }));
            players[currentPlayer].MyTurn = false;

            //The player gets busted
            if (players[currentPlayer].Score > 21)
            {
                Console.WriteLine(players[currentPlayer].Username + " got busted with " + players[currentPlayer].Score);
                players[currentPlayer].Joined = false; //Make sure Joined is set to false

                bustedPlayers.Add(players[currentPlayer]);
                players.RemoveAt(currentPlayer);
                players.TrimExcess();
            }
            else
                Console.WriteLine(players[currentPlayer].Username + " now has score: " + players[currentPlayer].Score);

            //If current player is the last one
            if (currentPlayer == players.Count)
            {
                List<Tuple<Card, int[]>> dealerCards = new List<Tuple<Card, int[]>>();
                dealerCards = DealerHit();
                cardsToReturn.AddRange(dealerCards);
            }
            else
                players[currentPlayer].MyTurn = true; //Need to make sure its still the current players turn because it changes at beginning of method

            return cardsToReturn;
        }

        /// <summary>
        /// Triggers when user hits the "Stand"-button. It moves on to the next player or dealer if all players played their round.
        /// </summary>
        /// <returns></returns>
        public List<Tuple<Card, int[]>> Stand()
        {
            //List to store all dealers cards
            List<Tuple<Card, int[]>> dealerCards = new List<Tuple<Card, int[]>>();

            //Iterate through all players
            int currentPlayer = 0;
            foreach (Player player in players)
            {
                if (player.MyTurn == true)
                    break;
                else
                    currentPlayer++;
            }

            Console.WriteLine(players[currentPlayer].Username + " decided to Stand");
            players[currentPlayer].MyTurn = false;

            //Check if its dealers turn or the next player
            if ((currentPlayer + 1) == players.Count)
                dealerCards = DealerHit();
            else
                players[currentPlayer + 1].MyTurn = true;

            return dealerCards;
        }

        /// <summary>
        /// Hits the dealer with some cards to figure out whos winning.
        /// </summary>
        /// <returns></returns>
        public List<Tuple<Card, int[]>> DealerHit()
        {
            Console.WriteLine("Now its dealers turn");

            //Card and list to store all dealers cards
            Card card = new Card();
            List<Tuple<Card, int[]>> cardsToReturn = new List<Tuple<Card, int[]>>();

            //Variable is storing maxstore
            int maxScore = 0;

            //Calculating maxscore
            foreach (Player player in players)
            {
                if (player.Joined)
                    if (player.Score >= maxScore)
                        maxScore = player.Score;
            }

            //We got maxscore calculated. Now lets see what happens.
            for (int i = 0; i < 1; i++)
            {

                if ((dealer.Score < maxScore) && (dealer.Score < 17))
                {
                    //Fetch a new card and add it to the list to be returned and shown in GUI
                    card = deck.FetchCard();
                    dealer.AddCard(card);
                    cardsToReturn.Add(new Tuple<Card, int[]>(card, new int[] { -1, dealer.AmountOfCards }));

                    Console.WriteLine("Dealer score is now: " + dealer.Score);
                    i--; //Run the loop once more
                }
                else if (dealer.Score > 21) //Dealer loses
                {
                    Console.WriteLine("Dealer lost! Give all players whos still in the game their money!");
                    foreach (Player p in players)
                    {
                        if (p.Joined == true)
                            p.Bank += p.Bet + betLimit;
                    }

                }
                else if (dealer.Score >= 17)
                {
                    Console.WriteLine("Dealer must stop. Players who has more points than " + dealer.Score + " wins!");
                    foreach (Player p in players)
                    {
                        if (p.Joined == true)
                        {
                            if (p.Score < dealer.Score)
                                p.Bank -= p.Bet;
                            else if (p.Score > 21)
                                p.Bank -= p.Bet;
                            else if (p.Score > dealer.Score)
                                p.Bank += p.Bet + betLimit;
                            else if (p.Score == dealer.Score)
                                p.Bank += p.Bet;
                        }
                    }
                }
                else if (dealer.Score == maxScore) //Dealer goes even with one or more players
                {
                    Console.WriteLine("Even! Players with maxScore gets their money back! The rest loses their money.");
                    foreach (Player p in players)
                        if (p.Joined == true)
                            if (p.Score == maxScore)
                                p.Bank += p.Bet;
                            else
                            {
                                p.Bank -= p.Bet;
                                if (p.Bank < betLimit) //Does the player have to little money to join next run? Then hes/shes out.
                                    p.Joined = false;
                            }

                }
                else if ((dealer.Score > maxScore) && (dealer.Score <= 21)) //Dealer wins!
                {
                    Console.WriteLine("Dealer wins! All players lose their money!");
                    foreach (Player p in players)
                        if (p.Joined == true)
                        {
                            p.Bank -= p.Bet;
                            if (p.Bank < betLimit) //Does the player have to little money to join next run? Then hes/shes out.
                                p.Joined = false;
                        }

                }
                else //Did we miss something?
                    Console.WriteLine("A condition is missing in if-else [DealerHit]");
            }

            isRoundPlaying = false; //The round is finished
            return cardsToReturn;
        }

        /// <summary>
        /// Calls function from class Deck to shuffle the cards.
        /// </summary>
        public void ShuffleDeck()
        {
            deck.ShuffleDeck();
        }

        /// <summary>
        /// Resets the blackjacktable
        /// </summary>
        public void ResetTable()
        {

            isRoundPlaying = false;

            //Reset the dealer
            dealer.ClearHand();

            //Combine the lists and clear bustedPlayers
            players.AddRange(bustedPlayers);
            players.AddRange(playersToJoin);
            bustedPlayers.Clear();
            playersToJoin.Clear();

            //Lets re-sort the list so all players are in order
            players = players.OrderBy(p => p.Position).ToList(); //This is working because the list IS sorted.

            //Reset all players
            foreach (Player player in players)
            {
                player.ClearHand();
                player.Joined = true;
                player.MyTurn = false;
            }

        }

        /// <summary>
        /// Let a player join the table
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="position">The players position on the table</param>
        public void JoinRound(Player player, int position)
        {
            //If round is playing. Make sure he/she joins next round and not the current now
            if (IsRoundPlaying)
                player.Joined = false;
            else
                player.Joined = true;

            //Add and remove players from relevant lists
            playersToJoin.Add(player);
            spots.Remove(position);
        }

        /// <summary>
        /// Removes the player from the blackjack-table
        /// </summary>
        /// <param name="position">The players Position</param>
        /// <returns></returns>
        public List<Tuple<Card, int[]>> LeaveGame(int position)
        {
            //Lets make it local since this is the only time we need to use it
            Database db = new Database();

            //List to store dealers card if its the last player (last position) to leave the table
            List<Tuple<Card, int[]>> cardsToReturn = new List<Tuple<Card, int[]>>();

            //Make sure the money the player bet is removed while leaving. In this case it doesnt matter since the currency is virtual, but in other cases its important. 
            foreach (Player player in players)
            {
                if (player.Position == position)
                {
                    player.Bank -= player.Bet;
                    player.Joined = false;
                    player.MyTurn = false;
                    db.UpdateBankBalance(player.Id, player.Bank);
                    break;
                }
            }

            //Where was the player located in the list?
            int myIndex = players.FindIndex(p => p.Position == position);

            //Remove all 
            players.RemoveAll(x => x.Position == position);
            playersToJoin.RemoveAll(x => x.Position == position);
            bustedPlayers.RemoveAll(x => x.Position == position);

            //Add the free position to spots
            spots.Add(position);

            //This is a fix to make sure its the correct player after a player left. it also fixes the issue with showing/moving the rectangle if a player leaves.
            if ((myIndex + 1) > players.Count)
                cardsToReturn = DealerHit();
            else
            {
                for (int i = myIndex; i <= players.Count; ++i)
                {
                    if (isRoundPlaying && players[i].Joined == true)
                    {
                        players[i].MyTurn = true;
                        break;
                    }
                }
            }

            if (GetAmountOfPlayers() == 0)
                ResetTable();

            return cardsToReturn;
        }

        /// <summary>
        /// Returns a free position on the table, if any 
        /// </summary>
        /// <returns>Returns 0 if all seats are taken, otherwise the free position</returns>
        public int GetFreePosition()
        {
            if (spots.Count > 0) //Check if the list is empty
            {
                int freeSpot = 10;
                foreach (int a in spots)
                    if (a <= freeSpot)
                        freeSpot = a;

                return freeSpot;
            }
            else //Return 0 if the list is empty
                return 0;
        }

        /// <summary>
        /// Property value
        /// Returns true if maxplayers are reached, otherwise false
        /// </summary>
        public bool IsMaxPlayersReached
        {
            get { return ((players.Count + playersToJoin.Count + bustedPlayers.Count) == MAX_PLAYERS); }
        }

        /// <summary>
        /// Returns the amount of players on the table
        /// </summary>
        /// <returns>amount of players</returns>
        public int GetAmountOfPlayers()
        {
            return players.Count + playersToJoin.Count + bustedPlayers.Count;
        }

        /// <summary>
        /// Returns the list of all players
        /// </summary>
        public List<Player> getPlayers
        {
            get { return players; }
        }

        /// <summary>
        /// Returns the list of all busted players
        /// </summary>
        public List<Player> getBustedPlayers
        {
            get { return bustedPlayers; }
        }

        /// <summary>
        /// Returns the dealers-score
        /// </summary>
        public int DealerScore
        {
            get { return dealer.Score; }
        }

        /// <summary>
        /// Returns whetever the round is playing or not
        /// </summary>
        public bool IsRoundPlaying
        {
            set { isRoundPlaying = value; }
            get { return isRoundPlaying; }
        }

        /// <summary>
        /// Returns an instance of the current player
        /// </summary>
        public Player CurrentPlayer
        {
            get
            {
                foreach (Player player in players)
                    if (player.MyTurn == true)
                        return player;

                return null;
            }
        }

    }
}