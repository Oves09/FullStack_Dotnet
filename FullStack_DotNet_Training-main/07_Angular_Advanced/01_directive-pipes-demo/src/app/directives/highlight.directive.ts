import { Directive, ElementRef, HostListener, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appHighlight]', // Usage: add this attribute to any element
  standalone: true
})
export class HighlightDirective {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  // When mouse enters, change background
  @HostListener('mouseenter') onMouseEnter() {
    this.renderer.setStyle(this.el.nativeElement, 'background-color', 'yellow');
  }

  // When mouse leaves, remove background
  @HostListener('mouseleave') onMouseLeave() {
    this.renderer.removeStyle(this.el.nativeElement, 'background-color');
  }
}
