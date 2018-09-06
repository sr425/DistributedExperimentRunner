import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatasetdetailsComponent } from './datasetdetails.component';

describe('DatasetdetailsComponent', () => {
  let component: DatasetdetailsComponent;
  let fixture: ComponentFixture<DatasetdetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatasetdetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatasetdetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
