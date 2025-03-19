using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities
{
    public class Duration
    {
        private int _days;
        private int _hours;
        private int _minutes;

        public int Days 
        {
            get => _days;
            set
            {
                if (value > 99)
                    throw new ArgumentException("Days value must be less than 100");
                _days = value;
            }
                
        }
        public int Hours
        {
            get => _hours;
            set
            {
                if (value > 23)
                    throw new ArgumentException("Hours value must be less than 24");
                _hours = value;
            }
        }
        public int Minutes
        {
            get => _minutes;
            set
            {
                if (value > 59)
                    throw new ArgumentException("Minutes value must be less than 60");
            }
        }
    }
}
