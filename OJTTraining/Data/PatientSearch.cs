using System;

namespace OJTTraining.Data
{
    public class PatientSearch
    {
        public string PatientName { get; set; }
        public string RoomNumber { get; set; }
        public DateTime? CheckinDateStart { get; set; }
        public DateTime? CheckinDateEnd { get; set; }
        public DateTime? CheckoutDateStart { get; set; }
        public DateTime? CheckoutDateEnd { get; set; }
        public PatientSearch Clone(PatientSearch fromPatient)
        {
            this.PatientName = fromPatient.PatientName;
            this.RoomNumber = fromPatient.RoomNumber;
            this.CheckinDateStart = fromPatient.CheckinDateStart;
            this.CheckinDateEnd = fromPatient.CheckinDateEnd;
            this.CheckoutDateStart = fromPatient.CheckoutDateStart;
            this.CheckoutDateEnd = fromPatient.CheckoutDateEnd;

            return this;
        }

    }
}