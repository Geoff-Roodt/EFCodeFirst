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

            Button btnSync = FindViewById<Button>(Resource.Main.btnSync);
            btnSync.Click += BtnSync_Click;

            Button btnSave = FindViewById<Button>(Resource.Main.btnSave);
            btnSave.Click += BtnSave_Click;

            LblError = FindViewById<TextView>(Resource.Main.lblError);

            await PopulateItems();
        }

        private async void BtnSave_Click(object sender, System.EventArgs e)
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

        private async void BtnSync_Click(object sender, System.EventArgs e)
        {
            await PopulateItems();
            Toast.MakeText(this, "Todo Items Refreshed", ToastLength.Long).Show();
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
    }
}

