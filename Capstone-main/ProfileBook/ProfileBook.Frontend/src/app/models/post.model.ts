export interface Post {
  postId: number;
  userId: string;
  userName: string;
  userFullName: string;
  userProfileImage?: string;
  content: string;
  postImagePath?: string;
  status: PostStatus;
  createdAt: Date;
  updatedAt: Date;
  adminComments?: string;
  approvedByUserName?: string;
  approvedAt?: Date;
  likesCount: number;
  commentsCount: number;
  isLikedByCurrentUser: boolean;
  // UI-only properties
  showComments?: boolean;
  comments?: PostComment[];
  newComment?: string;
}

export interface CreatePostRequest {
  content: string;
  postImage?: File;
}

export interface ApprovePostRequest {
  postId: number;
  isApproved: boolean;
  adminComments?: string;
}

export enum PostStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2
}

export interface PostComment {
  commentId: number;
  userId: string;
  userName: string;
  userFullName: string;
  userProfileImage?: string;
  postId: number;
  content: string;
  createdAt: Date;
  updatedAt: Date;
  isDeleted: boolean;
}

export interface CreateCommentRequest {
  postId: number;
  content: string;
}
