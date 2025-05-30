import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list.component';
import { TaskComponent } from './components/task/task.component';
import { loggedInCanActivateFunction } from '../guards/logged-in.guard';

/**
 * @file task-routing.module.ts
 * @author Richard Benny
 * @purpose Defines the routing configuration for the tasks feature module, including route guards and component mappings.
 * @dependencies Routes, loggedInCanActivateFunction, TaskListComponent, TaskComponent
 */
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
