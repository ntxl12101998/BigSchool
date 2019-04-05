using BigSchool.DTOs;
using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool.Controllers
{
    [Authorize]
    public class AttendacesController : ApiController
    {
        private readonly ApplicationDbContext _dbcontext;

        public AttendacesController()
        {
            _dbcontext = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendaceDto attendanceDto)
        {
            var userId = User.Identity.GetUserId();
            if (_dbcontext.Attendances.Any(a => a.AttendeeId == userId && a.CourseId == attendanceDto.CourseId))
                return BadRequest("The Attendance already exists!");

            var attendance = new Attendance
            {
                CourseId = attendanceDto.CourseId,
                AttendeeId = userId
            };
            _dbcontext.Attendances.Add(attendance);
            _dbcontext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendance(int id)
        {
            var userId = User.Identity.GetUserId();

            var attendance = _dbcontext.Attendances
               .SingleOrDefault(a => a.AttendeeId == userId && a.CourseId == id);

            if(attendance == null)
            {
                return NotFound();
            }

            _dbcontext.Attendances.Remove(attendance);
            _dbcontext.SaveChanges();

            return Ok(id);
        }
    } 
}
