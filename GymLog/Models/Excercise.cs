using GymLog.Data.Enum;

namespace GymLog.Models
{
    public class Excercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WeightType? WeightType { get; set; }

        public ICollection<BodyPart>? BodyParts { get; set; }
        public List<BodyPartExcercise>? BodyPartExcercises { get; set; }
    }
}
