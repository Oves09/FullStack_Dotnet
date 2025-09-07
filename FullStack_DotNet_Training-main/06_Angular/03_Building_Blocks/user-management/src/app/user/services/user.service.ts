import { Injectable } from '@angular/core';

export interface User {
  id: number;
  name: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private users: User[] = [
    { id: 1, name: 'Abhijeet', email: 'abhijeet@example.com' },
    { id: 2, name: 'Rohan', email: 'rohan@example.com' },
    { id: 3, name: 'Sneha', email: 'sneha@example.com' }
  ];

  getUsers(): User[] {
    return this.users;
  }

  getUserById(id: number): User | undefined {
    return this.users.find(user => user.id === id);
  }
}
