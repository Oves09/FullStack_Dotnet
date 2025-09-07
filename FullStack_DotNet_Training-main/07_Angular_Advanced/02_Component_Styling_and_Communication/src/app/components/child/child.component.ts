import { NgFor } from '@angular/common';
import { Component, Input, Output, EventEmitter, OnInit, OnChanges, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-child',
  standalone: true,
  imports: [NgFor],
  templateUrl: './child.component.html',
  styleUrls: ['./child.component.css'],
  styles: [`
    h3 { color: green; } /* Inline scoped styles */
  `]
})
export class ChildComponent implements OnInit, OnChanges, OnDestroy {
  // Receive data from parent
  @Input() childInput: string = "";

  // Send data back to parent
  @Output() childOutput = new EventEmitter<string>();

  // Lifecycle variables
  log: string[] = [];

  ngOnInit() {
    this.log.push("Child: ngOnInit called ✅");
  }

  ngOnChanges() {
    this.log.push("Child: ngOnChanges detected input change 🔄");
  }

  sendMessage() {
    this.childOutput.emit("Hello Parent! Message from Child 👶");
  }

  ngOnDestroy() {
    this.log.push("Child: ngOnDestroy cleanup 🧹");
  }
}
