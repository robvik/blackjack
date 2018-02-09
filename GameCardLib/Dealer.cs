///-----------------------------------------------------------------
///   Namespace:      GameCardLib2
///   Class:          Dealer
///   Description:    Contains data and information about the dealer and his/her hand and cards
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

namespace GameCardLib2 {

    /// <summary>
    /// Dealer-class contains information related to the dealer such as the dealers hand of cards and methods to clear the hand or add cards to the dealers hand.
    /// </summary>
    public class Dealer {

        //Variables
        private int score;
        private Hand hand;

        /// <summary>
        /// Constructor
        /// </summary>
        public Dealer()
        {
            score = 0;
            hand = new Hand();
        }

        /// <summary>
        /// Adds a card to the hand and increases the current score
        /// </summary>
        /// <param name="card">The card to be added</param>
        public void AddCard(Card card)
        {
            hand.AddCard(card);
            score += card.Value;
        }

        /// <summary>
        /// Clears the hand and resets the score
        /// </summary>
        public void ClearHand()
        {
            hand.ClearHand();
            score = 0;
        }

        /// <summary>
        /// Score property
        /// Returns the score of the hand
        /// </summary>
        public int Score
        {
            get { return hand.Score; }
        }

        /// <summary>
        /// AmountOfCards property
        /// Returns the amount of cards on players hand
        /// </summary>
        public int AmountOfCards
        {
            get { return hand.NumberOfCards; }
        }
    }
}