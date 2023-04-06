﻿namespace GymLog.Models
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ExcerciseSet>? ExcerciseSets { get; set; }
    }
}
