import { Component, ElementRef, OnInit, ViewChild, AfterViewInit, OnDestroy, Renderer2 } from '@angular/core';
import { LoginStatusService } from '../services/login-status.service';
import { ace_themes, allAceLanguages} from '../others'; 
import { ApiService } from '../services/api.service';
import { DocumentService } from '../document.service';
import { Router, NavigationStart  } from '@angular/router';
import { HubService } from '../services/hub.service';
import { ProjectApiService } from '../services/project-api.service';

declare var ace: any;


@Component({
  selector: 'app-code-editor',
  templateUrl: './code-editor.component.html',
  styleUrl: './code-editor.component.css'
})
export class CodeEditorComponent implements AfterViewInit, OnInit, OnDestroy{
  @ViewChild('editorContainer', { static: true }) editorContainer!: ElementRef; //reference to the html element

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

  //private autoSaveDelay: number = 5000; // 5 seconds delay for autosave
  //private autoSaveTimer: any;

  projectId !: any;//need?
  //editingAllowed: boolean = true;

  constructor(private elementRef: ElementRef,
    private loginStatus : LoginStatusService,
    private renderer: Renderer2,
    private router : Router,
    private documentService: DocumentService,
    private apiService : ApiService,
    private hubService : HubService,
    private projectService : ProjectApiService)
  {
    // Initial check for login status
    this.loginStatus.checkLoginStatus(router);

    const access = localStorage.getItem('AllowEditorAccess');
    if(!access){
      this.leave();
      this.router.navigate(['dashboard'])
    }

    const id = localStorage.getItem("ProjectId");
    if(id != null){
      //console.log("PROJECT ID FROM CODE_EDITOR", id)
      this.getCodeFromDb(id);
    }
  }

  ngOnInit(){

    console.log("ngOnInit")
    
    /*
    this.documentService.intialDoc$.subscribe((response)=> {
      if(this.editor && this.editor.getValue() == ""){
        this.editor.setValue(response)
      }
    })*/

    this.documentService.documentUpdate$.subscribe((updateDocument) => {
      if (this.editor && !this.isUpdatingEditor) {
        const currentPosition = this.editor.getCursorPosition();
        this.isUpdatingEditor = true;
        console.log("CHECK IN UPDATEDDOC")
        // Check if the updateDocument is different from the current editor content
        if (updateDocument !== this.editor.getValue()) {
          this.editor.setValue(updateDocument, currentPosition); // Set the updated document and cursor position
        }
        this.isUpdatingEditor = false;
      }
    });
  }

  ngOnDestroy(): void {
    this.leave();
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
      //this.editor.setReadOnly(true)

      if (this.editor && this.editor.getValue() == "") {
        this.documentService.intialDoc$.subscribe(response => {
          this.editor.setValue(response)
          console.log("EDITOR VALUE IS EMPTY AND INITAL DOC IS ", response) //WHY IS THIS EMPTY
        })
      }

      this.editor.session.on('change', () => {
        if(!this.isUpdatingEditor){
          const updatedCode = this.editor.getValue();
          this.code = updatedCode;
          this.documentService.broadcastDocumentUpdate(this.code);
          // Start or reset the auto-save timer when the document is changed
          //this.startAutoSaveTimer();
        }
      });  
      this.editor.session.selection.on('changeCursor', (e: any) => {
        const cursorPosition = this.editor.getCursorPosition();
      }); 
    });
  
    // Retrieve the item from localStorage
    const access = localStorage.getItem('AllowEditorAccess');

    if(access){
      // Remove the item from localStorage
      localStorage.removeItem('AllowEditorAccess');
    }
  }

  leave(){
    const user = localStorage.getItem("LoggedInUser");
    if(user){
      const username = JSON.parse(user).username;
      this.hubService.disconnectUser(username);
    }
    
    this.documentService.leaveChat();
    //window.location.reload();

    this.router.navigate(['dashboard'])
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

  getCodeFromDb(projectId : any){
    this.projectService.getCode(projectId).subscribe(response => {
      console.log(response)
    })
  }

}
  /**
   * 
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


    toggleEditing(): void {
    this.editingAllowed = !this.editingAllowed;
    this.updateEditorAccessibility();
  }

  private updateEditorAccessibility(): void {
    if (this.editingAllowed) {
      // Enable editor
      this.editor.setReadOnly(false);
      this.editor.container.style.opacity = '1';
    } else {
      // Disable editor
      this.editor.setReadOnly(true);
      this.editor.container.style.opacity = '0.5'; // You can adjust the opacity to give a visual cue that editing is disabled
    }
  }

   */

