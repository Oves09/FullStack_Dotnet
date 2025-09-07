import { Injectable } from '@angular/core';

export interface Student {
  id: number;
  name: string;
  age: number;
  course: string;
}

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private students: Student[] = [
    {id:1, name: 'Rushikesh', age: 22, course: 'Computer Science'},
    {id:2, name: 'Rohit', age: 22, course: 'Computer Science'},
    {id:3, name: 'Ganesh', age: 22, course: 'Data Analyst'},
    {id:4, name: 'Aryan', age: 23, course: 'Data Science'},
    {id:5, name: 'Viraj', age: 21, course: 'Social Science'},
    {id:6, name: 'Dilip', age: 23, course: 'Java'},

  ];

  getStudents(): Student[]{
    return this.students;
  }

  getStudentsById(id: number): Student | undefined {
    return this.students.find(s => s.id === id);
  }
}
