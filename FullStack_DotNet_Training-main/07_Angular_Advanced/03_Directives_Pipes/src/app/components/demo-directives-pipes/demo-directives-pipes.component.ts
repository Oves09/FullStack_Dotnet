import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// Import custom directives & pipes
import { HighlightDirective } from '../../highlight.directive';
import { UnlessDirective } from '../../unless.directive';
import { CustomUppercasePipe } from '../../custom-uppercase.pipe';
import { FilterPipe } from '../../filter.pipe';

@Component({
  selector: 'app-demo-directives-pipes',
  standalone: true,
  imports: [
    CommonModule,       // for *ngFor, *ngIf
    FormsModule,        // for [(ngModel)]
    HighlightDirective,
    UnlessDirective,
    CustomUppercasePipe,
    FilterPipe
  ],
  templateUrl: './demo-directives-pipes.component.html',
  styleUrls: ['./demo-directives-pipes.component.css']
})
export class DemoDirectivesPipesComponent {
  title = 'Angular Directives & Pipes Demo';
  condition = false;

  names: string[] = ['Abhijeet', 'Rushikesh', 'Anita', 'Rohan'];
  searchText = '';
}
