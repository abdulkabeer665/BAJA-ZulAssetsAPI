using System.Data;
using System.Data.SqlClient;

namespace ZulAssetsBackEnd_API.DAL
{
    public class DbReports
    {

        #region Declaration

        Connection con;
        DataTable dt;
        DataSet ds;
        SqlDataAdapter da;
        SqlCommand cmd;
        string query = string.Empty;

        #endregion

        #region With Parameters

        public DataTable DTWithParam(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    //cmd.Parameters.AddWithValue("@ID", ID);
                    //cmd.Parameters.AddWithValue("@from", from);
                    //cmd.Parameters.AddWithValue("@to", to);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataTable dt2 = new DataTable();
                DataRow dtRow = dt2.NewRow();
                dt2.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt2.Rows.Add(dtRow);
                return dt2;
            }
        }

        #endregion

        #region Without Parameters

        public DataTable DTWithOutParam(string storedProcedure, int connect)
        {
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataTable dt2 = new DataTable();
                DataRow dtRow = dt2.NewRow();
                dt2.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt2.Rows.Add(dtRow);
                return dt2;
            }
        }

        #endregion

        #region DataSet With Parameters

        public DataSet DSWithParam(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    //cmd.Parameters.AddWithValue("@ID", ID);
                    //cmd.Parameters.AddWithValue("@from", from);
                    //cmd.Parameters.AddWithValue("@to", to);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return new DataSet(errorMessage);
            }
        }

        #endregion

    }
}
