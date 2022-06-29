
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using WorkString;
using WorkStruct;
using WorkMisc;

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
      public DataSet ds = new DataSet();
      private SqlDataAdapter adapter;

      /// <summary>
      /// Индикатор успешного последнего подключения ... 
      /// </summary>
      public int TestConOk = 0;
      /// <summary>
      /// Конструктор
      /// </summary>
      /// <param name="StrCon"> Строка подключения </param>
      public Query(string StrCon)
      {
         StrConnect = StrCon;
         Entity entity = new Entity(StrConnect);
         nameBase = entity.Initial_Catalog;

         TypeSQL = new Dictionary<string, string>() 
         {
            ["Bool"]       = "bit",
            ["bool"]       = "bit",
            ["Boolean"]    = "bit",
            ["boolean"]    = "bit",
            ["Dint"]       = "int",
            ["dint"]       = "int",
            ["Float"]      = "real",
            ["float"]      = "real",
            ["Integer"]    = "int",
            ["Integer"]    = "int",
            ["Int"]        = "int",
            ["int"]        = "int",
            ["Int32"]      = "int",
            ["int32"]      = "int",
            ["String"]     = "nvarchar(255)",
            ["string"]     = "nvarchar(255)",
            ["Str"]        = "nvarchar(255)",
            ["str"]        = "nvarchar(255)",
            ["Uint"]       = "int",
            ["uint"]       = "int",
            ["Real"]       = "real",
            ["Single"]     = "real",
            ["single"]     = "real",
            ["real"]       = "real",
            ["Word"]       = "int",
            ["word"]       = "int"
         };
      }
      private string nameBase;
      /// <summary>
      /// Чтение / запись имени БД для текущего подключения
      /// </summary>
      public string BaseName 
      { 
         get{ return nameBase; } 
         set 
         {
            Entity entity = new Entity(StrConnect);
            entity.Initial_Catalog = value;
            StrConnect = entity.ToString();
            nameBase = value;
         }
      }

      /// <summary>
      /// Проверка связи с сервером (асинхронная проверка)
      /// </summary>
      public async void asyncTestCon()
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
            MessErr(ex);
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
      /// Проверка связи с сервером
      /// </summary>
      public bool TestCon()
      {
         // Создание подключения
         SqlConnection connection = new SqlConnection(StrConnect);
         try
         {
            // Открываем подключение
            connection.Open();
            TestConOk = 1;
            return true;

         }
         catch (SqlException ex)
         {
            TestConOk = 2;
            MessErr(ex);
            return false;
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
            nameBase = Connection.Database;
            return true;
         }
         catch(Exception ex)
         {
            MessErr(ex);
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
            MessErr(ex);
            return false;
         }
      }

      /// <summary>
      /// Удаление всех строк из таблицы
      /// </summary>
      /// <param name="NameTable"></param>
      /// <param name="NameBase"></param>
      /// <returns></returns>
      public bool DelAllDataTable(string NameTable, string NameBase = "")
      {
         if (NameBase != "") NameBase = NameBase + ".";
         try
         {
            string str = "DELETE FROM " + NameBase + "dbo." + NameTable;
            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
         }
         catch (Exception ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
         return true;
      }

      /// <summary>
      /// Задать первичный ключь в таблице
      /// </summary>
      /// <param name="NameTable"></param>
      /// <param name="NameColumn"></param>
      /// <param name="NameBase"></param>
      /// <returns></returns>
      public bool SetPrimKeyTable(string NameTable, string NameColumn = "ID", string NameBase = null)
      {
         if (NameBase == null) NameBase = this.nameBase;
         try
         {
            string str = "USE ["+ NameBase + "] ALTER TABLE [" + NameTable + "] ADD PRIMARY KEY (" + NameColumn + ")";
            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
         }
         catch (Exception ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
         return true;
      }
      /// <summary>
      /// Получает информацию о колонках таблицы 'COLUMN_NAME', 'IS_NULLABLE', 'DATA_TYPE'...
      /// </summary>
      /// <param name="table"></param>
      /// <param name="NameTable"></param>
      /// <param name="NameBase"></param>
      /// <param name="MsgYes"></param>
      /// <returns></returns>
      private bool GetColumnsInfo(out DataTable table, string NameTable, string NameBase = "", bool MsgYes=true)
      {
         table = null;
         if (!Open()) return false;
         if (NameBase == null || NameBase == "") NameBase = this.nameBase;
         if (NameBase != "" && NameBase.IndexOf("[") < 0 && NameBase.IndexOf("]") < 0)
            NameBase = "[" + NameBase + "]";
         try
         {
            string query = "USE " + NameBase + " SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" +  NameTable + "'";
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            table = ds.Tables[0];
            //table.TableName = nameTable; // Нельзя делать, теряет связь с базой
            table.Namespace = NameTable;

            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            if (MsgYes) MessErr(ex);
            Close();
            return false;
         }
      }
      /// <summary>
      /// Получить имена столбцов заданной таблицы
      /// </summary>
      /// <param name="lstNamesColumn"></param>
      /// <param name="NameTable"></param>
      /// <param name="NameBase"></param>
      /// <returns></returns>
      public bool GetNamesColumn( out List<string> lstNamesColumn, string NameTable, string NameBase = "")
      {
         lstNamesColumn = new List<string>();
         DataTable table = new DataTable();
         GetColumnsInfo(NameTable: NameTable, NameBase: NameBase, table: out table);
         foreach (DataRow row in table.Rows)
            lstNamesColumn.Add(row["COLUMN_NAME"].ToString());
         return true;
      }

      /// <summary>
      /// Удаление данных из таблицы по условию where
      /// </summary>
      /// <param name="NameTable"></param>
      /// <param name="NameBase"></param>
      /// <param name="where"></param>
      /// <returns></returns>
      public bool DelDataTable(string NameTable,
                               string NameBase = "",
                               string where = "")
      {
         // Подготовка данных

         // Формирование строки с именем бады данных
         if (NameBase != "") NameBase = NameBase + ".";
         // Формирование строки условия выборки
         if (where != "" && !where.ToUpper().Contains("WHERE")) where = " WHERE " + where;
         try
         {
            string str = "DELETE FROM " + NameBase + "dbo." + NameTable + where;

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
         }
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
         return true;
      }

      /// <summary>
      /// Копирование данных таблицы в существующую таблицу
      /// </summary>
      /// <param name="NameTableSource"></param>
      /// <param name="NameTableDest"></param>
      /// <param name="NameBaseSource"></param>
      /// <param name="NameBaseDest"></param>
      /// <param name="where"> Условие выборки данных </param>
      /// <param name="ExceptColumns"> Список столобцов для исключения </param>
      /// <returns></returns>
      public bool CopyDataTable( string NameTableSource, 
                                 string NameTableDest, 
                                 string NameBaseSource = "", 
                                 string NameBaseDest = "", 
                                 string where = "",
                                 List<string> ExceptColumns = null,
                                 List<string> NeedColumns = null)
      {
         // Подготовка данных
         if (NameBaseDest == null || NameBaseDest == "") NameBaseDest = nameBase;
         // Список столбцов для исключения
         if ( ExceptColumns == null ) ExceptColumns = new List<string>() { "ID" };
         // Список столбцов для копирования
         List<string> lstNamesColumn;
         if ( NeedColumns == null )
         {
            if (!GetNamesColumn(out lstNamesColumn, NameTableSource, NameBaseSource)) return false;
         }
         else
         {
            lstNamesColumn = NeedColumns;
         }
         
         // Удаление не нужных столбцов для копирования
         foreach ( string ExceptColumn in ExceptColumns)
         {
            lstNamesColumn.Remove(ExceptColumn);
         }
         // Формирование строки перечесления столбцов
         string NamesColumn = lstNamesColumn[0];
         for( int num=1; num < lstNamesColumn.Count; num++)
         {
            NamesColumn = NamesColumn + ", " + lstNamesColumn[num];
         }
         // Формирование строки с именем бады данных
         if ( NameBaseSource != "" ) NameBaseSource = NameBaseSource + ".";
         // Формирование строки условия выборки
         if ( where != "" && !where.ToUpper().Contains("WHERE")) where = " WHERE " + where;
         try
         {
            string str = "INSERT INTO " + NameBaseDest + ".dbo." + NameTableDest +
                         "( " + NamesColumn + ")" +
                         " SELECT DISTINCT " + NamesColumn +
                         " FROM " + NameBaseSource + "dbo." + NameTableSource + where;

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
         }
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
         return true;
      }

      /// <summary>
      /// Копировние структуры таблицы во вновь создоваемую таблицу и копирование данных при AllData = true
      /// </summary>
      /// <param name="NameTableSour"></param>
      /// <param name="NameTableDest"></param>
      /// <param name="NameBaseSour"></param>
      /// <param name="NameBaseDest"></param>
      /// <param name="AllData"></param>
      /// <returns></returns>
      public bool CopyTable(string NameTableSour, string NameTableDest, string NameBaseSour = null, string NameBaseDest = null, bool AllData = false)
      {
         if (NameBaseSour == null) NameBaseSour = nameBase;
         if (NameBaseDest == null) NameBaseDest = nameBase;
         string WHERE = AllData ? "" : " WHERE 0 = 1";

         try
         {
            string str = "SELECT * INTO " + NameBaseDest + ".dbo." + NameTableDest + 
                                 " FROM " + NameBaseSour + ".dbo." + NameTableSour +
                                 WHERE;

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            SetPrimKeyTable(NameTableDest, GetPrimKeyTable(NameTableSour));
         }
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
         return true;
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
         catch (SqlException ex)
         {
            MessErr(ex);
            Close(); 
            return false; 
         }
      }

      /// <summary>
      /// Удаление БД
      /// </summary>
      /// <param name="name"> Имя базы данных </param>
      /// <returns></returns>
      public bool DelDB(string name)
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
         catch (SqlException ex)
         {
            MessErr(ex);
            Close(); 
            return false; 
         }
      }
      public string GetStrConnect()
      {
         return StrConnect;
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
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }

      }
      #region CreateTable
      /// <summary>
      /// Создание таблицы со строковыми типами, с добавлением взаимосвязей ('NameColumn', 'NameTableForeign', 'NameColumnForeign')
      /// </summary>
      /// <param name="NameTable"> Название таблицы </param>
      /// <param name="NameColumns"> Массив имён столбцов таблицы</param>
      /// <param name="ForeignTabCol"> 'NameColumn', 'NameTableForeign', 'NameColumnForeign'</param>
      /// <param name="MsgYes"> Выдавать сообщение об ошибки</param>
      /// <returns></returns>
      public bool CreateTable(string NameTable, string[] NameColumns, List<string[]> ForeignTabCol = null, bool MsgYes = true)
      {
         try
         {
            string Foreign = "";
            // Взаимосвязи
            if (ForeignTabCol != null)
            {
               foreach(string[] data in ForeignTabCol)
               {
                  Foreign = ",CONSTRAINT [FK__" + NameTable + "_" + data[0] +" FOREIGN KEY (" + data[0] + ") REFERENCES " + data[1] + " (" + data[2] + ")";
               }
            }
            // Список баз данных
            string query = "CREATE TABLE " + NameTable + " (ID INT " + "CONSTRAINT [PK__" + NameTable + "_" + "PRIMARY KEY IDENTITY";
            int countColumn = NameColumns.Length;
            for(int i=0;i<countColumn;i++)
            {
               query = query + ", " + NameColumns[i] + " NVARCHAR(255)";
            }
            query = query + Foreign + ")";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(query, Connection);
            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            if ( MsgYes ) MessErr(ex);
            Close();
            return false;
         }
      }
      public Dictionary<string, string> TypeSQL;

      /// <summary>
      /// Создание таблицы с указанием типов данных ('NameColumn', 'TypeColumn', 'flag PK', 'flag NULL'), 
      /// с добавлением взаимосвязей ('NameColumn', 'NameTableForeign', 'NameColumnForeign')
      /// </summary>
      /// <param name="NameTable"></param>
      /// <param name="TuneColumns">('NameColumn', 'TypeColumn', 'flag PK', 'flag NULL', 'DefValue')</param>
      /// <param name="ForeignColumns">('NameColumn', 'NameTableForeign', 'NameColumnForeign')</param>
      /// <param name="MsgYes"></param>
      /// <returns></returns>
      public bool CreateTable(string NameTable, List<string[]> TuneColumns, List<string[]> ForeignColumns = null, string NameBase=null, bool MsgYes = true)
      {
         try
         {
            if (TuneColumns.Count == 0 || TuneColumns[0].Length == 0) 
               MessErr( error: "Таблица без колонок ни кому не нужна!");
            if (NameBase == null || NameBase == "") 
               NameBase = this.nameBase;
            string Foreign = "";
            // Взаимосвязи
            if (ForeignColumns != null){
               foreach (string[] data in ForeignColumns)
                  Foreign = Foreign + ",CONSTRAINT [FK__"+ NameTable +"_"+ data[0] + "] FOREIGN KEY ([" + data[0] + "]) REFERENCES [" + data[1] + "] ([" + data[2] + "])";
            }
            //Список настройки колонок
            string query = "USE " + NameBase + " CREATE TABLE [" + NameTable + "] (";
            int countColumn = TuneColumns.Count;
            for (int i = 0; i < countColumn; i++){
               query = query + "[" + TuneColumns[i][0] + "] " + // NAME
                  (TuneColumns[i].Length > 1 ? (TuneColumns[i][1] != null ? (TypeSQL.ContainsKey(TuneColumns[i][1]) ? TypeSQL[TuneColumns[i][1]] : "NVARCHAR(255)") : "NVARCHAR(255)") : "NVARCHAR(255)") + // TYPE
                  (TuneColumns[i].Length > 2 ? (TuneColumns[i][2] != null ? (TuneColumns[i][2].ToLower() == "true" ? (TuneColumns[i][1] != null ? (TuneColumns[i][1].ToLower() == "int" ? "CONSTRAINT [PK__" + NameTable + "_" + TuneColumns[i][0] + "] PRIMARY KEY IDENTITY" : "CONSTRAINT [PK__" + NameTable + "_" + TuneColumns[i][0] + "] PRIMARY KEY") : "CONSTRAINT [PK__" + NameTable + "_" + TuneColumns[i][0] + "] PRIMARY KEY") : "") : "") : "") + // PK
                  (TuneColumns[i].Length > 3 ? (TuneColumns[i][3] != null ? (TuneColumns[i][3].ToLower() == "true" ? " NULL" : " NOT NULL") : " NOT NULL") : " NULL") + // NULL
                  (TuneColumns[i].Length > 4 ? (TuneColumns[i][4] != null ? " CONSTRAINT [DF__" + NameTable + "_" + TuneColumns[i][0] + "] DEFAULT '" + TuneColumns[i][4] + "'" : "") : "") + ","; // DV
            }
            query = query.TrimEnd(',');
            query = query + Foreign + ")";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(query, Connection);
            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex){
            if (MsgYes) MessErr(ex);
            Close();
            return false;
         }
      }

      public bool CreateTable(string NameTable, List<Dictionary<string, string>> NameAndTypeColumns,
                              bool ErrMess = true, bool UseType = true, string PrimKey = "ID")
      {
         try
         {
            // Первичный ключ
            if (PrimKey == "ID")
               PrimKey = " (ID INT CONSTRAINT[PK__" + NameTable + "_ID] PRIMARY KEY IDENTITY";
            else
            {
               foreach (Dictionary<string, string> col in NameAndTypeColumns)
               {
                  if (col["NAME"] == PrimKey)
                  {
                     PrimKey = " ([" + col["NAME"] + "] " + col["TYPE"] + " CONSTRAINT [PK__" + NameTable + "_"+ col["NAME"] + "] PRIMARY KEY";
                     NameAndTypeColumns.Remove(col);
                     break;
                  }
               }
            }

            // Список баз данных
            string query = "CREATE TABLE " + NameTable + PrimKey;
            int countColumn = NameAndTypeColumns.Count;
            for (int i = 0; i < countColumn; i++)
            {
               query = query + ", [" + NameAndTypeColumns[i]["NAME"] + "] " +
                  (UseType && TypeSQL.ContainsKey(NameAndTypeColumns[i]["TYPE"]) ? TypeSQL[NameAndTypeColumns[i]["TYPE"]] : "NVARCHAR(255)");
            }
            query = query + ")";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(query, Connection);
            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            if (ErrMess) MessErr(ex);
            Close();
            return false;
         }
      }

      /// <summary>
      /// Создание таблицы с строковыми колонками
      /// </summary>
      /// <param name="NameTable"></param>
      /// <param name="nameColumns"></param>
      /// <param name="ErrMess"></param>
      /// <param name="PrimKey"></param>
      /// <returns></returns>
      public bool CreateTable(string NameTable, List<string> nameColumns, bool ErrMess = true, string PrimKey = "ID")
      {
         try
         {
            // Первичный ключ
            if (PrimKey == "ID")
               PrimKey = " (ID INT CONSTRAINT[PK__" + NameTable + "_ID] PRIMARY KEY IDENTITY";
            else
            {
               foreach (string cln in nameColumns)
               {
                  if (cln == PrimKey)
                  {
                     PrimKey = " ([" + cln + "] " + "NVARCHAR(255) CONSTRAINT [PK__" + NameTable + "_" + cln + "] PRIMARY KEY";
                     nameColumns.Remove(cln);
                     break;
                  }
               }
            }

            //Список баз данных
            string query = "CREATE TABLE " + NameTable + PrimKey;

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
         catch (SqlException ex)
         {
            if (ErrMess) MessErr(ex);
            Close();
            return false;
         }
      }
      #endregion CreateTable
      public string GetPrimKeyTable(string NameTable, string NameBase=null, bool MsgYes =true)
      {
         if (!Open()) return "";
         if (NameBase == null || NameBase == "") NameBase = nameBase;
         if (NameBase != "" && NameBase.IndexOf("[") < 0 && NameBase.IndexOf("]") < 0)
            NameBase = "[" + NameBase + "] ";
         try
         {
            string query = "USE " + NameBase + 
                           "SELECT column_name " +
                           "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                           "WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 "+
                           "AND table_name = '" + NameTable + "'";
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            DataTable table = ds.Tables[0];
            //table.TableName = nameTable; // Нельзя делать, теряет связь с базой
            table.Namespace = NameTable;
            if (!Close()) return "";
            return table.Rows.Count>0?table.Rows[0][0].ToString():"";
         }
         catch (SqlException ex)
         {
            if (MsgYes) MessErr(ex);
            Close();
            return "";
         }
      }
      public enum GetLinkTableType{
         ALL, CURR, REF
      }
      /// <summary>
      /// Получает список связей таблиц (TABLE_NAME, COLUMN_NAME, REF_TABLE_NAME, REF_COLUMN_NAME)
      /// </summary>
      /// <param name="NameTable"></param>
      /// <param name="NameBase"></param>
      /// <param name="MsgYes"></param>
      /// <returns></returns>
      public List<string[]> GetForeignKey(out DataTable table, string NameBase = null, string NameTable=null, string NameColumn=null, bool MsgYes = true, GetLinkTableType type = GetLinkTableType.ALL)
      {
         List<string[]> result = new List<string[]>();
         table = new DataTable();
         if (!Open()) return result;
         if (NameBase == null || NameBase == "") NameBase = nameBase;
         if (NameBase != "" && NameBase.IndexOf("[") < 0 && NameBase.IndexOf("]") < 0)
            NameBase = "[" + NameBase + "] ";
         string WHERE = "";
         if (NameTable != null) WHERE = "WHERE IS_KCU1.TABLE_NAME = '" + NameTable + "'" + " OR IS_KCU2.TABLE_NAME = '" + NameTable + "'" +
           ((NameColumn==null) ? "" : (" AND IS_KCU1.COLUMN_NAME = '" + NameColumn + "'"));
         try
         {
            string query = "USE " + NameBase +
                           "SELECT " +
                           "IS_KCU1.CONSTRAINT_SCHEMA," +
                           "IS_KCU1.CONSTRAINT_NAME," +
                           "IS_KCU1.TABLE_SCHEMA FK_TABLE_SCHEMA," +
                           "IS_KCU1.TABLE_NAME FK_TABLE_NAME," +
                           "IS_KCU1.COLUMN_NAME FK_COLUMN_NAME," +
                           "IS_KCU2.TABLE_SCHEMA REFERENCED_TABLE_SCHEMA," +
                           "IS_KCU2.TABLE_NAME REFERENCED_TABLE_NAME," +
                           "IS_KCU2.COLUMN_NAME REFERENCED_COLUMN_NAME " +
                           "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS IS_RC " +
                           "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE IS_KCU1 " +
                           "ON IS_KCU1.CONSTRAINT_CATALOG = IS_RC.CONSTRAINT_CATALOG " +
                           "AND IS_KCU1.CONSTRAINT_SCHEMA = IS_RC.CONSTRAINT_SCHEMA " +
                           "AND IS_KCU1.CONSTRAINT_NAME = IS_RC.CONSTRAINT_NAME " +
                           "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE IS_KCU2 " +
                           "ON IS_KCU2.CONSTRAINT_CATALOG = IS_RC.UNIQUE_CONSTRAINT_CATALOG " +
                           "AND IS_KCU2.CONSTRAINT_SCHEMA = IS_RC.UNIQUE_CONSTRAINT_SCHEMA " +
                           "AND IS_KCU2.CONSTRAINT_NAME = IS_RC.UNIQUE_CONSTRAINT_NAME " +
                           "AND IS_KCU2.ORDINAL_POSITION = IS_KCU1.ORDINAL_POSITION " +
                           WHERE;
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            table = ds.Tables[0];
            //table.TableName = nameTable; // Нельзя делать, теряет связь с базой
            table.Namespace = NameTable;
            if (!Close()) return result;
            switch (type)
            {
               case GetLinkTableType.ALL:
                  {
                     foreach (DataRow row in table.Rows)
                        result.Add(new string[] { row["FK_TABLE_NAME"].ToString(),
                                         row["FK_COLUMN_NAME"].ToString(),
                                         row["REFERENCED_TABLE_NAME"].ToString(),
                                         row["REFERENCED_COLUMN_NAME"].ToString() });
                     break;
                  }
               case GetLinkTableType.CURR:
                  {
                     foreach (DataRow row in table.Rows)
                        if (NameTable == row["FK_TABLE_NAME"].ToString())
                        result.Add(new string[] { row["FK_TABLE_NAME"].ToString(),
                                         row["FK_COLUMN_NAME"].ToString(),
                                         row["REFERENCED_TABLE_NAME"].ToString(),
                                         row["REFERENCED_COLUMN_NAME"].ToString() });
                     break;
                  }
               case GetLinkTableType.REF:
                  {
                     foreach (DataRow row in table.Rows)
                        if (NameTable == row["REFERENCED_TABLE_NAME"].ToString())
                           result.Add(new string[] { row["FK_TABLE_NAME"].ToString(),
                                         row["FK_COLUMN_NAME"].ToString(),
                                         row["REFERENCED_TABLE_NAME"].ToString(),
                                         row["REFERENCED_COLUMN_NAME"].ToString() });
                     break;
                  }
            }
            
            return result;
         }
         catch (SqlException ex)
         {
            if (MsgYes) MessErr(ex);
            Close();
            return result;
         }
      }

      /// <summary>
      /// Удаление таблицы в текущей базе данных
      /// </summary>
      /// <param name="nameTable"> Название таблицы </param>
      /// <returns></returns>
      public bool DelTable(string nameTable, bool ErrMess = true)
      {
         try
         {
            string str = "DROP TABLE " + nameTable;

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            if ( ErrMess ) MessErr(ex);
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
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
      }
      /// <summary>
      /// Сообщение об ошибки
      /// </summary>
      /// <param name="ex"></param>
      /// <param name="caption"></param>
      /// <param name="error"></param>
      /// <param name="buttons"></param>
      /// <param name="icon"></param>
      /// <returns></returns>
      public DialogResult MessErr(
         Exception ex=null, string caption = "Ошибка", string error = "",
         MessageBoxButtons buttons = MessageBoxButtons.OK,
         MessageBoxIcon icon = MessageBoxIcon.Error)
      {
         string exInfo = "";
         if (ex != null){
            exInfo = "Message: " + ex.Message + "\n" +
                     "Source: " + ex.Source + "\n" +
                     "StackTrace: " + ex.StackTrace + "\n" +
                     "TargetSite: " + ex.TargetSite + "\n" +
                     "HelpLink: " + ex.HelpLink;
         }
         error = error != "" ? "Error: " + error + "\n" : error;
         return MessageBox.Show(
            text: error + exInfo,
            caption: caption,
            buttons: buttons,
            icon: icon);
      }
      public bool RenameColumn(string tabName, string curName, string newName, string basName=null)
      {
         try
         {
            if (basName == null) basName = this.nameBase ;
            string str = "USE "+ basName + " EXEC sp_rename '"+ tabName+ "." + curName + "', [" + newName + "]; ";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
      }

      public bool RenameColumnType(string tabName, string colName, string typeName, string basName = null)
      {
         try
         {
            if (basName == null) basName = this.nameBase;
            string str = "USE [" + basName + "] ALTER TABLE [" + tabName + "] ALTER COLUMN [" + colName + "] [" + typeName + "]";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
      }

      /// <summary>
      /// Добавляет колонку в существующую таблицу ForeignTabCol:[COLUMN_NAME][FOREIGN_TABLE_NAME][FOREIGN_COLUMN_NAME]
      /// </summary>
      /// <param name="tabName"></param>
      /// <param name="colName"></param>
      /// <param name="colType"></param>
      /// <param name="NullYes"></param>
      /// <param name="DefaultData"></param>
      /// <param name="basName"></param>
      /// <param name="ForeignTabCol"></param>
      /// <returns></returns>
      public bool AddColumn(string tabName,
                            string colName,
                            string colType = "nvarchar(255)",
                              bool NullYes = true,
                            string DefaultData = null,
                            string basName = null,
                          string[] ForeignTabCol = null)
      {
         try
         {
            if (basName == null) basName = this.nameBase;
            string Foreign = "";
            // Взаимосвязи
            if (ForeignTabCol != null && ForeignTabCol[0] != null && ForeignTabCol[1] != null && ForeignTabCol[2] != null)
            {
               Foreign = Foreign + ",CONSTRAINT [FK__"+ ForeignTabCol[0] + "] FOREIGN KEY ([" + ForeignTabCol[0] + "]) REFERENCES [" + ForeignTabCol[1] + "] (" + ForeignTabCol[2] + ")";
            }
            string str = "USE "+ basName + " ALTER TABLE [" + tabName + "]"+
                         " ADD [" + colName + "] " + colType +
                         (NullYes ? " NULL":" NOT NULL") + 
                         (DefaultData==null? "" : " CONSTRAINT [DF__" + colName + "] DEFAULT '" + DefaultData + "'")+ Foreign + ";";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
      }
      public bool AddForeignKey(string tabName, string colName, string tabFName, string colFName, string basName = null)
      {
         try
         {
            if (basName == null) basName = this.nameBase;
            // Взаимосвязи
            string str = "USE " + basName + " ALTER TABLE [" + tabName + "]" +
                         "ADD FOREIGN KEY("+ colName + ") REFERENCES ["+ tabFName + "]("+ colFName + ")";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
      }

      public bool DelForeignKey(string tabName, string Key, string baseName = null, bool MsgYes = true)
      {
         try
         {
            if (baseName == null) baseName = this.nameBase;
            // Взаимосвязи
            string str = "USE [" + baseName + "] ALTER TABLE ["+ tabName + "] DROP [" + Key+"]";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            if(MsgYes)MessErr(ex);
            Close();
            return false;
         }
      }
      public bool DelConstraintColumn(string nameTable, string key, string nameBase=null, bool MsgYes = true)
      {
         try
         {
            if (this.nameBase == null) nameBase = this.nameBase;

            string query = "USE [" + nameBase + "] ALTER TABLE " + nameTable + " DROP [" + key + "]";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(query, Connection);
            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            if (MsgYes) MessErr(ex);
            Close();
            return false;
         }
      }

      public bool DelColumn(string tabName, 
                            string colName, 
                            string basName = null, 
                              bool MsgYes=true, 
                              bool DelDF = false,
                              bool DelFK=false)
      {
         try
         {
            if (basName == null) basName = this.nameBase;
            if (DelDF) DelConstraintColumn(tabName, "DF__" + colName, basName, false);
            if (DelFK) DelForeignKey(tabName, "FK__" + colName, basName, false);
            string str = "USE ["+ basName + "] ALTER TABLE ["  + tabName +"]" +
                         " DROP COLUMN [" + colName + "];";

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            if(MsgYes)MessErr(ex);
            Close();
            return false;
         }
      }

      /// <summary>
      /// Получение списка таблиц в текущей базе данных
      /// </summary>
      /// <param name="lstr"> Возвращаемый список таблиц </param>
      /// <returns></returns>
      public bool GetNamesTable(ref List<String> lstr, string baseNeme = "", string order_by = "TABLE_NAME")
      {
         if (baseNeme == "") baseNeme = this.nameBase;
         List<String> result = new List<string>();
         if (order_by != "")
         {
            if (order_by.IndexOf("ORDER") == -1)
            {
               order_by = " ORDER BY " + order_by;
            }
         }
         try
         {
            //Список таблиц
            string query = "SELECT TABLE_NAME FROM " + baseNeme + ".information_schema.TABLES WHERE TABLE_TYPE != 'VIEW'" + order_by;

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
         catch (SqlException ex)
         {
            MessErr(ex);
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
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }
      }


      /// <summary>
      /// Устанавливает, обновляет или удаляет (если val==nul) расширенное свойство БД|Таблицы|Колонки. 
      /// </summary>
      /// <param name="name_bas"></param>
      /// <param name="name_tab"></param>
      /// <param name="name_col"></param>
      /// <param name="name_prop"></param>
      /// <param name="val"></param>
      /// <returns></returns>
      public bool SetExtProp(string name_bas = null, string name_tab = null, string name_col = null, string name_prop = "Caption", string val = "")
      {
         bool result = false;
         if ( name_bas == null ) 
            name_bas = this.nameBase;

         if (name_tab == null ) 
            name_tab = "";
         else{
            name_tab = ", @level0type = N'Schema', @level0name = 'dbo'" +
                         ", @level1type = N'Table',  @level1name = '" + name_tab + "'"; }

         if (name_col == null || name_tab == null) 
            name_col = "";
         else{
            name_col = ", @level2type = N'Column',  @level2name = '" + name_col + "'"; }
         
         if (!Open()) return result;
         if (val != null)
         {
            try               // Добавление
            {
               string query = " USE " + name_bas +
                              " EXEC sys.sp_addextendedproperty" +
                              " @name = N'" + name_prop + "'," +
                              " @value = '" + val + "'" + name_tab + name_col;
               SqlCommand command = new SqlCommand(query, Connection);
               command.ExecuteNonQuery();
               command.Dispose();
               result = true;
               Close();
            }
            catch // Обновление
            {
               string query = " USE " + name_bas +
                              " EXEC sys.sp_updateextendedproperty" +
                              " @name = N'" + name_prop + "'," +
                              " @value = '" + val + "'" + name_tab + name_col;
               SqlCommand command = new SqlCommand(query, Connection);
               command.ExecuteNonQuery();
               command.Dispose();
               result = true;
               Close();
            }
         }
         else
         {
            try               // Удаление
            {
               string query = " USE " + name_bas +
                              " EXEC sys.sp_dropextendedproperty" +
                              " @name = N'" + name_prop + "'" + name_tab + name_col;
               SqlCommand command = new SqlCommand(query, Connection);
               command.ExecuteNonQuery();
               command.Dispose();
               result = true;
               Close();
            }
            catch (SqlException ex) {
               MessErr(ex);
               Close();
            }
         }
         return result;
      }


      /// <summary>
      /// Получает значение из расширенных свойств базы, таблицы или колонки 
      /// </summary>
      /// <param name="table_result">Все значения удовлетворяющие запросу</param>
      /// <param name="name_bas">Имя базы данных</param>
      /// <param name="name_tab">Имя таблицы данных</param>
      /// <param name="name_col">Имя столбца данных</param>
      /// <param name="name_prop">Имя расширенного свойства</param>
      /// <returns></returns>
      public string GetExtProp(out DataTable table_result, string name_bas = null, 
                                                           string name_tab = null, 
                                                           string name_col = null, 
                                                           string name_prop = null)
      {
         string result = "";
         string query  = "";
         string WHERE  = "";
         string AND    = "";
         table_result = new DataTable();

         if (name_bas == null || name_bas == "")
            if (this.nameBase != null)
               name_bas = this.nameBase;
            else return result;

         if ( name_col != null )
         {
            if (name_tab != null || name_prop != null)
            {
               WHERE = "WHERE";
               if ( name_tab  != "" && name_tab  != "*" && name_tab  != null) { WHERE = WHERE +       " tbl.name = '"   + name_tab  + "'"; AND = " AND"; }
               if ( name_col  != "" && name_col  != "*" && name_col  != null) { WHERE = WHERE + AND + " clmns.name = '" + name_col + "'"; AND = " AND"; }
               if ( name_prop != "" && name_prop != "*" && name_prop != null) { WHERE = WHERE + AND + " p.name = '"     + name_prop   + "'"; }
               if (WHERE == "WHERE") WHERE = "";
               }

            query = "USE " + name_bas + " " +
                    "SELECT SCHEMA_NAME(tbl.schema_id) AS SchemaName, " +
                    "tbl.name AS TableName, " +
                    "clmns.name AS ColumnName, " +
                    "p.name AS ExtendedPropertyName, " +
                    "CAST(p.value AS sql_variant) AS ExtendedPropertyValue " +
                    "FROM " +
                    "sys.tables AS tbl " +
                    "INNER JOIN sys.all_columns AS clmns ON clmns.object_id = tbl.object_id " +
                    "INNER JOIN sys.extended_properties AS p ON p.major_id = tbl.object_id AND p.minor_id = clmns.column_id AND p.class=1 " +
                    WHERE +
                    "ORDER BY [ExtendedPropertyName] ASC";
         }
         else if ( name_tab != null )
         {
            WHERE = "WHERE";
            if ( name_prop != "" && name_prop != "*" && name_prop != null ) { WHERE = WHERE + " p.name = '" + name_prop + "'"; AND = " AND"; }
            if ( name_tab  != "" && name_tab  != "*" )                      { WHERE = WHERE + AND + " tbl.name = '" + name_tab + "'"; }
            if (WHERE == "WHERE") WHERE = "";

            query = "USE " + name_bas + " " +
                    "SELECT SCHEMA_NAME(tbl.schema_id) AS SchemaName, " +
                    "tbl.name AS TableName, " +
                    "p.name AS ExtendedPropertyName, " +
                    "CAST(p.value AS sql_variant) AS ExtendedPropertyValue " +
                    "FROM " +
                     "sys.tables AS tbl " +
                    "INNER JOIN sys.extended_properties AS p ON p.major_id=tbl.object_id AND p.minor_id=0 AND p.class=1 " +
                    WHERE +
                    "ORDER BY [ExtendedPropertyName] ASC";
         }
         else
         {
            WHERE = "WHERE p.major_id=0 AND p.minor_id=0 AND p.class=0 ";
            if (name_prop != "" &&  name_prop != null && name_prop != "*") { WHERE = WHERE + "AND p.name = '" + name_prop + "'"; }
            if (name_prop == "*") WHERE = "";

            query = "USE " + name_bas + " " +
                    "SELECT DB_NAME() AS DatabaseName, " +
                    "p.name AS ExtendedPropertyName, " +
                    "p.value AS ExtendedPropertyValue " +
                    "FROM " +
                    "sys.extended_properties AS p " +
                    WHERE +
                    "ORDER BY [ExtendedPropertyName] ASC";
         }

         if (!Open()) return result;
         try
         {
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            table_result = ds.Tables[0];
            if(table_result.Rows.Count>0)
               result = table_result.Rows[0]["ExtendedPropertyValue"].ToString();
            Close();
         }
         catch (SqlException ex)
         {
            MessErr(ex);
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
            GetTable(NameTable: name_table, out dataTable, NameBase: nameBase);
            dataSet.Tables.Add(dataTable.Copy());
            result = true;
         }

         return result;
      }


      public bool GetTableStruct(string NameTable,
                                 out DataTable table,
                                 string NameBase = "",
                                 bool MsgYes = true)
      {
         table = new DataTable();
         if (!Open()) return false;
         if (NameBase == null || NameBase == "") NameBase = nameBase;
         if (NameBase != "" && NameBase.IndexOf("[") < 0 && NameBase.IndexOf("]") < 0)
            NameBase = "[" + NameBase + "]";
         try
         {
            string query = "USE "+ NameBase + " SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + NameTable + "'";
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            table = ds.Tables[0];
            //table.TableName = nameTable; // Нельзя делать, теряет связь с базой
            table.Namespace = NameTable;
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            if (MsgYes) MessErr(ex);
            Close();
            return false;
         }
      }
      public enum GetTableAddConf
      {
         AllowDBNull,
         PromKey,
         None
      }
      /// <summary>
      /// Получает данные из таблицы
      /// </summary>
      /// <param name="NameTable"></param>
      /// <param name="table"></param>
      /// <param name="NameBase"></param>
      /// <param name="select"></param>
      /// <param name="where"></param>
      /// <param name="order"></param>
      /// <param name="MsgYes"></param>
      /// <param name="AddConf">Доп запрос в Namespace</param>
      /// <returns></returns>
      public bool GetTable(string NameTable,
                           out DataTable table,
                           string NameBase = "",
                           string select = "SELECT *",
                           string where = "",
                           string order = "",
                           bool MsgYes = true,
                           GetTableAddConf AddConf = GetTableAddConf.None)
      {
         table = null;
         if (!Open()) return false;
         if (NameBase == null || NameBase == "") NameBase = nameBase;
         if (NameBase != "" && NameBase.IndexOf("[") < 0 && NameBase.IndexOf("]") < 0)
            NameBase = "[" + NameBase + "].";
         try
         {
            // Проверка ввода данных
            //select = select.Replace(" ","");
            if (select != "SELECT *")
            {
               if (select == ""|| select==null)
               {
                  select = "SELECT *";
               }
               else if (select.IndexOf("SELECT") == -1)
               {
                  select = "SELECT [" + select + "]";
               }
               if (!select.Contains("] ,") && !select.Contains("],"))
                  select = select.Replace(",", "],[");
            }
            if (where != ""&& where != null)
            {
               if (where.IndexOf("WHERE") == -1)
               {
                  where = "WHERE " + where;
               }
            }
            if (order != "" && order != null)
            {
               if (order.IndexOf("ORDER") == -1)
               {
                  order = " ORDER BY " + order;
               }
            }
            string query = select + " FROM " + NameBase + "[dbo].["+ NameTable + "] " + where + order;
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            table = ds.Tables[0];
            //table.TableName = nameTable; // Нельзя делать, теряет связь с базой
            table.Namespace = NameTable;
            switch(AddConf)
            {
               case GetTableAddConf.AllowDBNull:
                  {
                     DataTable dtStruct;
                     GetTableStruct(NameTable, out dtStruct);
                     foreach (DataColumn column in table.Columns)
                     {
                        column.Namespace = ((Find(dtStruct, "COLUMN_NAME", column.ColumnName))["IS_NULLABLE"].ToString() == "YES").ToString();
                     }
                     break;
                  }
               case GetTableAddConf.PromKey:
                  {
                     string colPK = GetPrimKeyTable(NameTable);
                     foreach (DataColumn column in table.Columns)
                     {
                        if (column.ColumnName == colPK) 
                           column.Namespace = "true";
                        else
                           column.Namespace = "false";
                     }
                     break;
                  }
            }
            
            if (!Close()) return false;
            return true;
         }
         catch(SqlException ex)
         {
            if(MsgYes) MessErr(ex);
            Close();
            return false;
         }
      }

      /// <summary>
      /// Возвращает количество строк в указанной таблице
      /// </summary>
      /// <param name="NameTable"></param>
      /// <param name="NameBase"></param>
      /// <param name="MsgYes"></param>
      /// <returns></returns>
      public int GetCntRowTable(string NameTable,
                           string NameBase = "",
                           bool MsgYes = true)
      {
         if (!Open()) return -1;
         if (NameBase == null || NameBase == "") NameBase = nameBase;
         if (NameBase != "" && NameBase.IndexOf("[") < 0 && NameBase.IndexOf("]") < 0)
            NameBase = "[" + NameBase + "].";
         try
         {
            string query = "SELECT * FROM " + NameBase + "[dbo].[" + NameTable + "]";
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            DataTable table = ds.Tables[0];
            //table.TableName = nameTable; // Нельзя делать, теряет связь с базой
            table.Namespace = NameTable;
            if (!Close()) return -3;
            return table.Rows.Count;
         }
         catch (SqlException ex)
         {
            if (MsgYes) MessErr(ex);
            Close();
            return -2;
         }
      }

      private DataRow Find(DataTable table, string NameColumn, string FindValue)
      {
         DataRow result = null;
         if (table != null)
         {
            foreach (DataRow row in table.Rows)
            {
               if (row[NameColumn]?.ToString() == FindValue)
               {
                  result = row;
                  break;
               }
            }
         }
         return result;
      }
      public bool SetTable()
      {
         if (!Open()) return false;
         try
         {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            //foreach (DataColumn col in ds.Tables[0].Columns) col.AllowDBNull = true;
            adapter.Update(ds);
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            MessErr(ex);
            Close();
            return false;
         }

      }

      /// <summary>
      /// Устанавливает, в переданной таблицы table, 
      /// знаения свойств колонок Namespace, из Extended Properties таблицы, по NameExtProp
      /// </summary>
      /// <param name="table"></param>
      /// <param name="NameExtProp"></param>
      /// <returns></returns>
      public bool SetDataTabColNamespaceFromExtProp(ref DataTable table, string NameExtProp)
      {
         bool result = false;
         DataTable dtExtProp;
         GetExtProp(table_result: out dtExtProp,
                    name_bas: this.nameBase,
                    name_tab: table.Namespace,
                    name_col: "*",
                    name_prop: NameExtProp);
         try
         {
            foreach (DataRow row in dtExtProp.Rows)
            {
               table.Columns[row["ColumnName"].ToString()].Namespace = row["ExtendedPropertyValue"].ToString();
            }
            result = true;
         }
         catch{ }
         
         return result;
      }

      public bool AddDataTable(string nameTable, string[] nameColumns, string[] values, string nameBase = null, bool MsgYes=true)
      {
         string val = " ('" + string.Join("', '", values) + "')";
         string nc = " ([" + string.Join("], [", nameColumns) + "])";
         try
         {
            if (nameBase == null) nameBase = this.nameBase;

            string str = "USE [" + nameBase + "] INSERT INTO [" + nameTable + "]" + nc +
                         " VALUES" + val;

            if (!Open()) return false;
            SqlCommand command = new SqlCommand(str, Connection);

            command.ExecuteNonQuery();
            command.Dispose();
            if (!Close()) return false;
            return true;
         }
         catch (SqlException ex)
         {
            if(MsgYes)MessErr(ex);
            Close();
            return false;
         }
      }

      ///// <summary>
      ///// Наполение строки данными из строкового массива
      ///// </summary>
      ///// <param name="row"> Формат строки </param>
      ///// <param name="sourse"> Данные для вставки </param>
      ///// <param name="fillEmpty"> Включение заполнения пустых </param>
      ///// <param name="fillText"> Текст для заполнения пустых </param>
      ///// <returns></returns>
      //public DataRow SetDataRow (DataRow row, string[] sourse, bool fillEmpty = false, string fillText = "")
      //{
      //   try
      //   {
      //      int count = row.Table.Columns.Count - 1; // Вычитаем столбец ID
      //      if (count < 0) return row;
      //      string[] result = new string[row.Table.Columns.Count];
      //      Str.CopyArrStr(sourse, result, 0, true);
      //      for (int i = 1; i < row.Table.Columns.Count; i++)
      //      {
      //         row[row.Table.Columns[i]] = result[i - 1];
      //      }
      //   }
      //   catch(Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return row;
      //}

      //public DataRow SetDataRow(DataRow row, List<string> sourse, bool fillEmpty = false, string fillText = "")
      //{
      //   try
      //   {
      //      int count = row.Table.Columns.Count - 1; // Вычитаем столбец ID
      //      if (count < 0) return row;
      //      List<string> result = new List<string>(row.Table.Columns.Count);
      //      Str.CopyLstStr(sourse, result, 0, true);
      //      for (int i = 1; i < row.Table.Columns.Count; i++)
      //      {
      //         row[i] = result[i - 1];
      //      }
      //   }
      //   catch(Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return row;
      //}

      ///// <summary>
      ///// Получение списка значений строк из одной колонки таблицы
      ///// </summary>
      ///// <param name="table"> Таблица с данными </param>
      ///// <param name="ColumnNum"> Номер колонки </param>
      ///// <returns></returns>
      //public List<string> GetListRowsStrInOneColumnTable( DataTable table, 
      //                                                 int       ColumnNum  = 0, 
      //                                                 bool      unique      = false, 
      //                                                 string    ColumnName = "",
      //                                                 bool      NotEmpty = true )
      //{
      //   List<string> lstResult = new List<string>();
      //   Dictionary<string, string> dicResult = new Dictionary<string, string>();
      //   try
      //   {
      //      foreach (DataRow Row in table.Rows)
      //      {
      //         if (unique)
      //         {
      //            if (ColumnName == "")
      //            {
      //               if (!dicResult.ContainsKey(Row[ColumnNum].ToString()))
      //               {
      //                  if ( NotEmpty )
      //                  {
      //                     if (Row[ColumnNum].ToString() != "")
      //                     {
      //                        dicResult.Add(Row[ColumnNum].ToString(), "value");
      //                        lstResult.Add(Row[ColumnNum].ToString());
      //                     }
      //                  }
      //                  else
      //                  {
      //                     dicResult.Add(Row[ColumnNum].ToString(), "value");
      //                     lstResult.Add(Row[ColumnNum].ToString());
      //                  }

      //               }
      //            }
      //            else
      //            {
      //               if (!dicResult.ContainsKey(Row[ColumnName].ToString()))
      //               {
      //                  if (NotEmpty)
      //                  {
      //                     if (Row[ColumnName].ToString() != "")
      //                     {
      //                        dicResult.Add(Row[ColumnName].ToString(), "value");
      //                        lstResult.Add(Row[ColumnName].ToString());
      //                     }
      //                  }
      //                  else
      //                  {
      //                     dicResult.Add(Row[ColumnName].ToString(), "value");
      //                     lstResult.Add(Row[ColumnName].ToString());
      //                  }
      //               }
      //            }
      //         }
      //         else
      //         {
      //            if (ColumnName == "")
      //            {
      //               if ( NotEmpty )
      //               {
      //                  if (Row[ColumnNum].ToString() != "")
      //                  {
      //                     lstResult.Add(Row[ColumnNum].ToString());
      //                  }
      //               }
      //               else
      //               {
      //                  lstResult.Add(Row[ColumnNum].ToString());
      //               }
      //            }
      //            else
      //            {
      //               if (NotEmpty)
      //               {
      //                  if (Row[ColumnName].ToString() != "")
      //                  {
      //                     lstResult.Add(Row[ColumnName].ToString());
      //                  }
      //               }
      //               else
      //               {
      //                  lstResult.Add(Row[ColumnName].ToString());
      //               }
      //            }
      //         }
      //      }
      //   }
      //   catch(Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return lstResult;
      //}

      //public List<int> GetListRowsIntInOneColumnTable(DataTable table,
      //                                                 int ColumnNum = 0,
      //                                                 bool unique = false,
      //                                                 string ColumnName = "",
      //                                                 bool NotEmpty = true)
      //{
      //   List<int> lstResult = new List<int>();
      //   Dictionary<string, string> dicResult = new Dictionary<string, string>();
      //   try
      //   {
      //      foreach (DataRow Row in table.Rows)
      //      {
      //         if (unique)
      //         {
      //            if (ColumnName == "")
      //            {
      //               if (!dicResult.ContainsKey(Row[ColumnNum].ToString()))
      //               {
      //                  if (NotEmpty)
      //                  {
      //                     if (Row[ColumnNum].ToString() != "")
      //                     {
      //                        dicResult.Add(Row[ColumnNum].ToString(), "value");
      //                        lstResult.Add(int.Parse(Row[ColumnNum].ToString()));
      //                     }
      //                  }
      //                  else
      //                  {
      //                     dicResult.Add(Row[ColumnNum].ToString(), "value");
      //                     lstResult.Add(int.Parse(Row[ColumnNum].ToString()));
      //                  }

      //               }
      //            }
      //            else
      //            {
      //               if (!dicResult.ContainsKey(Row[ColumnName].ToString()))
      //               {
      //                  if (NotEmpty)
      //                  {
      //                     if (Row[ColumnName].ToString() != "")
      //                     {
      //                        dicResult.Add(Row[ColumnName].ToString(), "value");
      //                        lstResult.Add(int.Parse(Row[ColumnName].ToString()));
      //                     }
      //                  }
      //                  else
      //                  {
      //                     dicResult.Add(Row[ColumnName].ToString(), "value");
      //                     lstResult.Add(int.Parse(Row[ColumnName].ToString()));
      //                  }
      //               }
      //            }
      //         }
      //         else
      //         {
      //            if (ColumnName == "")
      //            {
      //               if (NotEmpty)
      //               {
      //                  if (Row[ColumnNum].ToString() != "")
      //                  {
      //                     lstResult.Add(int.Parse(Row[ColumnNum].ToString()));
      //                  }
      //               }
      //               else
      //               {
      //                  lstResult.Add(int.Parse(Row[ColumnNum].ToString()));
      //               }
      //            }
      //            else
      //            {
      //               if (NotEmpty)
      //               {
      //                  if (Row[ColumnName].ToString() != "")
      //                  {
      //                     lstResult.Add(int.Parse(Row[ColumnName].ToString()));
      //                  }
      //               }
      //               else
      //               {
      //                  lstResult.Add(int.Parse(Row[ColumnName].ToString()));
      //               }
      //            }
      //         }
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return lstResult;
      //}

      //public List<string[]> GetListRowsFromTable(DataTable table,
      //                                                 int[] AddColNum =null,
      //                                                 string[] AddColName =null)
      //{
      //   List<string[]> lstResult = new List<string[]>();
      //   if (AddColNum  == null && AddColName  == null) return lstResult;
      //   try
      //   {
      //      bool ByName = AddColName ?.Length > 0 ? true : false;
      //      string[] tempReselats = new string[ByName ? AddColName .Length : AddColNum .Length];

      //      foreach (DataRow Row in table.Rows)
      //      {
      //         for( int i = 0; i < tempReselats.Length; i++)
      //         {
      //            tempReselats[i] = ByName ? Row[AddColName [i]].ToString() : Row[AddColNum [i]].ToString();
      //         }
      //         lstResult.Add((string[])tempReselats.Clone());
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return lstResult;
      //}
      //public List<string[]> GetListRowsFromTable(DataGridView table,
      //                                                 int[] AddColNum=null,
      //                                                 string[] AddColName=null,
      //                                                 string[] ExcColName=null,
      //                                                 int DataYes = 0,
      //                                                 bool NotNull = true)
      //{
      //   List<string[]> lstResult = new List<string[]>();
      //   try{
      //      bool ByName = false;
      //      bool AllCol = false;
      //      string[] strNotNull;
      //      string[] tempReselats;
      //      if (AddColNum == null & AddColName == null) 
      //         AllCol = true; // Все ячейки строки для копирования
      //      else
      //         ByName = AddColName?.Length > 0 ? true : false;
      //      int take = ExcColName == null ? 0 : ExcColName.Length;
      //      // Определение индексов столбиков для исключения
      //      int[] ExcColIdx = new int[take];
      //      int n = 0;
      //      if (ExcColIdx.Length > 0)
      //      {
      //         foreach (string ColName in ExcColName)
      //         { ExcColIdx[n] = table.Columns[ColName].Index; n++; }
      //      }
      //      // Определение индексов столбиков для чтения
      //      int[] ReadColIdx = new int[table.Columns.Count - take];
      //      n = 0;
      //      string strExcColIdx = Misc.ArrIntToString(ExcColIdx);
      //      foreach (DataGridViewColumn column in table.Columns)
      //      {
      //         if (!strExcColIdx.Contains(column.Index.ToString() + ","))
      //         { ReadColIdx[n] = column.Index; n++; }
      //      }

      //      tempReselats = new string[ByName ? AddColName.Length : (AllCol ? table.Columns.Count - take : AddColNum.Length)];

      //      foreach (DataGridViewRow Row in table.Rows){
      //         // Копирование из строки заданных ячеек в темповый массив
      //         for (int i = 0; i < tempReselats.Length; i++)
      //         {
      //            tempReselats[i] = ByName ? Row.Cells[AddColName[i]].Value?.ToString() : (AllCol ? Row.Cells[ReadColIdx[i]].Value?.ToString() : Row.Cells[AddColNum[i]].Value?.ToString());
      //         }
      //            // Все ячейки в строке должны иметь значения для копирования
      //         if (DataYes==0){
      //            bool Add = true;
      //            foreach(string data in tempReselats) if (data == null || data == "") Add = false;
      //            if (Add)
      //            {
      //               if (NotNull){
      //                  strNotNull = Str.CopyArrStr(source: tempReselats, NotNull: true);
      //                  lstResult.Add((string[])strNotNull.Clone());
      //               }
      //               else
      //                  lstResult.Add((string[])tempReselats.Clone());
      //            }
      //         }
      //         // Определённое количество ячейк в строке должны иметь значения для копирования
      //         else if (DataYes>0){
      //            int Add = DataYes;
      //            foreach (string data in tempReselats) if (data != null & data != "") Add--;
      //            if (Add<=0)
      //            {
      //               if (NotNull)
      //               {
      //                  strNotNull = Str.CopyArrStr(source: tempReselats, NotNull: true);
      //                  lstResult.Add((string[])strNotNull.Clone());
      //               }
      //               else
      //                  lstResult.Add((string[])tempReselats.Clone());
      //            }
      //         }
      //         // Все строки копируются не зависимо от содержания ячеек
      //         else
      //         {
      //            if (NotNull)
      //            {
      //               strNotNull = Str.CopyArrStr(source: tempReselats, NotNull: true);
      //               lstResult.Add((string[])strNotNull.Clone());
      //            }
      //            else
      //               lstResult.Add((string[])tempReselats.Clone());
      //         }
      //      }
      //   }
      //   catch (Exception ex){
      //      MessErr(ex);
      //   }
      //   return lstResult;
      //}

      ///// <summary>
      ///// Получение списка значений строк из трёх колонок таблицы
      ///// </summary>
      ///// <param name="table"> Таблица с данными </param>
      ///// <param name="ColumnNum1"> Номер колонки </param>
      ///// <returns></returns>
      //public List<string[]> GetListRowsInThreeColumnTable(DataTable table,
      //                                                 int ColumnNum1 = 0,
      //                                                 int ColumnNum2 = 1,
      //                                                 int ColumnNum3 = 2,
      //                                                 string ColumnName1 = "",
      //                                                 string ColumnName2 = "",
      //                                                 string ColumnName3 = "",
      //                                                 bool unique = false,
      //                                                 bool NotEmpty = true)
      //{
      //   List<string[]> lstResult = new List<string[]>();
      //   Dictionary<string, string> dicResult = new Dictionary<string, string>();
      //   try
      //   {
      //      foreach (DataRow Row in table.Rows)
      //      {
      //         if (unique)
      //         {
      //            if (ColumnName1 == "")
      //            {
      //               if (!dicResult.ContainsKey(Row[ColumnNum1].ToString()))
      //               {
      //                  if (NotEmpty)
      //                  {
      //                     if (Row[ColumnNum1].ToString() != "")
      //                     {
      //                        dicResult.Add(Row[ColumnNum1].ToString(), "value");
      //                        lstResult.Add(new string[] { Row[ColumnNum1].ToString(), 
      //                                                     Row[ColumnNum2].ToString(), 
      //                                                     Row[ColumnNum3].ToString() });
      //                     }
      //                  }
      //                  else
      //                  {
      //                     dicResult.Add(Row[ColumnNum1].ToString(), "value");
      //                     lstResult.Add(new string[] { Row[ColumnNum1].ToString(), 
      //                                                  Row[ColumnNum2].ToString(), 
      //                                                  Row[ColumnNum3].ToString() });
      //                  }

      //               }
      //            }
      //            else
      //            {
      //               if (!dicResult.ContainsKey(Row[ColumnName1].ToString()))
      //               {
      //                  if (NotEmpty)
      //                  {
      //                     if (Row[ColumnName1].ToString() != "")
      //                     {
      //                        dicResult.Add(Row[ColumnName1].ToString(), "value");
      //                        lstResult.Add(new string[] { Row[ColumnName1].ToString(), 
      //                                                     Row[ColumnName2].ToString(),
      //                                                     Row[ColumnName3].ToString() });
      //                     }
      //                  }
      //                  else
      //                  {
      //                     dicResult.Add(Row[ColumnName1].ToString(), "value");
      //                     lstResult.Add(new string[] { Row[ColumnName1].ToString(), 
      //                                                  Row[ColumnName2].ToString(),
      //                                                  Row[ColumnName3].ToString() });
      //                  }
      //               }
      //            }
      //         }
      //         else
      //         {
      //            if (ColumnName1 == "")
      //            {
      //               if (NotEmpty)
      //               {
      //                  if (Row[ColumnNum1].ToString() != "")
      //                  {
      //                     lstResult.Add(new string[] { Row[ColumnNum1].ToString(), 
      //                                                  Row[ColumnNum2].ToString(),
      //                                                  Row[ColumnNum3].ToString() });
      //                  }
      //               }
      //               else
      //               {
      //                  lstResult.Add(new string[] { Row[ColumnNum1].ToString(), 
      //                                               Row[ColumnNum2].ToString(),
      //                                               Row[ColumnNum3].ToString() });
      //               }
      //            }
      //            else
      //            {
      //               if (NotEmpty)
      //               {
      //                  if (Row[ColumnName1].ToString() != "")
      //                  {
      //                     lstResult.Add(new string[] { Row[ColumnName1].ToString(), 
      //                                                  Row[ColumnName2].ToString(),
      //                                                  Row[ColumnName3].ToString() });
      //                  }
      //               }
      //               else
      //               {
      //                  lstResult.Add(new string[] { Row[ColumnName1].ToString(), 
      //                                               Row[ColumnName2].ToString(),
      //                                               Row[ColumnName3].ToString() });
      //               }
      //            }
      //         }
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return lstResult;
      //}

      ///// <summary>
      ///// Получение списка значений строк из двух колонок таблицы
      ///// </summary>
      ///// <param name="table"> Таблица с данными </param>
      ///// <param name="ColumnNum1"> Номер колонки </param>
      ///// <returns></returns>
      //public List<string[]> GetListRowsInTwoColumnTable(DataTable table,
      //                                                 int ColumnNum1 = 0,
      //                                                 int ColumnNum2 = 1,
      //                                                 bool unique = false,
      //                                                 string ColumnName1 = "",
      //                                                 string ColumnName2 = "",
      //                                                 bool NotEmpty = true)
      //{
      //   List<string[]> lstResult = new List<string[]>();
      //   Dictionary<string, string> dicResult = new Dictionary<string, string>();
      //   try
      //   {
      //      foreach (DataRow Row in table.Rows)
      //      {
      //         if (unique)
      //         {
      //            if (ColumnName1 == "")
      //            {
      //               if (!dicResult.ContainsKey(Row[ColumnNum1].ToString()))
      //               {
      //                  if (NotEmpty)
      //                  {
      //                     if (Row[ColumnNum1].ToString() != "")
      //                     {
      //                        dicResult.Add(Row[ColumnNum1].ToString(), "value");
      //                        lstResult.Add(new string[] { Row[ColumnNum1].ToString(), 
      //                                                     Row[ColumnNum2].ToString() });
      //                     }
      //                  }
      //                  else
      //                  {
      //                     dicResult.Add(Row[ColumnNum1].ToString(), "value");
      //                     lstResult.Add(new string[] { Row[ColumnNum1].ToString(), 
      //                                                  Row[ColumnNum2].ToString() });
      //                  }

      //               }
      //            }
      //            else
      //            {
      //               if (!dicResult.ContainsKey(Row[ColumnName1].ToString()))
      //               {
      //                  if (NotEmpty)
      //                  {
      //                     if (Row[ColumnName1].ToString() != "")
      //                     {
      //                        dicResult.Add(Row[ColumnName1].ToString(), "value");
      //                        lstResult.Add(new string[] { Row[ColumnName1].ToString(), 
      //                                                     Row[ColumnName2].ToString() });
      //                     }
      //                  }
      //                  else
      //                  {
      //                     dicResult.Add(Row[ColumnName1].ToString(), "value");
      //                     lstResult.Add(new string[] { Row[ColumnName1].ToString(), 
      //                                                  Row[ColumnName2].ToString() });
      //                  }
      //               }
      //            }
      //         }
      //         else
      //         {
      //            if (ColumnName1 == "")
      //            {
      //               if (NotEmpty)
      //               {
      //                  if (Row[ColumnNum1].ToString() != "")
      //                  {
      //                     lstResult.Add(new string[] { Row[ColumnNum1].ToString(), 
      //                                                  Row[ColumnNum2].ToString() });
      //                  }
      //               }
      //               else
      //               {
      //                  lstResult.Add(new string[] { Row[ColumnNum1].ToString(), 
      //                                               Row[ColumnNum2].ToString() });
      //               }
      //            }
      //            else
      //            {
      //               if (NotEmpty)
      //               {
      //                  if (Row[ColumnName1].ToString() != "")
      //                  {
      //                     lstResult.Add(new string[] { Row[ColumnName1].ToString(), 
      //                                                  Row[ColumnName2].ToString() });
      //                  }
      //               }
      //               else
      //               {
      //                  lstResult.Add(new string[] { Row[ColumnName1].ToString(), 
      //                                               Row[ColumnName2].ToString() });
      //               }
      //            }
      //         }
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return lstResult;
      //}

      //public List<string> GetListRowsInOneColumnTable(DataTable table, string name, bool unique = false)
      //{
      //   List<string> lstResult = new List<string>();
      //   Dictionary<string, string> dicResult = new Dictionary<string, string>();
      //   try
      //   {
      //      foreach (DataRow Row in table.Rows)
      //      {
      //         if (unique)
      //         {
      //            if (!dicResult.ContainsKey(Row[name].ToString()))
      //            {
      //               dicResult.Add(Row[name].ToString(), "value");
      //               lstResult.Add(Row[name].ToString());
      //            }
      //         }
      //         else
      //         {
      //            lstResult.Add(Row[name].ToString());
      //         }
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return lstResult;
      //}

      ///// <summary>
      ///// Наполнение словаря данными из таблицы
      ///// </summary>
      ///// <param name="table">Таблица</param>
      ///// <param name="num">Номер столбца для ключа</param>
      ///// <param name="num2">Номер столбца для значения</param>
      ///// <param name="num3">Номер столбца для доп данных для замены символа</param>
      ///// <param name="find_text">Символы для замены</param>
      ///// <returns></returns>
      //public Dictionary<string,string> GetListRowsInOneColumnTable(DataTable table, int num, int num2, int num3 = 5, string find_text = "[]")
      //{
      //   Dictionary<string, string> dicResult = new Dictionary<string, string>();
      //   try
      //   {
      //      foreach (DataRow Row in table.Rows)
      //      {
      //         if (!dicResult.ContainsKey(Row[num].ToString()))
      //            dicResult.Add(Row[num].ToString(), Row[num2].ToString().IndexOf(find_text)>0 ? Row[num2].ToString().Replace("[]", "[" + Row[num3].ToString() + "]") : Row[num2].ToString());
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return dicResult;
      //}
      ///// <summary>
      ///// Получение списка значений столбцов из одной строки таблицы
      ///// </summary>
      ///// <param name="table"></param>
      ///// <param name="num"></param>
      ///// <returns></returns>
      //public List<string> GetListColumnsInOneRowTable(DataTable table, int num, bool unique = false)
      //{
      //   List<string> lstResult = new List<string>();
      //   Dictionary<string, string> dicResult = new Dictionary<string, string>();
      //   try
      //   {
      //      foreach (DataColumn Col in table.Columns)
      //      {
      //         if (unique)
      //         {
      //            if (!dicResult.ContainsKey(Col.Table.Rows[num].ToString()))
      //            {
      //               dicResult.Add(Col.Table.Rows[num].ToString(), "value");
      //               lstResult.Add(Col.Table.Rows[num].ToString());
      //            }
      //         }
      //         else
      //         {
      //            lstResult.Add(Col.Table.Rows[num].ToString());
      //         }
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return lstResult;
      //}

      ///// <summary>
      ///// Получает массив значений из row если любая cell == FindVal. 
      ///// NumColGetNull: 
      ///// "больше || = 0" - index column null-value,
      ///// "=-1" - all null-value,
      ///// "меньше-1" - not null-value.
      ///// Если null-value ограничен(меньше -1 или больше или = 0) результирующий массив сжимается
      ///// </summary>
      ///// <param name="table"></param>
      ///// <param name="FindVal"></param>
      ///// <param name="GetNumColNull "></param>
      ///// <returns></returns>
      //public string[] GetListColumnsInOneRowTable(DataGridView table, string FindVal, int GetNumColNull =-1, int GetStartCol = 0)
      //{
      //   List<string> lstResult = new List<string>();
      //   try
      //   {
      //      foreach(DataGridViewRow row in table.Rows)
      //      {
      //         foreach(DataGridViewCell cell in row.Cells)
      //         {
      //            if(cell.Value?.ToString()== FindVal)
      //            {
      //               foreach(DataGridViewCell cellResult in row.Cells)
      //               {
      //                  if (cellResult.ColumnIndex < GetStartCol) continue;
      //                  if (GetNumColNull  !=-1 || cellResult.Value != null)
      //                  { 
      //                     if (cellResult.ColumnIndex == GetNumColNull  || cellResult.Value != null)
      //                        lstResult.Add(cellResult.Value == null ? null : cellResult.Value.ToString()); 
      //                  }
      //                  else if ((GetNumColNull  == -1))
      //                     lstResult.Add(cellResult.Value == null ? null : cellResult.Value.ToString());
      //               }
      //            }
      //         }
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return lstResult.ToArray();
      //}

      //public List<string> GetListColumnsInOneRowTable(List<List<string>> table, int num, bool unique = false)
      //{
      //   List<string> lstResult = new List<string>();
      //   Dictionary<string, string> dicResult = new Dictionary<string, string>();
      //   try
      //   {
      //      for (int i = 0; i < table.Count; i++)
      //      {
      //         if (unique)
      //         {
      //            if (!dicResult.ContainsKey(table[i][num].ToString()))
      //            {
      //               dicResult.Add(table[i][num].ToString(), "value");
      //               lstResult.Add(table[i][num].ToString());
      //            }
      //         }
      //         else
      //         {
      //            lstResult.Add(table[i][num].ToString());
      //         }
      //      }
      //   }
      //   catch (Exception ex)
      //   {
      //      MessErr(ex);
      //   }
      //   return lstResult;
      //}

      public bool AvailabTable(string NameTable, string NameBase = "", bool MsgYes =true)
      {
         if (!Open()) return false;
         if ( NameBase == "" ) NameBase = nameBase;
         try
         {
            string query = "USE [" + nameBase + "] IF OBJECT_ID (N'" + NameTable + "', N'U') IS NOT NULL" +
                       " SELECT 1 AS res ELSE SELECT 0 AS res;";
            // Создаем объект DataAdapter
            adapter = new SqlDataAdapter(query, Connection);
            // Создаем объект Dataset
            ds = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            DataTable table = ds.Tables[0];
            //table.TableName = nameTable; // Нельзя делать, теряет связь с базой
            table.Namespace = NameTable;
            if (!Close()) return false;
            return int.Parse(table.Rows[0]["res"].ToString()) == 1;
         }
         catch (SqlException ex)
         {
            if (MsgYes) MessErr(ex);
            Close();
            return false;
         }
      }
   }    
}
