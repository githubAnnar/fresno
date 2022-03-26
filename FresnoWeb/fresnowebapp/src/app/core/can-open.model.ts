import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "./auth.service";
import { TokenStorageService } from "./token-storage.service";

@Injectable()
export class CanOpen implements CanActivate {

    constructor(private router: Router, private tokenService: TokenStorageService, private authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        if (this.authService.verify()) {
            return true;
        }

        return this.router.navigate(['/login'], { queryParams: { 'redirectURL': state.url } });
    }
}
