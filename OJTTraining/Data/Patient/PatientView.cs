using System;

namespace OJTTraining.Data
{
    public class PatientView
    {
        public string PatientName { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime? CheckoutDate { get; set; }
    }
}