namespace GymLog.Models
{
    public class BodyPart
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Exercise>? Exercises { get; set; }
        public List<BodyPartExercise>? BodyPartExercises { get; set; }
    }
}
