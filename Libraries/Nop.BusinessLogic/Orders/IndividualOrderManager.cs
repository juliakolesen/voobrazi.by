using System;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Orders;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using System.Web;

namespace NopSolutions.NopCommerce.BusinessLogic.Orders
{
    public partial class IndividualOrderManager
    {
        #region Utilities
        private static IndividualOrderCollection DBMapping(DBIndividualOrderCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            IndividualOrderCollection collection = new IndividualOrderCollection();
            foreach (DBIndividualOrder dbItem in dbCollection)
            {
                IndividualOrder item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static IndividualOrder DBMapping(DBIndividualOrder dbItem)
        {
            if (dbItem == null)
                return null;

            IndividualOrder item = new IndividualOrder();
            item.IndividualOrderID = dbItem.IndividualOrderID;
            item.OrderText = dbItem.OrderText;
            item.Price = dbItem.Price;
            item.SerialNumberInShCart = dbItem.SerialNumberInShCart;
            item.CurrentUserSessionGuid = item.CurrentUserSessionGuid;

            return item;
        }

        #endregion

        #region Methods
        public static void DeleteIndividualOrder(int IndividualOrderID)
        {
            IndividualOrder indivOrder = GetIndividualOrderByID(IndividualOrderID);
            if (indivOrder != null)
            {
                DBProviderManager<DBIndividualOrderProvider>.Provider.DeleteIndividualOrder(IndividualOrderID);
            }
        }


        public static IndividualOrderCollection GetIndividualOrderByCurrentUserSessionGuid(Guid userGuid)
        {
            DBIndividualOrderCollection dbCollection = DBProviderManager<DBIndividualOrderProvider>.Provider.IndividualOrderGetByCustomerSessionGUID(userGuid);
            IndividualOrderCollection indivCollection = DBMapping(dbCollection);
            return indivCollection;
        }

        public static IndividualOrder InsertIndividualOrder(Guid userGuid, long Price, String OrderText)
        {
            if (userGuid == null || Price == null)
                return null;

            if (HttpContext.Current.Request.Cookies["Currency"] != null && HttpContext.Current.Request.Cookies["Currency"].Value == "USD")
            {
                Price = PriceConverter.ToBYR(Price);
            }

            DBIndividualOrder dbItem = null;
            dbItem = DBProviderManager<DBIndividualOrderProvider>.Provider.InsertIndividualOrder(userGuid, Price, OrderText);
            IndividualOrder indOrder = DBMapping(dbItem);
            return indOrder;
        }

        public static IndividualOrder GetIndividualOrderByID(int individualOrderID)
        {
            DBIndividualOrder dbItem = DBProviderManager<DBIndividualOrderProvider>.Provider.GetIndividualOrderByID(individualOrderID);
            IndividualOrder indivOrder = DBMapping(dbItem);
            return indivOrder;
        }

        public static decimal GetTotalPriceIndOrders(IndividualOrderCollection indOrderCollection)
        {
            decimal price = 0;
            foreach (var order in indOrderCollection)
            {
                price += order.Price;
            }

            return price;
        }

        public static IndividualOrderCollection GetCurrentUserIndividualOrders()
        {
            if (NopContext.Current.Session == null)
                return new IndividualOrderCollection();
            Guid CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;
            return GetIndividualOrderByCurrentUserSessionGuid(CustomerSessionGUID);
        }
        #endregion
    }
}
