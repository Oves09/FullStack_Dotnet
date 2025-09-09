import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post, CreatePostRequest, ApprovePostRequest, PostComment, CreateCommentRequest } from '../models/post.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  private readonly API_URL = `${environment.apiUrl}/api/posts`;

  constructor(private http: HttpClient) {}

  getPosts(page: number = 1, pageSize: number = 10): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.API_URL}?page=${page}&pageSize=${pageSize}`);
  }

  getPost(id: number): Observable<Post> {
    return this.http.get<Post>(`${this.API_URL}/${id}`);
  }

  createPost(request: CreatePostRequest): Observable<Post> {
    const formData = new FormData();
    formData.append('content', request.content);
    if (request.postImage) {
      formData.append('postImage', request.postImage);
    }
    return this.http.post<Post>(this.API_URL, formData);
  }

  likePost(postId: number): Observable<any> {
    return this.http.post(`${this.API_URL}/${postId}/like`, {});
  }

  getMyPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.API_URL}/my-posts`);
  }

  getComments(postId: number): Observable<PostComment[]> {
    return this.http.get<PostComment[]>(`${this.API_URL}/${postId}/comments`);
  }

  addComment(postId: number, content: string): Observable<PostComment> {
    const request: CreateCommentRequest = { postId, content };
    return this.http.post<PostComment>(`${this.API_URL}/${postId}/comments`, request);
  }

  deletePost(postId: number): Observable<any> {
    return this.http.delete(`${this.API_URL}/${postId}`);
  }
}
