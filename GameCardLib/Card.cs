///-----------------------------------------------------------------
///   Namespace:      GameCardLib2
///   Class:          Card
///   Description:    Contains data and information about each and every instance of Card
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

namespace GameCardLib2 {

    /// <summary>
    /// Card Class contains information about a specific card, such as its value, id and suite. It also contains properties which return these kind of information.
    /// </summary>
    public class Card {

        //Variables
        private int id;
        private int val;
        private string suite;

        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public Card() { }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        /// <param name="id">card id</param>
        /// <param name="suite">card suite</param>
        /// <param name="value">card value</param>
        public Card(int id, string suite, int value)
        {
            this.id = id;
            this.suite = suite;
            this.val = value;
        }

        /// <summary>
        /// ID property
        /// Returns the id of the card
        /// </summary>
        public int ID
        {
            set { id = value; }
            get { return id; }
        }

        /// <summary>
        /// Suite property
        /// Returns the suite of the card
        /// </summary>
        public string Suite
        {
            set { suite = value; }
            get { return suite; }
        }

        /// <summary>
        /// Value property
        /// Returns the value of the card
        /// </summary>
        public int Value
        {
            set { val = value; }
            get { return val; }
        }

        /// <summary>
        /// Generates the filename of a specific card
        /// The cards filename.
        /// </summary>
        public string ImageName
        {
            get
            {
                //Get the first letter in lowercase
                char[] a = suite.ToCharArray();
                a[0] = char.ToLower(a[0]);
                //char x = char.ToLower(suite[0]);


                //Put all the pieces together
                string imgName = a[0].ToString() + val.ToString() + ".png";
                //string imgName = x.ToString() + val.ToString() + ".png";

                //And return the img-name
                return imgName;
            }
        }
    }
}