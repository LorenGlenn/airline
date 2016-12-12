using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Airline
{
  public class Flight
  {
    private int _departTime;
    private string _departCity;
    private string _destination;
    private string _status;
    private int _id;

    public Flight(int DepartTime, string DepartCity, string Destination, string Status, int Id = 0)
    {
      _departTime = DepartTime;
      _departCity = DepartCity;
      _destination = Destination;
      _status = Status;
      _id = Id;
    }

    public override bool Equals(System.Object otherFlight)
    {
      if(!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool idEquality = (this.GetId() == newFlight.GetId());
        return(idEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetDepartCity()
    {
      return _departCity;
    }

    public int GetDepartTime()
    {
      return _departTime;
    }

    public string GetDestination()
    {
      return _destination;
    }

    public string GetStatus()
    {
      return _status;
    }

    public SqlParameter SetParameter(string ParamName)
    {
      SqlParameter tempParameter = new SqlParameter();
      tempParameter.ParameterName = ParamName;
      tempParameter.Value = this.GetDepartTime();
      return tempParameter;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO flights (departure_time, departure_city, destination, status) OUTPUT INSERTED.id VALUES (@DepartTime, @DepartCity, @Destination, @Status)", conn);

      SqlParameter depart_timeParameter = SetParameter("@DepartTime");
      cmd.Parameters.Add(depart_timeParameter);

      SqlParameter depart_cityParameter = SetParameter("@DepartCity");
      cmd.Parameters.Add(depart_cityParameter);

      SqlParameter destinationParameter = SetParameter("@Destination");
      cmd.Parameters.Add(destinationParameter);

      SqlParameter statusParameter = SetParameter("@Status");
      cmd.Parameters.Add(statusParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static List<Flight> GetAll()
    {

      List<Flight> allFlights = new List<Flight>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        int departTime = rdr.GetInt32(1);
        string departCity = rdr.GetString(2);
        string destination = rdr.GetString(3);
        string status = rdr.GetString(4);
        Flight newFlight = new Flight(departTime, departCity, destination, status, flightId);
        allFlights.Add(newFlight);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allFlights;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM flights;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
