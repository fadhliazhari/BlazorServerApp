namespace OJTTraining.Data
{
    public class RoomSearch
    {
        public string RoomNumber { get; set; }
        public string Status { get; set; }

        public RoomSearch Clone(RoomSearch fromRoom)
        {
            this.RoomNumber = fromRoom.RoomNumber;
            this.Status = fromRoom.Status;
            return this;
        }
    }
}