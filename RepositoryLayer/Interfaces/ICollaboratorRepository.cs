using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaboratorRepository
    {
        public CollaboratorEntity AddCollaborator(CollaboratorModel model, int userId);

        public List<CollaboratorEntity> GetAllCollaborators(int userId);

        public bool DeleteCollaborator(int collaboratorId, int notesId, int userId);
    }
}