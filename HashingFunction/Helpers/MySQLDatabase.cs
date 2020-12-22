using MySql.Data.MySqlClient;
using System;

namespace HashingFunction
{
    public class MySQLDatabase
    {
        private const string connectionString = "";
        private readonly MySqlConnection connection;
        
        private MySqlCommand insertUserCommand;
        private MySqlCommand checkIfUserExistsCommand;
        private MySqlCommand readCommand;
        private MySqlCommand retrievePasswordCommand;
        private MySqlCommand retrieveSaltCommand;

        public MySQLDatabase()
        {
            //Initialize connection
            connection = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void CreateUser(string username, string password)
        {
            try
            {
                if (!CheckIfUsernameExists(username))
                {
                    //Username not taken

                    insertUserCommand = new MySqlCommand("INSERT INTO users(username, password, salt) VALUES(@username, @password, @salt)", connection);

                    //Make sure to hash and salt password
                    string[] hashedPasswordAndSalt = HashSalt.CreateHash(password);
                    

                    insertUserCommand.Parameters.AddWithValue("username", username);
                    insertUserCommand.Parameters.AddWithValue("password", hashedPasswordAndSalt[0]);
                    insertUserCommand.Parameters.AddWithValue("salt", hashedPasswordAndSalt[1]);

                    insertUserCommand.Prepare();
                    insertUserCommand.ExecuteNonQuery();
                }
                else
                {
                    //Username is taken
                    throw new Exception("Username is taken");
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Login(string username, string password)
        {
            try
            {
                if(CheckIfUsernameExists(username))
                {
                    string retrievedPassword = RetrievePassword(username);
                    string retrievedSalt = RetrieveSalt(username);

                    if(CheckIfPasswordMatch(password, retrievedPassword, retrievedSalt))
                    {
                        //If CheckIfPasswordMatch function returns true, then pwds matches. 
                        Console.WriteLine("Logged In");
                        return true;
                    }
                    else
                    {
                        //Else, the function returns false and both pwds are not matching
                        //Make sure not to let the user know whether the username or password is wrong
                        Console.WriteLine("Invalid credentials");
                    }
                }
                else
                {
                    //Username does not exist
                    //Make sure not to let the user know whether the username or password is wrong
                    Console.WriteLine("Invalid credentials");
                }

                return false;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private void Read()
        {
            readCommand = new MySqlCommand("SELECT * FROM users", connection);
            MySqlDataReader reader = readCommand.ExecuteReader();

            Console.WriteLine($"{reader.GetName(0)} {reader.GetName(1)}");
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetString(0)} {reader.GetString(1)}");
            }
        }

        public bool CheckIfUsernameExists(string username)
        {
            checkIfUserExistsCommand = new MySqlCommand("SELECT username FROM users WHERE username = @username", connection);

            checkIfUserExistsCommand.Parameters.AddWithValue("username", username);
            object res = checkIfUserExistsCommand.ExecuteScalar();

            if (res == null)
            {
                //Username is not taken
                return false;
            }
            else
            {
                //Username is taken
                return true;
            }
        }

        private bool CheckIfPasswordMatch(string expectedPassword, string actualPassword, string salt)
        {
            if (HashSalt.GetHash(expectedPassword, salt) == actualPassword)
            {
                //Hashified inputed pwd and salt gives the same result as actual pwd 
                //Thus, the inputed pwd is the same as the one in DB
                return true;
            }
            else
            {
                //Pwds don't match. 
                return false;
            }  
        }

        private string RetrievePassword(string username)
        {
            retrievePasswordCommand = new MySqlCommand("SELECT password FROM users WHERE username = @username", connection);
            retrievePasswordCommand.Parameters.AddWithValue("username", username);
            object res = retrievePasswordCommand.ExecuteScalar();

            return res.ToString();
        }

        private string RetrieveSalt(string username)
        {
            retrieveSaltCommand = new MySqlCommand("SELECT salt FROM users WHERE username = @username", connection);
            retrieveSaltCommand.Parameters.AddWithValue("username", username);
            object res = retrieveSaltCommand.ExecuteScalar();

            return res.ToString();
        }
    }
}
