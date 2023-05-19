using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using Microsoft.Extensions.Options;
using ZulAssetsBackEnd_API.Services;
using ZulAssetsBackEnd_API.Settings;
using ZulAssetsBackEnd_API.Model;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X500;

namespace ZulAssetsBackEnd_API.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        #region Declarations & Contruction

        public readonly static string SP_UserLogin = "[dbo].[SP_UserLogin]";
        public readonly static string SP_VerifyOldPassword = "[dbo].[SP_NewPasswordwithVerification]";
        public readonly static string UpdateRefreshToken_SP = "[dbo].[UpdateRefreshToken]";
        public readonly static string SP_GetUserByID = "[dbo].[GetUserByID]";

        private readonly IMailService _mailService;
        private readonly MailSettings _mailSettings;

        public UserController(IMailService mailService, IOptions<MailSettings> options)
        {
            _mailService = mailService;
            _mailSettings = options.Value;
        }

        #endregion

        #region User Login

        /// <summary>
        /// Login API
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        /// 

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Loginparam loginParam)
        {
            Message msg = new Message();
            LoginRes loginRes;
            try
            {
                DataTable dt = DataLogic.LoginDetails(loginParam, SP_UserLogin);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        return Ok(msg);
                    }
                    else
                    {

                        var loginName = dt.Rows[0]["LoginName"].ToString();
                        var userName = dt.Rows[0]["UserName"].ToString();
                        var roleID = dt.Rows[0]["RoleID"].ToString();
                        var status = dt.Rows[0]["Status"].ToString();

                        #region Return Parameters

                        if (status == "200")
                        {

                            #region JWT Authentication

                            var tokenString = JWTBuilder.Generation(loginName, userName, roleID, status);

                            var refreshToken = JWTBuilder.GenerateRefreshToken();
                            DataTable dt2 = DataLogic.UpdateRefreshToken(loginParam.LoginName, refreshToken, UpdateRefreshToken_SP);

                            #endregion


                            loginRes = new LoginRes();

                            loginRes.UserName = userName;
                            loginRes.LoginName = loginName;
                            loginRes.RoleID = roleID;
                            loginRes.Status = status;
                            loginRes.Message = "User Authenticated";
                            loginRes.RefreshToken = dt.Rows[0]["refreshToken"].ToString();
                            loginRes.RefreshTokenNew = refreshToken;
                            loginRes.Token = tokenString.Item1.ToString();
                            loginRes.ValidTill = tokenString.Item2.ToString();

                            return Ok(loginRes);
                        }
                        else if( status == "401")
                        {
                            loginRes = new LoginRes();

                            loginRes.UserName = "";
                            loginRes.LoginName = "";
                            loginRes.RoleID = "";
                            loginRes.Status = status;
                            loginRes.Message = "User Not Authenticated";
                            loginRes.RefreshToken = "";
                            loginRes.RefreshTokenNew = "";
                            loginRes.Token = "";
                            loginRes.ValidTill = "";

                            return Ok(loginRes);
                        }
                        else
                        {
                            loginRes = new LoginRes();

                            loginRes.UserName = "";
                            loginRes.LoginName = "";
                            loginRes.RoleID = "";
                            loginRes.Status = status;
                            loginRes.Message = "User Not Found";
                            loginRes.RefreshToken = "";
                            loginRes.RefreshTokenNew = "";
                            loginRes.Token = "";
                            loginRes.ValidTill = "";

                            return Ok(loginRes);
                        }

                        #endregion

                    }
                }
                else
                {
                    return Ok(dt);
                }

            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }

        }

        #endregion

        #region RefreshToken
        /// <summary>
        /// Refreshing Token when Last token is valid
        /// </summary>
        /// <param name="refreshTokenRequest"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                LoginRes loginRes = new LoginRes();
                var principal = JWTBuilder.GetClaimsFromExpiredToken(refreshTokenRequest.JWTToken);
                var loginName_ = principal?.Claims?.SingleOrDefault(p => p.Type == UserClaimParameters.LOGINNAME.ToString())?.Value;
                DataTable dt = DataLogic.GetUserByID(loginName_.ToString(), SP_GetUserByID);
                var status = dt.Rows[0]["Status"].ToString();
                if (dt.Columns.Contains("ErrorMessage"))
                {
                    return Ok(dt);
                }
                else
                {

                    #region Return Params

                    if (status == "200")
                    {

                        #region Refreshing Token

                        var refreshToken = dt.Rows[0]["RefreshToken"].ToString();
                        if (refreshToken != refreshTokenRequest.RefreshToken)
                            throw new SecurityTokenException("Invalid refresh token");
                        var loginName = dt.Rows[0]["LoginName"].ToString();
                        var userName = dt.Rows[0]["UserName"].ToString();
                        var roleID = dt.Rows[0]["RoleID"].ToString();
                        status = dt.Rows[0]["Status"].ToString();
                        var newJwtToken = JWTBuilder.Generation(loginName, userName, roleID, status);
                        var newRefreshToken = JWTBuilder.GenerateRefreshToken();

                        #endregion

                        DataTable dt2 = DataLogic.UpdateRefreshToken(loginName, newRefreshToken, UpdateRefreshToken_SP);
                        DataSet ds2 = DataLogic.GetDetails(loginName, SP_GetUserByID);


                        loginRes.UserName = userName;
                        loginRes.LoginName = loginName;
                        loginRes.RoleID = roleID;
                        loginRes.Status = status;
                        loginRes.Message = "User Authenticated";
                        loginRes.RefreshToken = dt.Rows[0]["refreshToken"].ToString();
                        loginRes.RefreshTokenNew = newRefreshToken;
                        loginRes.Token = newJwtToken.Item1.ToString();
                        loginRes.ValidTill = newJwtToken.Item2.ToString();


                        return Ok(loginRes);
                    }
                    else
                    {
                        loginRes.UserName = "";
                        loginRes.LoginName = "";
                        loginRes.RoleID = "";
                        loginRes.Status = status;
                        loginRes.Message = "User Not Found";
                        loginRes.RefreshToken = "";
                        loginRes.RefreshTokenNew = "";
                        loginRes.Token = "";
                        loginRes.ValidTill = "";

                        return Ok(loginRes);
                    }

                    #endregion

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion

        #region Change Password

        /// <summary>
        /// Change Password API
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns>Returns Success Message of Change Password</returns>
        [HttpPost("ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePassword changePassword)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.VerifyOldPassword(changePassword, SP_VerifyOldPassword);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        return Ok(msg);
                    }
                    else
                    {
                        msg.message = dt.Rows[0]["Message"].ToString(); ;
                        return Ok(msg);
                    }

                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
            
        }

        #endregion

        #region Forget Password
        /// <summary>
        /// Reset Password API
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns>Will check if user exists? if yes then will send an email to admin regarding password change</returns>
        [HttpPost("ForgetPassword")]
        [Consumes("application/json")]
        public IActionResult ForgetPassword([FromBody] Loginparam loginParam)
        {
            Message msg = new Message();
            MailRequest mailRequest = new MailRequest();
            mailRequest.AppliedUserName = loginParam.LoginName;
            try
            {
                //Verify that User is exist or not
                DataTable dt = DataLogic.LoginDetails(loginParam, SP_UserLogin);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        return Ok(msg);
                    }
                    else
                    {
                        if (dt.Rows[0]["Message"].ToString().Contains("Not"))
                        {
                            msg.message = dt.Rows[0]["Message"].ToString();
                            return Ok(msg);
                        }
                        else
                        {
                            var _1 = new EmailController(_mailService).ForgetPasswordEmail(mailRequest);
                            msg.message = "Email Sent Successfully";
                            return Ok(msg);
                        }
                        
                    }
                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }            
        }
        #endregion

    }
}
