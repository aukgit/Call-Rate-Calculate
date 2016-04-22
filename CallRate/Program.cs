using System;

namespace CallRate {
    static class Program {
        static void Main(string[] args) {
            Console.WriteLine("--- Bill Calculate ---");
            Console.WriteLine("--- 12 AM to 06 AM : Cost 1 ---");
            Console.WriteLine("--- 06 AM to 12 PM : Cost 3 ---");
            Console.WriteLine("--- 12 PM to 06 PM : Cost 4 ---");
            Console.WriteLine("--- 06 PM to 12 AM : Cost 2 ---");
            Console.WriteLine("--- 03/01/2009 05:42:00 means 01-Jan-2009 05:42 AM ---");
            Console.WriteLine("--- 03/02/2009 22:42:00 means 01-Feb-2009 10:42 PM ---");
            Console.WriteLine("--- ------------ ---");
            while (true) {
                Console.Write("Start Date ( eg. 03/01/2009 05:42:00 ): ");
                var startDateString = Console.ReadLine();
                Console.Write("End Date   ( eg. 03/01/2009 05:42:00 ): ");
                var endDateString = Console.ReadLine();
                var model = new CostCalculateModel();
                model.Start = DateTime.Parse(startDateString);
                model.End = DateTime.Parse(endDateString);
                var costLogic = new CostCalculateLogic(model);
                var sum = costLogic.GetTotalCost();
                Console.WriteLine("Total Cost : " + sum);
                costLogic = null;
                model = null;
                GC.Collect();
            }
        }
    }
}
