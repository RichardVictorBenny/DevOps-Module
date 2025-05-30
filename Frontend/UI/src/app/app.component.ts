/**
 * File: app.component.ts
 * Author: Richard Benny - 22825546
 * Purpose: Main root component for the Angular application. Handles user authentication state, layout changes, and navigation.
 * Dependencies: Angular core, Angular router, LayoutService, AuthService, User model, CommonModule, MatModule, TaskModule.
 */

import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { LayoutService } from './shared/services/layout.service';
import { CommonModule } from '@angular/common';
import { MatModule } from './shared/modules/mat.module';
import { TaskModule } from "./tasks/task.module";
import { AuthService } from './authentication/services/auth.service';
import { User } from './authentication/models';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, MatModule, TaskModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {

  title = 'UI';
  public user: User | null = null;
  public isLoggedIn = false;

  public isStandalonePage = false;

  constructor(private layoutService: LayoutService, private router: Router, private authService: AuthService) { }

  /**
   * On component initialization, subscribes to layout changes and checks for a logged-in user in localStorage.
   */
  ngOnInit(): void {
    this.layoutService.isStandalonePage.subscribe((val: boolean) => {
      this.isStandalonePage = val;
    });

    const user = localStorage.getItem('user');
    if (user) {
      this.user = JSON.parse(user);
      this.isLoggedIn = true;
    } else {
      this.isLoggedIn = false;
    }

  }

  /**
   * Navigates to the login page.
   */
  onLogin() {
    this.router.navigate(['/login'], { replaceUrl: true });
  }

  /**
   * Logs out the user and updates the authentication state.
   */
  onLogout() {
    this.isLoggedIn = false;
    this.authService.Logout();
  }

  /**
   * Example method to sum two numbers.
   */
  sum(a: number, b: number) {
    return a + b;
  }

}
