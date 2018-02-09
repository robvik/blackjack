///-----------------------------------------------------------------
///   Namespace:      DatabaseLib
///   Class:          Database
///   Description:    Handles interaction with the database
///   Author:         Robin Viktorsson                    
///   Date:           2017-10-15
///   Version         1.0
///-----------------------------------------------------------------

using System;
using GameCardLib2;
using System.Linq;
using System.Collections.Generic;

namespace DatabaseLib {

    /// <summary>
    /// The Database-class which communicates with the database and changes, inserts and deletes informaiton
    /// </summary>
    public class Database {
   
        /// <summary>
        /// Database constructor
        /// </summary>
        public Database() { }

        /// <summary>
        /// Adds credit to a users account
        /// </summary>
        /// <param name="username"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool AddCredit(string username, int amount)
        {
            bool success = false;
            try
            {

                using (var db = new DatabaseContext())
                {
                    var query = from b in db.Players
                                where b.Username == username
                                select b;

                    var record = query.FirstOrDefault();
                    if (record != null)
                    {
                        record.Bank += amount;
                        db.SaveChanges();
                        success = true;
                    }
                    else
                        Console.WriteLine("Why is record null?");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception [Database]: " + ex);
                return false;
            }

            return success;
        }

        /// <summary>
        /// Adds a new player to the database
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool registerPlayer(List<string> userInfo, out string message)
        {

            message = "Error while registering player. Please contact customer desk.";

            if (DoesPlayerExist(userInfo[8]))
            {
                message = "The username you entered already exists. Please try another one.";
                return false;
            }                

            bool success = false;
            try
            {

                using (var db = new DatabaseContext())
                {
                    User user = new User() { Firstname = userInfo[0], Lastname = userInfo[1], Streetname = userInfo[2], City = userInfo[3],
                                             State = userInfo[4], Zipcode = userInfo[5], Country = userInfo[6], Email = userInfo[7] };
                  
                    Player player = new Player() { Username = userInfo[8], Password = userInfo[9], Bank = 1000 };

                    db.Users.Add(user);
                    db.Players.Add(player);
                    db.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception [Database]: " + ex);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Updates the bankbalance of a player
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bankbalance"></param>
        /// <returns></returns>
        public bool UpdateBankBalance(int id, int bankbalance)
        {

            bool success = false;
            try
            {

                using (var db = new DatabaseContext())
                {
                    var query = from b in db.Players
                                where b.Id == id
                                select b;

                    var record = query.FirstOrDefault();
                    if(record != null)
                    {
                        record.Bank = bankbalance;
                        db.SaveChanges();
                        success = true;
                    } else
                        Console.WriteLine("Why is record null?");
                }

            } catch (Exception ex)
            {
                Console.WriteLine("Exception [Database]: " + ex);
                return false;
            }

            return success;
        }

        /// <summary>
        /// Returns a specific player
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Player GetPlayer(string username, string password)
        {
            Player player = null;

            try
            {
                using (var db = new DatabaseContext())
                {
                    var result = from b in db.Players
                                where b.Username.Equals(username)
                                && b.Password.Equals(password)
                                select b;

                    var record = result.FirstOrDefault();
                    if (record != null)
                        player = new Player(record.Id, record.Username, record.Password, record.Bank);
                    else
                        Console.WriteLine("WHY IS RECORD NULL?");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception [Database]: " + ex);
            }

            return player;
        }

        /// <summary>
        /// Checks if a player already exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool DoesPlayerExist(string username)
        {

            bool success = false;
            try
            {
                using (var db = new DatabaseContext())
                {
                    success = db.Players.Any(x => x.Username.Equals(username));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception [Database]: " + ex);
                success = false;
            }

            return success;
        }

    }
}
