using Microsoft.AspNetCore.Identity;

namespace GymLog.Models
{
    public class AppUser : IdentityUser
    {
        public List<Template>? Templates { get; set; }
        public List<Workout>? Workouts { get; set; }
        public List<Exercise>? Exercises { get; set; }
    }
}
