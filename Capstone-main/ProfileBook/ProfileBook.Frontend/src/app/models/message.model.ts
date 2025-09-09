export interface Message {
  messageId: number;
  senderId: string;
  senderName: string;
  receiverId: string;
  receiverName: string;
  messageContent: string;
  timeStamp: Date;
  isRead: boolean;
}

export interface CreateMessageRequest {
  receiverId: string;
  messageContent: string;
}

export interface Conversation {
  userId: string;
  userName: string;
  userFullName: string;
  userProfileImage?: string;
  lastMessage?: Message;
  unreadCount: number;
}
