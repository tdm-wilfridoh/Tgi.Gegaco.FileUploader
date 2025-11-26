/**
 * Tipos de modales disponibles
 */
export enum ModalType {
  CONFIRM = 'confirm',
  INFO = 'info',
  SUCCESS = 'success',
  ERROR = 'error',
  WARNING = 'warning'
}

/**
 * Configuración de botones del modal
 */
export interface ModalButton {
  label: string;
  class?: string;
  action: () => void;
  icon?: string;
}

/**
 * Configuración completa del modal
 */
export interface ModalConfig {
  type: ModalType;
  title: string;
  message: string;
  bodyContent?: string; // Contenido adicional opcional (HTML o texto)
  primaryButton: ModalButton;
  secondaryButton?: ModalButton;
  showCloseButton?: boolean;
  closable?: boolean; // Si se puede cerrar haciendo clic fuera o presionando ESC
}

