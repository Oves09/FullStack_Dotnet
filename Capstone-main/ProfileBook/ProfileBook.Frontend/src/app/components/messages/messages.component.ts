import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MessageService } from '../../services/message.service';
import { Conversation } from '../../models/message.model';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-8">
          <div class="card shadow-sm">
            <div class="card-header bg-primary text-white">
              <h4 class="mb-0">
                <i class="fas fa-envelope me-2"></i>Messages
              </h4>
            </div>
            <div class="card-body p-0">
              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Loading conversations...</span>
                </div>
                <p class="mt-2 text-muted">Loading your conversations...</p>
              </div>

              <!-- No Conversations -->
              <div class="text-center py-5" *ngIf="!isLoading && conversations.length === 0">
                <i class="fas fa-comments fa-4x text-muted mb-3"></i>
                <h5 class="text-muted">No conversations yet</h5>
                <p class="text-muted">Start a conversation by searching for users and sending them a message</p>
                <a routerLink="/search" class="btn btn-primary">
                  <i class="fas fa-search me-2"></i>Find People
                </a>
              </div>

              <!-- Conversations List -->
              <div class="list-group list-group-flush" *ngIf="!isLoading && conversations.length > 0">
                <a
                  *ngFor="let conversation of conversations"
                  [routerLink]="['/messages', conversation.userId]"
                  class="list-group-item list-group-item-action conversation-item"
                  [class.unread]="conversation.unreadCount > 0"
                >
                  <div class="d-flex align-items-center">
                    <div class="position-relative">
                      <img [src]="getUserAvatar(conversation)" 
                           class="rounded-circle me-3" width="56" height="56" alt="User avatar">
                      <span *ngIf="conversation.unreadCount > 0" 
                            class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                        {{ conversation.unreadCount > 99 ? '99+' : conversation.unreadCount }}
                      </span>
                    </div>
                    
                    <div class="flex-grow-1 min-w-0">
                      <div class="d-flex justify-content-between align-items-start">
                        <h6 class="mb-1 text-truncate">{{ conversation.userFullName }}</h6>
                        <small class="text-muted" *ngIf="conversation.lastMessage">
                          {{ formatMessageTime(conversation.lastMessage.timeStamp) }}
                        </small>
                      </div>
                      
                      <p class="mb-1 text-muted small">{{ '@' + conversation.userName }}</p>
                      
                      <div *ngIf="conversation.lastMessage" class="d-flex align-items-center">
                        <span class="text-truncate" 
                              [class.fw-bold]="conversation.unreadCount > 0"
                              [class.text-muted]="conversation.unreadCount === 0">
                          <span *ngIf="conversation.lastMessage.senderName === 'You'" class="me-1">You:</span>
                          {{ conversation.lastMessage.messageContent }}
                        </span>
                      </div>
                    </div>
                    
                    <div class="ms-2">
                      <i class="fas fa-chevron-right text-muted"></i>
                    </div>
                  </div>
                </a>
              </div>
            </div>
          </div>

          <!-- Error Message -->
          <div class="alert alert-danger mt-3" *ngIf="errorMessage">
            <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
            <button type="button" class="btn btn-sm btn-outline-danger ms-2" (click)="loadConversations()">
              <i class="fas fa-retry me-1"></i>Retry
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .conversation-item {
      padding: 1rem;
      border: none;
      border-bottom: 1px solid var(--border-color);
      transition: background-color 0.2s ease;
    }

    .conversation-item:hover {
      background-color: var(--light-bg);
    }

    .conversation-item.unread {
      background-color: rgba(59, 130, 246, 0.05);
      border-left: 4px solid var(--primary-color);
    }

    .conversation-item:last-child {
      border-bottom: none;
    }

    .min-w-0 {
      min-width: 0;
    }

    .text-truncate {
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }

    .badge {
      font-size: 0.7rem;
      min-width: 1.5rem;
      height: 1.5rem;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .conversation-item {
        padding: 0.75rem;
      }
      
      .rounded-circle {
        width: 48px !important;
        height: 48px !important;
      }
    }
  `]
})
export class MessagesComponent implements OnInit {
  conversations: Conversation[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(private messageService: MessageService) {}

  ngOnInit() {
    this.loadConversations();
  }

  loadConversations() {
    this.isLoading = true;
    this.errorMessage = '';

    this.messageService.getConversations().subscribe({
      next: (conversations) => {
        this.conversations = conversations;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load conversations. Please try again.';
        console.error('Error loading conversations:', error);
      }
    });
  }

  getUserAvatar(conversation: Conversation): string {
    if (conversation.userProfileImage) {
      return conversation.userProfileImage;
    }
    return 'https://via.placeholder.com/56x56/3b82f6/ffffff?text=' + 
           (conversation.userFullName?.charAt(0) || 'U');
  }

  formatMessageTime(timestamp: Date): string {
    const now = new Date();
    const messageDate = new Date(timestamp);
    const diffInMinutes = Math.floor((now.getTime() - messageDate.getTime()) / (1000 * 60));

    if (diffInMinutes < 1) return 'now';
    if (diffInMinutes < 60) return `${diffInMinutes}m`;
    if (diffInMinutes < 1440) return `${Math.floor(diffInMinutes / 60)}h`;
    if (diffInMinutes < 10080) return `${Math.floor(diffInMinutes / 1440)}d`;
    
    return messageDate.toLocaleDateString();
  }
}
