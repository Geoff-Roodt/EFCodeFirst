using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ToDoApp.Tables;
using ToDoApp.Interfaces;

namespace ToDoApp.Services
{
    public class RestService : IRestService
    {
        private HttpClient Client;
        public List<TodoItem> TodoItems { get; private set; }

        public RestService()
        {
            Client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };
        }

        public async Task<List<TodoItem>> RefreshDataAsync()
        {
            TodoItems = new List<TodoItem>();

            try
            {
                var response = await Client.GetAsync(Constants.RestUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    TodoItems = JsonConvert.DeserializeObject<List<TodoItem>>(content);
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

        public async Task<int> Update(List<TodoItem> items)
        {
            if (items == null || !items.Any()) return 0;

            try
            {
                var json = JsonConvert.SerializeObject(items);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Client.PutAsync($"{Constants.RestUrl}/many", content);
                if (!response.IsSuccessStatusCode) return 0;

                string responseContent = await response.Content.ReadAsStringAsync();
                bool success = JsonConvert.DeserializeObject<bool>(responseContent);
                return success ? 1 : 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}