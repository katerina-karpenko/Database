using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using NUnit.Framework;
using System.Data;

namespace SQL
{
    [TestFixture]

    public class Test
    {
        SQLConnector sqlConnector;

        [OneTimeSetUp]
        public void StartTest()
        {
            sqlConnector = new SQLConnector("sa", "qwer1234");
            sqlConnector.ConnectToCatalog("HomeWorkDatabase");
        }

        /// <summary>
        /// Cоответсвие данных БД в строках и колонках таблицы Persons.
        /// </summary>
        [TestCase("Monrovia", 5, 4)]
        [TestCase("32", 5, 3)]
        [TestCase("Jones", 5, 2)]
        [TestCase("Deborah", 5, 1)]
        public void CheckSelectFromPersonsInDataBase(string expResult, int rowsIndex, int columnIndex)
        {
            DataTable result = sqlConnector.Execute
                ("SELECT * FROM Persons;");
            Assert.AreEqual(expResult, result.Rows[rowsIndex].
                ItemArray[columnIndex].
                ToString());
        }

        /// <summary>
        /// Cоответсвие ID покупателя с суммой заказа.
        /// </summary>
        [TestCase(1, "37.65")]
        [TestCase(2, "12.34")]
        [TestCase(15, "55.34")]
        [TestCase(29, "767.00")]
        [TestCase(30, "868.00")]

        public void CheckSelectFromOrdersInDataBase(int personId, string expResult)
        {
            DataTable result = sqlConnector.Execute
                ($"SELECT * FROM Orders WHERE Person_ID = {personId};");
            Assert.AreEqual(expResult, result.Rows[0].
                ItemArray[2].
                ToString());
        }

        /// <summary>
        /// INNER JOIN.
        /// </summary>
        [TestCase("Esther", 0, 1)]
        [TestCase("Jones", 0, 2)]
        [TestCase("24", 0, 3)]
        [TestCase("Copenhagen", 0, 4)]
        [TestCase("1", 0, 6)]
        [TestCase("37.65", 0, 7)]

        public void SelectInInnerJoin(string expResult, int rowsIndex, int columnIndex)
        {
            DataTable result = sqlConnector.Execute
                ($"SELECT * FROM Persons INNER JOIN Orders ON Persons.ID = Orders.Person_ID");
            Assert.AreEqual(expResult, result.Rows[rowsIndex].
                ItemArray[columnIndex].
                ToString());
        }


    }
}
