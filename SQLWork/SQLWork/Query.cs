
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using WorkString;

namespace WorkSQL
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
      private string baseName;
      /// <summary>
      /// Чтение / запись имени БД для текущего подключения
      /// </summary>
      public string BaseName 
      { 
         get{ return baseName; } 
         set 
         {
            Entity entity = new Entity(StrConnect);
            entity.Initial_Catalog = value;
            StrConnect = entity.ToString();
            baseName = value;
         }
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
      private bool Open()
      {
         Connection = new SqlConnection(StrConnect);
         try
         {
            Connection.Open();
            baseName = Connection.Database;
            return true;
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
            return false;
         }
      }
      /// <summary>
      /// Отключение
      /// </summary>
      /// <returns></returns>
      public bool Close()
      {
         try
         {
            Connection.Close();
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            return false;
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
      public bool CreateDB(string name, int size, string patch, bool selectToNewDB = false)
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

            if(!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if(!Close()) return false;
            if(selectToNewDB) 
            {
               Entity Entity = new Entity(StrConnect);
               Entity.Initial_Catalog = name;
               StrConnect = Entity.ToString();
            }
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close(); 
            return false; 
         }    
      }

      /// <summary>
      /// Удаление БД
      /// </summary>
      /// <param name="name"> Имя базы данных </param>
      /// <returns></returns>
      public bool DeleteDB(string name)
      {

         try
         {
            string str = "DROP DATABASE " + name;

            if(!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close(); 
            return false; 
         }
      }

      /// <summary>
      /// Получение списка баз данных включённых в ядро SQL Server
      /// </summary>
      /// <param name="lstr"> Возвращаемый список баз данных </param>
      /// <returns></returns>
      public  bool GetNamesDB(ref List<String> lstr, bool only_gen = false, string name_gen = "TypesObjects")
      {
         List<String> result = new List<string>();
         List<String> name_bases = new List<string>();
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

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
               while (reader.Read())
               {
                  name_bases.Add((string)reader.GetValue(1));
               }
               int num = 0;
               foreach(string name_base in name_bases)
               {
                  num++;
                  if (only_gen)
                  {
                     List<string> name_table = new List<string>();
                     // Проверяем наличие таблицы: TypesObjects
                     if (GetNamesTable(name_base, ref name_table))
                     {
                        foreach (string name in name_table)
                        {
                           if (name == name_gen)
                           {
                              result.Add(name_base);
                           }
                        }
                     }

                  }
                  else
                  {
                     result.Add(name_base);
                  }
               }
            }
            reader.Close();
            command.Dispose();
            lstr = result;
            if (!Close()) return false;
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close();
            return false;
         }

      }

      /// <summary>
      /// Создание таблицы в текущей базе данных
      /// </summary>
      /// <param name="nameTable"> Название таблицы </param>
      /// <param name="nameColumns"> Массив имён столбцов таблицы</param>
      /// <returns></returns>
      public bool CreateTable(string nameTable, string[] nameColumns)
      {
         try
         {
            //Список баз данных
            string query = "CREATE TABLE " + nameTable + " (ID INT PRIMARY KEY IDENTITY";
            int countColumn = nameColumns.Length;
            for(int i=0;i<countColumn;i++)
            {
               query = query + ", " + nameColumns[i] + " NVARCHAR(255)";
            }
            query = query + ")";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(query, Connection);
            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close();
            return false;
         }
      }

      public bool CreateTable(string nameTable, List<string> nameColumns)
      {
         try
         {
            //Список баз данных
            string query = "CREATE TABLE " + nameTable + " (ID INT PRIMARY KEY IDENTITY";
  
            foreach (string nameColumn in nameColumns)
            {
               query = query + ", " + nameColumn + " NVARCHAR(255)";
            }
            query = query + ")";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(query, Connection);
            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close();
            return false;
         }
      }

      /// <summary>
      /// Удаление таблицы в текущей базе данных
      /// </summary>
      /// <param name="name"> Название таблицы </param>
      /// <returns></returns>
      public bool DeleteTable(string name)
      {
         try
         {
            string str = "DROP TABLE " + name;

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close();
            return false;
         }
      }

      /// <summary>
      /// Переименование названия таблицы
      /// </summary>
      /// <param name="curName"> Текущее название </param>
      /// <param name="newName"> Новое название</param>
      /// <returns></returns>
      public bool RenameTable(string curName, string newName)
      {
         try
         {
            string str = "EXEC sp_rename " + curName + ", " + newName + "; ";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close();
            return false;
         }
      }

      /// <summary>
      /// Получение списка таблиц в текущей базе данных
      /// </summary>
      /// <param name="lstr"> Возвращаемый список таблиц </param>
      /// <returns></returns>
      public bool GetNamesTable(ref List<String> lstr)
      {
         List<String> result = new List<string>();
         try
         {
            //Список таблиц
            string query = "SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_TYPE != 'VIEW'";

            if (!Open()) return false;
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
            if (!Close()) return false;
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close();
            return false;
         }
      }

      /// <summary>
      /// Получение списка таблиц в указанной базе данных
      /// </summary>
      /// <param name="lstr"> Возвращаемый список таблиц </param>
      /// <returns></returns>
      public bool GetNamesTable(string name_base, ref List<String> lstr)
      {
         List<String> result = new List<string>();
         if (name_base == null || name_base == "") name_base = BaseName;
         try
         {
            //Список таблиц
            string query = "SELECT TABLE_NAME FROM [" + name_base + "].information_schema.TABLES WHERE TABLE_TYPE != 'VIEW'";

            if (!Open()) return false;
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
            if (!Close()) return false;
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close();
            return false;
         }
      }

      public string DelExtPropDB(string name_base, string name_prop = "Caption")
      {
         string result = "";
         if (!Open()) return result;
         try
         {
            string query = "USE " + name_base +
                           " EXEC sys.sp_dropextendedproperty" +
                           " @name = '" + name_prop + "'";
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            result = name_prop;
            Close();
         }
         catch (Exception ex)
         {
            result = "Not property";
            Close();
         }
         return result;
      }

      /// <summary>
      /// Установить/обновить свойство БД
      /// </summary>
      /// <param name="name_base">Имя БД</param>
      /// <param name="name_prop">Имя свойства</param>
      /// <param name="val">Значение</param>
      /// <returns></returns>
      public bool SetExtPropDB(string name_base = null, string name_prop = "Caption", string val = "")
      {
         bool result = false;
         if (!Open()) return result;
         try
         {
            if (name_base == null) name_base = this.baseName;
            string query = "USE " + name_base +
                           " EXEC sys.sp_addextendedproperty"  +
                           " @name = '" + name_prop + "'," +
                           " @value = '" + val + "'";
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            result = true;
            Close();
         }
         catch (Exception ex)
         {
            string query = "USE " + name_base +
                           " EXEC sys.sp_updateextendedproperty" +
                           " @name = '" + name_prop + "'," +
                           " @value = '" + val + "'";
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            result = true;
            Close();
         }
         return result;
      }

      /// <summary>
      /// Получить указанное свойство базы данных
      /// </summary>
      /// <param name="name_base">Имя базы данных</param>
      /// <param name="name_prop">Имя свойства</param>
      /// <returns></returns>
      public string GetExtPropDB(string name_base = null, string name_prop = "Caption")
      {
         string result = "";
         if (!Open()) return result;
         try
         {
            if (name_base == null) name_base = this.baseName;
            string query = "SELECT *" + 
                           " FROM " + name_base + ".sys.extended_properties" +
                           " WHERE name = '" + name_prop + "'";
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            DataTable table = ds.Tables[0];
            if(table.Rows.Count>0)
               result = table.Rows[0]["value"].ToString();
            Close();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close();
         }
         return result;
      }
      /// <summary>
      /// Получение всех таблиц с данными из указанной базы или текущей базы
      /// </summary>
      /// <param name="dataSet"></param>
      /// <param name="nameBase"></param>
      /// <returns></returns>
      public bool GetTablesBase(ref DataSet dataSet, string nameBase = "")
      {
         bool result = false;
         List<string> names_table = new List<string>();
         GetNamesTable(nameBase, ref names_table);
         foreach(string name_table in names_table)
         {
            DataTable dataTable = new DataTable();
            GetTable(nameTable: name_table, ref dataTable, nameBase: nameBase);
            dataSet.Tables.Add(dataTable.Copy());
            result = true;
         }

         return result;
      }
      public bool GetTable(string nameTable, ref DataTable table, string nameBase = "", string select = "SELECT *", string where = "", string order_by = "")
      {
         if (!Open()) return false;
         if (nameBase == null || nameBase == "") nameBase = baseName;
         if (nameBase != "" && nameBase.IndexOf("[") < 0 && nameBase.IndexOf("]") < 0)
            nameBase = "[" + nameBase + "].";
         try
         {
            // Проверка ввода данных
            if (select != "SELECT *")
            {
               if (select == "")
               {
                  select = "SELECT *";
               }
               else if (select.IndexOf("SELECT") == -1)
               {
                  select = "SELECT " + select;
               }
            }
            if (where != "")
            {
               if (where.IndexOf("WHERE") == -1)
               {
                  where = "WHERE " + where;
               }
            }
            if (order_by != "")
            {
               if (order_by.IndexOf("ORDER") == -1)
               {
                  order_by = " ORDER BY " + order_by;
               }
            }
            string query = select + " FROM " + nameBase + "[dbo].["+ nameTable + "] " + where + order_by;
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            table = ds.Tables[0];
            //table.TableName = nameTable; // Нельзя делать, теряет связь с базой
            table.Namespace = nameTable;
            if (!Close()) return false;
            return true;
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            Close();
            return false;
         }
      }

      public bool SetDataTable()
      {
         if (!Open()) return false;
         try
         {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(ds);
            if (!Close()) return false;
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            Close();
            return false;
         }

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
         try
         {
            int count = row.Table.Columns.Count - 1; // Вычитаем столбец ID
            if (count < 0) return row;
            string[] result = new string[row.Table.Columns.Count];
            Str.CopyArrStr(sourse, result, 0, true);
            for (int i = 1; i < row.Table.Columns.Count; i++)
            {
               row[row.Table.Columns[i]] = result[i - 1];
            }
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         return row;
      }

      public DataRow SetDataRow(DataRow row, List<string> sourse, bool fillEmpty = false, string fillText = "")
      {
         try
         {
            int count = row.Table.Columns.Count - 1; // Вычитаем столбец ID
            if (count < 0) return row;
            List<string> result = new List<string>(row.Table.Columns.Count);
            Str.CopyLstStr(sourse, result, 0, true);
            for (int i = 1; i < row.Table.Columns.Count; i++)
            {
               row[i] = result[i - 1];
            }
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         return row;
      }

      /// <summary>
      /// Получение списка значений строк из одной колонки таблицы
      /// </summary>
      /// <param name="table"> Таблица с данными </param>
      /// <param name="column_num"> Номер колонки </param>
      /// <returns></returns>
      public List<string> GetListRowsInOneColumnTable( DataTable table, int column_num = 0, bool unique = false, string column_name = "" )
      {
         List<string> lstResult = new List<string>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataRow Row in table.Rows)
            {
               if (unique)
               {
                  if (column_name == "")
                  {
                     if (!dicResult.ContainsKey(Row[column_num].ToString()))
                     {
                        dicResult.Add(Row[column_num].ToString(), "value");
                        lstResult.Add(Row[column_num].ToString());
                     }
                  }
                  else
                  {
                     if (!dicResult.ContainsKey(Row[column_name].ToString()))
                     {
                        dicResult.Add(Row[column_name].ToString(), "value");
                        lstResult.Add(Row[column_name].ToString());
                     }
                  }
               }
               else
               {
                  if (column_name == "")
                  {
                     lstResult.Add(Row[column_num].ToString());
                  }
                  else
                  {
                     lstResult.Add(Row[column_name].ToString());
                  }
               }
            }
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         return lstResult;
      }

      public List<string> GetListRowsInOneColumnTable(DataTable table, string name, bool unique = false)
      {
         List<string> lstResult = new List<string>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataRow Row in table.Rows)
            {
               if (unique)
               {
                  if (!dicResult.ContainsKey(Row[name].ToString()))
                  {
                     dicResult.Add(Row[name].ToString(), "value");
                     lstResult.Add(Row[name].ToString());
                  }
               }
               else
               {
                  lstResult.Add(Row[name].ToString());
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         return lstResult;
      }

      /// <summary>
      /// Наполнение словаря данными из таблицы
      /// </summary>
      /// <param name="table">Таблица</param>
      /// <param name="num">Номер столбца для ключа</param>
      /// <param name="num2">Номер столбца для значения</param>
      /// <param name="num3">Номер столбца для доп данных для замены символа</param>
      /// <param name="find_text">Символы для замены</param>
      /// <returns></returns>
      public Dictionary<string,string> GetListRowsInOneColumnTable(DataTable table, int num, int num2, int num3 = 5, string find_text = "[]")
      {
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataRow Row in table.Rows)
            {
               if (!dicResult.ContainsKey(Row[num].ToString()))
                  dicResult.Add(Row[num].ToString(), Row[num2].ToString().IndexOf(find_text)>0 ? Row[num2].ToString().Replace("[]", "[" + Row[num3].ToString() + "]") : Row[num2].ToString());
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         return dicResult;
      }
      /// <summary>
      /// Получение списка значений столбцов из одной строки таблицы
      /// </summary>
      /// <param name="table"></param>
      /// <param name="num"></param>
      /// <returns></returns>
      public List<string> GetListColumnsInOneRowTable(DataTable table, int num, bool unique = false)
      {
         List<string> lstResult = new List<string>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataColumn Col in table.Columns)
            {
               if (unique)
               {
                  if (!dicResult.ContainsKey(Col.Table.Rows[num].ToString()))
                  {
                     dicResult.Add(Col.Table.Rows[num].ToString(), "value");
                     lstResult.Add(Col.Table.Rows[num].ToString());
                  }
               }
               else
               {
                  lstResult.Add(Col.Table.Rows[num].ToString());
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         return lstResult;
      }

      public List<string> GetListColumnsInOneRowTable(List<List<string>> table, int num, bool unique = false)
      {
         List<string> lstResult = new List<string>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            for (int i = 0; i < table.Count; i++)
            {
               if (unique)
               {
                  if (!dicResult.ContainsKey(table[i][num].ToString()))
                  {
                     dicResult.Add(table[i][num].ToString(), "value");
                     lstResult.Add(table[i][num].ToString());
                  }
               }
               else
               {
                  lstResult.Add(table[i][num].ToString());
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         return lstResult;
      }
   }    
}
