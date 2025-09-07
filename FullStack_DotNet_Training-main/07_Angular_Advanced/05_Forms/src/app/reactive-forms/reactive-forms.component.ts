import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-reactive-forms',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './reactive-forms.component.html',
  styleUrls: ['./reactive-forms.component.css']
})
export class ReactiveFormsComponent {

  userForm!: FormGroup;

  constructor(private fb: FormBuilder) {
    this.userForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      age: [null, [Validators.required, Validators.min(18)]]
    });
  }

  onSubmit() {
    if (this.userForm.valid) {
      alert('✅ Form submitted successfully!\n' + JSON.stringify(this.userForm.value, null, 2));
      console.log('Form submitted:', this.userForm.value);
    } else {
      alert('❌ Form is invalid. Please fix errors.');
      this.userForm.markAllAsTouched();  // ✅ force error messages to show
    }
  }

  // Easy getter for validation checks in template
  get f() {
    return this.userForm.controls;
  }
}
