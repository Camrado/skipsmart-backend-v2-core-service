﻿using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Courses;

public class Course : Entity {
    public CourseName CourseName { get; private set; }
    public Semester Semester { get; private set; }
    
    public int GroupId { get; private set; }
    
    public Course(Guid id, CourseName courseName, Semester semester, int groupId) : base(id) {
        CourseName = courseName;
        Semester = semester;
        GroupId = groupId;
    }
}