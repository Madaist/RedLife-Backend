using Microsoft.EntityFrameworkCore;
using RedLife.Core.Appointments;
using RedLife.Authorization.Users;
using System;
using System.Linq;

namespace RedLife.EntityFrameworkCore.Seed.Tenants
{
    public class AppointmentCreator
    {

        private readonly RedLifeDbContext _context;

        public AppointmentCreator(RedLifeDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateAppointments();
        }

        private void CreateAppointments()
        {
            var centerAdminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == User.CenterAdminUserName);
            var donorUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == User.DonorUserName);

            if (_context.Appointments.Count() < 3)
            {
                var appointment1 = new Appointment
                {
                    CenterId = centerAdminUser.Id,
                    DonorId = donorUser.Id,
                    Date = DateTime.UtcNow.AddDays(1)
                };
                var appointment2 = new Appointment
                {
                    CenterId = centerAdminUser.Id,
                    DonorId = donorUser.Id,
                    Date = DateTime.UtcNow.AddDays(2)
                };
                var appointment3 = new Appointment
                {
                    CenterId = centerAdminUser.Id,
                    DonorId = donorUser.Id,
                    Date = DateTime.UtcNow.AddDays(3)
                };
                _context.Appointments.AddRange(new Appointment[] { appointment1, appointment2, appointment3 });
                _context.SaveChanges();
            }
        }
    }
}
