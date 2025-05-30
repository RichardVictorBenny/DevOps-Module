import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of } from 'rxjs';

import { RegisterComponent } from './register.component';
import { MatModule } from '../../../shared/modules/mat.module';
import { AuthService } from '../../services/auth.service';
import { FormService } from '../../../shared/services/form.service';

describe('RegisterComponent (Jest)', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;

  const mockAuthService = {
    Register: jest.fn().mockReturnValue(of({}))
  };

  const mockFormService = {
    Handle: jest.fn().mockReturnValue(of({}))
  };

  const mockRouter = {
    navigate: jest.fn()
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, FormsModule, MatModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: FormService, useValue: mockFormService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeDefined();
  });

  it('should call FormService.Handle and navigate on valid form submit', () => {
    component.registerForm.setValue({
      firstname: 'Jane',
      lastname: 'Doe',
      username: 'jane@example.com',
      password: 'securePassword'
    });

    component.onSubmit();

    expect(mockFormService.Handle).toHaveBeenCalledWith(
      expect.anything(),
      component.registerForm,
      'Registration successful!'
    );

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login'], { replaceUrl: true });
  });


});

