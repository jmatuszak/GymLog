using GymLog.Data.Enum;
using GymLog.Models;

namespace GymLog.ViewModels
{
    public class CreateExcerciseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WeightType WeightType { get; set; }
        public List<BodyPart>? BodyParts { get; set; }
    }
}
