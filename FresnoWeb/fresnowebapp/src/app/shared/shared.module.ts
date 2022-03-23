import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { FilterTextboxComponent } from './filter-textbox.component';

@NgModule({
    declarations: [
        FilterTextboxComponent
    ],
    imports: [
        CommonModule,
        FormsModule
    ],
    exports: [
        FilterTextboxComponent
    ]
})
export class SharedModule { }
