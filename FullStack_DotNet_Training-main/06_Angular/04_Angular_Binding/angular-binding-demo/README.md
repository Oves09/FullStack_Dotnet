## Features Covered

### 1.String Interpolation (`{{ }}`)
- Display component variables directly in the template.
- Example:
  ```html
  <h2>{{ title }}</h2>
  <p>Count: {{ count }}</p>


### 2. Property Binding ([ ])
Bind DOM properties to component fields.

Example:

html
Copy code
<img [src]="imageUrl" alt="Angular Logo" width="100">
<button [disabled]="isDisabled">Click Me</button>

### 3.Event Binding (( ))
Handle events such as clicks.

Example:

html
Copy code
<button (click)="increaseCount()">Increase</button>


### 4.Two-Way Binding ([(ngModel)])
Sync data between component and template.

Example:

html
Copy code
<input [(ngModel)]="title" placeholder="Update title">
<p>Updated Title: {{ title }}</p>