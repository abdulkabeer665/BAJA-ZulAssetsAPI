using System.Data;
using System.Data.SqlClient;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;

namespace ZulAssetsBackEnd_API.DAL
{

    public class DataLogic
    {

        #region Device Logics

        #region DeviceRegistration

        public static DataTable InitializeDevice(DeviceReg deviceReg, string Storeprocedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] parameter =
                {
                    new SqlParameter("@NewDeviceFlag",deviceReg.NewDeviceFlag),
                    new SqlParameter("@HardwareID", deviceReg.DeviceSerialNo),
                    new SqlParameter("@DeviceDesc", deviceReg.DeviceDesc)
                };
                return CGD.DTWithParam(Storeprocedure, parameter, 1);
            }
            catch (Exception)
            {

                return new DataTable();
            }
        }

        #endregion

        #region VerifyDeviceLicKey

        public static DataTable VerifyDeviceLicKey(string HardwareID, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@HardwareID", HardwareID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Login & Password Related Logics

        #region LoginDetails

        public static DataTable LoginDetails(Loginparam loginParam, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", loginParam.LoginName),
                new SqlParameter ("@Pass", loginParam.Password)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region UpdateRefreshToken

        public static DataTable UpdateRefreshToken(string loginName, string refreshToken, string StoredProcedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@LoginName",loginName),
                    new SqlParameter ("@RefreshToken",refreshToken)
                };

                DataTable dt = CGD.DTWithParam(StoredProcedure, sqlParameters, 1);
                return dt;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return new DataTable(errorMessage);
            }
        }

        #endregion

        #region Get User By ID

        public static DataTable GetUserByID(string loginName, string StoreProcedure)
        {
            try
            {
                DbReports CGD = new DbReports();

                SqlParameter[] sqlParameters =
                {
                    new SqlParameter("@LoginName",loginName),
                };
                return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
            }
            catch (Exception ex)
            {
                return new DataTable(ex.Message);
            }
        }

        #endregion

        #region LoginDetails

        public static DataSet GetDetails(string loginName, string StoredProcedure)
        {
            try
            {
                DbReports CGD = new DbReports();
                SqlParameter[] sqlParameters = {
                    new SqlParameter ("@LoginName", loginName)
                };

                DataSet ds = CGD.DSWithParam(StoredProcedure, sqlParameters, 1);
                return ds;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return new DataSet(errorMessage);
            }
        }

        #endregion

        #region VerifyOldPassword

        public static DataTable VerifyOldPassword(ChangePassword changePassword, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@LoginName", changePassword.LoginName),
                new SqlParameter ("@OldPassword", changePassword.OldPassword),
                new SqlParameter ("@NewPassword", changePassword.NewPassword)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Assets Logics

        #region Asset Tracking

        public static DataTable AssetTracking(AssetTrackingRequest astTrkReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@Barcode", astTrkReq.Barcode)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Anonymous Assets

        public static DataTable AnonymousAssets(AnonymousAssetsRequests anonymousAstReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@ID", anonymousAstReq.ID),
                new SqlParameter ("@AstDesc", anonymousAstReq.AssetDescription),
                new SqlParameter ("@LocID", anonymousAstReq.LocID),
                new SqlParameter ("@AstCatID", anonymousAstReq.CatID),
                new SqlParameter ("@DeviceID", anonymousAstReq.DeviceID)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        public static DataTable GetAllAnonymousAssets(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Update Asset Location

        public static DataTable UpdateAssetLocation(UpdateAssetLocation updAstLoc, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter ("@Barcode", updAstLoc.Barcode),
                new SqlParameter ("@LocID", updAstLoc.LocID),
                new SqlParameter ("@DeviceID", updAstLoc.DeviceID),
                new SqlParameter ("@InventoryDate", updAstLoc.InventoryDate),
                new SqlParameter ("@LastEditDate", updAstLoc.LastEditDate),
                new SqlParameter ("@LastEditBy", updAstLoc.LastEditBy),
                new SqlParameter ("@Status", updAstLoc.Status)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Locations Logics

        #region Get All Locations

        public static DataTable GetAllLocations(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {};
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Get Location By ID

        public static DataSet GetAssetsByLocationID(LocationRequest locReq, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                 new SqlParameter ("@LocID", locReq.LocID),
                 new SqlParameter ("@From", locReq.From),
                 new SqlParameter ("@To", locReq.To)
            };
            return CGD.DSWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #endregion

        #region Category Logics

        #region Get All Category

        public static DataTable GetAllCategory(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #endregion

        #region Get All Assets Status

        public static DataTable GetAllAssetsStatus(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

        #region Update Asset Status By Barcode

        public static DataTable UpdateAssetStatusByBarcode(UpdateAssetStatus updAstStatus, string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters = {
                 new SqlParameter ("@Barcode", updAstStatus.Barcode),
                 new SqlParameter ("@AssetStatus", updAstStatus.AssetStatus)
            };
            return CGD.DTWithParam(StoreProcedure, sqlParameters, 1);
        }

        #endregion

        #region Get All Assets

        public static DataTable GetAllAssets(string StoreProcedure)
        {
            DbReports CGD = new DbReports();
            SqlParameter[] sqlParameters =
            { };
            return CGD.DTWithOutParam(StoreProcedure, 1);
        }

        #endregion

    }

}
