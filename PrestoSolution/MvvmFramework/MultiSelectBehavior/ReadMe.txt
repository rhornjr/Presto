6-Dec-2009

http://blog.functionalfun.net/2009/02/how-to-databind-to-selecteditems.html

This project exists to provide the functionality for view models to bind to SelectedItems (plural) of grids, list boxes, etc...

/*****************************************************************************************/

Excerpts from the above link:

Standard data binding doesn’t work, because the SelectedItems property is read-only

I define an attached property that you attach to a ListBox (or DataGrid, or anything that inherits from MultiSelector) and allows you to specify a collection (via data binding, of course) that you want to be kept in sync with the SelectedItems collection of the target. To work properly, the collection you give should implement INotifyCollectionChanged – using ObservableCollection<T> should do the trick. When you set the property, I instantiate another class of my invention, a TwoListSynchronizer. This listens to CollectionChanged events in both collections, and when either changes, it updates the other.

/*****************************************************************************************/

To get this to work, I did this:

Add this namespace definition to the view:
xmlns:ff="clr-namespace:FunctionalFun.UI.Behaviours;assembly=MultiSelectBehavior"

Within the datagrid, add the last two lines shown here (ff:... and SelectionMode....):
            <tk:DataGrid x:Name ="TasksGrid" 
                         CanUserSortColumns="False"
                         AutoGenerateColumns="False" 
                         ItemsSource="{Binding Tasks}"                         
                         ff:MultiSelectorBehaviours.SynchronizedSelectedItems="{Binding SelectedTasks}" 
                         SelectionMode="Extended"
                         
                         