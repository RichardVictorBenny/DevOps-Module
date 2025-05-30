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
