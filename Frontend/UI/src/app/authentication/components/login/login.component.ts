import { Component } from '@angular/core';
import { MatModule } from '../../../shared/modules/mat.module';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { FormService } from '../../../shared/services/form.service';
import { Router } from '@angular/router';

/**
 * @file login.component.ts
 * @author Richard Benny
 * @purpose Implements the login functionality for the application, including form validation, authentication, and navigation.
 * @dependencies MatModule, FormsModule, ReactiveFormsModule, CommonModule, FormBuilder, AuthService, FormService, Router
 */
@Component({
  selector: 'app-login',
  imports: [MatModule, FormsModule, ReactiveFormsModule, CommonModule ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder, 
    private authService: AuthService, 
    private formService: FormService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      rememberMe: [false],
    });
  }

  //@typescript-eslint/no-unused-vars
  onSubmit() {
    if (this.loginForm.valid) {
      this.formService.Handle(
        this.authService.Login(this.loginForm.value),
        this.loginForm,
        'Login successful!'
      ).subscribe(response => {
        localStorage.setItem('token', JSON.stringify(response));
        this.authService.GetCurrentUser().subscribe(user => {
          localStorage.setItem('user', JSON.stringify(user));
          this.router.navigate(['/'], { replaceUrl: true});
        })
      });
    }
  }

  onRegister() {
    this.router.navigate(['/register'], { replaceUrl: true });
  }

}
