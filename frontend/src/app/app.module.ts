import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CodeEditorComponent } from './code-editor/code-editor.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MonacoEditorService } from './services/monaco-editor.service';
import { TestComponent } from './test/test.component';
import { HttpClientModule } from '@angular/common/http';
import { NavbarComponent } from './navbar/navbar.component';
import { TryEditorComponent } from './try-editor/try-editor.component';
import { ChatBoxComponent } from './chat-box/chat-box.component';
import { ChatServiceService } from './services/chat-service.service';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { BackgroundColorService } from './services/background-color.service';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CookieService } from 'ngx-cookie-service';
import { TestCookieComponent } from './test-cookie/test-cookie.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AceComponent } from './ace/ace.component';
import { LoginTestComponent } from './login-test/login-test.component';
import { CreateProjectComponent } from './create-project/create-project.component';
import { JoinProjectComponent } from './join-project/join-project.component';
import { DeleteConfirmationComponent } from './delete-confirmation/delete-confirmation.component';
import { FilterPipe } from './pipes/filter.pipe';
import { EditProjectComponent } from './edit-project/edit-project.component';

@NgModule({
  declarations: [
    AppComponent,
    CodeEditorComponent,
    TestComponent,
    NavbarComponent,
    TryEditorComponent,
    ChatBoxComponent,
    LoginComponent,
    RegisterComponent,
    TestCookieComponent,
    DashboardComponent,
    AceComponent,
    LoginTestComponent,
    CreateProjectComponent,
    JoinProjectComponent,
    DeleteConfirmationComponent,
    FilterPipe,
    EditProjectComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [
    MonacoEditorService,
    ChatServiceService,
    BackgroundColorService,
    CookieService,
    provideClientHydration()
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
 }
