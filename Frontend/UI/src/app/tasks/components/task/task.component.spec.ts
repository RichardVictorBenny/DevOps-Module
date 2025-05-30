import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskComponent } from './task.component';
import { TaskService } from '../../services/task.service';
import { FormService } from '../../../shared/services/form.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { Task } from '../../models/task.model';
import { MatModule } from '../../../shared/modules/mat.module';

  /* eslint-disable @typescript-eslint/no-explicit-any */
describe('TaskComponent', () => {
  let component: TaskComponent;
  let fixture: ComponentFixture<TaskComponent>;
  let taskServiceMock: any;
  let formServiceMock: any;
  let routerMock: any;
  let activatedRouteMock: any;

  beforeEach(async () => {
    taskServiceMock = {
      GetTaskById: jest.fn(),
      CreateTask: jest.fn(),
      UpdateTask: jest.fn()
    };

    formServiceMock = {
      Handle: jest.fn()
    };

    routerMock = {
      navigate: jest.fn()
    };

    activatedRouteMock = {
      params: of({})
    };

    await TestBed.configureTestingModule({
      declarations: [TaskComponent],
      imports: [ReactiveFormsModule, MatModule],
      providers: [
        { provide: TaskService, useValue: taskServiceMock },
        { provide: FormService, useValue: formServiceMock },
        { provide: Router, useValue: routerMock },
        { provide: ActivatedRoute, useValue: activatedRouteMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load task and form in update mode', () => {
    const taskData: Task = {
      id: "1",
      title: 'Test Task',
      description: 'Desc',
      dueDate: new Date(),
      isFavorite: true,
      isHidden: false
    };

    activatedRouteMock.params = of({ id: 1 });
    taskServiceMock.GetTaskById.mockReturnValue(of(taskData));

    // Recreate component with updated mocks
    fixture = TestBed.createComponent(TaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(taskServiceMock.GetTaskById).toHaveBeenCalledWith(1);
    expect(component.taskForm.value.title).toBe('Test Task');
  });

  it('should create a task and navigate on save', () => {
    const newId = 99;
    taskServiceMock.CreateTask.mockReturnValue(of(newId));
    formServiceMock.Handle.mockImplementation((obs:any) => obs);

    component.taskForm.setValue({
      title: 'New Task',
      description: '',
      dueDate: null,
      isFavorite: false,
      isHidden: false
    });

    component.onSave();

    expect(taskServiceMock.CreateTask).toHaveBeenCalledWith(component.taskForm.value);
    expect(formServiceMock.Handle).toHaveBeenCalled();
    expect(routerMock.navigate).toHaveBeenCalledWith(['/tasks/task', newId], { replaceUrl: true });
  });

  it('should update a task and not navigate', () => {
    const updatedTask : Task = {
      id: "42",
      title: 'Updated',
      description: 'Updated desc',
      dueDate: new Date(),
      isFavorite: false,
      isHidden: true
    };

    component.task = { ...updatedTask };
    component.loadForm(updatedTask);

    taskServiceMock.UpdateTask.mockReturnValue(of(updatedTask));
    formServiceMock.Handle.mockImplementation((obs: any) => obs); 

    component.onSave();

    expect(taskServiceMock.UpdateTask).toHaveBeenCalledWith(updatedTask);
    expect(formServiceMock.Handle).toHaveBeenCalled();
    expect(routerMock.navigate).not.toHaveBeenCalled();
  });

  it('should not call services if form is invalid', () => {
    component.taskForm.patchValue({ title: '' });
    component.onSave();

    expect(taskServiceMock.CreateTask).not.toHaveBeenCalled();
    expect(formServiceMock.Handle).not.toHaveBeenCalled();
    expect(routerMock.navigate).not.toHaveBeenCalled();
  });
});
