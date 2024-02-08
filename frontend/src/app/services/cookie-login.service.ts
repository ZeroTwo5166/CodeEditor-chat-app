import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class CookieLoginService {
  private apiUrl = 'https://localhost:7235/api/User/login';

  constructor(private http: HttpClient, private cookieService: CookieService) { }

  login(credentials: any): Observable<any> {
    return this.http.post(this.apiUrl, credentials);
  }

  logout(): void {
    localStorage.removeItem("loggedInUser");
  }

}
