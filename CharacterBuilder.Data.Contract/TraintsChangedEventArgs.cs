using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CharacterBuilder.Data
{
    /// <summary>
    ///  Provides data for the <see cref="ICategorizedTraitContainer.TraitsChangedEvent" /> event.
    /// </summary>
    public class TraitsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TraitsChangedEventArgs"/>
        /// class that describes a NotifyCollectionChangedAction.Reset
        /// change.
        /// </summary>
        /// <param name="action">The action that caused the event. This must be set to <see cref="NotifyCollectionChangedAction.Reset" /></param>
        public TraitsChangedEventArgs(NotifyCollectionChangedAction action)
        {
            if (action != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException($"{nameof(action)} must be {NotifyCollectionChangedAction.Reset}");
            }
            Action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraitsChangedEventArgs"/>
        /// </summary>
        /// <param name="action">The action that caused the event. This can be set to <see cref="NotifyCollectionChangedAction.Reset" />,
        /// <see cref="NotifyCollectionChangedAction.Add" />, or <see cref="NotifyCollectionChangedAction.Remove" /></param>
        /// <param name="changedItems">The items that are affected by the change.</param>
        public TraitsChangedEventArgs(NotifyCollectionChangedAction action, IList<(string category, string name, ITrait trait)> changedItems)
        {
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    NewItems = changedItems;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OldItems = changedItems;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentException($"{nameof(action)} must be Reset, Add, or Remove.");
            }

        }

        /// <summary>
        /// Initializes a new instance of the CharacterBuilder.Data.TraitsChangedEventArgs
        /// class that describes a one-item change.
        /// </summary>
        /// <param name="action">
        /// The action that caused the event. This can be set to NotifyCollectionChangedAction.Reset,
        /// NotifyCollectionChangedAction.Add, or NotifyCollectionChangedAction.Remove.
        /// </param>
        /// <param name="changedItem">The item that is affected by the change.</param>
        /// <exception cref="ArgumentException">
        /// If action is not Reset, Add, or Remove, or if action is Reset and changedItem
        /// is not null.
        /// </exception>
        public TraitsChangedEventArgs(NotifyCollectionChangedAction action, (string category, string name, ITrait trait) changedItem)
        {
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    NewItems = new List<(string category, string name, ITrait trait)> { changedItem };
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OldItems = new List<(string category, string name, ITrait trait)> { changedItem };
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentException($"{nameof(action)} must be Reset, Add, or Remove.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the CharacterBuilder.Data.TraitsChangedEventArgs
        /// class that describes a one-item NotifyCollectionChangedAction.Replace
        /// change.
        /// </summary>
        /// <param name="action">The action that caused the event. This can only be set to NotifyCollectionChangedAction.Replace.</param>
        /// <param name="newItem">The new item that is replacing the original item.</param>
        /// <param name="oldItem">The original item that is replaced.</param>
        /// <exception cref="ArgumentException">
        /// If action is not Replace.
        /// </exception>
        public TraitsChangedEventArgs(NotifyCollectionChangedAction action, (string category, string name, ITrait trait) newItem, (string category, string name, ITrait trait) oldItem)
        {
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException($"{nameof(action)} must be Replace.");
            }

            OldItems = new List<(string category, string name, ITrait trait)> { oldItem };
            NewItems = new List<(string category, string name, ITrait trait)> { newItem };
        }

        /// <summary>
        /// Gets the action that caused the event.
        /// </summary>
        /// <returns>
        /// A NotifyCollectionChangedAction value that describes
        /// the action that caused the event.
        /// </returns>
        public NotifyCollectionChangedAction Action { get; }

        /// <summary>
        /// Gets the list of new traits involved in the change.
        /// </summary>
        /// <returns>
        /// The list of new traits involved in the change.
        /// </returns>
        public IList<(string category, string name, ITrait trait)> NewItems { get; }

        /// <summary>
        /// Gets the list of items affected by a NotifyCollectionChangedAction.Replace,
        /// Remove, or Move action.
        /// </summary>
        /// <returns>
        /// The list of items affected by a NotifyCollectionChangedAction.Replace,
        /// Remove, or Move action.
        /// </returns>
        public IList<(string category, string name, ITrait trait)> OldItems { get; }
    }
}
