///-----------------------------------------------------------------
///   Namespace:      GameCardLib2
///   Class:          Player
///   Description:    Contains data and information related to each and every Player
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCardLib2 {

    /// <summary>
    /// The player class contains information related to the player such as id, name, username, bank-balance and so on-
    /// </summary>
    public class Player {

        //Instances of classes
        private Hand hand;

        /// <summary>
        /// Constructor
        /// </summary>
        public Player() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">players id</param>
        /// <param name="name">players name</param>
        public Player(int position, int id, string username, int bank)
        {
            Position = position;
            Id = id;
            Username = username;
            Bank = bank;
            Bet = 0;
            MyTurn = false;
            Joined = false;
            hand = new Hand();
        }

        /// <summary>
        /// Player constructor
        /// </summary>
        /// <param name="id">players id</param>
        /// <param name="username">players username</param>
        /// <param name="password">players password</param>
        /// <param name="bank">players bank</param>
        public Player(int id, string username, string password, int bank)
        {
            Id = id;
            Username = username;
            Password = password;
            Bank = bank;
            Bet = 0;
            MyTurn = false;
            Joined = false;
            hand = new Hand();
        }

        //Properties
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Bank { get; set; }

        /// <summary>
        /// Adds a new card to the players hand
        /// </summary>
        /// <param name="card">The card to be added</param>
        public void AddCard(Card card)
        {
            hand.AddCard(card);
        }

        /// <summary>
        /// Clears/resets the players hand
        /// </summary>
        public void ClearHand()
        {
            hand.ClearHand();
        }

        /// <summary>
        /// Position property
        /// </summary>
        [NotMapped]
        public int Position { get; set; }

        /// <summary>
        /// Bet property
        /// </summary>
        [NotMapped]
        public int Bet { get; set; }

        /// <summary>
        /// Score property
        /// </summary>
        [NotMapped]
        public int Score {
            get { return hand.Score; }
        }

        /// <summary>
        /// MyTurn propery
        /// </summary>
        [NotMapped] //Doesnt need to be in the database-table
        public bool MyTurn { get; set; }

        /// <summary>
        /// Joined property
        /// </summary>
        [NotMapped] //Doesnt need to be in the database-table
        public bool Joined { get; set; }

        /// <summary>
        /// AmountOfCards property
        /// </summary>
        [NotMapped]
        public int AmountOfCards {
            get { return hand.NumberOfCards; }
        }
    }
}
