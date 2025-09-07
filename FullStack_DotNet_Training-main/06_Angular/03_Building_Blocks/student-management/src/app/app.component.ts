import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StudentListComponent } from './student/student-list/student-list.component';

@Component({
  selector: 'app-root',
  standalone:true,
  imports: [StudentListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'student-management';
}
