using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloApp
{
   class Program
   {
      static void Main(string[] args)
      {
         var tuple = (val:5, nal2:10);
         Console.WriteLine(tuple.nal2); // 5
         Console.WriteLine(tuple.Item2); // 10
         tuple.Item1 += 26;
         Console.WriteLine(tuple.Item1); // 31
         Console.Read();
      }
   }
}