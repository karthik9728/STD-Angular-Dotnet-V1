﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Domain.Models
{
    public class Photo
    {
        public int Id { get; set; } 

        public string Url { get; set; }

        public bool IsMain { get; set; }

        public string PublicId { get; set; }

        [ForeignKey(nameof(AppUser))]
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}