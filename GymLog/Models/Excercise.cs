using GymLog.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymLog.Models
{
    public class Excercise
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<BodyPart>? BodyParts { get; set; }
        public List<BodyPartExcercise>? BodyPartExcercises { get; set; }
        public List<TemplateSegment>? TemplateSegment { get; set; }
    }
}
