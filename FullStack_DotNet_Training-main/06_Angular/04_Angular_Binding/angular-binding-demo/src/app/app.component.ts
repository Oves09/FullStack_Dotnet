import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BindingDemoComponent } from './binding-demo/binding-demo.component';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ BindingDemoComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'angular-binding-demo';
}
