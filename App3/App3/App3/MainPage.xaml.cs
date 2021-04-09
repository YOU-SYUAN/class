using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;

namespace App3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private async void EnrollClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Page2());
        }
        private async void LogClicked(object sender, EventArgs e)
        {
            Login lgn = new Login()
            {
                id = UserID.Text,
                pwd = UserPassword.Text
            };
            try
            {
                //serialize序列化成json格式( {"id":"user1","pwd":"password","ans":"yes"}
                var json = JsonConvert.SerializeObject(lgn);
                //stringContent -> 依據字串提供http內容
                var content = new StringContent("[" + json + "]");
                //AlertMessage.Text += content;
                HttpClient client = new HttpClient();
                HttpResponseMessage result = await client.PostAsync("https://transfood.000webhostapp.com/transfood_ci/login.php", content);
                //AlertMessage.Text += result;
                var responseString = await result.Content.ReadAsStringAsync();
                //將json轉回物件
                Login dejson = JsonConvert.DeserializeObject<Login>(responseString);
                //AlertMessage.Text += dejson.ans;
                if (dejson.ans == "yes")
                {
                    await DisplayAlert("Logged in", "登入成功", "Go");
                    App3.session.DisplayUserId = UserID.Text;
                    await Navigation.PushAsync(new Page1());
                }
                else
                {
                    //AlertMessage.Text = "ID or password can't be empty!";
                    await DisplayAlert("Cannot Login", "帳號或密碼錯誤", "again");
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
