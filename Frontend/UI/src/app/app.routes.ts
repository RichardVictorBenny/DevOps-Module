import { Routes } from '@angular/router';
import { LoginComponent } from './authentication/components/login/login.component';
import { TaskListComponent } from './tasks/components/task-list/task-list.component';

export const routes: Routes = [
    {path: '', redirectTo: 'home', pathMatch: 'full'},
    {path: 'home', component: TaskListComponent},
    {path: 'login', component: LoginComponent},
    {path: 'register', loadComponent: () => import('./authentication/components/register/register.component').then(m => m.RegisterComponent)},
    {path: '**', redirectTo: "home", pathMatch: 'full'}// can pass 404 component here if needed
];
