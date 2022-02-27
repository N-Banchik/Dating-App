import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
@NgModule({
  declarations: [],
  imports: [
    TabsModule.forRoot(),
    CommonModule,
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ToastrModule.forRoot({ positionClass: 'toast-bottom-right' }),
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule,
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule,
    BsDatepickerModule,
  ],
})
export class SharedModule {}
