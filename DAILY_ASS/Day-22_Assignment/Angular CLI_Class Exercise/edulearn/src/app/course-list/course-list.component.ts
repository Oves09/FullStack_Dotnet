import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { Course } from '../models/course';

@Component({
  selector: 'app-course-list',
  imports: [CommonModule],
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent {
    @Output() selectCourse = new EventEmitter<Course>();

  courses: Course[] = [
    { id: 1, title: 'Angular Basics', instructor: 'Motu', price: 49, available: true },
    { id: 2, title: 'TypeScript Deep Dive', instructor: 'Jack', price: 59, available: true },
    { id: 3, title: 'RxJS In Practice', instructor: 'Oggy', price: 39, available: false }
  ];

  onViewDetails(course: Course) {
    this.selectCourse.emit(course);
  }
}
