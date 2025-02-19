using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;


namespace RepositoryLayer.Services
{
    public class NotesRepository : INoteRepository
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public NotesRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public NotesEntity AddNotes(int UserId, NotesModel model)
        {
            NotesEntity notes = new NotesEntity();
            notes.Title = model.Title;
            notes.Description = model.Description;
            notes.UserId = UserId;
            notes.CreateAt = DateTime.Now;
            notes.color = model.color;
            notes.image=model.image;
            notes.IsArchive=model.IsArchive;
            notes.IsTrash=model.IsTrash;
            notes.UpdateAt=model.UpdateAt;


            context.Add(notes);
            context.SaveChanges();
            return notes;
        }

        public List<NotesEntity> GetNotes(int UserId)
        {
            var listOfNotes = context.NotesEntity.Where(x => x.UserId == UserId).ToList().ToList();
            return listOfNotes;
        }
        
        public int GetNotesCount(int UserId)
        {

            var notesCount = context.NotesEntity.Count(x => x.UserId == UserId);

            return notesCount;
        }


        public NotesEntity UpdateNotes(int UserId, int NotesId, UpdateNotesModel model)
        {
            var notes = context.NotesEntity.FirstOrDefault(x => x.UserId == UserId && x.NotesId == NotesId);
            if (notes == null)
            {
                return null;
            }
            notes.Title = model.Title;
            notes.Description = model.Description;
            notes.Reminder = model.Reminder;
            notes.color = model.Colour;
            notes.IsPin = model.IsPin;
            notes.IsArchive = model.IsArchive;
            notes.IsTrash = model.IsTrash;
            notes.UpdateAt = DateTime.Now;

            context.SaveChanges();

            return notes;
        }

        public bool DeleteNotes(int UserId, int NotesId)
        {
            var notes = context.NotesEntity.FirstOrDefault(x => x.UserId == UserId && x.NotesId == NotesId);
            if (notes == null)
            {
                return false;
            }

            context.NotesEntity.Remove(notes);
            context.SaveChanges();
            return true;
        }

        //public bool IsPinUnpin(int UserId, int notesId)
        //{
        //    var checkExist = this.context.Users.FirstOrDefault(d => d.UserId == UserId);
        //    if (checkExist.pin == true)
        //    {
        //        checkExist.pin = false;
        //        context.SaveChanges();

        //    }
        //    return true;
        //}
        public bool IsArchive(int UserId, int notesId)
        {
            var ar = this.context.NotesEntity.FirstOrDefault(d => d.NotesId == notesId && d.UserId == UserId);

            if (ar.IsArchive != null)
            {
                //checkExist.IsPin = false;
                //context.SaveChanges();
                //return false;


                if (ar.IsArchive == true)
                {
                    ar.IsArchive = false;
                    context.SaveChanges();
                    return true;
                }

                else
                {
                    ar.IsArchive = true;
                    context.SaveChanges();
                    return true;

                }
            }

            else
            {
                return false;
            }

        }


        public bool IsPinUnpin(int UserId, int notesId)
        {
            var checkPin = this.context.NotesEntity.FirstOrDefault(d => d.NotesId == notesId && d.UserId == UserId);

            if (checkPin.IsPin != null)
            {
                 
                if (checkPin.IsPin == true)
                {
                    checkPin.IsPin = false;
                    context.SaveChanges();
                    return true;
                }

                else
                {
                    checkPin.IsPin = true;
                    context.SaveChanges();
                    return true;

                }
            }
           
            else
            {
                return false;
            }
            
        }

        public bool TrashNote(int UserId, int notesId)
        {
            var trash = this.context.NotesEntity.FirstOrDefault(d => d.NotesId == notesId && d.UserId == UserId);

            if (trash.IsPin != null)
            {
                if(trash.IsPin == true)
                {
                    trash.IsPin = false;
                    context.SaveChanges();
                    return true;
                }

                else
                {
                    trash.IsPin = true;
                    context.SaveChanges();
                    return true;

                }
            }

            else
            {
                return false;
            }
        }

       

        public bool DeleteNotesForever(int UserId, int NotesId)
        {

            var notesExists = context.NotesEntity.FirstOrDefault(x => x.UserId == UserId && x.NotesId == NotesId);
            if (notesExists == null)
            {
                return false;
            }
            context.NotesEntity.Remove(notesExists);
            context.SaveChanges();

            return true;
        }

        public NotesEntity AddReminder(int UserId, int NotesId, DateTime reminderTime)
        {

            var note = context.NotesEntity.FirstOrDefault(x => x.UserId == UserId && x.NotesId == NotesId);

            if (note == null)
            {
                return null;
            }
            if (reminderTime <= DateTime.Now)
            {
                return null;
            }
            note.Reminder = reminderTime;
            note.UpdateAt = DateTime.Now;
            context.SaveChanges();

            return note;
        }

        public string AddColorToNote(int UserId, int NotesId, string color)
        {

            var note = context.NotesEntity.FirstOrDefault(x => x.UserId == UserId && x.NotesId == NotesId);
            if (note == null)
            {
                return null;
            }
            note.color = color;
            context.SaveChanges();

            return null;
        }


        public string Image(string image, int notesId, int userId)
        {
            string path = "@C:\\Users\\Hp\\Pictures\\admin.jpg";
            var note = context.NotesEntity.FirstOrDefault(d => d.NotesId == notesId && d.UserId == userId);
            if (note == null)
            {
                return null;
            }
            else
            {
                note.image = image;
                context.SaveChanges();
                return note.image;
            }
        }

        //public NotesEntity GetNotesCount()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

