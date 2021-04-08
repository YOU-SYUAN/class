using System;
using System.Collections.Generic;
using System.Text;

namespace App3
{
    public class Enroll
    {
        public string name { get; set; }
        public string id { get; set; }
        public string pwd { get; set; }
        public string result { get; set; }
    }
    public class Login
    {
        public string id { get; set; }
        public string pwd { get; set; }
        public string ans { get; set; }
    }
    public class Record
    {
        public string no { get; set; }
        public string dID { get; set; }
        public string platitude { get; set; }
        public string plongitude { get; set; }
        public string time { get; set; }
    }
    public class Upload
    {
        public string dID { get; set; }
        public float plongitude { get; set; }
        public float platitude { get; set; }
        public DateTime time { get; set; }
        public bool ans { get; set; }
        //public class Result
        //{
        //    public string ans { get; set; }
        //}
    }
}