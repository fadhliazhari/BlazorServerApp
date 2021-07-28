using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OJTTraining.Data
{
    public class Patient
    {
        public Patient()
        {
            PatientGUID = new Guid();
        }

        [Key]
        public Guid PatientGUID { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Patient Name is too long.")]
        [DisplayName("Patient Name")]
        public string PatientName { get; set; }
        [Required]
        [MaxLength(4, ErrorMessage = "Room Number is too long.")]
        [DisplayName("Room Number")]
        public string RoomNumber { get; set; }
        [DisplayName("Checkin Date")]
        public DateTime CheckinDate { get; set; }
        [DisplayName("Checkout Date")]
        public DateTime? CheckoutDate { get; set; }
        public DateTime RegisterDateTime { get; set; }
        [MaxLength(256)]
        public string RegisterPIC { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        [MaxLength(256)]
        public string UpdatePIC { get; set; }
        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
    }
}