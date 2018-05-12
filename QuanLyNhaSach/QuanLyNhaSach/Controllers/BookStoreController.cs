using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyNhaSach.Models;

using PagedList;
using PagedList.Mvc;

namespace QuanLyNhaSach.Controllers
{
    public class BookStoreController : Controller
    {
        //tạo 1 đối tượng chứa toàn bộ csdl bán sách
        dbQLBansachDataContext data = new dbQLBansachDataContext();

        private List<SACH> Laysachmoi(int count)
        {
            //sắp xếp sách giảm dần theo Ngaycapnhat, lấy count dòng đầu
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        // GET: BookStore
        //public ActionResult Index()
        //{
            //Lấy 6 quyển sách mới nhất
        //    var sachmoi = Laysachmoi(6);
        //    return View(sachmoi);
        //}

        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }

        public ActionResult Nhaxuatban()
        {
            var nhaxuatban = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxuatban);
        }

        public ActionResult SPTheochude(int id, int? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pageSize = 6;
            //tạo biến số trang
            int pageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach.ToPagedList(pageNum, pageSize));
        }

        public ActionResult SPTheoNXB(int id, int? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pageSize = 6;
            //tạo biến số trang
            int pageNum = (page ?? 1);
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Index(int? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pageSize = 6;
            //tạo biến số trang
            int pageNum = (page ?? 1);

            //lấy top 18 album bán chạy nhất
            var sachmoi = Laysachmoi(18);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Details (int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
    }
}