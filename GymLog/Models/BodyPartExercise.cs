namespace GymLog.Models
{
    public class BodyPartExercise
    {
        public int BodyPartId { get; set; }
        public BodyPart? BodyPart { get; set; }

        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }
    }
}
