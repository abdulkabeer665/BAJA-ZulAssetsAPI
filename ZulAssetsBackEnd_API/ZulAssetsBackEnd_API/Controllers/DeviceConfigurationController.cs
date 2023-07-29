using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;

namespace ZulAssetsBackEnd_API.Controllers
{
    /// <summary>
    /// Device Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    
    public class DeviceConfigurationController : ControllerBase
    {

        #region Declarations

        public readonly static string SP_DeviceInsertUpdateDelete = "[dbo].[SP_DeviceInsertUpdateDelete]";
        public readonly static string SP_VerifyDeviceRegistrationKey = "[dbo].[SP_VerifyDeviceRegistrationKey]";

        #endregion

        #region Initialize Device
        /// <summary>
        /// Initialize Device API
        /// </summary>
        /// <param name="deviceReg"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("InitializeDevice")]
        //[Authorize]
        public IActionResult InitializeDevice([FromBody] DeviceReg deviceReg)
        {
            Message msg = new Message();            
            try
            {
                DataTable dt = DataLogic.InitializeDevice(deviceReg, SP_DeviceInsertUpdateDelete);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {                        
                        msg.message = dt.Rows[0]["Message"].ToString(); 
                        msg.status = "200";
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
                msg.status = "401";
                return Ok(msg);
            }
        }
        #endregion

        #region Verify Device Lic Key

        /// <summary>
        /// Verify Device License Key API
        /// </summary>
        /// <param name="deviceReg"></param>
        /// <returns>Returns License Key</returns>
        [HttpPost("VerifyDeviceLicKey")]
        //[Authorize]
        public IActionResult VerifyDeviceLicKey([FromBody] DeviceReg deviceReg)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.VerifyDeviceLicKey(deviceReg.DeviceSerialNo, SP_VerifyDeviceRegistrationKey);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        var dbLicKey = dt.Rows[0]["LicKey"].ToString();
                        bool LicKeyVerify = EncryptDecryptPassword.ValidateKey(dbLicKey, deviceReg.DeviceSerialNo);
                        if (LicKeyVerify)
                        {
                            msg.message = "License key verified";
                            msg.status = "200";
                            return Ok(msg);
                        }
                        else
                        {
                            msg.message = LicKeyVerify ? "Device not registered!" : "Invalid license key";
                            msg.status = "401";
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

        #region Generate Lic Key

        /// <summary>
        /// Generate License Key API
        /// </summary>
        /// <param name="deviceReg"></param>
        /// <returns>Returns License Key</returns>
        [HttpPost("GenerateLicKey")]
        //[Authorize]
        public IActionResult GenerateLicKey([FromBody] DeviceReg deviceReg)
        {
            Message msg = new Message();
            try
            {
                var dbLicKey = "1300-3994-3868-8509";
                //var substringSerialNo = deviceReg.DeviceSerialNo.Substring(4);
                string LicKey = EncryptDecryptPassword.GenerateLicKey(dbLicKey, deviceReg.DeviceSerialNo);
                //string LicKey = EncryptDecryptPassword.Encrypt2("ABTAK56", substringSerialNo, 2);
                msg.message = LicKey;
                msg.status = "200";
                return Ok(msg);
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
