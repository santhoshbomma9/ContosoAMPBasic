namespace Dashboard.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Dashboard.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SaaSFulfillmentClient;
    using SaaSFulfillmentClient.Models;

    [Authorize("DashboardAdmin")]
    public class MailLinkController : Controller
    {
        private readonly IFulfillmentClient fulfillmentClient;
        private readonly IOperationsStore operationsStore;

        public MailLinkController(IFulfillmentClient fulfillmentClient,
                                  IOperationsStore operationsStore)
        {
            this.fulfillmentClient = fulfillmentClient;
            this.operationsStore = operationsStore;
        }

        [HttpGet]
        public async Task<IActionResult> Activate(
            NotificationModel notificationModel,
            CancellationToken cancellationToken)
        {
            var result = await this.fulfillmentClient.ActivateSubscriptionAsync(
                             notificationModel.SubscriptionId,
                             new ActivatedSubscription { PlanId = notificationModel.PlanId },
                             Guid.Empty,
                             Guid.Empty,
                             cancellationToken);

            return result.Success
                       ? this.View(
                           new ActivateActionViewModel
                               {
                                   SubscriptionId = notificationModel.SubscriptionId, PlanId = notificationModel.PlanId
                               })
                       : this.View("MailActionError", FulfillmentRequestErrorViewModel.From(result));
        }

        [HttpGet]
        public async Task<IActionResult> QuantityChange(
            NotificationModel notificationModel,
            CancellationToken cancellationToken)
        {
            var result = await this.UpdateOperationAsync(notificationModel, cancellationToken);

            return result.Success
                       ? this.View("OperationUpdate", notificationModel)
                       : this.View("MailActionError", FulfillmentRequestErrorViewModel.From(result));
        }

        [HttpGet]
        public async Task<IActionResult> Reinstate(
            NotificationModel notificationModel,
            CancellationToken cancellationToken)
        {
            var result = await this.UpdateOperationAsync(notificationModel, cancellationToken);

            return result.Success
                       ? this.View("OperationUpdate", notificationModel)
                       : this.View("MailActionError", FulfillmentRequestErrorViewModel.From(result));
        }

        [HttpGet]
        public async Task<IActionResult> SuspendSubscription(
            NotificationModel notificationModel,
            CancellationToken cancellationToken)
        {
            var result = await this.UpdateOperationAsync(notificationModel, cancellationToken);

            return result.Success
                       ? this.View("OperationUpdate", notificationModel)
                       : this.View("MailActionError", FulfillmentRequestErrorViewModel.From(result));
        }

        [HttpGet]
        public async Task<IActionResult> Unsubscribe(
            NotificationModel notificationModel,
            CancellationToken cancellationToken)
        {
            var result = await this.UpdateOperationAsync(notificationModel, cancellationToken);

            return result.Success
                       ? this.View("OperationUpdate", notificationModel)
                       : this.View("MailActionError", FulfillmentRequestErrorViewModel.From(result));
        }

        [HttpGet]
        public async Task<IActionResult> Update(NotificationModel notificationModel)
        {
            var result = await this.fulfillmentClient.UpdateSubscriptionPlanAsync(
                             notificationModel.SubscriptionId,
                             notificationModel.PlanId,
                             Guid.Empty,
                             Guid.Empty,
                             CancellationToken.None);

            return result.Success
                       ? this.View(
                           new ActivateActionViewModel
                               {
                                   SubscriptionId = notificationModel.SubscriptionId, PlanId = notificationModel.PlanId
                               })
                       : this.View("MailActionError", FulfillmentRequestErrorViewModel.From(result));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateQuantity(NotificationModel notificationModel)
        {
            var result = await this.fulfillmentClient.UpdateSubscriptionQuantityAsync(
                             notificationModel.SubscriptionId,
                             notificationModel.Quantity,
                             Guid.Empty,
                             Guid.Empty,
                             CancellationToken.None);
            
            if (this.operationsStore != default)
            {
                await this.operationsStore.RecordAsync(notificationModel.SubscriptionId, result, new CancellationToken());
            }

            return result.Success
                       ? this.View("Update",
                           new ActivateActionViewModel
                           {
                               SubscriptionId = notificationModel.SubscriptionId,
                               Quantity = notificationModel.Quantity
                           })
                       : this.View("MailActionError", FulfillmentRequestErrorViewModel.From(result));
        }

        private async Task<FulfillmentRequestResult> UpdateOperationAsync(
            NotificationModel payload,
            CancellationToken cancellationToken)
        {
            return await this.fulfillmentClient.UpdateSubscriptionOperationAsync(
                       payload.SubscriptionId,
                       payload.OperationId,
                       new OperationUpdate
                           {
                               PlanId = payload.PlanId,
                               Quantity = payload.Quantity,
                               Status = OperationUpdateStatusEnum.Success
                           },
                       Guid.Empty,
                       Guid.Empty,
                       cancellationToken);
        }
    }
}