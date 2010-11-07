<%@ WebHandler Language="C#" Class="GetDownloadAdmin" %>
using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Media;

public class GetDownloadAdmin : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        int downloadID = CommonHelper.QueryStringInt("DownloadID");
        Download download = DownloadManager.GetDownloadByID(downloadID);
        if (download == null)
        {
            returnError(context, string.Format("Download is not available any more. Download ID={0}", downloadID));
            return;
        }

        if (download.UseDownloadURL)
        {
            //use URL
            if (String.IsNullOrEmpty(download.DownloadURL))
            {
                returnError(context, string.Format("Download URL is empty. Download ID={0}", downloadID));
                return;
            }

            context.Response.Redirect(download.DownloadURL);
        }
        else
        {
            //use stored data
            if (download.DownloadBinary == null)
            {
                returnError(context, string.Format("Download data is not available any more. Download ID={0}", downloadID));
                return;
            }

            context.Response.Clear();
            context.Response.ContentType = download.ContentType;
            context.Response.AddHeader("Content-disposition", string.Format("attachment;filename={0}{1}", downloadID, download.Extension));

            using (MemoryStream ms = new MemoryStream(download.DownloadBinary))
            {
                int length;
                long dataToRead = download.DownloadBinary.Length;
                byte[] buffer = new Byte[10000];

                while (dataToRead > 0)
                {
                    if (context.Response.IsClientConnected)
                    {
                        length = ms.Read(buffer, 0, 10000);
                        context.Response.OutputStream.Write(buffer, 0, length);
                        context.Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                        dataToRead = -1;
                }
            }
        }
    }
    
    private void returnError(HttpContext context, string Message)
    {
        context.Response.Clear();
        context.Response.Write(Message);
        context.Response.Flush(); ;
    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}