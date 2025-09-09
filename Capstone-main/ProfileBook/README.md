# ProfileBook - Social Media Platform

ProfileBook is a comprehensive social media platform built with Angular frontend and ASP.NET Core MVC backend.

## Tech Stack

- **Frontend**: Angular with TypeScript
- **Backend**: ASP.NET Core MVC with Web API
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Token-based authentication
- **File Storage**: Server-side file storage for images
- **API Documentation**: Swagger/OpenAPI
- **Real-time Communication**: SignalR (optional)

## Features

### User Features
- User registration and authentication
- View and interact with posts (like/comment)
- Create posts (subject to admin approval)
- Message other users
- Search for users
- Report inappropriate behavior

### Admin Features
- User management (CRUD operations)
- Post approval system
- View and manage reported users
- Create and manage user groups

## Project Structure

```
ProfileBook/
├── ProfileBook.API/          # ASP.NET Core Web API Backend
│   ├── Controllers/          # API Controllers
│   ├── Models/              # Data Models
│   ├── Data/                # Entity Framework DbContext
│   ├── Services/            # Business Logic Services
│   └── wwwroot/             # Static files and uploads
├── ProfileBook.Frontend/     # Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/  # Angular Components
│   │   │   ├── services/    # Angular Services
│   │   │   └── models/      # TypeScript Models
│   │   └── assets/          # Static assets
└── README.md
```

## Getting Started

### Prerequisites
- .NET 9.0 or later
- Node.js (v18 or later) and npm
- SQL Server or SQL Server Express LocalDB
- Angular CLI (`npm install -g @angular/cli`)

### Backend Setup (ProfileBook.API)

1. **Navigate to the API directory:**
   ```bash
   cd ProfileBook.API
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Update database connection string:**
   - Open `appsettings.json`
   - Modify the `DefaultConnection` string if needed (default uses LocalDB)

4. **Install Entity Framework tools (if not already installed):**
   ```bash
   dotnet tool install --global dotnet-ef
   ```

5. **Create and apply database migrations:**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

6. **Run the API:**
   ```bash
   dotnet run
   ```
   - API will be available at `https://localhost:7001` and `http://localhost:5001`
   - Swagger documentation at `https://localhost:7001/swagger`

### Frontend Setup (ProfileBook.Frontend)

1. **Navigate to the frontend directory:**
   ```bash
   cd ProfileBook.Frontend
   ```

2. **Install npm dependencies:**
   ```bash
   npm install
   ```

3. **Install Angular CLI globally (if not already installed):**
   ```bash
   npm install -g @angular/cli
   ```

4. **Start the development server:**
   ```bash
   ng serve
   ```
   - Application will be available at `http://localhost:4200`

### Default Admin Account

The system automatically creates a default admin account:
- **Email:** admin@profilebook.com
- **Password:** Admin123!
- **Username:** admin

### Environment Configuration

#### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProfileBookDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "Key": "JWT-KEY",
    "Issuer": "ProfileBook",
    "Audience": "ProfileBook",
    "ExpiryInMinutes": 60
  }
}
```

#### Frontend (src/environments/environment.ts)
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7001'
};
```

## Development Sprints

### Sprint I (Foundation)
- Database schema design
- Backend project setup
- Static frontend templates
- Use case documentation

### Sprint II (Core Features)
- User authentication with JWT
- CRUD operations for users
- Post management system
- Messaging API
- Angular components and routing

### Sprint III (Advanced Features)
- Search and filter functionality
- Admin post approval system
- User reporting system
- Groups management
- Swagger documentation
- Final integration and testing

## API Endpoints

### Authentication (`/api/auth`)
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout

### Users (`/api/users`)
- `GET /api/users/search` - Search users
- `GET /api/users/{id}` - Get user by ID
- `GET /api/users/profile` - Get current user profile
- `PUT /api/users/profile` - Update user profile

### Posts (`/api/posts`)
- `GET /api/posts` - Get approved posts
- `POST /api/posts` - Create new post
- `GET /api/posts/my-posts` - Get current user's posts
- `POST /api/posts/{id}/like` - Toggle like on post

### Messages (`/api/messages`)
- `GET /api/messages/conversations` - Get user conversations
- `GET /api/messages/conversation/{userId}` - Get messages with specific user
- `POST /api/messages` - Send message
- `DELETE /api/messages/{id}` - Delete message

### Reports (`/api/reports`)
- `POST /api/reports` - Create user report
- `GET /api/reports/my-reports` - Get current user's reports

### Admin (`/api/admin`)
- `GET /api/admin/pending-posts` - Get posts pending approval
- `POST /api/admin/approve-post` - Approve/reject post
- `GET /api/admin/users` - Get all users
- `PUT /api/admin/users/{id}/status` - Update user status
- `DELETE /api/admin/users/{id}` - Delete user
- `GET /api/admin/reports` - Get all reports
- `POST /api/admin/review-report` - Review report
- `POST /api/admin/create-group` - Create user group

## Database Schema

### Core Entities

#### Users (AspNetUsers + Custom Fields)
- `Id` (string, PK)
- `UserName` (string)
- `Email` (string)
- `FirstName` (string)
- `LastName` (string)
- `Bio` (string, optional)
- `ProfileImagePath` (string, optional)
- `CreatedAt` (DateTime)

#### Posts
- `PostId` (int, PK)
- `UserId` (string, FK)
- `Content` (string)
- `PostImagePath` (string, optional)
- `Status` (enum: Pending, Approved, Rejected)
- `CreatedAt` (DateTime)
- `ApprovedAt` (DateTime, optional)
- `ApprovedByUserId` (string, optional)
- `AdminComments` (string, optional)

#### Messages
- `MessageId` (int, PK)
- `SenderId` (string, FK)
- `ReceiverId` (string, FK)
- `Content` (string)
- `SentAt` (DateTime)
- `IsRead` (bool)

#### Reports
- `ReportId` (int, PK)
- `ReportedUserId` (string, FK)
- `ReportingUserId` (string, FK)
- `Reason` (string)
- `Description` (string, optional)
- `Status` (enum: Pending, UnderReview, Resolved, Dismissed)
- `TimeStamp` (DateTime)
- `ReviewedAt` (DateTime, optional)
- `ReviewedByUserId` (string, optional)
- `AdminNotes` (string, optional)

#### Likes
- `LikeId` (int, PK)
- `UserId` (string, FK)
- `PostId` (int, FK)
- `CreatedAt` (DateTime)

#### Comments
- `CommentId` (int, PK)
- `UserId` (string, FK)
- `PostId` (int, FK)
- `Content` (string)
- `CreatedAt` (DateTime)

#### Groups
- `GroupId` (int, PK)
- `GroupName` (string)
- `Description` (string, optional)
- `CreatedByUserId` (string, FK)
- `CreatedAt` (DateTime)

#### UserGroups
- `UserGroupId` (int, PK)
- `UserId` (string, FK)
- `GroupId` (int, FK)
- `JoinedAt` (DateTime)
- `IsActive` (bool)

## Application Features

### User Authentication & Authorization
- JWT token-based authentication
- Role-based authorization (User, Admin)
- Password policies and validation
- Secure logout with token invalidation

### Post Management
- Create posts with optional images
- Admin approval workflow
- Like and comment functionality
- Post status tracking (Pending, Approved, Rejected)

### Messaging System
- Direct messaging between users
- Conversation threading
- Message read status

### User Reporting
- Report inappropriate user behavior
- Admin review and resolution workflow
- Report status tracking
- Admin notes and actions

### Search & Discovery
- User search with filters
- Debounced search input
- Pagination support
- Profile viewing

### Admin Dashboard
- User management (view, suspend, delete)
- Post approval interface
- Report management system
- Statistics and overview

### File Upload
- Profile picture uploads
- Post image uploads
- Server-side file storage
- Image validation and processing

## Security Features

- JWT token authentication
- Password hashing with Identity
- Role-based access control
- Input validation and sanitization
- CORS configuration
- SQL injection prevention (EF Core)
- XSS protection

## Development Notes

### Frontend Architecture
- Angular standalone components
- Lazy-loaded routes
- HTTP interceptors for authentication
- Reactive forms with validation
- Bootstrap 5 styling
- Responsive design

### Backend Architecture
- Clean architecture principles
- Repository pattern with EF Core
- Dependency injection
- Swagger API documentation
- Structured logging
- Error handling middleware

### Database Design
- Entity Framework Core Code First
- Proper foreign key relationships
- Indexed columns for performance
- Data annotations for validation
- Seed data for admin user
