import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { TokenStorageService } from './token-storage.service';

const AUTH_API = 'http://localhost:8000/api/auth/';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private tokenService: TokenStorageService) { }

  login(username: string, password: string): Observable<any> {
    return this.http.post(`${AUTH_API}signin`, {
      username,
      password
    }, httpOptions);
  }

  register(username: string, email: string, password: string): Observable<any> {
    return this.http.post(`${AUTH_API}signup`, {
      username,
      email,
      password
    }, httpOptions);
  }

  verify(): boolean {
    if (this.tokenService.getToken()) {
      let test = this.http.get(`${AUTH_API}verify`);
      return true;
    }

    return false;
  }
}
