import { Component } from '@angular/core';
import { UserListComponent } from './user/user-list/user-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [UserListComponent],  
  template: `
    <h1>User Management</h1>
    <app-user-list></app-user-list>
  `
})
export class AppComponent {}
