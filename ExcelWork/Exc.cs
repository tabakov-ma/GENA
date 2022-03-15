using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using WorkString;

namespace ExcelWork
{
    /// <summary>
    /// Класс для работы с Excel
    /// </summary>
    public class Exc
    {
      public Exc()
      {  }
      public static List<List<string>> ListListData;
      public static string ResultGetDataStr = "";
      private static string Patch = "";
      private static int NumSheet = 1;
      private static int NumColumnStar = 0;
      private static int NumColumnEnd = 0;
      private static int NumRowStar = 0;
      private static int NumRowEnd = 0;
      /// <summary>
      /// Количество обработанных ячеек от max до 0
      /// </summary>
      public static int CellCount = 0;
      private static bool endGetDataStr = false;

      public delegate void EndLoadData();
      public static event EndLoadData EndLoad;              // Определение события



      /// <summary>
      /// Получение данных из листа Excel 
      /// </summary>
      /// <param name="patch"> Путь к файлу </param>
      /// <param name="NumSheet"> Номер листа </param>
      /// <param name="NumColumnStar"></param>
      /// <param name="NumColumnEnd"></param>
      /// <param name="NumRowStar"></param>
      /// <param name="NumRowEnd"></param>
      /// <returns></returns>
      public static async void GetDataStr(string patch, int numSheet = 1, int numColumnStar = 0, int numColumnEnd = 0, int numRowStar = 0, int numRowEnd = 0)
      {
         ListListData = new List<List<string>>();
         Patch = patch; NumSheet = numSheet; NumColumnStar = numColumnStar; NumColumnEnd = numColumnEnd; NumRowStar = numRowStar; NumRowEnd = numRowEnd;
         await Task.Run(()=>getDataStr());
      }
      public static  void EndGetDataStr()
      { endGetDataStr = true; }
      private static void getDataStr()
      {
         try
         {
            Excel.Application ObjWorkExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(Patch);
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[NumSheet]; // лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//последнюю ячейку
            // размеры базы
            int lastColumn = (int)lastCell.Column;
            int lastRow = (int)lastCell.Row;
            if (NumColumnEnd != 0) lastColumn = NumColumnEnd;
            if (NumRowEnd != 0) lastRow = NumRowEnd;
            CellCount = (lastColumn - NumColumnStar) * (lastRow - NumRowStar);
            // Перенос данных 
            for (int j = NumColumnStar; j < lastColumn; j++) //по всем колонкам
            {
               ListListData.Add(new List<string>());
               for (int i = NumRowStar; i < lastRow; i++) // по всем строкам
               {
                  ListListData[j].Add(ObjWorkSheet.Cells[i + 1, j + 1].Text.ToString()); //считываем данные
                  CellCount--;
                  if (endGetDataStr)
                  {
                     endGetDataStr = false; return;
                  }
               }
            }
            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из Excel
            GC.Collect(); // убрать за собой
            EndLoad?.Invoke();
            ResultGetDataStr = "OK";
         }
         catch (Exception ex)
         { ResultGetDataStr = ex.Message; }
      }
      public static List<string> GetRowInListListData(int num)
      {
         if (ListListData == null) return null;
         List<string> result = new List<string>();
         for(int i=0; i<ListListData.Count; i++)
         {
            result.Add(ListListData[i][num]);
         }
         return result;
      }
   }
}
