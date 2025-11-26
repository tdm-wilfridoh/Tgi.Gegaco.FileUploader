import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Documento, Result } from '../models/document.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  //private apiUrl = 'api/Documentos'; // Ajusta el puerto según tu API
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  /**
   * Obtener todos los documentos
   */
  getAllDocuments(): Observable<Documento[]> {
    return this.http.get<Result<Documento[]>>(this.apiUrl)
      .pipe(
        map(response => {
          if (response.isSuccess && response.data) {
            return response.data;
          }
          throw new Error(response.errorMessage || 'Error al obtener documentos');
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Subir un documento
   */
  uploadDocument(file: File): Observable<Documento> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.http.post<Result<Documento>>(`${this.apiUrl}/upload`, formData)
      .pipe(
        map(response => {
          if (response.isSuccess && response.data) {
            return response.data;
          }
          throw new Error(response.errorMessage || 'Error al subir documento');
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Eliminar un documento
   */
  deleteDocument(id: string): Observable<boolean> {
    return this.http.delete<Result<boolean>>(`${this.apiUrl}/${id}`)
      .pipe(
        map(response => {
          console.log('response: ', response);
          if (response.isSuccess) {
            return true;
          }
          throw new Error(response.errorMessage || 'Error al eliminar documento');
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Validar extensión de archivo
   */
  validateFileExtension(fileName: string): boolean {
    const allowedExtensions = ['.pdf', '.xlsx', '.xls'];
    const extension = fileName.substring(fileName.lastIndexOf('.')).toLowerCase();
    return allowedExtensions.includes(extension);
  }

  /**
   * Validar tamaño de archivo (10 MB máximo)
   */
  validateFileSize(fileSizeInBytes: number): boolean {
    const maxSizeInMB = 10;
    const maxSizeInBytes = maxSizeInMB * 1024 * 1024;
    return fileSizeInBytes <= maxSizeInBytes;
  }

  /**
   * Manejo de errores
   */
  private handleError(error: any): Observable<never> {
    console.error('Error en DocumentService:---------|', error);
    let errorMessage = 'Ha ocurrido un error';

    if (error.error instanceof ErrorEvent) {
      // Error del lado del cliente
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Error del lado del servidor
      errorMessage = error.error?.errorMessage || error.message || errorMessage;
    }

    console.error('Error en DocumentService:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
