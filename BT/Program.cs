
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
namespace Baitaplon
{
    class Program
    {
        public class class1
        {

            private string sbd, kv, ut, dan_toc, ten, ngay_sinh;

            public string SBD { get { return sbd; } }
            public string Ten { get { return ten; } }
            public string Khuvuc { get { return kv; } }
            public string Uutien { get { return ut; } }
            public string Dantoc { get { return dan_toc; } }
            public string Ngaysinh { get { return ngay_sinh; } }

            private string[] nguyen_vong = new string[5];
            public string[] NV { get { return nguyen_vong; } }

            public string[] diem = new string[13];
            public string[] Diem { get { return diem; } }

            private string[] monthi = new string[5];
            public string[] Mon_thi { get { return monthi; } }

            private double[] kq = new double[5];
            public double[] KQ { get { return kq; } }

            private double[] diemthi = new double[13];
            public double[] DIEM { get { return diemthi; } }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sql"></param>
            public void getdata(int stt)
            {
                string dknv = @"C:\Users\Nam\Desktop\BT_\BT\dulieu\dknv-bk.txt";
                string[] dk = File.ReadAllLines(dknv);
                string[] str = dk[stt + 1].Split(' ');
                int n = str.Length;
               
                NV[1] = str[1];
                NV[2] = str[3];
                NV[3] = str[5];
                NV[4] = str[7];
                Mon_thi[1] = str[2];
                Mon_thi[2] = str[4];
                Mon_thi[3] = str[6];
                Mon_thi[4] = str[8];


                string csdl = @"C:\Users\Nam\Desktop\BT_\BT\dulieu\csdl-bk.txt";
                string[] cs = File.ReadAllLines(csdl);
                string[] s = cs[stt + 1].Split('.');
                sbd = s[0];
                ten = s[1];
                ngay_sinh = s[2];
                kv = s[3];
                dan_toc = s[4];
                ut = s[5];
                for (int i = 6; i < s.Length; i++)
                {
                    diem[i - 6] = s[i];
                }

            }
            private double _diem;
            public double diemmon(string tenmon)
            {
                switch (tenmon)
                {
                    case "Toan": _diem = diemthi[0]; break;
                    case "Van": _diem = diemthi[1]; break;
                    case "Ly": _diem = diemthi[2]; break;
                    case "Hoa": _diem = diemthi[3]; break;
                    case "Sinh": _diem = diemthi[4]; break;
                    case "Su": _diem = diemthi[5]; break;
                    case "Dia": _diem = diemthi[6]; break;
                    case "Anh": _diem = diemthi[7]; break;
                    case "Nga": _diem = diemthi[8]; break;
                    case "Phap": _diem = diemthi[9]; break;
                    case "Trung": _diem = diemthi[10]; break;
                    case "Duc": _diem = diemthi[11]; break;
                    case "Nhat": _diem = diemthi[12]; break;
                }
                return _diem;
            }
           
            public void getvalue(int stt)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (Diem[i] == "NA") diemthi[i] = 0;
                    else diemthi[i] = double.Parse(Diem[i]);
                }
                for (int i = 1; i <= 4; i++)
                {
                    nguyen_vong[i] = NV[i];
                    monthi[i] = Mon_thi[i];
                }
            }

          

            private double diemkv;
            public double DiemKV(string tenkhuvuc)
            {
                switch (tenkhuvuc)
                {
                    case "\"KV1\"": diemkv = 1.5; break;
                    case "\"KV2-NT\"": diemkv = 1; break;
                    case "\"KV2\"": diemkv = 0.5; break;
                    case "\"KV3\"": diemkv = 0; break;
                }
                return diemkv;
            }
            private double diemDT;


            public double Diemdantoc(string nhomdt)
            {
                switch (nhomdt)
                {
                    case "\"NDT1\"": diemDT = 2; break;
                    case "\"NDT2\"": diemDT = 1; break;
                    case "\"Khong\"": diemDT = 0; break;
                }
                return diemDT;
            }

            private double diemUT;

            private double diemuutien(string s)
            {
                if (s == "UT") diemUT = 1;
                else diemUT = 0;
                return diemUT;
            }


            public void Xu_ly()
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (monthi[i] == "NA") kq[i] = -1;
                    else
                    {
                        string[] s = monthi[i].Split(',');

                        if (s[3] == "1")
                        {
                            kq[i] = (diemmon(s[0]) * 2 + diemmon(s[1]) + diemmon(s[2]) + DiemKV(Khuvuc)) / 4 + diemuutien(Uutien) + Diemdantoc(Dantoc);

                        }
                        else
                        {
                            kq[i] = (diemmon(s[0]) + diemmon(s[1]) + diemmon(s[2]) + DiemKV(Khuvuc)) / 3 + diemuutien(Uutien) + Diemdantoc(Dantoc);
                        }
                    }
                }
            }
            public void insert(string sql)
            {
                SQLiteConnection connect = new SQLiteConnection(@"Data Source = C:\Users\Nam\Desktop\26.db");
                connect.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = connect;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
        static void Main(string[] args)
        {     
               class1 tt = new class1();
               Console.WriteLine("Dang insert du lieu!!!");
              
               for (int i = 0; i < 100; i++)
               {
                   tt.getdata(2600 + i);
                   tt.getvalue(2600 + i);
                   tt.Xu_ly();
                     
                   for (int j = 1; j <= 4; j++)
                   {
                       if (tt.KQ[j] > 0)
                       {
                           string sql = "INSERT INTO nvxt VALUES('" + tt.SBD + "','" + j + "','" + tt.NV[j] + "','" + tt.KQ[j] + "')";
                           tt.insert(sql);
                       }
                   }
                   
               }
               Console.WriteLine("OK!!! Insert thanh cong!!!");
               Console.ReadLine();
        }
    }
}