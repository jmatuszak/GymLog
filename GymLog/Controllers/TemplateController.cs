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
        public TemplateVM AddSet(TemplateVM templateVM, int i)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.TemplateSegmentsVM == null) throw new ArgumentNullException();
            if (templateVM.TemplateSegmentsVM[i].SetsVM == null)
                templateVM.TemplateSegmentsVM[i].SetsVM = new List<SetVM>();
            templateVM.TemplateSegmentsVM[i].SetsVM.Add(new SetVM());
            return templateVM;

        }
        public TemplateVM RemoveSet(TemplateVM templateVM, int segment, int set)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.TemplateSegmentsVM == null) throw new ArgumentNullException();
            if (templateVM.TemplateSegmentsVM[segment].SetsVM == null)
                templateVM.TemplateSegmentsVM[segment].SetsVM = new List<SetVM>();
            var delete = templateVM.TemplateSegmentsVM[segment].SetsVM[set];
            templateVM.TemplateSegmentsVM[segment].SetsVM.RemoveAt(set);
            return templateVM;
        }


        public IActionResult AddSetCreate(TemplateVM templateVM, [FromQuery(Name = "set")] int i)
        {
            templateVM = AddSet(templateVM, i);
            return View("Create", templateVM);
        }
        public IActionResult RemoveSetCreate(TemplateVM templateVM, [FromQuery(Name = "segment")] int i, [FromQuery(Name = "set")] int j)
        {
            templateVM = RemoveSet(templateVM, i, j);
            ModelState.Clear();
            return View("Create", templateVM);
        }



        //<-----------------------   Excercise   ---------------------> 

        public TemplateVM AddTemplateSegment(TemplateVM templateVM)
        {
            if (templateVM.TemplateSegmentsVM == null) templateVM.TemplateSegmentsVM = new List<TemplateSegmentVM>();
            templateVM.TemplateSegmentsVM.Add(new TemplateSegmentVM());
            return templateVM;
        }
        public TemplateVM RemoveTemplateSegment(TemplateVM templateVM, int segment)
        {
            if (templateVM.TemplateSegmentsVM == null) templateVM.TemplateSegmentsVM = new List<TemplateSegmentVM>();
            templateVM.TemplateSegmentsVM.RemoveAt(segment);
            ModelState.Clear();
            return templateVM;
        }

        public IActionResult AddTemplateSegmentCreate(TemplateVM templateVM)
        {
            if (templateVM.TemplateSegmentsVM == null) templateVM.TemplateSegmentsVM = new List<TemplateSegmentVM>();
            templateVM = AddTemplateSegment(templateVM);
            return View("Create", templateVM);

        }
        public IActionResult RemoveTemplateSegmentCreate(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM.TemplateSegmentsVM == null) return View("Error");
            templateVM = RemoveTemplateSegment(templateVM, segment);
            return View("Create", templateVM);
        }


        //<-----------------------   Create   ---------------------> 
        public async Task<IActionResult> Create(TemplateVM? templateVM)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            if (templateVM.ExcercisesConcatVM == null)
            {
                var excercisesConcatVM = await CreateExcerciseConcatList();
                templateVM.ExcercisesConcatVM = excercisesConcatVM;
            }
            templateVM = AddTemplateSegment(templateVM);
            return View(templateVM);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTemplate(TemplateVM templateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(templateVM);
            }
            Template template = new Template() { Name = templateVM.Name };

            template.TemplateSegments = new List<TemplateSegment>();
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
                var template = await _context.Templates.FirstOrDefaultAsync(t => t.Id == id);
                templateVM = templateVM ?? new TemplateVM();
                templateVM.TemplateSegmentsVM = templateVM.TemplateSegmentsVM ?? new List<TemplateSegmentVM>();
                templateVM.Id = template.Id;
                templateVM.Name = template.Name;
                if (templateVM.ExcercisesConcatVM == null)
                {
                    var excercisesConcatVM = await CreateExcerciseConcatList();
                    templateVM.ExcercisesConcatVM = excercisesConcatVM;
                }
                var segmentsVM = new List<TemplateSegmentVM>();
                if (template.TemplateSegments!=null && template.TemplateSegments.Count>0)
                foreach(var segment in template.TemplateSegments)
                {
                    var setsVM = new List<SetVM>();
                    var segmentVM = new TemplateSegmentVM();
                    segmentVM.Id = segment.Id;
                    segmentVM.TemplateId = segment.TemplateId;
                    segmentVM.ExcerciseId = segment.ExcerciseId;

                    foreach(var set in segment.Sets)
                    {
                        setsVM.Add(new SetVM()
                        {
                            Description = set.Description,
                            Weight = set.Weight,
                            Reps = set.Reps,
                            Id = set.Id,
                            TemplateSegmentId = set.TemplateSegmentId,
                        });
                    }
                    segmentVM.SetsVM = setsVM;
                    templateVM.TemplateSegmentsVM.Add(segmentVM);
                }
                
            }
            return View(templateVM);

        }

    }
}
