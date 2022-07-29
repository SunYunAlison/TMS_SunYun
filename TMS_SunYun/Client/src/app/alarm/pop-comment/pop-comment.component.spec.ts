import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopCommentComponent } from './pop-comment.component';

describe('PopCommentComponent', () => {
  let component: PopCommentComponent;
  let fixture: ComponentFixture<PopCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PopCommentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PopCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
