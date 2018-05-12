using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.Controllers
{
    public class UserController : Controller
    {
        dbQLBansachDataContext db = new dbQLBansachDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            //Gán giá trị
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["DiachiKH"];
            var email = collection["Email"];
            var dienthoai = collection["DienthoaiKH"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
                ViewData["Loi1"] = "Họ tên không được để trống";
            else if (hoten.Length > 50)
                ViewData["Loi1"] = "Họ tên không được vượt quá 50 ký tự";
            else if (String.IsNullOrEmpty(tendn))
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            else if (tendn.Length > 15)
                ViewData["Loi2"] = "Tên đăng nhập không được vượt quá 15 ký tự";
            else if (String.IsNullOrEmpty(matkhau))
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            else if (matkhau.Length > 15)
                ViewData["Loi3"] = "Mật khẩu không được vượt quá 15 ký tự";
            else if (String.IsNullOrEmpty(matkhaunhaplai))
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            else if (matkhaunhaplai != matkhau)
                ViewData["Loi4"] = "Phải nhập lại đúng mật khẩu";
            else if (String.IsNullOrEmpty(diachi))
                ViewData["Loi5"] = "Phải nhập địa chỉ";
            else if (diachi.Length > 50)
                ViewData["Loi5"] = "Địa chỉ không được vượt quá 50 ký tự";
            else if (String.IsNullOrEmpty(email))
                ViewData["Loi6"] = "Phải nhập email";
            else if (email.Length > 50)
                ViewData["Loi6"] = "Email không được vượt quá 50 ký tự";
            else if (String.IsNullOrEmpty(dienthoai))
                ViewData["Loi7"] = "Phải nhập điện thoại";
            else if (dienthoai.Length > 10)
                ViewData["Loi7"] = "Không được vượt quá 10 ký tự";
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                kh.HoTenKH = hoten;
                kh.TenDN = tendn;
                kh.Matkhau = matkhau;
                kh.DiachiKH = diachi;
                kh.Email = email;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            else if(String.IsNullOrEmpty(matkhau))
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TenDN == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    //ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}