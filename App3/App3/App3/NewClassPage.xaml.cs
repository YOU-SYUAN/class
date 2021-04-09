using System;
using System.Text;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;

namespace App3
{
    public partial class NewClassPage : ContentPage
    {

        public NewClassPage()
        {
            InitializeComponent();
        }

        // 建立物件類別
        public class SendData
        {
            public string Uid { get; set; }
            public string Cname { get; set; }
            public string Ccontent { get; set; }
            public string Starttime { get; set; }
            public string Endtime { get; set; }
            public string QRsting { get; set; }
        }
        // 接收回PHP傳值的類別
        public class PHPresult
        {
            public string ans { get; set; }
        }

        public void AddClass(object sender, EventArgs e)
        {
            string uID = App3.session.DisplayUserId;
            string cName = classname.Text;
            string cContent = classcontent.Text;
            string startTime = startDatePicker.Date.ToString("yyyy/MM/dd") +"-"+startTimePicker.Time.ToString();
            string endTime = endDatePicker.Date.ToString("yyyy/MM/dd") + "-"+endTimePicker.Time.ToString();

            //取亂數
            var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var next = new Random();
            var builder = new StringBuilder();
            for (var i = 0; i < 5; i++)
            {
                builder.Append(str[next.Next(0, str.Length)]);
            }
            string randomString = builder.ToString();
            string qrString = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + randomString;

            //建立物件
            SendData sendData = new SendData
            {
                Uid = uID,
                Cname = cName,
                Ccontent = cContent,
                Starttime = startTime,
                Endtime = endTime,
                QRsting = qrString
            };

            // 物件序列化
            string strJson = JsonConvert.SerializeObject(sendData, Formatting.Indented);
            string strJson2 = "[" + strJson + "]";
            Console.WriteLine(strJson2);
            ToPHP(strJson2);
        }

        // HttpClient
        public async void ToPHP(string Str)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        // 目標php檔
                        string FooUrl = $"https://transfood.000webhostapp.com/transfood_ci/login.php";
                        HttpResponseMessage response = null;

                        //設定相關網址內容
                        var fooFullUrl = $"{FooUrl}";

                        // Accept 用於宣告客戶端要求服務端回應的文件型態 (底下兩種方法皆可任選其一來使用)
                        client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Content-Type 用於宣告遞送給對方的文件型態
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                        using (var fooContent = new StringContent(Str, Encoding.UTF8, "application/json"))
                        {
                            response = await client.PostAsync(fooFullUrl, fooContent);
                        }
                        Console.WriteLine("response = " + response);
                        // PHP回傳值
                        string strResult = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("strResult = " + strResult);
                        // 反序列化
                        PHPresult yesORno = JsonConvert.DeserializeObject<PHPresult>(strResult);

                        Console.WriteLine("result = " + yesORno.ans);
                        if (yesORno.ans == "yes")
                        {
                            Console.WriteLine("successsuccesssuccesssuccesssuccess");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("failfailfailfailfailfailfailfailfail");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
    }
}
