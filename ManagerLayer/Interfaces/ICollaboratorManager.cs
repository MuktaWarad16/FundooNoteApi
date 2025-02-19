using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface ICollaboratorManager
    {
        public CollaboratorEntity AddCollaborator(CollaboratorModel model, int userId);

        public List<CollaboratorEntity> GetAllCollaborators(int userId);

        public bool DeleteCollaborator(int collaboratorId, int notesId, int userId);

        public bool MailExists(string email);
    }
}