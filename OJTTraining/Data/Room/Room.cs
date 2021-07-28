using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OJTTraining.Data
{
    public class Room
    {
        [Key]
        [Required]
        [MaxLength(4, ErrorMessage = "Room Number is too long.")]
        [DisplayName("Room Number")]
        public string RoomNumber { get; set; }
        [Required]
        [DisplayName("Room Capacity")]
        public int RoomCapacity { get; set; }
        public DateTime RegisterDateTime { get; set; }
        [MaxLength(256)]
        public string RegisterPIC { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        [MaxLength(256)]
        public string UpdatePIC { get; set; }
    }
}