using System.Linq;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Security;
using System.Data.Entity.Validation;
using System.Web.Helpers;

using System.Data.SqlClient;
using EvidencijaVremena;
using EvidencijaVremena.Models;

namespace EvidencijaVremena.Controllers
{
	public class AutorizacijaController : Controller
	{
		private EvidencijaVremenaEntities db;

		public AutorizacijaController()
		{
			db = new EvidencijaVremenaEntities();
		}

		[HttpGet]
		public ActionResult Prijava()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Prijava(PrijavaModel model)
		{
			var korisnik = db.Korisnik.Where(k => k.KorisnickoIme == model.KorisnickoIme).FirstOrDefault();
			if (ModelState.IsValid)
				if (korisnik != null)
				{
					{
						if (Crypto.VerifyHashedPassword(korisnik.Lozinka, model.Lozinka))
						{
							Debug.WriteLine("uspjesna prijava");
							FormsAuthentication.SetAuthCookie(model.KorisnickoIme, false);
							return RedirectToAction("Index", "Predmet");
						}
						else
						{
							Debug.WriteLine("kriva lozinka");
							ModelState.AddModelError("error_loz", "Pogrešna Lozinka");
						}
					}
				}
				else
				{
					ModelState.AddModelError("error_kor_ime", "Nepostojeće korisničko ime");
				}
			else
			{
				Debug.WriteLine("model not valid");
			}
			return View();
		}

		public ActionResult Odjava()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Prijava", "Autorizacija");
		}

		[HttpGet]
		public ActionResult Registracija()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Registracija(RegistracijaModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (db.Korisnik.Where(k => k.KorisnickoIme == model.KorisnickoIme).Count() != 0)
			{
				ModelState.AddModelError("kor_ime_uniq", "Odabrano korisničko ime već postoji");
				return View();
			}

			Korisnik korisnik = new Korisnik();
			korisnik.KorisnickoIme = model.KorisnickoIme;
			korisnik.Email = model.Email;
			korisnik.Lozinka = Crypto.HashPassword(model.Lozinka);
			korisnik.Uloga = 0;

			db.Korisnik.Add(korisnik);
			try
			{
				db.SaveChanges();
			}
			catch (SqlException sqle)
			{
				foreach (SqlErrorCollection errors in sqle.Errors)
				{
					Debug.WriteLine(errors.ToString());
				}
			}
			catch (DbEntityValidationException deve)
			{
				foreach (var validationErrors in deve.EntityValidationErrors)
				{
					foreach (var error in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage);
					}
				}
			}
			FormsAuthentication.SetAuthCookie(korisnik.KorisnickoIme, false);
			return RedirectToAction("Index", "Predmet");
		}
	}
}