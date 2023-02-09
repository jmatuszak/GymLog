namespace GymLog.Models
{
    public class BodyPart
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Excercise>? Excercises { get; set; }
        public List<BodyPartExcercise>? BodyPartExcercises { get; set; }
    }
}
