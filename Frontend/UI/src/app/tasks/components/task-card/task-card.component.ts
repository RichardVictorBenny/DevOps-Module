import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormService } from '../../../shared/services/form.service';
import { Task } from '../../models/task.model';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-task-card',
  /* eslint-disable @angular-eslint/prefer-standalone */
  standalone: false,
  templateUrl: './task-card.component.html',
  styleUrl: './task-card.component.scss'
})
export class TaskCardComponent {
  @Input() tasks!: Task[];
  @Output() taskUpdated = new EventEmitter<void>();

  constructor(
    private taskService: TaskService,
    private formService: FormService
  ) {}

    toggleFavorite(task: Task): void {
    task.isFavorite = !task.isFavorite;
    this.taskService.UpdateTask(task).subscribe(() => {
      this.taskUpdated.emit();
    });
  }
  markAsCompleted(task: Task, isCompleted: boolean) {
    task.isHidden = isCompleted;
    this.taskService.UpdateTask(task).subscribe(() => {
      this.taskUpdated.emit();
    });
  }

  deleteTask(task: Task) {
    this.formService.Handle(
      this.taskService.DeleteTask(task.id!),
      null,
      'Task deleted successfully!'
    ).subscribe(() => {
      this.taskUpdated.emit();
    });
  }
}
