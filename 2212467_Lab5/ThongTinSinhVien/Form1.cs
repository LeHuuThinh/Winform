using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThongTinSinhVien
{
    public partial class Form1 : Form
    {
        QuanLySinhVien qlsv;  // Gọi tắt
        public Form1()
        {
            InitializeComponent();

        }
        // Chạy form
        private void Form1_Load(object sender, EventArgs e)
        {
            qlsv = new QuanLySinhVien();
            qlsv.DocTuFile();
            LoadListView();
        }
        // Sinh vien construct
        public class SinhVien
        {
            public string MSSV { get; set; }
            public string HoTenLot { get; set; }
            public string Ten { get; set; }
            public bool GioiTinh { get; set; }
            public DateTime NgaySinh { get; set; }
            public int Tuoi { get; set; }
            public string SoCCCD { get; set; }
            public string SDT { get; set; }
            public string DiaChi { get; set; }
            public string Lop { get; set; }
            public List<string> MonDK { get; set; }

            public SinhVien()
            {
                MonDK = new List<string>();
            }


            public SinhVien(
                string mssv,
                string hotenlot,
                string ten,
                bool gioitinh,
                DateTime ngaysinh,
                int tuoi,
                string cccd,
                string sdt,
                string diachi,
                string lop,
                List<string> mondk
                )
            {
                this.MSSV = mssv;
                this.HoTenLot = hotenlot;
                this.Ten = ten;
                this.GioiTinh = gioitinh;
                this.NgaySinh = ngaysinh;
                this.Tuoi = tuoi;
                this.SoCCCD = cccd;
                this.SDT = sdt;
                this.DiaChi = diachi;
                this.Lop = lop;
                this.MonDK = mondk;

            }
        }

        public delegate int SoSanh(object sv1, object sv2);
        // Quản lý sinh viên, thêm xóa sửa ....
        class QuanLySinhVien
        {
            public List<SinhVien> DanhSach;
            public QuanLySinhVien()
            {
                DanhSach = new List<SinhVien>();
            }
            // Thêm
            public void Them(SinhVien sv)
            {
                this.DanhSach.Add(sv);
            }

            public SinhVien this[int index]
            {
                get { return DanhSach[index]; }
                set { DanhSach[index] = value; }
            }
            // Tìm
            public SinhVien Tim(object obj, SoSanh ss)
            {
                SinhVien svresult = null;
                foreach (SinhVien sv in DanhSach)
                    if (ss(obj, sv) == 0)
                    {
                        svresult = sv;
                        break;
                    }
                return svresult;
            }
            // Sửa
            public bool Sua(SinhVien svsua, object obj, SoSanh ss)
            {
                int i, count;
                bool kq = false;
                count = this.DanhSach.Count - 1;
                for (i = 0; i < count; i++)
                    if (ss(obj, this[i]) == 0)
                    {
                        this[i] = svsua;
                        kq = true;
                        break;
                    }
                return kq;
            }
            // Đọc từ file txt
            public void DocTuFile()
            {
                string filename = "fileSinhVien.txt", t;
                string[] s;
                SinhVien sv;
                StreamReader sr = new StreamReader(
                new FileStream(filename, FileMode.Open));
                while ((t = sr.ReadLine()) != null)
                {
                    // Bỏ khoảng trắng gây bug
                    if (string.IsNullOrWhiteSpace(t))
                        continue; 
                    
                    s = t.Split('*');
                    sv = new SinhVien();
                    sv.MSSV = s[0];
                    sv.HoTenLot = s[1];
                    sv.Ten = s[2];
                    sv.NgaySinh = DateTime.Parse(s[3]);

                    sv.GioiTinh = false;
                    if (s[4] == "1")
                        sv.GioiTinh = true;

                    sv.Lop = s[5];
                    sv.SoCCCD = s[6];
                    sv.SDT = s[7];
                    sv.DiaChi = s[8];

                    string[] mk = s[9].Split(',');
                    foreach (string m in mk)
                        sv.MonDK.Add(m);
                    this.Them(sv);
                }
            }



            
        }
        // Thêm sv vào listview
        private void ThemSV(SinhVien sv)
        {
            ListViewItem lvitem = new ListViewItem(sv.MSSV);
            lvitem.SubItems.Add(sv.HoTenLot);
            lvitem.SubItems.Add(sv.Ten);
            lvitem.SubItems.Add(sv.NgaySinh.ToShortDateString());

            string gt = "Nữ";
            if (sv.GioiTinh)
                gt = "Nam";
            lvitem.SubItems.Add(gt);

            lvitem.SubItems.Add(sv.Lop);
            lvitem.SubItems.Add(sv.SoCCCD);
            lvitem.SubItems.Add(sv.SDT);
            lvitem.SubItems.Add(sv.DiaChi);

            string mk = "";
            foreach (string s in sv.MonDK)
                mk += s + ",";
            mk = mk.Substring(0, mk.Length - 1);

            lvitem.SubItems.Add(mk);


            this.lvDSSV.Items.Add(lvitem);
        }
        //Hiển thị các sinh viên trong qlsv lên ListView

        private void LoadListView()
        {
            this.lvDSSV.Items.Clear();
            foreach (SinhVien sv in qlsv.DanhSach)
            {
                ThemSV(sv);
            }
        }
        // Lấy thông tin từ textbox đã nhập 
        private SinhVien GetSinhVien()
        {
            SinhVien sv = new SinhVien();

            bool gioitinh = true;
            List<string> mk = new List<string>();

            sv.MSSV = this.txtMSSV.Text;
            sv.HoTenLot = this.txtHoTenLot.Text;
            sv.Ten = this.txtTen.Text;
            sv.NgaySinh = this.dtpNgaySinh.Value;

            if (rbNu.Checked)
                gioitinh = false;
            sv.GioiTinh = gioitinh;

            sv.Lop = this.cbbLop.Text;
            sv.SoCCCD = this.txtCCCD.Text;
            sv.SDT = this.txtSDT.Text;
            sv.DiaChi = this.txtDiaChi.Text;

            for (int i = 0; i < this.clbMDK.Items.Count; i++)
                if (clbMDK.GetItemChecked(i))
                    mk.Add(clbMDK.Items[i].ToString());
            sv.MonDK = mk;

            return sv;
        }

        //Lấy thông tin sinh viên từ dòng item của ListView
        private SinhVien GetSinhVienLV(ListViewItem lvitem)
        {
            SinhVien sv = new SinhVien();
            sv.MSSV = lvitem.SubItems[0].Text;
            sv.HoTenLot = lvitem.SubItems[1].Text;
            sv.Ten = lvitem.SubItems[2].Text;
            sv.NgaySinh = DateTime.Parse(lvitem.SubItems[3].Text);

            sv.GioiTinh = false;
            if (lvitem.SubItems[4].Text == "Nam")
                sv.GioiTinh = true;

            sv.Lop = lvitem.SubItems[5].Text;
            sv.SoCCCD = lvitem.SubItems[6].Text;
            sv.SDT = lvitem.SubItems[7].Text;
            sv.DiaChi = lvitem.SubItems[8].Text;

            List<string> mk = new List<string>();
            string[] s = lvitem.SubItems[9].Text.Split(',');
            foreach (string t in s)
                mk.Add(t);
            sv.MonDK = mk;


            return sv;
        }
        // Gắn các thông tin sv lên trên textbox
        private void ThietLapThongTin(SinhVien sv)
        {
            this.txtMSSV.Text = sv.MSSV;
            this.txtHoTenLot.Text = sv.HoTenLot;
            this.txtTen.Text = sv.Ten;
            this.dtpNgaySinh.Value = sv.NgaySinh;

            if (sv.GioiTinh)
                this.rbNam.Checked = true;
            else
                this.rbNu.Checked = true;

            this.cbbLop.Text = sv.Lop;
            this.txtCCCD.Text = sv.SoCCCD;
            this.txtSDT.Text = sv.SDT;
            this.txtDiaChi.Text = sv.DiaChi;

            for (int i = 0; i < this.clbMDK.Items.Count; i++)
                this.clbMDK.SetItemChecked(i, false);
            foreach (string s in sv.MonDK)
            {
                for (int i = 0; i < this.clbMDK.Items.Count;
                i++)
                    if
                    (s.CompareTo(this.clbMDK.Items[i]) == 0)
                        this.clbMDK.SetItemChecked(i,
                        true);
            }

        }
        // Nút thêm
        private void btnThem_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            SinhVien kq = qlsv.Tim(sv.MSSV, delegate (object obj1, object obj2)
            {
                return (obj2 as SinhVien).MSSV.CompareTo(obj1.ToString());
            });
            if (kq != null)
                MessageBox.Show("Mã sinh viên đã tồn tại!", "Lỗi thêm dữ liệu",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                this.qlsv.Them(sv);
                GhiVaoFile(sv);
                this.LoadListView();
            }
        }
        // Chọn sv nào thì hiện thông tin sv đó
        private void lvDSSV_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = this.lvDSSV.SelectedItems.Count;
            if (count > 0)
            {
                ListViewItem lvitem =
                this.lvDSSV.SelectedItems[0];
                SinhVien sv = GetSinhVienLV(lvitem);
                ThietLapThongTin(sv);
            }
        }
         private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult ketqua = MessageBox.Show("Bạn có chắc chắn muốn thoát ?", "Thoát chương trình",
               MessageBoxButtons.OKCancel); ;
            if (ketqua == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Duyệt qua ListView và xóa các item được chọn (checked)
            for (int i = lvDSSV.Items.Count - 1; i >= 0; i--)
            {
                if (lvDSSV.Items[i].Checked)
                {
                    //string mssv = lvDSSV.Items[i].SubItems[0].Text;
                    // Xóa item được checked
                    lvDSSV.Items.RemoveAt(i);
                    //XoaSinhVienKhoiFile(mssv);
                }
            }
        }
        // Hàm ghi sinh viên mới vào file .txt
        public void GhiVaoFile(SinhVien sv)
        {
            string filename = "fileSinhVien.txt";

            // Mở file và ghi thêm dòng thông tin sinh viên
            using (StreamWriter sw = new StreamWriter(filename, true)) // 'true' để ghi thêm vào cuối file
            {
                // Format: MSSV*HoTenLot*Ten*NgaySinh*GioiTinh*Lop*SoCCCD*SDT*DiaChi*MonDK
                string gioiTinh = sv.GioiTinh ? "1" : "0";  // 1 là Nam, 0 là Nữ
                string monDK = string.Join(",", sv.MonDK);  // Ghép các môn bằng dấu ','
                string line = $"{sv.MSSV}*{sv.HoTenLot}*{sv.Ten}*{sv.NgaySinh.ToShortDateString()}*{gioiTinh}*{sv.Lop}*{sv.SoCCCD}*{sv.SDT}*{sv.DiaChi}*{monDK}";

                // Ghi dòng vào file
                sw.WriteLine(line);
            }
        }

        // Xóa sv khỏi file txt
        //public void XoaSinhVienKhoiFile(string mssv)
        //{
        //    string filename = "fileSinhVien.txt";

        //    // Đọc toàn bộ nội dung file vào một danh sách
        //    List<string> lines = new List<string>();
        //    using (StreamReader sr = new StreamReader(filename))
        //    {
        //        string line;
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            // Chỉ thêm các dòng không chứa MSSV của sinh viên cần xóa
        //            string[] s = line.Split('*');
        //            if (s[0] != mssv) // MSSV nằm ở vị trí đầu tiên trong mỗi dòng
        //            {
        //                lines.Add(line);
        //            }
        //        }
        //    }

        //    // Ghi lại toàn bộ danh sách vào file (trừ sinh viên đã bị xóa)
        //    using (StreamWriter sw = new StreamWriter(filename, false)) // 'false' để ghi đè file
        //    {
        //        foreach (string line in lines)
        //        {
        //            sw.WriteLine(line);
        //        }
        //    }
        //}

    }
}
