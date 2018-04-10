namespace Fanex.Maintenance.Controller
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Home Controller.
    /// </summary>
    [NapSuperAuth]
    public class RemotingRefController : BaseController
    {
        /// <summary>
        /// Index Page.
        /// </summary>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = SiteFunctionKeys.RemotingRef, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var token = Cryptography.CfsEnCrypt(DateTime.Now.ToString());            ViewBag.RemotingRefDemoUrl = $"{ConfigurationManager.AppSettings["RemotingRefDemoUrl"]}?token={token}";
            return View();
        }
    }
}