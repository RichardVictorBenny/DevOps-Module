/**
 * File: task.model.ts
 * Author: Richard Benny
 * Purpose: Defines the Task model used for representing task data in the application.
 * Dependencies: None
 *
 * The Task class models a task entity with properties such as id, title, description, isFavorite, isHidden, and dueDate.
 * The constructor allows partial initialization and ensures default values for boolean fields and null for optional fields.
 */
export class Task {
  id: string | null;
  title: string | null;
  description: string | null;
  isFavorite: boolean;
  isHidden: boolean;
  dueDate: Date | null;

  constructor(task?: Partial<Task>) {
    this.id = task?.id ?? null;
    this.title = task?.title ?? null;
    this.description = task?.description ?? null;
    this.isFavorite = task?.isFavorite ?? false;
    this.isHidden = task?.isHidden ?? false;
    this.dueDate = task?.dueDate ? new Date(task.dueDate) : null;
  }
}
