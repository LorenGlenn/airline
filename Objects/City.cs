using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Airline
{
  public class City
  {
    private string _cityName;
    private int _id;

    public City(string CityName, int Id = 0)
    {
      _cityName = CityName;
      _id = Id;
    }

    public override bool Equals(System.Object otherCity)
    {
      if(!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool idEquality = (this.GetId() == newCity.GetId());
        return(idEquality);
      }
    }

    public string GetCityName()
    {
      return _cityName;
    }

    public int GetId()
    {
      return _id;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@Name)", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@Name";
      nameParameter.Value = this.GetCityName();
      cmd.Parameters.Add(nameParameter);


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

    public static City Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities WHERE id = @CityId;", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cityIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string foundName = null;

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundName = rdr.GetString(1);

      }
      City foundCity = new City(foundName, foundId);
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundCity;
    }

    public List<Flight> GetFlights()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT flight_id FROM cities_flights WHERE destination_id = @CityId OR departure_id = @CityId", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = this.GetId();
      cmd.Parameters.Add(cityIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> flightIds = new List<int> {};
      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        flightIds.Add(flightId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      List<Flight> flights = new List<Flight>{};
      foreach(int flightId in flightIds)
      {
        SqlCommand flightQuery = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);

        SqlParameter flightIdParameter = new SqlParameter();
        flightIdParameter.ParameterName = "@FlightId";
        flightIdParameter.Value = flightId;
        flightQuery.Parameters.Add(flightIdParameter);

        SqlDataReader queryReader = flightQuery.ExecuteReader();
        while(queryReader.Read())
        {
          int thisFlightId = queryReader.GetInt32(0);
          int flightDepartTime = queryReader.GetInt32(1);
          string flightDepartCity = queryReader.GetString(2);
          string flightDestination = queryReader.GetString(3);
          string flightStatus = queryReader.GetString(4);
          Flight foundFlight = new Flight(flightDepartTime, flightDepartCity, flightDestination, flightStatus, thisFlightId);
          flights.Add(foundFlight);
        }
        if(queryReader != null)
        {
          queryReader.Close();
        }
      }
      if(conn != null)
      {
        conn.Close();
      }
      return flights;
    }


    public static List<City> GetAll()
    {

      List<City> allCities = new List<City>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        City newCity = new City(name, cityId);
        allCities.Add(newCity);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allCities;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM cities;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
