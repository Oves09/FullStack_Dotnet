import { Component, Input } from '@angular/core';
import { NgIf } from '@angular/common';
import { Student } from '../services/student.service';
import { StudentListComponent } from '../student-list/student-list.component';

@Component({
  selector: 'app-student-detail',
  standalone: true,
  imports: [NgIf],
  templateUrl: './student-detail.component.html',
  styleUrl: './student-detail.component.css'
})
export class StudentDetailComponent {
  @Input() student?: Student;

}
