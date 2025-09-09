import { Component } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { PriceFormatPipe } from '../../pipes/price-format-pipe';
import { Highlight } from '../../directives/highlight';

interface EventItem {
  name: string;
  date: string;   // ISO date string
  price: number;
  category: 'Conference' | 'Workshop' | 'Concert';
}

@Component({
  selector: 'app-event-list',
  standalone: true,
  imports: [CommonModule, DatePipe, PriceFormatPipe, Highlight],
  templateUrl: './event-list.html',
  styles: [`
    table { width: 100%; border-collapse: collapse; }
    th, td { padding: 12px; border-bottom: 1px solid #ddd; vertical-align: top; }
    th { text-align: left; background: #f7f7f7; }
    .name { font-weight: 600; }
    .category { font-size: 12px; opacity: 0.8; text-transform: capitalize; }
  `]
})
export class EventList {
   events = [
    { name: 'Tech Innovators Conference', date: '2025-09-12', price: 3500, category: 'Conference' },
    { name: 'Creative Writing Workshop', date: '2025-10-05', price: 800, category: 'Workshop' },
    { name: 'Rock Music Concert', date: '2025-11-20', price: 2500, category: 'Concert' },
    { name: 'AI & Machine Learning Summit', date: '2025-12-02', price: 5000, category: 'Conference' }
  ];

}
