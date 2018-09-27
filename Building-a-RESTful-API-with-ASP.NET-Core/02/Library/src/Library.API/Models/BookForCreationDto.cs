using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Entities
{
    public class BookForCreationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
