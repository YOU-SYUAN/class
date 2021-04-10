using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
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
            public string Cid { get; set; }
            public bool Is_teacher { get; set; }
        }
        // 接收回PHP傳值的類別
        public class PHPresult
        {
            public string Cname { get; set; }
            public string Ccontent { get; set; }
            public string Teacher { get; set; }
            public string Starttime { get; set; }
            public string Endtime { get; set; }
            public string QRsting { get; set; }
        }

        public void SendCid()
        {
            string ClassId = "1234";
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
                        PHPresult classInfo = JsonConvert.DeserializeObject<PHPresult>(strResult);

                        Console.WriteLine("result = " + classInfo);
                        if (classInfo != null)
                        {
                            DispalyToPage(classInfo);
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
        private void DispalyToPage(PHPresult classInfo)
        {
            value.BarcodeValue = classInfo.QRsting;
            name.Text = name.Text + classInfo.Cname;
            content.Text = content.Text + classInfo.Ccontent;
            teacher.Text = teacher.Text + classInfo.Teacher;
            startTime.Text = startTime.Text + classInfo.Starttime;
            endTime.Text = endTime.Text + classInfo.Endtime;
        }
    }
}
