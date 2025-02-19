using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class LabelDataEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }

        public string LabelName { get; set; }

        [ForeignKey("LabelNotes")]
        public int NotesId { get; set; }

        [ForeignKey("LabelUser")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual NotesEntity LabelNotes { get; set; }

        [JsonIgnore]
        public virtual Users LabelUser { get; set; }
    }
}
