using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HashingFunction;

namespace HashingFunctionTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void OpenConnection()
        {
            MySQLDatabase database = new MySQLDatabase();

            try
            {
                database.OpenConnection();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void CloseConnection()
        {
            MySQLDatabase database = new MySQLDatabase();
            try
            {
                database.OpenConnection();
                database.CloseConnection();
                return;
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CloseConnectionThrowsInvalidOperationException()
        {
            MySQLDatabase database = new MySQLDatabase();
            database.CloseConnection();
        }

        [TestMethod]
        public void CreateUserTest()
        {
            MySQLDatabase database = new MySQLDatabase();
            database.OpenConnection();

            Random r = new Random();
            string username = "";

            for (int i = 0; i < 10; i++)
            {
                username += r.Next(0, 10).ToString();
            }

            Assert.IsTrue(database.CheckIfUsernameExists(username));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateUserThrowsUsernameTakenException()
        {
            MySQLDatabase database = new MySQLDatabase();
            database.OpenConnection();
            string username = "bill";

            database.CreateUser(username, "1223455");
        }
    }
}
