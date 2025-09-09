import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

export interface GroupMessage {
  messageId: number;
  groupId: number;
  userId: string;
  userName: string;
  userFullName: string;
  content: string;
  createdAt: Date;
}

export interface CreateGroupMessage {
  groupId: number;
  content: string;
}

export interface UserGroup {
  groupId: number;
  groupName: string;
  description?: string;
  createdByUserId: string;
  createdByUserName: string;
  createdAt: Date;
  updatedAt: Date;
  isActive: boolean;
  memberCount: number;
  members: any[];
}

@Injectable({
  providedIn: 'root'
})
export class UserGroupsService {
  private apiUrl = 'http://localhost:5120/api/usergroups';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  // Get user's groups
  getMyGroups(): Observable<UserGroup[]> {
    return this.http.get<UserGroup[]>(`${this.apiUrl}/my-groups`, {
      headers: this.getHeaders()
    });
  }

  // Get specific group details
  getGroup(groupId: number): Observable<UserGroup> {
    return this.http.get<UserGroup>(`${this.apiUrl}/${groupId}`, {
      headers: this.getHeaders()
    });
  }

  // Get group messages
  getGroupMessages(groupId: number, page: number = 1, pageSize: number = 50): Observable<GroupMessage[]> {
    return this.http.get<GroupMessage[]>(`${this.apiUrl}/${groupId}/messages?page=${page}&pageSize=${pageSize}`, {
      headers: this.getHeaders()
    });
  }

  // Send message to group
  sendGroupMessage(groupId: number, content: string): Observable<GroupMessage> {
    const messageData: CreateGroupMessage = {
      groupId,
      content
    };
    
    return this.http.post<GroupMessage>(`${this.apiUrl}/${groupId}/messages`, messageData, {
      headers: this.getHeaders()
    });
  }
}
