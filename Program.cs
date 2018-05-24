using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace WeddingSeating
{
    class Program
    {
        public static void Main()
        {
            EventsHall eventHall = new EventsHall();
            //eventHall.printTables();
            eventHall.TransferXLToTable();
            Console.ReadLine();
        }
      
    }
}
