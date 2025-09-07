import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TemplateDrivenComponent } from "./template-driven/template-driven.component";
import { ReactiveFormsComponent } from "./reactive-forms/reactive-forms.component";


@Component({
  selector: 'app-root',
  imports: [TemplateDrivenComponent, TemplateDrivenComponent, ReactiveFormsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = '04_Forms';
}
