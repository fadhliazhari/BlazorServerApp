using Microsoft.EntityFrameworkCore;
using OJTTraining.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OJTTraining.Service
{
    public class PatientService
    {
        #region Property
        private readonly ApplicationDbContext _appDBContext;
        #endregion

        #region Constructor
        public PatientService(ApplicationDbContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        #endregion

        #region Get List of Patients
        public async Task<PatientModel> GetPatientsAsync(int skip, int top, PatientSearch searchPatient)
        {
            Expression<Func<Patient, bool>> whereExpression = null;

            if (searchPatient is not null && !string.IsNullOrEmpty(searchPatient.PatientName))
            {
                whereExpression = patient => patient.PatientName.ToLower().Contains(searchPatient.PatientName.ToLower());
            }

            if (searchPatient is not null && !string.IsNullOrEmpty(searchPatient.RoomNumber))
            {
                Expression<Func<Patient, bool>> e1 = null;
                if (searchPatient.ExactMatch)
                {
                    e1 = patient => patient.RoomNumber.ToLower().Equals(searchPatient.RoomNumber.ToLower());
                }
                else
                {
                    e1 = patient => patient.RoomNumber.ToLower().Contains(searchPatient.RoomNumber.ToLower());
                }
                whereExpression = whereExpression != null ? whereExpression.And(e1) : e1;
            }

            if (searchPatient is not null && searchPatient.CheckinDateStart != null)
            {
                Expression<Func<Patient, bool>> e1 = patient => patient.CheckinDate >= searchPatient.CheckinDateStart;
                whereExpression = whereExpression != null ? whereExpression.And(e1) : e1;
            }
            if (searchPatient is not null && searchPatient.CheckinDateEnd != null)
            {
                Expression<Func<Patient, bool>> e1 = patient => patient.CheckinDate <= searchPatient.CheckinDateEnd;
                whereExpression = whereExpression != null ? whereExpression.And(e1) : e1;
            }

            if (searchPatient is not null && searchPatient.CheckoutDateStart != null)
            {
                Expression<Func<Patient, bool>> e1 = patient => patient.CheckoutDate == null || patient.CheckoutDate >= searchPatient.CheckoutDateStart;
                whereExpression = whereExpression != null ? whereExpression.And(e1) : e1;
            }
            if (searchPatient is not null && searchPatient.CheckoutDateEnd != null)
            {
                Expression<Func<Patient, bool>> e1 = patient => patient.CheckoutDate <= searchPatient.CheckoutDateEnd;
                whereExpression = whereExpression != null ? whereExpression.And(e1) : e1;
            }

            Expression<Func<Patient, bool>> isNotDeleted = patient => patient.IsDeleted == false;
            whereExpression = whereExpression != null ? whereExpression.And(isNotDeleted) : isNotDeleted;

            Expression<Func<Patient, string>> orderBy = patient => patient.PatientName;

            IQueryable<Patient> patientQuery = _appDBContext.Patients.Where(whereExpression).OrderBy(orderBy);

            var count = await patientQuery.CountAsync();
            var patient = await patientQuery.Skip(skip).Take(top == 0 ? count : top).ToListAsync();

            return new PatientModel { count = count, patient = patient };
        }
        #endregion

        #region Insert Patient
        public async Task<bool> InsertPatientAsync(Patient patient)
        {
            patient.CheckinDate = patient.CheckinDate.AddSeconds(-patient.CheckinDate.Second);
            patient.CheckoutDate = patient.CheckoutDate?.AddSeconds((double)-patient.CheckoutDate?.Second);
            await _appDBContext.Patients.AddAsync(patient);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get Patient by Id
        public async Task<Patient> GetPatientAsync(Guid Id)
        {
            Patient Patient = await _appDBContext.Patients.FirstOrDefaultAsync(c => c.PatientGUID.Equals(Id));
            return Patient;
        }
        #endregion

        #region Update Patient
        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            patient.CheckinDate = patient.CheckinDate.AddSeconds(-patient.CheckinDate.Second);
            patient.CheckoutDate = patient.CheckoutDate?.AddSeconds((double)-patient.CheckoutDate?.Second);
            _appDBContext.Patients.Update(patient);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region DeletePatient
        public async Task<bool> DeletePatientAsync(Patient patient)
        {
            _appDBContext.Remove(patient);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
