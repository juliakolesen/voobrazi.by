using System;
using System.Configuration;
using System.Linq;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Data;
using NopSolutions.NopCommerce.BusinessLogic.Infrastructure;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Tasks;

namespace Nop.Payment.EasyPay
{
    public class EasyPayTask : ITask
    {
        public void Execute(XmlNode node)
        {
            try
            {
                var ctx = IoC.Resolve<NopObjectContext>();
                var orderService = IoC.Resolve<OrderService>();
                var easyPayService = new EasyPayService(EasyPayPaymentProcessor.UseSandbox);
                var pendingOrders = ctx.Orders.Where(o => o.PaymentMethodName == EasyPayPaymentProcessor.MethodName && o.PaymentStatusId == (int)PaymentStatusEnum.Pending && o.OrderStatusId == (int)OrderStatusEnum.Pending).ToList();

                double eazyPayInvoiceLivingTimeSec = double.Parse(ConfigurationManager.AppSettings["EazyPayInvoiceLivingTimeSec"]);
                var outdatedOrders = pendingOrders.Where(o => o.CreatedOn.AddSeconds(eazyPayInvoiceLivingTimeSec) < DateTime.UtcNow).ToList();
                foreach (Order outdatedOrder in outdatedOrders)
                {
                    try
                    {
                        orderService.CancelOrder(outdatedOrder.OrderId, false);
                    }
                    catch (Exception exc)
                    {
                        IoC.Resolve<ILogService>().InsertLog(LogTypeEnum.OrderError, "Cannot cancel order", exc);
                    }
                }


                pendingOrders = pendingOrders.Except(outdatedOrders).ToList();
                foreach (Order pendingOrder in pendingOrders)
                {
                    try
                    {
                        var status = easyPayService.EP_IsInvoicePaid(EasyPayPaymentProcessor.MerchantNumber, EasyPayPaymentProcessor.P, pendingOrder.OrderId.ToString());
                        switch (status.code)
                        {
                            case 200:
                                orderService.MarkOrderAsPaid(pendingOrder.OrderId);
                                break;
                            case 211:
                                // Not paid yet. Skip it.
                                break;
                            case 503:
                            case 506:
                            case 517:
                                orderService.CancelOrder(pendingOrder.OrderId, false);
                                break;
                            default:
                                throw new Exception(string.Format("EasyPay check invoice status. Status: {0}. Error message: {1}", status.code, status.message));
                        }
                    }
                    catch (Exception exc)
                    {
                        IoC.Resolve<ILogService>().InsertLog(LogTypeEnum.OrderError, "Cannot cancel or mark order as paid", exc);
                    }
                }
            }
            catch (Exception ex)
            {
                IoC.Resolve<ILogService>().InsertLog(LogTypeEnum.OrderError, "Error checking EasyPay invoice statuses.", ex);
            }
        }
    }
}
