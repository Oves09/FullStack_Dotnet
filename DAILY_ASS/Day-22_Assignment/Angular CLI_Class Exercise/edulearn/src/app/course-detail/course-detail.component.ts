import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Course } from '../models/course';

@Component({
  selector: 'app-course-detail',
  standalone: true, 
  imports: [CommonModule, FormsModule],
  templateUrl: './course-detail.component.html',
  styleUrl: './course-detail.component.css'
})
export class CourseDetailComponent {
  @Input() course: Course | null = null;
}
