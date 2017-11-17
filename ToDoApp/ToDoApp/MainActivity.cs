using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using ToDoApp.Tables;
using ToDoApp.Adapters;
using ToDoApp.Services;

namespace ToDoApp
{
    public enum MenuType
    {
        Add,
        Delete,
        Save,
        Sync
    }

    [Activity(Label = "To Do Items", Icon = "@drawable/Launcher_Icon", Theme="@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Activity
    {
        private ListView LvTodoItems;
        private TextView LblError;
        private TodoService Service;
        private List<TodoItem> Items;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Service = new TodoService(new RestService());
            Items = new List<TodoItem>();

            LvTodoItems = FindViewById<ListView>(Resource.Main.lstTodoItems);
            LvTodoItems.ItemClick += LstLvTodoItemsItemClick;

            LblError = FindViewById<TextView>(Resource.Main.lblError);

            await PopulateItems();
        }
        
        private void LstLvTodoItemsItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            // Toggle the 'done' state of the item
            var foo = e;
            var bar = sender;
        }

        private async Task PopulateItems()
        {
            // Perform calls to API client to get all items then set the List Adapter
            Items = await Service.Get();
            if (Items != null)
            {
                var listAdapter = new TodoItemDetailAdapter(this, Items);
                LvTodoItems.Adapter = listAdapter;
                LvTodoItems.RefreshDrawableState();

                if (!Items.Any())
                {
                    LblError.Text = "You have no things to do!";
                    LblError.Visibility = ViewStates.Visible;
                }
            }
            else
            {
                LblError.Text = "Oops! Something went wrong. We could not load your things to do";
                LblError.Visibility = ViewStates.Visible;
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            IMenuItem addNew = menu.Add(0, (int)MenuType.Add, 1, Resource.String.AddOption);
            addNew.SetIcon(Android.Resource.Drawable.IcMenuRotate);
            IMenuItem saveItems = menu.Add(0, (int)MenuType.Save, 2, Resource.String.SaveOption);
            saveItems.SetIcon(Android.Resource.Drawable.IcPopupSync);
            IMenuItem syncItems = menu.Add(0, (int)MenuType.Sync, 3, Resource.String.SyncOption);
            syncItems.SetIcon(Android.Resource.Drawable.IcPopupSync);

            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch ((MenuType)item.ItemId)
            {
                case MenuType.Add:

                    break;
                case MenuType.Sync:
                    PopulateItems();
                    Toast.MakeText(this, "Todo Items Refreshed", ToastLength.Long).Show();
                    break;
                case MenuType.Save:
                    SaveItems();
                    break;
                case MenuType.Delete:
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private async Task SaveItems()
        {
            TodoItemDetailAdapter adapter = LvTodoItems.Adapter as TodoItemDetailAdapter;
            if (adapter != null)
            {
                foreach (TodoItem item in adapter.GetItems())
                {
                    bool response = await Service.Edit(item);
                    if (!response)
                    {
                        Toast.MakeText(this, $"{item.Description} was not updated", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, $"{item.Description} successfully updated", ToastLength.Long).Show();
                    }
                }
                await PopulateItems();
            }
        }
    }
}

