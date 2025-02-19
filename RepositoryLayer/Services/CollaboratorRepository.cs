using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private readonly FundooDBContext context;

        public CollaboratorRepository(FundooDBContext context)
        {
            this.context = context;
        }

        public CollaboratorEntity AddCollaborator(CollaboratorModel model, int userId)
        {
            CollaboratorEntity collaborator = new CollaboratorEntity();
            collaborator.Email = model.Email;
            collaborator.NotesId = model.NotesId;
            collaborator.UserId = userId;
            context.CollaboratorEntity.Add(collaborator);
            context.SaveChanges();
            return collaborator;
        }

        public List<CollaboratorEntity> GetAllCollaborators(int userId)
        {
            var collaboratorList = context.CollaboratorEntity
                .Select(x => x)
              .Where(x => x.UserId == userId).ToList();

            if (collaboratorList != null)
            {
                return collaboratorList;
            }
            return null;
        }

        public bool DeleteCollaborator(int collaboratorId, int notesId, int userId)
        {
            var collaborator = context.CollaboratorEntity.FirstOrDefault(x => x.CollaboratorId == collaboratorId && x.NotesId == notesId && x.UserId == userId);

            if (collaborator != null)
            {
                context.CollaboratorEntity.Remove(collaborator);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}