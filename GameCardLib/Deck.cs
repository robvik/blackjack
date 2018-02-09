///-----------------------------------------------------------------
///   Namespace:      GameCardLib2
///   Class:          Deck
///   Description:    Contains data and information about the current deck
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameCardLib2 {

    /// <summary>
    /// Deck-class contains information related to the class and necessary methods.
    /// </summary>
    public class Deck {

        //List and instances of classes
        private List<Card> cards;
        private static Random random = new Random();

        /// <summary>
        /// Constructor
        /// </summary>
        public Deck()
        {
            cards = new List<Card>();
            InitializeDeck();
        }

        /// <summary>
        /// Initializes the deck
        /// Adds unique cards to the deck.
        /// </summary>
        public void InitializeDeck()
        {
            cards.Clear(); //Clear the stack of cards

            int incrementedCardID = 1; //Should the ID start at 1?

            for (int i = 1; i < 5; i++) //Four types of cards: Ace, Spades, Heart and Diamond
            {
                for (int j = 1; j < 14; j++)
                {
                    Card card = new Card(); //Creating an new instance of a card
                    card.ID = incrementedCardID; //Setting the unique card id
                    card.Value = j; //Setting the value of the card

                    switch (i)
                    {
                        case 1:
                            card.Suite = "Spades";
                            break;
                        case 2:
                            card.Suite = "Clubs";
                            break;
                        case 3:
                            card.Suite = "Hearts";
                            break;
                        case 4:
                            card.Suite = "Diamonds";
                            break;
                        default:
                            Console.WriteLine("Default case. i is now: " + i);
                            break;
                    }

                    cards.Add(card); //Adding the card to the deck...
                    Console.WriteLine("ID: " + card.ID);
                    incrementedCardID++; //...and incrementing the value
                }

            }

        }

        /// <summary>
        /// Return a randomized card
        /// </summary>
        /// <returns>a card</returns>
        public Card FetchCard()
        {
            int randomCard = random.Next(cards.Count);

            Card card = new Card();
            card.ID = cards[randomCard].ID;
            card.Suite = cards[randomCard].Suite;
            card.Value = cards[randomCard].Value;

            cards.RemoveAt(randomCard);

            return card;
        }

        /// <summary>
        /// Shuffles the deck
        /// </summary>
        public void ShuffleDeck()
        {

            Random randomize = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = randomize.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }

        }

    }
}