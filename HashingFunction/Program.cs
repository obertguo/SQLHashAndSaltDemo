using System;

namespace HashingFunction
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateUser();
            AuthenticateUser();
        }

        private static void CreateUser()
        {
            MySQLDatabase database = new MySQLDatabase();
            database.OpenConnection();

            Console.WriteLine("Input username");
            string username = Console.ReadLine();

            Console.WriteLine("Input password");
            string password = HelperClass.InputPassword();

            database.CreateUser(username, password);

            database.CloseConnection();

            Console.ReadLine();
        }

        private static void AuthenticateUser()
        {
            MySQLDatabase database = new MySQLDatabase();
            database.OpenConnection();

            string username;
            string password;

            do
            {
                Console.WriteLine("Input username");
                username = Console.ReadLine();

                Console.WriteLine("Input password");
                password = HelperClass.InputPassword();

            } while (!database.Login(username, password));
            //While not logged in
                
            database.CloseConnection();

            Console.ReadLine();
        }
    }
}
