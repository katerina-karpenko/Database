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
        /// <summary>
        /// RIGHT JOIN.
        /// </summary>
        [TestCase("Holly", 28, 1)]
        [TestCase("Zimmerman", 28, 2)]
        [TestCase("18", 28, 3)]
        [TestCase("Madrid", 28, 4)]
        [TestCase("29", 28, 6)]
        [TestCase("767.00", 28, 7)]

        public void SelectInRigntJoin(string expResult, int rowsIndex, int columnIndex)
        {
            DataTable result = sqlConnector.Execute
                ($"SELECT * FROM Persons RIGHT JOIN Orders ON Persons.ID = Orders.Person_ID");
            Assert.AreEqual(expResult, result.Rows[rowsIndex].
                ItemArray[columnIndex].
                ToString());
        }
        /// <summary>
        /// LEFT JOIN. Покупатель внесенный в базу но не сделавший заказ.
        /// </summary>
        [TestCase("Esther", 30, 1)]
        [TestCase("Smith", 30, 2)]
        [TestCase("22", 30, 3)]
        [TestCase("London", 30, 4)]
        [TestCase("", 30, 6)]
        [TestCase("", 30, 7)]

        public void SelectInLeftJoin(string expResult, int rowsIndex, int columnIndex)
        {
            DataTable result = sqlConnector.Execute
                ($"SELECT * FROM Persons LEFT JOIN Orders ON Persons.ID = Orders.Person_ID");
            Assert.AreEqual(expResult, result.Rows[rowsIndex].
                ItemArray[columnIndex].
                ToString());
        }
        /// <summary>
        /// FULL JOIN.
        /// </summary>
        [TestCase("Benjamin", 10, 1)]
        [TestCase("Jackson", 10, 2)]
        [TestCase("67", 10, 3)]
        [TestCase("Baghdad", 10, 4)]
        [TestCase("11", 10, 6)]
        [TestCase("57.65", 10, 7)]

        public void SelectInFullJoin(string expResult, int rowsIndex, int columnIndex)
        {
            DataTable result = sqlConnector.Execute
                ($"SELECT * FROM Persons FULL JOIN Orders ON Persons.ID = Orders.Person_ID");
            Assert.AreEqual(expResult, result.Rows[rowsIndex].
                ItemArray[columnIndex].
                ToString());
        }

        /// <summary>
        /// GROUP. Количество людей в кадом городе.
        /// </summary>
        [TestCase("3", 0, 0)]
        [TestCase("Astana", 0, 1)]

       public void GroupCountPersonsInCity(string expResult, int rowsIndex, int columnIndex)
        {
            DataTable result = sqlConnector.Execute
                ("SELECT COUNT(ID) AS Count_Persons_In_City, City FROM Persons GROUP BY City;");
            Assert.AreEqual(expResult, result.Rows[rowsIndex].
                ItemArray[columnIndex].
                ToString());
        }

        /// <summary>
        /// SORT. Сортировка по количеству людей в городе, от большего к меньшему.
        /// </summary>
        [TestCase("3", 3, 0)]
        [TestCase("2", 4, 0)]

        public void SortCountPersonsInCity(string expResult, int rowsIndex, int columnIndex)
        {
            DataTable result = sqlConnector.Execute
                ("SELECT COUNT(ID) AS Count_Persons_In_City, City FROM Persons GROUP BY City ORDER BY Count_Persons_In_City DESC;");
            Assert.AreEqual(expResult, result.Rows[rowsIndex].
                ItemArray[columnIndex].
                ToString());
        }
    }
}
