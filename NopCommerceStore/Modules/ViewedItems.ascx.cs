using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Orders;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ViewedItems : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }

        protected void BindGrid()
        {
            ProductCollection products = GetProducts();

            if (products.Count > 0)
            {
                this.dlProducts.DataSource = products;
                this.dlProducts.DataBind();
                this.viewedItemsLink.Visible = true;
            }
            else
            {
                this.viewedItemsLink.Visible = false;
            }
        }

        private ProductCollection GetProducts()
        {
            int totalItems = 0;
            ViewedItemsCollection collection = ViewedItemManager.GetCurrentViewedItem(10, 0, out totalItems);
            List<int> productsIDs = collection.Select(x => x.ProductVariantID).ToList();
            ProductCollection productCollection = new ProductCollection();
            foreach (var productID in productsIDs)
            {
                ProductVariant product = ProductManager.GetProductVariantByID(productID);
                productCollection.Add(product.Product);
            }

            return productCollection;
        }

    }
}