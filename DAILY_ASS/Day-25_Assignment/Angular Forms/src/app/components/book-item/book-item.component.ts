import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DiscountPipe } from '../../pipes/discount.pipe';

@Component({
  selector: 'app-book-item',
  standalone: true,
  imports: [CommonModule, DiscountPipe],
  templateUrl: './book-item.component.html',
  styleUrls: ['./book-item.component.css']
})
export class BookItemComponent {
  @Input() book: any;
}
