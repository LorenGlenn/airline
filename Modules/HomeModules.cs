using Nancy;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace Airline
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] =_=> View["index.cshtml"];

      Get["/flights"] =_=> {
        List<Flight> allFlights = Flight.GetAll();
        return View["flights.cshtml", allFlights];
      };

      Post["/flights/added"] =_=> {
        int time = Int32.Parse(Request.Form["time"]);
        string depart = Request.Form["depart"];
        string destination = Request.Form["destination"];
        string status = Request.Form["status"];
        Flight newFlight = new Flight(time, depart, destination, status);
        newFlight.Save();
        newFlight.AddCity_JoinTable(depart, destination);
        List<Flight> allFlights = Flight.GetAll();
        return View["flights.cshtml", allFlights];
      };
    }
  }
}
