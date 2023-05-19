namespace ZulAssetsBackEnd_API.BAL
{
    public class RequestParameters
    {

        #region DeviceRegistrationParam

        public class DeviceReg
        {
            public int NewDeviceFlag { get; set; }
            public string DeviceDesc { get; set; }
            public string DeviceSerialNo { get; set; }
            
            
        }

        #endregion

        #region User Login Parameters

        #region Loginparam
        public class Loginparam
        {
            public string LoginName { get; set; }
            public string Password { get; set; }

        }
        #endregion

        #region ChangePassword
        public class ChangePassword
        {
            public string LoginName { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
        #endregion

        #region ForgotPassword
        public class ForgotPassword
        {
            public string LoginName { get; set; }
        }
        #endregion

        #region RefreshTokenRequest

        public class RefreshTokenRequest
        {
            public string JWTToken { get; set; }
            public string RefreshToken { get; set; }
        }

        #endregion

        #endregion

        #region Assets Parameters

        #region Asset Tracking

        public class AssetTrackingRequest
        {
            public string Barcode { get; set; }
        }

        #endregion

        #region Anonymous Assets

        public class AnonymousAssetsRequests
        {
            public int ID { get; set; }
            public string DeviceID { get; set; }
            public string LocID { get; set; }
            public string AssetDescription { get; set; }
            public string CatID { get; set; }
        }

        #endregion

        #region Update Asset Location

        public class UpdateAssetLocation
        {
            public string Barcode { get; set; }
            public string LocID { get; set; }
            public string DeviceID { get; set; }
            public string InventoryDate { get; set; }
            public string LastEditDate { get; set; }
            public string LastEditBy { get; set; }
            public string Status { get; set; }
        }

        #endregion

        #region Asset Status Update

        public class UpdateAssetStatus
        {
            public string Barcode { get; set; }
            public string AssetStatus { get; set; }
        }
        #endregion

        #endregion

        #region Location Parameters

        public class LocationRequest
        {
            public string LocID { get; set; }
            public int From { get; set; }
            public int To { get; set; }
        }

        #endregion

    }
}
