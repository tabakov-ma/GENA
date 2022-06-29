using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkStruct
{
    public class Struct
    {
      #region Tree
      // ====================================
      /// <summary>
      /// Горизонтальный уровень дерева данных
      /// </summary>
      public struct TreeLayer : ICloneable
      {
         public List<Dictionary<string, string>> Nodes;
         public int NodeSelector;

         public object Clone()
         {
            return new TreeLayer(Nodes, NodeSelector);
         }

         public TreeLayer(List<Dictionary<string,string>> Nodes, int CurrNode)
         {
            this.Nodes = Nodes.GetRange(0, Nodes.Count);
            NodeSelector = CurrNode;
         }
      }
      /// <summary>
      /// Динамическая структура дерева данных 
      /// </summary>
      public struct DynamicStructTreeData
      {
         public const string NAME = "NAME";
         public const string TYPE = "TYPE";
         public const string SIZE = "ARRAY_SIZE";
         public List<TreeLayer> Layers;
         public List<Dictionary<string, string>> ResultNodes;
         public int LayerSelector;
         public Dictionary<string, string> NodeCurr;
         /// <summary>
         /// Копирует на текущем (LayerSelector) уровне текущий (NodeSelector) узел данных
         /// </summary>
         /// <returns></returns>
         public bool NodeCopy(bool InResult = false)
         {
            bool result = false;
            if ( ( Layers.Count > 0 ) &&
                 ( LayerSelector >= 0) & (LayerSelector < Layers.Count) &
                 ( Layers[LayerSelector].NodeSelector >= 0) & 
                 ( Layers[LayerSelector].NodeSelector < Layers[LayerSelector].Nodes.Count) )
            {
               string FullPath = "";
               Dictionary<string, string> EditNode = Layers[LayerSelector].Nodes[Layers[LayerSelector].NodeSelector];
               EditNode[TYPE] = EditNode[TYPE].Replace("[", "").Replace("]", "");

               if (InResult) // Результирующая информация о узлах
               {
                  // Инициализация
                  if (ResultNodes == null) ResultNodes = new List<Dictionary<string, string>>();
                  // Построение полного пути вложенности в имени
                  if ( LayerSelector > 0)
                  {

                     for (int i = 1; i < Layers.Count-1; i++)
                     {
                        FullPath = FullPath + Layers[i].Nodes[Layers[i].NodeSelector][NAME] + ".";
                     }
                     EditNode[NAME] = FullPath + EditNode[NAME];

                     //Layers[LayerSelector].Nodes[Layers[LayerSelector].NodeSelector] = EditNode;
                  }

                  //ResultNodes.Add( new Dictionary<string, string>(Layers[LayerSelector].Nodes[Layers[LayerSelector].NodeSelector] ));
                  //EditNode[NAME] = EditNode[NAME].Replace(".", "_");
                  ResultNodes.Add(new Dictionary<string, string>(EditNode));
               }
               else
               {
                  //NodeCurr = Layers[LayerSelector].Nodes[Layers[LayerSelector].NodeSelector];
                  NodeCurr = EditNode;
               }
               result = true;
            }
            return result;
         }
         /// <summary>
         /// Переключить на текущем уровне узел ВПРАВО и скопировать в NodeCurr
         /// возвращает true если переключился
         /// </summary>
         /// <returns></returns>
         public bool NodeSelectRight()
         {
            bool result = false;
            TreeLayer layer = (TreeLayer)Layers[LayerSelector].Clone();
            if (layer.NodeSelector < layer.Nodes.Count-1)
            {
               layer.NodeSelector++;
               Layers[LayerSelector] = (TreeLayer)layer.Clone();
               NodeCopy();
               result = true;
            }
            return result;
         }

         /// <summary>
         /// Переключить текущей уровень ВВЕРХ и скопировать в NodeCurr
         /// возвращает true если переключился
         /// </summary>
         /// <param name="Layer"></param>
         /// <returns></returns>
         public bool LayerSelectUp(TreeLayer Layer)
         {
            bool result = false;
            //if ( Layer.Nodes.Count > 0 )
            //{
               LayerSelector++;
               Layers.Add(Layer);
               NodeCopy();
               result = true;
            //}
            //else
            //{
            //   NodeCopy(InResult: true);
            //}
            return result;
         }

         /// <summary>
         /// Переключить текущей уровень ВНИЗ и скопировать в NodeCurr
         /// возвращает true если переключился
         /// </summary>
         /// <returns></returns>
         public bool LayerSelectDown()
         {
            bool result = false;
            for (; ; )
            {
               if (LayerSelector > 0)
               {
                  Layers.RemoveAt(LayerSelector);
                  LayerSelector--;
                  if (Layers[LayerSelector].NodeSelector < Layers[LayerSelector].Nodes.Count-1)
                  {
                     result = true;
                     break;
                  }
               }
               else
               {
                  result = false;
                  break;
               }
            }
            return result;
         }

      }
      // ====================================
      #endregion Tree

   }
}
