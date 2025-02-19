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
    public class LabelRepository : ILabelRepository
    {
        private readonly FundooDBContext context;
        public LabelRepository(FundooDBContext context)
        {
            this.context = context;
        }

        public LabelEntity AddLabel(LabelModel model, int userId)
        {
            //var labelexist = context.Labels.FirstOrDefault(x => x.LabelName == model.LabelName);
            var labelExist = context.LabelEntity.FirstOrDefault(x => x.LabelName == model.LabelName && x.UserId == userId && x.NotesId == model.NotesId);
            if (labelExist == null)
            {
                LabelEntity label = new LabelEntity();
                label.LabelName = model.LabelName;
                label.UserId = userId;
                label.NotesId = model.NotesId;
                context.LabelEntity.Add(label);
                context.SaveChanges();
                return label;
            }
            else
            {
                LabelEntity newLabel = new LabelEntity();
                newLabel.LabelName = labelExist.LabelName;
                newLabel.NotesId = model.NotesId;
                newLabel.UserId = userId;
                context.Add(newLabel);
                context.SaveChanges();
                return newLabel;
            }
        }

        public List<LabelEntity> GetAllLabels(int userId)
        {
            try
            {
                List<LabelEntity> labelList = new List<LabelEntity>();
                var labels = context.LabelEntity.Select(x => x).Where(y => y.UserId == userId);
                if (labels != null)
                {
                    foreach (LabelEntity label in labels)
                    {
                        labelList.Add(label);
                    }
                    return labelList;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateLabel(int userId, int labelId, string labelName)
        {
            try
            {
                var label = context.LabelEntity.FirstOrDefault(x => x.UserId == userId && x.LabelId == labelId);
                if (label != null)
                {
                    label.LabelName = labelName;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteLabel(int userId, int labelId)
        {
            try
            {
                var label = context.LabelEntity.FirstOrDefault(x => x.UserId == userId && x.LabelId == labelId);
                if (label != null)
                {
                    context.Remove(label);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}