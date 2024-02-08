import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private switchToEditorSubject = new Subject<void>();

  switchToEditor$ = this.switchToEditorSubject.asObservable();

  emitSwitchToEditor() {
    this.switchToEditorSubject.next();
  }
}
