using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNhaSach.Models
{
    public class Giohang
    {
        //Tạo đối tượng data chứa dữ liệu từ model bdBansach đã tạo
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        public int iMasach { set; get; }
        public string sTensach { set; get; }
        public string sHinhminhhoa { set; get; }
        //public string sDonvitinh { set; get; }
        public double dDongia { set; get; }
        public int iSoluongban { set; get; }
        public double dThanhtien
        {
            get { return iSoluongban * dDongia; }
        }

        //Khởi tạo giỏ hàng theo Masach được truyền vào với số lượng là 1
        public Giohang(int Masach)
        {
            iMasach = Masach;
            SACH sach = data.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sHinhminhhoa = sach.Hinhminhhoa;
            dDongia = double.Parse(sach.Dongia.ToString());
            iSoluongban = 1;
        }
    }
}