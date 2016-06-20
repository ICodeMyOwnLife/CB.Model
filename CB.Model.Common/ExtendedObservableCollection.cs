using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;


namespace CB.Model.Common
{
    public class ExtendedObservableCollection<T>: ObservableCollection<T>
    {
        #region Methods
        public void AddRange(params T[] collection)
            => AddRange((IEnumerable<T>)collection);

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            AddItems(collection);
            RaiseResetCollectionChanged();
        }

        public void ReplaceRange(params T[] collection)
            => ReplaceRange((IEnumerable<T>)collection);

        public void ReplaceRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            Items.Clear();
            AddItems(collection);
            RaiseResetCollectionChanged();
        }

        public void SortBy<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
            => ReplaceRange(Items.OrderBy(keySelector, comparer).ToList());

        public void SortBy<TKey>(Func<T, TKey> keySelector)
            => SortBy(keySelector, null);

        public void SortByDescending<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
            => ReplaceRange(Items.OrderByDescending(keySelector, comparer).ToList());

        public void SortByDescending<TKey>(Func<T, TKey> keySelector)
            => SortByDescending(keySelector, null);
        #endregion


        #region Implementation
        private void AddItems(IEnumerable<T> collection)
        {
            foreach (var item in collection) Items.Add(item);
        }

        private void RaiseResetCollectionChanged()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion
    }
}