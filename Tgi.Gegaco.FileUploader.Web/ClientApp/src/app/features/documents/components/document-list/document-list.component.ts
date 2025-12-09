import { Component, Input, OnInit } from '@angular/core';
import { DocumentService } from '../../../../core/services/document.service';
import { Documento } from '../../../../core/models/document.model';
import { ModalService } from '../../../../shared/services/modal.service';

@Component({
  selector: 'app-document-list',
  templateUrl: './document-list.component.html',
  styleUrls: ['./document-list.component.css']
})
export class DocumentListComponent implements OnInit {

  documents: Documento[] = [];
  isLoading = false;
  error: string | null = null;
  private _loadList: boolean = false;

  constructor(
    private documentService: DocumentService,
    private modalService: ModalService
  ) { }

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
        this.error = error.errorMessage || 'Error al cargar los documentos';
        this.isLoading = false;
      }
    });
  }

  /**
   * Mostrar modal de confirmación de eliminación
   */
  confirmDelete(document: Documento): void {
    // Formatear tamaño usando la misma lógica del pipe
    const formattedSize = this.formatFileSize(document.tamano);
    const bodyContent = `
      <div class="alert alert-light mb-0">
        <strong>${document.nombre}</strong>
        <br>
        <small class="text-muted">${formattedSize}</small>
      </div>
    `;

    this.modalService.showConfirm(
      'Confirmar Eliminación',
      '¿Estás seguro de que deseas eliminar el documento?',
      () => this.deleteDocument(document.id),
      undefined,
      bodyContent
    );
  }

  /**
   * Eliminar documento
   */
  private deleteDocument(documentId: string): void {
    this.documentService.deleteDocument(documentId).subscribe({
      next: () => {
        // Eliminar documento de la lista localmente
        this.documents = this.documents.filter(d => d.id !== documentId);
        // Mostrar mensaje de éxito
/*         this.modalService.showSuccess(
          'Documento Eliminado',
          'El documento ha sido eliminado exitosamente.'
        ); */
      },
      error: (error) => {
        // Mostrar mensaje de error
        this.modalService.showError(
          'Error al Eliminar',
          error.message || 'Error al eliminar el documento'
        );
      }
    });
  }

  /**
   * Formatear tamaño de archivo (helper para el modal - misma lógica del pipe)
   */
  private formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i];
  }

  /**
   * Obtener icono según extensión
   */
  getFileIcon(extension: string): string {
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
