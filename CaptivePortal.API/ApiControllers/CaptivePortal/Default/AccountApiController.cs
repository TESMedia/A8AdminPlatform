using System;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using CaptivePortal.API.Repository.CaptivePortal;
using log4net;
using System.Text;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using CaptivePortal.API.Enums.CPEnums;
using CaptivePortal.API.Common;
using CaptivePortal.API.ViewModels;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.ViewModels.CPAdmin;

namespace CaptivePortal.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    public class AccountApiController : ApiController
    {
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(AccountApiController));

        private UserRepository _userRepo;
        private MacAddressRepository _MacRepo;
        private UserAddressRepository _UserAddressRepo;
        
        public AccountApiController()
        {

        }
        public AccountApiController(UserRepository userRepo, MacAddressRepository macAddressRepo, UserAddressRepository userAddressRepo)
        {
            UserRepository = userRepo;
            MacAddressRepository = macAddressRepo;
        }

        public UserRepository UserRepository
        {
            get
            {
                return new UserRepository();
            }
            private set
            {
                _userRepo = value;
            }
        }
        public MacAddressRepository MacAddressRepository
        {
            get
            {
                return new MacAddressRepository();
            }
            private set
            {
                _MacRepo = value;
            }
        }
        public UserAddressRepository UserAddressRepository
        {
            get
            {
                return new UserAddressRepository();
            }
            private set
            {
                _UserAddressRepo = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("a8Captiveportal/V1/CreateUserWifi")]
        public HttpResponseMessage CreateUserWifi(UserMacAddressDetails objUserMac)
        {
            StatusReturn objReturn = null;
            if (!ModelState.IsValid)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            }
            try
            {
                UserRepository.CreateWifiUser(objUserMac.objUser);
                UserRepository.CreateUserRole(objUserMac.objUser);

                objUserMac.objMacAddress.UserId = objUserMac.objUser.Id;
                MacAddressRepository.CreateNewMacDevice(objUserMac.objMacAddress);

                objUserMac.objAddress.UserId = objUserMac.objUser.Id;
                UserAddressRepository.CreateUserAddress(objUserMac.objAddress);

                //objRegisterDB.CreateNewUser(objUserMac.objUser.UserName, objUserMac.objUser.PasswordHash, objUserMac.objUser.Email, objUserMac.objUser.FirstName, objUserMac.objUser.LastName);
                if (UserRepository.IsMemeber(objUserMac.objUser.UserName) && MacAddressRepository.IsMacAddressExistInParticularSite(objUserMac.objMacAddress.MacAddressValue, (int)objUserMac.objUser.SiteId))
                {
                    using (CommunicateRTLS objCommunicateRtls = new CommunicateRTLS())
                    {
                        string retResult = objCommunicateRtls.RegisterInRealTimeLocationService(new[] { objUserMac.objMacAddress.MacAddressValue }).Result;
                        Notification objServiceReturn = JsonConvert.DeserializeObject<Notification>(retResult);
                        if (objServiceReturn.result.returncode == Convert.ToInt32(RTLSResult.Success))
                        {
                            MacAddressRepository.RegisterMacAddressToRtls(objUserMac.objMacAddress.MacAddressValue);
                        }
                    }
                }


                    objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Success), ReturnCode.Success.ToString(),Common.Common.Reg_Success);
                    //dbContextTransaction.Commit();
            }
            catch (Exception ex)
            {
                //dbContextTransaction.Rollback();
                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Failure), ReturnCode.Success.ToString(), Common.Common.Reg_Success);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objReturn), Encoding.UTF8, "application/json")
            };
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("a8Captiveportal/V1/LoginWithNewMacAddress")]
        public HttpResponseMessage Login(LoginWIthNewMacAddressModel model)
        {
            StatusReturn objReturn = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    var response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    return response;
                }
                if (!UserRepository.IsNewUserAsPerSite(model.UserName, model.SiteId))
                {
                    var objWifiUsers = UserRepository.GetUniqueUserPerSite(model.UserName, model.SiteId);
                    if (!MacAddressRepository.IsMacAddressExistInParticularSite(model.MacAddress, model.SiteId))
                    {
                        //log.Info("Check that the particular MacAddress exist or Not for particualr User with Different Site";
                        MacAddress objMac = new MacAddress();
                        objMac.MacAddressValue = model.MacAddress;
                        objMac.UserId = objWifiUsers.Id;
                        objMac.BrowserName = model.BrowserName;
                        objMac.UserAgentName = model.UserAgentName;
                        objMac.OperatingSystem = model.OperatingSystem;
                        objMac.IsMobile = model.IsMobile;
                        MacAddressRepository.CreateNewMacDevice(objMac);

                        //Save all the Users data in MySql DataBase
                        if (UserRepository.IsMemeber(model.UserName) && MacAddressRepository.IsMacAddressExistInParticularSite(model.UserName, (int)model.SiteId))
                        {
                            using (CommunicateRTLS objCommunicateRtls = new CommunicateRTLS())
                            {
                                string retResult = objCommunicateRtls.RegisterInRealTimeLocationService(new[] { model.MacAddress }).Result;
                                Notification objServiceReturn = JsonConvert.DeserializeObject<Notification>(retResult);
                                if (objServiceReturn.result.returncode == Convert.ToInt32(RTLSResult.Success))
                                {
                                    MacAddressRepository.RegisterMacAddressToRtls(model.MacAddress);
                                }
                            }
                        }
                    }
                }
                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Success), ReturnCode.Success.ToString(), Common.Common.Reg_Success);
            }
            catch (Exception ex)
            {
                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Failure), ReturnCode.Success.ToString(), Common.Common.Reg_Success);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objReturn), Encoding.UTF8, "application/json")
            };
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        [HttpPost]
        [Route("GetStatusOfUserForSite")]
        public HttpResponseMessage GetStatusOfUserForSite(LoginWIthNewMacAddressModel model)
        {
            AutoLoginStatus returnStatus = new AutoLoginStatus();
            try
            {
                //Need to check the MacAddress exist for the particular Site with Autologin true
                if (MacAddressRepository.IsMacAddressExistInParticularSite(model.MacAddress, model.SiteId))
                {
                    log.Info("inside Is Any MacAddressExist For Particular Site");
                    var User = UserRepository.GetUserPerDeviceMacAddress(model.MacAddress, model.SiteId);
                    if (User.AutoLogin == true)
                    {
                        log.Info("Check the AutoLogin of Site or User");         
                        //objReturn.returncode = Convert.ToInt32(ReturnCode.Success);
                        returnStatus.UserName = User.UserName;
                        returnStatus.Password = User.PasswordHash;
                        returnStatus.StatusReturn.ReturnCode = Convert.ToInt32(ReturnCode.Success);
                    }
                }
                else
                {
                    returnStatus.StatusReturn.ReturnCode = Convert.ToInt32(ReturnCode.Warning);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                returnStatus.StatusReturn.ReturnCode = Convert.ToInt32(ReturnCode.Failure);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(returnStatus), Encoding.UTF8, "application/json")
            };
        }
    }
}


