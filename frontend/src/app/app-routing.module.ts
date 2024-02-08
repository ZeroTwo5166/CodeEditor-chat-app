import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CodeEditorComponent } from './code-editor/code-editor.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { TryEditorComponent } from './try-editor/try-editor.component';
import { AceComponent } from './ace/ace.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  //{path: '', redirectTo: '/try-code-editor', pathMatch: 'full' },
  {path: '', redirectTo: '/try-code-editor', pathMatch: 'full' },
  {path : 'code-editor', component: CodeEditorComponent},
  {path : 'login', component: LoginComponent},
  {path : 'register', component: RegisterComponent},
  {path : 'try-code-editor', component: TryEditorComponent},
  {path : 'dashboard', component: DashboardComponent},
  {path : 'ace-editor', component: AceComponent},


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
