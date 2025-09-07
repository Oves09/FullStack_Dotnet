import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-binding-demo',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './binding-demo.component.html',
  styleUrl: './binding-demo.component.css'
})
export class BindingDemoComponent {
  title: string = 'Angular Property Binding Example';
  imageUrl: string = 'https://angular.io/assets/images/logos/angular/angular.png';
  isDisabled: boolean = true;

count: number = 0;
increaseCount() {
  this.count++
}

//For two-way binding demo

username: string ='';

}

