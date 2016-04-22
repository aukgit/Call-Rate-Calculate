using System;

namespace CallRate {
    internal class CostCalculateLogic {

        public CostCalculateLogic(IDateCalculateModel model) {
            CostCalculateModel = model;
        }

        public IDateCalculateModel CostCalculateModel { get; protected set; }


        /// <summary>
        ///     Calculate can only ran at once on a model.
        /// </summary>
        /// <returns></returns>
        public double GetTotalCost() {

            var startTime = CostCalculateModel.Start;
            var endTime = CostCalculateModel.End;
            if (startTime > endTime) {
                SwapDateTime(ref startTime, ref endTime);
            }
            var lastCalculatedSolt = new TimeSpan(0, 0, 0); // starting from 0 quad
            var timeDiff = endTime - startTime;

            int totalDaysAsInt = (int)timeDiff.TotalDays;
            double fractionTime = timeDiff.TotalDays - totalDaysAsInt;

            double totalCost = 0;
            if (fractionTime > 0) {
                totalCost = CalculateCost(startTime.TimeOfDay, lastCalculatedSolt, ref lastCalculatedSolt);
                totalCost += CalculateCost(endTime.TimeOfDay, lastCalculatedSolt, ref lastCalculatedSolt);
            }
            double totalCostForDays = totalDaysAsInt * Consts.OneDayCost;

            totalCost += totalCostForDays;
            return totalCost;
        }
        /// <summary>
        /// Calculate quad wise min cost
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeSlotStart">Give a starting timeslot.</param>
        /// <param name="lastSlot"></param>
        /// <returns></returns>
        private double CalculateCost(TimeSpan time, TimeSpan timeSlotStart, ref TimeSpan lastSlot) {
            var sixHour = new TimeSpan(6, 0, 0);
            var curTimeSpan = timeSlotStart; // starts from six hour quad
            int index = 0;
            double sum = 0;
            if (curTimeSpan.TotalHours > 0) {
                index = (int)((curTimeSpan.TotalHours / sixHour.TotalHours));
            }
            do {
                var cost = Consts.Cost[index++];
                curTimeSpan += sixHour;
                if (curTimeSpan <= time) { // already time + six hour quad
                    sum += 6 * 60 * cost; // 6 for each quad
                } else {
                    if (index == 1) {
                        sum += (curTimeSpan - time).TotalMinutes * cost;
                    } else {
                        sum += (time - (curTimeSpan - sixHour)).TotalMinutes * cost;
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