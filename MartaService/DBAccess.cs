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

        /*
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
                                break;
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
        }*/
        public TrainSchedule getTrainSchedule(string stationName)
        {
            TrainSchedule timeObj;
            timeObj = new TrainSchedule();

            string sqlNorthBound;
            string sqlSouthBound;
            if (stationName == "FivePoints")
            {
                sqlNorthBound = "sp_getFivePointsNorthBoundSchedule";
                sqlSouthBound = "sp_getFivePointsSouthBoundSchedule";
            }
            else
            {
                sqlNorthBound = "sp_getSandySpringsNorthBoundSchedule";
                sqlSouthBound = "sp_getSandySpringsSouthBoundSchedule";
            }

            DateTime currentTime = DateTime.Now;

            //Execute query for northbound data
            SqlCommand command = new SqlCommand(sqlNorthBound, connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        DateTime firstNorthBoundTrain = Convert.ToDateTime(Convert.ToString(reader["NorthBound"]));

                        do
                        {
                            DateTime northbound = Convert.ToDateTime(Convert.ToString(reader["NorthBound"]));
                                                 
                            if (DateTime.Compare(northbound, currentTime) > 0)
                            {
                                timeObj.northBound = northbound.ToString("hh:mm:ss tt");
                                break;
                            }

                        } while (reader.Read());

                        if(timeObj.northBound == null)
                        {
                            timeObj.northBound = firstNorthBoundTrain.ToString("hh:mm:ss tt");
                        }
                       
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            connection.Close();

            //Execute query for southbound data
            command = new SqlCommand(sqlSouthBound, connection);
            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                       DateTime firstSouthBoundTrain = Convert.ToDateTime(Convert.ToString(reader["SouthBound"]));

                        do
                        {
                            DateTime southbound = Convert.ToDateTime(Convert.ToString(reader["SouthBound"]));

                            if (DateTime.Compare(southbound, currentTime) > 0)
                            {
                                timeObj.southBound = southbound.ToString("hh:mm:ss tt");
                                break;
                            }

                        } while (reader.Read());

                        if (timeObj.southBound == null)
                        {
                            timeObj.southBound = firstSouthBoundTrain.ToString("hh:mm:ss tt");
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