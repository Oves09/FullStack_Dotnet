import { Component, OnInit, OnDestroy, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserGroupsService, UserGroup, GroupMessage } from '../../../services/user-groups.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-group-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container-fluid mt-3">
      <div class="row justify-content-center">
        <div class="col-lg-8 col-md-10">
          <div class="card shadow-sm h-100">
            <!-- Header -->
            <div class="card-header bg-info text-white d-flex justify-content-between align-items-center">
              <div>
                <h5 class="mb-0">
                  <i class="fas fa-comments me-2"></i>{{ group?.groupName || 'Group Chat' }}
                </h5>
                <small *ngIf="group">{{ group.memberCount }} members</small>
              </div>
              <button class="btn btn-warning btn-sm" (click)="goBack()">
                <i class="fas fa-arrow-left me-1"></i>Back
              </button>
            </div>

            <!-- Group Info -->
            <div class="card-body p-2 bg-light border-bottom" *ngIf="group">
              <small class="text-muted">
                <i class="fas fa-info-circle me-1"></i>
                {{ group.description || 'No description available' }}
              </small>
            </div>

            <!-- Messages Area -->
            <div class="card-body p-0 position-relative" style="height: 60vh;">
              
              <!-- Loading Messages -->
              <div class="d-flex justify-content-center align-items-center h-100" *ngIf="isLoadingMessages">
                <div class="text-center">
                  <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Loading messages...</span>
                  </div>
                  <p class="mt-2 text-muted">Loading messages...</p>
                </div>
              </div>

              <!-- Messages List -->
              <div class="messages-container p-3" #messagesContainer *ngIf="!isLoadingMessages" style="height: 100%; overflow-y: auto;">
                
                <!-- No Messages -->
                <div class="text-center py-5" *ngIf="messages.length === 0">
                  <i class="fas fa-comment-slash fa-3x text-muted mb-3"></i>
                  <h6 class="text-muted">No messages yet</h6>
                  <p class="text-muted">Be the first to start the conversation!</p>
                </div>

                <!-- Message List -->
                <div *ngFor="let message of messages; let i = index" class="mb-3">
                  <div class="d-flex" [class.justify-content-end]="isMyMessage(message)">
                    <div class="message-bubble" 
                         [class.my-message]="isMyMessage(message)"
                         [class.other-message]="!isMyMessage(message)"
                         style="max-width: 70%;">
                      
                      <!-- Message Header -->
                      <div class="message-header mb-1" *ngIf="!isMyMessage(message)">
                        <small class="fw-bold text-primary">
                          {{ message.userFullName || message.userName }}
                        </small>
                      </div>

                      <!-- Message Content -->
                      <div class="message-content">
                        {{ message.content }}
                      </div>

                      <!-- Message Time -->
                      <div class="message-time mt-1">
                        <small class="text-muted">
                          {{ formatMessageTime(message.createdAt) }}
                        </small>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Message Input -->
            <div class="card-footer bg-white border-top">
              <form (ngSubmit)="sendMessage()" class="d-flex gap-2">
                <div class="flex-grow-1">
                  <input
                    type="text"
                    class="form-control"
                    [(ngModel)]="newMessage"
                    name="newMessage"
                    placeholder="Type your message..."
                    [disabled]="isSending"
                    maxlength="1000"
                    (keydown.enter)="sendMessage()"
                    autocomplete="off">
                </div>
                <button 
                  type="submit" 
                  class="btn btn-primary"
                  [disabled]="!newMessage.trim() || isSending">
                  <span *ngIf="isSending" class="spinner-border spinner-border-sm me-1"></span>
                  <i *ngIf="!isSending" class="fas fa-paper-plane"></i>
                </button>
              </form>
              <div class="mt-1">
                <small class="text-muted">{{ newMessage.length }}/1000 characters</small>
              </div>
            </div>

            <!-- Error Message -->
            <div class="alert alert-danger m-3 mb-0" *ngIf="errorMessage">
              <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
              <button type="button" class="btn-close float-end" (click)="errorMessage = ''"></button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .messages-container {
      background: #f8f9fa;
    }

    .message-bubble {
      padding: 0.75rem;
      border-radius: 1rem;
      word-wrap: break-word;
    }

    .my-message {
      background: #007bff;
      color: white;
      border-bottom-right-radius: 0.25rem;
    }

    .other-message {
      background: white;
      border: 1px solid #dee2e6;
      border-bottom-left-radius: 0.25rem;
    }

    .message-header {
      font-size: 0.8rem;
    }

    .message-content {
      line-height: 1.4;
    }

    .message-time {
      font-size: 0.75rem;
      opacity: 0.8;
    }

    .my-message .message-time {
      color: rgba(255, 255, 255, 0.8);
    }

    .card-body {
      padding: 0;
    }

    @media (max-width: 768px) {
      .container-fluid {
        padding: 0 0.5rem;
      }
      
      .message-bubble {
        max-width: 85% !important;
      }
    }
  `]
})
export class GroupChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('messagesContainer') messagesContainer!: ElementRef;

  groupId!: number;
  group: UserGroup | null = null;
  messages: GroupMessage[] = [];
  newMessage = '';
  currentUserId = '';

  isLoadingGroup = false;
  isLoadingMessages = false;
  isSending = false;
  errorMessage = '';

  private shouldScrollToBottom = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userGroupsService: UserGroupsService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.currentUserId = this.authService.getCurrentUserId() || '';
    
    this.route.params.subscribe(params => {
      this.groupId = +params['id'];
      if (this.groupId) {
        this.loadGroup();
        this.loadMessages();
      }
    });
  }

  ngOnDestroy() {
    // Clean up any subscriptions if needed
  }

  ngAfterViewChecked() {
    if (this.shouldScrollToBottom) {
      this.scrollToBottom();
      this.shouldScrollToBottom = false;
    }
  }

  loadGroup() {
    this.isLoadingGroup = true;
    this.errorMessage = '';

    this.userGroupsService.getGroup(this.groupId).subscribe({
      next: (group) => {
        this.group = group;
        this.isLoadingGroup = false;
      },
      error: (error) => {
        this.isLoadingGroup = false;
        if (error.status === 404) {
          this.errorMessage = 'Group not found or you are not a member of this group.';
        } else {
          this.errorMessage = 'Failed to load group information.';
        }
        console.error('Error loading group:', error);
      }
    });
  }

  loadMessages() {
    this.isLoadingMessages = true;
    this.errorMessage = '';

    this.userGroupsService.getGroupMessages(this.groupId).subscribe({
      next: (messages) => {
        this.messages = messages.reverse(); // Reverse to show oldest first
        this.isLoadingMessages = false;
        this.shouldScrollToBottom = true;
      },
      error: (error) => {
        this.isLoadingMessages = false;
        if (error.status === 403) {
          this.errorMessage = 'You are not authorized to view messages in this group.';
        } else {
          this.errorMessage = 'Failed to load messages.';
        }
        console.error('Error loading messages:', error);
      }
    });
  }

  sendMessage() {
    if (!this.newMessage.trim() || this.isSending) {
      return;
    }

    this.isSending = true;
    this.errorMessage = '';

    this.userGroupsService.sendGroupMessage(this.groupId, this.newMessage.trim()).subscribe({
      next: (message) => {
        this.messages.push(message);
        this.newMessage = '';
        this.isSending = false;
        this.shouldScrollToBottom = true;
      },
      error: (error) => {
        this.isSending = false;
        if (error.status === 403) {
          this.errorMessage = 'You are not authorized to send messages to this group.';
        } else {
          this.errorMessage = 'Failed to send message. Please try again.';
        }
        console.error('Error sending message:', error);
      }
    });
  }

  isMyMessage(message: GroupMessage): boolean {
    return message.userId === this.currentUserId;
  }

  formatMessageTime(date: Date): string {
    const messageDate = new Date(date);
    const now = new Date();
    const diffInHours = (now.getTime() - messageDate.getTime()) / (1000 * 60 * 60);

    if (diffInHours < 24) {
      return messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    } else {
      return messageDate.toLocaleDateString() + ' ' + messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    }
  }

  private scrollToBottom() {
    try {
      if (this.messagesContainer) {
        const element = this.messagesContainer.nativeElement;
        element.scrollTop = element.scrollHeight;
      }
    } catch (err) {
      console.error('Error scrolling to bottom:', err);
    }
  }

  goBack() {
    this.router.navigate(['/my-groups']);
  }
}
