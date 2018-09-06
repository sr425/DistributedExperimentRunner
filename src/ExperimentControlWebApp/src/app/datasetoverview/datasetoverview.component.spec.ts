import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatasetoverviewComponent } from './datasetoverview.component';

describe('DatasetoverviewComponent', () => {
  let component: DatasetoverviewComponent;
  let fixture: ComponentFixture<DatasetoverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatasetoverviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatasetoverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
