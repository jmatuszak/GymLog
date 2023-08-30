using GymLog.Data.Enum;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymLog.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<BodyPart>? BodyParts { get; set; }
        public List<BodyPartExercise>? BodyPartExercises { get; set; }
        public string? ImageSrc { get; set; }
    }
}
