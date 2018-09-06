import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InputParameterDialogComponent } from './input-parameter-dialog.component';

describe('InputParameterDialogComponent', () => {
  let component: InputParameterDialogComponent;
  let fixture: ComponentFixture<InputParameterDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InputParameterDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InputParameterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
