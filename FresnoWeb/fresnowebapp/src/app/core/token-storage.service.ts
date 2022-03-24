import { Injectable } from '@angular/core';

const TOKEN_KEY = 'auth-token';
const USER_KEY = 'auth-user';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {

  constructor() { }

  signOut(): void {
    window.sessionStorage.clear();
    console.log('Session Storage cleaned');
  }

  public saveToken(token: string): void {
    window.sessionStorage.removeItem(TOKEN_KEY);
    window.sessionStorage.setItem(TOKEN_KEY, token);
  }

  public getToken(): string | null {
    return window.sessionStorage.getItem(TOKEN_KEY);
  }

  public saveUser(user: any): void {
    window.sessionStorage.removeItem(USER_KEY);
    window.sessionStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public getUser(): any {
    const user = window.sessionStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }

    return {};
  }

  public isAdmin(): boolean {
    const user = this.getUser();
    if (user) {
      if (user.roles.includes('ROLE_ADMIN')) {
        return true;
      }
    }

    return false;
  }

  public isModerator(): boolean {
    const user = this.getUser();
    if (user) {
      if (user.roles.includes('ROLE_MODERATOR')) {
        return true;
      }
    }

    return false;
  }

  public isModeratorOrAdmin(): boolean {
    const user = this.getUser();
    if (user) {
      if (user.roles.includes('ROLE_ADMIN') || user.roles.includes('ROLE_MODERATOR')) {
        return true;
      }
    }

    return false;
  }
}
