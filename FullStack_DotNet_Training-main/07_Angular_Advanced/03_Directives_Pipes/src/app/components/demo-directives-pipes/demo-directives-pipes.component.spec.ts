import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DemoDirectivesPipesComponent } from './demo-directives-pipes.component';

describe('DemoDirectivesPipesComponent', () => {
  let component: DemoDirectivesPipesComponent;
  let fixture: ComponentFixture<DemoDirectivesPipesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DemoDirectivesPipesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DemoDirectivesPipesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
