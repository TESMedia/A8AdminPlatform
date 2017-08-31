using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Script.Serialization;
using CaptivePortal.API.ViewModels.CPAdmin;
using CaptivePortal.API.Repository.CaptivePortal;

namespace CaptivePortal.API.Controllers
{
    [RoutePrefix("api/form")]
    public class FormApiController : ApiController
    {
        private FormRepository FormRepo { get; set; }
        public FormApiController()
        {
            FormRepo = new FormRepository();
        }
        private A8AdminDbContext db = new A8AdminDbContext();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginFormData")]
        public HttpResponseMessage LoginPageDynamicData(FormData formdata)
        {
            ReturnLoginFormData objLoginFormData = null;
            try
            {
                objLoginFormData = FormRepo.ReturnPageDynamicData(formdata);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objLoginFormData), Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterFormData")]
        public HttpResponseMessage RegisterHtmlDynamicCode(FormData formdata)
        {
            var formControlResult = FormRepo.GetFormData(formdata.SiteId);

            string termCondition = formControlResult.Site.TermsAndCondDoc;
            string bannerIcon = formControlResult.BannerIcon;
            string registerPageTitle = formControlResult.RegistrationPageTitle;
            string controllerIP= formControlResult.Site.ControllerIpAddress;
            bool IsPasswordRequire = formControlResult.IsPasswordRequire;

            var objReturnRegisterFormListData=FormRepo.ReturnRegistrationPageDynamicData(formdata);
            var formJsonResult = JsonConvert.SerializeObject(objReturnRegisterFormListData);
            return Request.CreateResponse(HttpStatusCode.OK, new { formJsonResult, IsPasswordRequire, bannerIcon, registerPageTitle, controllerIP }, JsonMediaTypeFormatter.DefaultMediaType);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TermAndConditionContent")]
        public HttpResponseMessage GetTermAndConditionContent(FormData formdata)
        {
            string TermCond = null;
            try
            {
                TermCond = FormRepo.GetTermConditionData(formdata.SiteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { TermCond }, JsonMediaTypeFormatter.DefaultMediaType);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ManagePromotion")]
        public HttpResponseMessage GetPromotionalData(FormData formdata)
        {
           if(formdata.SiteId!=0)
            {
                using (ManagePromotionRepository objManagePromotionRepository = new ManagePromotionRepository())
                {
                    var PromotinalData = objManagePromotionRepository.GetManagePromotionData(formdata.SiteId);
                    return Request.CreateResponse(HttpStatusCode.OK, new { PromotinalData }, JsonMediaTypeFormatter.DefaultMediaType);
                } 
            }
            else
            {
                string err = "SiteId required.";
                return Request.CreateResponse(HttpStatusCode.OK, err);
            }
        }
    }
}
