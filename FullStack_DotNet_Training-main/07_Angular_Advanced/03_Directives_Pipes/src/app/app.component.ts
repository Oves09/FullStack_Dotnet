import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

// Import your custom stuff
import { DemoDirectivesPipesComponent } from './components/demo-directives-pipes/demo-directives-pipes.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    DemoDirectivesPipesComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = '03_Directives_Pipes';
}
