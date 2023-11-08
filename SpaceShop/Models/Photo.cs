﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceShop.Models
{
    [Table("Photos")]
    public class Photo : BaseEntity
    {
        [Required]
        public string Imageurl { get; set; }
        public bool IsPrimary { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}