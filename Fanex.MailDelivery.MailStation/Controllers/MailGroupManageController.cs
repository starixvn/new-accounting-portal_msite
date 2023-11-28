using System.Web.Mvc;
using Fanex.MailDelivery.MailStation.Constants;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Extensions;
using Fanex.MailDelivery.MailStation.Services;
using Fanex.Maintenance.Controller;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.MailDelivery.MailStation.Controllers
{
    [NapSuperAuth]
    public class MailGroupManageController : BaseController
    {
        private readonly MailGroupService mailGroupService = new MailGroupService();

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Read)]
        public ActionResult Index(GetMailGroupCriteria criteria)
        {
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/MailGroup/ManageMailGroup/Index.cshtml", criteria);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
        public ActionResult CreateMailGroupView()
        {
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            return View("~/Areas/MailStation/MailGroup/CreateMailGroup/Index.cshtml");
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
        public JsonResult CreateMailGroup(CreateMailGroupCriteria criteria)
        {
            var commandResult = mailGroupService.CreateMailGroup(criteria);

            return Json(commandResult);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Read)]
        public JsonResult MailGroupList(GetMailGroupCriteria criteria)
        {
            var groups = mailGroupService.GetMailGroups(criteria);

            return Json(groups);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
        public ActionResult DeleteMailGroup(DeleteMailGroupCriteria criteria)
        {
            var commandResult = mailGroupService.DeleteMailGroup(criteria);

            return Json(commandResult);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
        public ActionResult UpdateMailGroupView(GetMailGroupByIdCriteria criteria)
        {
            var model = new UpdateMailGroupCriteria();
            var group = mailGroupService.GetMailGroupById(criteria);

            if (group != null)
            {
                model.MailGroupId = group.MailGroupId;
                model.MailGroupName = group.MailGroupName;
                model.Description = group.Description;
            }

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

            return View("~/Areas/MailStation/MailGroup/UpdateMailGroup/Index.cshtml", model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Host)]
        public JsonResult UpdateMailGroup(UpdateMailGroupCriteria criteria)
        {
            var commandResult = mailGroupService.UpdateMailGroup(criteria);

            return Json(commandResult);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMailGroup, Policy = Constant.Policy.Read)]
        public ActionResult ViewRelevantMail(int mailGroupId)
        {
            return RedirectToAction("Index", "MailManage", new { MailGroupId = mailGroupId });
        }
    }
}
