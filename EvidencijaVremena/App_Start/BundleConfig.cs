﻿using System.Web;
using System.Web.Optimization;

namespace EvidencijaVremena
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/chart").Include(
				"~/Scripts/Chart.js"));

			bundles.Add(new ScriptBundle("~/bundles/calendar").Include(
				"~/Scripts/fullcalendar.js",
				"~/Scripts/lang/hr.js"));

			bundles.Add(new ScriptBundle("~/bundles/moment").Include(
				"~/Scripts/moment.js"));

			bundles.Add(new StyleBundle("~/Content/calendar_css").Include(
					  "~/Content/fullcalendar.css"));

			bundles.Add(new ScriptBundle("~/bundles/duallist").Include(
				"~/Scripts/jquery.bootstrap-duallistbox.js"));

			bundles.Add(new StyleBundle("~/Content/duallist_css").Include(
					  "~/Content/bootstrap-duallistbox.css"));
		}
	}
}
