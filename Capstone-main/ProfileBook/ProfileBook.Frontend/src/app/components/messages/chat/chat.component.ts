import { Component, OnInit, OnDestroy, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, interval } from 'rxjs';
import { MessageService } from '../../../services/message.service';
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { Message, CreateMessageRequest } from '../../../models/message.model';
import { User } from '../../../models/user.model';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="container-fluid h-100">
      <div class="row h-100">
        <div class="col-12">
          <div class="chat-container d-flex flex-column h-100">
            <!-- Chat Header -->
            <div class="chat-header bg-white border-bottom p-3 shadow-sm">
              <div class="d-flex align-items-center">
                <button class="btn btn-outline-secondary me-3" (click)="goBack()">
                  <i class="fas fa-arrow-left"></i>
                </button>
                
                <img [src]="getUserAvatar()" class="rounded-circle me-3" 
                     width="48" height="48" alt="User avatar">
                
                <div class="flex-grow-1" *ngIf="otherUser">
                  <h5 class="mb-0">{{ otherUser.firstName }} {{ otherUser.lastName }}</h5>
                  <small class="text-muted">{{ '@' + otherUser.userName }}</small>
                </div>
                
                <div class="spinner-border spinner-border-sm text-primary" *ngIf="isLoadingUser">
                </div>
              </div>
            </div>

            <!-- Messages Area -->
            <div class="messages-container flex-grow-1 p-3" #messagesContainer>
              <!-- Loading Messages -->
              <div class="text-center py-4" *ngIf="isLoadingMessages">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Loading messages...</span>
                </div>
                <p class="mt-2 text-muted">Loading conversation...</p>
              </div>

              <!-- No Messages -->
              <div class="text-center py-5" *ngIf="!isLoadingMessages && messages.length === 0">
                <i class="fas fa-comments fa-3x text-muted mb-3"></i>
                <h5 class="text-muted">No messages yet</h5>
                <p class="text-muted">Start the conversation by sending a message</p>
              </div>

              <!-- Messages List -->
              <div class="messages-list" *ngIf="!isLoadingMessages && messages.length > 0">
                <div *ngFor="let message of messages" 
                     class="message mb-3"
                     [class.sent]="message.senderId === currentUserId"
                     [class.received]="message.senderId !== currentUserId">
                  
                  <div class="message-bubble">
                    <p class="mb-1">{{ message.messageContent }}</p>
                    <small class="message-time">
                      {{ formatMessageTime(message.timeStamp) }}
                      <i *ngIf="message.senderId === currentUserId && message.isRead" 
                         class="fas fa-check-double ms-1 text-primary"></i>
                      <i *ngIf="message.senderId === currentUserId && !message.isRead" 
                         class="fas fa-check ms-1"></i>
                    </small>
                  </div>
                </div>
              </div>

              <!-- Load More Messages -->
              <div class="text-center mb-3" *ngIf="hasMoreMessages && !isLoadingMessages">
                <button class="btn btn-outline-primary btn-sm" (click)="loadMoreMessages()" 
                        [disabled]="isLoadingMore">
                  <span *ngIf="isLoadingMore" class="spinner-border spinner-border-sm me-2"></span>
                  {{ isLoadingMore ? 'Loading...' : 'Load Earlier Messages' }}
                </button>
              </div>
            </div>

            <!-- Message Input -->
            <div class="message-input bg-white border-top p-3">
              <form [formGroup]="messageForm" (ngSubmit)="sendMessage()" class="d-flex gap-2">
                <div class="flex-grow-1">
                  <input
                    type="text"
                    class="form-control"
                    formControlName="messageContent"
                    placeholder="Type your message..."
                    [class.is-invalid]="isFieldInvalid('messageContent')"
                    (keypress)="onKeyPress($event)"
                  >
                  <div class="invalid-feedback" *ngIf="isFieldInvalid('messageContent')">
                    Message cannot be empty or exceed 1000 characters
                  </div>
                </div>
                
                <button type="submit" class="btn btn-primary" 
                        [disabled]="messageForm.invalid || isSending">
                  <span *ngIf="isSending" class="spinner-border spinner-border-sm"></span>
                  <i *ngIf="!isSending" class="fas fa-paper-plane"></i>
                </button>
              </form>
            </div>

            <!-- Error Message -->
            <div class="alert alert-danger m-3" *ngIf="errorMessage">
              <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
              <button type="button" class="btn-close" (click)="errorMessage = ''"></button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .chat-container {
      height: calc(100vh - 80px);
      max-height: calc(100vh - 80px);
    }

    .chat-header {
      flex-shrink: 0;
      z-index: 10;
    }

    .messages-container {
      overflow-y: auto;
      background-color: #f8f9fa;
      flex-grow: 1;
      display: flex;
      flex-direction: column;
    }

    .messages-list {
      flex-grow: 1;
      display: flex;
      flex-direction: column;
      justify-content: flex-end;
    }

    .message {
      max-width: 70%;
      word-wrap: break-word;
    }

    .message.sent {
      align-self: flex-end;
      text-align: right;
    }

    .message.received {
      align-self: flex-start;
      text-align: left;
    }

    .message-bubble {
      display: inline-block;
      padding: 0.75rem 1rem;
      border-radius: 1.25rem;
      word-wrap: break-word;
      max-width: 100%;
    }

    .message.sent .message-bubble {
      background-color: var(--primary-color);
      color: white;
      border-bottom-right-radius: 0.25rem;
    }

    .message.received .message-bubble {
      background-color: white;
      color: var(--text-primary);
      border: 1px solid var(--border-color);
      border-bottom-left-radius: 0.25rem;
    }

    .message-time {
      opacity: 0.8;
      font-size: 0.75rem;
    }

    .message.sent .message-time {
      color: rgba(255, 255, 255, 0.8);
    }

    .message.received .message-time {
      color: var(--text-secondary);
    }

    .message-input {
      flex-shrink: 0;
      box-shadow: 0 -2px 4px rgba(0,0,0,0.1);
    }

    .form-control:focus {
      border-color: var(--primary-color);
      box-shadow: 0 0 0 0.2rem rgba(59, 130, 246, 0.25);
    }

    @media (max-width: 768px) {
      .chat-container {
        height: calc(100vh - 70px);
      }
      
      .message {
        max-width: 85%;
      }
      
      .chat-header .btn {
        padding: 0.375rem 0.5rem;
      }
      
      .rounded-circle {
        width: 40px !important;
        height: 40px !important;
      }
    }

    /* Custom scrollbar */
    .messages-container::-webkit-scrollbar {
      width: 6px;
    }

    .messages-container::-webkit-scrollbar-track {
      background: #f1f1f1;
    }

    .messages-container::-webkit-scrollbar-thumb {
      background: #c1c1c1;
      border-radius: 3px;
    }

    .messages-container::-webkit-scrollbar-thumb:hover {
      background: #a8a8a8;
    }
  `]
})
export class ChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('messagesContainer') messagesContainer!: ElementRef;

  messageForm: FormGroup;
  messages: Message[] = [];
  otherUser: User | null = null;
  currentUserId: string | null = null;
  otherUserId: string = '';
  isLoadingMessages = false;
  isLoadingUser = false;
  isLoadingMore = false;
  isSending = false;
  hasMoreMessages = false;
  currentPage = 1;
  pageSize = 50;
  errorMessage = '';
  
  private refreshSubscription?: Subscription;
  private shouldScrollToBottom = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private messageService: MessageService,
    private userService: UserService,
    private authService: AuthService
  ) {
    this.messageForm = this.fb.group({
      messageContent: ['', [Validators.required, Validators.maxLength(1000)]]
    });
  }

  ngOnInit() {
    this.currentUserId = this.authService.getCurrentUser()?.id || null;
    
    this.route.params.subscribe(params => {
      this.otherUserId = params['userId'];
      if (this.otherUserId) {
        this.loadUser();
        this.loadMessages();
        this.startMessageRefresh();
      }
    });
  }

  ngOnDestroy() {
    if (this.refreshSubscription) {
      this.refreshSubscription.unsubscribe();
    }
  }

  ngAfterViewChecked() {
    if (this.shouldScrollToBottom) {
      this.scrollToBottom();
      this.shouldScrollToBottom = false;
    }
  }

  loadUser() {
    this.isLoadingUser = true;
    this.userService.getUser(this.otherUserId).subscribe({
      next: (user) => {
        this.otherUser = user;
        this.isLoadingUser = false;
      },
      error: (error) => {
        this.isLoadingUser = false;
        this.errorMessage = 'Failed to load user information.';
        console.error('Error loading user:', error);
      }
    });
  }

  loadMessages() {
    this.isLoadingMessages = true;
    this.messageService.getConversation(this.otherUserId, 1, this.pageSize).subscribe({
      next: (messages) => {
        this.messages = messages.reverse(); // Reverse to show oldest first
        this.isLoadingMessages = false;
        this.hasMoreMessages = messages.length === this.pageSize;
        this.currentPage = 1;
        this.shouldScrollToBottom = true;
      },
      error: (error) => {
        this.isLoadingMessages = false;
        this.errorMessage = 'Failed to load messages. Please try again.';
        console.error('Error loading messages:', error);
      }
    });
  }

  loadMoreMessages() {
    if (!this.isLoadingMore && this.hasMoreMessages) {
      this.isLoadingMore = true;
      const nextPage = this.currentPage + 1;

      this.messageService.getConversation(this.otherUserId, nextPage, this.pageSize).subscribe({
        next: (messages) => {
          const reversedMessages = messages.reverse();
          this.messages = [...reversedMessages, ...this.messages];
          this.isLoadingMore = false;
          this.hasMoreMessages = messages.length === this.pageSize;
          this.currentPage = nextPage;
        },
        error: (error) => {
          this.isLoadingMore = false;
          this.errorMessage = 'Failed to load more messages.';
          console.error('Error loading more messages:', error);
        }
      });
    }
  }

  sendMessage() {
    if (this.messageForm.valid) {
      this.isSending = true;
      this.errorMessage = '';

      const messageRequest: CreateMessageRequest = {
        receiverId: this.otherUserId,
        messageContent: this.messageForm.get('messageContent')?.value.trim()
      };

      this.messageService.sendMessage(messageRequest).subscribe({
        next: (message) => {
          this.messages.push(message);
          this.messageForm.reset();
          this.isSending = false;
          this.shouldScrollToBottom = true;
        },
        error: (error) => {
          this.isSending = false;
          this.errorMessage = error.error?.message || 'Failed to send message. Please try again.';
          console.error('Error sending message:', error);
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  onKeyPress(event: KeyboardEvent) {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.sendMessage();
    }
  }

  goBack() {
    this.router.navigate(['/messages']);
  }

  getUserAvatar(): string {
    if (this.otherUser?.profileImagePath) {
      return this.otherUser.profileImagePath;
    }
    return 'https://via.placeholder.com/48x48/3b82f6/ffffff?text=' + 
           (this.otherUser?.firstName?.charAt(0) || this.otherUser?.userName?.charAt(0) || 'U');
  }

  formatMessageTime(timestamp: Date): string {
    const messageDate = new Date(timestamp);
    const now = new Date();
    
    // If today, show time
    if (messageDate.toDateString() === now.toDateString()) {
      return messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    }
    
    // If this week, show day and time
    const daysDiff = Math.floor((now.getTime() - messageDate.getTime()) / (1000 * 60 * 60 * 24));
    if (daysDiff < 7) {
      return messageDate.toLocaleDateString([], { weekday: 'short' }) + ' ' +
             messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    }
    
    // Otherwise show date and time
    return messageDate.toLocaleDateString() + ' ' +
           messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }

  private startMessageRefresh() {
    // Refresh messages every 30 seconds to get new messages
    this.refreshSubscription = interval(30000).subscribe(() => {
      if (!this.isLoadingMessages && !this.isSending) {
        this.refreshMessages();
      }
    });
  }

  private refreshMessages() {
    this.messageService.getConversation(this.otherUserId, 1, this.pageSize).subscribe({
      next: (messages) => {
        const reversedMessages = messages.reverse();
        const newMessagesCount = reversedMessages.length - this.messages.length;
        
        if (newMessagesCount > 0) {
          this.messages = reversedMessages;
          this.shouldScrollToBottom = true;
        }
      },
      error: (error) => {
        console.error('Error refreshing messages:', error);
      }
    });
  }

  private scrollToBottom() {
    try {
      const container = this.messagesContainer.nativeElement;
      container.scrollTop = container.scrollHeight;
    } catch (err) {
      console.error('Error scrolling to bottom:', err);
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.messageForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  private markFormGroupTouched() {
    Object.keys(this.messageForm.controls).forEach(key => {
      const control = this.messageForm.get(key);
      control?.markAsTouched();
    });
  }
}
