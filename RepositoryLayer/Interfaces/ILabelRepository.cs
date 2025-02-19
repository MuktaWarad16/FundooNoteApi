using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRepository
    {
        public LabelEntity AddLabel(LabelModel model, int userId);

        public List<LabelEntity> GetAllLabels(int userId);

        public bool UpdateLabel(int userId, int labelId, string labelName);

        public bool DeleteLabel(int userId, int labelId);
    }
}