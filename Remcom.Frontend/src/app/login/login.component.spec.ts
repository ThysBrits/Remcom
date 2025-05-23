import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

class RouterStub {
  navigate(commands: any[]) {}
}

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let httpMock: HttpTestingController;
  let router: RouterStub;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, ReactiveFormsModule, HttpClientTestingModule, LoginComponent],
      providers: [
        { provide: Router, useClass: RouterStub }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
    router = TestBed.inject(Router) as any;
    fixture.detectChanges();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show error on failed login', () => {
    component.loginForm.setValue({ username: 'baduser', password: 'badpass' });
    component.login();
    const req = httpMock.expectOne('http://localhost:5000/api/users/login');
    req.flush('Invalid credentials', { status: 401, statusText: 'Unauthorized' });
    expect(component.error).toBe('Invalid credentials');
    expect(component.loading).toBeFalse();
  });

  it('should store token and navigate on successful login', () => {
    spyOn(localStorage, 'setItem');
    spyOn(router, 'navigate');
    component.loginForm.setValue({ username: 'test', password: 'testpass' });
    component.login();
    const req = httpMock.expectOne('http://localhost:5000/api/users/login');
    req.flush({ token: 'abc123' });
    expect(localStorage.setItem).toHaveBeenCalledWith('token', 'abc123');
    expect(router.navigate).toHaveBeenCalledWith(['/']);
    expect(component.error).toBe('');
    expect(component.loading).toBeFalse();
  });
});
