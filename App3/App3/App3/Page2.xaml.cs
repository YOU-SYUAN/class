using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;

namespace App3
{
    public partial class Page2 : ContentPage
    {
        public Page2()
        {
            InitializeComponent();
        }
        private async void EnrollClicked(object sender, EventArgs e)
        {
            Enroll enroll = new Enroll()
            {
                name = entryName.Text,
                id = Account.Text,
                pwd = entryPassword.Text,
            };
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(enroll, Formatting.Indented);
                    var data = "[" + json + "]";

                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("https://qrcodeapi.000webhostapp.com/enroll.php", content);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    

                    Enroll EnrollResult = JsonConvert.DeserializeObject<Enroll>(responseMessage);


                    if (EnrollResult.result == "true")
                    {
                        await DisplayAlert("成功", "上傳成功", "OK");
                        await Navigation.PopAsync(); //回上一頁
                    }
                    else
                    {
                        await DisplayAlert("失敗", "上傳失敗", "OK");
                        return;
                    }

                }
            }
        }
    }
}