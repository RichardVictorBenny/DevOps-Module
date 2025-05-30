/**
 * File: task-list.component.ts
 * Author: Richard Benny
 * Purpose: Angular component for displaying and managing a list of tasks, including favorites, regular, and completed tasks.
 * Dependencies: Angular core, Angular forms, Angular router, Task model, Task service.
 */
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Task } from '../../models/task.model';
import { Router } from '@angular/router';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-task-list',
  /* eslint-disable @angular-eslint/prefer-standalone */
  standalone: false,
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss'
})
export class TaskListComponent implements OnInit {




  taskList: Task[] = [];


  constructor(
    private fb: FormBuilder,
    private router: Router,
    private taskService: TaskService,
  ) {

  }
  ngOnInit(): void {
    this.loadTasks();

  }

  public loadTasks(): void {
    this.taskService.GetTasks().subscribe(tasks => {
      this.taskList = tasks;
    });
  }

  public get favoriteTasks(): Task[] {
    return this.taskList.filter(task => task.isFavorite && !task.isHidden);
  }
  public get regularTasks(): Task[] {
    return this.taskList.filter(task => !task.isFavorite && !task.isHidden);
  }
  public get completedTasks(): Task[] {
    return this.taskList.filter(task => task.isHidden);
  }


  createTask(): void {
    this.router.navigateByUrl('/tasks/task');
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  onTaskUpdated(_: void) {
    this.loadTasks();
  }



}