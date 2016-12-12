using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Airline
{
  public class CityTest
  {
    public CityTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_test;Integrated Security=SSPI;";
    }
  }
}
