using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace App3
{
    public partial class teacherClassInfo : ContentPage
    {
        public teacherClassInfo()
        {
            InitializeComponent();
            SendCid();
        }


        // 建立物件類別
        public class SendData
        {
            public int Cid { get; set; }
            public bool Is_teacher { get; set; }
        }
        // 接收回PHP傳值的類別
        public class PHPresult
        {
            public string tname { get; set; }
            public string courseContent { get; set; }
            public string courseName { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
            public string cQrcode { get; set; }
        }

        public void SendCid()
        {
            int ClassId = 1;
            bool is_teacher = true;

            // 建立物件
            SendData sendData = new SendData
            {
                Cid = ClassId,
                Is_teacher = is_teacher
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
                        string FooUrl = $"https://qrcodeapi.000webhostapp.com/course_detail.php";
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
                        //PHPresult classInfo = JsonConvert.DeserializeObject<PHPresult>(strResult);
                        JArray array = JsonConvert.DeserializeObject<JArray>(strResult);
                        Console.WriteLine(array[0]);
                        // 從list轉乘object
                        JObject obj = (JObject)array[0];
                        Console.WriteLine(obj);

                        Console.WriteLine("==========="+ obj["tName"].ToString());

                        if (array != null)
                        {
                            DispalyToPage(obj);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("失敗，PHP回傳發生錯誤");
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

        // 顯示到使用者頁面
        private void DispalyToPage(JObject obj)
        {
            value.BarcodeValue = obj["cQrcode"].ToString();
            name.Text = name.Text + obj["courseName"].ToString();
            content.Text = content.Text + obj["courseContent"].ToString();
            teacher.Text = teacher.Text + obj["tName"].ToString();
            startTime.Text = startTime.Text + obj["startTime"].ToString();
            endTime.Text = endTime.Text + obj["endTime"].ToString();
        }

        // 回主畫面
        private async void gotoPage(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
