import { Component, Input, OnInit } from '@angular/core';
import { DocumentService } from '../../../../core/services/document.service';
import { Documento } from '../../../../core/models/document.model';

@Component({
  selector: 'app-document-list',
  templateUrl: './document-list.component.html',
  styleUrls: ['./document-list.component.css']
})
export class DocumentListComponent implements OnInit {

  documents: Documento[] = [];
  isLoading = false;
  error: string | null = null;
  documentToDelete: Documento | null = null;
  private _loadList: boolean = false;

  constructor(private documentService: DocumentService) { }

  ngOnInit(): void {
    this.loadDocuments();
  }

  /**
   * Cargar documentos desde la API
   */
  loadDocuments(): void {
    this.isLoading = true;
    this.error = null;

    this.documentService.getAllDocuments().subscribe({
      next: (documents) => {
        this.documents = documents;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error al cargar los documentos:', error.errorMessage);
        this.error = error.errorMessage || 'Error al cargar los documentos';
        this.isLoading = false;
      }
    });
  }

  /**
   * Mostrar modal de confirmación
   */
  confirmDelete(document: Documento): void {
    this.documentToDelete = document;
  }

  /**
   * Cancelar eliminación
   */
  cancelDelete(): void {
    this.documentToDelete = null;
  }

  /**
   * Eliminar documento
   */
  deleteDocument(): void {
    if (!this.documentToDelete) {
      return;
    }

    const documentId = this.documentToDelete.id;

    this.documentService.deleteDocument(documentId).subscribe({
      next: () => {
        // Eliminar documento de la lista localmente
        this.documents = this.documents.filter(d => d.id !== documentId);
        this.documentToDelete = null;
      },
      error: (error) => {
        this.error = error.message || 'Error al eliminar el documento';
        this.documentToDelete = null;
      }
    });
  }

  /**
   * Obtener icono según extensión
   */
  getFileIcon(extension: string): string {
    console.log(extension);
    switch (extension.toLowerCase()) {
      case '.pdf':
        return 'bi-file-pdf-fill text-danger';
      case '.xlsx':
      case '.xls':
        return 'bi-file-excel-fill text-success';
      default:
        return 'bi-file-earmark-text';
    }
  }

  /**
   * Formatear fecha
   */
  formatDate(date: Date): string {
    return new Date(date).toLocaleString('es-ES', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}
