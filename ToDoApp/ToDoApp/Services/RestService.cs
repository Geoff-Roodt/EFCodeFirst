using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Runtime;
using ToDoApp.Tables;
using ToDoApp.Interfaces;

namespace ToDoApp.Services
{
    public class RestService : IRestService
    {
        private HttpClient Client;
        public JavaList<TodoItem> TodoItems { get; private set; }

        public RestService()
        {
            Client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };
        }

        public async Task<JavaList<TodoItem>> RefreshDataAsync()
        {
            TodoItems = new JavaList<TodoItem>();

            try
            {
                var response = await Client.GetAsync(Constants.RestUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    TodoItems = JsonConvert.DeserializeObject<JavaList<TodoItem>>(content);
                    return TodoItems;
                }

                return TodoItems;
            }
            catch (Exception ex)
            {
                return TodoItems;
            }
        }

        public async Task<int> Update(TodoItem item)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Client.PutAsync(Constants.RestUrl, content);

                return response.IsSuccessStatusCode ? 1 : 0;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<int> Add(TodoItem item)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Client.PostAsync(Constants.RestUrl, content);

                return response.IsSuccessStatusCode ? 1 : 0;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                var response = await Client.DeleteAsync($"{Constants.RestUrl}/{id}");
                return response.IsSuccessStatusCode ? 1 : 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}