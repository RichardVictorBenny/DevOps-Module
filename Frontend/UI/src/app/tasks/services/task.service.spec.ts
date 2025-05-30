import { TestBed } from '@angular/core/testing';
import { TaskService } from './task.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Task } from '../models/task.model';
import { environment } from '../../../environments/environment';

describe('TaskService', () => {
  let service: TaskService;
  let httpMock: HttpTestingController;

  const apiUrl = environment.apiUrl;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TaskService]
    });

    service = TestBed.inject(TaskService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('GetTasks()', () => {
    it('should return an array of tasks', () => {
      const mockTasks: Task[] = [
        new Task({ id: '1', title: 'Task 1', description: 'Desc 1', isFavorite: false, isHidden: false, dueDate: new Date() }),
        new Task({ id: '2', title: 'Task 2', description: 'Desc 2', isFavorite: true, isHidden: false, dueDate: new Date() }),
      ];

      service.GetTasks().subscribe(tasks => {
        expect(tasks.length).toBe(2);
        expect(tasks).toEqual(mockTasks);
      });

      const req = httpMock.expectOne(`${apiUrl}/Task`);
      expect(req.request.method).toBe('GET');
      req.flush(mockTasks);
    });
  });

  describe('GetTaskById()', () => {
    it('should return a single task', () => {
      const mockTask = new Task({ id: '1', title: 'Task 1', description: 'Desc', isFavorite: false, isHidden: false, dueDate: new Date() });

      service.GetTaskById('1').subscribe(task => {
        expect(task).toEqual(mockTask);
      });

      const req = httpMock.expectOne(`${apiUrl}/Task/1`);
      expect(req.request.method).toBe('GET');
      req.flush(mockTask);
    });
  });

  describe('CreateTask()', () => {
    it('should send POST request and return created task', () => {
      const taskToCreate = new Task({ title: 'New Task', description: 'New Desc', isFavorite: false, isHidden: false, dueDate: new Date() });
      const createdTask = new Task({ ...taskToCreate, id: '3' });

      service.CreateTask(taskToCreate).subscribe(task => {
        expect(task).toEqual(createdTask);
      });

      const req = httpMock.expectOne(`${apiUrl}/Task`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(taskToCreate);
      req.flush(createdTask);
    });
  });

  describe('UpdateTask()', () => {
    it('should send PUT request and return updated task', () => {
      const taskToUpdate = new Task({ id: '1', title: 'Updated', description: 'Updated Desc', isFavorite: true, isHidden: false, dueDate: new Date() });

      service.UpdateTask(taskToUpdate).subscribe(task => {
        expect(task).toEqual(taskToUpdate);
      });

      const req = httpMock.expectOne(`${apiUrl}/Task/1`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(taskToUpdate);
      req.flush(taskToUpdate);
    });
  });

  describe('DeleteTask()', () => {
    it('should send DELETE request', () => {
      service.DeleteTask('1').subscribe(response => {
        expect(response).toBeUndefined();
      });

      const req = httpMock.expectOne(`${apiUrl}/Task/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });
});
