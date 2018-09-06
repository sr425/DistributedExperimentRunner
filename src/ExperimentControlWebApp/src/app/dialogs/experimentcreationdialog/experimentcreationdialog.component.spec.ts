import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExperimentcreationdialogComponent } from './experimentcreationdialog.component';

describe('ExperimentcreationdialogComponent', () => {
  let component: ExperimentcreationdialogComponent;
  let fixture: ComponentFixture<ExperimentcreationdialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExperimentcreationdialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExperimentcreationdialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
