using GymLog.Interfaces;
using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace GymLog.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _context;

        public ExerciseRepository(AppDbContext context)
        {
            _context = context;
        }


        public void DeleteExercise(Exercise exercise)
        {
            _context.Remove(exercise);
            Save();
        }

        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            var exercise = await _context.Exercises.Include(b => b.BodyParts).FirstOrDefaultAsync(x => x.Id == id);
            return exercise;
        }

        public async Task<IEnumerable<Exercise>> GetExerciseListAsync()
        {
            var exercises = await _context.Exercises.Include(b => b.BodyParts).ToListAsync();
            return exercises;
        }

        public void InsertExercise(Exercise exercise)
        {
            exercise.BodyParts = null;
            _context.Exercises.Add(exercise);
            Save();
        }

        public void InsertBodyPartExercise(Exercise exercise)
        {
            var bodyPartExercises = new List<BodyPartExercise>();
            if (exercise.BodyParts != null)
                foreach (var bodyPart in exercise.BodyParts)
                {
                    bodyPartExercises.Add(new BodyPartExercise
                    {
                        BodyPartId = bodyPart.Id,
                        ExerciseId = exercise.Id
                    });
                }
            _context.BodyPartExercises.AddRangeAsync(bodyPartExercises);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateExercise(Exercise exercise)
        {
            _context.Update(exercise);
            Save();

        }

        public void UpdateBodyPartExercise(Exercise exercise)
        {
            var bodyPartExercises = _context.BodyPartExercises.Where(i => i.ExerciseId == exercise.Id);
            _context.RemoveRange(bodyPartExercises);
            if (exercise.BodyParts != null)
            {
                foreach (var bodyPart in exercise.BodyParts)
                {
                    BodyPartExercise bodyPartExercise = new BodyPartExercise()
                    {
                        BodyPartId = bodyPart.Id,
                        ExerciseId = exercise.Id
                    };
                    _context.BodyPartExercises.Add(bodyPartExercise);
                }
            }
            Save();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
