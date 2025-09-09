import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Product } from '../product.model';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent {
  @Input() product!: Product;
  @Output() feedback = new EventEmitter<string>();

  userFeedback: string = '';

  sendFeedback() {
    if (this.userFeedback.trim()) {
      this.feedback.emit(`${this.product.name}: ${this.userFeedback}`);
      this.userFeedback = '';
    }
  }
}
