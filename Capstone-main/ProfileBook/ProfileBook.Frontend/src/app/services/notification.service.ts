import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Notification {
  notificationId: number;
  userId: string;
  title: string;
  message: string;
  type: string;
  isRead: boolean;
  createdAt: Date;
  postId?: number;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly API_URL = `${environment.apiUrl}/api/notifications`;
  private unreadCountSubject = new BehaviorSubject<number>(0);
  public unreadCount$ = this.unreadCountSubject.asObservable();

  constructor(private http: HttpClient) {}

  getNotifications(page: number = 1, pageSize: number = 20): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.API_URL}?page=${page}&pageSize=${pageSize}`);
  }

  getUnreadCount(): Observable<number> {
    return this.http.get<number>(`${this.API_URL}/unread-count`);
  }

  markAsRead(notificationId: number): Observable<any> {
    return this.http.put(`${this.API_URL}/${notificationId}/mark-read`, {});
  }

  markAllAsRead(): Observable<any> {
    return this.http.put(`${this.API_URL}/mark-all-read`, {});
  }

  updateUnreadCount() {
    this.getUnreadCount().subscribe({
      next: (count) => {
        this.unreadCountSubject.next(count);
      },
      error: (error) => {
        console.error('Error fetching unread notification count:', error);
        // Set count to 0 on error to prevent UI issues
        this.unreadCountSubject.next(0);
      }
    });
  }

  refreshUnreadCount() {
    this.updateUnreadCount();
  }
}
