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

    public void AddCity_JoinTable(string departureName, string destinationName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand id_finder = new SqlCommand("SELECT id FROM cities WHERE name = @DepartureCityName;", conn);
      SqlParameter departureIdParameter = new SqlParameter();
      departureIdParameter.ParameterName = "@DepartureCityName";
      departureIdParameter.Value = departureName;
      id_finder.Parameters.Add(departureIdParameter);
      SqlDataReader dept = id_finder.ExecuteReader();

      int foundDepartureId = 0;
      while(dept.Read())
     {
       foundDepartureId = dept.GetInt32(0);
     }
     if (dept != null)
     {
       dept.Close();
     }

      SqlCommand destinationId_finder = new SqlCommand("SELECT id FROM cities WHERE name = @DestinationCityName;", conn);
      SqlParameter destinationIdParameter = new SqlParameter();
      destinationIdParameter.ParameterName = "@DestinationCityName";
      destinationIdParameter.Value = destinationName;
      destinationId_finder.Parameters.Add(destinationIdParameter);
      SqlDataReader dest = destinationId_finder.ExecuteReader();

      int foundDestinationId = 0;
      while(dest.Read())
     {
       foundDestinationId = dest.GetInt32(0);
     }
     if (dest != null)
     {
       dest.Close();
     }

      SqlCommand cmd = new SqlCommand("INSERT INTO cities_flights (flight_id, departure_id, destination_id) VALUES (@FlightId, @DepartureId, @DestinationId);", conn);

      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = this.GetId();
      cmd.Parameters.Add(flightIdParameter);

      SqlParameter departureCityIdParameter = new SqlParameter();
      departureCityIdParameter.ParameterName = "@DepartureId";
      departureCityIdParameter.Value = foundDepartureId.ToString();
      cmd.Parameters.Add(departureCityIdParameter);

      SqlParameter destinationCityIdParameter = new SqlParameter();
      destinationCityIdParameter.ParameterName = "@DestinationId";
      destinationCityIdParameter.Value = foundDestinationId.ToString();
      cmd.Parameters.Add(destinationCityIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }

    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO flights (departure_time, departure_city, destination, status) OUTPUT INSERTED.id VALUES (@DepartTime, @DepartCity, @Destination, @Status)", conn);

      SqlParameter depart_timeParameter = new SqlParameter();
      depart_timeParameter.ParameterName = "@DepartTime";
      depart_timeParameter.Value = this.GetDepartTime();
      cmd.Parameters.Add(depart_timeParameter);

      SqlParameter depart_cityParameter = new SqlParameter();
      depart_cityParameter.ParameterName = "@DepartCity";
      depart_cityParameter.Value = this.GetDepartCity();
      cmd.Parameters.Add(depart_cityParameter);

      SqlParameter destinationParameter = new SqlParameter();
      destinationParameter.ParameterName = "@Destination";
      destinationParameter.Value = this.GetDestination();
      cmd.Parameters.Add(destinationParameter);

      SqlParameter statusParameter = new SqlParameter();
      statusParameter.ParameterName = "Status";
      statusParameter.Value = this.GetStatus();
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

    public static Flight Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);
      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = id.ToString();
      cmd.Parameters.Add(flightIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundFlightId = 0;
      int foundDepartTime = 0;
      string foundDepartCity = null;
      string foundDestination = null;
      string foundStatus = null;

      while(rdr.Read())
      {
        foundFlightId = rdr.GetInt32(0);
        foundDepartTime = rdr.GetInt32(1);
        foundDepartCity = rdr.GetString(2);
        foundDestination = rdr.GetString(3);
        foundStatus = rdr.GetString(4);
      }
      Flight foundFlight = new Flight(foundDepartTime, foundDepartCity, foundDestination, foundDestination, foundFlightId);
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundFlight;
    }

    // public List<int> Temp(int id)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT * FROM cities_flights WHERE flight_id = @FlightId;", conn);
    //   SqlParameter flightIdParameter = new SqlParameter();
    //   flightIdParameter.ParameterName = "@FlightId";
    //   flightIdParameter.Value = id.ToString();
    //   cmd.Parameters.Add(flightIdParameter);
    //
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   int joinId = 0;
    //   int foundFlightId = 0;
    //   int foundDepartureId = 0;
    //   int foundDestinationId = 0;
    //
    //   while(rdr.Read())
    //   {
    //     joinId = rdr.GetInt32(0);
    //     foundFlightId = rdr.GetInt32(1);
    //     foundDepartureId = rdr.GetInt32(2);
    //     foundDestinationId = rdr.GetInt32(3);
    //   }
    //
    //   List<int> foundIds = new List<int> {foundFlightId, foundDepartureId, foundDestinationId};
    //   if(rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if(conn != null)
    //   {
    //     conn.Close();
    //   }
    //   return foundIds;
    // }


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
