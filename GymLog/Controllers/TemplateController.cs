using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace GymLog.Controllers
{
    public class TemplateController : Controller
    {
        private readonly AppDbContext _context;

        public TemplateController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var templates = _context.Templates.ToList();
            return View(templates);
        }


        public async Task<List<CreateExcerciseConcatVM>> CreateExcerciseConcatList()
        {
            var excercises = await _context.Excercises.ToListAsync();
            var excercisesVM = new List<CreateExcerciseConcatVM>();

            foreach (var item in excercises)
            {
                excercisesVM.Add(new CreateExcerciseConcatVM()
                {
                    Id = item.Id,
                    Name = item.Name + " - " + item.WeightType,
                });
            }
            return excercisesVM;
        }

        //<-----------------------   Set   ---------------------> 

        public IActionResult AddSet(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.TemplateSegmentsVM == null) throw new ArgumentNullException();
            if (templateVM.TemplateSegmentsVM[segment] == null) throw new ArgumentNullException();
            templateVM.TemplateSegmentsVM[segment].SetsVM ??= new List<SetVM>();
            templateVM.TemplateSegmentsVM[segment].SetsVM.Add(new SetVM());
            return View(templateVM.ActionName, templateVM);
        }
        /*        public IActionResult RemoveSet(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment, [FromQuery(Name = "set")] int set)
                {
                    if (templateVM == null) throw new ArgumentNullException();
                    if (templateVM.TemplateSegmentsVM == null) throw new ArgumentNullException();
                    if (templateVM.TemplateSegmentsVM[segment].SetsVM == null) return View("Error");
                    if (templateVM.TemplateSegmentsVM[segment].SetsVM[set] == null)  return View("Error");
                    templateVM.TemplateSegmentsVM[segment].SetsVM.RemoveAt(set);
                    ModelState.Clear();
                    return View(templateVM.ActionName, templateVM);
                }*/
        public IActionResult RemoveSet(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.TemplateSegmentsVM == null) throw new ArgumentNullException();
            if (templateVM.TemplateSegmentsVM[segment].SetsVM == null) return View("Error");
            templateVM.TemplateSegmentsVM[segment].SetsVM.RemoveAt(templateVM.TemplateSegmentsVM[segment].SetsVM.Count-1);
            ModelState.Clear();
            return View(templateVM.ActionName, templateVM);
        }



        //<-----------------------   Excercise   ---------------------> 

        public IActionResult AddTemplateSegment(TemplateVM templateVM)
        {
            templateVM.TemplateSegmentsVM ??= new List<TemplateSegmentVM>();
            var segment = new TemplateSegmentVM()
            {
                SetsVM = new List<SetVM>() { new SetVM() }
            };
            templateVM.TemplateSegmentsVM.Add(segment);
            return View(templateVM.ActionName, templateVM);

        }
        public IActionResult RemoveTemplateSegment(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM.TemplateSegmentsVM == null) return View("Error");
            templateVM.TemplateSegmentsVM.RemoveAt(segment);
            ModelState.Clear();
            return View(templateVM.ActionName, templateVM);
        }


        //<-----------------------   Create   ---------------------> 
        public async Task<IActionResult> Create(TemplateVM? templateVM)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            templateVM ??= new TemplateVM();
            if (templateVM.ExcercisesConcatVM == null)
            {
                var excercisesConcatVM = await CreateExcerciseConcatList();
                templateVM.ExcercisesConcatVM = excercisesConcatVM;
            }
            templateVM.TemplateSegmentsVM ??= new List<TemplateSegmentVM>()
                {
                    new TemplateSegmentVM()
                    {
                        SetsVM = new List<SetVM>{ new SetVM() }
                    }
                };
            templateVM.ActionName = "create";
            return View(templateVM);

        }
        [HttpPost]
        public IActionResult CreateTemplate(TemplateVM templateVM)
        {
            var tempolatex = new TemplateSegment();
            if (!ModelState.IsValid)
            {
                return View(templateVM);
            }
            Template template = new Template()
            {
                Name = templateVM.Name,
                TemplateSegments = new List<TemplateSegment>()
            };

            if (templateVM.TemplateSegmentsVM != null)
                foreach (var segment in templateVM.TemplateSegmentsVM)
                {
                    List<Set> sets = new List<Set>();
                    if (segment.SetsVM != null)
                        foreach (var set in segment.SetsVM)
                            sets.Add(new Set()
                            {
                                Weight = set.Weight,
                                Reps = set.Reps,
                                //TemplateSegmentId = template.Id
                            });
                    template.TemplateSegments.Add(new TemplateSegment
                    {
                        Description = segment.Description,
                        Order = segment.Order,
                        Sets = sets,
                        ExcerciseId = segment.ExcerciseId,
                    });
                }
            _context.Add(template);
            _context.SaveChanges();

            foreach (var segment in template.TemplateSegments)
            {
                foreach (var set in segment.Sets)
                    set.TemplateSegmentId = segment.Id;
            }
            _context.Update(template);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        //<-----------------------   Edit   ---------------------> 

        public async Task<IActionResult> Edit(TemplateVM? templateVM, int? id)
        {
            if (id != null)
            {
                var template = await _context.Templates.Include(t => t.TemplateSegments).FirstOrDefaultAsync(t => t.Id == id);
                templateVM = templateVM ?? new TemplateVM();
                templateVM.TemplateSegmentsVM = templateVM.TemplateSegmentsVM ?? new List<TemplateSegmentVM>();
                templateVM.Id = template.Id;
                templateVM.Name = template.Name;
                templateVM.ActionName = "Edit";
                if (templateVM.ExcercisesConcatVM == null)
                {
                    var excercisesConcatVM = await CreateExcerciseConcatList();
                    templateVM.ExcercisesConcatVM = excercisesConcatVM;
                }
                var segmentsVM = new List<TemplateSegmentVM>();
                if (template.TemplateSegments != null && template.TemplateSegments.Count > 0)
                    for (int t = 0; t < template.TemplateSegments.Count; t++) 
                    {
                        template.TemplateSegments[t].Sets = _context.Sets.Where(s => s.TemplateSegmentId == template.TemplateSegments[t].Id).ToList();
                        var setsVM = new List<SetVM>();
                        var segmentVM = new TemplateSegmentVM();
                        segmentVM.Id = template.TemplateSegments[t].Id;
                        segmentVM.TemplateId = template.TemplateSegments[t].TemplateId;
                        segmentVM.ExcerciseId = template.TemplateSegments[t].ExcerciseId;
                        if (template.TemplateSegments[t].Sets != null)
                        {
                            for (var s = 0; s < template.TemplateSegments[t].Sets.Count; s++)
                            {
                                if (template.TemplateSegments[t].Sets[s].Id != null)
                                {
                                    setsVM.Add(new SetVM()
                                    {
                                        Id = template.TemplateSegments[t].Sets[s].Id,
                                        Description = template.TemplateSegments[t].Sets[s].Description,
                                        Weight = template.TemplateSegments[t].Sets[s].Weight,
                                        Reps = template.TemplateSegments[t].Sets[s].Reps,
                                        TemplateSegmentId = template.TemplateSegments[t].Sets[s].TemplateSegmentId,
                                    });
                                }
                                else
                                {
                                    setsVM.Add(new SetVM()
                                    {
                                        Description = template.TemplateSegments[t].Sets[s].Description,
                                        Weight = template.TemplateSegments[t].Sets[s].Weight,
                                        Reps = template.TemplateSegments[t].Sets[s].Reps,
                                        TemplateSegmentId = template.TemplateSegments[t].Sets[s].TemplateSegmentId,
                                    });
                                }

                            }
                        }

                        segmentVM.SetsVM = setsVM;
                        templateVM.TemplateSegmentsVM.Add(segmentVM);
                    }

            }

            return View(templateVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditTemplate(TemplateVM templateVM)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit. ModelState Error.");
                return View(templateVM);
            }
            var template = await _context.Templates.Include(t => t.TemplateSegments).FirstOrDefaultAsync(i => i.Id == templateVM.Id);
            
            var segments = new List<TemplateSegment>();
            if(templateVM.TemplateSegmentsVM!= null)
            {
                foreach(var segmentVM in templateVM.TemplateSegmentsVM)
                {
                    var sets = new List<Set>();
                    if(segmentVM.SetsVM!= null)
                    {
                        foreach (var setVM in segmentVM.SetsVM)
                        {
                            sets.Add(new Set()
                            {
                                Id = setVM.Id,
                                Reps = setVM.Reps,
                                Weight = setVM.Weight,
                                TemplateSegmentId = setVM.TemplateSegmentId,
                            });
                        }
                    }
                    segments.Add(new TemplateSegment()
                    {
                        ExcerciseId= segmentVM.ExcerciseId,
                        Order= segmentVM.Order,
                        Description= segmentVM.Description,
                        Sets = sets
                    });
                }
                _context.RemoveRange(template.TemplateSegments);
                //_context.RemoveRange(segments) ;
                template.Id = templateVM.Id;
                template.Name ??= templateVM.Name;
                template.TemplateSegments = segments;
                _context.Update(template);
                _context.SaveChanges();
                _context.Sets.Where(s => s.TemplateSegmentId == null);
            }
            return RedirectToAction("Index");
        }
    }
}