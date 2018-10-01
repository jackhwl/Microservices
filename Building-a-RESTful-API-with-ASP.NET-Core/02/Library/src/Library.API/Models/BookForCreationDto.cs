using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Entities
{
    public class BookForCreationDto
    {
        [Required(ErrorMessage = "You should fill out a title.")]
        [MaxLength(100, ErrorMessage = "The titel shouldn't have more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessage = "The description shouldn't have more than 500 characters.")]
        public string Description { get; set; }
    }
}
