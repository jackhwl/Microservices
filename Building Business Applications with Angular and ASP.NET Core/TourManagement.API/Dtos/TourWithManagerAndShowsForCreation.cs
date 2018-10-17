using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourManagement.API.Dtos
{
    public class TourWithManagerAndShowsForCreation : TourWithManagerForCreation
    {
        public ICollection<ShowForCreation> Shows { get; set; } = new List<ShowForCreation>();
    }
}
