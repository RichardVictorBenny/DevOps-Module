/**
 * File: register.component.ts
 * Author: Richard Benny
 * Purpose: Implements the user registration form component, handling form validation and submission logic.
 * Dependencies: Angular Forms, Angular Router, AuthService, FormService, MatModule, CommonModule
 */

import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatModule } from '../../../shared/modules/mat.module';
import { AuthService } from '../../services/auth.service';
import { FormService } from '../../../shared/services/form.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [MatModule, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private formService: FormService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      username: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  /**
   * Handles form submission.
   * If the form is valid, it calls the AuthService to register the user.
   * The FormService handles the observable and displays a success message.
   * On successful registration, navigates to the login page.
   */
  onSubmit() {
    if (this.registerForm.valid) {
      this.formService.Handle(this.authService.Register(this.registerForm.value),
        this.registerForm,
        'Registration successful!'
      ).subscribe(() => {
        this.router.navigate(['/login'], { replaceUrl: true });
      });
    }
  }
}
