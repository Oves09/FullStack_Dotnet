import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post, ApprovePostRequest } from '../models/post.model';
import { User } from '../models/user.model';
import { Report, ReviewReportRequest, ReportStatus } from '../models/report.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private readonly API_URL = `${environment.apiUrl}/api/admin`;

  constructor(private http: HttpClient) {}

  getPendingPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(`${environment.apiUrl}/api/admin/pending-posts`);
  }

  approvePost(request: ApprovePostRequest): Observable<any> {
    return this.http.post(`${environment.apiUrl}/api/admin/approve-post`, request);
  }

  getUsers(page: number = 1, pageSize: number = 10): Observable<User[]> {
    return this.http.get<User[]>(`${environment.apiUrl}/api/admin/users?page=${page}&pageSize=${pageSize}`);
  }

  updateUserStatus(userId: string, isActive: boolean): Observable<any> {
    return this.http.put(`${environment.apiUrl}/api/admin/users/${userId}/status`, isActive);
  }

  getReports(status?: ReportStatus): Observable<Report[]> {
    const statusParam = status !== undefined ? `?status=${status}` : '';
    return this.http.get<Report[]>(`${environment.apiUrl}/api/admin/reports${statusParam}`);
  }

  getAllReports(): Observable<Report[]> {
    return this.http.get<Report[]>(`${environment.apiUrl}/api/admin/reports`);
  }

  reviewReport(request: ReviewReportRequest): Observable<any> {
    return this.http.post(`${environment.apiUrl}/api/admin/review-report`, request);
  }

  toggleUserStatus(userId: string, isActive: boolean): Observable<any> {
    return this.http.put(`${environment.apiUrl}/api/admin/users/${userId}/status`, { isActive });
  }

  deleteUser(userId: string): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/api/admin/users/${userId}`);
  }

  // User CRUD operations
  createUser(userData: any): Observable<User> {
    return this.http.post<User>(`${environment.apiUrl}/api/admin/users`, userData);
  }

  getUserById(userId: string): Observable<User> {
    return this.http.get<User>(`${environment.apiUrl}/api/admin/users/${userId}`);
  }

  updateUser(userId: string, userData: any): Observable<User> {
    return this.http.put<User>(`${environment.apiUrl}/api/admin/users/${userId}`, userData);
  }

  resetUserPassword(userId: string, newPassword: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}/api/admin/users/${userId}/reset-password`, { newPassword });
  }

  getRoles(): Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiUrl}/api/admin/users/roles`);
  }

  // Group management
  createGroup(groupData: { groupName: string; description?: string; memberIds?: string[] }): Observable<any> {
    return this.http.post(`${environment.apiUrl}/api/groups`, groupData);
  }

  getGroups(page: number = 1, pageSize: number = 10): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/api/groups?page=${page}&pageSize=${pageSize}`);
  }

  getGroup(id: number): Observable<any> {
    return this.http.get(`${environment.apiUrl}/api/groups/${id}`);
  }

  updateGroup(id: number, groupData: any): Observable<any> {
    return this.http.put(`${environment.apiUrl}/api/groups/${id}`, groupData);
  }

  deleteGroup(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/api/groups/${id}`);
  }
}
