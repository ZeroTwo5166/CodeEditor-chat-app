import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BackgroundColorService {
  private color1Subject = new BehaviorSubject<string>("#11088c");
  private color2Subject = new BehaviorSubject<string>("#b818b3");

  color1$ = this.color1Subject.asObservable();
  color2$ = this.color2Subject.asObservable();

  setColor1(color: string): void {
    this.color1Subject.next(color);
  }

  setColor2(color: string): void {
    this.color2Subject.next(color);
  }
}
