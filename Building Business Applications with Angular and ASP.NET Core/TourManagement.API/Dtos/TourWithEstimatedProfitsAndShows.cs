using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourManagement.API.Dtos
{
    public class TourWithEstimatedProfitsAndShows : TourWithEstimatedProfits
    {
        public ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
