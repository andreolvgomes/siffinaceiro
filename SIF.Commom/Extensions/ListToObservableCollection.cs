using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Classe responsável por covnerter um List em ObservableCollection
    /// </summary>
    public static class ListToObservableCollection
    {
        /// <summary>
        /// Converte List para ObservableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            ObservableCollection<T> observableCollection = new ObservableCollection<T>();
            foreach (T item in list)
                observableCollection.Add(item);
            return observableCollection;
        }
    }
}
