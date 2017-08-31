using System;
using System.Net.Http;
using System.Web.Http;
using log4net;
using System.Linq;
using System.Text;
using System.Web.Http.Cors;
using CaptivePortal.API.ViewModels;
using Newtonsoft.Json;
using CaptivePortal.API.Repository.CaptivePortal;
using CaptivePortal.API.Models.CustomIdentity;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.Enums.CPEnums;
using CaptivePortal.API.Common;
using CaptivePortal.API.ViewModels.CPAdmin;

namespace CaptivePortal.API.ApiControls
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    public class MemberApiController : ApiController
    {
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(MemberApiController));

        private UserRepository _userRepo;
        private MacAddressRepository _MacRepo;
        private UserAddressRepository _UserAddressRepo;
        private ApiAccessUserSessionRepository _ApiAccessUserRepo;

        private MemberApiController()
        {

        }
        public MemberApiController(UserRepository userRepo, MacAddressRepository macAddressRepo, UserAddressRepository userAddressRepo, ApiAccessUserSessionRepository apiUserSessionRepo)
        {
            UserRepository = userRepo;
            MacAddressRepository = macAddressRepo;
            ApiUserSessionRepository = apiUserSessionRepo;
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

        public ApiAccessUserSessionRepository ApiUserSessionRepository
        {
            get
            {
                return new ApiAccessUserSessionRepository();
            }
            private set
            {
                _ApiAccessUserRepo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("a8Captiveportal/V1/Login")]
        public HttpResponseMessage Login(MemberViewModel objMemberViewModel)
        {
            string strReturn = null;
            StatusReturn objReturn = null;
            try
            {
                //AuthenticateMemberValidation objCustomMemberShipValidation = new AuthenticateMemberValidation();
                //objCustomMemberShipValidation.CheckValidation(objMemberViewModel);
                if (UserRepository.IsMemberApplicationAdmin(objMemberViewModel.UserName, objMemberViewModel.Password))
                    {
                        string GenerateSessionId = GenerateUniqueSessionId.GenerateSeesionId();
                        ApiAccessUserSession objApiUserSession = new ApiAccessUserSession() { UserId = UserRepository.GetUserId(objMemberViewModel.UserName), SessionId = GenerateSessionId };
                        ApiUserSessionRepository.UpdateCreateUserSession(objApiUserSession);
                        objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.LoginSuccess), ReturnCode.LoginSuccess.ToString(), GenerateSessionId);
                    }
                    else
                    {
                        objReturn = new StatusReturn(Convert.ToInt32(ErrorCodeWarning.IncorrectPassword), ReturnCode.Warning.ToString(), "UserName or Password Not Match");
                    }
                    strReturn = JsonConvert.SerializeObject(objReturn);
                //strReturn = JsonConvert.SerializeObject(objCustomMemberShipValidation.listStatusReturn);
            }
            catch (Exception ex)
            {
                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Failure), ReturnCode.Failure.ToString(), "Some Error Occured");
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(strReturn, Encoding.UTF8, "application/json")
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("a8Captiveportal/V1/CreateUser")]
        public HttpResponseMessage CreateUserExpose(MemberViewModel objUser)
        {
            StatusReturn objReturn = null;
            string strReturn = null;

            try
            {
                ApplicationUser objApplicationUser = UserRepository.MapToApplicationUser(objUser);
                if (ApiUserSessionRepository.IsAuthorize(objUser.SessionId))
                {

                    UserRepository.CreateWifiUser(objApplicationUser);
                    UserRepository.CreateUserRole(objApplicationUser);
                    RegisterDB objRegisterDB = new RegisterDB();
                    objRegisterDB.CreateNewUser(objUser.UserName, objUser.Password, objUser.Email, objUser.FirstName, objUser.LastName);
                    objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Success), ReturnCode.Success.ToString(), Common.Common.Reg_Success);
                    strReturn = JsonConvert.SerializeObject(objReturn);

                }
                else
                {
                    objReturn = new StatusReturn(Convert.ToInt32(ErrorCodeWarning.NonAuthorize), ReturnCode.Warning.ToString(), "Invalid SessionId");
                    strReturn = JsonConvert.SerializeObject(objReturn);
                }

            }
            catch (Exception ex)
            {
                //dbContextTransaction.Rollback();
                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Failure), ReturnCode.Success.ToString(), Common.Common.Reg_Success);
                strReturn = JsonConvert.SerializeObject(objReturn);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(strReturn, Encoding.UTF8, "application/json")
            };
        }



        /// <summary>
        /// it will add or delete one or more mac adddress of a user.
        /// </summary>
        /// <param name="objUserMac"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("a8Captiveportal/V1/UpdateMacAddress")]
        public HttpResponseMessage UpdateMacAddress(MemberDevicesModel objUserMac)
        {
            StatusReturn objReturn = null;
            try
            {
                if (ApiUserSessionRepository.IsAuthorize(objUserMac.SessionId))
                {
                    if (objUserMac.OperationType == OperationType.Add)
                    {
                       MacAddressRepository.RegisterListOfMacAdressToDb(objUserMac);
                        using (CommunicateRTLS objCommunicateRtls = new CommunicateRTLS())
                        {
                            string strResult = objCommunicateRtls.RegisterInRealTimeLocationService(objUserMac.MacAddressList.Select(m => m.MacAddress).ToArray()).Result;
                            Notification objServiceReturn = JsonConvert.DeserializeObject<Notification>(strResult);
                            if (objServiceReturn.result.returncode == Convert.ToInt32(RTLSResult.Success))
                            {
                                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Success), ReturnCode.Success.ToString(), " ");
                            }
                        }
                    }
                    else if (objUserMac.OperationType == OperationType.Delete)
                    {
                       MacAddressRepository.RemoveListOfMacAddressFromDb(objUserMac);

                        using (CommunicateRTLS objCommunicateRtls = new CommunicateRTLS())
                        {
                            string strResult = objCommunicateRtls.DeregisterInRealTimeLocationServices(objUserMac.MacAddressList.Select(m => m.MacAddress).ToArray()).Result;
                            Notification objServiceReturn = JsonConvert.DeserializeObject<Notification>(strResult);
                            if (objServiceReturn.result.returncode == Convert.ToInt32(RTLSResult.Success))
                            {
                                objReturn = new StatusReturn(Convert.ToInt32(ErrorCodeWarning.SessionIdRequired), ReturnCode.Success.ToString(), " ");
                            }
                        }
                    }
                    else
                    {
                        objReturn = new StatusReturn(Convert.ToInt32(ErrorCodeWarning.NonAuthorize), ReturnCode.Warning.ToString(), "Invalid SessionId");
                    }
                }
            }
            catch (Exception ex)
            {
                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Failure), ReturnCode.Failure.ToString(), "some problem occured");
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objReturn), Encoding.UTF8, "application/json")
            };
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("a8Captiveportal/V1/GetMacAddresses")]
        public HttpResponseMessage GetMacAddress(MemberViewModel model)
        {
            ReturnMacDevices objReturnMac = new ReturnMacDevices();
            StatusReturn objReturn = new StatusReturn();
            try
            {
                if (ApiUserSessionRepository.IsAuthorize(model.SessionId))
                {
                    if (UserRepository.IsNewMemberAsPerSite(model.UserId, model.SiteId))
                    {
                        string[] Macs = MacAddressRepository.GetListMacAddress(model.UserId, model.SiteId);
                        foreach (var item in Macs)
                        {
                            MacAddress objMacAddress = new MacAddress();
                            objMacAddress.MacAddressValue = item;
                            objReturnMac.MacAddressList.Add(objMacAddress);
                        }
                        objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.GetMacAddressSuccess), ReturnCode.Success.ToString(), "Successfully return the MacAddresses");
                    }
                    else
                    {
                        objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Warning), ReturnCode.Warning.ToString(), "UserId or SiteId Not Exist");
                    }
                }
                else
                {
                    objReturn = new StatusReturn(Convert.ToInt32(ErrorCodeWarning.NonAuthorize), ErrorCodeWarning.NonAuthorize.ToString(), "Invalid SesssionId" + " " + model.SessionId);
                }
            }
            catch (Exception ex)
            {
                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Failure), ReturnCode.Failure.ToString(), "Error Occured");
            }
            objReturnMac.objReturn = objReturn;
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objReturnMac), Encoding.UTF8, "application/json")
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("a8Captiveportal/V1/DeleteUser")]
        public HttpResponseMessage DeleteWiFiUser(MemberViewModel model)
        {
            StatusReturn objReturn = null;
            try
            {
                if (ApiUserSessionRepository.IsAuthorize(model.SessionId))
                {
                    if (UserRepository.IsMemberRegisterInRTLS(model.UserId, model.SiteId))
                    {
                        using (CommunicateRTLS objCommunicateRtls = new CommunicateRTLS())
                        {
                            var lstMacAddress = MacAddressRepository.GetListMacAddress(model.UserId, model.SiteId);
                            string retResult = objCommunicateRtls.DeregisterInRealTimeLocationServices(lstMacAddress).Result;
                            Notification objServiceReturn = JsonConvert.DeserializeObject<Notification>(retResult);
                            if (objServiceReturn.result.returncode == Convert.ToInt32(RTLSResult.Success))
                            {
                                UserRepository.RemoveMemberAsUserUniqueId(model.UserId, model.SiteId);
                                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.DeleteUserSuccess), ReturnCode.DeleteUserSuccess.ToString(), "User Deleted");
                            }
                        }
                    }
                }
                else
                {
                    objReturn = new StatusReturn(Convert.ToInt32(ErrorCodeWarning.NonAuthorize), ReturnCode.Warning.ToString(), "Invalid SessionId");
                }
            }
            catch (Exception ex)
            {
                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Failure), ReturnCode.Failure.ToString(), "Error Occured");
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objReturn), Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("a8Captiveportal/V1/UpdateUser")]
        public HttpResponseMessage UpdateUser(MemberViewModel objUser)
        {
            StatusReturn objReturn;
            try
            {
                if (ApiUserSessionRepository.IsAuthorize(objUser.SessionId))
                {
                     ApplicationUser objApplicationUser = UserRepository.MapToApplicationUser(objUser);
                     UserRepository.UpdateWifiUser(objApplicationUser);
                     UpdateDb objUpdateDb = new UpdateDb();
                     objUpdateDb.UpdateUser(objUser.UserName, objUser.Password, objUser.Email, objUser.FirstName, objUser.LastName);
                     log.Info("User data commited successfully");
                     objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.UpdateUserSuccess), ReturnCode.UpdateUserSuccess.ToString(), "user details updated ");
                }
                else
                {
                    objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Warning), ReturnCode.Warning.ToString(), "Invalid SessionId");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //dbContextTransaction.Rollback();
                objReturn = new StatusReturn(Convert.ToInt32(ReturnCode.Failure), ReturnCode.Failure.ToString(), "some problem occure");
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objReturn), Encoding.UTF8, "application/json")
            };
        }
    }
}
