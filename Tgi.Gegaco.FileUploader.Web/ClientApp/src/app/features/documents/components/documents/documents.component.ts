import { Component, ViewChild } from '@angular/core';
import { DocumentListComponent } from '../document-list/document-list.component';

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.css']
})
export class DocumentsComponent {
  @ViewChild(DocumentListComponent) documentList!: DocumentListComponent;
  /**
   * Manejar evento de documento cargado
   */
  onDocumentUploaded(): void {
    // Recargar la lista de documentos
    if (this.documentList) {
      this.documentList.loadDocuments();
    }
  }
}
