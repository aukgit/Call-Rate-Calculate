using System;

namespace CallRate {
    internal class CostCalculateLogic {

        public CostCalculateLogic(IDateCalculateModel model) {
            CostCalculateModel = model;
        }

        public IDateCalculateModel CostCalculateModel { get; protected set; }


        /// <summary>
        ///     Calculate can only ran at once on a model.
        ///     Please create new
        /// </summary>
        /// <returns></returns>
        public double GetTotalCost() {

            var timeDiff = CostCalculateModel.End - CostCalculateModel.Start;
            var daysAsInt = (int)timeDiff.TotalDays;
            var startTime = CostCalculateModel.Start;
            var endTime = CostCalculateModel.End;
            if (startTime > endTime) {
                SwapDateTime(ref startTime, ref endTime);
            }
            var lastCalculatedSolt = new TimeSpan(0, 0, 0); // starting from 0 quad
            double totalCost = -CalculateCost(startTime.TimeOfDay, lastCalculatedSolt, ref lastCalculatedSolt);
            startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day); // Pointing the time to the starting of the day

            totalCost += CalculateCost(endTime.TimeOfDay, lastCalculatedSolt, ref lastCalculatedSolt);
            endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day); // Pointing the time to the starting of the day

            int totalDays = (int)(endTime - startTime).TotalDays;

            double totalCostForDays = totalDays * Consts.OneDayCost;

            totalCost += totalCostForDays;
            return totalDays;
        }

        private double CalculateCost(TimeSpan time, TimeSpan timeSlotStart, ref TimeSpan lastSlot) {
            var sixHour = new TimeSpan(6, 0, 0);
            var curTimeSpan = timeSlotStart; // starts from six hour quad
            int index = 0;
            double sum = 0;
            if (curTimeSpan.TotalHours > 0) {
                index = (int) ((curTimeSpan.TotalHours / sixHour.TotalHours) );
            }
            do {
                var cost = Consts.Cost[index++];
                curTimeSpan += sixHour;
                if (curTimeSpan <= time) { // already time + six hour quad
                    sum += 6 * 60 * cost; // 6 for each quad
                } else {
                    var closetTimeSpan = curTimeSpan < time ? timeSlotStart : time;
                    var diff = time - closetTimeSpan;
                    if (diff.TotalMinutes > 0) {
                        sum += diff.TotalMinutes * cost;
                    } else if( diff.TotalMinutes == 0 ) {
                        sum += (curTimeSpan - time).TotalMinutes * cost;
                    } else {
                        sum += ((curTimeSpan + sixHour) - time ).TotalMinutes * cost;
                    }
                }
            } while (curTimeSpan < time);
            lastSlot = curTimeSpan; // remove last added six hour
            return sum;
        }

        private void SwapDateTime(ref DateTime d1, ref DateTime d2) {
            var temp = d1;
            d1 = d2;
            d2 = temp;
        }

    }
}