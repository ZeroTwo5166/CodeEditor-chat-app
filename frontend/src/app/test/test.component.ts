import { Component, ElementRef, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MonacoEditorService } from '../services/monaco-editor.service'; 
import { DocumentService } from '../document.service';


@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent implements AfterViewInit, OnInit{
  @ViewChild('editorContainer', { static: true }) editorContainer!: ElementRef;

  code: string = ''; // Declare the code property here
  private editor: any;
  private isUpdatingEditor: boolean = false;

  constructor(private monacoEditorService : MonacoEditorService,
    private documentService : DocumentService){}

  ngOnInit(): void {
    this.documentService.documentUpdate$.subscribe((updateDocument) => {
      if (this.editor && !this.isUpdatingEditor) {
        const currentPosition = this.editor.getPosition();
        this.isUpdatingEditor = true;
        this.editor.setValue(updateDocument);
        this.isUpdatingEditor = false;
        this.editor.setPosition(currentPosition);
      }
    });
  }
    
  ngAfterViewInit(): void {
    this.monacoEditorService.initMonaco().then(() => {
      // Use the initial code received from the server
      //const initialValue = this.documentService.getInitialDocument() || 'console.log("Hello, Monaco Editor!");';
      const initialValue = 'console.log("Hello, Monaco Editor!");';

      this.editor = this.monacoEditorService.createEditor(this.editorContainer.nativeElement, {
        value: initialValue,
        language: 'javascript', // Assuming the default language is JavaScript
        theme: 'vs-dark' // Assuming the default theme is vs-dark
      });

      this.editor.onDidChangeModelContent(() => {
        if (!this.isUpdatingEditor) {
          const updatedCode = this.editor.getValue();
          this.code = updatedCode;
          this.documentService.broadcastDocumentUpdate(this.code);
        }
      });
      console.log(this.editor.getValue());
    });
  }
  
  // Modify the onCodeChange method to use broadcastDocumentUpdate
  onCodeChange(): void {
    // Send the updated document to the server
    this.documentService.broadcastDocumentUpdate(this.code);
  }

}
