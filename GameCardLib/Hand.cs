///-----------------------------------------------------------------
///   Namespace:      GameCardLib2
///   Class:          Hand
///   Description:    Contains data and information about each and every instances of Hand
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

using System.Collections.Generic;

namespace GameCardLib2 {

    /// <summary>
    /// Hand-class contains information related to a specific hand. Example an collection of cards, methods to modify the hand and properties which returns information about the hand.
    /// </summary>
    public class Hand {

        //List
        private List<Card> cards;

        /// <summary>
        /// Constructor
        /// </summary>
        public Hand()
        {
            cards = new List<Card>();
        }

        /// <summary>
        /// Adds a new card to the hand
        /// </summary>
        /// <param name="card">the card to be added</param>
        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        /// <summary>
        /// Clears/resets the hand
        /// </summary>
        public void ClearHand()
        {
            cards.Clear();
        }

        /// <summary>
        /// Number of cards property
        /// Returns the amount of cards on a hand
        /// </summary>
        public int NumberOfCards
        {
            get { return cards.Count; }
        }

        /// <summary>
        /// Score property
        /// Calculates and return the value of all cards
        /// </summary>
        public int Score
        {
            get
            {
                int totalValue = 0;
                foreach (Card a in cards)
                    totalValue += a.Value;

                return totalValue;
            }
        }

    }
}