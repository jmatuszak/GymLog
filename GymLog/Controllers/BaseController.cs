using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Controllers
{
    public abstract class BaseController : Controller
    {
        protected AppDbContext _context { get; private set; }
        //private readonly UserManager<AppUser> _userManager;

        public BaseController()
        {
            _context = new AppDbContext();
        }

        // TEMPLATE TO  VIEW MODEL
        public async Task<TemplateVM> TemplateToTemplateVM(int id)
        {
            var templateVM = new TemplateVM();
            var template = await _context.Templates.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(t => t.Id == id);
            templateVM = templateVM ?? new TemplateVM();
            templateVM.Excercises = _context.Excercises.ToList();
            templateVM.WorkoutSegmentsVM = templateVM.WorkoutSegmentsVM ?? new List<WorkoutSegmentVM>();
            templateVM.Id = template.Id;
            templateVM.Name = template.Name;
            templateVM.ActionName = "Create";

            var segmentsVM = new List<WorkoutSegmentVM>();
            if (template.WorkoutSegments != null && template.WorkoutSegments.Count > 0)
                for (int t = 0; t < template.WorkoutSegments.Count; t++)
                {
                    template.WorkoutSegments[t].Sets = _context.Sets.Where(s => s.WorkoutSegmentId == template.WorkoutSegments[t].Id).ToList();
                    var setsVM = new List<SetVM>();
                    var segmentVM = new WorkoutSegmentVM();
                    segmentVM.Id = template.WorkoutSegments[t].Id;
                    segmentVM.TemplateId = template.WorkoutSegments[t].TemplateId;
                    segmentVM.ExcerciseId = template.WorkoutSegments[t].ExcerciseId;
                    segmentVM.Description = template.WorkoutSegments[t].Description;
                    segmentVM.WeightType = template.WorkoutSegments[t].WeightType;
                    if (template.WorkoutSegments[t].Sets != null)
                    {
                        for (var s = 0; s < template.WorkoutSegments[t].Sets.Count; s++)
                        {
                            if (template.WorkoutSegments[t].Sets[s].Id != null)
                            {
                                setsVM.Add(new SetVM()
                                {
                                    Id = template.WorkoutSegments[t].Sets[s].Id,
                                    Description = template.WorkoutSegments[t].Sets[s].Description,
                                    Weight = template.WorkoutSegments[t].Sets[s].Weight,
                                    Reps = template.WorkoutSegments[t].Sets[s].Reps,
                                    WorkoutSegmentId = template.WorkoutSegments[t].Sets[s].WorkoutSegmentId,
                                });
                            }
                            else
                            {
                                setsVM.Add(new SetVM()
                                {
                                    Description = template.WorkoutSegments[t].Sets[s].Description,
                                    Weight = template.WorkoutSegments[t].Sets[s].Weight,
                                    Reps = template.WorkoutSegments[t].Sets[s].Reps,
                                    WorkoutSegmentId = template.WorkoutSegments[t].Sets[s].WorkoutSegmentId,
                                });
                            }

                        }
                    }

                    segmentVM.SetsVM = setsVM;
                    templateVM.WorkoutSegmentsVM.Add(segmentVM);
                }

            return templateVM;
        }
    }
}
