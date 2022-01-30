using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime DOB)
        {
         var Age = (DateTime.Today.Year- DOB.Year);
         return DOB.AddYears(Age)>DateTime.Today?--Age:Age;
        
           
        }
    }
}