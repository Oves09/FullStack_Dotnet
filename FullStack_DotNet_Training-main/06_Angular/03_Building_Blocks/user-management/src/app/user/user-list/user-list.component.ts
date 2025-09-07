import { Component } from '@angular/core';
import { UserService, User } from '../services/user.service';
import { NgFor } from '@angular/common';
import { CommonModule } from '@angular/common';
import { UserDetailComponent } from '../user-detail/user-detail.component';

@Component({
  selector: 'app-user-list',
  imports:[NgFor, CommonModule, UserDetailComponent],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent {
  users: User[] = [];
  selectedUser?: User;

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.users = this.userService.getUsers();
  }

  onSelect(user: User) {
    this.selectedUser = user;
  }
}
