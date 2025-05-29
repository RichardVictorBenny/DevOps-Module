import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgForm, FormGroup } from '@angular/forms';
import { Observable, throwError } from 'rxjs';
import { ModelStateErrors } from '../models/model-state-errors.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class FormService {

  constructor(private messageService: MatSnackBar) { }

   public Handle<T>(func: Observable<T>, form: NgForm | FormGroup | null, message: string | null = null): Observable<T> {
    if (form && form instanceof NgForm && form.valid === false) {
      for (const control in form.controls) {
        form.controls[control].markAsTouched();
      }
      return throwError(() => 'Form is invalid');
    } else if (form && form instanceof FormGroup && form.invalid) {
      for (const control in form.controls) {
        form.controls[control].markAsTouched();
      }
      return throwError(() => 'Form is invalid');
    }

    return new Observable<T>(observer => {
      func.subscribe({
        next: (data) => {
          if (message) {
            this.ShowSuccess('', message);
          } else {
            this.ShowSaveSuccess();
          }
          if (form instanceof FormGroup) {
            form.markAsPristine();
          } else if (form instanceof NgForm) {
            form.form.markAsPristine();
          }
          observer.next(data);
        },
        error: (error: HttpErrorResponse) => {
          if (error.status === 400) {
            const modelStateErrors = error.error as ModelStateErrors;
            for (const key in modelStateErrors.errors) {
              const formControl =
                form instanceof FormGroup ? form.controls[key] : form?.controls[key];
              const formControlErrors = modelStateErrors.errors[key];
              if (formControl) {
                formControl.setErrors({ server: formControlErrors });
              } else {
                this.ShowSaveError(formControlErrors);
              }
            }
          } else if (error.status === 401 && error.error.title === 'Unauthorized') {
            this.ShowError('', error.error.detail, null);
            observer.error(error);
          } else if (error.status === 500) {
            this.ShowSaveError(error.error.title);
            observer.error(error);
          } else {
            this.ShowSaveError();
            observer.error(error);
          }
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }

  public ShowSaveSuccess(): void {
    this.ShowSuccess('', 'Saved successfully');
  }

  // todo define a type for the error
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public ShowSaveError(error?: any): void {
    this.ShowError('', 'Error saving', error);
  }

  public ShowSuccess(title: string, message: string): void {
    this.messageService.open(
      message,
      'x',
      {
        duration: 5000,
        panelClass: ['mat-snackbar-success'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
        announcementMessage: title ? `${title} - ${message}` : message
      });
  }

  public ShowInfo(title: string, message: string): void {
    this.messageService.open(
      message,
      'x',
      {
        duration: 5000,
        panelClass: ['mat-snackbar-info'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
        announcementMessage: title ? `${title} - ${message}` : message
      });
  }

  public ShowWarning(title: string, message: string): void {
    this.messageService.open(
      message,
      'x',
      {
        duration: 5000,
        panelClass: ['mat-snackbar-warn'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
        announcementMessage: title ? `${title} - ${message}` : message
      });
  }

  // todo define a type for the error
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public ShowError(title: string, message: string, error: any): void {
    let displayMessage = message;

    if (error) {
      displayMessage += ` - ${error}`;
    }

    this.messageService.open(
      displayMessage,
      'x',
      {
        duration: 5000,
        panelClass: ['mat-snackbar-error'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
        announcementMessage: title ? `${title} - ${message}` : message
      });
  }
}
