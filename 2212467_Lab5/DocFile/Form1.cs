using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace DocFile
{
    public partial class Form1 : Form
    {
        // Tool -> NuGet Package Manager -> Package Manager Console -> Gõ "Install-Package Newtonsoft.Json"
        // using Newtonsoft.Json, System.IO, Newtonsoft.Json.Linq
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public class StudentInfo
        {
            public string MSSV { get; set; }
            public string HoTen { get; set; }
            public int Tuoi { get; set; }
            public double Diem { get; set; }
            public bool TonGiao { get; set; }

            public StudentInfo(string mssv, string hoten, int tuoi, double diem, bool tongiao)
            {
                this.MSSV = mssv;
                this.HoTen = hoten;
                this.Tuoi = tuoi;
                this.Diem = diem;
                this.TonGiao = tongiao;
            }
        }

        private List<StudentInfo> LoadJSON(string path)
        {
            List<StudentInfo> List = new List<StudentInfo>();

            StreamReader r = new StreamReader(path);
            string json = r.ReadToEnd();

            var array = (JObject)JsonConvert.DeserializeObject(json);

            var students = array["sinh vien"].Children();
            foreach (var item in students)
            {
                string mssv = item["MSSV"].Value<string>();
                string hoten = item["hoten"].Value<string>();
                int tuoi = item["tuoi"].Value<int>();
                double diem = item["diem"].Value<int>();
                bool tongiao = item["tongiao"].Value<bool>();

                StudentInfo info = new StudentInfo(mssv, hoten, tuoi, diem, tongiao);
                List.Add(info);
            }
            return List;
        }

        private void btnJSON_Click(object sender, EventArgs e)
        {
            string Str = "";
            string Path = "../../fileJSON.json";
            List<StudentInfo> List = LoadJSON(Path);
            for (int i = 0; i < List.Count; i++)
            {
                StudentInfo Info = List[i];
                Str += string.Format("Sinh viên {0} có MSSV: {1}, Họ tên: {2}, Điểm TB {3}\r\n", (i + 1), Info.MSSV, Info.HoTen, Info.Diem);
            }
            MessageBox.Show(Str);
        }
    }
}
