import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskCardComponent } from './task-card.component';
import { TaskService } from '../../services/task.service';
import { FormService } from '../../../shared/services/form.service';
import { of } from 'rxjs';
import { MatModule } from '../../../shared/modules/mat.module';

describe('TaskCardComponent', () => {
  let component: TaskCardComponent;
  let fixture: ComponentFixture<TaskCardComponent>;
  let taskServiceMock: jest.Mocked<TaskService>;
  let formServiceMock: jest.Mocked<FormService>;

  beforeEach(async () => {
    taskServiceMock = {
      UpdateTask: jest.fn(),
      DeleteTask: jest.fn()
    } as any;

    formServiceMock = {
      Handle: jest.fn()
    } as any;

    await TestBed.configureTestingModule({
      declarations: [TaskCardComponent],
      imports: [MatModule],
      providers: [
        { provide: TaskService, useValue: taskServiceMock },
        { provide: FormService, useValue: formServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TaskCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('toggleFavorite', () => {
    it('should toggle isFavorite and call UpdateTask, then emit taskUpdated', done => {
      const task = { id: 1, isFavorite: false } as any;
      taskServiceMock.UpdateTask.mockReturnValue(of(null));

      // Spy on the output emitter
      const emitSpy = jest.spyOn(component.taskUpdated, 'emit');

      component.toggleFavorite(task);

      expect(task.isFavorite).toBe(true);
      expect(taskServiceMock.UpdateTask).toHaveBeenCalledWith(task);

      // Since UpdateTask returns observable, subscribe to confirm emit
      taskServiceMock.UpdateTask(task).subscribe(() => {
        expect(emitSpy).toHaveBeenCalled();
        done();
      });
    });
  });

  describe('markAsCompleted', () => {
    it('should update isHidden and call UpdateTask, then emit taskUpdated', done => {
      const task = { id: 1, isHidden: false } as any;
      taskServiceMock.UpdateTask.mockReturnValue(of(null));
      const emitSpy = jest.spyOn(component.taskUpdated, 'emit');

      component.markAsCompleted(task, true);

      expect(task.isHidden).toBe(true);
      expect(taskServiceMock.UpdateTask).toHaveBeenCalledWith(task);

      taskServiceMock.UpdateTask(task).subscribe(() => {
        expect(emitSpy).toHaveBeenCalled();
        done();
      });
    });
  });

  describe('deleteTask', () => {
    it('should call formService.Handle and emit taskUpdated after success', done => {
      const task = { id: 42 } as any;
      const handleObservable = of(null);
      formServiceMock.Handle.mockReturnValue(handleObservable);
      const emitSpy = jest.spyOn(component.taskUpdated, 'emit');

      component.deleteTask(task);

      expect(formServiceMock.Handle).toHaveBeenCalledWith(
        taskServiceMock.DeleteTask(task.id),
        null,
        'Task deleted successfully!'
      );

      handleObservable.subscribe(() => {
        expect(emitSpy).toHaveBeenCalled();
        done();
      });
    });
  });
});
