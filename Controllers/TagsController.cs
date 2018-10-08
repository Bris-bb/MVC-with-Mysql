using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class TagsController : Controller
    {
        public TagsClass modelClass = new TagsClass();
        public Case_tagClass case_tagClass = new Case_tagClass();

        

        [HttpPost]
        public JsonResult GetTags()
        {
            return Json(modelClass.Read(), JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public JsonResult GetTestandTags()
        {
            return Json(case_tagClass.Read(), JsonRequestBehavior.AllowGet);
        }

        // GET: Tags
        public ActionResult Index()
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            return View(modelClass.Read());
        }
        // GET: Tags/Details/5
        public ActionResult Details(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            return View(modelClass.Detail((int)id));
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tag_id,tag_name")] TagsClass tag)
        {
            ViewBag.isExist = "NO";
            if (modelClass.isExist(tag))
            {
                //when isExist tag_name
                ViewBag.isExist = "Same tag Name is exist already.";
            }

            else
            {
                if (ModelState.IsValid)
                {
                    modelClass.Add(tag);
                    return RedirectToAction("Index");
                }
            }
            return View(tag);
        }



        // GET: Tags/Retire/5
        public ActionResult Retire(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            return View(modelClass.Detail((int)id));
        }

        // POST: Tags/Retire/5
        [HttpPost, ActionName("Retire")]
        [ValidateAntiForgeryToken]
        public ActionResult RetireConfirmed(int id)
        {
            modelClass.Retire( modelClass.Detail((int)id) );
            return RedirectToAction("Index");
        }


        // GET: Tags/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            return View(modelClass.Detail((int)id));
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tag_id,tag_name")] TagsClass tag)
        {
            ViewBag.isExist = "NO";
            if (modelClass.isExist(tag))
            {
                ViewBag.isExist = "Same Tag Name is exist already.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    modelClass.Edit(tag);
                    return RedirectToAction("Index");
                }
            }
            return View(tag);
        }
    }
}