﻿using GymLog.Data;
using GymLog.Data.Enum;
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


        //<-----------------------   Set   ---------------------> 

        public IActionResult AddSet(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.WorkoutSegmentsVM == null) throw new ArgumentNullException();
            if (templateVM.WorkoutSegmentsVM[segment] == null) throw new ArgumentNullException();
            templateVM.WorkoutSegmentsVM[segment].SetsVM ??= new List<SetVM>();
            templateVM.WorkoutSegmentsVM[segment].SetsVM.Add(new SetVM());
            return View(templateVM.ActionName, templateVM);
        }
        public IActionResult RemoveSet(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.WorkoutSegmentsVM == null) throw new ArgumentNullException();
            if (templateVM.WorkoutSegmentsVM[segment].SetsVM == null) return View("Error");
            templateVM.WorkoutSegmentsVM[segment].SetsVM.RemoveAt(templateVM.WorkoutSegmentsVM[segment].SetsVM.Count - 1);
            ModelState.Clear();
            return View(templateVM.ActionName, templateVM);
        }



        //<-----------------------   Excercise   ---------------------> 

        public IActionResult AddWorkoutSegment(TemplateVM templateVM)
        {
            templateVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>();
            var segment = new WorkoutSegmentVM()
            {
                SetsVM = new List<SetVM>() { new SetVM() }
            };
            templateVM.WorkoutSegmentsVM.Add(segment);
            return View(templateVM.ActionName, templateVM);

        }
        public IActionResult RemoveWorkoutSegment(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM.WorkoutSegmentsVM == null) return View("Error");
            templateVM.WorkoutSegmentsVM.RemoveAt(segment);
            ModelState.Clear();
            return View(templateVM.ActionName, templateVM);
        }


        //<-----------------------   Create   ---------------------> 
        public async Task<IActionResult> Create(TemplateVM? templateVM)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            templateVM ??= new TemplateVM();

            templateVM.Excercises = _context.Excercises.ToList();
            templateVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>()
                {
                    new WorkoutSegmentVM()
                    {
                        SetsVM = new List<SetVM>{ new SetVM() }
                    }
                };
            templateVM.ActionName = "create";
            return View(templateVM);

        }
        [HttpPost]
        public IActionResult CreatePost(TemplateVM templateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(templateVM);
            }
            Template template = new Template()
            {
                Name = templateVM.Name,
                WorkoutSegments = new List<WorkoutSegment>()
            };

            if (templateVM.WorkoutSegmentsVM != null)
                foreach (var segment in templateVM.WorkoutSegmentsVM)
                {
                    List<Set> sets = new List<Set>();
                    if (segment.SetsVM != null)
                        foreach (var set in segment.SetsVM)
                            sets.Add(new Set()
                            {
                                Weight = set.Weight,
                                Reps = set.Reps,
                                //WorkoutSegmentId = template.Id
                            });
                    template.WorkoutSegments.Add(new WorkoutSegment
                    {
                        WeightType = segment.WeightType,
                        Description = segment.Description,
                        Order = segment.Order,
                        Sets = sets,
                        ExcerciseId = segment.ExcerciseId,
                    });
                }
            _context.Add(template);
            _context.SaveChanges();

            foreach (var segment in template.WorkoutSegments)
            {
                foreach (var set in segment.Sets)
                    set.WorkoutSegmentId = segment.Id;
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

                var template = await _context.Templates.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(t => t.Id == id);
                templateVM = templateVM ?? new TemplateVM();
                templateVM.Excercises = _context.Excercises.ToList();
                templateVM.WorkoutSegmentsVM = templateVM.WorkoutSegmentsVM ?? new List<WorkoutSegmentVM>();
                templateVM.Id = template.Id;
                templateVM.Name = template.Name;
                templateVM.ActionName = "Edit";

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

            }

            return View(templateVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(TemplateVM templateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit. ModelState Error.");
                return View(templateVM);
            }
            var template = await _context.Templates.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(i => i.Id == templateVM.Id);
            if (template == null) return View("Error");
            if (templateVM.WorkoutSegmentsVM == null) return View("Error");
            var segments = new List<WorkoutSegment>();
            var segmentsVMIds = new List<int>();
            var segmentsIdToRemove = new List<int>();
            //Removing segments that are not in templateVM
            //Creating list with segments IDs
            foreach (var segmentsVM in templateVM.WorkoutSegmentsVM)
            {
                segmentsVMIds.Add(segmentsVM.Id);
            }
            //Creating list with segments IDs to remove
            foreach (var segment in template.WorkoutSegments)
            {
                if (!segmentsVMIds.Contains(segment.Id))
                {
                    segmentsIdToRemove.Add(segment.Id);
                }
            }
            //Removing segments
            foreach(var id in segmentsIdToRemove)
            {
                var segmentToRemove = await _context.WorkoutSegments.Include(s=>s.Sets).FirstOrDefaultAsync(i => i.Id == id);
                //Removing sets
                if (segmentToRemove != null)
                {
                    if (segmentToRemove.Sets != null)
                    {
                        _context.Sets.RemoveRange(segmentToRemove.Sets);
                        await _context.SaveChangesAsync();
                    }
                    _context.WorkoutSegments.Remove(segmentToRemove);
                    await _context.SaveChangesAsync();
                }
            }

            //Updating remaining segments
            foreach (var segmentVM in templateVM.WorkoutSegmentsVM)
            {
                var segment = new WorkoutSegment();
                if (segmentVM.Id != 0)
                {
                    segment = await _context.WorkoutSegments.FirstOrDefaultAsync(i => i.Id == segmentVM.Id);
                    if (segment != null)
                    {
                        segment.ExcerciseId = segmentVM.ExcerciseId;
                        segment.Description = segmentVM.Description;
                        segment.WeightType = segmentVM.WeightType;
                        segment.TemplateId = templateVM.Id;
                        _context.WorkoutSegments.Update(segment);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    segment = new WorkoutSegment();
                    segment.ExcerciseId = segmentVM.ExcerciseId;
                    segment.Description = segmentVM.Description;
                    segment.WeightType = segmentVM.WeightType;
                    segment.TemplateId = templateVM.Id;
                    _context.WorkoutSegments.Add(segment);
                    await _context.SaveChangesAsync();

                }
                //Removing sets that are not in segmentVM
                var setsVMIds = new List<int>();
                var setsIdToRemove = new List<int>();
                //Creating list with sets IDs from VM
                if (segmentVM.SetsVM!=null)
                foreach (var setVM in segmentVM.SetsVM)
                {
                    setsVMIds.Add(setVM.Id);
                }
                //Removing sets
                var segm = template.WorkoutSegments.FirstOrDefault(i => i.Id == segmentVM.Id);
                if (segm!=null)
                {
                    if(segm.Sets!=null)
                    foreach(var set in segm.Sets)
                    {
                        if (!setsVMIds.Contains(set.Id))
                        {
                                var setToRemove = _context.Sets.FirstOrDefault(i => i.Id == set.Id);
                                if (setToRemove != null)
                                _context.Sets.Remove(setToRemove);
                                await _context.SaveChangesAsync();
                        }
                    }
                }

                var sets = new List<Set>();
                if (segmentVM.SetsVM != null)
                {
                    foreach (var setVM in segmentVM.SetsVM)
                    {
                        var set = new Set();
                        if (setVM.Id != 0)
                        {
                            set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == setVM.Id);

                            if (set != null)
                            {
                                set.Reps = setVM.Reps;
                                set.Weight = setVM.Weight;
                                set.WorkoutSegmentId = segment.Id;
                                _context.Update(set);
                                await _context.SaveChangesAsync();

                                sets.Add(set);
                            }
                        }
                        else
                        {
                            set.Reps = setVM.Reps;
                            set.Weight = setVM.Weight;
                            set.WorkoutSegmentId = segment.Id;
                            _context.Sets.Add(set);
                            await _context.SaveChangesAsync();

                            sets.Add(set);
                        }
                    }
                }
                segment.Sets = sets;
                segments.Add(segment);
            }
            template.Name = templateVM.Name;
            template.WorkoutSegments = segments;
            _context.Update(template);
            await _context.SaveChangesAsync();

            /*
            _context.Sets.Where(s => s.WorkoutSegmentId == null);*/
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(s => s.Id == id);
            if (template != null)
            {
                _context.Templates.Remove(template);
                var segments = _context.WorkoutSegments.Where(x => x.TemplateId == id).ToList();
                if (segments.Any()) _context.RemoveRange(segments);
                foreach (var segment in segments)
                {
                    var sets = _context.Sets.Where(x => x.WorkoutSegmentId == id).ToList();
                    if (sets.Any()) _context.RemoveRange(sets);
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}