using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class NoteManager : INoteManager
    {
        public readonly INoteRepository note;
        public readonly FundooDBContext context;

        public NoteManager(INoteRepository note, FundooDBContext context)
        {
            this.note = note;
            this.context = context;
        }

        public NotesEntity AddNotes(int UserId, NotesModel model)
        {
            return note.AddNotes(UserId, model);
        }

        public List<NotesEntity> GetNotes(int UserId)
        {
            return note.GetNotes(UserId);
        }

        public NotesEntity UpdateNotes(int UserId, int NotesId, UpdateNotesModel model)
        {
            return note.UpdateNotes(UserId, NotesId, model);
        }

        public bool DeleteNotes(int NotesId, int UserId)
        {
            return note.DeleteNotes(NotesId, UserId);
        }

        public int GetNotesCount(int UserId)
        {
            return note.GetNotesCount(UserId);
        }
       
        public bool IsPinUnpin(int userId,int notesId)
        {
            return note.IsPinUnpin(userId,notesId);
        }

        public NotesEntity GetNotesCount()
        {
            throw new NotImplementedException();
        }

        //public NotesEntity GetNotesCount()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
