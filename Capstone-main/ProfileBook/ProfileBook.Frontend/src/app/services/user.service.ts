import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, UpdateProfileRequest } from '../models/user.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly API_URL = `${environment.apiUrl}/api/users`;

  constructor(private http: HttpClient) {}

  searchUsers(query: string, page: number = 1, pageSize: number = 10): Observable<User[]> {
    return this.http.get<User[]>(`${this.API_URL}/search?query=${encodeURIComponent(query)}&page=${page}&pageSize=${pageSize}`);
  }

  getUser(id: string): Observable<User> {
    return this.http.get<User>(`${this.API_URL}/${id}`);
  }

  getProfile(): Observable<User> {
    return this.http.get<User>(`${this.API_URL}/profile`);
  }

  updateProfile(request: UpdateProfileRequest): Observable<User> {
    const formData = new FormData();
    formData.append('firstName', request.firstName);
    formData.append('lastName', request.lastName);
    if (request.bio) {
      formData.append('bio', request.bio);
    }
    if (request.profileImage) {
      formData.append('profileImage', request.profileImage);
    }
    return this.http.put<User>(`${this.API_URL}/profile`, formData);
  }
}
