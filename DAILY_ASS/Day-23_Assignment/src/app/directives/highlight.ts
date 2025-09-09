import { Directive, HostBinding, Input, OnChanges } from '@angular/core';

@Directive({
  selector: '[appHighlight]',
  standalone: true
})
export class Highlight implements OnChanges {

  @Input('appHighlight') price: number | null | undefined;

  @HostBinding('style.backgroundColor') bg?: string;

  ngOnChanges(): void {
    const p = Number(this.price ?? 0);
    // Premium: price > 2000 → light gold
    this.bg = p > 2000 ? 'lightgoldenrodyellow' : '';
  }

}
