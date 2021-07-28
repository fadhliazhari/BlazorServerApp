using System;
using System.ComponentModel.DataAnnotations;

namespace OJTTraining.Data
{
    public class Room
    {
        [Key]
        [Required]
        [MaxLength(4, ErrorMessage = "Room Number is too long.")]
        public string RoomNumber { get; set; }
        [Required]
        public int RoomCapacity { get; set; }
        public DateTime RegisterDateTime { get; set; }
        [MaxLength(256)]
        public string RegisterPIC { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        [MaxLength(256)]
        public string UpdatePIC { get; set; }
    }
}