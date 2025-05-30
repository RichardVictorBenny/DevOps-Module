import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskRoutingModule } from './task-routing.module';
import { TaskListComponent } from './components/task-list/task-list.component';
import { MatModule } from '../shared/modules/mat.module';
import { TaskComponent } from './components/task/task.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TaskCardComponent } from './components/task-card/task-card.component';



/**
 * @file task.module.ts
 * @author Richard Benny
 * @purpose Defines the TaskModule, which encapsulates components and dependencies related to task management features.
 * @dependencies MatModule, CommonModule, TaskRoutingModule, ReactiveFormsModule, FormsModule
 */
@NgModule({
  declarations: [
    TaskListComponent,
    TaskComponent,
    TaskCardComponent
  ],
  imports: [
    MatModule,
    CommonModule,
    TaskRoutingModule,
    ReactiveFormsModule,
    FormsModule
  ],
  exports: [
    TaskListComponent,
    TaskComponent
  ],
})
export class TaskModule { }
