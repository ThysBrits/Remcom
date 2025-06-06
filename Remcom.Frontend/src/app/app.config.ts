import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, Routes } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

import { HomeComponent } from './home.component';
import { LoginComponent } from './login/login.component';
import { authGuard } from './auth.guard';
import { NoticeSearchComponent } from './notice-search.component';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'notices/search', component: NoticeSearchComponent, canActivate: [authGuard] },
  // Add stubs for other routes as needed
];

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    provideAnimations(),
  ]
};
