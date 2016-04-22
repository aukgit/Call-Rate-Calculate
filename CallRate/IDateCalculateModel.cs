using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallRate {
    interface IDateCalculateModel {
        DateTime Start { get; set; }
        DateTime End { get; set; }
    }
}
