using Microsoft.EntityFrameworkCore;
using OJTTraining.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OJTTraining.Service
{
    public class RoomService
    {
        #region Property
        private readonly ApplicationDbContext _appDBContext;
        #endregion

        #region Constructor
        public RoomService(ApplicationDbContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        #endregion

        #region Get List of Rooms
        public async Task<RoomModel> GetRoomsAsync(int skip, int top, RoomSearch searchRoom)
        {
            var leftJoinQuery = from rooms in _appDBContext.Rooms
                                join patients in _appDBContext.Patients
                                    on rooms.RoomNumber equals patients.RoomNumber into grouping
                                from patients in grouping.DefaultIfEmpty()
                                select new { rooms, patients };

            var roomQuery1 = leftJoinQuery
                .Select(r => new
                {
                    r.rooms.RoomNumber,
                    r.rooms.RoomCapacity,
                    r.patients.PatientGUID,
                    r.patients.CheckinDate,
                    r.patients.CheckoutDate,
                    PatientIsDeleted = r.patients.IsDeleted
                })
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                .Where(patient => patient.PatientIsDeleted == null || patient.PatientIsDeleted == false)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                .Where(patient => patient.CheckinDate == null || patient.CheckinDate <= DateTime.Now)
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                .Where(patient => patient.CheckoutDate == null || patient.CheckoutDate > DateTime.Now);


            if (searchRoom is not null && !string.IsNullOrEmpty(searchRoom.RoomNumber))
            {
                roomQuery1 = roomQuery1.Where(room => room.RoomNumber.ToLower().Contains(searchRoom.RoomNumber.ToLower()));
            }

            var roomQuery2 = roomQuery1
                .Select(r => new
                {
                    r.RoomNumber,
                    r.RoomCapacity,
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                    HasPatient = r.PatientGUID != null,
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                    r.PatientGUID
                })
                .GroupBy(room => new { room.RoomNumber, room.RoomCapacity, room.HasPatient })
                .Select(r => new
                {
                    r.Key.RoomNumber,
                    r.Key.RoomCapacity,
                    r.Key.HasPatient,
                    PatientCount = r.Count()
                })
                ;

            Expression<Func<RoomView, bool>> whereExpression = room => true;
            if (searchRoom != null && !string.IsNullOrEmpty(searchRoom.Status))
            {
                whereExpression = room => room.RoomStatus.ToLower().Equals(searchRoom.Status.ToLower());
            }

            IQueryable<RoomView> finalQuery = roomQuery2
                .Select(r => new RoomView
                {
                    RoomNumber = r.RoomNumber,
                    RoomStatus = (r.RoomCapacity == 0) ? "Closed" :
                                    ((r.HasPatient ? r.PatientCount : 0) > r.RoomCapacity) ? "Overcrowded" :
                                        ((r.HasPatient ? r.PatientCount : 0) == r.RoomCapacity) ? "Full" :
                                            ((r.HasPatient ? r.PatientCount : 0) < r.RoomCapacity) ? "Available" : ""
                })
                .Where(whereExpression)
                .OrderBy(room => room.RoomNumber);

            var count = await finalQuery.CountAsync();
            var room = await finalQuery.Skip(skip).Take(top == 0 ? count : top).ToListAsync();

            return new RoomModel { Count = count, Rooms = room };
        }
        #endregion

        #region Insert Room
        public async Task<bool> InsertRoomAsync(Room room)
        {
            await _appDBContext.Rooms.AddAsync(room);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get Room by Id
        public async Task<Room> GetRoomAsync(string Id)
        {
            Room Room = await _appDBContext.Rooms.FirstOrDefaultAsync(c => c.RoomNumber.Equals(Id));
            return Room;
        }
        #endregion

        #region Update Room
        public async Task<bool> UpdateRoomAsync(Room room)
        {
            _appDBContext.Rooms.Update(room);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region DeleteRoom
        public async Task<bool> DeleteRoomAsync(Room room)
        {
            _appDBContext.Remove(room);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region CheckUsed
        public async Task<bool> CheckIfUsedAsync(Room room)
        {
            return await GetNoUsedAsync(room) != 0;
        }

        public async Task<int> GetNoUsedAsync(Room room)
        {
            return await _appDBContext.Patients
                .Where(patient => patient.IsDeleted == false)
                .Where(patient => patient.CheckinDate <= DateTime.Now)
                .Where(patient => patient.CheckoutDate == null || patient.CheckoutDate > DateTime.Now)
                .Where(patient => patient.RoomNumber.Equals(room.RoomNumber))
                .CountAsync();
        }
        #endregion

        #region CheckAvailable
        public async Task<string> CheckIfAvailableAsync(string roomNumber)
        {
            var roomNumberQuery = await _appDBContext.Rooms
                .Where(room => room.RoomNumber.ToLower().Equals(roomNumber.ToLower()))
                .Where(room => room.RoomCapacity != 0).FirstOrDefaultAsync();

            if (roomNumberQuery?.RoomNumber == null)
            {
                return "";
            }
            return roomNumberQuery.RoomNumber;
        }
        #endregion
    }
}
