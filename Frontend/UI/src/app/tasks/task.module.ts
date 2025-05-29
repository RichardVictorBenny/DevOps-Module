import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskRoutingModule } from './task-routing.module';
import { TaskListComponent } from './components/task-list/task-list.component';
import { MatModule } from '../shared/modules/mat.module';
import { TaskComponent } from './components/task/task.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TaskCardComponent } from './components/task-card/task-card.component';



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
