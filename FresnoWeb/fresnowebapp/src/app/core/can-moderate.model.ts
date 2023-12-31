import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { TokenStorageService } from "./token-storage.service";

@Injectable()
export class CanModerate implements CanActivate {

    constructor(private router: Router, private tokenService: TokenStorageService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        if (this.tokenService.isModeratorOrAdmin()) {
            return true;
        }

        return this.router.parseUrl('/');
    }
}
