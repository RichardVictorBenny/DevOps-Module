import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskListComponent } from './task-list.component';
import { TaskService } from '../../services/task.service';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { Task } from '../../models/task.model';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('TaskListComponent', () => {
  let component: TaskListComponent;
  let fixture: ComponentFixture<TaskListComponent>;
  let taskServiceMock: Partial<jest.Mocked<TaskService>>;
  let routerMock: Partial<jest.Mocked<Router>>;

  const mockTasks: Task[] = [
    { id: '1', title: 'Fav Task', description: null, isFavorite: true, isHidden: false, dueDate: null },
    { id: '2', title: 'Regular Task', description: null, isFavorite: false, isHidden: false, dueDate: null },
    { id: '3', title: 'Completed Task', description: null, isFavorite: false, isHidden: true, dueDate: null }
  ];

  beforeEach(async () => {
    taskServiceMock = {
      GetTasks: jest.fn().mockReturnValue(of(mockTasks))
    };

    routerMock = {
      navigateByUrl: jest.fn()
    };

    await TestBed.configureTestingModule({
      declarations: [TaskListComponent],
      providers: [
        { provide: TaskService, useValue: taskServiceMock },
        { provide: Router, useValue: routerMock }
      ],
      schemas: [NO_ERRORS_SCHEMA] // Ignore unknown template elements/styles
    }).compileComponents();

    fixture = TestBed.createComponent(TaskListComponent);
    component = fixture.componentInstance;
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should load tasks on init and populate taskList', () => {
    component.ngOnInit();
    expect(taskServiceMock.GetTasks).toHaveBeenCalled();
    expect(component.taskList).toEqual(mockTasks);
  });

  it('should return only favorite and not hidden tasks for favoriteTasks getter', () => {
    component.taskList = mockTasks;
    const favTasks = component.favoriteTasks;
    expect(favTasks.length).toBe(1);
    expect(favTasks[0].isFavorite).toBe(true);
    expect(favTasks[0].isHidden).toBe(false);
  });

  it('should return only regular (not favorite and not hidden) tasks for regularTasks getter', () => {
    component.taskList = mockTasks;
    const regularTasks = component.regularTasks;
    expect(regularTasks.length).toBe(1);
    expect(regularTasks[0].isFavorite).toBe(false);
    expect(regularTasks[0].isHidden).toBe(false);
  });

  it('should return only hidden tasks for completedTasks getter', () => {
    component.taskList = mockTasks;
    const completed = component.completedTasks;
    expect(completed.length).toBe(1);
    expect(completed[0].isHidden).toBe(true);
  });

  it('should navigate to /tasks/task when createTask is called', () => {
    component.createTask();
    expect(routerMock.navigateByUrl).toHaveBeenCalledWith('/tasks/task');
  });

  it('should reload tasks when onTaskUpdated is called', () => {
    const spy = jest.spyOn(component, 'loadTasks');
    component.onTaskUpdated(undefined);
    expect(spy).toHaveBeenCalled();
  });
});
