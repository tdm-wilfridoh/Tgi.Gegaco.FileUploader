import { Component, EventEmitter, Output } from '@angular/core';
import { DocumentService } from '../../../../core/services/document.service';

@Component({
  selector: 'app-document-upload',
  templateUrl: './document-upload.component.html',
  styleUrls: ['./document-upload.component.css']
})
export class DocumentUploadComponent {
  @Output() documentUploaded = new EventEmitter<void>();

  selectedFile: File | null = null;
  isDragging = false;
  isUploading = false;
  uploadError: string | null = null;
  uploadSuccess: string | null = null;

  constructor(private documentService: DocumentService) { }

  /**
   * Manejar selección de archivo desde input
   */
  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.validateAndSetFile(file);
    }
  }

  /**
   * Manejar drag over
   */
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }

  /**
   * Manejar drag leave
   */
  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
  }

  /**
   * Manejar drop de archivo
   */
  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.validateAndSetFile(files[0]);
    }
  }

  /**
   * Validar y establecer archivo seleccionado
   */
  private validateAndSetFile(file: File): void {
    this.uploadError = null;
    this.uploadSuccess = null;

    // Validar extensión
    if (!this.documentService.validateFileExtension(file.name)) {
      this.uploadError = 'Extensión no permitida. Solo se aceptan archivos .pdf, .xlsx y .xls';
      this.selectedFile = null;
      return;
    }

    // Validar tamaño
    if (!this.documentService.validateFileSize(file.size)) {
      this.uploadError = 'El archivo excede el tamaño máximo permitido (10 MB)';
      this.selectedFile = null;
      return;
    }

    this.selectedFile = file;
  }

  /**
   * Subir documento
   */
  uploadDocument(): void {
    if (!this.selectedFile) {
      return;
    }

    this.isUploading = true;
    this.uploadError = null;
    this.uploadSuccess = null;

    this.documentService.uploadDocument(this.selectedFile).subscribe({
      next: (document) => {
        this.uploadSuccess = `Documento "${document.nombre}" cargado exitosamente`;
        this.selectedFile = null;
        this.isUploading = false;

        // Emitir evento para actualizar la lista
        this.documentUploaded.emit();

        // Limpiar mensaje de éxito después de 3 segundos
        setTimeout(() => {
          this.uploadSuccess = null;
        }, 3000);

        // Limpiar el input file
        const fileInput = window.document.getElementById('fileInput') as HTMLInputElement;
        if (fileInput) {
          fileInput.value = '';
        }
      },
      error: (error) => {
        this.uploadError = error.message || 'Error al cargar el documento';
        this.isUploading = false;
        this.selectedFile = null;
      }
    });
  }

  /**
   * Cancelar selección de archivo
   */
  cancelSelection(): void {
    this.selectedFile = null;
    this.uploadError = null;
    this.uploadSuccess = null;

    const fileInput = window.document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }
}
