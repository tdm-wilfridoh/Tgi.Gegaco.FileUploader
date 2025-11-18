export interface Documento {
  id: string;
  nombre: string;
  tamano: number;
  tamanoFormateado: string;
  extension: string;
  fechaCreacion: Date;
}

export interface Result<T> {
  isSuccess: boolean;
  data: T;
  errorMessage?: string;
}

export interface UploadResult {
  id: string;
  nombre: string;
  tamaño: number;
  tamañoFormateado: string;
  extension: string;
  fechaCreacion: Date;
}
