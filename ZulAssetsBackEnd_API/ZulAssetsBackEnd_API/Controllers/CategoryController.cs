using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        #region Declaration

        private static string SP_GetAllCategories = "[dbo].[SP_GetAllCategories]";
        private static string SP_GetLocationByLocID = "[dbo].[SP_GetLocationByLocID]";

        #endregion

        #region All Category
        /// <summary>
        /// Get All Category API
        /// </summary>
        /// <returns>Returns a message ""</returns>
        [HttpGet("GetAllCategory")]
        //[Authorize]
        public IActionResult GetAllCategory()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllCategory(SP_GetAllCategories);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        //msg.status = "401";
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

    }
}
