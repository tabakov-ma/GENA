
using System.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RequestSQL
{
    public class SQL
    {
    private string StrConnect;
    public int ConEnd = 0;
      public SQL(string strConnect)
      {
      StrConnect = strConnect;
      }

      public async void TestConnect()
      {
      // Создание подключения
      SqlConnection connection = new SqlConnection(StrConnect);
      try
      {
        // Открываем подключение
        await connection.OpenAsync();
        ConEnd = 1;
        MessageBox.Show("Подключение открыто.");
        
      }
      catch (SqlException ex)
      {
        ConEnd = 2;
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
    }
}
