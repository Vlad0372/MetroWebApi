using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MetroWebApi.Models;
using MetroWebApi.Entities;

namespace MetroWebApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
           // context.Database.ExecuteSqlCommand("TRUNCATE TABLE Train");          
            
            if(context.Trains.Any())
            {
                return;
            }

            var trains = new Train[]
            {
                new Train{Model = "MA-001", StartPoint = "Chernivtsi", EndPoint = "Kiev", DepartureHour=3, ArrivalHour=20},
                new Train{Model = "MA-002", StartPoint = "Chernivtsi", EndPoint = "Donetsk", DepartureHour=10, ArrivalHour=20},
                new Train{Model = "MA-003", StartPoint = "Chernivtsi", EndPoint = "Lugansk", DepartureHour=6, ArrivalHour=15},
                new Train{Model = "MA-004", StartPoint = "Chernivtsi", EndPoint = "Lviv", DepartureHour=9, ArrivalHour=14},
                new Train{Model = "MA-005", StartPoint = "Chernivtsi", EndPoint = "Chernigiv", DepartureHour=21, ArrivalHour=2},
                new Train{Model = "MA-006", StartPoint = "Chernivtsi", EndPoint = "Ivano-Frankivsk", DepartureHour=9, ArrivalHour=12},
            };
            foreach(Train t in trains)
            {
                context.Trains.Add(t);
            }
            
            context.SaveChanges();
        }

    }
}
