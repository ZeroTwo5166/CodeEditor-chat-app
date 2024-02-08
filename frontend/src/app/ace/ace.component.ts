import { Component, ElementRef, OnDestroy, OnInit, Renderer2, ViewChild, ViewEncapsulation } from '@angular/core';
import { ace_themes, allAceLanguages} from '../others'; 
import { AceServiceService } from '../services/ace-service.service';
import { DocumentService } from '../document.service';
import { ApiService } from '../services/api.service';

declare var ace: any;

@Component({
  selector: 'app-ace',
  templateUrl: './ace.component.html',
  styleUrl: './ace.component.css'

})
export class AceComponent implements OnInit, OnDestroy{

  aceThemes: any = ace_themes;
  selectedThemeKey: string = 'Clouds'; // Store the selected key
  selectedThemeValue: string = ''; // Store the selected value

  documentCreated: boolean = false;

  code: string = ''; // Declare the code property here
  editor !: any;
  private isUpdatingEditor: boolean = false;

  aceLanguages : any = allAceLanguages;
  selectedLanguageKey: string = 'Javascript'; // Store the selected key
  selectedLanguageValue: string = ''; // Store the selected value

  private autoSaveTimer: any;

  constructor(private elementRef: ElementRef, private renderer: Renderer2, private documentService: DocumentService, private apiService : ApiService) { }

  ngOnInit() {

      this.documentService.intialDoc$.subscribe((response)=> {
      if(this.editor && this.editor.getValue() == ""){
        this.editor.setValue(response)
        console.log("initial sub")
      }
    })


    this.documentService.documentUpdate$.subscribe((updateDocument) => {
      if (this.editor && !this.isUpdatingEditor) {
        const currentPosition = this.editor.getCursorPosition();
        this.isUpdatingEditor = true;
        console.log("doc sub")
  
        // Check if the updateDocument is different from the current editor content
        if (updateDocument !== this.editor.getValue()) {
          this.editor.setValue(updateDocument, currentPosition); // Set the updated document and cursor position
        }
        this.isUpdatingEditor = false;
      }
    });
  }


  ngAfterViewInit(): void {
    // Delay Ace Editor initialization
    setTimeout(() => {
      const editorElement = this.elementRef.nativeElement.querySelector('#editor');

      this.renderer.setStyle(editorElement, 'height', '500px');

      this.editor = ace.edit(editorElement);
      this.updateEditorTheme(this.editor);
      this.updateEditorMode(this.editor);
      this.editor.session.setMode('ace/mode/javascript');

      if (this.editor && this.editor.getValue() == "") {
        this.documentService.intialDoc$.subscribe(response => {
          this.editor.setValue(response)
        })
      }


      this.editor.session.on('change', () => {
        if(!this.isUpdatingEditor){
          const updatedCode = this.editor.getValue();
          this.code = updatedCode;
          //this.documentService.broadcastDocumentUpdate(this.code);
          // Start or reset the auto-save timer when the document is changed
          //this.startAutoSaveTimer();
        }
      });  
      this.editor.session.selection.on('changeCursor', (e: any) => {
        const cursorPosition = this.editor.getCursorPosition();

      }); 
    });
  }

  ngOnDestroy(): void {
    // Clear the auto-save timer when the component is destroyed
   // this.clearAutoSaveTimer();
    this.documentService.leaveChat();
  }

  leavegroup(){
    this.documentService.leaveChat();
  }

  /*
  startAutoSaveTimer(): void {
    // Clear the existing timer if it exists
    this.clearAutoSaveTimer();

    // Start a new timer for auto-save after 5 seconds of inactivity
    this.autoSaveTimer = setTimeout(() => {
      this.sendCode(); // Perform auto-save
    }, 5000);
  }

  clearAutoSaveTimer(): void {
    // Clear the existing auto-save timer
    if (this.autoSaveTimer) {
      clearTimeout(this.autoSaveTimer);
      this.autoSaveTimer = null;
    }
  } */


  sendCode() {
    // Check if the document is already created in the database
    if (!this.documentCreated) {
      // If not created, perform an insert operation
      this.apiService.insertCodeToSql("TESTT", this.code).subscribe(resp => {
        console.log(resp);
        this.documentCreated = true;  // Mark the document as created
      }, err => {
        console.log(err);
      });
    } else {
      // If already created, perform an update operation
      this.apiService.updateCodeToSql("TESTT", this.code).subscribe(resp => {
        console.log(resp);
      }, err => {
        console.log(err);
      });
    }
  }
  

  //Changing the theme of code editor
  onThemeChange(): void {
    this.selectedThemeValue = this.aceThemes[this.selectedThemeKey];
    console.log('Selected Theme:', this.selectedThemeValue);

    const editor = ace.edit('editor');
    this.updateEditorTheme(editor);
  }

  //Changing the language of code editor
  onLanguageChange(){
    this.selectedLanguageValue = this.aceLanguages[this.selectedLanguageKey]
    console.log('Selected Language: ', this.selectedLanguageValue)

    const editor = ace.edit('editor');
    this.updateEditorMode(editor);
  }

  private updateEditorTheme(editor: any): void {
    editor.setTheme(`ace/theme/${this.selectedThemeValue}`);
  }
  
  private updateEditorMode(editor: any): void {
    const mode = `ace/mode/${this.selectedLanguageValue}`;
    editor.session.setMode(mode);
  }

  getObjectKeys(obj : any): string[] {
    return Object.keys(obj);
  }
}
