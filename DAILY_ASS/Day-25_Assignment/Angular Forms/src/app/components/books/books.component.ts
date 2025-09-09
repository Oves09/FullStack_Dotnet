import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { DataService } from '../../services/data.service';
import { BookItemComponent } from '../book-item/book-item.component';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [CommonModule, BookItemComponent],
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})
export class BooksComponent implements OnInit, OnDestroy {
  books: any[] = [];
  books$ = this.dataService.getBooks(); // async pipe
  subscription!: Subscription;

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.subscription = this.dataService.getBooks().subscribe(data => {
      this.books = data;
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
