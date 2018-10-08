using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class FilterController : Controller
    {
        public parentClass modelClass = new parentClass();
        public ChildrenClass childrenClass = new ChildrenClass();

        public JsonResult Checkfilter(string[] child_names)
        {
            return Json(modelClass.GetRelationOfTestcaseAndTagCode(child_names), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTestCasesForParent(int parent_id)
        {
            return Json(modelClass.GetTestCasesForParent(parent_id), JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult GetChildren(int f_parent_id)
        {
            return Json(childrenClass.GetChildren(f_parent_id), JsonRequestBehavior.AllowGet);
        }

        // GET: Parents
        public ActionResult Index()
        {
            //if ((Object)Session["Login"] == null)
            //{
            //    return RedirectToAction("Login", "Users");
            //}
            if ((Object)Session["Login"] == null)
                ViewBag.isLogin = false;
            else 
                ViewBag.isLogin = true;
            return View(modelClass.Read());
        }

        // GET: Parents
        public ActionResult UntaggedIds()
        {
            //if ((Object)Session["Login"] == null)
            //{
            //    return RedirectToAction("Login", "Users");
            //}
            //ViewBag.isLogin = true;
            if ((Object)Session["Login"] == null)
                ViewBag.isLogin = false;
            else
                ViewBag.isLogin = true;

            TcaseClass tc = new TcaseClass();
            return View(tc.GetUntaggedTcases());
        }
        // GET: Parents
        public ActionResult ParentAndChildren()
        {
            //if ((Object)Session["Login"] == null)
            //{
            //    return RedirectToAction("Login", "Users");
            //}
            //ViewBag.isLogin = true;
            if ((Object)Session["Login"] == null)
                ViewBag.isLogin = false;
            else
                ViewBag.isLogin = true;

            TcaseClass tc = new TcaseClass();
            return View(childrenClass.Read());
        }
        // GET: Parents
        public ActionResult Tags()
        {
            //if ((Object)Session["Login"] == null)
            //{
            //    return RedirectToAction("Login", "Users");
            //}
            //ViewBag.isLogin = true;
            if ((Object)Session["Login"] == null)
                ViewBag.isLogin = false;
            else
                ViewBag.isLogin = true;

            TagsClass tc = new TagsClass();
            return View(tc.ReadWithUsedNumber());
        }
        
            
    }
}