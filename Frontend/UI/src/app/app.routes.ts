import { Routes } from '@angular/router';
import { LoginComponent } from './authentication/components/login/login.component';
import { TaskListComponent } from './tasks/components/task-list/task-list.component';

export const routes: Routes = [
    {path: '', component: TaskListComponent},

    {path: 'login', component: LoginComponent},
    {path: 'register', loadComponent: () => import('./authentication/components/register/register.component').then(m => m.RegisterComponent)},
];
