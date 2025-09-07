import { Component } from '@angular/core';
import { StudentService,Student } from '../services/student.service';
import { StudentDetailComponent } from '../student-detail/student-detail.component';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [StudentDetailComponent, NgFor],
  templateUrl: './student-list.component.html',
  styleUrl: './student-list.component.css'
})
export class StudentListComponent {
  students: Student[] =[];
  selectedStudent?: Student;

  constructor(private studentService: StudentService) {}

  ngOnInit() {
    this.students = this.studentService.getStudents();
  }

  onSelect(student: Student) {
    this.selectedStudent = student;
  }

}
