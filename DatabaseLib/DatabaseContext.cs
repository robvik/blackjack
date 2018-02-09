///-----------------------------------------------------------------
///   Namespace:      DatabaseLib
///   Class:          DatabaseContext
///   Description:    Handles interaction with the database
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

using System.Data.Entity;
using GameCardLib2;

namespace DatabaseLib
{
    /// <summary>
    /// Creates the database and its tables
    /// </summary>
    public class DatabaseContext : DbContext {

        /// <summary>
        /// DatabaseContext Constructor
        /// </summary>
        public DatabaseContext() : base() { }

        /// <summary>
        /// DbSet<Player> corresponds to your Players-table in the database
        /// </summary>
        public DbSet<Player> Players { get; set; }

        /// <summary>
        /// DbSet<User> corresponds to your User-table in the database
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}
