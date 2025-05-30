import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FormService } from '../../../shared/services/form.service';
import { Task } from '../../models/task.model';
import { TaskService } from '../../services/task.service';

/**
 * @file task.component.ts
 * @author Richard Benny
 * @purpose Defines the TaskComponent, which is responsible for displaying, creating, and editing tasks in the application.
 * @dependencies
 * - @angular/core: For Angular component and lifecycle hooks.
 * - @angular/forms: For reactive form handling.
 * - @angular/router: For navigation and route parameter handling.
 * - TaskService: Custom service for task data operations.
 * - FormService: Custom service for form submission handling.
 *
 * @description
 * This file implements the TaskComponent, an Angular component that manages the UI and logic for viewing, creating, and updating tasks.
 * It uses Angular's reactive forms to bind task data to the form, and interacts with TaskService to fetch, create, or update tasks based on route parameters.
 * The component also handles navigation after task creation or update.
 *
 * @remarks
 * The component checks for a task ID in the route parameters to determine whether to load an existing task or initialize a new one.
 * It provides form validation, submission handling, and user feedback upon successful operations.
 */
@Component({
  selector: 'app-task',
  /* eslint-disable @angular-eslint/prefer-standalone */
  standalone: false,
  templateUrl: './task.component.html',
  styleUrl: './task.component.scss'
})
export class TaskComponent implements OnInit {
  task: Task | null = null;
  taskForm!: FormGroup;

  constructor(
    private taskService: TaskService,
    private route: ActivatedRoute,
    private formService: FormService,
    private router: Router,
    private fb: FormBuilder
  ) {

  }
  public ngOnInit(): void {
    this.route.params.subscribe(params => {
      const taskId = params['id'];
      if (taskId) {
        this.taskService.GetTaskById(taskId).subscribe(task => {
          console.log('Task fetched:', task);
          this.task = task;
          this.loadForm(task);
        });
      } else {
        this.task = new Task();
        this.loadForm(this.task);
      }
    });
  }

  public loadForm(task: Task) {
    this.taskForm = this.fb.group({
      title: [task.title || '', [Validators.required]],
      description: [task.description || '', []],
      dueDate: [task.dueDate ? new Date(task.dueDate) : null, []],
      isFavorite: [task.isFavorite || false],
      isHidden: [task.isHidden || false]
    });
  }

  public createNew(){
    this.router.navigate(['/tasks/task'], { replaceUrl: true });
  }


  public onSave() {
    if (this.taskForm.valid) {
      console.log('Form Submitted!', this.taskForm.value);
      if (this.task?.id) {
        this.task = { ...this.task, ...this.taskForm.value };
        this.formService.Handle(this.taskService.UpdateTask(this.task!), this.taskForm, 'Task updated successfully!')
          .subscribe({
            next: (updatedTask: Task) => {
              this.task = updatedTask;
            }
          });
      } else {
        this.formService.Handle(this.taskService.CreateTask(this.taskForm.value), this.taskForm, 'Task created').subscribe(id => {
          this.router.navigate(['/tasks/task', id], { replaceUrl: true });
        }
        );

      }
    }
  }
}