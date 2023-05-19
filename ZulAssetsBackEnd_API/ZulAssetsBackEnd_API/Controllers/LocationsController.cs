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
    public class LocationsController : ControllerBase
    {

        #region Declaration

        private static string SP_AllLocations = "[dbo].[SP_AllLocation]";
        private static string SP_GetAssetsByLocationID = "[dbo].[SP_GetAssetsByLocID]";

        #endregion

        #region All Locations
        /// <summary>
        /// Get All Locations API
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpGet("GetAllLocations")]
        //[Authorize]
        public IActionResult GetAllLocations()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllLocations(SP_AllLocations);
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
                return Ok(msg);
            }
        }

        #endregion

        #region Location By ID
        /// <summary>
        /// Get Location By ID API
        /// </summary>
        /// <returns>Returns a detail against Loc ID</returns>
        [HttpPost("GetAssetsByLocationID")]
        //[Authorize]
        public IActionResult GetAssetsByLocationID([FromBody] LocationRequest locReq)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAssetsByLocationID(locReq, SP_GetAssetsByLocationID);
                //DataTable dt = DataLogic.GetLocationByID(locReq, SP_GetLocationByLocID);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        DataSet newDS = new DataSet();
                        DataTable newDT = new DataTable();
                        DataRow newDR = newDT.NewRow();
                        newDT.Columns.Add("ErrorMessage");
                        newDT.Columns.Add("Status");
                        newDR["ErrorMessage"] = msg.message;
                        newDR["Status"] = msg.status;
                        newDT.Rows.Add(newDR);
                        //newDS.Tables.Add(newDT);
                        return Ok(newDT);
                    }
                    else
                    {

                        DataTable dtforDSTable1 = new DataTable();
                        DataTable dtforDSTable2 = new DataTable();
                        dtforDSTable1 = ds.Tables[0];
                        dtforDSTable2 = ds.Tables[1];

                        List<DataRow> rows_to_remove = new List<DataRow>();
                        foreach (DataRow row1 in dtforDSTable1.Rows)
                        {
                            foreach (DataRow row2 in dtforDSTable2.Rows)
                            {
                                if (row1["Barcode"].ToString() == row2["Barcode"].ToString())
                                {
                                    rows_to_remove.Add(row1);
                                }
                            }
                        }

                        foreach (DataRow row in rows_to_remove)
                        {
                            dtforDSTable1.Rows.Remove(row);
                            dtforDSTable1.AcceptChanges();
                        }

                        dtforDSTable1.Merge(dtforDSTable2);

                        return Ok(dtforDSTable1);

                    }
                }
                else
                {
                    DataTable noDataDT = new DataTable();
                    noDataDT = ds.Tables[0];
                    return Ok(noDataDT);
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
