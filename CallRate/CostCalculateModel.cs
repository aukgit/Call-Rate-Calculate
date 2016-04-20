using System;

namespace CallRate {
    class CostCalculateModel {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan CalculatingStart { get; set; }
        public TimeSpan CalculatingEnd { get; set; }
        public bool IsComplete { get; set; }

    }
}