import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductDetailsComponent } from '../product-details/product-details.component';
import { Product } from '../product.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ProductDetailsComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  products: Product[] = [
    { name: 'Laptop', price: 80000 },
    { name: 'Smartphone', price: 40000 },
    { name: 'Headphones', price: 2000 }
  ];

  selectedProduct?: Product;
  feedbackList: string[] = [];

  selectProduct(product: Product) {
    this.selectedProduct = product;
  }

  receiveFeedback(feedback: string) {
    this.feedbackList.push(feedback);
  }
}
