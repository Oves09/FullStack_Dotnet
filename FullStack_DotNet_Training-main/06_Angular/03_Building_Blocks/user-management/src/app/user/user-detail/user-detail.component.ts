import { Component, Input } from '@angular/core';
import { NgIf } from '@angular/common';
import { User } from '../services/user.service';

@Component({
  selector: 'app-user-detail',
  imports: [NgIf],
  standalone: true,
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent {
  @Input() user?: User;
}
