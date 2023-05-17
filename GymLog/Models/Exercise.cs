using GymLog.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymLog.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<BodyPart>? BodyParts { get; set; }
        public List<BodyPartExercise>? BodyPartExercises { get; set; }
        //public WorkoutSegment? WorkoutSegment { get; set; }
    }
}
