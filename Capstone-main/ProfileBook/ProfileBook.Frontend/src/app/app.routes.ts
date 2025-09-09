import { Routes } from '@angular/router';
import { AuthGuard, AdminGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/feed', pathMatch: 'full' },
  { 
    path: 'login', 
    loadComponent: () => import('./components/auth/login/login.component').then(m => m.LoginComponent) 
  },
  { 
    path: 'register', 
    loadComponent: () => import('./components/auth/register/register.component').then(m => m.RegisterComponent) 
  },
  { 
    path: 'feed', 
    loadComponent: () => import('./components/feed/feed.component').then(m => m.FeedComponent),
    canActivate: [AuthGuard]
  },
  { 
    path: 'profile', 
    loadComponent: () => import('./components/profile/profile.component').then(m => m.ProfileComponent),
    canActivate: [AuthGuard]
  },
  { 
    path: 'messages', 
    loadComponent: () => import('./components/messages/messages.component').then(m => m.MessagesComponent),
    canActivate: [AuthGuard]
  },
  { 
    path: 'messages/:userId', 
    loadComponent: () => import('./components/messages/chat/chat.component').then(m => m.ChatComponent),
    canActivate: [AuthGuard]
  },
  { 
    path: 'search', 
    loadComponent: () => import('./components/search/search.component').then(m => m.SearchComponent),
    canActivate: [AuthGuard]
  },
  { 
    path: 'my-posts', 
    loadComponent: () => import('./components/my-posts/my-posts.component').then(m => m.MyPostsComponent),
    canActivate: [AuthGuard]
  },
  { 
    path: 'reports', 
    loadComponent: () => import('./components/reports/reports.component').then(m => m.ReportsComponent),
    canActivate: [AuthGuard]
  },
  { 
    path: 'admin', 
    loadComponent: () => import('./components/admin/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent),
    canActivate: [AdminGuard]
  },
  { 
    path: 'admin/posts', 
    loadComponent: () => import('./components/admin/post-approval/post-approval.component').then(m => m.PostApprovalComponent),
    canActivate: [AdminGuard]
  },
  { 
    path: 'admin/users', 
    loadComponent: () => import('./components/admin/user-management/user-management.component').then(m => m.UserManagementComponent),
    canActivate: [AdminGuard]
  },
  { 
    path: 'admin/reports', 
    loadComponent: () => import('./components/admin/report-management/report-management.component').then(m => m.ReportManagementComponent),
    canActivate: [AdminGuard]
  },
  { 
    path: 'admin/groups', 
    loadComponent: () => import('./components/admin/group-management/group-management.component').then(m => m.GroupManagementComponent),
    canActivate: [AdminGuard]
  },
  { 
    path: 'notifications', 
    loadComponent: () => import('./components/notifications/notifications.component').then(m => m.NotificationsComponent),
    canActivate: [AuthGuard]
  },
  { 
    path: 'my-groups', 
    loadComponent: () => import('./components/user/my-groups/my-groups.component').then(m => m.MyGroupsComponent),
    canActivate: [AuthGuard]
  },
  { 
    path: 'group-chat/:id', 
    loadComponent: () => import('./components/user/group-chat/group-chat.component').then(m => m.GroupChatComponent),
    canActivate: [AuthGuard]
  },
  { path: '**', redirectTo: '/feed' }
];
