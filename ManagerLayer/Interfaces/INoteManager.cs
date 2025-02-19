using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface INoteManager
    {
        //public NotesEntity AddNotes(int UserId,NotesModel model);

        //public List<NotesEntity> GetNotes(int UserId);

        //public NotesEntity UpdateNotes(int UserId,int NotesId,NotesModel model);

        //public bool DeleteNotes(int NotesId,int UserId);

        public NotesEntity AddNotes(int UserId, NotesModel model);

        public int GetNotesCount(int UserId);
        public List<NotesEntity> GetNotes(int UserId);
        public NotesEntity UpdateNotes(int UserId, int NotesId, UpdateNotesModel model);
        public bool DeleteNotes(int NotesId, int UserId);

        public NotesEntity GetNotesCount();

        public bool IsPinUnpin(int UserId, int notesId);







    }
}
