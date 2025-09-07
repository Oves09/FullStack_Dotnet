import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserService } from './user/services/user.service';


@NgModule({
  declarations: [ ],
  imports: [
    CommonModule,
    FormsModule
  ],
  providers: [UserService],
  exports: []
})
export class UserModule { }
