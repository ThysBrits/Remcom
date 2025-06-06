import { Component } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  error = '';
  loading = false;
  loginForm;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  login() {
    this.error = '';
    this.loading = true;
    const { username, password } = this.loginForm.value;
    this.http.post<any>('http://localhost:5254/api/users/login', { username, password })
      .subscribe({
        next: (res) => {
          localStorage.setItem('token', res.token);
          // Store role and username for convenience
          localStorage.setItem('role', res.role);
          localStorage.setItem('username', res.username);
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.error = err.error || 'Login failed';
          this.loading = false;
        },
        complete: () => this.loading = false
      });
  }
}
