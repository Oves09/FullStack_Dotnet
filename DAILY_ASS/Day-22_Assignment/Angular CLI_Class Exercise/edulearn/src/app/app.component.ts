import { Component } from '@angular/core';
import { CourseListComponent } from './course-list/course-list.component';
import { CourseDetailComponent } from './course-detail/course-detail.component';
import { Course } from './models/course';

@Component({
  selector: 'app-root',
  imports: [CourseListComponent, CourseDetailComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title(title: any) {
    throw new Error('Method not implemented.');
  }
   selectedCourse: Course | null = null;

  onCourseSelected(course: Course) {
    this.selectedCourse = course;
  }
  
}
