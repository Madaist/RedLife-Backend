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
            var users = _context.Users;

            if (_context.Appointments.Count() < 3)
            {
                var appointment1 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "madaist").Id,
                    Date = new DateTime(2021, 1, 12)
                };
                var appointment2 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "madaist").Id,
                    Date = new DateTime(2021, 4, 15)
                };
                var appointment3 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminCTS").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "madaist").Id,
                    Date = new DateTime(2020, 6, 1)
                };
                var appointment4 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminSinevo").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "madaist").Id,
                    Date = new DateTime(2020, 2, 20)
                };
                var appointment5 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "andreiz").Id,
                    Date = new DateTime(2021, 2, 3)
                };
                var appointment6 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "popescumaria").Id,
                    Date = new DateTime(2021, 2, 12)
                };
                var appointment7 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "popescumaria").Id,
                    Date = new DateTime(2021, 5, 20)
                };
                var appointment8 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "ion.g").Id,
                    Date = new DateTime(2021, 6, 2)
                };
                var appointment9 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "ion.g").Id,
                    Date = new DateTime(2021, 3, 1)
                };
                var appointment10 = new Appointment
                {
                    CenterId = users.FirstOrDefault(u => u.UserName == "adminReginaMaria").Id,
                    DonorId = users.FirstOrDefault(u => u.UserName == "gabimusat").Id,
                    Date = new DateTime(2021, 6, 15)
                };
                _context.Appointments.AddRange(new Appointment[] { appointment1, appointment2, appointment3,
                    appointment4, appointment5, appointment6, appointment7, appointment8, appointment9, appointment10});
                _context.SaveChanges();
            }
        }
    }
}
