using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
namespace App3
{
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }
        public class MyCourse
        {
            public string id { get; set; }
        }

        public class Returnresult
        {
            public string tName { get; set; }
            public string courseName { get; set; }
        }
        public class Finalresult
        {
            //public string result { get; set; }
            public string tName { get; set; }
            public string courseName { get; set; }
            public List<string> Final { get; set; }
        }
        private async void OpenNew(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewClassPage());
        }
        private async void SearchCourse(object sender, EventArgs e)
        {
            string uID = App3.session.DisplayUserId;
            MyCourse MC = new MyCourse()
            {
                id = uID,
            };
            try
            {
                var json = JsonConvert.SerializeObject(MC);
                //results.Text += json;
                //stringContent -> 依據字串提供http內容
                var content = new StringContent("[" + json + "]");
                //results.Text += content;
                HttpClient client = new HttpClient();
                HttpResponseMessage Rresult = await client.PostAsync("https://qrcodeapi.000webhostapp.com/mainPage.php", content);
                //results.Text += result;
                var responseString = await Rresult.Content.ReadAsStringAsync();
                //將json轉回物件
                Console.WriteLine(responseString);
                JArray array = JsonConvert.DeserializeObject<JArray>(responseString);
                foreach (JObject Jobj in array)
                {
                    //Console.WriteLine(Jobj["tName"]);
                    //Console.WriteLine(Jobj["courseName"]);
                    results.Text += Jobj["tName"];
                    results.Text += $"{Jobj["courseName"]} {Environment.NewLine}";
                }
                //JObject obj = 
                //List<Finalresult> dejson = JsonConvert.DeserializeObject<List<Finalresult>>(responseString);
                //List<finalresult> finalresult = dejson;
                //results.Text += returnresult.courseName;
                //results.Text += returnresult.tName;
                //results.Text += responseString;
                //results.Text += array;






                //AlertMessage.Text += dejson.ans;
                if (array != null)
                {
                    //results.Text += dejson;
                    await DisplayAlert("success", "成功", "Go");
                    //await Navigation.PushAsync(new Page1());
                }
                else
                {
                    await DisplayAlert("empty", "空的", "again");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.ToString(), "Ok");
                return;
            }

        }

    }
}