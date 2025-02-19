using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface INoteRepository
    {
        public NotesEntity AddNotes(int UserId, NotesModel model);

        public List<NotesEntity> GetNotes(int UserId);

        public NotesEntity UpdateNotes(int UserId, int NotesId, UpdateNotesModel model);

        public bool DeleteNotes(int UserId, int NotesId);

        
        public int GetNotesCount(int UserId);

        public bool IsPinUnpin(int UserId, int notesId);

        public bool TrashNote(int UserId, int notesId);
        

        public bool DeleteNotesForever(int UserId, int NotesId);

        public NotesEntity AddReminder(int UserId, int NotesId, DateTime reminderTime);

        public string AddColorToNote(int UserId, int NotesId, string color);

        public string Image(string image, int notesId, int userId);


    }
}
