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