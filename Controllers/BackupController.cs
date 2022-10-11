using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShawkanyDb.Models;
using System.Collections.Generic;
using System.IO;

namespace ShawkanyDb.Controllers
{
    [Authorize(Roles = "Admin Department")]
    public class BackupController : Controller
    {
        // GET: Backup

        SQLLocalBackup backup = new SQLLocalBackup();
        public ActionResult Index()
        {

            if (TempData["updated"] != null)
            {
                ViewBag.Alert = TempData["updated"];
            }
            else
            {
                ViewBag.Alert = "empty";
            }

            return View();
        }
        //public string Restoredb(IFormFile Backupfile)
        //{
        //    SQLLocalBackup restore = new SQLLocalBackup();
        //    string s = restore.DoLocalRestore(Backupfile);
        //    return s;


        //}
        public IActionResult Create(string AlocalPath)
        {
            backup.DoLocalBackup(AlocalPath);
            TempData["updated"] = "1";
            return RedirectToAction("Index", "Backup");

        }

        public IActionResult Restoredb(string Restorepath)
        {
            backup.Restoredb(Restorepath);
            TempData["updated"] = "2";
            return RedirectToAction("Index", "Backup");

        }



        public JsonResult List()
        {
            List<string> lst = new List<string>();
            string[] AremoteTempPath = Directory.GetFiles("C:\\Backup");
            foreach (string Path in AremoteTempPath)
            {
                lst.Add(Path);
            }
            return Json(lst);
        }

        public IActionResult DeleteBackup(string path)
        {
            path = path.Replace("/", "\\");
            //System.Net.WebClient web = new System.Net.WebClient();
            //web.DownloadFile(path, "Backup" + DateTime.Now.ToLongDateString());
            System.IO.File.Delete(path);

            TempData["updated"] = "3";
            return RedirectToAction("Index", "Backup");
        }

        public FileResult DownloadBackup(string path)
        {
            // path = path.Replace("/", "\\");
            //System.Net.WebClient web = new System.Net.WebClient();
            //web.DownloadFile(path, "Backup" + DateTime.Now.ToLongDateString());
            //return Json("Backup Downloaded", JsonRequestBehavior.AllowGet);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            //string fileName = "Backup" + DateTime.Now.ToLongDateString()+".bak";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, path.Replace("C:\\Backup\\", ""));
        }

    }
}