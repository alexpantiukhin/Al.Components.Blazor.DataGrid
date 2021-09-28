using System.Collections.Generic;

namespace Al.Components.Blazor.AlDataGrid
{
    /// <summary>
    /// Расширение для упорядоченной коллекции
    /// </summary>
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Возвращает все предыдущие элементы последовательности относительно указанного
        /// </summary>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <param name="node">Узел последовательност</param>
        /// <returns>Список узлов последовательности</returns>
        public static IEnumerable<LinkedListNode<T>> GetPreviousElements<T>(this LinkedListNode<T> node)
        {
            //var result = new List<LinkedListNode<T>>();

            while(node.Previous != null)
            {
                var item = node.Previous;
                node = node.Previous;

                yield return item;

                //result.Add(node.Previous);

                //node = node.Previous;
            }

            //return result;
        }

        /// <summary>
        /// Возвращает все следующие елементы последовательности относительно указанного
        /// </summary>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <param name="node">Узел последовательност</param>
        /// <returns>Список узлов последовательности</returns>
        public static IEnumerable<LinkedListNode<T>> GetNextElements<T>(this LinkedListNode<T> node)
        {
            //var result = new List<LinkedListNode<T>>();

            while (node.Next != null)
            {
                var item = node.Next;
                node = node.Next;

                yield return item;
                //result.Add(node.Next);

                //node = node.Next;
            }

            //return result;
        }
    }
}
