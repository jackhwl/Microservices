using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourManagement.API.Dtos
{
    public class TourForUpdate : TourAbstractBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "When updating a tour, the description is required.")]
        public override string Description { get; set; }
    }
}
