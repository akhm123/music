using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using music.Models;

namespace music.Controllers
{
    public class SongsController : Controller
    {
        private dbcon db = new dbcon();

        public ActionResult DisplaySong()
        {
            //Console.WriteLine(System.IO.File.Exists("~/Models/dbcon.cs"));
            ////if(System.IO.File.Exists("~/Models/dbcon.cs"))
            //    ViewBag.s = Server.MapPath("~/App_Data/file_example_MP3_700KB.mp3");

            //ViewBag.song = db.song;
            string user = User.Identity.GetUserName();
            if (user == "")
            {

                ViewBag.songs = db.song;
            }
            else
            {
                List<Song> s = db.song.ToList();
                List<FavouriteSong> f = (db.favouritesong.Where(x => x.user == user)).ToList();
                // var id1 = Convert.ToInt32(id);
                // Console.WriteLine(id1);
                // ViewBag.songs = db.song;

                List<Song> l = new List<Song>();

              // var id= db.song.Select(x => x.SongId).ToList();
                   

                //int flag = 0;
               
                    foreach (var item in f)
                    {
                        // var id1 = Convert.ToInt32(f.SongId);
                        Song f1 = db.song.Where(x => x.SongId == item.SongId).FirstOrDefault();
                        if (f1 != null)
                        {
                            //flag = 1;
                            l.Add(f1);
                        }
                        //else

                    }

               var l1= s.Except(l);
                
                Console.WriteLine(l);
                // if (flag == 1)
                ViewBag.songs = l1;
            }
            ViewBag.Favourite = (db.favouritesong.Where(x => x.user == user)).ToList();
            return View();
           
        }


        public int AddToFavourite(FavouriteSong fs)
        {
            FavouriteSong f = new FavouriteSong();
            f.SongName = fs.SongName;
            f.SongId = fs.SongId;
            f.user = User.Identity.GetUserName();
            
                var f1 = db.favouritesong.Where(x => x.SongId == f.SongId && x.user == f.user).FirstOrDefault();
                Console.WriteLine(f1);
                if (f1 == null)
                {
                    db.favouritesong.Add(f);
                    db.SaveChanges();
                    return f.SongId;

                }

                return 0;
            


           
           
                //return RedirectToAction("/Account/Login");
            
}
  public ActionResult Search(string searchString)
        {
            var song = from s in db.song
                          select s;
          //  var searchString = Request.QueryString[""];
            if (!string.IsNullOrEmpty(searchString))
            {
                //courses = courses.Where(x => x.CourseName == searchString);
                ViewBag.Songs = song.Where(x => x.name.Contains(searchString) || x.singer.Contains(searchString)).ToList() ;
               
                   
               
            }
            else
            {
                ViewBag.Songs = song;
            }
           
         return View("DisplaySong");
        }

 public ActionResult RemoveFavourite(FavouriteSong fs)
        {
            
            return RedirectToAction("DisplaySong");

        }
        public ActionResult DisplayFavouriteSong()
        {
           
            string user = User.Identity.GetUserName();
            
            if (user != "")
            {
                ViewBag.FavouriteSong = db.favouritesong.Where(x => x.user == user).ToList();

                return View();
            }

            return RedirectToAction("Login", "Account");
            
        }

        // GET: Songs
        public ActionResult Index()
        {
            return View(db.song.ToList());
        }

        // GET: Songs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.song.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // GET: Songs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SongId,name,type,path,singer")] Song song)
        {
            if (ModelState.IsValid)
            {
                db.song.Add(song);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(song);
        }

        // GET: Songs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.song.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SongId,name,type,path,singer")] Song song)
        {
            if (ModelState.IsValid)
            {
                db.Entry(song).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(song);
        }

        // GET: Songs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.song.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Song song = db.song.Find(id);
            db.song.Remove(song);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
