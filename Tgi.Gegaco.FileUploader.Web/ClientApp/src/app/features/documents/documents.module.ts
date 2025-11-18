import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DocumentsRoutingModule } from './documents-routing.module';
import { DocumentsComponent } from './components/documents/documents.component';
import { DocumentListComponent } from './components/document-list/document-list.component';
import { DocumentUploadComponent } from './components/document-upload/document-upload.component';
import { FileSizePipe } from '../../shared/pipes/file-size.pipe';

@NgModule({
  declarations: [
    DocumentsComponent,
    DocumentListComponent,
    DocumentUploadComponent,
    FileSizePipe  // Importar el pipe
 ],
  imports: [
    CommonModule,
    DocumentsRoutingModule
  ]
})
export class DocumentsModule { }
