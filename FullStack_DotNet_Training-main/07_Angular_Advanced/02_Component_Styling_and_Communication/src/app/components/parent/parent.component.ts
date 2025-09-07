import { Component } from '@angular/core';
import { ChildComponent } from "../child/child.component";
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-parent',
  imports: [ChildComponent, NgIf],
  templateUrl: './parent.component.html',
  styleUrl: './parent.component.css'
})
export class ParentComponent {
  parentMessage: string = "Hello from parent.";
  receivedMessage: string = "";

  //Event listner for child component
  onMessageFromChild(message: string) {
    this.receivedMessage = message;
  }

}
