using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using Microsoft.AspNetCore.Authorization;
using ZulAssetsBackEnd_API.BAL;

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
        private static string SP_TransferDataFromBEToDevice_Offline = "[dbo].[SP_TransferDataFromBEToDevice_Offline]";
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
        //[Authorize]
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
                            //astTrkRes.Barcode = dt.Rows[0]["Barcode"].ToString();
                            //astTrkRes.Status = status;
                            //astTrkRes.AssestDescription = dt.Rows[0]["AssetDescription"].ToString();
                            //astTrkRes.CatID = dt.Rows[0]["CatID"].ToString();
                            //astTrkRes.AssetCategoryDescription = dt.Rows[0]["AssetCategoryDescription"].ToString();
                            //astTrkRes.Custodian = dt.Rows[0]["Custodian"].ToString();
                            //astTrkRes.AcquisitionPrice = dt.Rows[0]["AcquisitionPrice"].ToString();
                            //astTrkRes.CostCenter = dt.Rows[0]["CostCenter"].ToString();
                            //astTrkRes.CurrentBV = dt.Rows[0]["CurrentBV"].ToString();
                            //astTrkRes.AssetPurchaseDate = dt.Rows[0]["AssetPurchaseDate"].ToString();
                            //astTrkRes.LocID = dt.Rows[0]["LocID"].ToString();
                            //astTrkRes.AssetLocationDescription = dt.Rows[0]["AssetLocationDescription"].ToString();
                            //astTrkRes.Message = "";
                            return Ok(dt);
                        }
                        else
                        {
                            //astTrkRes.Barcode = "";
                            //astTrkRes.Status = status;
                            //astTrkRes.Message = "Asset Not Found";
                            //astTrkRes.AssestDescription = "";
                            //astTrkRes.CatID = "";
                            //astTrkRes.AssetCategoryDescription = "";
                            //astTrkRes.LocID = "";
                            //astTrkRes.AssetLocationDescription = "";
                            return Ok(dt);
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
        //[Authorize]
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
            //var logResult1 = GeneralFunctions.CreateAndWriteToFile("Testing Brother ", "Barcodes", updAstLoc.Barcode);
            //var logResult2 = GeneralFunctions.CreateAndWriteToFile("Testing Brother ", "LocIDs", updAstLoc.LocID);
            //var logResult3 = GeneralFunctions.CreateAndWriteToFile("Testing Brother ", "Status", updAstLoc.Status);
            //var logResult4 = GeneralFunctions.CreateAndWriteToFile("Testing Brother ", "Asset Status", updAstLoc.AssetStatus);
            Message msg = new Message();
            DataTable dt = new DataTable();
            UpdateAssetLocationResponse updAstLocRes = new UpdateAssetLocationResponse();
            try
            {

                var length = updAstLoc.Barcode.Split("|");
                for (int i = 0; i < length.Length; i++)
                {
                    var barcode = updAstLoc.Barcode.Split("|")[i];
                    var LocID = updAstLoc.LocID.Split("|")[i];
                    var DeviceID = updAstLoc.DeviceID;
                    var InventoryDate = updAstLoc.InventoryDate;
                    var LastEditDate = updAstLoc.LastEditDate;
                    var LastEditBy = updAstLoc.LastEditBy;
                    var Status = updAstLoc.Status.Split("|")[i];
                    var AssetStatus = updAstLoc.AssetStatus.Split("|")[i];

                    UpdateAssetLocation updAstLocRes2 = new UpdateAssetLocation();

                    updAstLocRes2.Status = Status;
                    updAstLocRes2.AssetStatus = AssetStatus;
                    updAstLocRes2.Barcode = barcode;
                    updAstLocRes2.LocID = LocID;
                    updAstLocRes2.DeviceID = DeviceID;
                    updAstLocRes2.InventoryDate = InventoryDate;
                    updAstLocRes2.LastEditDate = LastEditDate;
                    updAstLocRes2.LastEditBy = LastEditBy;

                    dt = DataLogic.UpdateAssetLocation(updAstLocRes2, SP_UpdateAssetLocation);

                }

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
                        LocationRequest locReq = new LocationRequest();
                        locReq.LocID = updAstLoc.LocID.Split("|")[0];
                        var _1 = new LocationsController().GetAssetsByLocationID(locReq);
                        updAstLocRes.Message = dt.Rows[0]["Message"].ToString();
                        updAstLocRes.Status = dt.Rows[0]["Status"].ToString();
                        updAstLocRes.dt = _1;
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
                return Ok(msg);
            }
        }

        #endregion

        #region Transfer Assets From ZulAssetsBE to ZulAssetsBE_Temp

        /// <summary>
        /// Transfer Assets From BE To Temp
        /// </summary>
        /// <returns>Returns a DataSet with a message and status as well</returns>
        [HttpPost("TransferFromBEToTemp")]
        [Authorize]
        public IActionResult TransferFromBEToTemp(string deviceID)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.TransferAssetsFromBEToTemp(deviceID, SP_TransferData_BE_Temp);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        DataSet newDS = new DataSet();
                        DataTable newDT = new DataTable();
                        DataRow newDR = newDT.NewRow();
                        newDT.Columns.Add("ErrorMessage");
                        newDR["ErrorMessage"] = msg.message;
                        newDT.Rows.Add(newDR);
                        newDS.Tables.Add(newDT);
                        return Ok(newDS);
                    }
                    else
                    {
                        return Ok(ds);
                    }
                }
                else
                {
                    return Ok(ds);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Transfer Assets From Backend to Device SQLite Database

        /// <summary>
        /// Transfer Assets From BE To Temp
        /// </summary>
        /// <returns>Returns a DataSet with a message and status as well</returns>
        [HttpPost("TransferFromBEToDeviceOffline")]
        //[Authorize]
        public IActionResult TransferFromBEToDeviceOffline(string deviceID)
        {
            Message msg = new Message();
            try
            {
                
                DataSet ds = DataLogic.TransferAssetsFromBEToDeviceOffline(deviceID, SP_TransferDataFromBEToDevice_Offline);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        DataSet newDS = new DataSet();
                        DataTable newDT = new DataTable();
                        DataRow newDR = newDT.NewRow();
                        newDT.Columns.Add("ErrorMessage");
                        newDR["ErrorMessage"] = msg.message;
                        newDT.Rows.Add(newDR);
                        newDS.Tables.Add(newDT);
                        return Ok(newDS);
                    }
                    else
                    {
                        return Ok(ds);
                    }
                }
                else
                {
                    return Ok(ds);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Export Audit Data To BETemp 

        /// <summary>
        /// Export Audit Data to BETemp
        /// </summary>
        /// <param name="exportAuditDataReqParam"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("ExportDataToBE")]
        //[Authorize]
        public IActionResult ExportDataToBE([FromBody] ExportAuditDataReqParam exportAuditDataReqParam)
        {
            Message msg = new Message();
            AnonymousAssetResponse anonymousAstRes = new AnonymousAssetResponse();
            try
            {

                DataTable AuditListToDT = new DataTable();
                AuditListToDT.Columns.Add("AssetStatus"); AuditListToDT.Columns.Add("Barcode"); AuditListToDT.Columns.Add("DeviceID");
                AuditListToDT.Columns.Add("Id"); AuditListToDT.Columns.Add("InventoryDate"); AuditListToDT.Columns.Add("LastEditBy");
                AuditListToDT.Columns.Add("LastEditDate"); AuditListToDT.Columns.Add("LocID"); AuditListToDT.Columns.Add("AuditStatus");
                AuditListToDT.Columns.Add("AstID");

                if (AuditListToDT != null)
                {
                    AuditListToDT = ListintoDataTable.ToDataTable(exportAuditDataReqParam.AuditTable);
                }

                DataTable dt2 = new DataTable();

                dt2.Columns.Add("AssetStatus"); dt2.Columns.Add("Barcode"); dt2.Columns.Add("DeviceID");
                dt2.Columns.Add("Id"); dt2.Columns.Add("InventoryDate"); dt2.Columns.Add("LastEditBy");
                dt2.Columns.Add("LastEditDate"); dt2.Columns.Add("LocID"); dt2.Columns.Add("AuditStatus");
                dt2.Columns.Add("AstID");

                for (int i = 0; i < AuditListToDT.Rows.Count; i++)
                {

                    var length = AuditListToDT.Rows[i]["Barcode"].ToString().Split("|");

                    var count1 = AuditListToDT.Rows[i]["Barcode"].ToString().Split("|").Count();

                    for (int j = 0; j < length.Length; j++)
                    {
                            
                        DataRow dtRow = dt2.NewRow();
                        dtRow["AssetStatus"] = AuditListToDT.Rows[i]["AssetStatus"].ToString().Split("|")[j];
                        dtRow["Barcode"] = AuditListToDT.Rows[i]["Barcode"].ToString().Split("|")[j];
                        dtRow["DeviceID"] = AuditListToDT.Rows[i]["DeviceID"].ToString();
                        dtRow["Id"] = AuditListToDT.Rows[i]["Id"].ToString();
                        dtRow["InventoryDate"] = AuditListToDT.Rows[i]["InventoryDate"].ToString();
                        dtRow["LastEditBy"] = AuditListToDT.Rows[i]["LastEditBy"].ToString();
                        dtRow["LastEditDate"] = AuditListToDT.Rows[i]["LastEditDate"].ToString();
                        dtRow["LocID"] = AuditListToDT.Rows[i]["LocID"].ToString().Split("|")[j];
                        dtRow["AuditStatus"] = AuditListToDT.Rows[i]["AuditStatus"].ToString().Split("|")[j];
                        dtRow["AstID"] = "";
                        dt2.Rows.Add(dtRow);

                    }
                }

                DataTable dt = DataLogic.InsertExportedData(dt2, "[dbo].[SP_TransferDataFromDeviceToBE]");

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

        #region Export Anonymous Data To BETemp 

        /// <summary>
        /// Export Anonymous Data to BETemp
        /// </summary>
        /// <param name="exportAnonymousDataReqParam"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("ExportDataToBEAnonymous")]
        //[Authorize]
        public IActionResult ExportDataToBEAnonymous([FromBody] ExportAnonymousDataReqParam exportAnonymousDataReqParam)
        {
            Message msg = new Message();
            AnonymousAssetResponse anonymousAstRes = new AnonymousAssetResponse();
            try
            {

                DataTable anonymousAssetsCount = DataLogic.AnonymousAssetsCount("[dbo].[SP_CountAnonymousAssets]");

                DataTable AnonymousListToDT = new DataTable();
                AnonymousListToDT.Columns.Add("NonBCode");
                AnonymousListToDT.Columns.Add("DeviceID"); AnonymousListToDT.Columns.Add("LocID"); AnonymousListToDT.Columns.Add("AstCatID");
                AnonymousListToDT.Columns.Add("AstDesc"); AnonymousListToDT.Columns.Add("TransDate");

                if (AnonymousListToDT != null)
                {
                    AnonymousListToDT = ListintoDataTable.ToDataTable(exportAnonymousDataReqParam.AnonymousTable);
                }
                
                int anonymousAssetsCountInt = Convert.ToInt32(anonymousAssetsCount.Rows[0]["AnonymousAssetsCount"].ToString());
                
                for (int i = 0; i < AnonymousListToDT.Rows.Count; i++)
                {
                    AnonymousListToDT.Rows[i]["NonBCode"] = anonymousAssetsCountInt + (i + 1);
                }

                DataTable dt = DataLogic.InsertAnonymousExportedData(AnonymousListToDT, "[dbo].[SP_TransferDataFromDeviceToBEAnonymous]");

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
                        //var status = dt.Rows[0]["Status"].ToString();

                        //if (status == "200")
                        //{
                        //    msg.status = status;
                        //    msg.message = dt.Rows[0]["Message"].ToString();
                        //    return Ok(msg);
                        //}
                        //else
                        //{
                        //    msg.status = status;
                        //    msg.message = dt.Rows[0]["Message"].ToString();
                        //    return Ok(msg);
                        //}
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

        #region Export Asset Status Data To BETemp 

        /// <summary>
        /// Export Anonymous Data to BETemp
        /// </summary>
        /// <param name="exportAssetStatusDataReqParam"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("ExportDataToBEAssetStatus")]
        //[Authorize]
        public IActionResult ExportDataToBEAnonymous([FromBody] ExportAssetStatusDataReqParam exportAssetStatusDataReqParam)
        {
            Message msg = new Message();
            AnonymousAssetResponse anonymousAstRes = new AnonymousAssetResponse();
            try
            {

                //DataTable anonymousAssetsCount = DataLogic.AnonymousAssetsCount("[dbo].[SP_CountAnonymousAssets]");

                DataTable AssetStatusListToDT = new DataTable();
                AssetStatusListToDT.Columns.Add("AstID"); AssetStatusListToDT.Columns.Add("Barcode"); AssetStatusListToDT.Columns.Add("AssetStatus");
                AssetStatusListToDT.Columns.Add("DeviceID");

                if (AssetStatusListToDT != null)
                {
                    AssetStatusListToDT = ListintoDataTable.ToDataTable(exportAssetStatusDataReqParam.AssetStatusTable);
                }

                DataTable dt = DataLogic.InsertAssetStatusExportedData(AssetStatusListToDT, "[dbo].[SP_TransferDataFromDeviceToBEAssetStatus]");

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

        #region Delete Asset

        /// <summary>
        /// Delete Ast
        /// </summary>
        /// <param name="delAst"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("DeleteAsset")]
        //[Authorize]
        public IActionResult DeleteAsset([FromBody] DelAst delAst)
        {
            Message msg = new Message();
            AnonymousAssetResponse anonymousAstRes = new AnonymousAssetResponse();
            try
            {
                DataTable dt = DataLogic.DeleteAst(delAst, "[dbo].[SP_DeleteAstFromTempDB]");
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

                        msg.status = status;
                        msg.message = dt.Rows[0]["Message"].ToString();
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

    }
}
