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
        public string RoomNumber { get; set; }
        [Required]
        public int RoomCapacity { get; set; }
        [DefaultValue("LOCALTIMESTAMP(0)")]
        public DateTime? RegisterDateTime { get; set; }
        public string RegisterPIC { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdatePIC { get; set; }
    }
}