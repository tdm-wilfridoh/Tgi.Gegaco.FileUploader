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

