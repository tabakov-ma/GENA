using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLWork;

namespace Test_Console
{
   class Program
   {
      delegate void MessageHandler(string message);
      static void Main(string[] args)
      {
         
            MessageHandler handler = delegate
            {
               Console.WriteLine("анонимный метод");
            };
            handler("hello world!");    // анонимный метод

            Console.Read();
         
         //Query query = new Query("Server=.\\SQLEXPRESS;Database=test;Trusted_Connection=True;");
         //DataTable dt = new DataTable();
         //query.GetDataTable("TestCreateTable", ref dt);
         ////string connectionString = "Server=.\\SQLEXPRESS;Database=test;Trusted_Connection=True;";
         ////string sql = "SELECT * FROM TestCreateTable";
         ////using (SqlConnection connection = new SqlConnection(connectionString))
         ////{
         ////   // Создаем объект DataAdapter
         ////   SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
         ////   // Создаем объект Dataset
         ////   DataSet ds = new DataSet();
         ////   // Заполняем Dataset
         ////   adapter.Fill(ds);


         ////   DataTable dt = ds.Tables[0];
         //   // добавим новую строку
         //   DataRow newRow = dt.NewRow();
         //   newRow["FirsName"] = "MAT";
         //   newRow["LastName"] = "MAT13";
         //   dt.Rows.Add(newRow);

         //   // Изменим значение в столбце Age для первой строки
         //   dt.Rows[0]["LastName"] = "MAT12";
         //   query.SetDataTable();
         //   // создаем объект SqlCommandBuilder
         //   //SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
         //   //adapter.Update(ds);
         //   //// альтернативный способ - обновление только одной таблицы
         //   ////adapter.Update(dt);
         //   //// заново получаем данные из бд
         //   //// очищаем полностью DataSet
         //   //ds.Clear();
         //   //// перезагружаем данные
         //   //adapter.Fill(ds);

         //// Отображаем данные
         //// перебор всех таблиц
         //foreach (DataColumn column in dt.Columns)
         //      Console.Write($"{column.ColumnName}\t");
         //   Console.WriteLine();
         //   // перебор всех строк таблицы
         //   foreach (DataRow row in dt.Rows)
         //   {
         //      // получаем все ячейки строки
         //      var cells = row.ItemArray;
         //      foreach (object cell in cells)
         //         Console.Write($"{cell}\t");
         //      Console.WriteLine();
         //   }
         ////}
         //Console.Read();
      }
   }
}
