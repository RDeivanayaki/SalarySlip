import { TestBed } from '@angular/core/testing';

import { SalarySlipService } from './services/salaryslip.service';

describe('FileuploadService', () => {
  let service: SalarySlipService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SalarySlipService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
