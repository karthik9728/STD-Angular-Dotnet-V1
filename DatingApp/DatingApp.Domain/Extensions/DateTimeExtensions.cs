﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.UtcNow;

            var age = today.Year - dob.Year;

            if(dob > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

    }
}