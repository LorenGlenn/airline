using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Airline
{
  public class CityTest : IDisposable
  {
    public CityTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void City_Objects_AreTheSame()
    {
      City cityOne = new City("Portland");
      City cityTwo = new City("Portland");

      Assert.Equal(cityOne, cityTwo);
    }

    [Fact]
    public void Save_GetAll_Cities_FromDatabase()
    {
      List<City> testList = new List<City> {};
      City cityOne = new City("Portland");
      cityOne.Save();

      testList.Add(cityOne);
      List<City> resultList = City.GetAll();

      Assert.Equal(testList, resultList);
    }

    [Fact]
    public void Test_FindFindsCityInDatabase()
    {
      //Arrange
      City testCity = new City("Seattle");
      testCity.Save();

      //Act
      City result = City.Find(testCity.GetId());

      //Assert
      Assert.Equal(testCity, result);
    }

    [Fact]
    public void Test_AddCityIdsToFlight()
    {
      List<Flight> result = new List<Flight> {};
      List<Flight> test = new List<Flight>{};
      Flight newFlight = new Flight(1, "Portland", "Seattle", "On Time");
      City portland = new City("Portland");
      City seattle = new City("Seattle");
      newFlight.Save();
      portland.Save();
      seattle.Save();

      test.Add(newFlight);
      newFlight.AddCity_JoinTable("Portland", "Seattle");
      result = portland.GetFlights();

      Assert.Equal(test, result);
    }


    public void Dispose()
    {
      City.DeleteAll();
    }
  }
}
