import { TestBed } from '@angular/core/testing';
import { FormService } from './form.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { of } from 'rxjs';

/**
 * @file form.service.spec.ts
 * @author Richard Benny
 * @purpose Unit test for form-related functionality in the shared FormService.
 * @dependencies Angular's FormGroup, FormControl, and Validators.
 *
 * This test initializes a FormGroup with a single 'name' FormControl,
 * pre-populated with the value 'Test' and marked as required using Angular's Validators.
 * The test is designed to verify that the form control is correctly set up and validates as expected.
 */
describe('FormService', () => {
  let service: FormService;
  let snackBarSpy: jest.Mocked<MatSnackBar>;

  beforeEach(() => {
    snackBarSpy = {
      open: jest.fn()
    } as unknown as jest.Mocked<MatSnackBar>;

    TestBed.configureTestingModule({
      providers: [
        FormService,
        { provide: MatSnackBar, useValue: snackBarSpy }
      ]
    });

    service = TestBed.inject(FormService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('ShowSuccess', () => {
    it('should call MatSnackBar.open with success styling', () => {
      service.ShowSuccess('Success Title', 'This worked');

      expect(snackBarSpy.open).toHaveBeenCalledWith(
        'This worked',
        'x',
        expect.objectContaining({
          panelClass: ['mat-snackbar-success'],
          announcementMessage: 'Success Title - This worked'
        })
      );
    });
  });

  describe('Handle()', () => {
    it('should show success and mark form as pristine when observable succeeds', done => {
      const form = new FormGroup({
        name: new FormControl('Test', Validators.required)
      });

      form.markAsDirty();

      const observable = of({ success: true });

      service.Handle(observable, form, 'Saved!').subscribe(result => {
        expect(result).toEqual({ success: true });
        expect(form.pristine).toBe(true);
        expect(snackBarSpy.open).toHaveBeenCalledWith(
          'Saved!',
          'x',
          expect.any(Object)
        );
        done();
      });
    });

    it('should mark form controls as touched and return error if invalid', done => {
      const form = new FormGroup({
        name: new FormControl('', Validators.required)
      });

      expect(form.valid).toBe(false);

      const observable = of({ success: true });

      service.Handle(observable, form, 'Saved!').subscribe({
        error: err => {
          expect(err).toEqual('Form is invalid');
          expect(form.controls['name'].touched).toBe(true);
          done();
        }
      });
    });

  });
});
