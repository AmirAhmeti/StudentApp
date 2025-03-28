﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentContext _studentContext;

        public StudentController(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            if (_studentContext.Students == null)
            {
                return NotFound();
            }
            return await _studentContext.Students.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentbyid(int id)
        {
            if (_studentContext.Students == null)
            {
                return NotFound();
            }
            var student = await _studentContext.Students.FindAsync(id);
            if(student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> SaveStudent(Student student)
        {
            _studentContext.Students.Add(student);
            await _studentContext.SaveChangesAsync();
            return CreatedAtAction(nameof(SaveStudent),new {id=student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStudent(int id, Student student)
        {
            if(id != student.Id)
            {
                return BadRequest();
            }
            _studentContext.Entry(student).State = EntityState.Modified;
            try
            {
                await _studentContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            if(_studentContext.Students == null)
            {
                return NotFound();
            }
            var student = await _studentContext.Students.FindAsync(id);
            if(student == null)
            {
                return NotFound();
            }
            _studentContext.Students.Remove(student);
            await _studentContext.SaveChangesAsync();
            return Ok();
        }

    }
}
