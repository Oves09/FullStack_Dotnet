import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Message, CreateMessageRequest, Conversation } from '../models/message.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private readonly API_URL = `${environment.apiUrl}/api/messages`;

  constructor(private http: HttpClient) {}

  getConversations(): Observable<Conversation[]> {
    return this.http.get<Conversation[]>(`${this.API_URL}/conversations`);
  }

  getConversation(otherUserId: string, page: number = 1, pageSize: number = 50): Observable<Message[]> {
    return this.http.get<Message[]>(`${this.API_URL}/conversation/${otherUserId}?page=${page}&pageSize=${pageSize}`);
  }

  sendMessage(request: CreateMessageRequest): Observable<Message> {
    return this.http.post<Message>(this.API_URL, request);
  }

  getMessage(id: number): Observable<Message> {
    return this.http.get<Message>(`${this.API_URL}/${id}`);
  }

  deleteMessage(id: number): Observable<any> {
    return this.http.delete(`${this.API_URL}/${id}`);
  }
}
