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

    public SqlParameter SetParameter(string ParamName)
    {
      SqlParameter tempParameter = new SqlParameter();
      tempParameter.ParameterName = ParamName;
      tempParameter.Value = this.GetCityName();
      return tempParameter;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@Name)", conn);

      SqlParameter nameParameter = SetParameter("@Name");
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
