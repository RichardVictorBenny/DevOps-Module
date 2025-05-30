// File: layout.service.spec.ts
// Author: Richard Benny
// Purpose: Unit test for LayoutService to verify its creation.
// Dependencies: @angular/core/testing, ./layout.service

import { TestBed } from '@angular/core/testing';

import { LayoutService } from './layout.service';

// This test suite checks if the LayoutService can be instantiated successfully.
describe('LayoutService', () => {
  let service: LayoutService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LayoutService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
