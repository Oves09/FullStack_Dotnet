import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';  // <-- Import this
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-template-driven',
  standalone: true,
  imports: [FormsModule, CommonModule],  // <-- Add here
  templateUrl: './template-driven.component.html',
  styleUrls: ['./template-driven.component.css']
})
export class TemplateDrivenComponent {
  user = {
    name: '',
    email: '',
    password: ''
  };

  onSubmit() {
    console.log('Form Submitted:', this.user);
    alert('Welcome ' + this.user.name + '!');
  }

}
