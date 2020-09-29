using LanterneRouge.Fresno.WpfClient.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LanterneRouge.Fresno.WpfClient.MVVM
{
    public class TabControlBehavior
    {
        #region Property FocusFirstVisibleTab

        /// <summary>Whether to focus the first visible tab. </summary>
        /// <remarks>Setting the FocusFirstVisibleTab attached property to true will focus the next visible tab when the current selected tab's Visibility property is set to Collapsed or Hidden.</remarks>
        public static readonly DependencyProperty FocusFirstVisibleTabProperty = DependencyProperty.RegisterAttached("FocusFirstVisibleTab", typeof(bool), typeof(TabControlBehavior), new FrameworkPropertyMetadata(OnFocusFirstVisibleTabPropertyChanged));

        /// <summary>Gets the focus first visible tab value of the given element.</summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static bool GetFocusFirstVisibleTab(TabControl element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(FocusFirstVisibleTabProperty);
        }

        // </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetFocusFirstVisibleTab(TabControl element, bool value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(FocusFirstVisibleTabProperty, value);
        }

        /// <summary>Determines whether the value of the dependency property <c>IsFocused</c> has change.</summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnFocusFirstVisibleTabPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl tabControl)
            {
                // Attach or detach the event handlers
                if ((bool)e.NewValue)
                {
                    // Enable the attached behavior
                    tabControl.Items.CurrentChanged += new EventHandler(TabControl_Items_CurrentChanged);
                    if (tabControl.Items is INotifyCollectionChanged collection)
                    {
                        collection.CollectionChanged += new NotifyCollectionChangedEventHandler(TabControl_Items_CollectionChanged);
                    }
                }

                else
                {
                    // Disable the attached behavior
                    tabControl.Items.CurrentChanged -= new EventHandler(TabControl_Items_CurrentChanged);
                    if (tabControl.Items is INotifyCollectionChanged collection)
                    {
                        collection.CollectionChanged -= new NotifyCollectionChangedEventHandler(TabControl_Items_CollectionChanged);
                    }

                    foreach (var item in tabControl.Items)
                    {
                        if (item is TabItem tab)
                        {
                            tab.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(TabItem_IsVisibleChanged);
                        }
                    }
                }
            }
        }

        /// <summary>Handles the CollectionChanged event of the TabControl.Items collection. </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private static void TabControl_Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Attach event handlers to each tab so that when the 
            // Visibility property changes of the selected tab,
            // the focus can be shifted to the next (or previous, if not next tab available) tab.
            var collection = sender as ItemCollection;
            if (collection != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Replace:
                        // Attach event handlers to the Visibility and IsEnabled properties.
                        if (e.NewItems != null)
                        {
                            foreach (var item in e.NewItems)
                            {
                                if (item is TabItem tab)
                                {
                                    tab.IsVisibleChanged += new DependencyPropertyChangedEventHandler(TabItem_IsVisibleChanged);
                                }
                            }
                        }

                        // Detach event handlers from old items.
                        if (e.OldItems != null)
                        {
                            foreach (var item in e.OldItems)
                            {
                                if (item is TabItem tab)
                                {
                                    tab.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(TabItem_IsVisibleChanged);
                                }
                            }
                        }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        // Attach event handlers to the Visibility and IsEnabled properties.
                        foreach (var item in collection)
                        {
                            if (item is TabItem tab)
                            {
                                tab.IsVisibleChanged += new DependencyPropertyChangedEventHandler(TabItem_IsVisibleChanged);
                            }
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                    default:
                        break;
                }

                // Select the first element if necessary.
                if (collection.Count > 0 && collection.CurrentItem == null)
                {
                    collection.MoveCurrentToFirst();
                }
            }
        }

        /// <summary>Handles the CurrentChanged event of the TabControl.Items collection. </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>

        private static void TabControl_Items_CurrentChanged(object sender, EventArgs e)
        {
            if (sender is ItemCollection collection)
            {
                if (collection.CurrentItem is UIElement element && element.Visibility != Visibility.Visible)
                {
                    element.Dispatcher.BeginInvoke(new Action(() => collection.MoveCurrentToNext()), DispatcherPriority.Input);
                }
            }
        }

        /// <summary>Handles the IsVisibleChanged event of the tab item. </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void TabItem_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TabItem tab && tab.IsSelected && tab.Visibility != Visibility.Visible)
            {
                // Move to the next tab item.
                if (tab.Parent is TabControl tabControl)
                {
                    if (!tabControl.Items.MoveCurrentToNext())
                    {
                        // Could not move to next, try previous.
                        tabControl.Items.MoveCurrentToPrevious();
                    }
                }
            }
        }

        #endregion

        #region Property ActivateCommands

        public static readonly DependencyProperty ActivateCommandsProperty = DependencyProperty.RegisterAttached("ActivateCommands", typeof(bool), typeof(TabControlBehavior), new FrameworkPropertyMetadata(OnActivateCommandsPropertyChanged));

        public static bool GetActivateCommands(TabControl element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(ActivateCommandsProperty);
        }

        public static void SetActivateCommands(TabControl element, bool value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(ActivateCommandsProperty, value);
        }

        private static void OnActivateCommandsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl tabControl)
            {
                var tabControlItems = tabControl.Items;
                if (tabControlItems != null)
                {
                    var newValue = (bool)e.NewValue;
                    var activateCommandsWorker = new EventHandler((s1, e2) => Items_CurrentChanged(tabControl, tabControl.Items.CurrentItem as WorkspaceViewModel));

                    if (newValue)
                    {
                        tabControlItems.CurrentChanged += activateCommandsWorker;
                    }

                    else
                    {
                        tabControlItems.CurrentChanged -= activateCommandsWorker;
                    }
                }
            }
        }

        private static void Items_CurrentChanged(TabControl tabControl, WorkspaceViewModel current)
        {
            if (current != null)
            {
                SetCommandsCollection(tabControl, current.SubCommands);
            }
        }

        #endregion

        #region Property CommandsCollection

        public static ObservableCollection<CommandViewModel> GetCommandsCollection(DependencyObject elemen)
        {
            return (ObservableCollection<CommandViewModel>)elemen.GetValue(CommandsCollectionProperty);
        }

        public static void SetCommandsCollection(DependencyObject element, ObservableCollection<CommandViewModel> value)
        {
            element.SetValue(CommandsCollectionProperty, value);
        }

        // Using a DependencyProperty as the backing store for CommandsCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandsCollectionProperty = DependencyProperty.RegisterAttached("CommandsCollection", typeof(ObservableCollection<CommandViewModel>), typeof(TabControlBehavior), new PropertyMetadata(null));

        #endregion
    }
}