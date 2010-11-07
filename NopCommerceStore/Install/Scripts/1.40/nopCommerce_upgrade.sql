--upgrade scripts from nopCommerce 1.30 to 1.40




--new locale resources
declare @resources xml
set @resources='
<Language LanguageID="7">
 <LocaleResource Name="Address.FirstNameIsRequired">
    <Value>First name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Address.LastNameIsRequired">
    <Value>Last name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Address.PhoneNumberIsRequired">
    <Value>Phone number is required</Value>
  </LocaleResource>
  <LocaleResource Name="Address.StreetAddressIsRequired">
    <Value>Address is required</Value>
  </LocaleResource>
  <LocaleResource Name="Address.CityIsRequired">
    <Value>City is required</Value>
  </LocaleResource>
  <LocaleResource Name="Address.ZipPostalCodeIsRequired">
    <Value>Zip / Postal code is required</Value>
  </LocaleResource>
  <LocaleResource Name="Products.AlsoPurchased">
    <Value>Customers Who Bought This Item Also Bought</Value>
  </LocaleResource>
  <LocaleResource Name="Products.DownloadSample">
    <Value>Download sample</Value>
  </LocaleResource>
  <LocaleResource Name="Forum.ActiveDiscussions">
    <Value>Active discussions</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Order#">
    <Value>Order# {0}</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.OrderDate">
    <Value>Date: {0}</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.BillingInformation">
    <Value>Billing Information:</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Name">
    <Value>Name: {0}</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Phone">
    <Value>Phone: {0}</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Fax">
    <Value>Fax: {0}</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Address">
    <Value>Address: {0}</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Address2">
    <Value>Address 2: {0}</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.ShippingInformation">
    <Value>Shipping Information:</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Product(s)">
    <Value>Product(s)</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.ProductName">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.ProductPrice">
    <Value>Price</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.ProductQuantity">
    <Value>Q-ty</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.ProductTotal">
    <Value>Total</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Sub-Total">
    <Value>Sub-total:</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Shipping">
    <Value>Shipping:</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.PaymentMethodAdditionalFee">
    <Value>Payment Method Additional Fee:</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.Tax">
    <Value>Tax:</Value>
  </LocaleResource>
  <LocaleResource Name="PDFInvoice.OrderTotal">
    <Value>Order total:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Dashboard">
    <Value>Dashboard</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StoreStatistics">
    <Value>Store Statistics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderAverageReport.OrderTotals">
    <Value>Order totals</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderAverageReport.OrderStatus">
    <Value>Order Status</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderAverageReport.Today">
    <Value>Today</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderAverageReport.ThisWeek">
    <Value>This Week</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderAverageReport.ThisMonth">
    <Value>This Month</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderAverageReport.ThisYear">
    <Value>This Year</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderAverageReport.AllTime">
    <Value>All Time</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderStatistics.IncompleteOrders">
    <Value>Incomplete orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderStatistics.Item">
    <Value>Item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderStatistics.Total">
    <Value>Total</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderStatistics.Count">
    <Value>Count</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderStatistics.TotalUnpaidOrders">
    <Value>Total unpaid orders (pending payment status)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderStatistics.TotalNotShippedOrders">
    <Value>Total not yet shipped orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderStatistics.TotalIncompleteOrders">
    <Value>Total incomplete orders (pending order status)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.RegisteredCustomers">
    <Value>Registered customers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.Item">
    <Value>Item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.Count">
    <Value>Count</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.Action">
    <Value>Action</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.InTheLast">
    <Value>In the last</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.7days">
    <Value>7 days</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.14days">
    <Value>14 days</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.month">
    <Value>month</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.year">
    <Value>year</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerStatistics.View">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BestSellersStat.BestSellers">
    <Value>Best Sellers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BestSellersStat.Product">
    <Value>Product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BestSellersStat.TotalCount">
    <Value>Total count</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BestSellersStat.TotalAmount">
    <Value>Total amount (excl tax)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Warnings.Warnings">
    <Value>Warnings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.nopCommerceNews.News">
    <Value>nopCommerce News</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SearchTermStat.PopularSearches">
    <Value>Popular Searches</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SearchTermStat.SearchTerm">
    <Value>Search Term</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SearchTermStat.Count">
    <Value>Count</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.CatalogHome">
    <Value>Catalog Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.intro">
    <Value>Use the links on this page to manage all aspects of your product catalog such as products, categories, manufacturers and attributes.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Categories.Title">
    <Value>Categories</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Categories.TitleDescription">
    <Value>Manage product categories.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Categories.Description1">
    <Value>Product categories are the hierarchical grouping for the products in your catalog (e.g. "computers"). They enable your customers to find what they are looking for.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Categories.Description2">
    <Value>Manage category details, product mappings, SEO options and category discounts.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Products.Title">
    <Value>Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Products.TitleDescription">
    <Value>Manage Products.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Products.Description">
    <Value>Manage products, product variants (SKUs) and product reviews.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Attributes.Title">
    <Value>Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Attributes.TitleDescription">
    <Value>Manage Attributes.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Attributes.Description">
    <Value>Manage product attributes and specification attributes.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Manufacturers.Title">
    <Value>Manufacturers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Manufacturers.TitleDescription">
    <Value>Manage product manufacturers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Manufacturers.Description1">
    <Value>Manage the manufacturers (brands) for products in your catalog. You can map individual products to your manufacturers so that a customer can view products by manufacturer as well as category.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CatalogHome.Manufacturers.Description2">
    <Value>Manage manfacturer details, product mappings and SEO options.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.ProductsHome">
    <Value>Products Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.intro">
    <Value>Use the links on this page to manage the products in your catalog, customer reviews and view product related reports.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.Products.Title">
    <Value>Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.Products.TitleDescription">
    <Value>Manage Products.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.Products.Description1">
    <Value>Your store can have multiple products, mapped to a number of different categories and manufacturers. A product can also have several product variants (SKUs), each with it''s own description, price and attributes.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.Products.Description2">
    <Value>Manage product details, images, SEO options, category mappings, manufacturer mappings, related products, attributes and specifications.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.ProductReviews.Title">
    <Value>Product Reviews</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.ProductReviews.TitleDescription">
    <Value>Manage Product Reviews.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.ProductReviews.Description">
    <Value>Product reviews allow your customers to give you feedback about your products. You must approve product reviews before they are visible in your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.ProductVariantsLowStock.Title">
    <Value>Low Stock Report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.ProductVariantsLowStock.TitleDescription">
    <Value>View product variant low stock report.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductsHome.ProductVariantsLowStock.Description">
    <Value>View product variants that are low in stock. When stock management is enabled for a product, the stock quantities are automatically adjusted when a purchase is made. This report will display all the product variants with a current stock quantity that falls under your minimum stock quantity value (set on each individual product).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.AttributesHome">
    <Value>Attributes Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.intro">
    <Value>Use the links on this page to manage both product attributes and specification attributes. You must create the attribute types first before you can use them with your products.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.ProductAttributes.Title">
    <Value>Product Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.ProductAttributes.TitleDescription">
    <Value>Manage product attributes.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.ProductAttributes.Description">
    <Value>Product attributes (e.g. "Size" or "Color") are used to create inter-dependant product variations in your catalog. After creating a Product Attribute, you can then create specific attribute values for an individual product (e.g. "Red", "Green", "Blue")</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.SpecificationAttributes.Title">
    <Value>Product Specification Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.SpecificationAttributes.TitleDescription">
    <Value>Manage product specification attributes.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AttributesHome.SpecificationAttributes.Description">
    <Value>In addition to standard attributes, you can create product specification attributes to provide your customers with technical details about your product (e.g. "Fabric"). Once defined, you can create specification values for an individual product (e.g. "Cotton"). The customer can view the specification from the specification tab on the product detail page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesHome.SalesHome">
    <Value>Sales Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesHome.intro">
    <Value>Use the links on this page to manage your sales orders and view sales related reports.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesHome.Orders.Title">
    <Value>Manage Orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesHome.Orders.TitleDescription">
    <Value>Manage orders.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesHome.Orders.Description">
    <Value>Manage sales orders. Capture payments and process open orders.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesHome.SalesReport.Title">
    <Value>View Sales Report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesHome.SalesReport.TitleDescription">
    <Value>View sales report.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesHome.SalesReport.Description">
    <Value>View a report of all your sales that match the specified criteria.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.CustomersHome">
    <Value>Customers Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.intro">
    <Value>Use the links on this page to manage your customers and their roles.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.Customers.Title">
    <Value>Manage Customers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.Customers.TitleDescription">
    <Value>Manage customers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.Customers.Description">
    <Value>View customer details such as contact information, address lists and past orders. Assign your customers to specific roles (see role nagement below).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.CustomerRoles.Title">
    <Value>Manage Customer Roles</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.CustomerRoles.TitleDescription">
    <Value>Manage customer roles.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.CustomerRoles.Description1">
    <Value>Customer roles allow you to group your customers. A customer can be a member of more than one role and a role can have many customers. Roles allow you to apply benefits such as free shipping and tax exemption to a group.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomersHome.CustomerRoles.Description2">
    <Value>You can also create a discount promotion and assign this to a customer role (go to Promotions : Discounts to manage discounts).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.PromotionsHome">
    <Value>Promotions Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.intro">
    <Value>Use the links on this page to manage store promotion features, such as affiliates, advertising/marketing campaigns, discounts, pricelists and external promotion providers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Affiliates.Title">
    <Value>Manage Affiliates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Affiliates.TitleDescription">
    <Value>Manage affiliates.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Affiliates.Description1">
    <Value>Affiliates allow you to generate a unique URL for partner sites that link to your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Affiliates.Description2">
    <Value>When a customer clicks this link from an affiliate''s site, the customer is tagged with the affiliate''s unique ID. If the customer proceeds to place an order, the order is also tagged. This allows you to analyse the value of the traffic from the affiliate''s site.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Campaigns.Title">
    <Value>Manage Campaigns</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Campaigns.TitleDescription">
    <Value>Manage campaigns.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Campaigns.Description">
    <Value>You can use campaigns to easily send promotion emails to the customers that are registered on your site. This is a great way of informing your customers of new offers and keep them coming back to your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Discounts.Title">
    <Value>Manage Discounts</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Discounts.TitleDescription">
    <Value>Manage discounts.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Discounts.Description1">
    <Value>Discounts can be used to deduct a set value or percentage from individual items or the total value of an order. You can set a date range for a discount as well as generating coupon codes for your customers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Discounts.Description2">
    <Value>Discounts can also be restricted to a specific customer role, allowing you to reward your higher value customers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Pricelist.Title">
    <Value>Manage Pricelists</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Pricelist.TitleDescription">
    <Value>Manage pricelists.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.Pricelist.Description">
    <Value>You can use the Pricelist feature to generate pricelists that can be consumed by external sites or for the use of customers. Pricelists can be created for your entire product catalog, or for individually selected items. You can also make price adjustments to the items on the pricelist.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.PromotionProvidersHome.Title">
    <Value>Manage Promotion Providers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.PromotionProvidersHome.TitleDescription">
    <Value>Manage promotion providers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionsHome.PromotionProvidersHome.Description">
    <Value>Manage various external promotion providers and sitemap settings for search engines.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionProvidersHome.PromotionProvidersHome">
    <Value>Promotion Providers Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionProvidersHome.intro">
    <Value>Use the links on this page to manage specific promotion providers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionProvidersHome.Froogle.Title">
    <Value>Froogle</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionProvidersHome.Froogle.TitleDescription">
    <Value>Manage froogle.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PromotionProvidersHome.Froogle.Description">
    <Value>Manage the Froogle data feed. Froogle is a service from Google (US only) that allows consumers to easily find the products that they are looking for.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.ContentManagementHome">
    <Value>Content Management Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.intro">
    <Value>Use the links on this page to manage the dynamic content on your site such as opinion polls, news, your store blog, templates and localization resources.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.Polls.Title">
    <Value>Manage Polls</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.Polls.TitleDescription">
    <Value>Manage polls.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.Polls.Description">
    <Value>Polls are a great way to find out your customers opinion on something. You can create multiple custom polls and review the results of existing polls.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.NewsHome.Title">
    <Value>Manage News</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.NewsHome.TitleDescription">
    <Value>Manage news</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.NewsHome.Description">
    <Value>Manage store news items and customer comments.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.BlogHome.Title">
    <Value>Manage Blog</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.BlogHome.TitleDescription">
    <Value>Manage blog</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.BlogHome.Description">
    <Value>Manage your store blog and customer comments.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.Topics.Title">
    <Value>Manage Topics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.Topics.TitleDescription">
    <Value>Manage topics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.Topics.Description">
    <Value>Manage topics.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.Forums.Title">
    <Value>Manage Forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.Forums.TitleDescription">
    <Value>Manage forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.Forums.Description">
    <Value>Manage forums.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.TemplatesHome.Title">
    <Value>Manage Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.TemplatesHome.TitleDescription">
    <Value>Manage templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.TemplatesHome.Description">
    <Value>Manage templates for product, category and manufacturer pages. Also manage email templates for the languages enabled in your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.LocaleStringResources.Title">
    <Value>Manage Localization</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.LocaleStringResources.TitleDescription">
    <Value>Manage localization resources</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ContentManagementHome.LocaleStringResources.Description">
    <Value>Manage localized content in your store. This allows customers of different languages to understand your content.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.NewsHome">
    <Value>News Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.intro">
    <Value>Use the links on this page to manage your store''s news feed.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.NewsSettings.Title">
    <Value>Manage News Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.NewsSettings.TitleDescription">
    <Value>Manage news settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.NewsSettings.Description">
    <Value>Use this page to manage general news settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.News.Title">
    <Value>Manage News Items</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.News.TitleDescription">
    <Value>Manage news items.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.News.Description">
    <Value>Create and manage existing news items with an easy to use editor control.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.NewsComments.Title">
    <Value>Manage News Comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.NewsComments.TitleDescription">
    <Value>Manage news comments.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsHome.NewsComments.Description">
    <Value>Manage customer comments on your news items. You must approve all comments before they are visible in your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.BlogHome">
    <Value>Blog Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.intro">
    <Value>Use the links on this page to manage your store blog.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.BlogSettings.Title">
    <Value>Manage Blog Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.BlogSettings.TitleDescription">
    <Value>Manage blog settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.BlogSettings.Description">
    <Value>Use this page to manage general blog settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.Blog.Title">
    <Value>Manage Blog Entries</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.Blog.TitleDescription">
    <Value>Manage blog entries.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.Blog.Description">
    <Value>Create and manage existing blog entries with an easy to use editor control.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.BlogComments.Title">
    <Value>Manage Blog Comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.BlogComments.TitleDescription">
    <Value>Manage blog comments.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogHome.BlogComments.Description">
    <Value>Manage customer comments on your blog entries. Your must approve all comments before they are visible in your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsHome.ForumsHome">
    <Value>Forums Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsHome.intro">
    <Value>Use the links on this page to manage forums settings for your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsHome.ForumsSettings.Title">
    <Value>Manage Forums Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsHome.ForumsSettings.TitleDescription">
    <Value>Manage forums settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsHome.ForumsSettings.Description">
    <Value>Use this page to manage general forums settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsHome.Forums.Title">
    <Value>Manage Forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsHome.Forums.TitleDescription">
    <Value>Manage forums.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsHomea.Forums.Description">
    <Value>Use this page to manage forums.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.TemplatesHome">
    <Value>Templates Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.intro">
    <Value>Use the links on this page to manage the various content templates used in your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.ProductTemplates.Title">
    <Value>Manage Product Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.ProductTemplates.TitleDescription">
    <Value>Manage product templates.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.ProductTemplates.Description">
    <Value>Product templates allow you to control the formatting of your product pages. After creating a template file, you must set it up here so that it can be used by products in your catalog.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.CategoryTemplates.Title">
    <Value>Manage Category Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.CategoryTemplates.TitleDescription">
    <Value>Manage category templates.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.CategoryTemplates.Description">
    <Value>Category templates allow you to control the formatting of products on your category pages. After creating a template file, you must set it up here so that it can be used by product categories in your catalog.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.ManufacturerTemplates.Title">
    <Value>Manage Manufacturer Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.ManufacturerTemplates.TitleDescription">
    <Value>Manage manufacturer templates.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.ManufacturerTemplates.Description">
    <Value>Manufacturer templates allow you to control the formatting of products on your manufacturer pages. After creating a template file, you must set it up here so that it can be used by manufacturers in your catalog.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.MessageTemplates.Title">
    <Value>Manage Message Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.MessageTemplates.TitleDescription">
    <Value>Manage message templates.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TemplatesHome.MessageTemplates.Description">
    <Value>Message templates provide localized email output to the languages enabled in your store. When a customer places an order on your site, you can ensure that any notifications they receive are in the language of their choice.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.ConfigurationHome">
    <Value>Configuration Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.intro">
    <Value>Use the links on this page to manage various storefront settings including security, display/SEO, shipping, taxes and payment options.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.GlobalSettings.Title">
    <Value>Global Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.GlobalSettings.TitleDescription">
    <Value>Global settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.GlobalSettings.Description">
    <Value>Use global settings to manage general store settings, SEO/Display settings, image settings, mail settings and security settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.PaymentSettingsHome.Title">
    <Value>Payment Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.PaymentSettingsHome.TitleDescription">
    <Value>Payment settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.PaymentSettingsHome.Description">
    <Value>Manage the credit card types and payment methods for your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.TaxSettingsHome.Title">
    <Value>Tax Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.TaxSettingsHome.TitleDescription">
    <Value>Tax settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.TaxSettingsHome.Description">
    <Value>Manage the tax providers and product tax classifications for your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.ShippingSettingsHome.Title">
    <Value>Shipping Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.ShippingSettingsHome.TitleDescription">
    <Value>Shipping settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.ShippingSettingsHome.Description">
    <Value>Manage shipping providers and calculation methods for your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.LocationSettingsHome.Title">
    <Value>Location Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.LocationSettingsHome.TitleDescription">
    <Value>Location settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.LocationSettingsHome.Description">
    <Value>Manage various location settings for your store such as countries, languages and currencies.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.Settings.Title">
    <Value>All Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.Settings.TitleDescription">
    <Value>All settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ConfigurationHome.Settings.Description">
    <Value>View all the settings for your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentHome.PaymentHome">
    <Value>Payment Settings Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentHome.intro">
    <Value>Use the links on this page to manage payment settings for your store such as credit cards and payment provider settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentHome.CreditCardTypes.Title">
    <Value>Manage Credit Cards</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentHome.CreditCardTypes.TitleDescription">
    <Value>Manage credit card types.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentHome.CreditCardTypes.Description">
    <Value>Manage the credit card types that are enabled in your store. Customers can use these card types for purchases.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentHome.PaymentMethods.Title">
    <Value>Manage Payment Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentHome.PaymentMethods.TitleDescription">
    <Value>Manage payment methods.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentHome.PaymentMethods.Description">
    <Value>Manage the payment methods (providers) available in your store e.g. PayPal, WorldPay, Google Checkout. Here you manage specific payment provider settings such as your account information for the provider.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.LocationHome">
    <Value>Location Settings Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.intro">
    <Value>Use the links on this page to manage location settings for your store such as countries/regions, languages, currencies and warehouses.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Countries.Title">
    <Value>Manage Countries</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Countries.TitleDescription">
    <Value>Manage countries.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Countries.Description">
    <Value>Manage the countries that are enabled in your store. Set whether customers of a specific country can register for your store or whether you allow billing or shipping to addresses in a country.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.StateProvinces.Title">
    <Value>Manage States / Provinces</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.StateProvinces.TitleDescription">
    <Value>Manage states / provinces.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.StateProvinces.Description">
    <Value>Manage the states / provinces of countries in your store. These will be visible for customer registrations / address creation.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Languages.Title">
    <Value>Manage Languages</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Languages.TitleDescription">
    <Value>Manage languages.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Languages.Description">
    <Value>Manage the languages that are available in your store. Customers can choose a language of their choice in order to view localized content.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Currencies.Title">
    <Value>Manage Currencies</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Currencies.TitleDescription">
    <Value>Manage currencies.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Currencies.Description">
    <Value>Manage the currrencies that are available to purchasing customers of your store. Set the exchange the rates for each currency for automatic order calculation during checkout for your customers'' selected currency.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Warehouses.Title">
    <Value>Manage Warehouses</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Warehouses.TitleDescription">
    <Value>Manage warehouses.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocationHome.Warehouses.Description">
    <Value>Manage your warehouses. Products can be assigned to a specific warehouse for better stock management.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingHome">
    <Value>Shipping Settings Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.intro">
    <Value>Use the links on this page to manage shipping settings for your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingSettings.Title">
    <Value>Manage Shipping Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingSettings.TitleDescription">
    <Value>Manage shipping settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingSettings.Description">
    <Value>Use this page to manage general shipping settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingMethods.Title">
    <Value>Manage Shipping Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingMethods.TitleDescription">
    <Value>Manage shipping methods.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingMethods.Description">
    <Value>Shipping methods dictate the type of shipping e.g. "Standard Post", "Personal Courier", "By Land", "By Air". Once you have created a shipping method, it can be used by a shipping rate computation provider (see below) to calculate shipping costs based on various criteria.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingRateComputationMethods.Title">
    <Value>Manage Rate Computation Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingRateComputationMethods.TitleDescription">
    <Value>Manage shipping rate computation methods.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingHome.ShippingRateComputationMethods.Description">
    <Value>Shipping Rate Computation Methods are used to calculate the shipping costs on a customer''s order. The rates can be calculated based on the weight of the products, total cost of the order or can even use an external service such as UPS.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxHome">
    <Value>Tax Settings Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.intro">
    <Value>Use the links on this page to manage tax settings for your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxSettings.Title">
    <Value>Manage Tax Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxSettings.TitleDescription">
    <Value>Manage tax settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxSettings.Description">
    <Value>Use this page to manage general tax settings.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxProviders.Title">
    <Value>Manage Tax Providers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxProviders.TitleDescription">
    <Value>Manage tax providers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxProviders.Description">
    <Value>Tax providers are used to calculate tax on customer orders. There are a number of different providers that can be used, each with it''s own configuration options, such as tax by location.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxCategories.Title">
    <Value>Manage Tax Classes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxCategories.TitleDescription">
    <Value>Manage tax classes.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxHome.TaxCategories.Description">
    <Value>Tax classifications (categories) allow you to perform different tax calculations depending on the type of product. After creating a tax classfication it can then be used by a tax provider (see above). Products within this tax classification will then be taxed accordingly (see Catalog : Products : Manage Products)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SystemHome.SystemHome">
    <Value>System Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SystemHome.intro">
    <Value>Use the links on this page to view system logs and queues.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SystemHome.Logs.Title">
    <Value>View System Log</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SystemHome.Logs.TitleDescription">
    <Value>View system log.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SystemHome.Logs.Description">
    <Value>The sytem log captures events that may occur whilst your store is online. If you or your customers experience any problems with your store, you can check the system log to see the cause of the problem.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SystemHome.MessageQueue.Title">
    <Value>View Message Queue</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SystemHome.MessageQueue.TitleDescription">
    <Value>View message queue.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SystemHome.MessageQueue.Description">
    <Value>Manage the system message queue. View the status of queued emails all previously sent emails (notifications, campaigns etc.)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.HelpHome.HelpHome">
    <Value>Help Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.HelpHome.intro">
    <Value>Use the links on this page to get help and other useful resources about using the nopCommerce ecommerce solution.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.HelpHome.nopDocumentation.Title">
    <Value>Help Topics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.HelpHome.nopDocumentation.TitleDescription">
    <Value>Help topics.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.HelpHome.nopDocumentation.Description">
    <Value>Visit the nopCommerce website for useful documentation and tutorials on many different aspects of setting up and managing your online store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.HelpHome.nopCommunity.Title">
    <Value>Community Forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.HelpHome.nopCommunity.TitleDescription">
    <Value>Visit community forums.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.HelpHome.nopCommunity.Description">
    <Value>nopCommerce has a great community of people who are always happy to help and give advice on using your nopCommerce store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.ManageCategories">
    <Value>Manage Categories</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.SaveButton.ToolTip">
    <Value>Save category settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.AddNewButton.ToolTip">
    <Value>Add a new category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.ExportXMLButton.Text">
    <Value>Export to XML</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.ExportXMLButton.ToolTip">
    <Value>Export category list to an xml file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.ShowOnTheMainPage">
    <Value>Show on the main page:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.Categories">
    <Value>Categories</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.AddNewCategory">
    <Value>Add new category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Categories.Products">
    <Value>Products:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryAdd.AddNewCategory">
    <Value>Add a new category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryAdd.BackToCategoryList">
    <Value>back to category list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryAdd.SaveButton.ToolTip">
    <Value>Save category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryAdd.CategoryInfo">
    <Value>Category Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.EditCategoryDetails">
    <Value>Edit category details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.BackToCategoryList">
    <Value>back to category list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.SaveButton.ToolTip">
    <Value>Save changes to this category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.DeleteButton.ToolTip">
    <Value>Delete this category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.CategoryInfo">
    <Value>Category Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.SEO">
    <Value>SEO</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.Products">
    <Value>Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryDetails.Discounts">
    <Value>Discounts applied to the category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Name.ToolTip">
    <Value>The category''s name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Name.Required">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Image">
    <Value>Image:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Image.ToolTip">
    <Value>The category image.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Image.Remove">
    <Value>Remove image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Image.Remove.Tooltip">
    <Value>Remove this category image.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Image.Upload">
    <Value>Choose a new category image to upload.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Description.Tooltip">
    <Value>A description of the category.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.CategoryTemplate">
    <Value>Template:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.CategoryTemplate.Tooltip">
    <Value>Choose a category template. This template defines how products within this category will be displayed. You can manage the templates from Content Management : Templates : Category Templates.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.ParentCategory">
    <Value>Parent category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.ParentCategory.Tooltip">
    <Value>Select a parent category for this category. Leave as [ --- ] to make this the root level category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.PriceRanges">
    <Value>Price ranges:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.PriceRanges.Tooltip">
    <Value>Define the price ranges for the store price range filter. Separate ranges with a semicolon e.g. ''0-199;200-300;301-;'' (301- means 301 and over)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Published">
    <Value>Published:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.Published.Tooltip">
    <Value>Check to publish this category (visible in store). Uncheck to unpublish (category not available in store)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.DisplayOrder.Tooltip">
    <Value>Set the category''s display order. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.MetaKeywords">
    <Value>Meta keywords:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.MetaKeywords.Tooltip">
    <Value>Meta keywords to be added to category page header</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.MetaDescription">
    <Value>Meta description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.MetaDescription.Tooltip">
    <Value>Meta description to be added to category page header</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.MetaTitle">
    <Value>Meta title:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.MetaTitle.Tooltip">
    <Value>Override the page title. The default is the name of the category.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.SEName">
    <Value>Search engine friendly page name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.SEName.Tooltip">
    <Value>Set a search engine friendly page name e.g. ''The Best Computers'' to make your page name ''##-the-best-computers.aspx'' (## represents the category ID). The default is the name of category.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.PageSize">
    <Value>Page size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.PageSize.Tooltip">
    <Value>Set the page size for products in this category e.g. ''4'' products per page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.PageSize.RequiredErrorMessage">
    <Value>Enter page size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategorySEO.PageSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.Product">
    <Value>Product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.Mapped.Tooltip">
    <Value>Map this product to the category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.View">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.View.Tooltip">
    <Value>View the product details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.FeaturedProduct">
    <Value>Featured Product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.FeaturedProduct.Tooltip">
    <Value>Make this a featured product in this category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryProducts.AddNewButton.Text">
    <Value>Add product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ManageProducts">
    <Value>Manage Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.SearchButton.Text">
    <Value>Search</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.SearchButton.Tooltip">
    <Value>Search for products based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.AddButton.Tooltip">
    <Value>Add a new product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ExportXMLButton.Text">
    <Value>Export to XML</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ExportXMLButton.Tooltip">
    <Value>Export product list to a xml file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ExportXLSButton.Text">
    <Value>Export to Excel</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ExportXLSButton.Tooltip">
    <Value>Export products to an excel file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ImportXLSButton.Text">
    <Value>Import from Excel</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ImportXLSButton.Tooltip">
    <Value>Import products from Excel file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ImportXLS.ExcelFile">
    <Value>Excel file:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ProductName">
    <Value>Product name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.ProductName.Tooltip">
    <Value>A product name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Category">
    <Value>Category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Category.Tooltip">
    <Value>Search by a specific category.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Manufacturer">
    <Value>Manufacturer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Manufacturer.Tooltip">
    <Value>Search by a specific manufacturer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Tooltip">
    <Value>Click to edit product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.Edit.Tooltip">
    <Value>Click to edit product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Products.NoProductsFound">
    <Value>No products found</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.EditProductDetails">
    <Value>Edit product details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.BackToProductList">
    <Value>back to product list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.SaveButton.Tooltip">
    <Value>Save product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.DeleteButton.Tooltip">
    <Value>Delete product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.ProductInfo">
    <Value>Product Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.SEO">
    <Value>SEO</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.ProductVariants">
    <Value>Product Variants (SKUs)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.CategoryMappings">
    <Value>Category Mappings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.ManufacturerMappings">
    <Value>Manufacturer Mappings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.RelatedProducts">
    <Value>Related Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.Pictures">
    <Value>Pictures</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductDetails.ProductSpecification">
    <Value>Product specification</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAdd.AddNewProduct">
    <Value>Add a new product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAdd.SaveButton.Tooltip">
    <Value>Save product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAdd.ProductInfo">
    <Value>Product Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAdd.BackToProductList">
    <Value>back to product list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ProductName">
    <Value>Product name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ProductName.Tooltip">
    <Value>The name of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ProductName.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ShortDescription">
    <Value>Short description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ShortDescription.Tooltip">
    <Value>A short description of the product. This is the text that displays in product lists i.e. category / manufacturer pages</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.FullDescription">
    <Value>Full description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.FullDescription.Tooltip">
    <Value>A full description of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AdminComment">
    <Value>Admin comment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AdminComment.Tooltip">
    <Value>Admin comment. For internal use.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ProductType">
    <Value>Product type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ProductType.Tooltip">
    <Value>The product classification.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ProductTemplate">
    <Value>Product template:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ProductTemplate.Tooltip">
    <Value>Choose a product template. This template defines how this product (and it''s variants) will be displayed. You can manage the templates from Content Management : Templates : Product Templates.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ShowOnHomePage">
    <Value>Show on home page:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ShowOnHomePage.Tooltip">
    <Value>Check to display this product on your store''s home page. Recommended for your most popular products.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Published">
    <Value>Published:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Published.Tooltip">
    <Value>Check to publish this product (visible in store). Uncheck to unpublish (product not available in store)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AllowCustomerReviews">
    <Value>Allow customer reviews:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AllowCustomerReviews.Tooltip">
    <Value>Check to allow customers to review this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AllowCustomerReviewsView">
    <Value>View Reviews ({0})</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AllowCustomerReviewsView.Tooltip">
    <Value>View this product''s customer reviews</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AllowCustomerRatings">
    <Value>Allow customer ratings:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AllowCustomerRatings.Tooltip">
    <Value>Check to allow customers to rate this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.SKU">
    <Value>Product SKU:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.SKU.Tooltip">
    <Value>Product stock keeping unit (SKU). Your internal unique identifier that can be used to track this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ManufacturerPartNumber">
    <Value>Manufacturer part number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ManufacturerPartNumber.Tooltip">
    <Value>The manufacturer''s part number for this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.IsDownload">
    <Value>Downloadable product:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.IsDownload.Tooltip">
    <Value>Check if this product is a downloadable product. When a customer purchases a download product, they can download the item direct from your store by viewing their completed order.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.UseDownloadURL">
    <Value>Use download URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.UseDownloadURL.Tooltip">
    <Value>Use file URL to download file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.DownloadURL">
    <Value>Download URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.DownloadURL.Tooltip">
    <Value>The URL that will be used to download file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.DownloadFile">
    <Value>Download file:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.DownloadFile.Tooltip">
    <Value>The download file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.UnlimitedDownloads">
    <Value>Unlimited downloads:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.UnlimitedDownloads.Tooltip">
    <Value>When a customer purchases a download product, they can download the item unlimited number of times.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MaxNumberOfDownloads">
    <Value>Max. downloads:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MaxNumberOfDownloads.Tooltip">
    <Value>The maximum number of downloads.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MaxNumberOfDownloads.RequiredErrorMessage">
    <Value>Enter maximum number of downloads</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MaxNumberOfDownloads.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.HasSampleDownload">
    <Value>Has sample download file:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.HasSampleDownload.Tooltip">
    <Value>Check if this product has a sample download file that can be downloaded before checkout.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.UseSampleDownloadURL">
    <Value>Use download URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.UseSampleDownloadURL.Tooltip">
    <Value>Use file URL to download sample file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.SampleDownloadURL">
    <Value>Download URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.SampleDownloadURL.Tooltip">
    <Value>The URL that will be used to download sample file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.SampleDownloadFile">
    <Value>Sample download file:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.SampleDownloadFile.Tooltip">
    <Value>The sample download file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ShipEnabled">
    <Value>Shipping enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ShipEnabled.Tooltip">
    <Value>Determines whether the product can be shipped.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.FreeShipping">
    <Value>Free shipping:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.FreeShipping.Tooltip">
    <Value>Check if this product comes with FREE shipping.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AdditionalShippingCharge">
    <Value>Additional shipping charge:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AdditionalShippingCharge.Tooltip">
    <Value>The additional shipping charge.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AdditionalShippingCharge.RequiredErrorMessage">
    <Value>Additional shipping charge is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AdditionalShippingCharge.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.TaxExempt">
    <Value>Tax exempt:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.TaxExempt.Tooltip">
    <Value>Determines whether this product is tax exempt (tax will not be applied to this product at checkout).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.TaxCategory">
    <Value>Tax category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.TaxCategory.Tooltip">
    <Value>The tax classification for this product. You can manage product tax classifications from Configuration : Tax : Tax Classes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ManageStock">
    <Value>Manage Stock:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.ManageStock.Tooltip">
    <Value>Check to enable system stock management. When enabled, stock quantities are automatically adjusted when a customer makes a purchase. You can also set low stock activity actions and receive notifications.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.StockQuantity">
    <Value>Stock quantity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.StockQuantity.Tooltip">
    <Value>The current stock quantity of this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.StockQuantity.RequiredErrorMessage">
    <Value>Enter stock quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.StockQuantity.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MinStockQuantity">
    <Value>Minimum stock quantity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MinStockQuantity.Tooltip">
    <Value>If you have enabled ''Manage Stock'' you can perform a number of different actions when the current stock quantity falls below your minimum stock quantity.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MinStockQuantity.RequiredErrorMessage">
    <Value>Enter minimum stock quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.MinStockQuantity.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.LowStockActivity">
    <Value>Low stock activity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.LowStockActivity.Tooltip">
    <Value>Action to be taken when your current stock quantity falls below the ''Minimum stock quantity''.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.NotifyForQuantityBelow">
    <Value>Notify admin for quantity below:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.NotifyForQuantityBelow.Tooltip">
    <Value>When the current stock quantity falls below this quantity, the storekeeper (admin) will receive a notification.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.NotifyForQuantityBelow.RequiredErrorMessage">
    <Value>Enter notify for quantity below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.NotifyForQuantityBelow.RangeErrorMessage">
    <Value>The value must be from 1 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OrderMinimumQuantity">
    <Value>Minimum cart quantity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OrderMinimumQuantity.Tooltip">
    <Value>Set the minimum quantity allowed in a customers shopping cart e.g. set to 3 to only allow customers to purchase 3 or more of this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OrderMinimumQuantity.RequiredErrorMessage">
    <Value>Enter minimum quantity allowed in shopping cart</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OrderMinimumQuantity.RangeErrorMessage">
    <Value>The value must be from 1 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OrderMaximumQuantity">
    <Value>Maximum cart quantity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OrderMaximumQuantity.Tooltip">
    <Value>Set the maximum quantity allowed in a customers shopping cart e.g. set to 5 to only allow customers to purchase 5 of this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OrderMaximumQuantity.RequiredErrorMessage">
    <Value>Enter maximum quantity allowed in shopping cart</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OrderMaximumQuantity.RangeErrorMessage">
    <Value>The value must be from 1 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Warehouse">
    <Value>Warehouse:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Warehouse.Tooltip">
    <Value>Select the warehouse where this product is stored / to be shipped from.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.DisableBuyButton">
    <Value>Disable buy button:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.DisableBuyButton.Tooltip">
    <Value>Check to disable the buy button for this product. This may be necessary for products that are ''available upon request''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Price">
    <Value>Price:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Price.Tooltip">
    <Value>The price of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Price.RequiredErrorMessage">
    <Value>Price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Price.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OldPrice">
    <Value>Old price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OldPrice.Tooltip">
    <Value>The old price of the product. If you set an old price, this will display alongside the current price on the product page to show the difference in price.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OldPrice.RequiredErrorMessage">
    <Value>Old price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.OldPrice.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AvailableStartDateTime">
    <Value>Available start date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AvailableStartDateTime.ToolTip">
    <Value>The start of the product availability in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AvailableStartDateTime.ClickToShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AvailableEndDateTime">
    <Value>Available end date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AvailableEndDateTime.ToolTip">
    <Value>The end of the product availability in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.AvailableEndDateTime.ClickToShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Weight">
    <Value>Weight:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Weight.ToolTip">
    <Value>The weight of the product. Can be used in shipping calculations.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Weight.RequiredErrorMessage">
    <Value>Weight is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Weight.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Length">
    <Value>Length:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Length.ToolTip">
    <Value>The length of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Length.RequiredErrorMessage">
    <Value>Length is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Length.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Width">
    <Value>Width:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Width.ToolTip">
    <Value>The width of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Width.RequiredErrorMessage">
    <Value>Width is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Width.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Height">
    <Value>Height:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Height.ToolTip">
    <Value>The height of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Height.RequiredErrorMessage">
    <Value>Height is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductInfo.Height.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSEO.MetaKeywords">
    <Value>Meta keywords:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSEO.MetaKeywords.ToolTip">
    <Value>Meta keywords to be added to product page header</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSEO.MetaDescription">
    <Value>Meta description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSEO.MetaDescription.ToolTip">
    <Value>Meta description to be added to product page header</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSEO.MetaTitle">
    <Value>Meta title:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSEO.MetaTitle.ToolTip">
    <Value>Override the page title. The default is the name of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSEO.SEName">
    <Value>Search engine friendly page name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSEO.SEName.ToolTip">
    <Value>Set a search engine friendly page name e.g. ''The Best Product'' to make your page name ''##-the-best-product.aspx'' (## represents the product ID). The default is the name of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.Name">
    <Value>Product variant name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.Unnamed">
    <Value>Unnamed product variant</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.SKU">
    <Value>SKU</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.Price">
    <Value>Price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.StockQuantity">
    <Value>Stock</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.View">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Product.ProductVariants.AddButton.Tooltip">
    <Value>Add a new product variant</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductCategory.Category">
    <Value>Category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductCategory.Category.Tooltip">
    <Value>Map product to this category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductCategory.View">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductCategory.View.Tooltip">
    <Value>View category details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductCategory.FeaturedProduct">
    <Value>Featured Product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductCategory.FeaturedProduct.Tooltip">
    <Value>Make product a featured product of this category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductCategory.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductCategory.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductCategory.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductManufacturer.Manufacturer">
    <Value>Manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductManufacturer.Manufacturer.Tooltip">
    <Value>Map product to this manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductManufacturer.View">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductManufacturer.View.Tooltip">
    <Value>View manufacturer details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductManufacturer.FeaturedProduct">
    <Value>Featured Product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductManufacturer.FeaturedProduct.Tooltip">
    <Value>Make this product a featured manufacturer product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductManufacturer.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductManufacturer.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductManufacturer.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.RelatedProducts.Product">
    <Value>Product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.RelatedProducts.Product.Tooltip">
    <Value>Mark this product as a related product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.RelatedProducts.View">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.RelatedProducts.View.Tooltip">
    <Value>View product details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.RelatedProducts.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.RelatedProducts.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.RelatedProducts.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.RelatedProducts.AddNewButton.Text">
    <Value>Add new related product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.Image">
    <Value>Image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.Update">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.Update.Tooltip">
    <Value>Update this picture</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.Delete.Tooltip">
    <Value>Delete this picture</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.AddNewPicture">
    <Value>Add a new picture</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.SelectPicture">
    <Value>Select picture:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.SelectPicture.Tooltip">
    <Value>Choose a picture to upload. If the picture size exceeds your stores max image size setting, it will be automatically resized. You can manage resizing from Configuration : Global Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.FileUpload">
    <Value>Choose a file to upload</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.New.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.New.DisplayOrder.Tooltip">
    <Value>Display order of the picture. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.New.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.New.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.UploadButton.Text">
    <Value>Upload</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductPictures.UploadButton.Tooltip">
    <Value>Upload the picture</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.Attribute">
    <Value>Attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.AttributeOption">
    <Value>Attribute option</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.AllowFiltering">
    <Value>Allow filtering</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.ShowOnProductPage">
    <Value>Show on product page</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.Update">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.Update.Tooltip">
    <Value>Update specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.Delete.Tooltip">
    <Value>Delete specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.AddNew">
    <Value>Add a new product specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.AttributeType">
    <Value>Select specification attribute:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.AttributeType.Tooltip">
    <Value>Choose a product specification attribute. You can manage specification attributes from Catalog : Attributes : Product Specification.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.AttributeOption">
    <Value>Attribute option:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.AttributeOption.Tooltip">
    <Value>The value of the specification attribute.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.AllowFiltering">
    <Value>Allow filtering:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.AllowFiltering.Tooltip">
    <Value>Allow product filtering by this attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.ShowOnProductPage">
    <Value>Show on product page:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.ShowOnProductPage.Tooltip">
    <Value>The value of the specification attribute.be visible on the product page</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.DisplayOrder.Tooltip">
    <Value>Display order of the specification attribute. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.New.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.AddNewButton">
    <Value>Add attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductSpecifications.AddNewButton.Tooltip">
    <Value>Add specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAdd.AddNewProductVariant">
    <Value>Add a new product variant</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAdd.BackToProductDetails">
    <Value>(back to product details)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAdd.SaveButton.Tooltip">
    <Value>Save product variant</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAdd.ProductVariantInfo">
    <Value>Product Variant Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.EditProductVariant">
    <Value>Edit product variant for product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.BackToProductDetails">
    <Value>(back to product details)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.SaveButton.Tooltip">
    <Value>Save product variant</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.DeleteButton.Tooltip">
    <Value>Delete product variant</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.ProductVariantInfo">
    <Value>Product Variant Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.TierPrices">
    <Value>Tier Prices</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.Attributes">
    <Value>Product Variant Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantDetails.Discounts">
    <Value>Discounts</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.ProductVariantName">
    <Value>Product variant name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.ProductVariantName.Tooltip">
    <Value>The name of the product variant.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.SKU">
    <Value>Product SKU:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.SKU.Tooltip">
    <Value>Product stock keeping unit (SKU). Your internal unique identifier that can be used to track this product variant.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Image">
    <Value>Image:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Image.Tooltip">
    <Value>Product variant image.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.RemoveImage">
    <Value>Remove image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Description.Tooltip">
    <Value>A description of the product variant.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AdminComment">
    <Value>Admin comment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AdminComment.Tooltip">
    <Value>Admin comment. For internal use.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.ManufacturerPartNumber">
    <Value>Manufacturer part number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.ManufacturerPartNumber.Tooltip">
    <Value>The manufacturer''s part number for this product variant.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.IsDownload">
    <Value>Downloadable product:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.IsDownload.Tooltip">
    <Value>Check if this product variant is a downloadable product. When a customer purchases a download product, they can download the item direct from your store by viewing their completed order.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.UseDownloadURL">
    <Value>Use download URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.UseDownloadURL.Tooltip">
    <Value>Use file URL to download file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DownloadURL">
    <Value>Download URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DownloadURL.Tooltip">
    <Value>The URL that will be used to download file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DownloadFile">
    <Value>Download file:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DownloadFile.Tooltip">
    <Value>The download file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DownloadFile.Download">
    <Value>Download</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DownloadFile.Remove">
    <Value>Remove download</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.UnlimitedDownloads">
    <Value>Unlimited downloads:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.UnlimitedDownloads.Tooltip">
    <Value>When a customer purchases a download product, they can download the item unlimited number of times.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MaxNumberOfDownloads">
    <Value>Max. downloads:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MaxNumberOfDownloads.Tooltip">
    <Value>The maximum number of downloads.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MaxNumberOfDownloads.RequiredErrorMessage">
    <Value>The maximum number of downloads.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MaxNumberOfDownloads.RangeErrorMessage">
    <Value>The maximum number of downloads.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.HasSampleDownload">
    <Value>Has sample download file:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.HasSampleDownload.Tooltip">
    <Value>Check if this product has a sample download file that can be downloaded before checkout.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.UseSampleDownloadURL">
    <Value>Use download URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.UseSampleDownloadURL.Tooltip">
    <Value>Use file URL to download sample file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.SampleDownloadURL">
    <Value>Download URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.SampleDownloadURL.Tooltip">
    <Value>The URL that will be used to download sample file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.SampleDownloadFile">
    <Value>Sample download file:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.SampleDownloadFile.Tooltip">
    <Value>The sample download file.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.SampleDownloadFile.Download">
    <Value>Download</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.SampleDownloadFile.Remove">
    <Value>Remove sample download</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.ShipEnabled">
    <Value>Shipping enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.ShipEnabled.Tooltip">
    <Value>Determines whether the product can be shipped.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.FreeShipping">
    <Value>Free shipping:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.FreeShipping.Tooltip">
    <Value>Check if this product comes with FREE shipping.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AdditionalShippingCharge">
    <Value>Additional shipping charge:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AdditionalShippingCharge.Tooltip">
    <Value>The additional shipping charge.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AdditionalShippingCharge.RequiredErrorMessage">
    <Value>Additional shipping charge is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AdditionalShippingCharge.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.TaxExempt">
    <Value>Tax exempt:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.TaxExempt.Tooltip">
    <Value>Determines whether this product is tax exempt (tax will not be applied to this product at checkout).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.TaxCategory">
    <Value>Tax category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.TaxCategory.Tooltip">
    <Value>The tax classification for this product. You can manage product tax classifications from Configuration : Tax : Tax Classes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.ManageStock">
    <Value>Manage Stock:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.ManageStock.Tooltip">
    <Value>Check to enable system stock management. When enabled, stock quantities are automatically adjusted when a customer makes a purchase. You can also set low stock activity actions and receive notifications.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.StockQuantity">
    <Value>Stock quantity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.StockQuantity.Tooltip">
    <Value>The current stock quantity of this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.StockQuantity.RequiredErrorMessage">
    <Value>Enter stock quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.StockQuantity.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MinStockQuantity">
    <Value>Minimum stock quantity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MinStockQuantity.Tooltip">
    <Value>If you have enabled ''Manage Stock'' you can perform a number of different actions when the current stock quantity falls below your minimum stock quantity.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MinStockQuantity.RequiredErrorMessage">
    <Value>Enter minimum stock quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.MinStockQuantity.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.LowStockActivity">
    <Value>Low stock activity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.LowStockActivity.Tooltip">
    <Value>Action to be taken when your current stock quantity falls below the ''Minimum stock quantity''.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.NotifyForQuantityBelow">
    <Value>Notify admin for quantity below:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.NotifyForQuantityBelow.Tooltip">
    <Value>When the current stock quantity falls below this quantity, the storekeeper (admin) will receive a notification.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.NotifyForQuantityBelow.RequiredErrorMessage">
    <Value>Enter notify for quantity below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.NotifyForQuantityBelow.RangeErrorMessage">
    <Value>The value must be from 1 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OrderMinimumQuantity">
    <Value>Minimum cart quantity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OrderMinimumQuantity.Tooltip">
    <Value>Set the minimum quantity allowed in a customers shopping cart e.g. set to 3 to only allow customers to purchase 3 or more of this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OrderMinimumQuantity.RequiredErrorMessage">
    <Value>Enter minimum quantity allowed in shopping cart</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OrderMinimumQuantity.RangeErrorMessage">
    <Value>The value must be from 1 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OrderMaximumQuantity">
    <Value>Maximum cart quantity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OrderMaximumQuantity.Tooltip">
    <Value>Set the maximum quantity allowed in a customers shopping cart e.g. set to 5 to only allow customers to purchase 5 of this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OrderMaximumQuantity.RequiredErrorMessage">
    <Value>Enter maximum quantity allowed in shopping cart</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OrderMaximumQuantity.RangeErrorMessage">
    <Value>The value must be from 1 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Warehouse">
    <Value>Warehouse:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Warehouse.Tooltip">
    <Value>Select the warehouse where this product is stored / to be shipped from.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DisableBuyButton">
    <Value>Disable buy button:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DisableBuyButton.Tooltip">
    <Value>Check to disable the buy button for this product. This may be necessary for products that are ''available upon request''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Price">
    <Value>Price:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Price.Tooltip">
    <Value>The price of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Price.RequiredErrorMessage">
    <Value>Price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Price.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OldPrice">
    <Value>Old price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OldPrice.Tooltip">
    <Value>The old price of the product. If you set an old price, this will display alongside the current price on the product page to show the difference in price.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OldPrice.RequiredErrorMessage">
    <Value>Old price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.OldPrice.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AvailableStartDateTime">
    <Value>Available start date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AvailableStartDateTime.ToolTip">
    <Value>The start of the product availability in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AvailableStartDateTime.ClickToShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AvailableEndDateTime">
    <Value>Available end date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AvailableEndDateTime.ToolTip">
    <Value>The end of the product availability in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.AvailableEndDateTime.ClickToShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Weight">
    <Value>Weight:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Weight.ToolTip">
    <Value>The weight of the product. Can be used in shipping calculations.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Weight.RequiredErrorMessage">
    <Value>Weight is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Weight.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Length">
    <Value>Length:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Length.ToolTip">
    <Value>The length of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Length.RequiredErrorMessage">
    <Value>Length is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Length.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Width">
    <Value>Width:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Width.ToolTip">
    <Value>The width of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Width.RequiredErrorMessage">
    <Value>Width is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Width.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Height">
    <Value>Height:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Height.ToolTip">
    <Value>The height of the product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Height.RequiredErrorMessage">
    <Value>Height is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Height.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Published">
    <Value>Published:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.Published.ToolTip">
    <Value>Check to publish this product variant (visible in store). Uncheck to unpublish (product not available in store)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DisplayOrder.ToolTip">
    <Value>Set the display order of this product variant. 1 represents the first item in the collection of variants for this product.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Title">
    <Value>Tier prices are applied only to product variant price (and not applied to product attributes).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Quantity">
    <Value>Quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Quantity.RequiredErrorMessage">
    <Value>Quantity is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Quantity.RangeErrorMessage">
    <Value>The value must be from 0 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Quantity.AndAbove">
    <Value>and above.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Price">
    <Value>Price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Price.RequiredErrorMessage">
    <Value>Price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Price.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Update">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Update.Tooltip">
    <Value>Update tier price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.Delete.Tooltip">
    <Value>Delete tier price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.AddNew">
    <Value>Add tier price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.Quantity">
    <Value>Quantity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.Quantity.Tooltip">
    <Value>Enter quantity of product variant</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.Quantity.RequiredErrorMessage">
    <Value>Quantity is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.Quantity.RangeErrorMessage">
    <Value>The value must be from 0 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.Quantity.AndAbove">
    <Value>and above.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.Price">
    <Value>Price:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.Price.Tooltip">
    <Value>Price per item.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.Price.RequiredErrorMessage">
    <Value>Price is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.Price.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.AddNewButton.Text">
    <Value>Add tier price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantTierPrices.New.AddNewButton.Tooltip">
    <Value>Add tier price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.Attribute">
    <Value>Attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.TextPrompt">
    <Value>Text prompt</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.IsRequired">
    <Value>Is Required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.IsRequired.Tooltip">
    <Value>Is this attribute required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.ControlType">
    <Value>Control type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.Values">
    <Value>Values</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.Values.Tooltip">
    <Value>View / Edit attribute values</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.Values.Count">
    <Value>View/Edit value (Total: {0})</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.Update">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.Update.Tooltip">
    <Value>Update product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.Delete.Tooltip">
    <Value>Delete product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.AddNew">
    <Value>Add a new attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.Attribute">
    <Value>Select attribute:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.Attribute.Tooltip">
    <Value>Select a product attribute to add to this product variant. To manage product attribute types, go to Catalog : Attributes : Product Attributes.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.TextPrompt">
    <Value>Text prompt:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.TextPrompt.Tooltip">
    <Value>Enter text prompt.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.Required">
    <Value>Required:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.Required.Tooltip">
    <Value>When an attribute is required, the customer must choose an appropriate attribute value before they can add the product to their shopping cart.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.ControlType">
    <Value>Control Type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.ControlType.Tooltip">
    <Value>Choose how to display your attribute values.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.DisplayOrder.Tooltip">
    <Value>The product attribute display order. 1 represents the first item in the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.AddNewButton.Text">
    <Value>Add attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributes.New.AddNewButton.Tooltip">
    <Value>Add product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.AddEdit">
    <Value>Add/Edit values for [{0}] attribute. Product: {1}</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.BackToProductDetails">
    <Value>(back to product)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.PriceAdjustment">
    <Value>Price Adjustment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.PriceAdjustment.RequiredErrorMessage">
    <Value>Price adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.PriceAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.WeightAdjustment">
    <Value>Weight Adjustment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.WeightAdjustment.RequiredErrorMessage">
    <Value>Weight adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.WeightAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.IsPreSelected">
    <Value>Is pre-selected</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.Update">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.AddNew">
    <Value>Add new values</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.Name.Tooltip">
    <Value>The attribute value name e.g. ''Blue'' for Color attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.PriceAdjustment">
    <Value>Price adjustment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.PriceAdjustment.Tooltip">
    <Value>The price adjustment applied when choosing this attribute value e.g. ''10'' to add 10 euros.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.PriceAdjustment.RequiredErrorMessage">
    <Value>Price adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.PriceAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.WeightAdjustment">
    <Value>Weight adjustment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.WeightAdjustment.Tooltip">
    <Value>The weight adjustment applied when choosing this attribute value</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.WeightAdjustment.RequiredErrorMessage">
    <Value>Weight adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.WeightAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.PreSelected">
    <Value>Pre-selected:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.PreSelected.Tooltip">
    <Value>Determines whether this attribute value is pre selected for the customer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.DisplayOrder.Tooltip">
    <Value>The display order of the attribute value. 1 represents the first item in attribute value list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.AddNewButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantAttributeValues.New.AddNewButton.Tooltip">
    <Value>Save attribute value</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.ProductReviews">
    <Value>Product Reviews</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.ViewProductDetails">
    <Value>Click to view product details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.Disapprove">
    <Value>Disapprove</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.Approve">
    <Value>Approve</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.Disapprove.Tooltip">
    <Value>Disapprove the product review</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.Approve.Tooltip">
    <Value>Approve the product review</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.Edit.Tooltip">
    <Value>Edit the product review</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviews.Delete.Tooltip">
    <Value>Delete the product review</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.EditProductReview">
    <Value>Edit Product Review</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.BackToProductReviews">
    <Value>back to product reviews list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.SaveButton.Tooltip">
    <Value>Save review changes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.DeleteButton.Tooltip">
    <Value>Delete this product review</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Customer">
    <Value>Customer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Customer.Tooltip">
    <Value>The ID of the customer who created the review.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Product">
    <Value>Product:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Product.Tooltip">
    <Value>The name of the product that the review is for.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Title">
    <Value>Title:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Title.Tooltip">
    <Value>The title of the product review.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Title.ErrorMessage">
    <Value>Title is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.ReviewText">
    <Value>Review text:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.ReviewText.Tooltip">
    <Value>The review text.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Rating">
    <Value>Rating:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Rating.Tooltip">
    <Value>The customer''s product rating.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Approved">
    <Value>Is approved:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Approved.Tooltip">
    <Value>Is the review approved? Marking it as approved means that it is visible to all your site''s visitors.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Created">
    <Value>Created On:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductReviewDetails.Created.Tooltip">
    <Value>The date/time that the review was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.Title">
    <Value>Product Variant Low Stock Report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.Name.Tooltip">
    <Value>Click to view product variant details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.Price">
    <Value>Price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.StockQuantity">
    <Value>Stock quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.MinStockQuantity">
    <Value>Min stock quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.Edit.Tooltip">
    <Value>Click to view product variant details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductVariantsLowStock.ProductStockIsOkay">
    <Value>Product stock is okay.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributes.ProductAttributes">
    <Value>Product Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributes.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributes.AddButton.Tooltip">
    <Value>Add a new product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributes.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributes.Name.Tooltip">
    <Value>Edit product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributes.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributes.Edit.Tooltip">
    <Value>Edit product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeAdd.Title">
    <Value>Add a new product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeAdd.BackToAttributeList">
    <Value>back to product attribute list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeAdd.SaveButton.Tooltip">
    <Value>Save product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeDetails.Title">
    <Value>Product Attribute Details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeDetails.BackToAttributeList">
    <Value>back to product attribute list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeDetails.SaveButton.Tooltip">
    <Value>Save product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeDetails.DeleteButton.Tooltip">
    <Value>Delete product attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeInfo.Name.Text">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeInfo.Name.Tooltip">
    <Value>The name of the product attribute.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeInfo.Name.ErrorMessage">
    <Value>The name of the product attribute is required.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeInfo.Description.Text">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductAttributeInfo.Description.Tooltip">
    <Value>The product attribute description.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributes.Title">
    <Value>Specification Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributes.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributes.AddButton.Tooltip">
    <Value>Add a new product specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributes.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributes.Name.Tooltip">
    <Value>Edit product specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributes.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributes.Edit.Tooltip">
    <Value>Edit product specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeAdd.Title">
    <Value>Add a new specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeAdd.BackToAttributeList">
    <Value>back to specification attribute list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeAdd.SaveButton.Tooltip">
    <Value>Save product specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeDetails.Title">
    <Value>Edit specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeDetails.BackToAttributeList">
    <Value>back to specification attribute list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeDetails.SaveButton.Tooltip">
    <Value>Save specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeDetails.DeleteButton.Tooltip">
    <Value>Delete specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.Name.Tooltip">
    <Value>The name of the product specification attribute.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.DisplayOrder.Tooltip">
    <Value>The display order of the product specification attribute.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOptions">
    <Value>Specification attribute options</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption">
    <Value>Attribute option</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.ErrorMessage">
    <Value>Option name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.Update">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.Update.Tooltip">
    <Value>Update specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.Delete.Tooltip">
    <Value>Delete specification attribute</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.NoOptions">
    <Value>No specification attribute options defined</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.NewButton.Text">
    <Value>Add new specification attribute option</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeInfo.AttributeOption.NewButton.Tooltip">
    <Value>Add new specification attribute option</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.Title">
    <Value>Add a new specification attribute option</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.BackToAttributeDetails">
    <Value>back to specification attribute details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.SaveButton.Tooltip">
    <Value>Save product specification attribute option</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.Name">
    <Value>Option name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.Name.Tooltip">
    <Value>The name of the specification attribute.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.DisplayOrder.Tooltip">
    <Value>The display order of the specification attribute. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SpecificationAttributeOptionAdd.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.Title">
    <Value>Manage Manufacturers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.AddButton.Tooltip">
    <Value>Add a new manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.ExportXMLButton.Text">
    <Value>Export to XML</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.ExportXMLButton.Tooltip">
    <Value>Export manufacturer list to a xml file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.Name.Tooltip">
    <Value>Edit manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.Edit.Tooltip">
    <Value>Edit manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Manufacturers.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerAdd.Title">
    <Value>Add a new manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerAdd.BackToManufacturers">
    <Value>back to manufacturer list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerAdd.SaveButton.Tooltip">
    <Value>Save manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerAdd.ManufacturerInfo">
    <Value>Manufacturer Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerDetails.Title">
    <Value>Edit manufacturer details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerDetails.BackToManufacturers">
    <Value>back to manufacturer list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerDetails.SaveButton.Tooltip">
    <Value>Save manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerDetails.DeleteButton.Tooltip">
    <Value>Delete manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerDetails.ManufacturerInfo">
    <Value>Manufacturer Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerDetails.SEO">
    <Value>SEO</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerDetails.Products">
    <Value>Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Name.Tooltip">
    <Value>The manufacturer''s name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Image">
    <Value>Image:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Image.Tooltip">
    <Value>The image to be used for this manufacturer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Image.Remove">
    <Value>Remove image</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Description.Tooltip">
    <Value>A description of the manufacturer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Template">
    <Value>Template:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Template.Tooltip">
    <Value>Choose a manufacturer template. This template defines how products for this manufacturer will be displayed. You can manage the templates from Content Management : Templates : Manufacturer Templates.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.PriceRanges">
    <Value>Price ranges:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.PriceRanges.Tooltip">
    <Value>Define the price ranges for the store price range filter. Separate ranges with a semicolon e.g. ''0-199;200-300;301-;'' (301- means 301 and over)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Published">
    <Value>Published:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.Published.Tooltip">
    <Value>Check to publish this manufacturer (visible in store). Uncheck to unpublish (manufacturer not available in store)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.DisplayOrder.Tooltip">
    <Value>Set the manufacturer''s display order. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.MetaKeywords">
    <Value>Meta keywords:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.MetaKeywords.Tooltip">
    <Value>Meta keywords to be added to manufacturer page header</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.MetaDescription">
    <Value>Meta description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.MetaDescription.Tooltip">
    <Value>Meta description to be added to manufacturer page header</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.MetaTitle">
    <Value>Meta title:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.MetaTitle.Tooltip">
    <Value>Override the page title. The default is the name of the manufacturer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.SEName">
    <Value>Search engine friendly page name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.SEName.Tooltip">
    <Value>Set a search engine friendly page name e.g. ''The Manufacturer Name'' to make your page name ''##-the-manufacturer-name.aspx'' (## represents the manufacturer ID). The default is the name of manufacturer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.PageSize">
    <Value>Page size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.PageSize.Tooltip">
    <Value>Set the page size for products for this manufacturer e.g. ''4'' products per page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.PageSize.RequiredErrorMessage">
    <Value>Enter page size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerSEO.PageSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.Product">
    <Value>Product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.Product.Tooltip">
    <Value>Add this product to the manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.View">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.View.Tooltip">
    <Value>View product details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.FeaturedProduct">
    <Value>Featured Product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.FeaturedProduct.Tooltip">
    <Value>Make this a featured product for the manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerProducts.AddNewButton.Text">
    <Value>Add product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.Title">
    <Value>Orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.SearchButton">
    <Value>Search</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.SearchButton.Tooltip">
    <Value>Search for orders based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.ExportXMLButton">
    <Value>Export to XML</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.ExportXMLButton.Tooltip">
    <Value>Export orders list to a xml file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.ExportXLSButton">
    <Value>Export to Excel</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.ExportXLSButton.Tooltip">
    <Value>Export orders to an excel file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.StartDate">
    <Value>Start date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.StartDate.Tooltip">
    <Value>The start date for the search in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.StartDate.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.EndDate">
    <Value>End date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.EndDate.Tooltip">
    <Value>The end date for the search in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.EndDate.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.CustomerEmail">
    <Value>Customer email address:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.CustomerEmail.Tooltip">
    <Value>A customer email address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.OrderStatus">
    <Value>Order status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.OrderStatus.Tooltip">
    <Value>Search by a specific order status e.g. Complete.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.PaymentStatus">
    <Value>Payment status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.PaymentStatus.Tooltip">
    <Value>Search by a specific payment status e.g. Paid.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.ShippingStatus">
    <Value>Shipping status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.ShippingStatus.Tooltip">
    <Value>Search by a specific shipping status e.g. Not yet shipped.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.GoDirectly">
    <Value>Go directly to order #:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.GoDirectly.Tooltip">
    <Value>Go directly to order #</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.GoDirectly.ErrorMessage">
    <Value>Order number is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.GoButton.Text">
    <Value>Go</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.GoButton.Tooltip">
    <Value>Go directly to order #</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.OrderIDColumn">
    <Value>Order ID</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.OrderTotalColumn">
    <Value>Order total</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.OrderStatusColumn">
    <Value>Order Status</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.PaymentStatusColumn">
    <Value>Payment Status</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.ShippingStatusColumn">
    <Value>Shipping Status</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.CustomerColumn">
    <Value>Customer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.CustomerColumn.Guest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.ViewColumn">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.ViewColumn.Tooltip">
    <Value>View order details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.CreatedOnColumn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Orders.NoOrdersFound">
    <Value>No orders found</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.Title">
    <Value>Sales Report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.SearchButton.Text">
    <Value>Run report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.SearchButton.Tooltip">
    <Value>Search for sales based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.StartDate">
    <Value>Start date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.StartDate.Tooltip">
    <Value>The start date for the report in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.StartDate.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.EndDate">
    <Value>End date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.EndDate.Tooltip">
    <Value>The end date for the report in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.EndDate.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.OrderStatus">
    <Value>Order status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.OrderStatus.Tooltip">
    <Value>Search for orders with this order status.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.PaymentStatus">
    <Value>Payment status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.PaymentStatus.Tooltip">
    <Value>Search for orders with this payment status.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.Name.Tooltip">
    <Value>View product variant details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.TotalCount">
    <Value>Total count</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SalesReport.TotalPrice">
    <Value>Total price (excl tax)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Title">
    <Value>Order Details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BackToOrders">
    <Value>back to orders list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Guest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.InvoicePDF.Text">
    <Value>Invoice (PDF)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.InvoicePDF.Tooltip">
    <Value>Get invoice in PDF format</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.DeleteButton.Tooltip">
    <Value>Delete order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderInfo">
    <Value>Order Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderStatus">
    <Value>Order Status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderStatus.Tooltip">
    <Value>The status of this order.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CancelButton.Text">
    <Value>Cancel order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderID">
    <Value>Order ID:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderID.Tooltip">
    <Value>The unique ID of this order.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderGUID">
    <Value>Order GUID:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderGUID.Tooltip">
    <Value>Internal reference for order.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Customer">
    <Value>Customer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Customer.Tooltip">
    <Value>The name of the customer who placed this order.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Affiliate">
    <Value>Affiliate:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Affiliate.Tooltip">
    <Value>The name of the affiliate who reffered the customer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Discount">
    <Value>Order discount:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Discount.Tooltip">
    <Value>The total discount applied to this order. Manage your discounts from Promotions : Discounts</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.SubtotalInclTax">
    <Value>Order subtotal (incl tax):</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.SubtotalInclTax.Tooltip">
    <Value>The subtotal of this order (including tax).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.SubtotalExclTax">
    <Value>Order subtotal (excl tax):</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.SubtotalExclTax.Tooltip">
    <Value>The subtotal of this order (excluding tax).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingInclTax">
    <Value>Order shipping (incl tax):</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingInclTax.Tooltip">
    <Value>The total shipping cost for this order (including tax).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingExclTax">
    <Value>Order shipping (excl tax):</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingExclTax.Tooltip">
    <Value>The total shipping cost for this order (excluding tax).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PaymentMethodAdditionalFeeInclTax">
    <Value>Payment method additional fee (incl tax):</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PaymentMethodAdditionalFeeInclTax.Tooltip">
    <Value>The payment method additional fee for this order (including tax).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PaymentMethodAdditionalFeeExclTax">
    <Value>Payment method additional fee (excl tax):</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PaymentMethodAdditionalFeeExclTax.Tooltip">
    <Value>The payment method additional fee for this order (excluding tax).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderTax">
    <Value>Order tax:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderTax.Tooltip">
    <Value>Total tax applied to this order. Manage your tax settings from Configuration : Tax</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderTotal">
    <Value>Order total:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderTotal.Tooltip">
    <Value>The total cost of this order (includes discounts, shipping and tax).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CartType">
    <Value>Card type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CartType.Tooltip">
    <Value>The type of card used in the transaction.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardName">
    <Value>Card name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardName.Tooltip">
    <Value>The name on the card used in the transaction.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardNumber">
    <Value>Card number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardNumber.Tooltip">
    <Value>The number of the card used in the transaction.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardCVV2">
    <Value>Card CVV2:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardCVV2.Tooltip">
    <Value>The card security code of the card used in the transaction.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardExpiryMonth">
    <Value>Card expiry month:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardExpiryMonth.Tooltip">
    <Value>The expiry month of the card used in the transaction.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardExpiryYear">
    <Value>Card expiry year:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CardExpiryYear.Tooltip">
    <Value>The expiry year of the card used in the transaction.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PONumber">
    <Value>Purchase order number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PONumber.Tooltip">
    <Value>The customer''s purchase order number when using the Purchase Order payment method. You can manage payment methods from Configuration : Payment : Payment Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PaymentMethod">
    <Value>Payment method:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PaymentMethod.Tooltip">
    <Value>The payment method used for this transaction. You can manage Payment Methods from Configuration : Payment : Payment Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PaymentStatus">
    <Value>Payment status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.PaymentStatus.Tooltip">
    <Value>The status of the payment.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CaptureButton.Text">
    <Value>Capture</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CaptureButton.Tooltip">
    <Value>Capture this payment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.MarkAsPaidButton.Text">
    <Value>Mark as paid</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.MarkAsPaidButton.Tooltip">
    <Value>Mark the payment status of this order as ''Paid''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.CreatedOn.Tooltip">
    <Value>The date/time the order was placed/created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingInfo">
    <Value>Billing Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress">
    <Value>Billing address:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.Tooltip">
    <Value>The customer''s billing address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.Email">
    <Value>Email:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.Phone">
    <Value>Phone:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.BillingAddress.Fax">
    <Value>Fax:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingInfo">
    <Value>Shipping Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingNotRequired">
    <Value>Shipping not required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress">
    <Value>Shipping address:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.Tooltip">
    <Value>The customer''s shipping address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.Email">
    <Value>Email:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.Phone">
    <Value>Phone:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingAddress.Fax">
    <Value>Fax:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderWeight">
    <Value>Order weight:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderWeight.Tooltip">
    <Value>The total weight of this order. You can set the weights for individual product variants in Catalog : Products : Manage Products. To manage weight units go to Configuration : Global Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingMethod">
    <Value>Shipping method:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippingMethod.Tooltip">
    <Value>The customers chosen shipping method for this order. You can manage shipping methods from Configuration : Shipping : Shipping Methods.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippedDate">
    <Value>Shipped date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.ShippedDate.Tooltip">
    <Value>The date this order was shipped.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.SetAsShippedButton.Text">
    <Value>Set as shipped</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Products">
    <Value>Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Products.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Products.Name.Tooltip">
    <Value>View product details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Products.Download">
    <Value>Download</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Products.Price">
    <Value>Price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Products.Quantity">
    <Value>Quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Products.Discount">
    <Value>Discount</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.Products.Discount">
    <Value>Total</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderNotes">
    <Value>Order notes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderNotes.CreatedOn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderNotes.Note">
    <Value>Note</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderNotes.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderNotes.New.Note">
    <Value>New order note:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderNotes.New.Note.Tooltip">
    <Value>The new order note.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderNotes.New.AddNewButton.Text">
    <Value>Add order note</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.OrderDetails.OrderNotes.New.AddNewButton.Tooltip">
    <Value>Add new order note</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.Title">
    <Value>Manage Customers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.SearchButton.Text">
    <Value>Search</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.SearchButton.Tooltip">
    <Value>Search for customers based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.ExportXMLButton.Text">
    <Value>Export to XML</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.ExportXMLButton.Tooltip">
    <Value>Export customers list to a xml file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.ExportXLS.Text">
    <Value>Export to Excel</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.ExportXLS.Tooltip">
    <Value>Export customers list to an excel file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.ImportXLS.Text">
    <Value>Import from Excel</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.ImportXLS.Tooltip">
    <Value>Import customer list from Excel file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.ImportXLS.ExcelFile">
    <Value>Excel file:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.AddNewButton.Tooltip">
    <Value>Add a new customer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.RegistrationFrom">
    <Value>Registration from:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.RegistrationFrom.Tooltip">
    <Value>The registration from date for the search in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.RegistrationFrom.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.RegistrationTo">
    <Value>Registration to:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.RegistrationTo.Tooltip">
    <Value>The registration to date for the search in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.RegistrationTo.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.Email">
    <Value>Email:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.Email.Tooltip">
    <Value>A customer Email.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.Username">
    <Value>Username:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.Username.Tooltip">
    <Value>A customer username (if usernames are enabled).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.DontLoadGuest">
    <Value>Don''t load guests:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.DontLoadGuest.Tooltip">
    <Value>Check to don''t load guest customers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.Guest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.EmailColumn">
    <Value>Email</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.NameColumn">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.UsernameColumn">
    <Value>Username</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.ActiveColumn">
    <Value>Active</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.RegistrationColumn">
    <Value>Registration date</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.EditColumn">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Customers.EditColumn.Tooltip">
    <Value>Edit customer details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerAdd.Title">
    <Value>Add a new customer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerAdd.BackToCustomers">
    <Value>back to customer list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerAdd.AddButton">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerAdd.AddButton.Tooltip">
    <Value>Add customer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.Title">
    <Value>Edit customer details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.BackToCustomers">
    <Value>back to customer list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.SaveButton">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.SaveButton.Tooltip">
    <Value>Save customer changes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.DeleteButton">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.DeleteButton.Tooltip">
    <Value>Delete customer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.CustomerInfo">
    <Value>Customer Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.BillingAddresses">
    <Value>Customer Billing Addresses</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.ShippingAddresses">
    <Value>Customer Shipping Addresses</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.Orders">
    <Value>Customer Orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.Roles">
    <Value>Customer Roles</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerDetails.CurrentCart">
    <Value>Current Shopping Cart</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Email.Required">
    <Value>Email Address is required field.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Email.WrongEmail">
    <Value>Wrong email format</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Email">
    <Value>Email address:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Email.Tooltip">
    <Value>The customer''s email address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Username">
    <Value>Username:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Username.Tooltip">
    <Value>The customer''s email address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Password">
    <Value>Password:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Password.Tooltip">
    <Value>The password.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Gender">
    <Value>Gender:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Gender.Tooltip">
    <Value>The customer''s gender.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Gender.Male">
    <Value>Male</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Gender.Female">
    <Value>Female</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.FirstName">
    <Value>First name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.FirstName.Tooltip">
    <Value>The customer''s first name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.LastName">
    <Value>Last name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.LastName.Tooltip">
    <Value>The customer''s last name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.DateOfBirth">
    <Value>Date of birth:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.DateOfBirth.Tooltip">
    <Value>The customer''s date of birth.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.DateOfBirth.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Company">
    <Value>Company:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Company.Tooltip">
    <Value>The customer''s company.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Address">
    <Value>Street address:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Address.Tooltip">
    <Value>The customer''s street address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Address2">
    <Value>Street address 2:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Address2.Tooltip">
    <Value>The customer''s street address 2.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Zip">
    <Value>Zip / postal code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Zip.Tooltip">
    <Value>The customer''s zip.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.City">
    <Value>City:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.City.Tooltip">
    <Value>The customer''s city.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Country">
    <Value>Country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Country.Tooltip">
    <Value>The customer''s country.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.State">
    <Value>State / province:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.State.Tooltip">
    <Value>The customer''s state / province.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Phone">
    <Value>Phone:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Phone.Tooltip">
    <Value>The customer''s phone number.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Fax">
    <Value>Fax:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Fax.Tooltip">
    <Value>The customer''s fax number.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Newsletter">
    <Value>Newsletter:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Newsletter.Tooltip">
    <Value>Newsletter.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.TimeZone">
    <Value>TimeZone:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.TimeZone.Tooltip">
    <Value>The time zone.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Affiliate">
    <Value>Affiliate:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Affiliate.Tooltip">
    <Value>The affiliate who the customer is linked to.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Affiliate.None">
    <Value>None (ID=0)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.TaxExempt">
    <Value>Is tax exempt:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.TaxExempt.Tooltip">
    <Value>Determines whether the customer is tax exempt.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Admin">
    <Value>Is administrator:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Admin.Tooltip">
    <Value>Determines whether the account is an adminstrator account (cookies should be cleared after this setting is applied). WARNING - THIS ALLOWS ACCESS TO THE ADMINISTRATION AREA OF YOUR STORE.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.ForumModerator">
    <Value>Is forum moderator:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.ForumModerator.Tooltip">
    <Value>Determines whether the account is a forum moderator.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.AdminComment">
    <Value>Admin comment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.AdminComment.Tooltip">
    <Value>Admin comment. For internal use.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Active">
    <Value>Active:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.Active.Tooltip">
    <Value>Is this customer''s account active. To disable/suspend a customer''s account, uncheck this.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.RegistrationDate">
    <Value>Registration Date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerInfo.RegistrationDate.Tooltip">
    <Value>The date the customer registered.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerBillingAddresses.AddButton.Text">
    <Value>Add New Address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerBillingAddresses.AddButton.Tooltip">
    <Value>Add a new billing address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerBillingAddresses.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerShippingAddresses.AddButton.Text">
    <Value>Add New Address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerShippingAddresses.AddButton.Tooltip">
    <Value>Add a new shipping address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerShippingAddresses.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerOrders.OrderID">
    <Value>Order number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerOrders.Date">
    <Value>Date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerOrders.OrderStatus">
    <Value>Order Status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerOrders.PaymentStatus">
    <Value>Payment Status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerOrders.ShippingStatus">
    <Value>Shipping Status:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerOrders.OrderTotal">
    <Value>Order total:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerOrders.Details">
    <Value>Details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerShoppingCart.Empty">
    <Value>Shopping cart is empty</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerShoppingCart.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerShoppingCart.Name.Tooltip">
    <Value>View details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerShoppingCart.Price">
    <Value>Price</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerShoppingCart.Quantity">
    <Value>Quantity</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerShoppingCart.Total">
    <Value>Total</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.Title">
    <Value>Customer Roles</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.AddNewButton.Tooltip">
    <Value>Add a new customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.Name.Tooltip">
    <Value>Edit customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.FreeShipping">
    <Value>Free shipping</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.TaxExempt">
    <Value>Tax exempt</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.Active">
    <Value>Active</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoles.Edit.Tooltip">
    <Value>Edit customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleAdd.Title">
    <Value>Add a new customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleAdd.BackToCustomerRoles">
    <Value>back to customer roles list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleAdd.AddButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleAdd.AddButton.Tooltip">
    <Value>Save customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleDetails.Title">
    <Value>Edit customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleDetails.BackToCustomerRoles">
    <Value>back to customer roles list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleDetails.SaveButton">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleDetails.SaveButton.Tooltip">
    <Value>Save customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleDetails.DeleteButton">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleDetails.DeleteButton.Tooltip">
    <Value>Delete customer role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleDetails.RoleInfo">
    <Value>Role Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleDetails.Customers">
    <Value>Customers of the role</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleInfo.RoleName">
    <Value>Role name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleInfo.RoleName.Tooltip">
    <Value>The name of the customer role.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleInfo.RoleName.ErrorMessage">
    <Value>Role name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleInfo.FreeShipping">
    <Value>Free shipping:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleInfo.FreeShipping.Tooltip">
    <Value>Check to allow customers in this role to get free shipping on their orders.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleInfo.TaxExempt">
    <Value>Tax exempt:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleInfo.TaxExempt.Tooltip">
    <Value>Check to allow customers in this role to make tax free purchases.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleInfo.Active">
    <Value>Active:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CustomerRoleInfo.Active.Tooltip">
    <Value>Check to make this role active.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Affiliates.Title">
    <Value>Manage Affiliates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Affiliates.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Affiliates.AddNewButton.Tooltip">
    <Value>Add a new affiliate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Affiliates.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Affiliates.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Affiliates.Edit.Tooltip">
    <Value>Edit affiliate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Affiliates.Active">
    <Value>Active</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateAdd.Title">
    <Value>Add a new affiliate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateAdd.BackToAffiliates">
    <Value>back to affiliate list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateAdd.AddButton.Text">
    <Value>Add</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateAdd.AddButton.Tooltip">
    <Value>Add affiliate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateDetails.Title">
    <Value>Edit affiliate details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateDetails.BackToAffiliates">
    <Value>back to affiliate list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateDetails.SaveButton.Tooltip">
    <Value>Save affiliate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateDetails.DeleteButton.Tooltip">
    <Value>Delete affiliate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateDetails.AffiliateInfo">
    <Value>Affiliate Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateDetails.Customers">
    <Value>Affiliated customers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateDetails.Orders">
    <Value>Affiliate orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.AffiliateID">
    <Value>Affiliate identifier:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.AffiliateID.Tooltip">
    <Value>Affiliate''s unique identifier.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.FirstName">
    <Value>First name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.FirstName.Tooltip">
    <Value>Affiliate''s first name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.LastName">
    <Value>Last name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.LastName.Tooltip">
    <Value>Affiliate''s last name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.LastName.ErrorMessage">
    <Value>Last name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.MiddleName">
    <Value>Middle name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.MiddleName.Tooltip">
    <Value>Affiliate''s middle name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.PhoneNumber">
    <Value>Phone number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.PhoneNumber.Tooltip">
    <Value>Affiliate''s phone number.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Email">
    <Value>Email:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Email.Tooltip">
    <Value>Affiliate''s email address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.FaxNumber">
    <Value>Fax number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.FaxNumber.Tooltip">
    <Value>Affiliate''s fax number.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Company">
    <Value>Company:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Company.Tooltip">
    <Value>Affiliate''s company name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Address1">
    <Value>Address 1:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Address1.Tooltip">
    <Value>Affiliate''s address line 1.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Address1.ErrorMessage">
    <Value>Address is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Address2">
    <Value>Address 2:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Address2.Tooltip">
    <Value>Affiliate''s address line 2.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.City">
    <Value>City:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.City.Tooltip">
    <Value>Affiliate''s address city.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.City.ErrorMessage">
    <Value>City is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.StateProvince">
    <Value>State / Province:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.StateProvince.Tooltip">
    <Value>Affiliate''s state / province.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Zip">
    <Value>Zip / Postal Code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Zip.Tooltip">
    <Value>Affiliate''s zip / postal code.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Zip.ErrorMessage">
    <Value>Zip / Postal code is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Country">
    <Value>Country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Country.Tooltip">
    <Value>Affiliate''s country.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Active">
    <Value>Active:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateInfo.Active.Tooltip">
    <Value>Determines whether the affiliate is active.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateCustomers.Email">
    <Value>Email</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateCustomers.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateCustomers.View">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateCustomers.View.Tooltip">
    <Value>View customer details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateOrders.OrderID">
    <Value>Order ID</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateOrders.OrderTotal">
    <Value>Order total</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateOrders.OrderStatus">
    <Value>Order Status</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateOrders.PaymentStatus">
    <Value>Payment Status</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateOrders.ShippingStatus">
    <Value>Shipping Status</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateOrders.Customer">
    <Value>Customer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateOrders.Customer.Guest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateOrders.View">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AffiliateOrders.CreatedOn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.Title">
    <Value>Manage Campaigns</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.AddNewButton.Tooltip">
    <Value>Add a new sales campaign</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.CreatedOn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.Edit.Tooltip">
    <Value>Edit campaign</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.CampaignAdd.Title">
    <Value>Add a new campaign</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.CampaignAdd.BackToCampaign">
    <Value>back to campaign list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.AddButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Campaigns.AddButton.Tooltip">
    <Value>Save campaign</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignDetails.Title">
    <Value>Edit campaign details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignDetails.BackToCampaign">
    <Value>back to campaign list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignDetails.SaveButton.Tooltip">
    <Value>Save campaign</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignDetails.DeleteButton.Tooltip">
    <Value>Delete campaign</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.Title">
    <Value>Make sure you''ve tested the campaign before sending it out to multiple customers. Save your campaign first by clicking "Save" button.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.SendTestEmailTo">
    <Value>Send test email to:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.SendTestEmailTo.Tooltip">
    <Value>The email address to which you want to send your test email.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.SendTestEmailButton.Text">
    <Value>Send test Email</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.SendTestEmailButton.Tooltip">
    <Value>Send test Email</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.AllowedTokens">
    <Value>Allowed message tokens:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.AllowedTokens.Tooltip">
    <Value>This is a list of the message tokens you can use in your campaign emails. You can also add message tokens using the ''Message Tokens'' drop down list in the editor below.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.SendMassEmailButton.Text">
    <Value>Send Mass Email</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.SendMassEmailButton.Tooltip">
    <Value>Send Mass Email</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.CampaignName">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.CampaignName.Tooltip">
    <Value>The name of this campaign.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.CampaignName.ErrorMessage">
    <Value>Campaign name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.Subject">
    <Value>Subject:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.Subject.Tooltip">
    <Value>The subject of your campaign email.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.Subject.ErrorMessage">
    <Value>Subject is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.Body">
    <Value>Body:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.Body.Tooltip">
    <Value>The body of your campaign email.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.CreatedOn.Tooltip">
    <Value>The date/time the campaign was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.EmailSent">
    <Value>Email has been successfully sent.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CampaignInfo.EmailSentToCustomers">
    <Value>Emails has been successfully sent to {0} customers.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.Title">
    <Value>Manage Discounts</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.AddNewButton.Tooltip">
    <Value>Add a new sales discount</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.UsePercentage">
    <Value>Use percentage</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.DiscountPercentage">
    <Value>Discount percentage</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.DiscountAmount">
    <Value>Discount amount</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.StartDate">
    <Value>Start date</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.EndDate">
    <Value>End date</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Discounts.Edit.Tooltip">
    <Value>Edit discount</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountAdd.Title">
    <Value>Add a new discount</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountAdd.BackToDiscounts">
    <Value>back to discount list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountAdd.AddButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountAdd.AddButton.Tooltip">
    <Value>Save discount</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountDetails.Title">
    <Value>Edit discount details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountDetails.BackToDiscounts">
    <Value>back to discount list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountDetails.SaveButton">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountDetails.SaveButton.Tooltip">
    <Value>Save discount</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountDetails.DeleteButton">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountDetails.DeleteButton.Tooltip">
    <Value>Delete discount</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountInfo">
    <Value>Discount Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.CustomerRoles">
    <Value>Customer Roles</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountType">
    <Value>Discount type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountType.Tooltip">
    <Value>The type of discount. Choose from applying to whole order or to individual product variants (SKU''s).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountRequirement">
    <Value>Discount requirement:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountRequirement.Tooltip">
    <Value>Requirements for discount to be applied.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.Name.Tooltip">
    <Value>The discount name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.UsePercentage">
    <Value>Use percentage:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.UsePercentage.Tooltip">
    <Value>Determines whether to apply a percentage discount to the order/SKUs. If not enabled, a set value is discounted.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountPercentage">
    <Value>Discount Percentage:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountPercentage.Tooltip">
    <Value>The percentage discount to apply to order/SKUs.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountPercentage.RequiredErrorMessage">
    <Value>Discount percentage is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountPercentage.RangeErrorMessage">
    <Value>The value must be from 0 to 100</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountAmount">
    <Value>Discount amount:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountAmount.Tooltip">
    <Value>The discount amount to apply to the order/SKUs.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountAmount.RequiredErrorMessage">
    <Value>Discount amount is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.DiscountAmount.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.StartDate">
    <Value>Start date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.StartDate.Tooltip">
    <Value>The start of the discount period in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.StartDate.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.EndDate">
    <Value>End date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.EndDate.Tooltip">
    <Value>The end of the discount period in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.EndDate.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.RequiresCouponCode">
    <Value>Requires coupon code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.RequiresCouponCode.Tooltip">
    <Value>If checked, a customer supply a valid coupon code for the discount to be applied.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.CouponCode">
    <Value>Coupon code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.DiscountInfo.CouponCode.Tooltip">
    <Value>The discount coupon code.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Pricelists.Title">
    <Value>Price Lists</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Pricelists.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Pricelists.AddNewButton.Tooltip">
    <Value>Add a new price list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Pricelists.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Pricelists.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Pricelists.Edit.Tooltip">
    <Value>Edit price list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistAdd.Title">
    <Value>Add a new pricelist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistAdd.BackToPricelists">
    <Value>back to pricelist list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistAdd.SaveButton.Tooltip">
    <Value>Save pricelist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistDetails.Title">
    <Value>Edit pricelist details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistDetails.BackToPricelists">
    <Value>back to pricelist list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistDetails.SaveButton.Tooltip">
    <Value>Save pricelist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistDetails.DeleteButton.Tooltip">
    <Value>Delete pricelist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.PricelistInfo">
    <Value>Pricelist Info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.AllowedTokens">
    <Value>List of allowed message tokens: please be aware, that you can also add formatting conditions (e.g. %pv.price:0.00%), the selected "Format-Localization" will decide which decimal points etc. are used</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.RequestTooltip">
    <Value>The pricelist can be requested by "http://www.yourStore.com/Pricelist.csv?PricelistGuid=abc"</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.DisplayName">
    <Value>Display name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.DisplayName.Tooltip">
    <Value>The pricelist display name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.DisplayName.ErrorMessage">
    <Value>Display name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ShortName">
    <Value>Short name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ShortName.Tooltip">
    <Value>The pricelist short name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.PricelistGUID">
    <Value>Pricelist GUID:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.PricelistGUID.Tooltip">
    <Value>The pricelist GUID.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Header">
    <Value>Exportfile header:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Header.Tooltip">
    <Value>The pricelist exportfile header.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Body">
    <Value>Exportfile body:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Body.Tooltip">
    <Value>The pricelist exportfile body.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Body.ErrorMessage">
    <Value>The pricelist exportfile body.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Footer">
    <Value>Exportfile footer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Footer.Tooltip">
    <Value>The pricelist exportfile footer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ExportMode">
    <Value>Export mode:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ExportMode.Tooltip">
    <Value>The pricelist export mode.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ExportType">
    <Value>Export type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ExportType.Tooltip">
    <Value>The pricelist export type.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Affiliate">
    <Value>Affiliate:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Affiliate.Tooltip">
    <Value>The associated affiliate.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Affiliate.None">
    <Value>None (ID=0)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.FormatLocalization">
    <Value>Format localization:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.FormatLocalization.Tooltip">
    <Value>The pricelist exportfile header.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.CacheTime">
    <Value>Cache time:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.CacheTime.Tooltip">
    <Value>The pricelist cache time.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.CacheTime.RangeErrorMessage">
    <Value>The value must be from 0 to 64000</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.CacheTime.RequiredErrorMessage">
    <Value>Cache time is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Description.Tooltip">
    <Value>The pricelist description.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.AdminNotes">
    <Value>Admin notes:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.AdminNotes.Tooltip">
    <Value>Admin notes relating to this pricelist.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Override">
    <Value>Override price adjustments:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.Override.Tooltip">
    <Value>Pricelist override price adjustments.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.PriceAdjustmentType">
    <Value>Price adjustment type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.PriceAdjustmentType.Tooltip">
    <Value>Type of price adjustment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.PriceAdjustment">
    <Value>Price adjustment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.PriceAdjustment.Tooltip">
    <Value>The price adjustment to make.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.PriceAdjustment.RequiredErrorMessage">
    <Value>Price adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.PriceAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ProductVariants">
    <Value>Product Variants</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ProductVariants.ProductVariant">
    <Value>Product variant</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ProductVariants.PriceAdjustmentType">
    <Value>Price adjustment type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ProductVariants.PriceAdjustment">
    <Value>Price adjustment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ProductVariants.PriceAdjustment.RequiredErrorMessage">
    <Value>Price adjustment is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ProductVariants.PriceAdjustment.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PricelistInfo.ProductVariants.Empty">
    <Value>No product variants</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Froogle.Title">
    <Value>Froogle</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Froogle.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Froogle.SaveButton.Tooltip">
    <Value>Save settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Froogle.GenerateButton.Text">
    <Value>Generate feed</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Froogle.GenerateButton.Tooltip">
    <Value>Generate froogle feed</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Froogle.AllowPublicAccess">
    <Value>Allow public access:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Froogle.SuccessResult">
    <Value>Froogle feed has been successfully generated. {0} to see generated feed</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Froogle.ClickHere">
    <Value>Click here</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.Title">
    <Value>Manage Polls</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.SaveButton.Tooltip">
    <Value>Save changes to polls</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.AddNewButton.Tooltip">
    <Value>Add a new poll</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.ShowOnMainPage">
    <Value>Show on home page:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.ShowOnMainPage.Tooltip">
    <Value>Check to show polls on your store home page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.Language">
    <Value>Language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Polls.Edit.Tooltip">
    <Value>Edit poll</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollAdd.Title">
    <Value>Add a new poll</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollAdd.BackToPolls">
    <Value>back to polls list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollAdd.AddButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollAdd.AddButton.Tooltip">
    <Value>Save poll</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollDetails.Title">
    <Value>Edit poll details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollDetails.BackToPolls">
    <Value>back to polls list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollDetails.SaveButton.Tooltip">
    <Value>Save poll</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollDetails.DeleteButton.Tooltip">
    <Value>Delete poll</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.Language">
    <Value>Language:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.Language.Tooltip">
    <Value>Choose the language for this poll. Customers who have this language selected in your store, will then see the poll.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.Name.Tooltip">
    <Value>The descriptive name of this poll. This is the text that the customer will see e.g. ''What do you think of our store?''.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.SystemKeyword">
    <Value>System keyword:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.SystemKeyword.Tooltip">
    <Value>A system keyword for this poll.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.Published">
    <Value>Published:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.Published.Tooltip">
    <Value>Determines whether this poll is published (visible) in your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.DisplayOrder.Tooltip">
    <Value>The display order of this poll. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.PollAnswerColumn">
    <Value>Poll answer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.PollAnswerColumn.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.CountColumn">
    <Value>Count</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.DisplayOrderColumn">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.DisplayOrderColumn.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.DisplayOrderColumn.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.UpdateColumn">
    <Value>Update</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.UpdateColumn.Tooltip">
    <Value>Update poll answer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.DeleteColumn">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.DeleteColumn.Tooltip">
    <Value>Delete poll answer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.Title">
    <Value>Add a new poll answer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.AnswerName">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.AnswerName.Tooltip">
    <Value>The name (value) of this answer. This is the text that the customer will see e.g. ''Excellant''.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.AnswerName.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.DisplayOrder.Tooltip">
    <Value>The display order of this poll answer. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.AddNewAnswerButton.Text">
    <Value>Add poll answer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PollInfo.New.AddNewAnswerButton.Tooltip">
    <Value>Add poll answer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.Title">
    <Value>News Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.SaveButton.Tooltip">
    <Value>Save changes to news</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.NewsEnabled">
    <Value>News enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.NewsEnabled.Tooltip">
    <Value>Check to enable news.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.AllowGuestComments">
    <Value>Allow not registered users to leave comments:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.AllowGuestComments.Tooltip">
    <Value>Check to allow not registered users to leave comments.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.NotifyComments">
    <Value>Notify about new news comments:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.NotifyComments.Tooltip">
    <Value>Check to notify store owner about new news comments.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.ShowOnMainPage">
    <Value>Show on home page:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.ShowOnMainPage.Tooltip">
    <Value>Check to display your news items on your store home page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.NewsCount">
    <Value>Number of items to display:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.NewsCount.Tooltip">
    <Value>The number of news items to display on your home page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.NewsCount.RequiredErrorMessage">
    <Value>News count is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsSettings.NewsCount.RangeErrorMessage">
    <Value>The value must be from 0 to 99</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.Title">
    <Value>Manage News</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.AddNewButton.Tooltip">
    <Value>Add a news entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.Title">
    <Value>Title</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.Language">
    <Value>Language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.Edit.Tooltip">
    <Value>Edit news item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.CreatedOn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.ViewComments">
    <Value>View comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.ViewComments.Tooltip">
    <Value>View news entry comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.News.ViewComments.Link">
    <Value>{0} comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsAdd.Title">
    <Value>Add a new news item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsAdd.BackToNews">
    <Value>back to news item list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsAdd.SaveButton.Tooltip">
    <Value>Save news item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsDetails.Title">
    <Value>Edit news item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsDetails.BackToNews">
    <Value>back to news item list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsDetails.SaveButton.Tooltip">
    <Value>Save news item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsDetails.DeleteButton.Tooltip">
    <Value>Delete news item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Language">
    <Value>Language:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Language.Tooltip">
    <Value>The language of this news item. A customer will only see the news items for their selected language.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Title">
    <Value>Title:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Title.Tooltip">
    <Value>The title of this news item e.g. ''Launch of our new nopCommerce store.''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Title.ErrorMessage">
    <Value>Title is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Short">
    <Value>Short description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Short.Tooltip">
    <Value>A short description (abstract) of this news item. This is the text that visitors will see when looking at your news item list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Full">
    <Value>Full description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Full.Tooltip">
    <Value>The full description (body) of the news item.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Published">
    <Value>Published:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.Published.Tooltip">
    <Value>Determines whether the news item is published (visible) in your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.AllowComments">
    <Value>Allow comments:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.AllowComments.Tooltip">
    <Value>When checked, customers can leave comments about your news items.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.CreatedOn.Tooltip">
    <Value>The date/time the news item was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.ViewComments">
    <Value>View Comments ({0})</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsInfo.ViewComments.Tooltip">
    <Value>View comments for this news item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsComments.Title">
    <Value>Manage News Comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsComments.Customer.Guest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsComments.EditButton.Text">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsComments.EditButton.Tooltip">
    <Value>Edit news entry comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsComments.Delete.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsComments.Delete.Tooltip">
    <Value>Delete news entry comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.Title">
    <Value>Edit news item comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.BackToNewsComments">
    <Value>back to news comments list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.SaveButton.Tooltip">
    <Value>Save news item comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.DeleteButton.Tooltip">
    <Value>Delete news item comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.Customer">
    <Value>Customer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.Customer.Tooltip">
    <Value>The customer who made the comment.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.Customer.Guest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.News">
    <Value>News item:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.News.Tooltip">
    <Value>The news item for this comment. Click link to view/edit the news item.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.CommentTitle">
    <Value>Title:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.CommentTitle.Tooltip">
    <Value>The title of the customer''s comment.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.CommentBody">
    <Value>Comment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.CommentBody.Tooltip">
    <Value>The comment made by the customer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.NewsCommentDetails.CreatedOn.Tooltip">
    <Value>The date/time the comment was made.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.Title">
    <Value>Blog Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.SaveButton.Tooltip">
    <Value>Save changes to blog</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.EnableBlog">
    <Value>Blog enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.EnableBlog.Tooltip">
    <Value>Check to enable the blog in your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.AllowGuestComments">
    <Value>Allow not registered users to leave comments:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.AllowGuestComments.Tooltip">
    <Value>Check to allow not registered users to leave comments.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.NotifyNewComments">
    <Value>Notify about new blog comments:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogSettings.NotifyNewComments.Tooltip">
    <Value>Check to notify store owner about new blog comments.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.Title">
    <Value>Manage Blog</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.AddNewButton.Tooltip">
    <Value>Add a new blog entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.BlogPostTitle">
    <Value>Blog post title</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.Language">
    <Value>Language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.Edit.Tooltip">
    <Value>Edit blog entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.CreatedOn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.ViewComments">
    <Value>View comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.ViewComments.Tooltip">
    <Value>View blog entry comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blog.ViewComments.Link">
    <Value>{0} comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostAdd.Title">
    <Value>Add a blog entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostAdd.BackToBlog">
    <Value>back to blog entry list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostAdd.SaveButton.Tooltip">
    <Value>Save blog entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostDetails.Title">
    <Value>Edit blog entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostDetails.BackToBlog">
    <Value>back to blog entry list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostDetails.SaveButton.Tooltip">
    <Value>Save blog entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostDetails.DeleteButton.Tooltip">
    <Value>Delete blog entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.Language">
    <Value>Language:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.Language.Tooltip">
    <Value>The language of this blog entry. A customer will only see the blog entries for their selected language.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.Name">
    <Value>Blog entry title:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.Name.Tooltip">
    <Value>The title of this blog entry.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.Name.ErrorMessage">
    <Value>Title is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.Body">
    <Value>Blog entry body:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.Body.Tooltip">
    <Value>The body of this blog entry.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.AllowComments">
    <Value>Allow comments:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.AllowComments.Tooltip">
    <Value>When checked, customers can leave comments about your blog entry.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.CreatedOn.Tooltip">
    <Value>The date/time the blog entry was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogPostInfo.ViewComments">
    <Value>View Comments ({0})</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogComments.Title">
    <Value>Manage Blog Comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogComments.Customer.Guest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogComments.EditButton.Text">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogComments.EditButton.Tooltip">
    <Value>Edit blog entry comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogComments.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogComments.DeleteButton.Tooltip">
    <Value>Delete blog entry comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.Title">
    <Value>Edit blog entry comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.BackToComments">
    <Value>back to blog comments list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.SaveButton.Tooltip">
    <Value>Save blog entry comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.DeleteButton.Tooltip">
    <Value>Delete blog entry comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.Customer">
    <Value>Customer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.Customer.Tooltip">
    <Value>The customer who made the comment.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.Customer.Guest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.BlogTitle">
    <Value>Blog entry:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.BlogTitle.Tooltip">
    <Value>The blog entry for this comment. Click link to view/edit the blog entry.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.Comment">
    <Value>Comment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.Comment.Tooltip">
    <Value>The comment made by the customer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlogCommentDetails.CreatedOn.Tooltip">
    <Value>The date/time the comment was made.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.Title">
    <Value>Manage Topics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.AddNewButton.Tooltip">
    <Value>Add a new topic</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.Language">
    <Value>Language:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.Language.Tooltip">
    <Value>Select a language for the topic. A topic can be created for each language that your store supports.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.EditInfo">
    <Value>Edit (info)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.EditInfo.Tooltip">
    <Value>Edit topic info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.EditInfo.Link">
    <Value>Edit topic info</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.EditContent">
    <Value>Edit (content)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.EditContent.Tooltip">
    <Value>Edit topic content</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Topics.EditContent.Link">
    <Value>Edit topic content</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicAdd.Title">
    <Value>Add a new topic</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicAdd.BackToTopics">
    <Value>back to topics list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicAdd.AddButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicAdd.AddButton.Tooltip">
    <Value>Save topic</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicDetails.Title">
    <Value>Edit topic</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicDetails.BackToTopics">
    <Value>back to topics list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicDetails.SaveButton.Tooltip">
    <Value>Save topic</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicDetails.DeleteButton.Tooltip">
    <Value>Delete topic</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicInfo.Name.Tooltip">
    <Value>The topic name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.Title">
    <Value>Edit topic details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.BackToTopics">
    <Value>back to topics list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.SaveButton.Tooltip">
    <Value>Save topic</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.DeleteButton.Tooltip">
    <Value>Delete topic</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.Language">
    <Value>Language:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.Language.Tooltip">
    <Value>The language that this topic is for.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.Topic">
    <Value>Topic:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.Topic.Tooltip">
    <Value>The name of this topic (read only).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.LocalizedTopicTitle">
    <Value>Title:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.LocalizedTopicTitle.Tooltip">
    <Value>The title of the topic.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.URL">
    <Value>Topic URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.URL.Tooltip">
    <Value>This is the localized topic URL that you can use to link to this topic from other areas of your site.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.Body">
    <Value>Body:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.Body.Tooltip">
    <Value>The body of your topic.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.CreatedOn.Tooltip">
    <Value>The date/time the topic was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TopicLocalizedDetails.UpdatedOn.Tooltip">
    <Value>The date/time the topic was updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.Title">
    <Value>Forums Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.SaveButton.Tooltip">
    <Value>Save changes to forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.ForumsEnabled">
    <Value>Forums enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.ForumsEnabled.Tooltip">
    <Value>Check to enable forums.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.ShowPostCount">
    <Value>Show customers post count:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.ShowPostCount.Tooltip">
    <Value>A value indicating whether to show customers post count.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.AllowToEditPosts">
    <Value>Allow customers to edit posts:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.AllowToEditPosts.Tooltip">
    <Value>A value indicating whether customers are allowed to edit posts that they created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.AllowToDeletePosts">
    <Value>Allow customers to delete posts:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.AllowToDeletePosts.Tooltip">
    <Value>A value indicating whether customers are allowed to delete posts that they created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.TopicsPageSize">
    <Value>Topics page size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.TopicsPageSize.Tooltip">
    <Value>Set the page size for topics in forums e.g. ''10'' topics per page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.TopicsPageSize.RequiredErrorMessage">
    <Value>Enter page size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.TopicsPageSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.PostsPageSize">
    <Value>Posts page size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.PostsPageSize.Tooltip">
    <Value>Set the page size for posts in topics e.g. ''10'' posts per page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.PostsPageSize.RequiredErrorMessage">
    <Value>Enter page size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.PostsPageSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.ForumEditor">
    <Value>Forum editor:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.ForumEditor.Tooltip">
    <Value>Forum editor type. WARNING: not recommended to change in production environment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.SignatureEnabled">
    <Value>Signature enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumsSettings.SignatureEnabled.Tooltip">
    <Value>Add an opportunity for customers to speficy signature. Signature should be dispayed below each forum post.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.Title">
    <Value>Manage Forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.ForumGroupAdd.Text">
    <Value>Add new forum group</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.ForumGroupAdd.Tooltip">
    <Value>Add new forum group</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.AddNewForum.Text">
    <Value>Add new forum</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.AddNewForum.Tooltip">
    <Value>Add new forum</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.NameColumn">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.DisplayOrderColumn">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.CreatedOnColumn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.EditColumn">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.EditColumn.ForumGroup">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Forums.EditColumn.Forum">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupAdd.Title">
    <Value>Add a new forum group</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupAdd.BackToForums">
    <Value>back to forum group list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupAdd.SaveButton.Tooltip">
    <Value>Save forum group</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupDetails.Title">
    <Value>Edit forum group details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupDetails.BackToForums">
    <Value>back to forum group list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupDetails.SaveButton.Tooltip">
    <Value>Save forum group</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupDetails.DeleteButton.Tooltip">
    <Value>Delete forum group</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.Name.Tooltip">
    <Value>The name of this forum group. This is the name that the customer will see.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.Name.ErrorMessage">
    <Value>Name is required"></Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.Description.Tooltip">
    <Value>A description of the forum group. This is the description that the customer will see.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.DisplayOrder.Tooltip">
    <Value>The display order of the forum group. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.CreatedOn.Tooltip">
    <Value>The date/time the forum group was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumGroupInfo.UpdatedOn.Tooltip">
    <Value>The date/time the forum group was updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumAdd.Title">
    <Value>Add a new forum</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumAdd.BackToForums">
    <Value>back to forum list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumAdd.AddButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumAdd.AddButton.Tooltip">
    <Value>Save forum</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumDetails.Title">
    <Value>Edit forum details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumDetails.BackToForums">
    <Value>back to forum list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumDetails.SaveButton.Tooltip">
    <Value>Save forum</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumDetails.DeleteButton.Tooltip">
    <Value>Delete forum</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.ForumGroup">
    <Value>Forum group:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.ForumGroup.Tooltip">
    <Value>Choose a forum group.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.Name.Tooltip">
    <Value>The name of this forum. This is the name that the customer will see.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.Description.Tooltip">
    <Value>A description of the forum. This is the description that the customer will see.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.DisplayOrder.Tooltip">
    <Value>The display order of the forum. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.CreatedOn.Tooltip">
    <Value>The date/time the forum was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ForumInfo.UpdatedOn.Tooltip">
    <Value>The date/time the forum was updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplates.Title">
    <Value>Manage Product Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplates.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplates.AddButton.Tooltip">
    <Value>Add a new product template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplates.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplates.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplates.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplates.Edit.Tooltip">
    <Value>Edit product template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateAdd.Title">
    <Value>Add a new product template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateAdd.BackToTemplates">
    <Value>back to product templates list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateAdd.AddButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateAdd.AddButton.Tooltip">
    <Value>Save template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateDetails.Title">
    <Value>Edit product template details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateDetails.BackToTemplates">
    <Value>back to product templates list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateDetails.SaveButton.Tooltip">
    <Value>Save product template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateDetails.DeleteButton.Tooltip">
    <Value>Delete product template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.Name.Tooltip">
    <Value>The friendly name of the template.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.TemplatePath">
    <Value>Template path:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.TemplatePath.Tooltip">
    <Value>The logical path of the template file e.g. ''Templates\Products\MyTemplate.ascx''.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.DisplayOrder.Tooltip">
    <Value>The display order of this template. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.CreatedOn.Tooltip">
    <Value>The date/time the template was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ProductTemplateInfo.UpdatedOn.Tooltip">
    <Value>The date/time the template was updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplates.Title">
    <Value>Manage Category Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplates.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplates.AddButton.Tooltip">
    <Value>Add a new category template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplates.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplates.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplates.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplates.Edit.Tooltip">
    <Value>Edit category template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateAdd.Title">
    <Value>Add a new category template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateAdd.BackToTemplates">
    <Value>back to category templates list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateAdd.AddButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateAdd.AddButton.Tooltip">
    <Value>Save template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateDetails.Title">
    <Value>Edit category template details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateDetails.BackToTemplates">
    <Value>back to category templates list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateDetails.SaveButton.Tooltip">
    <Value>Save category template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateDetails.DeleteButton.Tooltip">
    <Value>Delete category template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.Name.Tooltip">
    <Value>The friendly name of the template.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.TemplatePath">
    <Value>Template path:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.TemplatePath.Tooltip">
    <Value>The logical path of the template file e.g. ''Templates\Categories\MyTemplate.ascx''.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.DisplayOrder.Tooltip">
    <Value>The display order of this template. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.CreatedOn.Tooltip">
    <Value>The date/time the template was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CategoryTemplateInfo.UpdatedOn.Tooltip">
    <Value>The date/time the template was updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplates.Title">
    <Value>Manage Manufacturer Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplates.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplates.AddButton.Tooltip">
    <Value>Add a new manufacturer template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplates.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplates.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplates.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplates.Edit.Tooltip">
    <Value>Edit manufacturer template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateAdd.Title">
    <Value>Add a new manufacturer template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateAdd.BackToTemplates">
    <Value>back to manufacturer templates list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateAdd.AddButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateAdd.AddButton.Tooltip">
    <Value>Save template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateDetails.Title">
    <Value>Edit manufacturer template details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateDetails.BackToTemplates">
    <Value>back to manufacturer templates list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateDetails.SaveButton.Tooltip">
    <Value>Save manufacturer template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateDetails.DeleteButton.Tooltip">
    <Value>Delete manufacturer template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.Name.Tooltip">
    <Value>The friendly name of the template.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.TemplatePath">
    <Value>Template path:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.TemplatePath.Tooltip">
    <Value>The logical path of the template file e.g. ''Templates\Manufacturers\MyTemplate.ascx''.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.DisplayOrder.Tooltip">
    <Value>The display order of this template. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.CreatedOn.Tooltip">
    <Value>The date/time the template was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ManufacturerTemplateInfo.UpdatedOn.Tooltip">
    <Value>The date/time the template was updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplates.Title">
    <Value>Manage Message Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplates.Language">
    <Value>Select language:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplates.Language.Tooltip">
    <Value>Select a language for the template. A message template can be created for each language that your store supports.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplates.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplates.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplates.Edit.Tooltip">
    <Value>Edit message template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Title">
    <Value>Edit message template details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.BackToTemplates">
    <Value>back to message templates list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.SaveButton.Tooltip">
    <Value>Save message template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.DeleteButton.Tooltip">
    <Value>Delete message template</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.AllowedTokens">
    <Value>Allowed message tokens:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.AllowedTokens.Tooltip">
    <Value>This is a list of tokens you can use in your messages. You can also select tokens from the ''Message Tokens'' drop down list in the editor below.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Language">
    <Value>Language:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Language.Tooltip">
    <Value>The language that this template is for.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Name">
    <Value>Template:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Name.Tooltip">
    <Value>The name of this template (read only).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Subject">
    <Value>Subject:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Subject.Tooltip">
    <Value>The subject of the message (email). TIP - You can include tokens in your subject.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Subject.ErrorMessage">
    <Value>Subject is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Body">
    <Value>Body:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageTemplateDetails.Body.Tooltip">
    <Value>The body of your message.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.Title">
    <Value>Localization</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.AddNewButton.Tooltip">
    <Value>Add a new resource string</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.Language">
    <Value>Select language:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.SelectLanguage">
    <Value>Select language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.LanguageColumn">
    <Value>Language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.ResourceNameColumn">
    <Value>Resource name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.ResourceValueColumn">
    <Value>Resource value</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.EditColumn">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResources.EditColumn.Tooltip">
    <Value>Edit resource string</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceAdd.Title">
    <Value>Add a new resource string</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceAdd.BackToResources">
    <Value>(back to resources list)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceAdd.SaveButton.Tooltip">
    <Value>Save resource string</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceDetails.Title">
    <Value>Edit resource string</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceDetails.BackToResources">
    <Value>(back to resources list)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceDetails.SaveButton.Tooltip">
    <Value>Save resource string</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceDetails.DeleteButton.Tooltip">
    <Value>Delete resource string</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceInfo.Language">
    <Value>Language:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceInfo.Language.Tooltip">
    <Value>The language that this resource string is for.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceInfo.Name">
    <Value>Resource name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceInfo.Name.Tooltip">
    <Value>The resource string identifier.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceInfo.Name.ErrorMessage">
    <Value>Resource name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceInfo.Value">
    <Value>Resource value:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceInfo.Value.Tooltip">
    <Value>The value for this resource string.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LocaleStringResourceInfo.Value.ErrorMessage">
    <Value>Resource value is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Title">
    <Value>Global Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SaveButton.Tooltip">
    <Value>Save global store settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.Title">
    <Value>General</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.StoreName">
    <Value>Store name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.StoreName.Tooltip">
    <Value>Enter the name of your store e.g. Your Store</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.ErrorMessage">
    <Value>Store name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.StoreUrl">
    <Value>Store URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.StoreUrl.Tooltip">
    <Value>The URL of your store e.g. http://www.yourstore.com</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.StoreUrl.ErrorMessage">
    <Value>Store URL is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.StoreClosed">
    <Value>Store closed:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.StoreClosed.Tooltip">
    <Value>Check to close the store. Uncheck to re-open.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.AnonymousCheckout">
    <Value>Anonymous checkout allowed:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.General.AnonymousCheckout.Tooltip">
    <Value>Check to enable anonymous checkout (customers are not required to login/register when purchasing products)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.Title">
    <Value>SEO/Display</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.StoreNamePrefix">
    <Value>Enable store name prefix:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.StoreNamePrefix.Tooltip">
    <Value>Prefix product / category / manufacturer page titles with the store name e.g. Your Store: Your Product Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.DefaultTitle">
    <Value>Default title:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.DefaultTitle.Tooltip">
    <Value>The default title for pages in your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ErrorMessage">
    <Value>Default title is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.MetaDescription">
    <Value>Default meta description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.MetaDescription.Tooltip">
    <Value>The default meta description for pages in your store. You can override this for individual categories / products / manufacturer pages.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.MetaDescription.ErrorMessage">
    <Value>Default meta description is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.MetaKeywords">
    <Value>Default meta keywords:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.MetaKeywords.Tooltip">
    <Value>The default meta keywords for pages in your store. You can override these for individual categories / products / manufacturer pages.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.MetaKeywords.ErrorMessage">
    <Value>Default meta keywords are required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.PublicStoreTheme">
    <Value>Store theme:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.PublicStoreTheme.Tooltip">
    <Value>The public store theme. You can download themes from the extensions page at www.nopcommerce.com.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ShowWelcomeMessage">
    <Value>Show welcome message on home page:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ShowWelcomeMessage.Tooltip">
    <Value>Show the welcome message on your store''s home page</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.NewsRssLink">
    <Value>Display news RSS feed link in the browser address bar:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.NewsRssLink.Tooltip">
    <Value>Check to enable the news rss feed link in customers browser address bar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.BlogRssLink">
    <Value>Display blog RSS feed link in the browser address bar:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.BlogRssLink.Tooltip">
    <Value>Check to enable the blog rss feed link in customers browser address bar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.UrlRewriting">
    <Value>If you modify the default url rewriting formats, please ensure you have created the necessary rewrite rule in UrlRewriting.config. By default, only .aspx extensions will be rewritten.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ProductUrl">
    <Value>Product url rewrite format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ProductUrl.Tooltip">
    <Value>The format for product urls. Must have 3 arguments i.e. ''{0}Products/{1}-{2}.aspx''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ProductUrl.ErrorMessage">
    <Value>You must enter a valid rewrite format string.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.CategoryUrl">
    <Value>Category url rewrite format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.CategoryUrl.Tooltip">
    <Value>The format for category urls. Must have 3 arguments i.e. ''{0}Category/{1}-{2}.aspx''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.CategoryUrl.ErrorMessage">
    <Value>You must enter a valid rewrite format string.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ManufacturerUrl">
    <Value>Manufacturer url rewrite format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ManufacturerUrl.Tooltip">
    <Value>The format for manufacturer urls. Must have 3 arguments i.e. ''{0}Manufacturer/{1}-{2}.aspx''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.ManufacturerUrl.ErrorMessage">
    <Value>You must enter a valid rewrite format string.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.NewsUrl">
    <Value>News url rewrite format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.NewsUrl.Tooltip">
    <Value>The format for news urls. Must have 3 arguments i.e. ''{0}News/{1}-{2}.aspx''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.NewsUrl.ErrorMessage">
    <Value>You must enter a valid rewrite format string.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.BlogUrl">
    <Value>Blog url rewrite format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.BlogUrl.Tooltip">
    <Value>The format for blog urls. Must have 3 arguments i.e. ''{0}Blog/{1}-{2}.aspx''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.BlogUrl.ErrorMessage">
    <Value>You must enter a valid rewrite format string.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.TopicUrl">
    <Value>Topic url rewrite format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.TopicUrl.Tooltip">
    <Value>The format for topic urls. Must have 3 arguments i.e. ''{0}Topic/{1}-{2}.aspx''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.SEODisplay.TopicUrl.ErrorMessage">
    <Value>You must enter a valid rewrite format string.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.Title">
    <Value>Media</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.MaxImageSize">
    <Value>Maximum image size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.MaxImageSize.Tooltip">
    <Value>The maximum image size (longest side) allowed for image uploads.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.MaxImageSize.RequiredErrorMessage">
    <Value>Enter a maximum image size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.MaxImageSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductThumbSize">
    <Value>Product thumbnail image size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductThumbSize.Tooltip">
    <Value>The default size (pixels) for product thumbnail images.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductThumbSize.RequiredErrorMessage">
    <Value>Enter a product thumbnail image size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductThumbSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductDetailSize">
    <Value>Product detail image size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductDetailSize.Tooltip">
    <Value>The default size (pixels) for product detail images.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductDetailSize.RequiredErrorMessage">
    <Value>Enter a product detail image size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductDetailSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductVariantSize">
    <Value>Product variant detail image size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductVariantSize.Tooltip">
    <Value>The default size (pixels) for product variant images.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductVariantSize.RequiredErrorMessage">
    <Value>Enter a product variant detail image size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ProductVariantSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.CategoryThumbSize">
    <Value>Category thumbnail image size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.CategoryThumbSize.Tooltip">
    <Value>The default size (pixels) for product thumbnail images on category pages.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.CategoryThumbSize.RequiredErrorMessage">
    <Value>Enter a category thumbnail image size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.CategoryThumbSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ManufacturerThumbSize">
    <Value>Manufacturer thumbnail image size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ManufacturerThumbSize.Tooltip">
    <Value>The default size (pixels) for product thumbnail images on manufacturer pages.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ManufacturerThumbSize.RequiredErrorMessage">
    <Value>Enter a manufacturer thumbnail image size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ManufacturerThumbSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ShowCartImages">
    <Value>Show product images on cart:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ShowCartImages.Tooltip">
    <Value>Determines whether product images should are displayed in your store shopping cart.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ShowWishListImages">
    <Value>Show product images on wishlist:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.ShowWishListImages.Tooltip">
    <Value>Determines whether product images should are displayed on customer wishlists.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.CartThumbSize">
    <Value>Cart/Wishlist thumbnail image size:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.CartThumbSize.Tooltip">
    <Value>The default size (pixels) for product thumbnail images on the shopping cart and wishlist.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.CartThumbSize.RequiredErrorMessage">
    <Value>Enter a shopping cart/wishlist thumbnail image size</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Media.CartThumbSize.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Units.Title">
    <Value>Units</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Units.BaseWeight">
    <Value>Base weight unit:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Units.BaseWeight.Tooltip">
    <Value>Set your store''s base weight unit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Units.BaseDimension">
    <Value>Base dimension unit:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Units.BaseDimension.Tooltip">
    <Value>Set your store''s base dimension unit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.Title">
    <Value>Mail Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.Email">
    <Value>Store Admin Email:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.Email.Tooltip">
    <Value>This is the from address for all outgoing emails from your store e.g. ''sales@yourstore.com''.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.EmailDisplayName">
    <Value>Store Admin Email Display Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.EmailDisplayName.Tooltip">
    <Value>This is the friendly display name for outgoing emails from your store e.g. ''Your Store Sales Department''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.EmailHost">
    <Value>Host:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.EmailHost.Tooltip">
    <Value>This is the host name or IP address of your mail server. You can normally find this out from your ISP or web host.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.EmailPort">
    <Value>Port:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.EmailPort.Tooltip">
    <Value>This is the SMTP port of your mail server. This is usually port 25.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.User">
    <Value>User:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.User.Tooltip">
    <Value>This is the username you use to authenticate to your mail server.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.Password">
    <Value>Password:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.Password.Tooltip">
    <Value>This is the password you use to authenticate to your mail server.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.SSL">
    <Value>Enable SSL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.SSL.Tooltip">
    <Value>Check to use Secure Sockets Layer (SSL) to encrypt the SMTP connection.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.DefaultCredentials">
    <Value>Use default credentials:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.DefaultCredentials.Tooltip">
    <Value>Check to use default credentials for the connection</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.SendTestEmail">
    <Value>Send Test Email (save settings first by clicking "Save" button)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.TestEmailTo">
    <Value>Send email to:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.TestEmailTo.Tooltip">
    <Value>The email address to which you want to send your test email.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.SendTestEmailButton.Text">
    <Value>Send Test Email</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.SendTestEmailButton.Tooltip">
    <Value>Send the test email</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.MailSettings.SendTestEmailSuccess">
    <Value>Email has been successfully sent.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Security.Title">
    <Value>Security</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Security.PrivateKey">
    <Value>Encryption private key:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Security.PrivateKey.Tooltip">
    <Value>The encryption private key used for storing sensitive data.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Security.PrivateKeyButton">
    <Value>Change</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Security.ChangeKeySuccess">
    <Value>Encryption private key has been successfully changed.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Security.LoginCaptcha">
    <Value>Login captcha image enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Security.LoginCaptcha.Tooltip">
    <Value>Check to enable captcha verification on the customer login page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Security.RegistrationCaptcha">
    <Value>Registration captcha image enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Security.RegistrationCaptcha.Tooltip">
    <Value>Check to enable captcha verification on the customer registration page (recommended).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.Title">
    <Value>Customer profiles</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.NameFormat">
    <Value>Customer name format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.NameFormat.Tooltip">
    <Value>Customer name format.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.AllowedAvatars">
    <Value>Allow customers to upload avatars:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.AllowedAvatars.Tooltip">
    <Value>A value indicating whether customers are allowed to upload avatars.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.DefaultAvatar">
    <Value>Default avatar enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.DefaultAvatar.Tooltip">
    <Value>A value indicating whether to display default user avatar.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.ViewingProfiles">
    <Value>Allow viewing customer profiles:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.ViewingProfiles.Tooltip">
    <Value>A value indicating whether to viewing profiles of customers are allowed.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.ShowLocation">
    <Value>Show customers location:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.ShowLocation.Tooltip">
    <Value>A value indicating whether customers location is shown.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.ShowJoinDate">
    <Value>Show customers join date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.ShowJoinDate.Tooltip">
    <Value>A value indicating whether to show customers join date.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.AllowPM">
    <Value>Allow private messages:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.AllowPM.Tooltip">
    <Value>A value indicating whether private messages are allowed.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.AllowToSetTimeZone">
    <Value>Allow customers to select time zone:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.AllowToSetTimeZone.Tooltip">
    <Value>Check to allow customers to select time zone. If checked, then time zone can be selected on the public store (account page). If not, then default time zone will be used.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.CurrentTimeZone">
    <Value>Current time zone:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.CurrentTimeZone.Tooltip">
    <Value>The current user time zone.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.DefaultTimeZone">
    <Value>Default store time zone:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Profiles.DefaultTimeZone.Tooltip">
    <Value>The default store time zone used to display dates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.Title">
    <Value>Other</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.Usernames">
    <Value>''Usernames'' enabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.Usernames.Tooltip">
    <Value>Check to use usernames for login/registration instead of emails. WARNING: not recommended to change in production environment.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.RegistrationDisabled">
    <Value>New customer registration is not allowed</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.RegistrationDisabled.Tooltip">
    <Value>Check to disable new customer registration.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.NavigationOnlyRegisteredCustomers">
    <Value>Allow navigation only for registered customers:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.NavigationOnlyRegisteredCustomers.Tooltip">
    <Value>Check to don''t allow customers navigate anywhere without entering their login information</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.EmailValidation">
    <Value>''Customer email validation'' enabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.EmailValidation.Tooltip">
    <Value>Check to require email validation during customer registration</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.CompareProducts">
    <Value>''Compare Products'' enabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.CompareProducts.Tooltip">
    <Value>Check to allow customers to use the ''Compare Products'' option in your store</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.WishList">
    <Value>''Wishlist'' Enabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.WishList.Tooltip">
    <Value>Check to enable customer wishlists in your store</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.EmailAFriend">
    <Value>''Email a friend'' enabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.EmailAFriend.Tooltip">
    <Value>Check to allow customers to use the ''Email a friend'' option in your store</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.RecentlyViewedProducts">
    <Value>''Recently viewed products'' enabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.RecentlyViewedProducts.Tooltip">
    <Value>Check to allow customers to use the ''Recently viewed products'' feature in your store</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.RecentlyAddedProducts">
    <Value>''Recently added products'' enabled</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.RecentlyAddedProducts.Tooltip">
    <Value>Check to allow customers to use the ''Recently added products'' feature in your store</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.NotifyNewProductReviews">
    <Value>Notify about new product reviews:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.NotifyNewProductReviews.Tooltip">
    <Value>Check to notify store owner about new product reviews.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.ShowBestsellersOnHomePage">
    <Value>Show best sellers on home page:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.ShowBestsellersOnHomePage.Tooltip">
    <Value>Check to show best sellers on home page.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.AlsoPurchased">
    <Value>''Products also purchased'' enabled:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.AlsoPurchased.Tooltip">
    <Value>Check to allow customers to view a list of products purchased by other customers who purchased the above</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.AlsoPurchasedNumber">
    <Value>Number of also purchased products to display:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.AlsoPurchasedNumber.Tooltip">
    <Value>The number of products also purchased by other customers to display when ''Products also purchased'' option is enabled.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.AlsoPurchasedNumber.RequiredErrorMessage">
    <Value>Enter a number of products also purchased by other customers to display</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.GlobalSettings.Other.AlsoPurchasedNumber.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.Title">
    <Value>Manage blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.AddBannedIP.Text">
    <Value>Add banned IP</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.AddBannedIP.Tooltip">
    <Value>Add IP to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.AddBannedNetwork.Text">
    <Value>Add banned network</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.AddBannedNetwork.Tooltip">
    <Value>Add network to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpAddress.Title">
    <Value>IP addresses</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpAddress.IPAddress">
    <Value>IP address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpAddress.IPAddress.Edit">
    <Value>Edit IP address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpAddress.Comment">
    <Value>Comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpAddress.CreatedOn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpAddress.UpdatedOn">
    <Value>Updated on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpNetwork.Title">
    <Value>IP networks</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpNetwork.IPRange">
    <Value>IP range</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpNetwork.IPRange.Edit">
    <Value>Edit IP network</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpNetwork.Comment">
    <Value>Comment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpNetwork.Exceptions">
    <Value>IP exceptions</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpNetwork.CreatedOn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Blacklist.IpNetwork.UpdatedOn">
    <Value>Updated on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPAdd.Title">
    <Value>Add IP to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPAdd.BackToBlacklist">
    <Value>back to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPAdd.SaveButton.Tooltip">
    <Value>Save IP to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPDetails.Title">
    <Value>Edit IP from blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPDetails.BackToBlacklist">
    <Value>back to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPDetails.SaveButton.Tooltip">
    <Value>Save IP to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPDetails.DeleteButton.Tooltip">
    <Value>Delete IP address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPInfo.BannedIP">
    <Value>Banned IP address:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPInfo.BannedIP.Tooltip">
    <Value>Banned IP address or network</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPInfo.Format">
    <Value>format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPInfo.Comment">
    <Value>Comment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPInfo.Comment.Tooltip">
    <Value>Reason why the IP address or network was banned</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPInfo.CreatedOn.Tooltip">
    <Value>Date and time when the IP was banned</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPInfo.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistIPInfo.UpdatedOn.Tooltip">
    <Value>Date and time when the IP was last updated</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkAdd.Title">
    <Value>Add network to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkAdd.BackToBlacklist">
    <Value>back to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkAdd.SaveButton.Tooltip">
    <Value>Save IP to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkDetails.Title">
    <Value>Edit network from blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkDetails.BackToBlacklist">
    <Value>back to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkDetails.SaveButton.Tooltip">
    <Value>Save IP to blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkDetails.DeleteButton.Tooltip">
    <Value>Delete IP address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.BannedIP">
    <Value>Banned IP network:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.BannedIP.Tooltip">
    <Value>Banned IP address or network</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.Format">
    <Value>format:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.OrLeaveEmpty">
    <Value>or leave empty</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.Exception">
    <Value>IP Exception:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.Exception.Tooltip">
    <Value>Excepted IPs in the network</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.Comment">
    <Value>Comment:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.Comment.Tooltip">
    <Value>Reason why the IP address or network was banned</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.CreatedOn.Tooltip">
    <Value>Date and time when the network was banned</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.BlacklistNetworkInfo.UpdatedOn.Tooltip">
    <Value>Date and time when the network was last updated</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypes.Title">
    <Value>Manage Credit Cards</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypes.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypes.AddButton.Tooltip">
    <Value>Add a new credit card type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypes.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypes.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypes.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypes.Edit.Tooltip">
    <Value>Edit credit card type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeAdd.Title">
    <Value>Add a new credit card type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeAdd.BackToCards">
    <Value>back to credit card types list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeAdd.SaveButton.Tooltip">
    <Value>Save credit card type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeDetails.Title">
    <Value>Edit credit card type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeDetails.BackToCards">
    <Value>back to credit card types list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeDetails.SaveButton.Tooltip">
    <Value>Save credit card type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeDetails.DeleteButton.Tooltip">
    <Value>Delete credit card type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.Name.Tooltip">
    <Value>The credit card type name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.SystemKeyword">
    <Value>System Keyword:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.SystemKeyword.Tooltip">
    <Value>The system keyword for this credit card type.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.SystemKeyword.ErrorMessage">
    <Value>System keyword is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.DisplayOrder.Tooltip">
    <Value>Display order for credit card type. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CreditCardTypeInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethods.Title">
    <Value>Payment Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethods.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethods.AddButton.Tooltip">
    <Value>Add a new payment method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethods.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethods.VisibleName">
    <Value>Visible name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethods.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethods.IsActive">
    <Value>Is active</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethods.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethods.Edit.Tooltip">
    <Value>Edit payment method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodAdd.Title">
    <Value>Add a new payment method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodAdd.BackToMethodList">
    <Value>back to payment methods list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodAdd.SaveButton.Tooltip">
    <Value>Save payment method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodDetails.Title">
    <Value>Edit payment method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodDetails.BackToMethodList">
    <Value>back to payment methods list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodDetails.SaveButton.Tooltip">
    <Value>Save payment method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodDetails.DeleteButton.Tooltip">
    <Value>Delete payment method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.Name.Tooltip">
    <Value>Name of payment method.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.VisibleName">
    <Value>Visible name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.VisibleName.Tooltip">
    <Value>The visible name for this payment method. This is what your customer will see when they proceed to checkout.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.VisibleName.ErrorMessage">
    <Value>Visible name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.Description.Tooltip">
    <Value>A description of this payment method.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.ConfigureTemplatePath">
    <Value>Configuration template path:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.ConfigureTemplatePath.Tooltip">
    <Value>The path to the configuration template for this payment method e.g. ''Payment\YourPaymentMethod\ConfigurePaymentMethod.ascx''.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.UserTemplatePath">
    <Value>User template path:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.UserTemplatePath.Tooltip">
    <Value>The path to the user template for this payment method. Used during checkout.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.ClassName">
    <Value>Class name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.ClassName.Tooltip">
    <Value>The fully qualified name of the payment method class.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.ClassName.ErrorMessage">
    <Value>Class name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.SystemKeyword">
    <Value>System keyword:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.SystemKeyword.Tooltip">
    <Value>A system keyword for this payment method.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.Active">
    <Value>Active:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.Active.Tooltip">
    <Value>Determines whether this payment method is active and can be selected by customers during checkout.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.DisplayOrder.Tooltip">
    <Value>The display order for this payment method. 1 represents the first item in the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.PaymentMethodInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.Title">
    <Value>Tax Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.SaveButton.Tooltip">
    <Value>Save tax changes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.PricesIncludeTax">
    <Value>Prices include tax:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.PricesIncludeTax.Tooltip">
    <Value>A value indicating whether entered prices include tax.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.AllowSelectTaxDisplayType">
    <Value>Allow customers to select tax display type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.AllowSelectTaxDisplayType.Tooltip">
    <Value>A value indicating whether customers are allowed to select tax display type.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxDisplayType">
    <Value>Tax display type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxDisplayType.Tooltip">
    <Value>Tax display type.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.DisplayTaxSuffix">
    <Value>Display tax suffix:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.DisplayTaxSuffix.Tooltip">
    <Value>A value indicating whether to display tax suffix (incl tax/excl tax).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.HideZeroTax">
    <Value>Hide zero tax:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.HideZeroTax.Tooltip">
    <Value>A value indicating whether to hide zero tax in order summary.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.HideTaxInOrderSummary">
    <Value>Hide tax in order summary:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.HideTaxInOrderSummary.Tooltip">
    <Value>A value indicating whether to hide tax in order summary when prices are shown tax inclusive.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxBasedOn">
    <Value>Tax based on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxBasedOn.Tooltip">
    <Value>Tax based on.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxDefaultCountry">
    <Value>Default Country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxDefaultCountry.Tooltip">
    <Value>The default country used for tax calculation.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxDefaultCountry.SelectCountry">
    <Value>Select country</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxDefaultState">
    <Value>Default State / Province:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxDefaultState.Tooltip">
    <Value>The default state / province used for tax calculation</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxDefaultZip">
    <Value>Default Zip / Postal code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.TaxDefaultZip.Tooltip">
    <Value>The default zip / postal code used for tax calculation.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.ShippingIsTaxable">
    <Value>Shipping is taxable:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.ShippingIsTaxable.Tooltip">
    <Value>A value indicating whether shipping is taxable.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.ShippingPriceIncludesTax">
    <Value>Shipping price includes tax:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.ShippingPriceIncludesTax.Tooltip">
    <Value>A value indicating whether shipping price includes tax.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.ShippingTaxClass">
    <Value>Shipping tax class:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.ShippingTaxClass.Tooltip">
    <Value>Select tax class used for shipping tax calculation.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.PaymentMethodFeeIsTaxable">
    <Value>Payment method additional fee is taxable:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.PaymentMethodFeeIsTaxable.Tooltip">
    <Value>A value indicating whether payment method additional fee is taxable.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.PaymentMethodFeeIncludesTax">
    <Value>Payment method additional fee includes tax:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.PaymentMethodFeeIncludesTax.Tooltip">
    <Value>A value indicating whether payment method additional fee includes tax.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.PaymentMethodTaxClass">
    <Value>Payment method additional fee tax class:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxSettings.PaymentMethodTaxClass.Tooltip">
    <Value>Select tax class used for payment method additional fee tax calculation.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviders.Title">
    <Value>Tax Providers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviders.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviders.AddNewButton.Tooltip">
    <Value>Add a new tax provider</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviders.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviders.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviders.IsDefault">
    <Value>Is default</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviders.IsDefault.Tooltip">
    <Value>Click to make this tax provider the default</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviders.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviders.Edit.Tooltip">
    <Value>Edit tax provider</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderAdd.Title">
    <Value>Add a new tax provider</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderAdd.BackToProviders">
    <Value>back to tax providers list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderAdd.SaveButton.Tooltip">
    <Value>Save tax provider</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderDetails.Title">
    <Value>Edit tax provider details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderDetails.BackToProviders">
    <Value>back to tax providers list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderDetails.SaveButton.Tooltip">
    <Value>Save tax provider</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderDetails.DeleteButton.Tooltip">
    <Value>Delete tax provider</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.Name.Tooltip">
    <Value>The tax provider name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.Description.Tooltip">
    <Value>A description of the tax provider.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.ConfigureTemplatePath">
    <Value>Configuration template path:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.ConfigureTemplatePath.Tooltip">
    <Value>The path to the configuration template for this provider.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.ClassName">
    <Value>Class name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.ClassName.Tooltip">
    <Value>The fully qualified class name for this tax provider.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.ClassName.ErrorMessage">
    <Value>Class name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.DisplayOrder.Tooltip">
    <Value>The display order of this tax provider. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxProviderInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategories.Title">
    <Value>Tax classes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategories.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategories.AddNewButton.Tooltip">
    <Value>Add a new tax classification</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategories.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategories.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategories.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategories.Edit.Tooltip">
    <Value>Edit tax classification</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryAdd.Title">
    <Value>Add a new tax classification</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryAdd.BackToClasses">
    <Value>back to tax classes list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryAdd.SaveButton.Tooltip">
    <Value>Save tax classification</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryDetails.Title">
    <Value>Edit tax class details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryDetails.BackToClasses">
    <Value>back to tax classes list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryDetails.SaveButton.Tooltip">
    <Value>Save tax classification</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryDetails.DeleteButton.Tooltip">
    <Value>Delete tax classification</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.Name.Tooltip">
    <Value>Name of the tax classification (category).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.DisplayOrder.Tooltip">
    <Value>The display order of the tax classification. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.CreatedOn.Tooltip">
    <Value>Date/Time the tax classification was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.TaxCategoryInfo.UpdatedOn.Tooltip">
    <Value>Date/Time the tax classification was updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.Title">
    <Value>Shipping Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.SaveButton.Tooltip">
    <Value>Save shipping changes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.FreeShippingOverX">
    <Value>Free shipping over ''X'':</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.FreeShippingOverX.Tooltip">
    <Value>Check to enable free shipping for all orders over ''X''. Set the value for X below.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.ValueOfX">
    <Value>Value of ''X''</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.ValueOfX.Tooltip">
    <Value>All orders with a total greater than the value of ''X'' will qualify for free shipping.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.ValueOfX.RequiredErrorMessage">
    <Value>$X is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.ValueOfX.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.OriginCountry">
    <Value>Shipping Origin Country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.OriginCountry.Tooltip">
    <Value>The shipping origin country.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.OriginCountry.SelectCountry">
    <Value>Select country</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.OriginState">
    <Value>Shipping Origin State / Province:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.OriginState.Tooltip">
    <Value>The shipping origin state / province</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.ShippingOriginZip">
    <Value>Shipping Origin Zip / Postal code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingSettings.ShippingOriginZip.Tooltip">
    <Value>The shipping origin zip / postal code.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethods.Title">
    <Value>Shipping Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethods.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethods.AddButton.Tooltip">
    <Value>Add a new shipping method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethods.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethods.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethods.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethods.Edit.Tooltip">
    <Value>Edit shipping method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodAdd.Title">
    <Value>Add a new shipping method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodAdd.BackToMethods">
    <Value>back to shipping methods list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodAdd.SaveButton.Tooltip">
    <Value>Save shipping method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodDetails.Title">
    <Value>Edit shipping method details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodDetails.BackToMethods">
    <Value>back to shipping methods list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodDetails.SaveButton.Tooltip">
    <Value>Save shipping method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodDetails.DeleteButton.Tooltip">
    <Value>Delete shipping method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodInfo.Name.Tooltip">
    <Value>The name of this shipping method. This is the name that the customer will see.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodInfo.Description.Tooltip">
    <Value>A description of the shipping method. This is the description that the customer will see.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodInfo.DisplayOrder.Tooltip">
    <Value>The display order of the shipping method. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingMethodInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethods.Title">
    <Value>Shipping Rate Computation Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethods.AddButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethods.AddButton.Tooltip">
    <Value>Add a new shipping rate computation method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethods.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethods.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethods.IsDefault">
    <Value>Is default</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethods.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethods.Edit.Tooltip">
    <Value>Edit shipping rate computation method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodAdd.Title">
    <Value>Add a new shipping rate computation method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodAdd.BackToMethods">
    <Value>back to shipping rate computation methods list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodAdd.SaveButton.Tooltip">
    <Value>Save shipping rate computation method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodDetails.Title">
    <Value>Edit shipping rate computation method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodDetails.BackToMethods">
    <Value>back to shipping rate computation methods list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodDetails.SaveButton.Tooltip">
    <Value>Save shipping rate computation method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodDetails.DeleteButton.Tooltip">
    <Value>Delete shipping rate computation method</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.Name.Tooltip">
    <Value>The name of this shipping rate computation method.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.Description.Tooltip">
    <Value>A description of this shipping rate computation method.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.ConfigureTemplatePath">
    <Value>Configuration template path:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.ConfigureTemplatePath.Tooltip">
    <Value>The path to the configuration template for this shipping rate computation method e.g. Shipping\MyShippingCalculator\ConfigureShipping.ascx.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.ClassName">
    <Value>Class name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.ClassName.Tooltip">
    <Value>The fully qualified class name for this shipping rate computation method.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.ClassName.ErrorMessage">
    <Value>Class name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.DisplayOrder.Tooltip">
    <Value>The display order for this shipping rate computation method. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.ShippingRateComputationMethodInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.Title">
    <Value>Countries</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.AddNewButton.Tooltip">
    <Value>Add a new country</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.AllowsRegistration">
    <Value>Allows registration</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.AllowsBilling">
    <Value>Allows billing</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.AllowsShipping">
    <Value>Allows shipping</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.TwoLetterISOCode">
    <Value>Two letter ISO Code</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.ThreeLetterISOCode">
    <Value>Three letter ISO Code</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.NumericISOCode">
    <Value>Numeric ISO code</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Countries.Edit.Tooltip">
    <Value>Edit country</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryAdd.Title">
    <Value>Add a new country</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryAdd.BackToCountries">
    <Value>back to countries list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryAdd.SaveButton.Tooltip">
    <Value>Save country</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryDetails.Title">
    <Value>Edit country details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryDetails.BackToCountries">
    <Value>back to countries list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryDetails.SaveButton.Tooltip">
    <Value>Save country</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryDetails.DeleteButton.Tooltip">
    <Value>Delete country</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.Name.Tooltip">
    <Value>The name of the country.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.Name.ErrorMessage">
    <Value>Country name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.AllowsRegistration">
    <Value>Allows registration:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.AllowsRegistration.Tooltip">
    <Value>Allow customers located in this country to register for a store account.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.AllowsBilling">
    <Value>Allows billing:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.AllowsBilling.Tooltip">
    <Value>Allow billing to customers located in this country.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.AllowsShipping">
    <Value>Allows shipping:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.AllowsShipping.Tooltip">
    <Value>Allow shipping to this country.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.TwoLetterISOCode">
    <Value>Two letter ISO code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.TwoLetterISOCode.Tooltip">
    <Value>The two letter ISO code for this country. For a complete list of ISO codes go to: http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.TwoLetterISOCode.ErrorMessage">
    <Value>Two letter ISO code is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.ThreeLetterISOCode">
    <Value>Three letter ISO code:"</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.ThreeLetterISOCode.Tooltip">
    <Value>The three letter ISO code for this country. For a complete list of ISO codes go to: http://en.wikipedia.org/wiki/ISO_3166-1_alpha-3</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.ThreeLetterISOCode.ErrorMessage">
    <Value>Three letter ISO code is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.NumbericISOCode">
    <Value>Numeric ISO code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.NumbericISOCode.Tooltip">
    <Value>The numeric ISO code for this country. For a complete list of ISO codes go to: http://en.wikipedia.org/wiki/ISO_3166-1_numeric</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.NumbericISOCode.RequiredErrorMessage">
    <Value>Numeric ISO code is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.NumbericISOCode.RangeErrorMessage">
    <Value>The value must be from 1 to 9999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.Published">
    <Value>Published:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.Published.Tooltip">
    <Value>Determines whether this country is published (visible for new account registrations and creation of shipping/billing addresses).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.DisplayOrder.Tooltip">
    <Value>The display order for this country. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CountryInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.Title">
    <Value>States / Provinces</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.AddNewButton.Tooltip">
    <Value>Add a new state/province</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.SelectCountry">
    <Value>Select country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.SelectCountry.Tooltip">
    <Value>Select a country to view it''s states/provinces.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.SelectCountry.Tooltip2">
    <Value>Select  a country to view it''s states/provinces</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.Abbreviation">
    <Value>Abbreviation</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinces.Edit.Tooltip">
    <Value>Edit state/province</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceAdd.Title">
    <Value>Add a new state/province</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceAdd.BackToStates">
    <Value>back to state/province list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceAdd.SaveButton.Tooltip">
    <Value>Save state/province</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceDetails.Title">
    <Value>Edit state/province details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceDetails.BackToStates">
    <Value>back to state/province list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceDetails.SaveButton.Tooltip">
    <Value>Save state/province</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceDetails.DeleteButton.Tooltip">
    <Value>Delete state/province</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.Country">
    <Value>Country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.Country.Tooltip">
    <Value>The name of the country to which this state/province belongs to.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.Name.Tooltip">
    <Value>The name of the state/province.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.Abbreviation">
    <Value>Abbreviation:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.Abbreviation.Tooltip">
    <Value>An abbreviation for this state/province.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.DisplayOrder.Tooltip">
    <Value>The display order of this state/province. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.StateProvinceInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.Title">
    <Value>Languages</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.AddNewButton.Tooltip">
    <Value>Add a new language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.LanguageCulture">
    <Value>Language culture</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.ViewResources">
    <Value>View string resources</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.ViewResources.Tooltip">
    <Value>View language string resources</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Languages.Edit.Tooltip">
    <Value>Edit language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageAdd.Title">
    <Value>Add a new language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageAdd.BackToLanguages">
    <Value>back to language list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageAdd.SaveButton.Tooltip">
    <Value>Save language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageDetails.Title">
    <Value>Edit language details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageDetails.BackToLanguages">
    <Value>back to language list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageDetails.SaveButton.Tooltip">
    <Value>Save language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageDetails.DeleteButton.Tooltip">
    <Value>Delete language</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.Name.Tooltip">
    <Value>The name of the language.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.LanguageCulture">
    <Value>Language culture:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.LanguageCulture.Tooltip">
    <Value>The language specific culture code.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.Published">
    <Value>Published:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.Published.Tooltip">
    <Value>Determines whether this language is published and can therefore be selected by visitors to your store.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.DisplayOrder.Tooltip">
    <Value>The display order of this language. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.Import">
    <Value>Import resources from XML:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.Import.Tooltip">
    <Value>Import resource strings from an XML file. You can download XML resource files from the nopCommerce website.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.ImportButton">
    <Value>Import</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.ImportButton.Tooltip">
    <Value>Import resource strings from file</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LanguageInfo.ResourcesImported">
    <Value>{0} resources have been successfully imported</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.Title">
    <Value>Currencies</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.GetRateButton.Text">
    <Value>Get live rates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.GetRateButton.Tooltip">
    <Value>Get live currency rates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.AddNewButton.Tooltip">
    <Value>Add a new currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyCode">
    <Value>Currency code</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.DisplayLocale">
    <Value>Display locale</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.Rate">
    <Value>Rate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.Published">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.DisplayOrder">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.PrimaryExchangeRateCurrency">
    <Value>Primary exchange rate currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.PrimaryExchangeRateCurrency.Tooltip">
    <Value>Set as primary exchange rate currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.PrimaryStoreCurrency">
    <Value>Primary store currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.PrimaryStoreCurrency.Tooltip">
    <Value>Set as primary store currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.Edit.Tooltip">
    <Value>Edit currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.LiveCurrencyRates">
    <Value>Live currency rates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.LiveRates.CurrencyCode">
    <Value>Currency code</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.LiveRates.Rate">
    <Value>Rate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.LiveRates.Rate.RequiredErrorMessage">
    <Value>Rate is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.LiveRates.Rate.RangeErrorMessage">
    <Value>The value must be from 0 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.LiveRates.ApplyRate">
    <Value>Apply rate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.LiveRates.ApplyRateButton">
    <Value>Apply rate</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyAdd.Title">
    <Value>Add a new currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyAdd.BackToCurrencies">
    <Value>back to currency list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyAdd.SaveButton.Tooltip">
    <Value>Save currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyDetails.Title">
    <Value>Edit currency details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyDetails.BackToCurrencies">
    <Value>back to currency list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyDetails.SaveButton.Tooltip">
    <Value>Save currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Currencies.CurrencyDetails.DeleteButton.Tooltip">
    <Value>Delete currency</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.Name.Tooltip">
    <Value>The name of the currency.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.CurrencyCode">
    <Value>Currency code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.CurrencyCode.Tooltip">
    <Value>The currency code. For a list of currency codes, go to: http://www.iso.org/iso/support/currency_codes_list-1.htm</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.CurrencyCode.ErrorMessage">
    <Value>Currency code is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.Rate">
    <Value>Rate to primary exchange currency:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.Rate.Tooltip">
    <Value>The exchange rate against the primary exchange rate currency.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.Rate.RequiredErrorMessage">
    <Value>Rate is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.Rate.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.DisplayLocale">
    <Value>Display locale:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.DisplayLocale.Tooltip">
    <Value>The display locale for currency values.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.CustomFormatting">
    <Value>Custom formatting:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.CustomFormatting.Tooltip">
    <Value>Custom formatting to be applied to the currency values.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.Published">
    <Value>Published:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.Published.Tooltip">
    <Value>Determines whether the currency is published.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.DisplayOrder">
    <Value>Display order:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.DisplayOrder.Tooltip">
    <Value>The display order of the currency. 1 represents the top of the list.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.DisplayOrder.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.DisplayOrder.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.CreatedOn.Tooltip">
    <Value>The date/time the currency was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.UpdateOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.CurrencyInfo.UpdateOn.Tooltip">
    <Value>The date/time the currency was updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Warehouses.Title">
    <Value>Warehouses</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Warehouses.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Warehouses.AddNewButton.Tooltip">
    <Value>Add a new warehouse</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Warehouses.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Warehouses.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Warehouses.Edit.Tooltip">
    <Value>Edit warehouse</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseAdd.Title">
    <Value>Add a new warehouse</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseAdd.BackToWarehouses">
    <Value>back to warehouse list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseAdd.SaveButton.Tooltip">
    <Value>Save warehouse</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseDetails.Title">
    <Value>Edit warehouse details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseDetails.BackToWarehouses">
    <Value>back to warehouse list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseDetails.SaveButton.Tooltip">
    <Value>Save warehouse</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseDetails.DeleteButton.Tooltip">
    <Value>Delete warehouse</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Name.Tooltip">
    <Value>The name of the warehouse.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Phone">
    <Value>Phone number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Phone.Tooltip">
    <Value>The phone number of the warehouse.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Email">
    <Value>Email:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Email.Tooltip">
    <Value>The email address of the warehouse.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Fax">
    <Value>Fax number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Fax.Tooltip">
    <Value>The fax number of the warehouse.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Address1">
    <Value>Address 1:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Address1.Tooltip">
    <Value>Address line 1 of the warehouse address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Address2">
    <Value>Address 2:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Address2.Tooltip">
    <Value>Address line 2 of the warehouse address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.City">
    <Value>City:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.City.Tooltip">
    <Value>Address city.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.State">
    <Value>State / Province:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.State.Tooltip">
    <Value>Address State / Province.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Zip">
    <Value>Zip / postal code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Zip.Tooltip">
    <Value>Address Zip / Postal Code.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Country">
    <Value>Country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.Country.Tooltip">
    <Value>Address country.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.CreatedOn.Tooltip">
    <Value>The date /time the warehouse was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.UpdatedOn">
    <Value>Updated on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.WarehouseInfo.UpdatedOn.Tooltip">
    <Value>The date / time the warehouse was updated.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Settings.Title">
    <Value>All Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Settings.AddNewButton.Text">
    <Value>Add new</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Settings.AddNewButton.Tooltip">
    <Value>Add a new setting</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Settings.Name">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Settings.Value">
    <Value>Value</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Settings.Edit">
    <Value>Edit</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Settings.Edit.Tooltip">
    <Value>Edit setting</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingAdd.Title">
    <Value>Add a new setting</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingAdd.BackToSettings">
    <Value>back to settings list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingAdd.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingAdd.SaveButton.Tooltip">
    <Value>Save setting</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingDetails.Title">
    <Value>Edit setting details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingDetails.BackToSettings">
    <Value>back to settings list</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingDetails.SaveButton.Tooltip">
    <Value>Save setting</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingDetails.DeleteButton.Tooltip">
    <Value>Delete setting</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingInfo.Name">
    <Value>Name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingInfo.Name.Tooltip">
    <Value>The name of the setting.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingInfo.Name.ErrorMessage">
    <Value>Name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingInfo.Value">
    <Value>Value:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingInfo.Value.Tooltip">
    <Value>The value for this setting.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingInfo.Description">
    <Value>Description:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.SettingInfo.Description.Tooltip">
    <Value>A description of this setting.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.Title">
    <Value>System Log</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.ClearButton.Text">
    <Value>Clear Log</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.ClearButton.Tooltip">
    <Value>Clear system log</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.LogType">
    <Value>Log type</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.Customer">
    <Value>Customer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.Customer.Guest">
    <Value>Guest</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.CreatedOn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.Message">
    <Value>Message</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.Details">
    <Value>Details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.Details.Tooltip">
    <Value>View log details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.Delete">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Logs.DeleteButton.Tooltip">
    <Value>Delete log entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.Title">
    <Value>View log entry details</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.BackToLog">
    <Value>back to system log</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.DeleteButton.Tooltip">
    <Value>Delete log entry</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.LogType">
    <Value>Log type:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.LogType.Tooltip">
    <Value>The type of log entry.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.Severity">
    <Value>Severity:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.Severity.Tooltip">
    <Value>The severity of the log entry.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.Message">
    <Value>Message:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.Message.Tooltip">
    <Value>The log entry message.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.Exception">
    <Value>Exception:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.Exception.Tooltip">
    <Value>The exception details for the log entry.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.IPAddress">
    <Value>IP address:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.IPAddress.Tooltip">
    <Value>IP address of the machine that caused the exception.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.Customer">
    <Value>Customer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.Customer.Tooltip">
    <Value>Name of the customer who caused the exception.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.PageURL">
    <Value>Page URL:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.PageURL.Tooltip">
    <Value>Originating page of exception</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.LogDetails.CreatedOn.Tooltip">
    <Value>Date/Time the log entry was created.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.Title">
    <Value>Message Queue</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.LoadButton.Text">
    <Value>Load</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.LoadButton.Tooltip">
    <Value>Load messages in queue</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.StartDate">
    <Value>Start date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.StartDate.Tooltip">
    <Value>The start date for the search in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.StartDate.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.EndDate">
    <Value>End date:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.EndDate.Tooltip">
    <Value>The end date for the search in Coordinated Universal Time (UTC).</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.EndDate.ShowCalendar">
    <Value>Click to show calendar</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.From">
    <Value>From address:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.From.Tooltip">
    <Value>From address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.ToEmail">
    <Value>To address:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.ToEmail.Tooltip">
    <Value>To address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.LoadNotSent">
    <Value>Load not sent emails only:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.LoadNotSent.Tooltip">
    <Value>Only load emails into queue that have not yet been sent.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.MaxSendTries">
    <Value>Maximum send attempts:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.MaxSendTries.Tooltip">
    <Value>The maximum number of attempts to send a message.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.MaxSendTries.RequiredErrorMessage">
    <Value>Enter maximum send tries</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.MaxSendTries.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.GoDirectly">
    <Value>Go directly to email #:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.GoDirectly.Tooltip">
    <Value>Go directly to email #</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.EmailID.Required">
    <Value>Email number is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.GoButton.Text">
    <Value>Go</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.GoButton.Tooltip">
    <Value>Go directly to email #</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.QueuedEmailIDColumn">
    <Value>Queued email ID</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.PriorityColumn">
    <Value>Priority</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.FromColumn">
    <Value>From</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.ToColumn">
    <Value>To</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.ViewColumn">
    <Value>View</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.CreatedOnColumn">
    <Value>Created on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.SentOnColumn">
    <Value>Sent on</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueue.NoEmailsFound">
    <Value>No queued emails found</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Title">
    <Value>Edit message queue item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.BackToEmails">
    <Value>back to message queue</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.RequeueButton.Text">
    <Value>Requeue</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.RequeueButton.Tooltip">
    <Value>Requeue email</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.SaveButton.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.SaveButton.Tooltip">
    <Value>Save message queue item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.DeleteButton.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.DeleteButton.Tooltip">
    <Value>Delete message queue item</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Priority">
    <Value>Message priority:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Priority.Tooltip">
    <Value>The priority of the message.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Priority.RequiredErrorMessage">
    <Value>Enter priority</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Priority.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.From">
    <Value>From:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.From.Tooltip">
    <Value>From address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.From.ErrorMessage">
    <Value>From is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.FromName">
    <Value>From name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.FromName.Tooltip">
    <Value>From name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.To">
    <Value>To:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.To.Tooltip">
    <Value>To address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.To.ErrorMessage">
    <Value>To is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.ToName">
    <Value>To name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.ToName.Tooltip">
    <Value>To name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.CC">
    <Value>Cc:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.CC.Tooltip">
    <Value>Cc address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Bcc">
    <Value>Bcc:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Bcc.Tooltip">
    <Value>Bcc address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Subject">
    <Value>Subject:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Subject.Tooltip">
    <Value>Message subject.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Body">
    <Value>Body:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.Body.Tooltip">
    <Value>Message body.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.CreatedOn">
    <Value>Created on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.CreatedOn.Tooltip">
    <Value>Date/Time message added to queue.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.SendTries">
    <Value>Send attempts</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.SendTries.Tooltip">
    <Value>The number of times to attempt to send this message.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.SendTries.RequiredErrorMessage">
    <Value>Enter send tries</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.SendTries.RangeErrorMessage">
    <Value>The value must be from 0 to 999999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.SendOn">
    <Value>Sent on:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.MessageQueueDetails.SendOn.Tooltip">
    <Value>The date/time message was sent.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressAdd.Title">
    <Value>Add a new address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressAdd.BackToCustomer">
    <Value>(back to customer details)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressAdd.SaveAddress.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressAdd.SaveAddress.Tooltip">
    <Value>Save address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressDetails.Title">
    <Value>Edit customer address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressDetails.BackToCustomer">
    <Value>(back to customer details)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressDetails.SaveAddress.Text">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressDetails.SaveAddress.Tooltip">
    <Value>Save address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressDetails.DeleteAddress.Text">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressDetails.DeleteAddress.Tooltip">
    <Value>Delete address</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Customer">
    <Value>Customer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Customer.Tooltip">
    <Value>The customer you are creating an address for.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.FirstName">
    <Value>First name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.FirstName.Tooltip">
    <Value>The contact first name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.LastName">
    <Value>Last name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.LastName.Tooltip">
    <Value>The contact last name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.LastName.ErrorMessage">
    <Value>Last name is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Phone">
    <Value>Phone number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Phone.Tooltip">
    <Value>Contact phone number.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Email">
    <Value>Email:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Email.Tooltip">
    <Value>The contact email address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Fax">
    <Value>Fax number:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Fax.Tooltip">
    <Value>Contact fax number.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Company">
    <Value>Company:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Company.Tooltip">
    <Value>Contact company name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Address1">
    <Value>Address 1:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Address1.Tooltip">
    <Value>Line 1 of the address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.ErrorMessage">
    <Value>Address is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Address2">
    <Value>Address 2:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Address2.Tooltip">
    <Value>Line 2 of the address.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.City">
    <Value>City:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.City.Tooltip">
    <Value>The address city.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.City.ErrorMessage">
    <Value>City is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.State">
    <Value>State / Province:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.State.Tooltip">
    <Value>The address state / province</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Zip">
    <Value>Zip / Postal code:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Zip.Tooltip">
    <Value>The address zip / postal code.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Zip.ErrorMessage">
    <Value>Zip / Postal code is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Country">
    <Value>Country:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddressInfo.Country.Tooltip">
    <Value>The address country.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.State.Other">
    <Value>Other (Non us)</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.Editor.Simple">
    <Value>Simple</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.Editor.BBCodeEditor">
    <Value>BBCode Editor</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.Editor.HTMLEditor">
    <Value>HTML Editor</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.All">
    <Value>All</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.Wait...">
    <Value>Wait .....</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.AreYouSure">
    <Value>Are you sure?</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.OK">
    <Value>OK</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.Cancel">
    <Value>Cancel</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.Yes">
    <Value>Yes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Common.No">
    <Value>No</Value>
  </LocaleResource>
  <LocaleResource Name="Account.PasswordRecovery.ReEnterNewPassword">
    <Value>Reenter new password</Value>
  </LocaleResource>
  <LocaleResource Name="Forum.Topics.GotoPostPager">
    <Value>[Go to page: {0}]</Value>
  </LocaleResource>
  <LocaleResource Name="Reports.BestSellingProducts">
    <Value>Best Selling Products</Value>
  </LocaleResource>
  <LocaleResource Name="PageTitle.PrivateMessages">
    <Value>Private Messages</Value>
  </LocaleResource>
  <LocaleResource Name="PageTitle.SendPM">
    <Value>Post a private message</Value>
  </LocaleResource>
  <LocaleResource Name="PageTitle.ViewPM">
    <Value>View the private message</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.PrivateMessages">
    <Value>Private Messages</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Inbox">
    <Value>Inbox</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.SentItems">
    <Value>Sent Items</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.PM">
    <Value>PM</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Inbox.FromColumn">
    <Value>From</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Inbox.SubjectColumn">
    <Value>Subject</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Inbox.DateColumn">
    <Value>Date</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Inbox.DeleteSelected">
    <Value>Delete selected</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Sent.ToColumn">
    <Value>To</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Sent.SubjectColumn">
    <Value>Subject</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Sent.DateColumn">
    <Value>Date</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Sent.DeleteSelected">
    <Value>Delete selected</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Send.PostMessage">
    <Value>Post a Private Message</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Send.To">
    <Value>To:</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Send.Subject">
    <Value>Subject:</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Send.Subject.Required">
    <Value>Subject is required.</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Send.Message">
    <Value>Message:</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Send.SendButton">
    <Value>Send</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.Send.CancelButton">
    <Value>Cancel</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.SubjectCannotBeEmpty">
    <Value>Subject cannot be empty</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.MessageCannotBeEmpty">
    <Value>Message cannot be empty</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.View.ViewMessage">
    <Value>View Message</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.View.From">
    <Value>From:</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.View.To">
    <Value>To:</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.View.Subject">
    <Value>Subject:</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.View.Message">
    <Value>Message:</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.View.ReplyButton">
    <Value>Reply</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.View.DeleteButton">
    <Value>Delete</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.View.BackButton">
    <Value>Back to messages</Value>
  </LocaleResource>
  <LocaleResource Name="PrivateMessages.TotalUnread">
    <Value>({0} Unread)</Value>
  </LocaleResource>
  <LocaleResource Name="Common.SpecificationFilter">
    <Value>Filter by attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Common.SpecificationFilterRemove">
    <Value>Remove Filter</Value>
  </LocaleResource>
  <LocaleResource Name="Common.CurrentlyFilteredBy">
    <Value>Currently shopping by:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.Title">
    <Value>Add related product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.ProductName">
    <Value>Product name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.ProductName.Tooltip">
    <Value>A product name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.Category">
    <Value>Category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.Category.Tooltip">
    <Value>Search by a specific category.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.Manufacturer">
    <Value>Manufacturer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.Manufacturer.Tooltip">
    <Value>Search by a specific manufacturer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.SearchButton.Text">
    <Value>Search</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.SearchButton.Tooltip">
    <Value>Search for products based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.ProductColumn">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.ProductColumn.Tooltip">
    <Value>Mark this product as a related product</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.PublishedColumn">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.DisplayOrderColumn">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.DisplayOrderColumn.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.DisplayOrderColumn.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.NoProductsFound">
    <Value>No products found</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.SaveColumn">
    <Value>Save</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddRelatedProduct.SaveColumn.Tooltip">
    <Value>Save related products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.Title">
    <Value>Add product to category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.ProductName">
    <Value>Product name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.ProductName.Tooltip">
    <Value>A product name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.Category">
    <Value>Category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.Category.Tooltip">
    <Value>Search by a specific category.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.Manufacturer">
    <Value>Manufacturer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.Manufacturer.Tooltip">
    <Value>Search by a specific manufacturer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.SearchButton.Text">
    <Value>Search</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.SearchButton.Tooltip">
    <Value>Search for products based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.ProductColumn">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.ProductColumn.Tooltip">
    <Value>Add this product to the category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.PublishedColumn">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.DisplayOrderColumn">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.DisplayOrderColumn.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.DisplayOrderColumn.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.NoProductsFound">
    <Value>No products found</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.SaveColumn">
    <Value>Add to category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddCategoryProduct.SaveColumn.Tooltip">
    <Value>Add products to category</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.Title">
    <Value>Add product to manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.ProductName">
    <Value>Product name:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.ProductName.Tooltip">
    <Value>A product name.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.Category">
    <Value>Category:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.Category.Tooltip">
    <Value>Search by a specific category.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.Manufacturer">
    <Value>Manufacturer:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.Manufacturer.Tooltip">
    <Value>Search by a specific manufacturer.</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.SearchButton.Text">
    <Value>Search</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.SearchButton.Tooltip">
    <Value>Search for products based on the criteria below</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.ProductColumn">
    <Value>Name</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.ProductColumn.Tooltip">
    <Value>Add this product to the manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.PublishedColumn">
    <Value>Published</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.DisplayOrderColumn">
    <Value>Display order</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.DisplayOrderColumn.RequiredErrorMessage">
    <Value>Display order is required</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.DisplayOrderColumn.RangeErrorMessage">
    <Value>The value must be from -99999 to 99999</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.NoProductsFound">
    <Value>No products found</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.SaveColumn">
    <Value>Add to manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.AddManufacturerProduct.SaveColumn.Tooltip">
    <Value>Add products to manufacturer</Value>
  </LocaleResource>
  <LocaleResource Name="Wishlist.ItemYouSave">
    <Value>You save:</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.HomeTitle">
    <Value>Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.HomeDescription">
    <Value>Administration Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.DashboardTitle">
    <Value>Dashboard</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.DashboardDescription">
    <Value>View the dashboard</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CatalogTitle">
    <Value>Catalog</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CatalogDescription">
    <Value>Catalog Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CategoriesTitle">
    <Value>Categories</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CategoriesDescription">
    <Value>Manage Product Categories</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductsHomeTitle">
    <Value>Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductsHomeDescription">
    <Value>Products Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductsTitle">
    <Value>Manage Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductsDescription">
    <Value>Manage Products</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductReviewsTitle">
    <Value>Manage Reviews</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductReviewsDescription">
    <Value>Manage Product Reviews</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LowStockReportTitle">
    <Value>Low Stock Report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LowStockReportDescription">
    <Value>View Low Product Stock Report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.AttributesHomeTitle">
    <Value>Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.AttributesHomeDescription">
    <Value>Attributes Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductAttributesTitle">
    <Value>Product Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductAttributesDescription">
    <Value>Manage Product Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SpecificationAttributesTitle">
    <Value>Product Specifications</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SpecificationAttributesDescription">
    <Value>Manage Specification Attributes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ManufacturersTitle">
    <Value>Manufacturers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ManufacturersDescription">
    <Value>Manage Product Manufacturers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SalesHomeTitle">
    <Value>Sales</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SalesHomeDescription">
    <Value>Sales Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.OrdersTitle">
    <Value>Orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.OrdersDescription">
    <Value>Manage Customer Orders</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SalesReportTitle">
    <Value>Sales Report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SalesReportDescription">
    <Value>View Sales Report</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CustomersHomeTitle">
    <Value>Customers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CustomersHomeDescription">
    <Value>Customers Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CustomersTitle">
    <Value>Manage Customers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CustomersDescription">
    <Value>Manage Customers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CustomerRolesTitle">
    <Value>Customer Roles</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CustomerRolesDescription">
    <Value>Manage Customer Roles</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PromotionsHomeTitle">
    <Value>Promotions</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PromotionsHomeDescription">
    <Value>Promotions Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.AffiliatesTitle">
    <Value>Affiliates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.AffiliatesDescription">
    <Value>Manage Affiliates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CampaignsTitle">
    <Value>Campaigns</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CampaignsDescription">
    <Value>Manage Campaigns</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.DiscountsTitle">
    <Value>Discounts</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.DiscountsDescription">
    <Value>Manage Discounts</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PricelistTitle">
    <Value>Price Lists</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PricelistDescription">
    <Value>Manage Price Lists</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PromotionProvidersHomeTitle">
    <Value>Providers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PromotionProvidersHomeDescription">
    <Value>Promotion Providers Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.FroogleTitle">
    <Value>Froogle</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.FroogleDescription">
    <Value>Manage Froogle Provider</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ContentManagementHomeTitle">
    <Value>Content&#160;Management</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ContentManagementHomeDescription">
    <Value>Content Management Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PollsTitle">
    <Value>Polls</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PollsDescription">
    <Value>Manage Polls</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.NewsHomeTitle">
    <Value>News</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.NewsHomeDescription">
    <Value>News Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.NewsSettingsTitle">
    <Value>News Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.NewsSettingsDescription">
    <Value>Manage News Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.NewsTitle">
    <Value>Manage News</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.NewsDescription">
    <Value>Manage News</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.NewsCommentsTitle">
    <Value>News comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.NewsCommentsDescription">
    <Value>Manage News Comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlogHomeTitle">
    <Value>Blog</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlogHomeDescription">
    <Value>Blog Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlogSettingsTitle">
    <Value>Blog Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlogSettingsDescription">
    <Value>Manage Blog Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlogTitle">
    <Value>Manage Blog</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlogDescription">
    <Value>Manage Blog"</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlogCommentsTitle">
    <Value>Blog comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlogCommentsDescription">
    <Value>Manage Blog Comments</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TopicsTitle">
    <Value>Topics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TopicsDescription">
    <Value>Manage Topics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ForumsHomeTitle">
    <Value>Forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ForumsHomeDescription">
    <Value>Forums Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ForumsSettingsTitle">
    <Value>Forums Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ForumsSettingsDescription">
    <Value>Manage Forums Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ForumsTitle">
    <Value>Forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ForumsDescription">
    <Value>Manage Forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TemplatesHomeTitle">
    <Value>Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TemplatesHomeDescription">
    <Value>Templates Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductTemplatesTitle">
    <Value>Product Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ProductTemplatesDescription">
    <Value>Manage Product Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CategoryTemplatesTitle">
    <Value>Category Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CategoryTemplatesDescription">
    <Value>Manage Category Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ManufacturerTemplatesTitle">
    <Value>Manufacturer Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ManufacturerTemplatesDescription">
    <Value>Manage Manufacturer Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.MessageTemplatesTitle">
    <Value>Message templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.MessageTemplatesDescription">
    <Value>Manage Message Templates</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LocaleStringResourcesTitle">
    <Value>Localization</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LocaleStringResourcesDescription">
    <Value>Manage Store Localization</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ConfigurationTitle">
    <Value>Configuration</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ConfigurationDescription">
    <Value>Configuration Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.GlobalSettingsTitle">
    <Value>Global Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.GlobalSettingsDescription">
    <Value>Configure Global Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlacklistTitle">
    <Value>Blacklist</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.BlacklistDescription">
    <Value>Manage banned IP addresses and networks</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PaymentSettingsHomeTitle">
    <Value>Payment</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PaymentSettingsHomeDescription">
    <Value>Payment Settings Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CreditCardTypesTitle">
    <Value>Credit Cards</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CreditCardTypesDescription">
    <Value>Manage Credit Cards</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PaymentMethodsTitle">
    <Value>Payment Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.PaymentMethodsDescription">
    <Value>Manage Payment Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TaxSettingsHomeTitle">
    <Value>Tax</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TaxSettingsHomeDescription">
    <Value>Tax Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TaxSettingsTitle">
    <Value>Tax Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TaxSettingsDescription">
    <Value>Manage Tax Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TaxProvidersTitle">
    <Value>Tax Providers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TaxProvidersDescription">
    <Value>Manage Tax Providers</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TaxCategoriesTitle">
    <Value>Tax Classes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.TaxCategoriesDescription">
    <Value>Manage Product Tax Classes</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ShippingSettingsHomeTitle">
    <Value>Shipping</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ShippingSettingsHomeDescription">
    <Value>Shipping Settings Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ShippingSettingsTitle">
    <Value>Shipping Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ShippingSettingsDescription">
    <Value>Manage Shipping Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ShippingMethodsTitle">
    <Value>Shipping Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ShippingMethodsDescription">
    <Value>Manage Shipping Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ShippingRateComputationMethodsTitle">
    <Value>Shipping Rate Computation</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.ShippingRateComputationMethodsDescription">
    <Value>Manage Shipping Rate Computation Methods</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LocationSettingsHomeTitle">
    <Value>Location</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LocationSettingsHomeDescription">
    <Value>Location Settings Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CountriesTitle">
    <Value>Countries</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CountriesDescription">
    <Value>Manage Countries</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.StateProvincesTitle">
    <Value>States/Provinces</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.StateProvincesDescription">
    <Value>Manage States / Provinces</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LanguagesTitle">
    <Value>Languages</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LanguagesDescription">
    <Value>Configure Language Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CurrenciesTitle">
    <Value>Currencies</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CurrenciesDescription">
    <Value>Manage Store Currencies</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.WarehousesTitle">
    <Value>Warehouses</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.WarehousesDescription">
    <Value>Manage Warehouses</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SettingsTitle">
    <Value>All Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SettingsDescription">
    <Value>Manage All Settings</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SystemHomeTitle">
    <Value>System</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.SystemHomeDescription">
    <Value>System Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LogsTitle">
    <Value>Log</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.LogsDescription">
    <Value>View Application Log</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.MessageQueueTitle">
    <Value>Message queue</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.MessageQueueDescription">
    <Value>Manage Message Queue</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.HelpHomeTitle">
    <Value>Help</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.HelpHomeDescription">
    <Value>Help Home</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.HelpTopicsTitle">
    <Value>Help Topics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.HelpTopicsDescription">
    <Value>View Help Topics</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CommunityForumsTitle">
    <Value>Community Forums</Value>
  </LocaleResource>
  <LocaleResource Name="Admin.Sitemap.CommunityForumsDescription">
    <Value>Visit nopCommerce Community Forum</Value>
  </LocaleResource>
</Language>
'

CREATE TABLE #LocaleStringResourceTmp
	(
		[LanguageID] [int] NOT NULL,
		[ResourceName] [nvarchar](200) NOT NULL,
		[ResourceValue] [nvarchar](max) NOT NULL
	)


INSERT INTO #LocaleStringResourceTmp (LanguageID, ResourceName, ResourceValue)
SELECT	@resources.value('(/Language/@LanguageID)[1]', 'int'), nref.value('@Name', 'nvarchar(200)'), nref.value('Value[1]', 'nvarchar(MAX)')
FROM	@resources.nodes('//Language/LocaleResource') AS R(nref)

DECLARE @LanguageID int
DECLARE @ResourceName nvarchar(200)
DECLARE @ResourceValue nvarchar(MAX)
DECLARE cur_localeresource CURSOR FOR
SELECT LanguageID, ResourceName, ResourceValue
FROM #LocaleStringResourceTmp
OPEN cur_localeresource
FETCH NEXT FROM cur_localeresource INTO @LanguageID, @ResourceName, @ResourceValue
WHILE @@FETCH_STATUS = 0
BEGIN
	IF (EXISTS (SELECT 1 FROM Nop_LocaleStringResource WHERE LanguageID=@LanguageID AND ResourceName=@ResourceName))
	BEGIN
		UPDATE [Nop_LocaleStringResource]
		SET [ResourceValue]=@ResourceValue
		WHERE LanguageID=@LanguageID AND ResourceName=@ResourceName
	END
	ELSE 
	BEGIN
		INSERT INTO [Nop_LocaleStringResource]
		(
			[LanguageID],
			[ResourceName],
			[ResourceValue]
		)
		VALUES
		(
			@LanguageID,
			@ResourceName,
			@ResourceValue
		)
	END
	
	
	FETCH NEXT FROM cur_localeresource INTO @LanguageID, @ResourceName, @ResourceValue
	END
CLOSE cur_localeresource
DEALLOCATE cur_localeresource

DROP TABLE #LocaleStringResourceTmp
GO







-- Determines whether to allow navigation only for registered users
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Common.AllowNavigationOnlyRegisteredCustomers')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Common.AllowNavigationOnlyRegisteredCustomers', N'false', N'Determines whether to allow navigation only for registered users')
END
GO



-- Determines whether to hide advertisements on admin dashboard
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Common.HideAdvertisementsOnAdminArea')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Common.HideAdvertisementsOnAdminArea', N'false', N'Determines whether to hide advertisements on admin dashboard')
END
GO

-- "List of products purchased by other customers who purchased the above" option
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.ListOfProductsAlsoPurchasedEnabled')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.ListOfProductsAlsoPurchasedEnabled', N'True', N'Determines whether "List of products purchased by other customers who purchased the above" option is enable')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.ListOfProductsAlsoPurchasedNumberToDisplay')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.ListOfProductsAlsoPurchasedNumberToDisplay', N'2', N'Determines number of products also purchased by other customers to display')
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductAlsoPurchasedLoadByProductID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductAlsoPurchasedLoadByProductID]
GO
CREATE PROCEDURE [dbo].[Nop_ProductAlsoPurchasedLoadByProductID]
(
	@ProductID			int,
	@ShowHidden			bit,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ProductID int NOT NULL,
		ProductsPurchased int NOT NULL,
	)

	INSERT INTO #PageIndex (ProductID, ProductsPurchased)
	SELECT p.ProductID, SUM(opv.Quantity) as ProductsPurchased
	FROM    
		dbo.Nop_OrderProductVariant opv WITH (NOLOCK)
		INNER JOIN dbo.Nop_ProductVariant pv ON pv.ProductVariantId = opv.ProductVariantId
		INNER JOIN dbo.Nop_Product p ON p.ProductId = pv.ProductId
	WHERE
		opv.OrderID IN 
		(
			/* This inner query should retrieve all orders that have contained the productID */
			SELECT 
				DISTINCT OrderID
			FROM 
				dbo.Nop_OrderProductVariant opv2 WITH (NOLOCK)
				INNER JOIN dbo.Nop_ProductVariant pv2 ON pv2.ProductVariantId = opv2.ProductVariantId
				INNER JOIN dbo.Nop_Product p2 ON p2.ProductId = pv2.ProductId				
			WHERE 
				p2.ProductID = @ProductID
		)
		AND 
			(
				p.ProductId != @ProductID
			)
		AND 
			(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				p.Deleted = 0
			)
		AND 
			(
				@ShowHidden = 1
				OR
				GETUTCDATE() BETWEEN ISNULL(pv.AvailableStartDateTime, '1/1/1900') AND ISNULL(pv.AvailableEndDateTime, '1/1/2999')
			)
	GROUP BY
		p.ProductId
	ORDER BY 
		ProductsPurchased desc


	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		p.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex

END
GO



-- new notifications (News comment, Blog comment, Product Review)

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'News.NotifyAboutNewNewsComments')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'News.NotifyAboutNewNewsComments', N'false', N'Determines whether to notify about new news comments')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Blog.NotifyAboutNewBlogComments')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Blog.NotifyAboutNewBlogComments', N'false', N'Determines whether to notify about new blog comments')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Product.NotifyAboutNewProductReviews')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Product.NotifyAboutNewProductReviews', N'false', N'Determines whether to notify about new product reviews')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_MessageTemplate]
		WHERE [Name] = N'News.NewsComment')
BEGIN
	INSERT [dbo].[Nop_MessageTemplate] ([Name])
	VALUES (N'News.NewsComment')

	DECLARE @MessageTemplateID INT 
	SELECT @MessageTemplateID =	mt.MessageTemplateID FROM Nop_MessageTemplate mt
							WHERE mt.Name = 'News.NewsComment' 

	IF (@MessageTemplateID > 0)
	BEGIN
		INSERT [dbo].[Nop_MessageTemplateLocalized] ([MessageTemplateID], [LanguageID], [Subject], [Body]) 
		VALUES (@MessageTemplateID, 7, N'%Store.Name%. New news comment.', N'<p><a href="%Store.URL%">%Store.Name%</a> <br />
		<br />
		A new news comment has been created for news "%NewsComment.NewsTitle%".</p>')
	END
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_MessageTemplate]
		WHERE [Name] = N'Blog.BlogComment')
BEGIN
	INSERT [dbo].[Nop_MessageTemplate] ([Name])
	VALUES (N'Blog.BlogComment')

	DECLARE @MessageTemplateID INT 
	SELECT @MessageTemplateID =	mt.MessageTemplateID FROM Nop_MessageTemplate mt
							WHERE mt.Name = 'Blog.BlogComment' 

	IF (@MessageTemplateID > 0)
	BEGIN
		INSERT [dbo].[Nop_MessageTemplateLocalized] ([MessageTemplateID], [LanguageID], [Subject], [Body]) 
		VALUES (@MessageTemplateID, 7, N'%Store.Name%. New blog comment.', N'<p><a href="%Store.URL%">%Store.Name%</a> <br />
		<br />
		A new blog comment has been created for blog post "%BlogComment.BlogPostTitle%".</p>')
	END
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_MessageTemplate]
		WHERE [Name] = N'Product.ProductReview')
BEGIN
	INSERT [dbo].[Nop_MessageTemplate] ([Name])
	VALUES (N'Product.ProductReview')

	DECLARE @MessageTemplateID INT 
	SELECT @MessageTemplateID =	mt.MessageTemplateID FROM Nop_MessageTemplate mt
							WHERE mt.Name = 'Product.ProductReview' 

	IF (@MessageTemplateID > 0)
	BEGIN
		INSERT [dbo].[Nop_MessageTemplateLocalized] ([MessageTemplateID], [LanguageID], [Subject], [Body]) 
		VALUES (@MessageTemplateID, 7, N'%Store.Name%. New product review.', N'<p><a href="%Store.URL%">%Store.Name%</a> <br />
		<br />
		A new product review has been written for product "%ProductReview.ProductName%".</p>')
	END
END
GO



--customers can be tax exempt
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='IsTaxExempt')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD IsTaxExempt bit NOT NULL CONSTRAINT [DF_Nop_Customer_IsTaxExempt] DEFAULT ((0))
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerInsert]
(
	@CustomerId int = NULL output,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsTaxExempt bit,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Signature nvarchar(300),
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100),
	@AvatarID int
)
AS
BEGIN
	INSERT
	INTO [Nop_Customer]
	(
		CustomerGUID,
		Email,
		PasswordHash,
		SaltKey,
		AffiliateID,
		BillingAddressID,
		ShippingAddressID,
		LastPaymentMethodID,
		LastAppliedCouponCode,
		LanguageID,
		CurrencyID,
		TaxDisplayTypeID,
		IsTaxExempt,
		IsAdmin,
		IsGuest,
		IsForumModerator,
		TotalForumPosts,
		[Signature],
		Active,
		Deleted,
		RegistrationDate,
		TimeZoneID,
		Username,
		AvatarID
	)
	VALUES
	(
		@CustomerGUID,
		@Email,
		@PasswordHash,
		@SaltKey,
		@AffiliateID,
		@BillingAddressID,
		@ShippingAddressID,
		@LastPaymentMethodID,
		@LastAppliedCouponCode,
		@LanguageID,
		@CurrencyID,
		@TaxDisplayTypeID,
		@IsTaxExempt,
		@IsAdmin,
		@IsGuest,
		@IsForumModerator,
		@TotalForumPosts,
		@Signature,
		@Active,
		@Deleted,
		@RegistrationDate,
		@TimeZoneID,
		@Username,
		@AvatarID
	)

	set @CustomerId=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerUpdate]
(
	@CustomerId int,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsTaxExempt bit,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Signature nvarchar(300),
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100),
	@AvatarID int
)
AS
BEGIN

	UPDATE [Nop_Customer]
	SET
		CustomerGUID=@CustomerGUID,
		Email=@Email,
		PasswordHash=@PasswordHash,
		SaltKey=@SaltKey,
		AffiliateID=@AffiliateID,
		BillingAddressID=@BillingAddressID,
		ShippingAddressID=@ShippingAddressID,
		LastPaymentMethodID=@LastPaymentMethodID,
		LastAppliedCouponCode=@LastAppliedCouponCode,
		LanguageID=@LanguageID,
		CurrencyID=@CurrencyID,
		TaxDisplayTypeID=@TaxDisplayTypeID,
		IsTaxExempt=@IsTaxExempt,
		IsAdmin=@IsAdmin,
		IsGuest=@IsGuest,
		IsForumModerator=@IsForumModerator,
		TotalForumPosts=@TotalForumPosts,
		[Signature]=@Signature,
		Active=@Active,
		Deleted=@Deleted,
		RegistrationDate=@RegistrationDate,
		TimeZoneID=@TimeZoneID,
		Username=@Username,
		AvatarID=@AvatarID
	WHERE
		[CustomerId] = @CustomerId

END
GO 

--fixed rate shipping
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_ShippingRateComputationMethod]
		WHERE [Name] = N'Fixed Rate Shipping')
BEGIN
	INSERT [dbo].[Nop_ShippingRateComputationMethod] ([Name], [Description], [ConfigureTemplatePath], [ClassName], [DisplayOrder]) 
	VALUES (N'Fixed Rate Shipping', N'', N'Shipping\FixedRateConfigure\ConfigureShipping.ascx', N'NopSolutions.NopCommerce.Shipping.Methods.FisedRateShippingCM.FixedRateShippingComputationMethod, Nop.Shipping.FixedRateShipping', 6)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FixedRate.Rate')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FixedRate.Rate', N'10', N'Fixed rate shipping')
END
GO

--fixed rate tax
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_TaxProvider]
		WHERE [Name] = N'Fixed Rate Tax')
BEGIN
INSERT [dbo].[Nop_TaxProvider] ([Name], [Description], [ConfigureTemplatePath], [ClassName], [DisplayOrder]) 
VALUES (N'Fixed Rate Tax', N'', N'Tax\FixedRate\ConfigureTax.ascx', N'NopSolutions.NopCommerce.Tax.FixedRateTaxProvider, Nop.Tax.FixedRateTaxProvider', 10)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Tax.TaxProvider.FixedRate.Rate')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Tax.TaxProvider.FixedRate.Rate', N'10', N'Fixed rate tax')
END
GO


--added UpdatedOn column to Nop_Address table
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Address]') and NAME='UpdatedOn')
BEGIN
	ALTER TABLE [dbo].[Nop_Address] 
	ADD UpdatedOn DATETIME NOT NULL CONSTRAINT [DF_Nop_Address_UpdatedOn] DEFAULT ((getutcdate()))

	exec('UPDATE [dbo].[Nop_Address] SET UpdatedOn=CreatedOn')
END
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_AddressInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_AddressInsert]
GO
CREATE PROCEDURE [dbo].[Nop_AddressInsert]
(
	@AddressId int = NULL output,
	@CustomerID int,
	@IsBillingAddress bit,
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@PhoneNumber nvarchar(50),
	@Email nvarchar(255),
	@FaxNumber nvarchar(50),
	@Company nvarchar(100),
	@Address1 nvarchar(100),
	@Address2 nvarchar(100),
	@City nvarchar(100),
	@StateProvinceID int,
	@ZipPostalCode nvarchar(10),
	@CountryID int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Address]
	(
		CustomerID,
		IsBillingAddress,
		FirstName,
		LastName,
		PhoneNumber,
		Email,
		FaxNumber,
		Company,
		Address1,
		Address2,
		City,
		StateProvinceID,
		ZipPostalCode,
		CountryID,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@CustomerID,
		@IsBillingAddress,
		@FirstName,
		@LastName,
		@PhoneNumber,
		@Email,
		@FaxNumber,
		@Company,
		@Address1,
		@Address2,
		@City,
		@StateProvinceID,
		@ZipPostalCode,
		@CountryID,
		@CreatedOn,
		@UpdatedOn
	)

	set @AddressId=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_AddressUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_AddressUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_AddressUpdate]
(
	@AddressId int,
	@CustomerID int,
	@IsBillingAddress bit,
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@PhoneNumber nvarchar(50),
	@Email nvarchar(255),
	@FaxNumber nvarchar(50),
	@Company nvarchar(100),
	@Address1 nvarchar(100),
	@Address2 nvarchar(100),
	@City nvarchar(100),
	@StateProvinceID int,
	@ZipPostalCode nvarchar(10),
	@CountryID int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Address]
	SET
	CustomerID=@CustomerID,
	IsBillingAddress=@IsBillingAddress,
	FirstName=@FirstName,
	LastName=@LastName,
	PhoneNumber=@PhoneNumber,
	Email=@Email,
	FaxNumber=@FaxNumber,
	Company=@Company,
	Address1=@Address1,
	Address2=@Address2,
	City=@City,
	StateProvinceID=@StateProvinceID,
	ZipPostalCode=@ZipPostalCode,
	CountryID=@CountryID,
	CreatedOn=@CreatedOn,
	UpdatedOn=@UpdatedOn
	WHERE
		[AddressId] = @AddressId
END
GO




--enhanced support for downloadable products
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Download]') and NAME='UseDownloadURL')
BEGIN
	ALTER TABLE [dbo].[Nop_Download] 
	ADD UseDownloadURL bit NOT NULL CONSTRAINT [DF_Nop_Nop_Download_UseDownloadURL] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Download]') and NAME='DownloadURL')
BEGIN
	ALTER TABLE [dbo].[Nop_Download] 
	ADD DownloadURL nvarchar(400) NOT NULL CONSTRAINT [DF_Nop_Download_DownloadURL] DEFAULT ((''))
END
GO

ALTER TABLE Nop_Download ALTER COLUMN DownloadBinary image NULL;
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_DownloadInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_DownloadInsert]
GO
CREATE PROCEDURE [dbo].[Nop_DownloadInsert]
(
	@DownloadID int = NULL output,
	@UseDownloadURL bit,
	@DownloadURL nvarchar(400),
	@DownloadBinary image,	
	@ContentType nvarchar(20),
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN
	INSERT
	INTO [Nop_Download]
	(
		UseDownloadURL,
		DownloadURL,
		DownloadBinary,
		ContentType,
		Extension,
		IsNew
	)
	VALUES
	(
		@UseDownloadURL,
		@DownloadURL,
		@DownloadBinary,
		@ContentType,
		@Extension,
		@IsNew
	)

	set @DownloadID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_DownloadUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_DownloadUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_DownloadUpdate]
(
	@DownloadID int,
	@UseDownloadURL bit,
	@DownloadURL nvarchar(400),
	@DownloadBinary image,
	@ContentType nvarchar(20),
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN

	UPDATE [Nop_Download]
	SET		
		UseDownloadURL=@UseDownloadURL,
		DownloadURL=@DownloadURL,
		DownloadBinary=@DownloadBinary,
		ContentType = @ContentType,
		Extension=@Extension,
		IsNew=@IsNew
	WHERE
		DownloadID = @DownloadID

END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='UnlimitedDownloads')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD UnlimitedDownloads bit NOT NULL CONSTRAINT [DF_Nop_ProductVariant_UnlimitedDownloads] DEFAULT ((1))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='MaxNumberOfDownloads')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD MaxNumberOfDownloads int NOT NULL CONSTRAINT [DF_Nop_ProductVariant_MaxNumberOfDownloads] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='HasSampleDownload')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD HasSampleDownload bit NOT NULL CONSTRAINT [DF_Nop_ProductVariant_HasSampleDownload] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='SampleDownloadID')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	ADD SampleDownloadID int NOT NULL CONSTRAINT [DF_Nop_ProductVariant_SampleDownloadID] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantInsert]
(
    @ProductVariantID int = NULL output,
    @ProductId int,
    @Name nvarchar(400),
    @SKU nvarchar (400),
    @Description nvarchar(4000),
    @AdminComment nvarchar(4000),
    @ManufacturerPartNumber nvarchar(100),
    @IsDownload bit,
    @DownloadID int,
	@UnlimitedDownloads bit,
	@MaxNumberOfDownloads int,
	@HasSampleDownload bit,
	@SampleDownloadID int,
    @IsShipEnabled bit,
    @IsFreeShipping bit,
	@AdditionalShippingCharge money,
    @IsTaxExempt bit,
    @TaxCategoryID int,
	@ManageInventory bit,
    @StockQuantity int,
    @MinStockQuantity int,
    @LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
    @WarehouseId int,
    @DisableBuyButton int,
    @RequiresTextOption bit,
    @TextOptionPrompt nvarchar(400),
    @Price money,
    @OldPrice money,
    @Weight float,
    @Length decimal(18, 4),
    @Width decimal(18, 4),
    @Height decimal(18, 4),
    @PictureID int,
	@AvailableStartDateTime datetime,
	@AvailableEndDateTime datetime,
    @Published bit,
    @Deleted bit,
    @DisplayOrder int,
	@CreatedOn datetime,
    @UpdatedOn datetime
)
AS
BEGIN
    INSERT
    INTO [Nop_ProductVariant]
    (
        ProductId,
        [Name],
        SKU,
        [Description],
        AdminComment,
        ManufacturerPartNumber,
        IsDownload,
        DownloadID,
		UnlimitedDownloads,
		MaxNumberOfDownloads,
		HasSampleDownload,
		SampleDownloadID,
        IsShipEnabled,
        IsFreeShipping,
		AdditionalShippingCharge,
        IsTaxExempt,
        TaxCategoryID,
		ManageInventory,
        StockQuantity,
        MinStockQuantity,
        LowStockActivityID,
		NotifyAdminForQuantityBelow,
		OrderMinimumQuantity,
		OrderMaximumQuantity,
        WarehouseId,
        DisableBuyButton,
        RequiresTextOption,
        TextOptionPrompt,
        Price,
        OldPrice,
        Weight,
        [Length],
        Width,
        Height,
        PictureID,
		AvailableStartDateTime,
		AvailableEndDateTime,
        Published,
        Deleted,
        DisplayOrder,
        CreatedOn,
        UpdatedOn
    )
    VALUES
    (
        @ProductId,
        @Name,
        @SKU,
        @Description,
        @AdminComment,
        @ManufacturerPartNumber,
        @IsDownload,
        @DownloadID,
		@UnlimitedDownloads,
		@MaxNumberOfDownloads,
		@HasSampleDownload,
		@SampleDownloadID,
        @IsShipEnabled,
        @IsFreeShipping,
		@AdditionalShippingCharge,
        @IsTaxExempt,
        @TaxCategoryID,
		@ManageInventory,
        @StockQuantity,
        @MinStockQuantity,
        @LowStockActivityID,
		@NotifyAdminForQuantityBelow,
		@OrderMinimumQuantity,
		@OrderMaximumQuantity,
        @WarehouseId,
        @DisableBuyButton,
        @RequiresTextOption,
        @TextOptionPrompt,
        @Price,
        @OldPrice,
        @Weight,
        @Length,
        @Width,
        @Height,
        @PictureID,
		@AvailableStartDateTime,
		@AvailableEndDateTime,
        @Published,
        @Deleted,
        @DisplayOrder,
        @CreatedOn,
        @UpdatedOn
    )

    set @ProductVariantID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantUpdate]
(
	@ProductVariantID int,
	@ProductId int,
	@Name nvarchar(400),
	@SKU nvarchar (400),
	@Description nvarchar(4000),
	@AdminComment nvarchar(4000),
	@ManufacturerPartNumber nvarchar(100),
	@IsDownload bit,
	@DownloadID int,
	@UnlimitedDownloads bit,
	@MaxNumberOfDownloads int,
	@HasSampleDownload bit,
	@SampleDownloadID int,
	@IsShipEnabled bit,
	@IsFreeShipping bit,
	@AdditionalShippingCharge money,
	@IsTaxExempt bit,
	@TaxCategoryID int,
	@ManageInventory bit,
	@StockQuantity int,
	@MinStockQuantity int,
	@LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
	@WarehouseId int,
	@DisableBuyButton bit,
	@RequiresTextOption bit,
	@TextOptionPrompt nvarchar(400),
	@Price money,
	@OldPrice money,
	@Weight float,
	@Length decimal(18, 4),
	@Width decimal(18, 4),
	@Height decimal(18, 4),
	@PictureID int,
	@AvailableStartDateTime datetime,
	@AvailableEndDateTime datetime,
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ProductVariant]
	SET
		ProductId=@ProductId,
		[Name]=@Name,
		[SKU]=@SKU,
		[Description]=@Description,
		AdminComment=@AdminComment,
		ManufacturerPartNumber=@ManufacturerPartNumber,
		IsDownload=@IsDownload,
		DownloadID=@DownloadID,
		UnlimitedDownloads=@UnlimitedDownloads,
		MaxNumberOfDownloads=@MaxNumberOfDownloads,
		HasSampleDownload=@HasSampleDownload,
		SampleDownloadID=@SampleDownloadID,
		IsShipEnabled=@IsShipEnabled,
		IsFreeShipping=@IsFreeShipping,
		AdditionalShippingCharge=@AdditionalShippingCharge,
		IsTaxExempt=@IsTaxExempt,
		TaxCategoryID=@TaxCategoryID,
		ManageInventory=@ManageInventory,
		StockQuantity=@StockQuantity,
		MinStockQuantity=@MinStockQuantity,
		LowStockActivityID=@LowStockActivityID,
		NotifyAdminForQuantityBelow=@NotifyAdminForQuantityBelow,
		OrderMinimumQuantity=@OrderMinimumQuantity,
		OrderMaximumQuantity=@OrderMaximumQuantity,
		WarehouseId=@WarehouseId,
		DisableBuyButton=@DisableBuyButton,
		RequiresTextOption=@RequiresTextOption,
		TextOptionPrompt=@TextOptionPrompt,
		Price=@Price,
		OldPrice=@OldPrice,
		Weight=@Weight,
		[Length]=@Length,
		Width=@Width,
		Height=@Height,
		PictureID=@PictureID,
		AvailableStartDateTime=@AvailableStartDateTime,
		AvailableEndDateTime=@AvailableEndDateTime,
		Published=@Published,
		Deleted=@Deleted,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		ProductVariantID = @ProductVariantID
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='DownloadCount')
BEGIN
	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	ADD DownloadCount int NOT NULL CONSTRAINT [DF_Nop_OrderProductVariant_DownloadCount] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantInsert]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantInsert]
(
	@OrderProductVariantID int = NULL output,
	@OrderID int,
	@ProductVariantID int,
	@UnitPriceInclTax money,
	@UnitPriceExclTax money,
	@PriceInclTax money,
	@PriceExclTax money,
	@UnitPriceInclTaxInCustomerCurrency money,
	@UnitPriceExclTaxInCustomerCurrency money,
	@PriceInclTaxInCustomerCurrency money,
	@PriceExclTaxInCustomerCurrency money,
	@AttributeDescription nvarchar(4000),
	@TextOption nvarchar(400),
	@Quantity int,
	@DiscountAmountInclTax decimal (18, 4),
	@DiscountAmountExclTax decimal (18, 4),
	@DownloadCount int
)
AS
BEGIN
	INSERT
	INTO [Nop_OrderProductVariant]
	(
		OrderID,
		ProductVariantID,
		UnitPriceInclTax,
		UnitPriceExclTax,
		PriceInclTax,
		PriceExclTax,
		UnitPriceInclTaxInCustomerCurrency,
		UnitPriceExclTaxInCustomerCurrency,
		PriceInclTaxInCustomerCurrency,
		PriceExclTaxInCustomerCurrency,
		AttributeDescription,
		TextOption,
		Quantity,
		DiscountAmountInclTax,
		DiscountAmountExclTax,
		DownloadCount
	)
	VALUES
	(
		@OrderID,
		@ProductVariantID,
		@UnitPriceInclTax,
		@UnitPriceExclTax,
		@PriceInclTax,
		@PriceExclTax,
		@UnitPriceInclTaxInCustomerCurrency,
		@UnitPriceExclTaxInCustomerCurrency,
		@PriceInclTaxInCustomerCurrency,
		@PriceExclTaxInCustomerCurrency,
		@AttributeDescription,
		@TextOption,
		@Quantity,
		@DiscountAmountInclTax,
		@DiscountAmountExclTax,
		@DownloadCount
	)

	set @OrderProductVariantID=@@identity
END
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantUpdate]
(
	@OrderProductVariantID int,
	@OrderID int,
	@ProductVariantID int,
	@UnitPriceInclTax money,
	@UnitPriceExclTax money,
	@PriceInclTax money,
	@PriceExclTax money,
	@UnitPriceInclTaxInCustomerCurrency money,
	@UnitPriceExclTaxInCustomerCurrency money,
	@PriceInclTaxInCustomerCurrency money,
	@PriceExclTaxInCustomerCurrency money,
	@AttributeDescription nvarchar(4000),
	@TextOption nvarchar(400),
	@Quantity int,
	@DiscountAmountInclTax decimal (18, 4),
	@DiscountAmountExclTax decimal (18, 4),
	@DownloadCount int
)
AS
BEGIN

	UPDATE [Nop_OrderProductVariant]
	SET		
		OrderID=@OrderID,
		ProductVariantID=@ProductVariantID,
		UnitPriceInclTax=@UnitPriceInclTax,
		UnitPriceExclTax = @UnitPriceExclTax,
		PriceInclTax=@PriceInclTax,
		PriceExclTax=@PriceExclTax,
		UnitPriceInclTaxInCustomerCurrency=@UnitPriceInclTaxInCustomerCurrency,
		UnitPriceExclTaxInCustomerCurrency=@UnitPriceExclTaxInCustomerCurrency,
		PriceInclTaxInCustomerCurrency=@PriceInclTaxInCustomerCurrency,
		PriceExclTaxInCustomerCurrency=@PriceExclTaxInCustomerCurrency,
		AttributeDescription=@AttributeDescription,
		TextOption=@TextOption,
		Quantity=@Quantity,
		DiscountAmountInclTax=@DiscountAmountInclTax,
		DiscountAmountExclTax=@DiscountAmountExclTax,
		DownloadCount=@DownloadCount
	WHERE
		OrderProductVariantID = @OrderProductVariantID
END
GO


--OrderSearch - add ShippingStatusID to query

IF EXISTS (
		SELECT * FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderSearch]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderSearch]
GO

CREATE PROCEDURE [dbo].[Nop_OrderSearch]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@CustomerEmail nvarchar(255) = NULL,
	@OrderStatusID int,
	@PaymentStatusID int,
	@ShippingStatusID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		o.*
	FROM [Nop_Order] o
	LEFT OUTER JOIN [Nop_Customer] c ON o.CustomerID = c.CustomerID
	WHERE
		(@CustomerEmail IS NULL or LEN(@CustomerEmail)=0 or (c.Email like '%' + COALESCE(@CustomerEmail,c.Email) + '%')) and
		(@StartTime is NULL or DATEDIFF(day, @StartTime, o.CreatedOn) >= 0) and
		(@EndTime is NULL or DATEDIFF(day, @EndTime, o.CreatedOn) <= 0) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(@ShippingStatusID IS NULL OR @ShippingStatusID = 0 OR o.ShippingStatusID = @ShippingStatusID) and
		(o.Deleted=0)
	ORDER BY o.CreatedOn desc
END
GO

--encrypt Order.CardName and Order.CartType values. Add masked credit card number
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='MaskedCreditCardNumber')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD MaskedCreditCardNumber nvarchar(100) NOT NULL CONSTRAINT [DF_Nop_Order_MaskedCreditCardNumber] DEFAULT ((''))

	exec('UPDATE [dbo].[Nop_Order] SET CardName=''''')
	exec('UPDATE [dbo].[Nop_Order] SET CardType=''''')
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Order]') and NAME='AllowStoringCreditCardNumber')
BEGIN
	ALTER TABLE [dbo].[Nop_Order] 
	ADD AllowStoringCreditCardNumber bit NOT NULL CONSTRAINT [DF_Nop_Order_AllowStoringCreditCardNumber] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT * FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderInsert]
GO
CREATE PROCEDURE [dbo].[Nop_OrderInsert]
(
	@OrderID int = NULL output,
	@OrderGUID uniqueidentifier,
	@CustomerID int,
	@CustomerLanguageID int,
	@CustomerTaxDisplayTypeID int,
	@OrderSubtotalInclTax money,
	@OrderSubtotalExclTax money,
	@OrderShippingInclTax money,
	@OrderShippingExclTax money,
	@PaymentMethodAdditionalFeeInclTax money,
	@PaymentMethodAdditionalFeeExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency money,
	@OrderTaxInCustomerCurrency money,
	@OrderTotalInCustomerCurrency money,
	@CustomerCurrencyCode nvarchar(5),
	@OrderWeight float,
	@AffiliateID int,
	@OrderStatusID int,
	@AllowStoringCreditCardNumber bit,
	@CardType nvarchar(100),
	@CardName nvarchar(100),
	@CardNumber nvarchar(100),
	@MaskedCreditCardNumber nvarchar(100),
	@CardCVV2 nvarchar(100),
	@CardExpirationMonth nvarchar(100),
	@CardExpirationYear nvarchar(100),
	@PaymentMethodID int,
	@PaymentMethodName nvarchar(100),
	@AuthorizationTransactionID nvarchar(4000),
	@AuthorizationTransactionCode nvarchar(4000),
	@AuthorizationTransactionResult nvarchar(1000),
	@CaptureTransactionID nvarchar(4000),
	@CaptureTransactionResult nvarchar(1000),
	@PurchaseOrderNumber nvarchar(100),
	@PaymentStatusID int,
	@BillingFirstName nvarchar(100),
	@BillingLastName nvarchar(100),
	@BillingPhoneNumber nvarchar(50),
	@BillingEmail nvarchar(255),
	@BillingFaxNumber nvarchar(50),
	@BillingCompany nvarchar(100),
	@BillingAddress1 nvarchar(100),
	@BillingAddress2 nvarchar(100),
	@BillingCity nvarchar(100),
	@BillingStateProvince nvarchar(100),
	@BillingStateProvinceID int,
	@BillingZipPostalCode nvarchar(10),
	@BillingCountry nvarchar(100),
	@BillingCountryID int,
	@ShippingStatusID int,
	@ShippingFirstName nvarchar(100),
	@ShippingLastName nvarchar(100),
	@ShippingPhoneNumber nvarchar(50),
	@ShippingEmail nvarchar(255),
	@ShippingFaxNumber nvarchar(50),
	@ShippingCompany nvarchar(100),
	@ShippingAddress1 nvarchar(100),
	@ShippingAddress2 nvarchar(100),
	@ShippingCity nvarchar(100),
	@ShippingStateProvince nvarchar(100),
	@ShippingStateProvinceID int,
	@ShippingZipPostalCode nvarchar(10),
	@ShippingCountry nvarchar(100),
	@ShippingCountryID int,
	@ShippingMethod nvarchar(100),
	@ShippingRateComputationMethodID int,
	@ShippedDate datetime,
	@Deleted bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Order]
	(
		OrderGUID,
		CustomerID,
		CustomerLanguageID,
		CustomerTaxDisplayTypeID,
		OrderSubtotalInclTax,
		OrderSubtotalExclTax,
		OrderShippingInclTax,
		OrderShippingExclTax,
		PaymentMethodAdditionalFeeInclTax,
		PaymentMethodAdditionalFeeExclTax,
		OrderTax,
		OrderTotal,
		OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
		OrderTaxInCustomerCurrency,
		OrderTotalInCustomerCurrency,
		CustomerCurrencyCode,
		OrderWeight,
		AffiliateID,
		OrderStatusID,
		AllowStoringCreditCardNumber,
		CardType,
		CardName,
		CardNumber,
		MaskedCreditCardNumber,
		CardCVV2,
		CardExpirationMonth,
		CardExpirationYear,
		PaymentMethodID,
		PaymentMethodName,
		AuthorizationTransactionID,
		AuthorizationTransactionCode,
		AuthorizationTransactionResult,
		CaptureTransactionID,
		CaptureTransactionResult,
		PurchaseOrderNumber,
		PaymentStatusID,
		BillingFirstName,
		BillingLastName,
		BillingPhoneNumber,
		BillingEmail,
		BillingFaxNumber,
		BillingCompany,
		BillingAddress1,
		BillingAddress2,
		BillingCity,
		BillingStateProvince,
		BillingStateProvinceID,
		BillingZipPostalCode,
		BillingCountry,
		BillingCountryID,
		ShippingStatusID,
		ShippingFirstName,
		ShippingLastName,
		ShippingPhoneNumber,
		ShippingEmail,
		ShippingFaxNumber,
		ShippingCompany,
		ShippingAddress1,
		ShippingAddress2,
		ShippingCity,
		ShippingStateProvince,
		ShippingZipPostalCode,
		ShippingStateProvinceID,
		ShippingCountry,
		ShippingCountryID,
		ShippingMethod,
		ShippingRateComputationMethodID,
		ShippedDate,
		Deleted,
		CreatedOn
	)
	VALUES
	(
		@OrderGUID,
		@CustomerID,
		@CustomerLanguageID,		
		@CustomerTaxDisplayTypeID,
		@OrderSubtotalInclTax,
		@OrderSubtotalExclTax,		
		@OrderShippingInclTax,
		@OrderShippingExclTax,
		@PaymentMethodAdditionalFeeInclTax,
		@PaymentMethodAdditionalFeeExclTax,
		@OrderTax,
		@OrderTotal,
		@OrderDiscount,		
		@OrderSubtotalInclTaxInCustomerCurrency,
		@OrderSubtotalExclTaxInCustomerCurrency,		
		@OrderShippingInclTaxInCustomerCurrency,
		@OrderShippingExclTaxInCustomerCurrency,
		@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
		@OrderTaxInCustomerCurrency,
		@OrderTotalInCustomerCurrency,
		@CustomerCurrencyCode,
		@OrderWeight,
		@AffiliateID,
		@OrderStatusID,
		@AllowStoringCreditCardNumber,
		@CardType,
		@CardName,
		@CardNumber,
		@MaskedCreditCardNumber,
		@CardCVV2,
		@CardExpirationMonth,
		@CardExpirationYear,
		@PaymentMethodID,
		@PaymentMethodName,
		@AuthorizationTransactionID,
		@AuthorizationTransactionCode,
		@AuthorizationTransactionResult,
		@CaptureTransactionID,
		@CaptureTransactionResult,
		@PurchaseOrderNumber,
		@PaymentStatusID,
		@BillingFirstName,
		@BillingLastName,
		@BillingPhoneNumber,
		@BillingEmail,
		@BillingFaxNumber,
		@BillingCompany,
		@BillingAddress1,
		@BillingAddress2,
		@BillingCity,
		@BillingStateProvince,
		@BillingStateProvinceID,
		@BillingZipPostalCode,
		@BillingCountry,
		@BillingCountryID,
		@ShippingStatusID,
		@ShippingFirstName,
		@ShippingLastName,
		@ShippingPhoneNumber,
		@ShippingEmail,
		@ShippingFaxNumber,
		@ShippingCompany,
		@ShippingAddress1,
		@ShippingAddress2,
		@ShippingCity,
		@ShippingStateProvince,
		@ShippingZipPostalCode,
		@ShippingStateProvinceID,
		@ShippingCountry,
		@ShippingCountryID,
		@ShippingMethod,
		@ShippingRateComputationMethodID,
		@ShippedDate,
		@Deleted,
		@CreatedOn
	)

	set @OrderID=@@identity
END
GO



IF EXISTS (
		SELECT * FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_OrderUpdate]
(
	@OrderID int,
	@OrderGUID uniqueidentifier,
	@CustomerID int,
	@CustomerLanguageID int,
	@CustomerTaxDisplayTypeID int,
	@OrderSubtotalInclTax money,
	@OrderSubtotalExclTax money,
	@OrderShippingInclTax money,
	@OrderShippingExclTax money,
	@PaymentMethodAdditionalFeeInclTax money,
	@PaymentMethodAdditionalFeeExclTax money,
	@OrderTax money,
	@OrderTotal money,
	@OrderDiscount money,
	@OrderSubtotalInclTaxInCustomerCurrency money,
	@OrderSubtotalExclTaxInCustomerCurrency money,
	@OrderShippingInclTaxInCustomerCurrency money,
	@OrderShippingExclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency money,
	@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency money,
	@OrderTaxInCustomerCurrency money,
	@OrderTotalInCustomerCurrency money,
	@CustomerCurrencyCode nvarchar(5),
	@OrderWeight float,
	@AffiliateID int,
	@OrderStatusID int,
	@AllowStoringCreditCardNumber bit,
	@CardType nvarchar(100),
	@CardName nvarchar(100),
	@CardNumber nvarchar(100),
	@MaskedCreditCardNumber nvarchar(100),
	@CardCVV2 nvarchar(100),
	@CardExpirationMonth nvarchar(100),
	@CardExpirationYear nvarchar(100),
	@PaymentMethodID int,
	@PaymentMethodName nvarchar(100),
	@AuthorizationTransactionID nvarchar(4000),
	@AuthorizationTransactionCode nvarchar(4000),
	@AuthorizationTransactionResult nvarchar(1000),
	@CaptureTransactionID nvarchar(4000),
	@CaptureTransactionResult nvarchar(1000),
	@PurchaseOrderNumber nvarchar(100),
	@PaymentStatusID int,
	@BillingFirstName nvarchar(100),
	@BillingLastName nvarchar(100),
	@BillingPhoneNumber nvarchar(50),
	@BillingEmail nvarchar(255),
	@BillingFaxNumber nvarchar(50),
	@BillingCompany nvarchar(100),
	@BillingAddress1 nvarchar(100),
	@BillingAddress2 nvarchar(100),
	@BillingCity nvarchar(100),
	@BillingStateProvince nvarchar(100),
	@BillingStateProvinceID int,
	@BillingZipPostalCode nvarchar(10),
	@BillingCountry nvarchar(100),
	@BillingCountryID int,
	@ShippingStatusID int,
	@ShippingFirstName nvarchar(100),
	@ShippingLastName nvarchar(100),
	@ShippingPhoneNumber nvarchar(50),
	@ShippingEmail nvarchar(255),
	@ShippingFaxNumber nvarchar(50),
	@ShippingCompany nvarchar(100),
	@ShippingAddress1 nvarchar(100),
	@ShippingAddress2 nvarchar(100),
	@ShippingCity nvarchar(100),
	@ShippingStateProvince nvarchar(100),
	@ShippingStateProvinceID int,
	@ShippingZipPostalCode nvarchar(10),
	@ShippingCountry nvarchar(100),
	@ShippingCountryID int,
	@ShippingMethod nvarchar(100),
	@ShippingRateComputationMethodID int,
	@ShippedDate datetime,
	@Deleted bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Order]
	SET
		OrderGUID=@OrderGUID,
		CustomerID=@CustomerID,
		CustomerLanguageID=@CustomerLanguageID,		
		CustomerTaxDisplayTypeID=@CustomerTaxDisplayTypeID,
		OrderSubtotalInclTax=@OrderSubtotalInclTax,
		OrderSubtotalExclTax=@OrderSubtotalExclTax,		
		OrderShippingInclTax=@OrderShippingInclTax,
		OrderShippingExclTax=@OrderShippingExclTax,
		PaymentMethodAdditionalFeeInclTax=@PaymentMethodAdditionalFeeInclTax,
		PaymentMethodAdditionalFeeExclTax=@PaymentMethodAdditionalFeeExclTax,
		OrderTax=@OrderTax,
		OrderTotal=@OrderTotal,
		OrderDiscount=@OrderDiscount,
		OrderSubtotalInclTaxInCustomerCurrency=@OrderSubtotalInclTaxInCustomerCurrency,
		OrderSubtotalExclTaxInCustomerCurrency=@OrderSubtotalExclTaxInCustomerCurrency,
		OrderShippingInclTaxInCustomerCurrency=@OrderShippingInclTaxInCustomerCurrency,
		OrderShippingExclTaxInCustomerCurrency=@OrderShippingExclTaxInCustomerCurrency,	
		PaymentMethodAdditionalFeeInclTaxInCustomerCurrency=@PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
		PaymentMethodAdditionalFeeExclTaxInCustomerCurrency=@PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,	
		OrderTaxInCustomerCurrency=@OrderTaxInCustomerCurrency,
		OrderTotalInCustomerCurrency=@OrderTotalInCustomerCurrency,
		CustomerCurrencyCode=@CustomerCurrencyCode,
		OrderWeight=@OrderWeight,
		AffiliateID=@AffiliateID,
		OrderStatusID=@OrderStatusID,
		AllowStoringCreditCardNumber=@AllowStoringCreditCardNumber,
		CardType=@CardType,
		CardName=@CardName,
		CardNumber=@CardNumber,
		MaskedCreditCardNumber=@MaskedCreditCardNumber,
		CardCVV2=@CardCVV2,
		CardExpirationMonth=@CardExpirationMonth,
		CardExpirationYear=@CardExpirationYear,
		PaymentMethodID=@PaymentMethodID,
		PaymentMethodName=@PaymentMethodName,
		AuthorizationTransactionID=@AuthorizationTransactionID,
		AuthorizationTransactionCode=@AuthorizationTransactionCode,
		AuthorizationTransactionResult=@AuthorizationTransactionResult,
		CaptureTransactionID=@CaptureTransactionID,
		CaptureTransactionResult=@CaptureTransactionResult,
		PurchaseOrderNumber=@PurchaseOrderNumber,
		PaymentStatusID=@PaymentStatusID,
		BillingFirstName=@BillingFirstName,
		BillingLastName=@BillingLastName,
		BillingPhoneNumber=@BillingPhoneNumber,
		BillingEmail=@BillingEmail,
		BillingFaxNumber=@BillingFaxNumber,
		BillingCompany=@BillingCompany,
		BillingAddress1=@BillingAddress1,
		BillingAddress2=@BillingAddress2,
		BillingCity=@BillingCity,
		BillingStateProvince=@BillingStateProvince,
		BillingStateProvinceID=@BillingStateProvinceID,
		BillingZipPostalCode=@BillingZipPostalCode,
		BillingCountry=@BillingCountry,
		BillingCountryID=@BillingCountryID,
		ShippingStatusID=@ShippingStatusID,
		ShippingFirstName=@ShippingFirstName,
		ShippingLastName=@ShippingLastName,
		ShippingPhoneNumber=@ShippingPhoneNumber,
		ShippingEmail=@ShippingEmail,
		ShippingFaxNumber=@ShippingFaxNumber,
		ShippingCompany=@ShippingCompany,
		ShippingAddress1=@ShippingAddress1,
		ShippingAddress2=@ShippingAddress2,
		ShippingCity=@ShippingCity,
		ShippingStateProvince=@ShippingStateProvince,
		ShippingStateProvinceID=@ShippingStateProvinceID,
		ShippingZipPostalCode=@ShippingZipPostalCode,
		ShippingCountry=@ShippingCountry,
		ShippingCountryID=@ShippingCountryID,
		ShippingMethod=@ShippingMethod,
		ShippingRateComputationMethodID=@ShippingRateComputationMethodID,
		ShippedDate=@ShippedDate,
		Deleted=@Deleted,
		CreatedOn=@CreatedOn
	WHERE
		OrderID = @OrderID
END
GO

--active topics

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_TopicLoadActive]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_TopicLoadActive]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_TopicLoadActive]
(
	@ForumID			int,
	@TopicCount			int
)
AS
BEGIN
	if (@TopicCount > 0)
	      SET ROWCOUNT @TopicCount

	SELECT ft2.* FROM Nop_Forums_Topic ft2 with (NOLOCK) 
	WHERE ft2.TopicID IN 
	(
		SELECT DISTINCT
			ft.TopicID
		FROM Nop_Forums_Topic ft with (NOLOCK)
		WHERE  (
					@ForumID IS NULL OR @ForumID=0
					OR (ft.ForumID=@ForumID)
				)
				AND
				(
					ft.LastPostTime is not null
				)
	)
	ORDER BY ft2.LastPostTime desc

	SET ROWCOUNT 0
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Forums.ActiveDiscussions.TopicCount')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Forums.ActiveDiscussions.TopicCount', N'5', N'')
END
GO


--country display order
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_CountryLoadAll]
	@ShowHidden bit = 0
AS
BEGIN
	SELECT *
	FROM [Nop_Country]
	WHERE (Published = 1 or @ShowHidden = 1)
	ORDER BY DisplayOrder, [Name]
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadAllForBilling]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadAllForBilling]
GO
CREATE PROCEDURE [dbo].[Nop_CountryLoadAllForBilling]
	@ShowHidden bit = 0
AS
BEGIN
	SELECT *
	FROM [Nop_Country]
	WHERE (Published = 1 or @ShowHidden = 1) and AllowsBilling=1
	ORDER BY DisplayOrder, [Name]
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadAllForRegistration]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadAllForRegistration]
GO
CREATE PROCEDURE [dbo].[Nop_CountryLoadAllForRegistration]
	@ShowHidden bit = 0
AS
BEGIN
	SELECT *
	FROM [Nop_Country]
	WHERE (Published = 1 or @ShowHidden = 1) and AllowsRegistration=1
	ORDER BY DisplayOrder, [Name]
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CountryLoadAllForShipping]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CountryLoadAllForShipping]
GO
CREATE PROCEDURE [dbo].[Nop_CountryLoadAllForShipping]
	@ShowHidden bit = 0
AS
BEGIN
	SELECT *
	FROM [Nop_Country]
	WHERE (Published = 1 or @ShowHidden = 1) and AllowsShipping=1
	ORDER BY DisplayOrder, [Name]
END
GO

--customer admin comment

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Customer]') and NAME='AdminComment')
BEGIN
	ALTER TABLE [dbo].[Nop_Customer] 
	ADD AdminComment nvarchar(4000) NOT NULL CONSTRAINT [DF_Nop_Customer_AdminComment] DEFAULT ((''))
END
GO
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerInsert]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerInsert]
(
	@CustomerId int = NULL output,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsTaxExempt bit,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Signature nvarchar(300),
	@AdminComment nvarchar(4000),
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100),
	@AvatarID int
)
AS
BEGIN
	INSERT
	INTO [Nop_Customer]
	(
		CustomerGUID,
		Email,
		PasswordHash,
		SaltKey,
		AffiliateID,
		BillingAddressID,
		ShippingAddressID,
		LastPaymentMethodID,
		LastAppliedCouponCode,
		LanguageID,
		CurrencyID,
		TaxDisplayTypeID,
		IsTaxExempt,
		IsAdmin,
		IsGuest,
		IsForumModerator,
		TotalForumPosts,
		[Signature],
		AdminComment,
		Active,
		Deleted,
		RegistrationDate,
		TimeZoneID,
		Username,
		AvatarID
	)
	VALUES
	(
		@CustomerGUID,
		@Email,
		@PasswordHash,
		@SaltKey,
		@AffiliateID,
		@BillingAddressID,
		@ShippingAddressID,
		@LastPaymentMethodID,
		@LastAppliedCouponCode,
		@LanguageID,
		@CurrencyID,
		@TaxDisplayTypeID,
		@IsTaxExempt,
		@IsAdmin,
		@IsGuest,
		@IsForumModerator,
		@TotalForumPosts,
		@Signature,
		@AdminComment,
		@Active,
		@Deleted,
		@RegistrationDate,
		@TimeZoneID,
		@Username,
		@AvatarID
	)

	set @CustomerId=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerUpdate]
(
	@CustomerId int,
	@CustomerGUID uniqueidentifier,
	@Email nvarchar(255),
	@PasswordHash nvarchar(255),
	@SaltKey nvarchar(255),
	@AffiliateID int,
	@BillingAddressID int,
	@ShippingAddressID int,
	@LastPaymentMethodID int,
	@LastAppliedCouponCode nvarchar(100),
	@LanguageID int,
	@CurrencyID int,
	@TaxDisplayTypeID int,
	@IsTaxExempt bit,
	@IsAdmin bit,
	@IsGuest bit,
	@IsForumModerator bit,
	@TotalForumPosts int,
	@Signature nvarchar(300),
	@AdminComment nvarchar(4000),
	@Active bit,
	@Deleted bit,
	@RegistrationDate datetime,
	@TimeZoneID nvarchar(200),
	@Username nvarchar(100),
	@AvatarID int
)
AS
BEGIN

	UPDATE [Nop_Customer]
	SET
		CustomerGUID=@CustomerGUID,
		Email=@Email,
		PasswordHash=@PasswordHash,
		SaltKey=@SaltKey,
		AffiliateID=@AffiliateID,
		BillingAddressID=@BillingAddressID,
		ShippingAddressID=@ShippingAddressID,
		LastPaymentMethodID=@LastPaymentMethodID,
		LastAppliedCouponCode=@LastAppliedCouponCode,
		LanguageID=@LanguageID,
		CurrencyID=@CurrencyID,
		TaxDisplayTypeID=@TaxDisplayTypeID,
		IsTaxExempt=@IsTaxExempt,
		IsAdmin=@IsAdmin,
		IsGuest=@IsGuest,
		IsForumModerator=@IsForumModerator,
		TotalForumPosts=@TotalForumPosts,
		[Signature]=@Signature,
		AdminComment=@AdminComment,
		Active=@Active,
		Deleted=@Deleted,
		RegistrationDate=@RegistrationDate,
		TimeZoneID=@TimeZoneID,
		Username=@Username,
		AvatarID=@AvatarID
	WHERE
		[CustomerId] = @CustomerId

END
GO


--incomplete orders and registered customers reports
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderIncompleteReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderIncompleteReport]
GO

CREATE PROCEDURE [dbo].[Nop_OrderIncompleteReport]
(
	@OrderStatusID int,
	@PaymentStatusID int,
	@ShippingStatusID int
)
AS
BEGIN

	SELECT
		SUM(o.OrderTotal) [Total],
		COUNT(o.OrderID) [Count]
	FROM Nop_Order o
	WHERE 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) AND
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) AND
		(@ShippingStatusID IS NULL or @ShippingStatusID=0 or o.ShippingStatusID = @ShippingStatusID) AND
		o.Deleted=0

END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerRegisteredReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerRegisteredReport]
GO

CREATE PROCEDURE [dbo].[Nop_CustomerRegisteredReport]
(
	@Date datetime
)
AS
BEGIN

	SELECT COUNT(CustomerID) [Count]
	FROM Nop_Customer
	WHERE 
		Active = 1 
		AND Deleted = 0
		AND IsGuest = 0
		AND RegistrationDate BETWEEN @Date AND GETUTCDATE()

END
GO



--
-- product specs
--

--Nop_SpecificationAttribute
IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_SpecificationAttribute]') and NAME='DisplayOrder')
begin
	ALTER TABLE [dbo].[Nop_SpecificationAttribute] 
	ADD [DisplayOrder] [int] NOT NULL CONSTRAINT [DF_Nop_SpecificationAttribute_DisplayOrder]  DEFAULT ((1))
end
go

--Nop_SpecificationAttributeOption
if not exists (select 1 from sysobjects where id = object_id(N'[dbo].[Nop_SpecificationAttributeOption]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[Nop_SpecificationAttributeOption](
	[SpecificationAttributeOptionID] [int] IDENTITY(1,1) NOT NULL,
	[SpecificationAttributeID] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_Nop_SpecificationAttributeOption_DisplayOrder]  DEFAULT ((1)),
 CONSTRAINT [PK_Nop_SpecificationAttributeOption] PRIMARY KEY CLUSTERED 
(
	[SpecificationAttributeOptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_SpecificationAttributeOption_Nop_SpecificationAttribute'
           AND parent_obj = Object_id('Nop_SpecificationAttributeOption')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_SpecificationAttributeOption
DROP CONSTRAINT FK_Nop_SpecificationAttributeOption_Nop_SpecificationAttribute
GO
ALTER TABLE [dbo].[Nop_SpecificationAttributeOption]  WITH CHECK ADD  CONSTRAINT [FK_Nop_SpecificationAttributeOption_Nop_SpecificationAttribute] FOREIGN KEY([SpecificationAttributeID])
REFERENCES [dbo].[Nop_SpecificationAttribute] ([SpecificationAttributeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Nop_SpecificationAttributeOption] CHECK CONSTRAINT [FK_Nop_SpecificationAttributeOption_Nop_SpecificationAttribute]
GO

--Nop_Product_SpecificationAttribute_Mapping
IF  EXISTS (SELECT * FROM sys.foreign_keys 
	WHERE object_id = OBJECT_ID(N'[dbo].[FK_Nop_Product_SpecificationAttribute_Mapping_Nop_Product]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[Nop_Product_SpecificationAttribute_Mapping]'))
ALTER TABLE [dbo].[Nop_Product_SpecificationAttribute_Mapping] 
DROP CONSTRAINT [FK_Nop_Product_SpecificationAttribute_Mapping_Nop_Product]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys 
	WHERE object_id = OBJECT_ID(N'[dbo].[FK_Nop_Product_SpecificationAttribute_Mapping_Nop_SpecificationAttribute]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[Nop_Product_SpecificationAttribute_Mapping]'))
ALTER TABLE [dbo].[Nop_Product_SpecificationAttribute_Mapping] 
DROP CONSTRAINT [FK_Nop_Product_SpecificationAttribute_Mapping_Nop_SpecificationAttribute]
GO

IF  EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[dbo].[Nop_Product_SpecificationAttribute_Mapping]') 
	AND type in (N'U'))
DROP TABLE [dbo].[Nop_Product_SpecificationAttribute_Mapping]
GO
CREATE TABLE [dbo].[Nop_Product_SpecificationAttribute_Mapping](
	[ProductSpecificationAttributeID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[SpecificationAttributeOptionID] [int] NOT NULL,
	[AllowFiltering] [bit] NOT NULL CONSTRAINT [DF_Nop_Product_SpecificationAttribute_Mapping_AllowFiltering]  DEFAULT ((0)),
	[ShowOnProductPage] [bit] NOT NULL CONSTRAINT [DF_Nop_Product_SpecificationAttribute_Mapping_ShowOnProductPage]  DEFAULT ((1)),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_Nop_Product_SpecificationAttribute_Mapping_DisplayOrder]  DEFAULT ((1)),
 CONSTRAINT [PK_Nop_Product_SpecificationAttribute_Mapping] PRIMARY KEY CLUSTERED 
(
	[ProductSpecificationAttributeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO


IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_Product_SpecificationAttribute_Mapping_Nop_Product'
           AND parent_obj = Object_id('Nop_Product_SpecificationAttribute_Mapping')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_Product_SpecificationAttribute_Mapping
DROP CONSTRAINT FK_Nop_Product_SpecificationAttribute_Mapping_Nop_Product
GO
ALTER TABLE [dbo].[Nop_Product_SpecificationAttribute_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Nop_Product_SpecificationAttribute_Mapping_Nop_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Nop_Product] ([ProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Nop_Product_SpecificationAttribute_Mapping] CHECK CONSTRAINT [FK_Nop_Product_SpecificationAttribute_Mapping_Nop_Product]
GO

IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'FK_Nop_Product_SpecificationAttribute_Mapping_Nop_SpecificationAttributeOption'
           AND parent_obj = Object_id('Nop_Product_SpecificationAttribute_Mapping')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.Nop_Product_SpecificationAttribute_Mapping
DROP CONSTRAINT FK_Nop_Product_SpecificationAttribute_Mapping_Nop_SpecificationAttributeOption
GO
ALTER TABLE [dbo].[Nop_Product_SpecificationAttribute_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Nop_Product_SpecificationAttribute_Mapping_Nop_SpecificationAttributeOption] FOREIGN KEY([SpecificationAttributeOptionID])
REFERENCES [dbo].[Nop_SpecificationAttributeOption] ([SpecificationAttributeOptionID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Nop_Product_SpecificationAttribute_Mapping] CHECK CONSTRAINT [FK_Nop_Product_SpecificationAttribute_Mapping_Nop_SpecificationAttributeOption]
GO


--Nop_SpecificationAttributeLoadAll
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeLoadAll]

AS
BEGIN

	SET NOCOUNT ON
	
	SELECT *
	FROM [Nop_SpecificationAttribute]
	ORDER BY DisplayOrder
	
END
GO

--Nop_SpecificationAttributeInsert
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeInsert]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeInsert]
(
	@SpecificationAttributeID int = NULL output,
	@Name nvarchar(400),
	@DisplayOrder int
)
AS
BEGIN
	
	INSERT
	INTO [Nop_SpecificationAttribute]
	(
		[Name],
		DisplayOrder
	)
	VALUES
	(
		@Name,
		@DisplayOrder
	)

	SET @SpecificationAttributeID = SCOPE_IDENTITY()
	
END
GO

--Nop_SpecificationAttributeUpdate
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeUpdate]
(
	@SpecificationAttributeID int,
	@Name nvarchar(400),
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_SpecificationAttribute] SET
		[Name] = @Name,
		DisplayOrder = @DisplayOrder
	WHERE
		SpecificationAttributeID = @SpecificationAttributeID
END
GO

--Nop_SpecificationAttributeOptionLoadAll
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadAll]

AS
BEGIN

	SELECT *
	FROM Nop_SpecificationAttributeOption
	ORDER BY DisplayOrder
	
END
GO

--Nop_SpecificationAttributeOptionLoadByPrimaryKey
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLoadByPrimaryKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadByPrimaryKey]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadByPrimaryKey]

	@SpecificationAttributeOptionID int

AS
BEGIN

	SELECT *
	FROM Nop_SpecificationAttributeOption
	WHERE SpecificationAttributeOptionID = @SpecificationAttributeOptionID

END
GO

--Nop_SpecificationAttributeOptionLoadBySpecificationAttributeID
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionLoadBySpecificationAttributeID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadBySpecificationAttributeID]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionLoadBySpecificationAttributeID]

	@SpecificationAttributeID int

AS
BEGIN

	SELECT *
	FROM Nop_SpecificationAttributeOption 
	WHERE SpecificationAttributeID = @SpecificationAttributeID
	ORDER BY DisplayOrder

END
GO

--Nop_SpecificationAttributeOptionInsert
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionInsert]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionInsert]

	@SpecificationAttributeOptionID int = NULL OUTPUT,
	@SpecificationAttributeID int,
	@Name nvarchar(500),
	@DisplayOrder int

AS
BEGIN

	INSERT INTO [Nop_SpecificationAttributeOption] (SpecificationAttributeID, Name, DisplayOrder)
	VALUES (@SpecificationAttributeID, @Name, @DisplayOrder)
	
	SET @SpecificationAttributeOptionID = SCOPE_IDENTITY()

END
GO

--Nop_SpecificationAttributeOptionUpdate
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionUpdate]

	@SpecificationAttributeOptionID int,
	@SpecificationAttributeID int,
	@Name nvarchar(500),
	@DisplayOrder int

AS
BEGIN

	UPDATE Nop_SpecificationAttributeOption SET
		SpecificationAttributeID = @SpecificationAttributeID,
		Name = @Name,
		DisplayOrder = @DisplayOrder
	WHERE SpecificationAttributeOptionID = @SpecificationAttributeOptionID

END
GO

--Nop_SpecificationAttributeOptionDelete
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionDelete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionDelete]
GO
CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionDelete]

	@SpecificationAttributeOptionID int

AS
BEGIN

	DELETE 
	FROM Nop_SpecificationAttributeOption
	WHERE SpecificationAttributeOptionID = @SpecificationAttributeOptionID

END
GO

--Nop_Product_SpecificationAttribute_MappingLoadByProductID
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Product_SpecificationAttribute_MappingLoadByProductID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingLoadByProductID]
GO
CREATE PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingLoadByProductID]
(
	@ProductID int,
	@AllowFiltering bit,
	@ShowOnProductPage bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		psam.*
	FROM Nop_Product_SpecificationAttribute_Mapping psam
	WHERE psam.ProductID = @ProductID
		AND (@AllowFiltering IS NULL OR psam.AllowFiltering=@AllowFiltering)
		AND (@ShowOnProductPage IS NULL OR psam.ShowOnProductPage=@ShowOnProductPage)
	ORDER BY psam.DisplayOrder
END
GO

--Nop_Product_SpecificationAttribute_MappingInsert
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Product_SpecificationAttribute_MappingInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingInsert]
GO
CREATE PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingInsert]
(
	@ProductSpecificationAttributeID int = NULL output,
	@ProductID int,	
	@SpecificationAttributeOptionID int,
	@AllowFiltering bit,
	@ShowOnProductPage bit,
	@DisplayOrder int
)
AS
BEGIN

	INSERT	
	INTO [Nop_Product_SpecificationAttribute_Mapping]
	(
		ProductID,
		SpecificationAttributeOptionID,
		AllowFiltering,
		ShowOnProductPage,
		DisplayOrder
	)
	VALUES
	(
		@ProductID,
		@SpecificationAttributeOptionID,
		@AllowFiltering,
		@ShowOnProductPage,
		@DisplayOrder
	)

	SET @ProductSpecificationAttributeID = SCOPE_IDENTITY()
	
END
GO

--Nop_Product_SpecificationAttribute_MappingUpdate
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Product_SpecificationAttribute_MappingUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_Product_SpecificationAttribute_MappingUpdate]
(
	@ProductSpecificationAttributeID int,
	@ProductID int,	
	@SpecificationAttributeOptionID int,
	@AllowFiltering bit,
	@ShowOnProductPage bit,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_Product_SpecificationAttribute_Mapping]
	SET
		ProductID = @ProductID,
		SpecificationAttributeOptionID = @SpecificationAttributeOptionID,
		AllowFiltering = @AllowFiltering,
		ShowOnProductPage = @ShowOnProductPage,
		DisplayOrder=@DisplayOrder
	WHERE
		ProductSpecificationAttributeID = @ProductSpecificationAttributeID

END
GO

--fix for sales report stored procedure
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SalesBestSellersReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SalesBestSellersReport]
GO
CREATE PROCEDURE [dbo].[Nop_SalesBestSellersReport]
(
	@LastDays int = 360,
	@RecordsToReturn int = 10,
	@OrderBy int = 1
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @cmd varchar(500)
	
	CREATE TABLE #tmp (
		ID int not null identity,
		ProductVariantID int,
		SalesTotalCount int,
		SalesTotalAmount money)
	INSERT #tmp (
		ProductVariantID,
		SalesTotalCount,
		SalesTotalAmount)
	SELECT 
		s.ProductVariantID,
		s.SalesTotalCount, 
		s.SalesTotalAmount 
	FROM (SELECT opv.ProductVariantID, SUM(opv.Quantity) AS SalesTotalCount, SUM(opv.PriceExclTax) AS SalesTotalAmount
		  FROM [Nop_OrderProductVariant] opv
				INNER JOIN [Nop_Order] o on opv.OrderID = o.OrderID 
				WHERE o.CreatedOn >= dateadd(dy, -@LastDays, getdate())
				AND o.Deleted=0
		  GROUP BY opv.ProductVariantID 
		 ) s
		INNER JOIN [Nop_ProductVariant] pv with (nolock) on s.ProductVariantID = pv.ProductVariantID
		INNER JOIN [Nop_Product] p with (nolock) on pv.ProductID = p.ProductID
	WHERE p.Deleted = 0 
		AND p.Published = 1  
		AND pv.Published = 1 
		AND pv.Deleted = 0
	ORDER BY case @OrderBy when 1 then s.SalesTotalCount when 2 then s.SalesTotalAmount else s.SalesTotalCount end desc

	SET @cmd = 'SELECT TOP ' + convert(varchar(10), @RecordsToReturn ) + ' * FROM #tmp Order By ID'

	EXEC (@cmd)

	DROP TABLE #tmp
END
GO

--exchange rates (4 digits)
ALTER TABLE [dbo].[Nop_Currency]
ALTER COLUMN [Rate] decimal(18, 4) NOT NULL
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CurrencyInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE [dbo].[Nop_CurrencyInsert]
END
GO
CREATE PROCEDURE [dbo].[Nop_CurrencyInsert]
(
	@CurrencyID int = NULL output,
	@Name nvarchar(50),
	@CurrencyCode nvarchar(5),
	@Rate decimal (18, 4),
	@DisplayLocale nvarchar(50),
	@CustomFormatting nvarchar(50),
	@Published bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Currency]
	(
		[Name],
		CurrencyCode,
		Rate,
		DisplayLocale,
		CustomFormatting,
		Published,
		DisplayOrder,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@Name,
		@CurrencyCode,
		@Rate,
		@DisplayLocale,
		@CustomFormatting,
		@Published,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @CurrencyID=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CurrencyUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE [dbo].[Nop_CurrencyUpdate]
END
GO

CREATE PROCEDURE [dbo].[Nop_CurrencyUpdate]
(
	@CurrencyID int,
	@Name nvarchar(50),
	@CurrencyCode nvarchar(5),
	@Rate decimal (18, 4),
	@DisplayLocale nvarchar(50),
	@CustomFormatting nvarchar(50),
	@Published bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN

	UPDATE [Nop_Currency]
	SET
		[Name]=@Name,
		CurrencyCode=@CurrencyCode,
		Rate=@Rate,
		DisplayLocale=@DisplayLocale,
		CustomFormatting=@CustomFormatting,
		Published=@Published,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		CurrencyID = @CurrencyID

END
GO


--Load customers stored procedure. Filter guest customers
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_CustomerLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_CustomerLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_CustomerLoadAll]
(
	@StartTime				datetime = NULL,
	@EndTime				datetime = NULL,
	@Email					nvarchar(200),
	@Username				nvarchar(200),
	@DontLoadGuestCustomers	bit = 0,
	@PageIndex				int = 0, 
	@PageSize				int = 2147483644,
	@TotalRecords			int = null OUTPUT
)
AS
BEGIN

	SET @Email = isnull(@Email, '')
	SET @Email = '%' + rtrim(ltrim(@Email)) + '%'

	SET @Username = isnull(@Username, '')
	SET @Username = '%' + rtrim(ltrim(@Username)) + '%'


	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	DECLARE @TotalThreads int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		CustomerID int NOT NULL,
		RegistrationDate datetime NOT NULL,
	)

	INSERT INTO #PageIndex (CustomerID, RegistrationDate)
	SELECT DISTINCT
		c.CustomerID, c.RegistrationDate
	FROM [Nop_Customer] c with (NOLOCK)
	WHERE 
		(@StartTime is NULL or DATEDIFF(day, @StartTime, c.RegistrationDate) >= 0) and
		(@EndTime is NULL or DATEDIFF(day, @EndTime, c.RegistrationDate) <= 0) and 
		(patindex(@Email, isnull(c.Email, '')) > 0) and
		(patindex(@Username, isnull(c.Username, '')) > 0) and
		(@DontLoadGuestCustomers = 0 or (c.IsGuest=0)) and 
		c.deleted=0
	order by c.RegistrationDate desc 

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		c.*
	FROM
		#PageIndex [pi]
		INNER JOIN [Nop_Customer] c on c.CustomerID = [pi].CustomerID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
	
END
GO

--froogle handler

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Froogle.AllowPublicFroogleAccess')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Froogle.AllowPublicFroogleAccess', N'false', N'')
END
GO

--order report bug
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantReport]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantReport]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantReport]
(
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@OrderStatusID int,
	@PaymentStatusID int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT DISTINCT opv.ProductVariantID,
		(	
			select sum(opv2.PriceExclTax)
			from Nop_OrderProductVariant opv2
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or DATEDIFF(day, @StartTime, o2.CreatedOn) >= 0) and
				(@EndTime is NULL or DATEDIFF(day, @EndTime, o2.CreatedOn) <= 0) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) PriceExclTax, 
		(
			select sum(opv2.Quantity)  
			from Nop_OrderProductVariant opv2 
			INNER JOIN [Nop_Order] o2 
			on o2.OrderId = opv2.OrderID 
			where
				(@StartTime is NULL or DATEDIFF(day, @StartTime, o2.CreatedOn) >= 0) and
				(@EndTime is NULL or DATEDIFF(day, @EndTime, o2.CreatedOn) <= 0) and 
				(@OrderStatusID IS NULL or @OrderStatusID=0 or o2.OrderStatusID = @OrderStatusID) and
				(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o2.PaymentStatusID = @PaymentStatusID) and
				(o2.Deleted=0) and 
				(opv2.ProductVariantID = opv.ProductVariantID)) Total 
	FROM Nop_OrderProductVariant opv 
	INNER JOIN [Nop_Order] o 
	on o.OrderId = opv.OrderID
	WHERE
		(@StartTime is NULL or DATEDIFF(day, @StartTime, o.CreatedOn) >= 0) and
		(@EndTime is NULL or DATEDIFF(day, @EndTime, o.CreatedOn) <= 0) and 
		(@OrderStatusID IS NULL or @OrderStatusID=0 or o.OrderStatusID = @OrderStatusID) and
		(@PaymentStatusID IS NULL or @PaymentStatusID=0 or o.PaymentStatusID = @PaymentStatusID) and
		(o.Deleted=0)

END
GO


-- Determines whether to show best selling products on home page
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Display.ShowBestsellersOnMainPage')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Display.ShowBestsellersOnMainPage', N'false', N'Determines whether to show best sellers on the home page')
END
GO

--further private messages changes

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Forums_PrivateMessage]') and NAME='IsDeletedByAuthor')
BEGIN
	ALTER TABLE [dbo].[Nop_Forums_PrivateMessage] 
	ADD IsDeletedByAuthor bit NOT NULL CONSTRAINT [DF_Nop_Forums_PrivateMessage_IsDeletedByAuthor] DEFAULT ((0))
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_Forums_PrivateMessage]') and NAME='IsDeletedByRecipient')
BEGIN
	ALTER TABLE [dbo].[Nop_Forums_PrivateMessage] 
	ADD IsDeletedByRecipient bit NOT NULL CONSTRAINT [DF_Nop_Forums_PrivateMessage_IsDeletedByRecipient] DEFAULT ((0))
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PrivateMessageInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PrivateMessageInsert]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageInsert]
(
	@PrivateMessageID int = NULL output,
	@FromUserID int,
	@ToUserID int,
	@Subject nvarchar(450),
	@Text nvarchar(max),
	@IsRead bit,
	@IsDeletedByAuthor bit,
	@IsDeletedByRecipient bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Forums_PrivateMessage]
	(
		[FromUserID],
		[ToUserID],
		[Subject],
		[Text],
		[IsRead],
		[IsDeletedByAuthor],
		[IsDeletedByRecipient],
		[CreatedOn]
	)
	VALUES
	(
		@FromUserID,
		@ToUserID,
		@Subject,
		@Text,
		@IsRead,
		@IsDeletedByAuthor,
		@IsDeletedByRecipient,
		@CreatedOn
	)

	set @PrivateMessageID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PrivateMessageLoadAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PrivateMessageLoadAll]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageLoadAll]
(
	@FromUserID			int,
	@ToUserID			int,
	@IsRead				bit = null,	--0 not read only, 1 read only, null - load all messages
	@IsDeletedByAuthor		bit = null,	--0 deleted by author only, 1 not deleted by author only, null - load all messages
	@IsDeletedByRecipient	bit = null,	--0 deleted by recipient only, 1 not deleted by recipient, null - load all messages
	@Keywords			nvarchar(MAX),
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		PrivateMessageID int NOT NULL,
		CreatedOn datetime NOT NULL,
	)

	INSERT INTO #PageIndex (PrivateMessageID, CreatedOn)
	SELECT DISTINCT
		fpm.PrivateMessageID, fpm.CreatedOn
	FROM Nop_Forums_PrivateMessage fpm with (NOLOCK)
	WHERE   (
				@FromUserID IS NULL OR @FromUserID=0
				OR (fpm.FromUserID=@FromUserID)
			)
		AND (
				@ToUserID IS NULL OR @ToUserID=0
				OR (fpm.ToUserID=@ToUserID)
			)
		AND (
				@IsRead IS NULL OR fpm.IsRead=@IsRead
			)
		AND (
				@IsDeletedByAuthor IS NULL OR fpm.IsDeletedByAuthor=@IsDeletedByAuthor
			)
		AND (
				@IsDeletedByRecipient IS NULL OR fpm.IsDeletedByRecipient=@IsDeletedByRecipient
			)
		AND	(
				(patindex(@Keywords, isnull(fpm.Subject, '')) > 0)
				or (patindex(@Keywords, isnull(fpm.Text, '')) > 0)
			)
	ORDER BY fpm.CreatedOn desc, fpm.PrivateMessageID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		fpm.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_PrivateMessage fpm on fpm.PrivateMessageID = [pi].PrivateMessageID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_Forums_PrivateMessageUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_Forums_PrivateMessageUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageUpdate]
(
	@PrivateMessageID int,
	@FromUserID int,
	@ToUserID int,
	@Subject nvarchar(450),
	@Text nvarchar(max),
	@IsRead bit,
	@IsDeletedByAuthor bit,
	@IsDeletedByRecipient bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_PrivateMessage]
	SET
		[FromUserID]=@FromUserID,
		[ToUserID]=@ToUserID,
		[Subject]=@Subject,
		[Text]=@Text,
		[IsRead]=@IsRead,
		[IsDeletedByAuthor]=@IsDeletedByAuthor,
		[IsDeletedByRecipient]=@IsDeletedByRecipient,
		[CreatedOn]=@CreatedOn
	WHERE
		PrivateMessageID = @PrivateMessageID
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Messaging.AllowPM')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Messaging.AllowPM', N'false', N'Determines whether to allow private messages')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Messaging.PMSubjectMaxLength')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Messaging.PMSubjectMaxLength', N'150', N'')
END
GO



IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Messaging.PMTextMaxLength')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Messaging.PMTextMaxLength', N'0', N'')
END
GO




-- further product filter changes
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_SpecificationAttributeOptionFilter_LoadByFilter]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_SpecificationAttributeOptionFilter_LoadByFilter]
GO

CREATE PROCEDURE [dbo].[Nop_SpecificationAttributeOptionFilter_LoadByFilter]
(
	@CategoryID int
)
AS
BEGIN
	SELECT 
		sa.SpecificationAttributeID,
		sa.Name 'SpecificationAttributeName',
		sa.DisplayOrder,
		sao.SpecificationAttributeOptionID,
		sao.Name 'SpecificationAttributeOptionName'
	FROM Nop_Product_SpecificationAttribute_Mapping psam with (NOLOCK)
		INNER JOIN Nop_SpecificationAttributeOption sao with (NOLOCK) ON
			sao.SpecificationAttributeOptionID = psam.SpecificationAttributeOptionID
		INNER JOIN Nop_SpecificationAttribute sa with (NOLOCK) ON
			sa.SpecificationAttributeID = sao.SpecificationAttributeID	
		INNER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON 
			pcm.ProductID = psam.ProductID	
		INNER JOIN Nop_Product p ON 
			psam.ProductID = p.ProductID
		LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON 
			p.ProductID = pv.ProductID
	WHERE 
			p.Published = 1
		AND 
			pv.Published = 1
		AND 
			p.Deleted=0
		AND
			pcm.CategoryID = @CategoryID
		AND
			psam.AllowFiltering = 1
		AND
			getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999')
	GROUP BY
		sa.SpecificationAttributeID, 
		sa.Name,
		sa.DisplayOrder,
		sao.SpecificationAttributeOptionID,
		sao.Name
	ORDER BY sa.DisplayOrder, sao.Name
END
GO




IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[NOP_splitstring_to_table]') AND xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[NOP_splitstring_to_table]
GO
CREATE FUNCTION [dbo].[NOP_splitstring_to_table]
(
    @string NVARCHAR(1000),
    @delimiter CHAR(1)
)
RETURNS @output TABLE(
    data NVARCHAR(256)
)
BEGIN
    DECLARE @start INT, @end INT
    SELECT @start = 1, @end = CHARINDEX(@delimiter, @string)

    WHILE @start < LEN(@string) + 1 BEGIN
        IF @end = 0 
            SET @end = LEN(@string) + 1

        INSERT INTO @output (data) 
        VALUES(SUBSTRING(@string, @start, @end - @start))
        SET @start = @end + 1
        SET @end = CHARINDEX(@delimiter, @string, @start)
    END
    RETURN
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductLoadAllPaged]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
GO
CREATE PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
(
	@CategoryID			int = 0,
	@ManufacturerID		int = 0,
	@FeaturedProducts	bit = null,	--0 featured only , 1 not featured only, null - load all products
	@PriceMin			money = null,
	@PriceMax			money = null,
	@Keywords			nvarchar(MAX),	
	@SearchDescriptions bit = 0,
	@ShowHidden			bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@FilteredSpecs		nvarchar(300) = null,	--filter by attributes (comma-separated list). e.g. 14,15,16
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--init
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	SET @PriceMin = isnull(@PriceMin, 0)
	SET @PriceMax = isnull(@PriceMax, 2147483644)

	--display order
	CREATE TABLE #DisplayOrder
	(
		ProductID int not null PRIMARY KEY,
		DisplayOrder int not null
	)	

	IF @CategoryID IS NOT NULL AND @CategoryID > 0
		BEGIN
			INSERT #DisplayOrder 
			SELECT pcm.ProductID, pcm.DisplayOrder 
			FROM [Nop_Product_Category_Mapping] pcm WHERE pcm.CategoryID = @CategoryID
		END
    ELSE IF @ManufacturerID IS NOT NULL AND @ManufacturerID > 0
		BEGIN
			INSERT #DisplayOrder 
			SELECT pmm.ProductID, pmm.Displayorder 
			FROM [Nop_Product_Manufacturer_Mapping] pmm WHERE pmm.ManufacturerID = @ManufacturerID
		END
	ELSE
		BEGIN
			INSERT #DisplayOrder 
			SELECT p.ProductID, 1 
			FROM [Nop_Product] p
			ORDER BY p.[Name]
		END
	
	--filter by attributes
	SET @FilteredSpecs = isnull(@FilteredSpecs, '')
	CREATE TABLE #FilteredSpecs
	(
		SpecificationAttributeOptionID int not null
	)
	INSERT INTO #FilteredSpecs (SpecificationAttributeOptionID)
	SELECT CAST(data as int) FROM dbo.[NOP_splitstring_to_table](@FilteredSpecs, ',');
	
	DECLARE @SpecAttributesCount int	
	SELECT @SpecAttributesCount = COUNT(1) FROM #FilteredSpecs

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ProductID int NOT NULL,
		DisplayOrder int NOT NULL,
	)
	INSERT INTO #PageIndex (ProductID, DisplayOrder)
	SELECT DISTINCT p.ProductID, do.DisplayOrder
	FROM Nop_Product p with (NOLOCK) 
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	LEFT OUTER JOIN Nop_Product_Manufacturer_Mapping pmm with (NOLOCK) ON p.ProductID=pmm.ProductID
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
	JOIN #DisplayOrder do on p.ProductID = do.ProductID
	WHERE 
		(
			(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				@ShowHidden = 1 OR pv.Published = 1
			)
		AND 
			(
				p.Deleted=0
			)
		AND (
				@CategoryID IS NULL OR @CategoryID=0
				OR (pcm.CategoryID=@CategoryID AND (@FeaturedProducts IS NULL OR pcm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ManufacturerID IS NULL OR @ManufacturerID=0
				OR (pmm.ManufacturerID=@ManufacturerID AND (@FeaturedProducts IS NULL OR pmm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				pv.Price BETWEEN @PriceMin AND @PriceMax
			)
		AND	(
				patindex(@Keywords, isnull(p.name, '')) > 0
				or patindex(@Keywords, isnull(pv.name, '')) > 0
				or patindex(@Keywords, isnull(pv.sku , '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pv.Description, '')) > 0)
			)
		AND
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
			)
		AND
			(
				--filter by specs
				@SpecAttributesCount = 0
				OR
				(
					NOT EXISTS(
						SELECT 1 
						FROM #FilteredSpecs [fs]
						WHERE [fs].SpecificationAttributeOptionID NOT IN (
							SELECT psam.SpecificationAttributeOptionID
							FROM dbo.Nop_Product_SpecificationAttribute_Mapping psam
							WHERE psam.AllowFiltering = 1 AND psam.ProductID = p.ProductID
							)
						)
					
				)
			)
		)
	ORDER BY do.DisplayOrder

	--total records
	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	--return
	SELECT  
		p.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #DisplayOrder
	DROP TABLE #FilteredSpecs
	DROP TABLE #PageIndex
END
GO



--FedEx shipping rate computation method
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_ShippingRateComputationMethod]
		WHERE [ClassName] = N'NopSolutions.NopCommerce.Shipping.Methods.FedEx.FedExComputationMethod, Nop.Shipping.FedEx')
BEGIN
	INSERT [dbo].[Nop_ShippingRateComputationMethod] 
	([Name], [Description], [ConfigureTemplatePath], [ClassName], [DisplayOrder]) 
	VALUES (N'FedEx', N'', N'Shipping\FedExConfigure\ConfigureShipping.ascx', N'NopSolutions.NopCommerce.Shipping.Methods.FedEx.FedExComputationMethod, Nop.Shipping.FedEx', 25)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.URL')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.URL', N'https://gatewaybeta.fedex.com:443/web-services/rate', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.Key')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.Key', N'', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.Password')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.Password', N'', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.AccountNumber')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.AccountNumber', N'', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.MeterNumber')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.MeterNumber', N'', N'')
END
GO


IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.ShippingOrigin.Street')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.ShippingOrigin.Street', N'Sender Address Line 1', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.ShippingOrigin.City')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.ShippingOrigin.City', N'Memphis', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.ShippingOrigin.StateOrProvinceCode')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.ShippingOrigin.StateOrProvinceCode', N'TN', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.ShippingOrigin.PostalCode')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.ShippingOrigin.PostalCode', N'38115', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'ShippingRateComputationMethod.FedEx.ShippingOrigin.CountryCode')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'ShippingRateComputationMethod.FedEx.ShippingOrigin.CountryCode', N'US', N'')
END
GO




--eWay (UK) payment gateway
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_PaymentMethod]
		WHERE [ClassName] = N'NopSolutions.NopCommerce.Payment.Methods.eWayUK.eWayPaymentProcessor, Nop.Payment.eWayUK')
BEGIN
	INSERT [dbo].[Nop_PaymentMethod] ([Name], [VisibleName], [Description], [ConfigureTemplatePath], [UserTemplatePath], [ClassName], [SystemKeyword], [IsActive], [DisplayOrder]) 
	VALUES (N'eWay (UK)', N'Credit Card', N'', N'Payment\eWayUK\ConfigurePaymentMethod.ascx', N'~\Templates\Payment\eWayUK\PaymentModule.ascx', N'NopSolutions.NopCommerce.Payment.Methods.eWayUK.eWayPaymentProcessor, Nop.Payment.eWayUK', N'EWAYUK', 0, 100)
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'PaymentMethod.eWayUK.CustomerID')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'PaymentMethod.eWayUK.CustomerID', N'87654321', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'PaymentMethod.eWayUK.Username')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'PaymentMethod.eWayUK.Username', N'TestAccount', N'')
END
GO

IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'PaymentMethod.eWayUK.PaymentPage')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'PaymentMethod.eWayUK.PaymentPage', N'https://payment.ewaygateway.com/', N'')
END
GO



-- fixed [Nop_OrderNoteLoadByOrderID] stored procedure
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderNoteLoadByOrderID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderNoteLoadByOrderID]
GO
CREATE PROCEDURE [dbo].[Nop_OrderNoteLoadByOrderID]
(
	@OrderID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderNote]
	WHERE
		OrderID=@OrderID
	ORDER BY CreatedOn desc, OrderNoteID
END
GO


--moved "textbox" (requires text prompt) option of product variants to attrbiutes
IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ShoppingCartItem]') and NAME='TextOption')
BEGIN
	exec('DELETE FROM [dbo].[Nop_ShoppingCartItem]')
	
	ALTER TABLE [dbo].[Nop_ShoppingCartItem] 
	DROP COLUMN TextOption
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ShoppingCartItem]') and NAME='AttributeIDs')
BEGIN
	exec('DELETE FROM [dbo].[Nop_ShoppingCartItem]')
	
	ALTER TABLE [dbo].[Nop_ShoppingCartItem] 
	DROP COLUMN AttributeIDs
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ShoppingCartItem]') and NAME='AttributesXML')
BEGIN
	ALTER TABLE [dbo].[Nop_ShoppingCartItem] 
	ADD AttributesXML XML NOT NULL CONSTRAINT [DF_Nop_ShoppingCartItem_AttributesXML] DEFAULT ((''))
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_OrderProductVariant]') and NAME='TextOption')
BEGIN

	exec('UPDATE [dbo].[Nop_OrderProductVariant] 
		SET AttributeDescription=AttributeDescription + ''<br />'' + TextOption
		WHERE TextOption <> ''''')

	ALTER TABLE [dbo].[Nop_OrderProductVariant] 
	DROP COLUMN TextOption
END
GO

IF NOT EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant_ProductAttribute_Mapping]') and NAME='TextPrompt')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant_ProductAttribute_Mapping] 
	ADD TextPrompt nvarchar(200) NOT NULL CONSTRAINT [DF_Nop_ProductVariant_ProductAttribute_Mapping_Attributes] DEFAULT ((''))
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='RequiresTextOption')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	DROP COLUMN RequiresTextOption
END
GO

IF EXISTS (
		SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Nop_ProductVariant]') and NAME='TextOptionPrompt')
BEGIN
	ALTER TABLE [dbo].[Nop_ProductVariant] 
	DROP COLUMN TextOptionPrompt
END
GO



IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ShoppingCartItemInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ShoppingCartItemInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemInsert]
(
	@ShoppingCartItemID int = NULL output,
	@ShoppingCartTypeID int,
	@CustomerSessionGUID uniqueidentifier,
	@ProductVariantID int,
	@AttributesXML XML,
	@Quantity int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_ShoppingCartItem]
	(
		ShoppingCartTypeID,
		CustomerSessionGUID,
		ProductVariantID,
		AttributesXML,
		Quantity,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@ShoppingCartTypeID,
		@CustomerSessionGUID,
		@ProductVariantID,
		@AttributesXML,
		@Quantity,
		@CreatedOn,
		@UpdatedOn
	)

	set @ShoppingCartItemID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ShoppingCartItemUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ShoppingCartItemUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemUpdate]
(
	@ShoppingCartItemID int,
	@ShoppingCartTypeID int,
	@CustomerSessionGUID uniqueidentifier,
	@ProductVariantID int,
	@AttributesXML XML,
	@Quantity int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ShoppingCartItem]
	SET
			ShoppingCartTypeID=@ShoppingCartTypeID,
			CustomerSessionGUID=@CustomerSessionGUID,
			ProductVariantID=@ProductVariantID,	
			AttributesXML=@AttributesXML,
			Quantity=@Quantity,
			CreatedOn=@CreatedOn,
			UpdatedOn=@UpdatedOn
	WHERE
		ShoppingCartItemID = @ShoppingCartItemID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantInsert]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantInsert]
(
	@OrderProductVariantID int = NULL output,
	@OrderID int,
	@ProductVariantID int,
	@UnitPriceInclTax money,
	@UnitPriceExclTax money,
	@PriceInclTax money,
	@PriceExclTax money,
	@UnitPriceInclTaxInCustomerCurrency money,
	@UnitPriceExclTaxInCustomerCurrency money,
	@PriceInclTaxInCustomerCurrency money,
	@PriceExclTaxInCustomerCurrency money,
	@AttributeDescription nvarchar(4000),
	@Quantity int,
	@DiscountAmountInclTax decimal (18, 4),
	@DiscountAmountExclTax decimal (18, 4),
	@DownloadCount int
)
AS
BEGIN
	INSERT
	INTO [Nop_OrderProductVariant]
	(
		OrderID,
		ProductVariantID,
		UnitPriceInclTax,
		UnitPriceExclTax,
		PriceInclTax,
		PriceExclTax,
		UnitPriceInclTaxInCustomerCurrency,
		UnitPriceExclTaxInCustomerCurrency,
		PriceInclTaxInCustomerCurrency,
		PriceExclTaxInCustomerCurrency,
		AttributeDescription,
		Quantity,
		DiscountAmountInclTax,
		DiscountAmountExclTax,
		DownloadCount
	)
	VALUES
	(
		@OrderID,
		@ProductVariantID,
		@UnitPriceInclTax,
		@UnitPriceExclTax,
		@PriceInclTax,
		@PriceExclTax,
		@UnitPriceInclTaxInCustomerCurrency,
		@UnitPriceExclTaxInCustomerCurrency,
		@PriceInclTaxInCustomerCurrency,
		@PriceExclTaxInCustomerCurrency,
		@AttributeDescription,
		@Quantity,
		@DiscountAmountInclTax,
		@DiscountAmountExclTax,
		@DownloadCount
	)

	set @OrderProductVariantID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_OrderProductVariantUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_OrderProductVariantUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_OrderProductVariantUpdate]
(
	@OrderProductVariantID int,
	@OrderID int,
	@ProductVariantID int,
	@UnitPriceInclTax money,
	@UnitPriceExclTax money,
	@PriceInclTax money,
	@PriceExclTax money,
	@UnitPriceInclTaxInCustomerCurrency money,
	@UnitPriceExclTaxInCustomerCurrency money,
	@PriceInclTaxInCustomerCurrency money,
	@PriceExclTaxInCustomerCurrency money,
	@AttributeDescription nvarchar(4000),
	@Quantity int,
	@DiscountAmountInclTax decimal (18, 4),
	@DiscountAmountExclTax decimal (18, 4),
	@DownloadCount int
)
AS
BEGIN

	UPDATE [Nop_OrderProductVariant]
	SET		
		OrderID=@OrderID,
		ProductVariantID=@ProductVariantID,
		UnitPriceInclTax=@UnitPriceInclTax,
		UnitPriceExclTax = @UnitPriceExclTax,
		PriceInclTax=@PriceInclTax,
		PriceExclTax=@PriceExclTax,
		UnitPriceInclTaxInCustomerCurrency=@UnitPriceInclTaxInCustomerCurrency,
		UnitPriceExclTaxInCustomerCurrency=@UnitPriceExclTaxInCustomerCurrency,
		PriceInclTaxInCustomerCurrency=@PriceInclTaxInCustomerCurrency,
		PriceExclTaxInCustomerCurrency=@PriceExclTaxInCustomerCurrency,
		AttributeDescription=@AttributeDescription,
		Quantity=@Quantity,
		DiscountAmountInclTax=@DiscountAmountInclTax,
		DiscountAmountExclTax=@DiscountAmountExclTax,
		DownloadCount=@DownloadCount
	WHERE
		OrderProductVariantID = @OrderProductVariantID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariant_ProductAttribute_MappingInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariant_ProductAttribute_MappingInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariant_ProductAttribute_MappingInsert]
(
	@ProductVariantAttributeID int = NULL output,
	@ProductVariantID int,
	@ProductAttributeID int,
	@TextPrompt nvarchar(200),
	@IsRequired bit,
	@AttributeControlTypeID int,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductVariant_ProductAttribute_Mapping]
	(
		ProductVariantID,
		ProductAttributeID,
		TextPrompt,
		IsRequired,
		AttributeControlTypeID,
		DisplayOrder
	)
	VALUES
	(
		@ProductVariantID,
		@ProductAttributeID,
		@TextPrompt,
		@IsRequired,
		@AttributeControlTypeID,
		@DisplayOrder
	)

	set @ProductVariantAttributeID=@@identity
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariant_ProductAttribute_MappingUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariant_ProductAttribute_MappingUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariant_ProductAttribute_MappingUpdate]
(
	@ProductVariantAttributeID int,
	@ProductVariantID int,
	@ProductAttributeID int,
	@TextPrompt nvarchar(200),
	@IsRequired bit,
	@AttributeControlTypeID int,
	@DisplayOrder int
)
AS
BEGIN

	UPDATE [Nop_ProductVariant_ProductAttribute_Mapping]
	SET
		ProductVariantID=@ProductVariantID,
		ProductAttributeID=@ProductAttributeID,
		TextPrompt=@TextPrompt,
		IsRequired=@IsRequired,
		AttributeControlTypeID=@AttributeControlTypeID,
		DisplayOrder=@DisplayOrder
	WHERE
		ProductVariantAttributeID = @ProductVariantAttributeID
END
GO

IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantInsert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantInsert]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantInsert]
(
    @ProductVariantID int = NULL output,
    @ProductId int,
    @Name nvarchar(400),
    @SKU nvarchar (400),
    @Description nvarchar(4000),
    @AdminComment nvarchar(4000),
    @ManufacturerPartNumber nvarchar(100),
    @IsDownload bit,
    @DownloadID int,
	@UnlimitedDownloads bit,
	@MaxNumberOfDownloads int,
	@HasSampleDownload bit,
	@SampleDownloadID int,
    @IsShipEnabled bit,
    @IsFreeShipping bit,
	@AdditionalShippingCharge money,
    @IsTaxExempt bit,
    @TaxCategoryID int,
	@ManageInventory bit,
    @StockQuantity int,
    @MinStockQuantity int,
    @LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
    @WarehouseId int,
    @DisableBuyButton int,
    @Price money,
    @OldPrice money,
    @Weight float,
    @Length decimal(18, 4),
    @Width decimal(18, 4),
    @Height decimal(18, 4),
    @PictureID int,
	@AvailableStartDateTime datetime,
	@AvailableEndDateTime datetime,
    @Published bit,
    @Deleted bit,
    @DisplayOrder int,
	@CreatedOn datetime,
    @UpdatedOn datetime
)
AS
BEGIN
    INSERT
    INTO [Nop_ProductVariant]
    (
        ProductId,
        [Name],
        SKU,
        [Description],
        AdminComment,
        ManufacturerPartNumber,
        IsDownload,
        DownloadID,
		UnlimitedDownloads,
		MaxNumberOfDownloads,
		HasSampleDownload,
		SampleDownloadID,
        IsShipEnabled,
        IsFreeShipping,
		AdditionalShippingCharge,
        IsTaxExempt,
        TaxCategoryID,
		ManageInventory,
        StockQuantity,
        MinStockQuantity,
        LowStockActivityID,
		NotifyAdminForQuantityBelow,
		OrderMinimumQuantity,
		OrderMaximumQuantity,
        WarehouseId,
        DisableBuyButton,
        Price,
        OldPrice,
        Weight,
        [Length],
        Width,
        Height,
        PictureID,
		AvailableStartDateTime,
		AvailableEndDateTime,
        Published,
        Deleted,
        DisplayOrder,
        CreatedOn,
        UpdatedOn
    )
    VALUES
    (
        @ProductId,
        @Name,
        @SKU,
        @Description,
        @AdminComment,
        @ManufacturerPartNumber,
        @IsDownload,
        @DownloadID,
		@UnlimitedDownloads,
		@MaxNumberOfDownloads,
		@HasSampleDownload,
		@SampleDownloadID,
        @IsShipEnabled,
        @IsFreeShipping,
		@AdditionalShippingCharge,
        @IsTaxExempt,
        @TaxCategoryID,
		@ManageInventory,
        @StockQuantity,
        @MinStockQuantity,
        @LowStockActivityID,
		@NotifyAdminForQuantityBelow,
		@OrderMinimumQuantity,
		@OrderMaximumQuantity,
        @WarehouseId,
        @DisableBuyButton,
        @Price,
        @OldPrice,
        @Weight,
        @Length,
        @Width,
        @Height,
        @PictureID,
		@AvailableStartDateTime,
		@AvailableEndDateTime,
        @Published,
        @Deleted,
        @DisplayOrder,
        @CreatedOn,
        @UpdatedOn
    )

    set @ProductVariantID=@@identity
END
GO


IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[Nop_ProductVariantUpdate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Nop_ProductVariantUpdate]
GO
CREATE PROCEDURE [dbo].[Nop_ProductVariantUpdate]
(
	@ProductVariantID int,
	@ProductId int,
	@Name nvarchar(400),
	@SKU nvarchar (400),
	@Description nvarchar(4000),
	@AdminComment nvarchar(4000),
	@ManufacturerPartNumber nvarchar(100),
	@IsDownload bit,
	@DownloadID int,
	@UnlimitedDownloads bit,
	@MaxNumberOfDownloads int,
	@HasSampleDownload bit,
	@SampleDownloadID int,
	@IsShipEnabled bit,
	@IsFreeShipping bit,
	@AdditionalShippingCharge money,
	@IsTaxExempt bit,
	@TaxCategoryID int,
	@ManageInventory bit,
	@StockQuantity int,
	@MinStockQuantity int,
	@LowStockActivityID int,
	@NotifyAdminForQuantityBelow int,
	@OrderMinimumQuantity int,
	@OrderMaximumQuantity int,
	@WarehouseId int,
	@DisableBuyButton bit,
	@Price money,
	@OldPrice money,
	@Weight float,
	@Length decimal(18, 4),
	@Width decimal(18, 4),
	@Height decimal(18, 4),
	@PictureID int,
	@AvailableStartDateTime datetime,
	@AvailableEndDateTime datetime,
	@Published bit,
	@Deleted bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ProductVariant]
	SET
		ProductId=@ProductId,
		[Name]=@Name,
		[SKU]=@SKU,
		[Description]=@Description,
		AdminComment=@AdminComment,
		ManufacturerPartNumber=@ManufacturerPartNumber,
		IsDownload=@IsDownload,
		DownloadID=@DownloadID,
		UnlimitedDownloads=@UnlimitedDownloads,
		MaxNumberOfDownloads=@MaxNumberOfDownloads,
		HasSampleDownload=@HasSampleDownload,
		SampleDownloadID=@SampleDownloadID,
		IsShipEnabled=@IsShipEnabled,
		IsFreeShipping=@IsFreeShipping,
		AdditionalShippingCharge=@AdditionalShippingCharge,
		IsTaxExempt=@IsTaxExempt,
		TaxCategoryID=@TaxCategoryID,
		ManageInventory=@ManageInventory,
		StockQuantity=@StockQuantity,
		MinStockQuantity=@MinStockQuantity,
		LowStockActivityID=@LowStockActivityID,
		NotifyAdminForQuantityBelow=@NotifyAdminForQuantityBelow,
		OrderMinimumQuantity=@OrderMinimumQuantity,
		OrderMaximumQuantity=@OrderMaximumQuantity,
		WarehouseId=@WarehouseId,
		DisableBuyButton=@DisableBuyButton,
		Price=@Price,
		OldPrice=@OldPrice,
		Weight=@Weight,
		[Length]=@Length,
		Width=@Width,
		Height=@Height,
		PictureID=@PictureID,
		AvailableStartDateTime=@AvailableStartDateTime,
		AvailableEndDateTime=@AvailableEndDateTime,
		Published=@Published,
		Deleted=@Deleted,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		ProductVariantID = @ProductVariantID
END
GO


-- some SMTP servers allows only real "From" property
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[Nop_Setting]
		WHERE [Name] = N'Email.UseSystemEmailForContactUsForm')
BEGIN
	INSERT [dbo].[Nop_Setting] ([Name], [Value], [Description])
	VALUES (N'Email.UseSystemEmailForContactUsForm', N'true', N'')
END
GO


--update current version
UPDATE [dbo].[Nop_Setting] 
SET [Value]='1.40'
WHERE [Name]='Common.CurrentVersion'
GO