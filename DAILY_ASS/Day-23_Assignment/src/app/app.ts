import { Component, signal } from '@angular/core';
import { EventList } from './components/event-list/event-list';

@Component({
  selector: 'app-root',
  imports: [EventList],
  styles: [`
    .container { max-width: 960px; margin: 24px auto; padding: 0 16px; }
    h1 { margin: 16px 0 24px; }
  `],
  template: `
    <main class="container">
      <h1>Event Management Portal</h1>
      <app-event-list />
    </main>
  `,
})
export class App {
  protected readonly title = signal('event-mgmt');
}
