import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HighlightDirective } from './directives/highlight.directive';
import { ReversePipe } from './pipes/reverse.pipe';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, HighlightDirective, ReversePipe], // Required for ngIf, ngFor, etc.
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Angular Directives & Pipes Demo';

  // For ngIf and ngFor
  showEmployees = true;
  employees = [
    { id: 1, name: 'Abhijeet', role: 'Developer', salary: 50000, joinDate: new Date(2023, 5, 15) },
    { id: 2, name: 'Anita', role: 'Designer', salary: 45000, joinDate: new Date(2022, 8, 10) },
    { id: 3, name: 'Rohan', role: 'Manager', salary: 70000, joinDate: new Date(2021, 2, 25) }
  ];

  // For ngClass and ngStyle
  highSalaryThreshold = 60000;

  toggleEmployees() {
    this.showEmployees = !this.showEmployees;
  }
}
