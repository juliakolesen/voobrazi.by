<%@ WebHandler Language="C#" Class="GetDownload" %>
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
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;

public class GetDownload : IHttpHandler
{

    private void processOrderProductVariantDownload(HttpContext context, int orderProductVariantID)
    {
        if (NopContext.Current.User == null)
            context.Response.Redirect("~/Login.aspx");

        OrderProductVariant orderProductVariant = OrderManager.GetOrderProductVariantByID(orderProductVariantID);
        if (orderProductVariant == null)
        {
            returnError(context, "Order product variant doesn't exist.");
            return;
        }

        Order order = OrderManager.GetOrderByID(orderProductVariant.OrderID);
        if (order == null)
        {
            returnError(context, string.Format("Order doesn't exist. ID={0}", orderProductVariant.OrderID));
            return;
        }

        if (!OrderManager.AreDownloadsAllowed(order))
        {
            returnError(context, string.Format("Downloads are not allowed. ID={0}", order.OrderID));
            return;
        }

        if (order.CustomerID != NopContext.Current.User.CustomerID)
        {
            returnError(context, string.Format("This is not your order. ID={0}", order.OrderID));
            return;
        }

        ProductVariant productVariant = orderProductVariant.ProductVariant;
        if (productVariant == null)
        {
            returnError(context, string.Format("Product variant doesn't exist. ID={0}", orderProductVariant.ProductVariantID));
            return;
        }

        if (!productVariant.IsDownload)
        {
            returnError(context, string.Format("Product variant is not downloadable. ID={0}", orderProductVariant.ProductVariantID));
            return;
        }

        if (!productVariant.UnlimitedDownloads && orderProductVariant.DownloadCount >= productVariant.MaxNumberOfDownloads)
        {
            returnError(context, string.Format("You have reached maximum number of downloads. ID={0}", orderProductVariant.ProductVariantID));
            return;
        }

        Download download = productVariant.Download;
        if (download == null)
        {
            returnError(context, string.Format("Download is not available any more. ID={0}", productVariant.DownloadID));
            return;
        }

        if (download.UseDownloadURL)
        {
            //use URL
            if (String.IsNullOrEmpty(download.DownloadURL))
            {
                returnError(context, string.Format("Download URL is empty. Download ID={0}", download.DownloadID));
                return;
            }

            orderProductVariant = OrderManager.IncreaseOrderProductDownloadCount(orderProductVariant.OrderProductVariantID);

            context.Response.Redirect(download.DownloadURL);
        }
        else
        {
            if (download.DownloadBinary == null)
            {
                returnError(context, string.Format("Download data is not available any more. Download ID={0}", download.DownloadID));
                return;
            }

            //use stored data
            context.Response.Clear();
            context.Response.ContentType = download.ContentType;
            context.Response.AddHeader("Content-disposition",
                string.Format("attachment;filename={0}{1}", orderProductVariant.OrderProductVariantID, download.Extension));

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

            orderProductVariant = OrderManager.IncreaseOrderProductDownloadCount(orderProductVariant.OrderProductVariantID);
        }
    }

    private void processSampleDownloadProductVariant(HttpContext context, int productVariantID)
    {
        ProductVariant productVariant = ProductManager.GetProductVariantByID(productVariantID);
        if (productVariant == null)
        {
            returnError(context, string.Format("Product variant doesn't exist."));
            return;
        }
        
        if (!productVariant.HasSampleDownload)
        {
            returnError(context, string.Format("Product variant doesn't have a sample download. ID={0}", productVariant.ProductVariantID));
            return;
        }

        Download sampleDownload = productVariant.SampleDownload;
        if (sampleDownload == null)
        {
            returnError(context, string.Format("Sample download is not available any more. ID={0}", productVariant.SampleDownloadID));
            return;
        }

        if (sampleDownload.UseDownloadURL)
        {
            //use URL
            if (String.IsNullOrEmpty(sampleDownload.DownloadURL))
            {
                returnError(context, string.Format("Download URL is empty. Download ID={0}", sampleDownload.DownloadID));
                return;
            }

            context.Response.Redirect(sampleDownload.DownloadURL);
        }
        else
        {
            if (sampleDownload.DownloadBinary == null)
            {
                returnError(context, string.Format("Download data is not available any more. Download ID={0}", sampleDownload.DownloadID));
                return;
            }

            //use stored data
            context.Response.Clear();
            context.Response.ContentType = sampleDownload.ContentType;
            context.Response.AddHeader("Content-disposition",
                string.Format("attachment;filename={0}{1}", productVariant.ProductVariantID, sampleDownload.Extension));

            using (MemoryStream ms = new MemoryStream(sampleDownload.DownloadBinary))
            {
                int length;
                long dataToRead = sampleDownload.DownloadBinary.Length;
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

    public void ProcessRequest(HttpContext context)
    {
        int orderProductVariantID = CommonHelper.QueryStringInt("OrderProductVariantID");
        int sampleDownloadProductVariantID = CommonHelper.QueryStringInt("SampleDownloadProductVariantID");

        if (orderProductVariantID > 0)
            processOrderProductVariantDownload(context, orderProductVariantID);
        else if (sampleDownloadProductVariantID > 0)
            processSampleDownloadProductVariant(context, sampleDownloadProductVariantID);
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