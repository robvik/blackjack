///-----------------------------------------------------------------
///   Namespace:      GameCardLib2
///   Class:          User
///   Description:    Contains data and information about each and every User
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCardLib2 {

    /// <summary>
    /// User-class contains properties related to the users information such as id, firstname, lastname, streetname, email, and so on...
    /// </summary>
    public class User {

        /// <summary>
        /// Constructor
        /// </summary>
        public User() { }

        /// <summary>
        /// Properties
        /// </summary>
        [Key]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Streetname { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }

        [NotMapped] //This is stored in another table
        public string Username { get; set; }

        [NotMapped] //This is stored in another table
        public string Password { get; set; }

    }
}
