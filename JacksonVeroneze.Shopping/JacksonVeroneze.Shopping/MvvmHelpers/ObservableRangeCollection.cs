using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace JacksonVeroneze.Shopping.MvvmHelpers
{
    //
    // Summary:
    //     Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed. 
    //
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        //
        // Summary:
        //     Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class. 
        //
        public ObservableRangeCollection() : base() { }

        //
        // Summary:
        //     Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class. 
        //
        // Parameters:
        //   collection:
        //     The collection param.
        //
        public ObservableRangeCollection(IEnumerable<T> collection) : base(collection) { }

        //
        // Summary:
        //     Adds the elements of the specified collection to the end of the ObservableCollection(Of T). 
        //
        // Parameters:
        //   collection:
        //     The collection param.
        //
        //   notificationMode:
        //     The notificationMode param.
        //
        public void AddRange(IEnumerable<T> collection, NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
        {
            if (notificationMode != NotifyCollectionChangedAction.Add && notificationMode != NotifyCollectionChangedAction.Reset)
                throw new ArgumentException("Mode must be either Add or Reset for AddRange.", nameof(notificationMode));

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                var raiseEvents = true;

                foreach (var i in collection)
                {
                    Items.Add(i);
                    raiseEvents = true;
                }

                if (raiseEvents)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }

                return;
            }

            var startIndex = Count;
            var changedItems = collection is List<T> ? (List<T>)collection : new List<T>(collection);

            foreach (var i in changedItems)
                Items.Add(i);

            if (changedItems.Count == 0)
                return;

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startIndex));
        }

        //
        // Summary:
        //     Removes the first occurence of each item in the specified collection from ObservableCollection(Of T). NOTE: with notificationMode = Remove, removed items starting index is not set because items are not guaranteed to be consecutive.
        //
        // Parameters:
        //   collection:
        //     The collection param.
        //
        //   notificationMode:
        //     The notificationMode param.
        //
        public void RemoveRange(IEnumerable<T> collection, NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Reset)
        {
            if (notificationMode != NotifyCollectionChangedAction.Remove && notificationMode != NotifyCollectionChangedAction.Reset)
                throw new ArgumentException("Mode must be either Remove or Reset for RemoveRange.", nameof(notificationMode));

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                var raiseEvents = false;
                foreach (var i in collection)
                {
                    Items.Remove(i);
                    raiseEvents = true;
                }

                if (raiseEvents)
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                return;
            }

            var changedItems = collection is List<T> ? (List<T>)collection : new List<T>(collection);

            for (var i = 0; i < changedItems.Count; i++)
            {
                if (!Items.Remove(changedItems[i]))
                {
                    changedItems.RemoveAt(i);
                    i--;
                }
            }

            if (changedItems.Count == 0)
                return;

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItems, -1));
        }

        //
        // Summary:
        //     Clears the current collection and replaces it with the specified item. 
        //
        // Parameters:
        //   item:
        //     The item param.
        //
        public void Replace(T item) => ReplaceRange(new T[] { item });

        //
        // Summary:
        //     Clears the current collection and replaces it with the specified collection. 
        //
        // Parameters:
        //   collection:
        //     The collection param.
        //
        public void ReplaceRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            Items.Clear();

            AddRange(collection, NotifyCollectionChangedAction.Reset);
        }
    }
}