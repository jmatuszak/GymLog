using GymLog.Data.Enum;
using GymLog.Models;

namespace GymLog.ViewModels
{
    public class ExcerciseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BodyPartVM>? BodyPartsVM { get; set; }
    }
}
