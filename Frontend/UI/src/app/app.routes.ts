import { Routes } from '@angular/router';
import { LoginComponent } from './authentication/components/login/login.component';

export const routes: Routes = [
    {path: '', redirectTo: 'tasks', pathMatch: 'full'},
    {path: 'tasks', loadChildren: () => import('./tasks/task.module').then(m => m.TaskModule)},
    {path: 'login', component: LoginComponent},
    {path: 'register', loadComponent: () => import('./authentication/components/register/register.component').then(m => m.RegisterComponent)},
    {path: '**', redirectTo: "tasks", pathMatch: 'full'}// can pass 404 here
];
