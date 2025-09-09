import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { provideAnimations } from '@angular/platform-browser/animations';
import 'zone.js';

bootstrapApplication(App, {
  providers: [provideAnimations()]
});



bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));



