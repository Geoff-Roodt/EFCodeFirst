using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Runtime;
using Android.Widget;
using Android.OS;
using ToDoApp.Adapters;
using ToDoApp.Services;
using ToDoApp.Tables;

namespace ToDoApp
{
    [Activity(Label = "ToDo App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private ListView LvTodoItems;
        private TextView LblHeading;
        private TodoService Service;
        private JavaList<TodoItem> Items;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Service = new TodoService(new RestService());
            Items = new JavaList<TodoItem>();

            LvTodoItems = FindViewById<ListView>(Resource.Main.lstTodoItems);
            LvTodoItems.ItemClick += LstLvTodoItemsItemClick;

            Button btnSync = FindViewById<Button>(Resource.Main.btnSync);
            btnSync.Click += BtnSync_Click;

            LblHeading = FindViewById<TextView>(Resource.Main.lblHeading);

            await PopulateItems();
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
                var listAdapter = new TodoItemDetailAdapter(this, Items.ToList());
                LvTodoItems.Adapter = listAdapter;

                LblHeading.Text = Items != null && Items.Count > 0 ? "Things To Do" : "You have no things to do!";
            }
            else
            {
                LblHeading.Text = "Oops! Something went wrong. We could not load your things to do";
            }
        }
    }
}

