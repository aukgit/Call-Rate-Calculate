using System;

namespace CallRate {
    internal class CostCalculateLogic {

        private bool _isInFirstClockCycle = true;
        public CostCalculateLogic(CostCalculateModel model) {
            CostCalculateModel = model;
        }

        public CostCalculateModel CostCalculateModel { get; protected set; }

        private void SetInitialStartEndCalculateTime() {
            CostCalculateModel.CalculatingStart = CostCalculateModel.Start.TimeOfDay;
            CostCalculateModel.CalculatingEnd = CostCalculateModel.End.TimeOfDay;
        }

        /// <summary>
        ///     Calculate can only ran at once on a model.
        ///     Please create new
        /// </summary>
        /// <returns></returns>
        public double CalculateRates() {
            if (CostCalculateModel.IsComplete) {
                throw new Exception("Calculate method is already executed. Please initialize with new model.");
            }
            var timeDiff = CostCalculateModel.End - CostCalculateModel.Start;
            var daysAsInt = (int)timeDiff.TotalDays;
            SetInitialStartEndCalculateTime();
            double costSummation = 0;
            if (daysAsInt > 0) {
                costSummation = daysAsInt * Consts.OneDayCost;
            }
            double specificTimeSlotCosts = 0;
            while (!CostCalculateModel.IsComplete) {
                var slot1 = Calculate12AmTo6AmCost();
                var solt2 = Calculate6AmTo12PmCost();
                var solt3 = Calculate12PmTo6PmCost();
                var solt4 = Calculate6PmTo12AmCost();
                specificTimeSlotCosts += slot1 + solt2 + solt3 + solt4;
            }
            costSummation += specificTimeSlotCosts;

            return costSummation;
        }

        private double Calculate12AmTo6AmCost() {
            double sum = 0;
            if (!CostCalculateModel.IsComplete) {
                var nextSolt = Consts.Time6Am;
                var cost = Consts.Am12To6AmRate;

                if (IsTimeBetween12AmTo6Am(CostCalculateModel.CalculatingStart)) {
                    SetEndTimeWithClosetBoundary(nextSolt);
                    var timeDiff = CostCalculateModel.CalculatingEnd - CostCalculateModel.CalculatingStart;
                    sum = timeDiff.TotalMinutes * cost;
                    CheckStartEndDateAndSetDateTimeForNextCalculation(nextSolt);
                }
            }
            return sum;
        }

        private double Calculate6AmTo12PmCost() {
            double sum = 0;
            if (!CostCalculateModel.IsComplete) {
                var nextSolt = Consts.Time12Pm;
                var cost = Consts.Am6To12PmRate;

                if (IsTimeBetween6AmTo12Pm(CostCalculateModel.CalculatingStart)) {
                    SetEndTimeWithClosetBoundary(nextSolt);
                    var timeDiff = CostCalculateModel.CalculatingEnd - CostCalculateModel.CalculatingStart;
                    sum = timeDiff.TotalMinutes * cost;
                    CheckStartEndDateAndSetDateTimeForNextCalculation(nextSolt);
                }
            }
            return sum;
        }

        private double Calculate12PmTo6PmCost() {
            double sum = 0;
            if (!CostCalculateModel.IsComplete) {
                var nextSolt = Consts.Time6Pm;
                var cost = Consts.Pm12To6PmRate;

                if (IsTimeBetween12PmTo6Pm(CostCalculateModel.CalculatingStart)) {
                    SetEndTimeWithClosetBoundary(nextSolt);
                    var timeDiff = CostCalculateModel.CalculatingEnd - CostCalculateModel.CalculatingStart;
                    sum = timeDiff.TotalMinutes * cost;
                    CheckStartEndDateAndSetDateTimeForNextCalculation(nextSolt);
                }
            }
            return sum;
        }

        private double Calculate6PmTo12AmCost() {
            double sum = 0;
            if (!CostCalculateModel.IsComplete) {
                var endSolt = Consts.Time1159Pm;
                var nextSolt = Consts.Time12Am;
                var cost = Consts.Pm6To12AmRate;
                if (IsTimeBetween6PmTo12Am(CostCalculateModel.CalculatingStart)) {
                    SetEndTimeWithClosetBoundary(endSolt);
                    var timeDiff = CostCalculateModel.CalculatingEnd - CostCalculateModel.CalculatingStart;
                    sum = timeDiff.TotalMinutes * cost;
                    CheckStartEndDateAndSetDateTimeForNextCalculation(nextSolt);
                }
            }
            _isInFirstClockCycle = false;
            return sum;
        }

        private void CheckStartEndDateAndSetDateTimeForNextCalculation(TimeSpan nextBoundarySlotStartingTime) {
            if (CostCalculateModel.CalculatingEnd.Equals(CostCalculateModel.End.TimeOfDay)) {
                CostCalculateModel.IsComplete = true;
            }
            CostCalculateModel.CalculatingEnd = CostCalculateModel.End.TimeOfDay;
            CostCalculateModel.CalculatingStart = nextBoundarySlotStartingTime;
        }

        /// <summary>
        ///     Find the closest boundary to the CalculatingEnd time and set it to the end time.
        ///     For example : if the user input start time 10:30 AM to 6:30 PM
        ///     then end time should be changed to 11:59 AM and then calculate the price for that boundary.
        /// </summary>
        private void SetEndTimeWithClosetBoundary(TimeSpan boundaryTime) {
            var timeDiff = CostCalculateModel.CalculatingEnd - boundaryTime;
            if (timeDiff.TotalSeconds < 0 && _isInFirstClockCycle) {
                CostCalculateModel.CalculatingEnd = CostCalculateModel.CalculatingEnd <= boundaryTime
                                           ? boundaryTime
                                           : CostCalculateModel.CalculatingEnd;
            } else {
                CostCalculateModel.CalculatingEnd = CostCalculateModel.CalculatingEnd <= boundaryTime
                                           ? CostCalculateModel.CalculatingEnd
                                           : boundaryTime;
            }

        }

        private bool IsTimeBetween12AmTo6Am(TimeSpan time) {
            var span12Am = Consts.Time12Am; // 12:00 AM;
            var span6Am = Consts.Time559Am; // 05:59 AM;
            return IsTimeBetween(time, span12Am, span6Am);
        }

        private bool IsTimeBetween6AmTo12Pm(TimeSpan time) {
            var span6Am = Consts.Time6Am; // 06:00 AM;
            var span12Pm = Consts.Time1159Am; // 11: 59 AM;
            return IsTimeBetween(time, span6Am, span12Pm);
        }

        private bool IsTimeBetween12PmTo6Pm(TimeSpan time) {
            var span12Pm = Consts.Time12Pm; //12:00 PM;
            var span6Pm = Consts.Time559Pm; // 5:59 PM;
            return IsTimeBetween(time, span12Pm, span6Pm);
        }

        private bool IsTimeBetween6PmTo12Am(TimeSpan time) {
            var span6Pm = Consts.Time6Pm; // 06:00 PM;
            var span12Am = Consts.Time1159Pm; // 11:59 PM;
            return IsTimeBetween(time, span6Pm, span12Am);
        }

        private bool IsTimeBetween(TimeSpan givenTime, TimeSpan start, TimeSpan end) {
            // see if start comes before end
            if (start < end) {
                return start <= givenTime && givenTime <= end;
            }
            // start is after end, so do the inverse comparison
            return !(end < givenTime && givenTime < start);
        }
    }
}