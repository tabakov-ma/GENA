
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using StringWork;

namespace SQLWork
{
    
    /// <summary>
    /// Формирование заросов в SQL Server
    /// </summary>
   public class Query
    {
      /// <summary>
      /// Строка подключения
      /// </summary>
      private string StrConnect = "";
      /// <summary>
      /// Объект для подключения
      /// </summary>
      private SqlConnection Connection;
      private DataSet ds = new DataSet();
      private SqlDataAdapter adapter;

      /// <summary>
      /// Индикатор успешного последнего подключения ... 
      /// </summary>
      public int TestConOk = 0;
      /// <summary>
      /// Конструктор
      /// </summary>
      /// <param name="StrCon"> Строка подключения </param>
      public  Query(string StrCon)
      {
         StrConnect = StrCon;
      }

      /// <summary>
      /// Проверка связи с сервером
      /// </summary>
      public async void TestCon()
      {
      // Создание подключения
         SqlConnection connection = new SqlConnection(StrConnect);
         try
         {
            // Открываем подключение
            await connection.OpenAsync();
            TestConOk = 1;

         }
         catch (SqlException ex)
         {
            TestConOk = 2;
            MessageBox.Show(ex.Message);
         }
         finally
         {
               // если подключение открыто
            if (connection.State == ConnectionState.Open)
               {
                  // закрываем подключение
                  connection.Close();
               }
         }
      }
      /// <summary>
      /// Подключение
      /// </summary>
      /// <returns></returns>
      private string Open()
      {
         Connection = new SqlConnection(StrConnect);
         try
         {
               Connection.Open();
               return "";
         }
         catch(Exception ex)
         {
               return ex.Message;
         }
      }
      /// <summary>
      /// Отключение
      /// </summary>
      /// <returns></returns>
      public string Close()
      {
         try
         {
               Connection.Close();
               return "";
         }
         catch (Exception ex)
         {
               return ex.Message;
         }
      }

      /// <summary>
      /// Создание БД
      /// </summary>
      /// <param name="name"> Имя базы данных </param>
      /// <param name="size"> Размер базы данных </param>
      /// <param name="patch"> Путь к файлам базы данных </param>
      /// <param name="selectToNewDB"> Заменить в строке подключения имя базы на новое </param>
      /// <returns></returns>
      public string CreateDB(string name, int size, string patch, bool selectToNewDB = false)
      {
            
         try
         {
            string str = "CREATE DATABASE " + name + " ON PRIMARY " +
                        "(NAME = " + name + ", " +
                        "FILENAME = '" + patch + "\\" + name + ".mdf', " +
                        "SIZE = 10MB, MAXSIZE = " + size + "MB, FILEGROWTH = 10%)" +
                        "LOG ON (NAME = " + name + "_log, " +
                        "FILENAME = '" + patch + "\\" + name + ".ldf', " +
                        "SIZE = 1MB, " +
                        "MAXSIZE = 5MB, " +
                        "FILEGROWTH = 10%)";

            Open();
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            Close();
            if(selectToNewDB) 
            {
               StrConToEntity Entity = new StrConToEntity(StrConnect);
               Entity.Initial_Catalog = name;
               StrConnect = Entity.ToString();
            }
            return "";
         }
         catch (Exception ex)
         { Close(); return ex.Message; }    
      }

      /// <summary>
      /// Удаление БД
      /// </summary>
      /// <param name="name"> Имя базы данных </param>
      /// <returns></returns>
      public string DeleteDB(string name)
      {

         try
         {
            string str = "DROP DATABASE " + name;

            Open();
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            Close();
            return "";
         }
         catch (Exception ex)
         { Close(); return ex.Message; }
      }

      /// <summary>
      /// Получение списка баз данных включённых в ядро SQL Server
      /// </summary>
      /// <param name="lstr"> Возвращаемый список баз данных </param>
      /// <returns></returns>
      public  string GetNamesDB(ref List<String> lstr)
      {
         List<String> result = new List<string>();
         try
         {
            //Список баз данных
            string query = "SELECT database_id AS[ID]," +
                           "name AS[Название]," +
                           "CONVERT(CHAR(10), create_date, 104) AS[Дата создания]," +
                           "state_desc AS[Статус]," +
                           "compatibility_level AS[Уровень совместимости]," +
                           "recovery_model_desc AS[Модель восстановления]" +
                           "FROM sys.databases";
        
            Open();
            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
               while (reader.Read())
               {
                  result.Add((string)reader.GetValue(1));
               }
            }
            reader.Close();
            command.Dispose();
            lstr = result;
            Close();
            return "";
         }
         catch (Exception ex)
         { Close(); return ex.Message; }
      
      }

      /// <summary>
      /// Создание таблицы в текущей базе данных
      /// </summary>
      /// <param name="nameTable"> Название таблицы </param>
      /// <param name="nameColumn"> Массив имён столбцов таблицы</param>
      /// <returns></returns>
      public string CreateTable(string nameTable, string[] nameColumn)
      {
         try
         {
            //Список баз данных
            string query = "CREATE TABLE " + nameTable + " (ID INT PRIMARY KEY IDENTITY";
            int countColumn = nameColumn.Length;
            for(int i=0;i<countColumn;i++)
            {
               query = query + ", " + nameColumn[i] + " NVARCHAR(100)";
            }
            query = query + ")";

            Open();
            SqlCommand command = new SqlCommand(query, Connection);
            command.ExecuteNonQuery();
            command.Dispose();
            Close();
            return "";
         }
         catch (Exception ex)
         { Close(); return ex.Message; }
      }

      /// <summary>
      /// Удаление таблицы в текущей базе данных
      /// </summary>
      /// <param name="name"> Название таблицы </param>
      /// <returns></returns>
      public string DeleteTable(string name)
      {
         try
         {
            string str = "DROP TABLE " + name;

            Open();
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            Close();
            return "";
         }
         catch (Exception ex)
         { Close(); return ex.Message; }
      }

      /// <summary>
      /// Переименование названия таблицы
      /// </summary>
      /// <param name="curName"> Текущее название </param>
      /// <param name="newName"> Новое название</param>
      /// <returns></returns>
      public string RenameTable(string curName, string newName)
      {
         try
         {
            string str = "EXEC sp_rename " + curName + ", " + newName + "; ";

            Open();
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            Close();
            return "";
         }
         catch (Exception ex)
         { Close(); return ex.Message; }
      }

      /// <summary>
      /// Получение списка таблиц в текущей базе данных
      /// </summary>
      /// <param name="lstr"> Возвращаемый список таблиц </param>
      /// <returns></returns>
      public string GetNamesTable(ref List<String> lstr)
      {
         List<String> result = new List<string>();
      try
      {
         //Список таблиц
         string query = "SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_TYPE != 'VIEW'";

         Open();
         SqlCommand command = new SqlCommand(query, Connection);
         SqlDataReader reader = command.ExecuteReader();
         if (reader.HasRows)
         {
            while (reader.Read())
            {
               result.Add((string)reader.GetValue(0));
            }
         }
         reader.Close();
         command.Dispose();
         lstr = result;
         Close();
         return "";
      }
      catch (Exception ex)
      { Close(); return ex.Message; }
      }

      public string GetDataTable(string nameTable, ref DataTable table)
      {
         Open();
         try
         {
            string query = "SELECT * FROM " + nameTable;
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            table = ds.Tables[0];
            Close();
            return "";
         }
         catch(Exception ex)
         { Close(); return ex.Message; }
      }

      public string SetDataTable()
      {
         Open();
         try
         {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(ds);
            Close();
            return "";
         }
         catch (Exception ex)
         { Close(); return ex.Message; }

      }


      /// <summary>
      /// Наполение строки данными из строкового массива
      /// </summary>
      /// <param name="row"> Формат строки </param>
      /// <param name="sourse"> Данные для вставки </param>
      /// <param name="fillEmpty"> Включение заполнения пустых </param>
      /// <param name="fillText"> Текст для заполнения пустых </param>
      /// <returns></returns>
      public DataRow SetDataRow (DataRow row, string[] sourse, bool fillEmpty = false, string fillText = "")
      {
         int count = row.Table.Columns.Count - 1; // Вычитаем столбец ID
         if (count < 0) return row;
         string[] result = new string[count];
         Str.CopyArrStr(sourse, result, 0, true);
         for (int i=1;i<row.Table.Columns.Count; i++)
         {
            row[row.Table.Columns[i]] = result[i - 1];
         }
         return row;
      }
   }    
}
