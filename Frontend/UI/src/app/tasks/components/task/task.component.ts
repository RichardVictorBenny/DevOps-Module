import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FormService } from '../../../shared/services/form.service';
import { Task } from '../../models/task.model';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-task',
  
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