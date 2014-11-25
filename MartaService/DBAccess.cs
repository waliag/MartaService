using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MartaService
{
    public class DBAccess
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString;
        SqlConnection connection = new SqlConnection(connectionString);

        public SandySpringsSchedule getSandySpringsSchedule()
        {
            SandySpringsSchedule timeObj;
            timeObj = new SandySpringsSchedule();

            string sql = "sp_getSandySpringsSchedule";
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        DateTime currentTime = DateTime.Now;

                        while (reader.Read())
                        {

                            DateTime northbound = Convert.ToDateTime(Convert.ToString(reader["NorthBound"]));
                            DateTime southbound = Convert.ToDateTime(Convert.ToString(reader["SouthBound"]));
                            if ((DateTime.Compare(northbound, currentTime) > 0) &&
                                (DateTime.Compare(southbound, currentTime) > 0))
                            {
                                timeObj.northBound = Convert.ToString(reader["NorthBound"]);
                                timeObj.southBound = Convert.ToString(reader["SouthBound"]);
                            }
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            connection.Close();
            return timeObj;
        }
        public FivePointsSchedule getFivePointsSchedule()
        {
            FivePointsSchedule timeObj;
            timeObj = new FivePointsSchedule();

            string sql = "sp_getFivePointsSchedule";
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        DateTime currentTime = DateTime.Now;

                        while (reader.Read())
                        {

                            DateTime northbound = Convert.ToDateTime(Convert.ToString(reader["NorthBound"]));
                            DateTime southbound = Convert.ToDateTime(Convert.ToString(reader["SouthBound"]));
                            //string southbound = Convert.ToString(reader["SouthBound"]);
                            if ((DateTime.Compare(northbound, currentTime) > 0) &&
                                (DateTime.Compare(southbound, currentTime) > 0))
                            {
                                timeObj.northBound = Convert.ToString(reader["NorthBound"]);
                                timeObj.southBound = Convert.ToString(reader["SouthBound"]);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            connection.Close();
            return timeObj;
        }
    }
}