import { Component } from '@angular/core';
import { MatModule } from '../../../shared/modules/mat.module';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { FormService } from '../../../shared/services/form.service';
import { response } from 'express';

@Component({
  selector: 'app-login',
  imports: [MatModule, FormsModule, ReactiveFormsModule, CommonModule ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private formService: FormService) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      rememberMe: [false],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      console.log('Form Submitted!', this.loginForm.value);
      this.formService.Handle(
        this.authService.Login(this.loginForm.value),
        this.loginForm,
        'Login successful!'
      ).subscribe(response => {
        console.log('Login response:', response);
      });
    }
  }

}
