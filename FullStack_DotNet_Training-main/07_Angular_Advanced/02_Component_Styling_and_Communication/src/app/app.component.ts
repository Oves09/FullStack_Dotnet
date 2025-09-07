import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ParentComponent } from "./components/parent/parent.component";


@Component({
  selector: 'app-root',
  imports: [ ParentComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = '02_Component_Styling_and_Communication';
}
