export interface User {
  id: string;
  userName: string;
  email: string;
  firstName?: string;
  lastName?: string;
  fullName?: string;
  bio?: string;
  profileImagePath?: string;
  createdAt: Date;
  roles?: string[];
  lockoutEnd?: Date | null;
  isActive?: boolean;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  password: string;
  confirmPassword: string;
  bio?: string;
}

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  expiration: Date;
  user: User;
}

export interface UpdateProfileRequest {
  firstName: string;
  lastName: string;
  bio?: string;
  profileImage?: File;
}
