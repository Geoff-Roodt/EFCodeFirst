using System;
using Android.App;
using Android.Views;
using Android.Widget;
using ToDoApp.Tables;
using Object = Java.Lang.Object;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Text;
using ToDoApp.Annotations;

namespace ToDoApp.Adapters
{
    public class TodoItemDetailAdapter : BaseAdapter, INotifyPropertyChanged
    {
        private readonly Activity Activity;
        private readonly IList<TodoItem> Objects;

        public TodoItemDetailAdapter(Activity activity, IList<TodoItem> objects)
        {
            try
            {
                Activity = activity;
                Objects = objects;
            }
            catch (Exception ex)
            {
                Objects = new List<TodoItem>();
            }
        }

        public override Object GetItem(int position)
        {
            return Objects[position].ToJavaObject();
        }

        public override long GetItemId(int position)
        {
            return Objects[position].Id;
        }

        public bool GetItemCompleted(int position)
        {
            return Objects[position].Completed;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var Item = Objects[position];
            var View = (LinearLayout)(convertView ?? Activity.LayoutInflater.Inflate(Resource.Layout.TodoItemDetailRow, parent, false));
            var Description = View.FindViewById<TextView>(Resource.TodoItemDetailRow.TxtDescription);
            var Created = View.FindViewById<TextView>(Resource.TodoItemDetailRow.TxtCreated);
            var Updated = View.FindViewById<TextView>(Resource.TodoItemDetailRow.TxtUpdated);
            var Completed = View.FindViewById<CheckBox>(Resource.TodoItemDetailRow.ChkCompleted);

            Completed.Checked = Item.Completed;
            Description.Text = Item.Description;
            Created.TextFormatted = Html.FromHtml($"Created: {Item.Created.ToString("U")}");
            Updated.TextFormatted = Html.FromHtml($"Updated: {Item.Modified.ToString("U")}");

            return View;
        }

        public override int Count
        {
            get { return Objects.Count; }
        }
        
        public IList<TodoItem> GetItems()
        {
            return Objects;
        }

        public TodoItem GetTodoItem(int position)
        {
            if (position >= 0 && position < Objects.Count)
            {
                return Objects[position];
            }

            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}