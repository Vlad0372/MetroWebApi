using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MetroWebApi.Models;

namespace MetroWebApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MetroContext context)
        {
            context.Database.EnsureCreated();
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE Trains");

            if(context.Trains.Any())
            {
                return;
            }

            var trains = new Train[]
            {
                new Train{Model = "MA-001", StartPoint = "Chernivtsi", EndPoint = "Kiev", DepartureTime=DateTime.Parse("2020-03-01"), ArrivalTime=DateTime.Parse("2020-03-01")},
                new Train{Model = "MA-002", StartPoint = "Chernivtsi", EndPoint = "Donetsk", DepartureTime=DateTime.Parse("2020-03-10"), ArrivalTime=DateTime.Parse("2020-04-09")},
                new Train{Model = "MA-003", StartPoint = "Chernivtsi", EndPoint = "Lugansk", DepartureTime=DateTime.Parse("2020-04-06"), ArrivalTime=DateTime.Parse("2020-05-04")},
                new Train{Model = "MA-004", StartPoint = "Chernivtsi", EndPoint = "Lviv", DepartureTime=DateTime.Parse("2020-09-14"), ArrivalTime=DateTime.Parse("2020-03-02")},
                new Train{Model = "MA-005", StartPoint = "Chernivtsi", EndPoint = "Chernigiv", DepartureTime=DateTime.Parse("2020-02-21"), ArrivalTime=DateTime.Parse("2020-02-11")},
                new Train{Model = "MA-006", StartPoint = "Chernivtsi", EndPoint = "Ivano-Frankivsk", DepartureTime=DateTime.Parse("2020-09-03"), ArrivalTime=DateTime.Parse("2020-10-25")},
            };
            foreach(Train t in trains)
            {
                context.Trains.Add(t);
            }
            context.SaveChanges();
        }

    }
}
