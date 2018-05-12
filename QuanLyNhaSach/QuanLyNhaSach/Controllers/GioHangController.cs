using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.Controllers
{
    public class GioHangController : Controller
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }

        //Lấy giỏ hàng
        public List<Giohang> Laygiohang()
        {
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if(listGiohang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì khởi tạo list giỏ hàng
                listGiohang = new List<Giohang>();
                Session["Giohang"] = listGiohang;
            }
            return listGiohang;
        }

        //Thêm hàng vào giỏ
        public ActionResult ThemGiohang(int iMasach, string strURL)
        {
            //Lấy ra session giỏ hàng
            List<Giohang> listGiohang = Laygiohang();
            //Kiểm tra sách này tồn tại trong Session["giohang"] chưa??
            Giohang sanpham = listGiohang.Find(n => n.iMasach == iMasach);
            if(sanpham == null)
            {
                sanpham = new Giohang(iMasach);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluongban++;
                return Redirect(strURL);
            }
        }

        //Tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if(listGiohang != null)
            {
                iTongSoLuong = listGiohang.Sum(n => n.iSoluongban);
            }
            return iTongSoLuong;
        }

        //Tính tổng tiền
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
                iTongTien = listGiohang.Sum(n => n.dThanhtien);
            return iTongTien;
        }

        //Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            List<Giohang> listGiohang = Laygiohang();
            if (listGiohang.Count == 0)
                return RedirectToAction("Index", "BookStore");
            ViewBag.TongSoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGiohang);
        }

        //Tạo partial view để hiện thông tin giỏ hàng
        public ActionResult GiohangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        //Xóa giỏ hàng
        public ActionResult XoaGiohang(int iMaSP)
        {
            //Lấy giỏ hàng từ session
            List<Giohang> listGiohang = Laygiohang();
            //Kiểm tra sách đã có trong session["Giohang"]
            Giohang sanpham = listGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            //Nếu tồn tại thì cho sửa số lượng
            if (sanpham != null)
            {
                listGiohang.RemoveAll(n => n.iMasach == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (listGiohang.Count == 0)
                return RedirectToAction("Index", "BookStore");
            return RedirectToAction("GioHang");
        }

        //Cập nhật giỏ hàng
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            //Lấy giỏ hàng từ session
            List<Giohang> listGiohang = Laygiohang();
            //Kiểm tra sách đã có trong session["Giohang"]
            Giohang sanpham = listGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            //Nếu tồn tại thì cho sửa số lượng
            if (sanpham != null)
                sanpham.iSoluongban = int.Parse(f["txtSoluongban"].ToString());
            return RedirectToAction("GioHang");
        }

        //Xóa tất cả giỏ hàng
        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("Index", "BookStore");
        }

        //Hiện thị view đặt hàng để cập nhật các thông tin cho đơn hàng
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
                return RedirectToAction("Dangnhap", "User");
            if (Session["Giohang"] == null)
                return RedirectToAction("Index", "BookStore");

            List<Giohang> listGiohang = Laygiohang();
            ViewBag.TongSoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGiohang);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            //Thêm đơn hàng
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiaohang = DateTime.Parse(ngaygiao);
            ddh.HTGiaohang = false;
            ddh.HTThanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Thêm chi tiết đơn hàng
            foreach(var item in gh)
            {
                CTDATHANG ctdh = new CTDATHANG();
                ctdh.SoDH = ddh.SoDH;
                ctdh.Masach = item.iMasach;
                ctdh.Soluong = item.iSoluongban;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CTDATHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}