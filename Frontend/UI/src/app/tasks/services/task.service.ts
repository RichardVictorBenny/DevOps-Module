import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Task } from '../models/task.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private http: HttpClient) { }

  public GetTasks(): Observable<Task[]> {
    const endpoint = `${environment.apiUrl}/Task`;
    return this.http.get<Task[]>(endpoint);
  }

  public GetTaskById(id: string): Observable<Task> {
    const endpoint = `${environment.apiUrl}/Task/${id}`;
    return this.http.get<Task>(endpoint);
  }

  public CreateTask(task: Task): Observable<Task> {
    const endpoint = `${environment.apiUrl}/Task`;
    return this.http.post<Task>(endpoint, task);
  }
  
  /* eslint-disable @typescript-eslint/no-explicit-any */
  public UpdateTask(task: Task): Observable<any> {
    const endpoint = `${environment.apiUrl}/Task/${task.id}`;
    return this.http.put<any>(endpoint, task);
  }

  public DeleteTask(id: string): Observable<void> {
    const endpoint = `${environment.apiUrl}/Task/${id}`;
    return this.http.delete<void>(endpoint);
  }


}
