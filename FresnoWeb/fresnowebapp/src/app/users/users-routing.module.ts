import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CanModerate } from "../core/can-moderate.model";
import { CanOpen } from "../core/can-open.model";
import { UsersComponent } from "./users.component";

const routes: Routes = [
    { path: 'users', component: UsersComponent, canActivate: [CanOpen] }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    providers: [CanModerate, CanOpen]
})
export class UsersRoutingModule { }