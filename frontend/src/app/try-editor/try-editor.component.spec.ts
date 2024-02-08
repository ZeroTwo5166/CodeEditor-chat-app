import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TryEditorComponent } from './try-editor.component';

describe('TryEditorComponent', () => {
  let component: TryEditorComponent;
  let fixture: ComponentFixture<TryEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TryEditorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TryEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
