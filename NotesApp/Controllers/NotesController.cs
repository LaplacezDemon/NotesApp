using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using NotesApp.Models;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteContext _context;

        public NotesController(NoteContext context)
        {
            _context = context;

        }

        [HttpGet]
        public ActionResult<List<Note>> GetAll()
        {
            if (_context.Notes.Count() == 0)
            {
                return new EmptyResult();
            } 
            else {
                return _context.Notes.ToList();
            }
        }

        [HttpGet("{id}", Name = "GetNote")]
        public ActionResult<Note> GetById(long id)
        {
            var item = _context.Notes.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public IActionResult Create(Note note)
        {
            note.CreatedAt = DateTime.Now;
            note.UpdatedAt = note.CreatedAt;

            _context.Notes.Add(note);
            _context.SaveChanges();

            return CreatedAtRoute("GetNote", new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, Note note)
        {
            var selectedNote = _context.Notes.Find(id);

            if (selectedNote == null)
            {
                return NotFound();
            }

            selectedNote.Title = note.Title;
            selectedNote.Content = note.Content;
            selectedNote.Tag = note.Tag;

            _context.Notes.Update(selectedNote);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
