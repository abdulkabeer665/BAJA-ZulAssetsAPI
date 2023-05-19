using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using Microsoft.AspNetCore.Authorization;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {

        #region Declaration

        private static string SP_AssetTracking = "[dbo].[SP_AssetTracking]";
        private static string SP_AnonymousAssetInsetUpdate = "[dbo].[SP_AnonymousAssetInsetUpdate]";
        private static string SP_GetAllAnonymousAssets = "[dbo].[SP_GetAllAnonymousAssets]";
        private static string SP_UpdateAssetLocation = "[dbo].[SP_UpdateAssetLocation]";
        private static string SP_TransferData_BE_Temp = "[dbo].[SP_TransferDataFromBEToTemp]";
        private static string SP_GetAllAssetsFrom_Temp = "[dbo].[GetAllAssetsFromTemp]";
        private static string SP_GetAllAssetsStatus = "[dbo].[SP_GetAllAssetsStatus]";
        private static string SP_UpdateAssetStatusByBarocde = "[dbo].[SP_UpdateAssetStatusByBarocde]";

        #endregion

        #region Get All Assets

        /// <summary>
        /// Get All Assets
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpGet("GetAllAssets")]
        //[Authorize]
        public IActionResult GetAllAssets()
        {
            Message msg = new Message();
            try
            {

                DataTable dt = DataLogic.GetAllAssets(SP_GetAllAssetsFrom_Temp);

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
                        return Ok(dt);
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

        #region Asset Tracking

        /// <summary>
        /// Asset Tracking API
        /// </summary>
        /// <param name="assetTrackingReq"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("AssetTrackingByID")]
        [Authorize]
        public IActionResult AssetTrackingByID([FromBody] AssetTrackingRequest assetTrackingReq)
        {
            Message msg = new Message();
            AssetTrackingResponse astTrkRes = new AssetTrackingResponse();
            try
            {
                DataTable dt = DataLogic.AssetTracking(assetTrackingReq, SP_AssetTracking);
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
                        var status = dt.Rows[0]["Status"].ToString();

                        if (status == "200")
                        {
                            astTrkRes.Barcode = dt.Rows[0]["Barcode"].ToString();
                            astTrkRes.Status = status;
                            astTrkRes.AssestDescription = dt.Rows[0]["AssetDescription"].ToString();
                            astTrkRes.CatID = dt.Rows[0]["CatID"].ToString();
                            astTrkRes.AssetCategoryDescription = dt.Rows[0]["AssetCategoryDescription"].ToString();
                            astTrkRes.Custodian = dt.Rows[0]["Custodian"].ToString();
                            astTrkRes.AcquisitionPrice = dt.Rows[0]["AcquisitionPrice"].ToString();
                            astTrkRes.CostCenter = dt.Rows[0]["CostCenter"].ToString();
                            astTrkRes.CurrentBV = dt.Rows[0]["CurrentBV"].ToString();
                            astTrkRes.AssetPurchaseDate = dt.Rows[0]["AssetPurchaseDate"].ToString();
                            astTrkRes.LocID = dt.Rows[0]["LocID"].ToString();
                            astTrkRes.AssetLocationDescription = dt.Rows[0]["AssetLocationDescription"].ToString();
                            astTrkRes.Message = "";
                            return Ok(astTrkRes);
                        }
                        else
                        {
                            astTrkRes.Barcode = "";
                            astTrkRes.Status = status;
                            astTrkRes.Message = "Asset Not Found";
                            astTrkRes.AssestDescription = "";
                            astTrkRes.CatID = "";
                            astTrkRes.AssetCategoryDescription = "";
                            astTrkRes.LocID = "";
                            astTrkRes.AssetLocationDescription = "";
                            return Ok(astTrkRes);
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
                msg.status = "401";
                return Ok(msg);
            }
        }

        #endregion

        #region Get Assets Status

        /// <summary>
        /// Get All Assets Status
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpGet("GetAssetsStatus")]
        [Authorize]
        public IActionResult GetAssetsStatus()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllAssetsStatus(SP_GetAllAssetsStatus);
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
                        return Ok(dt);
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

        #region Update Asset Status By Barcode

        /// <summary>
        /// Update Asset Status By Barcode
        /// </summary>
        /// <returns>Returns a message "Asset Status Updated"</returns>
        [HttpPost("UpdateAssetStatusByBarcode")]
        [Authorize]
        public IActionResult UpdateAssetStatusByBarcode([FromBody] UpdateAssetStatus updAstStatus)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateAssetStatusByBarcode(updAstStatus, SP_UpdateAssetStatusByBarocde);
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
                        var status = dt.Rows[0]["StatusCode"].ToString();

                        if (status == "200")
                        {
                            msg.status = status;
                            msg.message = dt.Rows[0]["Message"].ToString();
                            return Ok(msg);
                        }
                        else
                        {
                            msg.status = status;
                            msg.message = dt.Rows[0]["Message"].ToString();
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
                msg.status = "401";
                return Ok(msg);
            }
        }

        #endregion

        #region Anonymous Asset 

        /// <summary>
        /// Anonymous Asset API
        /// </summary>
        /// <param name="anonymousAstReq"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("AnonymousAssets")]
        [Authorize]
        public IActionResult AnonymousAssets([FromBody] AnonymousAssetsRequests anonymousAstReq)
        {
            Message msg = new Message();
            AnonymousAssetResponse anonymousAstRes = new AnonymousAssetResponse();
            try
            {
                DataTable dt = DataLogic.AnonymousAssets(anonymousAstReq, SP_AnonymousAssetInsetUpdate);
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
                        var status = dt.Rows[0]["Status"].ToString();

                        if (status == "200")
                        {
                            anonymousAstRes.Status = status;
                            anonymousAstRes.Message = dt.Rows[0]["Message"].ToString();
                            return Ok(anonymousAstRes);
                        }
                        else
                        {
                            anonymousAstRes.Status = status;
                            anonymousAstRes.Message = dt.Rows[0]["Message"].ToString();
                            return Ok(anonymousAstRes);
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
                msg.status = "401";
                return Ok(msg);
            }
        }

        #endregion

        #region Get Anonymous Assets

        /// <summary>
        /// Asset Tracking API
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpGet("GetAllAnonymousAssets")]
        [Authorize]
        public IActionResult GetAllAnonymousAssets()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllAnonymousAssets(SP_GetAllAnonymousAssets);
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
                        return Ok(dt);
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

        #region Update Asset Location

        /// <summary>
        /// Update Asset Location
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("UpdateAssetLocation")]
        [Authorize]
        public IActionResult UpdateAssetLocation([FromBody] UpdateAssetLocation updAstLoc)
        {
            Message msg = new Message();
            UpdateAssetLocationResponse updAstLocRes = new UpdateAssetLocationResponse();
            try
            {
                DataTable dt = DataLogic.UpdateAssetLocation(updAstLoc, SP_UpdateAssetLocation);
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
                        updAstLocRes.Message = dt.Rows[0]["Message"].ToString();
                        updAstLocRes.Status = dt.Rows[0]["Status"].ToString();
                        return Ok(updAstLocRes);
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

        #region Transfer Assets From ZulAssetsBE to ZulAssetsBE_Temp



        #endregion

    }
}
