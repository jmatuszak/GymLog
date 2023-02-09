namespace GymLog.Models
{
    public class BodyPartExcercise
    {
        public int BodyPartId { get; set; }
        public BodyPart? BodyPart { get; set; }

        public int ExcerciseId { get; set; }
        public Excercise? Excercise { get; set; }
    }
}
