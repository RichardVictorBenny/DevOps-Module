import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list.component';
import { TaskComponent } from './components/task/task.component';
import { loggedInCanActivateFunction } from '../guards/logged-in.guard';

const routes: Routes = [
  {path: '', canActivate: [loggedInCanActivateFunction], component: TaskListComponent},
  {path: 'task', canActivate: [loggedInCanActivateFunction], component: TaskComponent},
  {path: 'task/:id', canActivate: [loggedInCanActivateFunction], component: TaskComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TaskRoutingModule { }
