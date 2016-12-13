using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Airline
{
  public class FlightTest : IDisposable
  {
    public FlightTest()
    {
    DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Flight_Objects_AreTheSame()
    {
      Flight flightOne = new Flight(1, "Portland", "Seattle", "On Time");
      Flight flightTwo = new Flight(1, "Portland", "Seattle", "On Time");

      Assert.Equal(flightOne, flightTwo);
    }

    [Fact]
    public void Save_GetAll_Flights_FromDatabase()
    {
      List<Flight> testList = new List<Flight> {};
      Flight flightOne = new Flight(1, "Portland", "Seattle", "On Time");
      flightOne.Save();

      testList.Add(flightOne);
      List<Flight> resultList = Flight.GetAll();

      Assert.Equal(testList, resultList);
    }

    [Fact]
    public void Test_FindFindsFlightInDatabase()
    {
      //Arrange
      Flight testFlight = new Flight(1, "Portland", "Seattle", "On Time");
      testFlight.Save();

      //Act
      Flight result = Flight.Find(testFlight.GetId());

      //Assert
      Assert.Equal(testFlight, result);
    }

    


    public void Dispose()
    {
      Flight.DeleteAll();
      City.DeleteAll();
    }
  }
}
