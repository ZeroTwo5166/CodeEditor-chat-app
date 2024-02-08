import { Injectable } from '@angular/core';

declare const require: any;

@Injectable({
  providedIn: 'root'
})
export class MonacoEditorService {
  private monaco: any;

  public initMonaco(): Promise<void> {
    return new Promise<void>((resolve) => {
      if (typeof this.monaco !== 'undefined') {
        resolve();
        return;
      }

      const onGotAmdLoader = () => {
        // Load monaco
        require.config({ paths: { 'vs': 'https://cdn.jsdelivr.net/npm/monaco-editor@0.45.0/min/vs' } });
        require(['vs/editor/editor.main'], () => {
          this.monaco = (window as any).monaco;
          resolve();
        });
      };

      // Load AMD loader if necessary
      if (!(window as any).require) {
        const loaderScript = document.createElement('script');
        loaderScript.type = 'text/javascript';
        loaderScript.src = 'https://cdn.jsdelivr.net/npm/monaco-editor@0.45.0/min/vs/loader.js';
        loaderScript.addEventListener('load', onGotAmdLoader);
        document.body.appendChild(loaderScript);
      } else {
        onGotAmdLoader();
      }
    });
  }

  public createEditor(container: HTMLElement, options?: any): any {
    return this.monaco.editor.create(container, options);
  }

}
