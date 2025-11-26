import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ModalConfig, ModalType } from '../models/modal.model';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modalSubject = new BehaviorSubject<ModalConfig | null>(null);
  public modal$: Observable<ModalConfig | null> = this.modalSubject.asObservable();

  /**
   * Mostrar modal de confirmación
   */
  showConfirm(
    title: string,
    message: string,
    onConfirm: () => void,
    onCancel?: () => void,
    bodyContent?: string
  ): void {
    const config: ModalConfig = {
      type: ModalType.CONFIRM,
      title,
      message,
      bodyContent,
      primaryButton: {
        label: 'Confirmar',
        class: 'btn-danger',
        icon: 'bi-check-circle',
        action: () => {
          onConfirm();
          this.close();
        }
      },
      secondaryButton: onCancel ? {
        label: 'Cancelar',
        class: 'btn-secondary',
        action: () => {
          onCancel();
          this.close();
        }
      } : undefined,
      showCloseButton: true,
      closable: true
    };

    this.modalSubject.next(config);
  }

  /**
   * Mostrar modal informativo
   */
  showInfo(
    title: string,
    message: string,
    onOk?: () => void,
    bodyContent?: string
  ): void {
    const config: ModalConfig = {
      type: ModalType.INFO,
      title,
      message,
      bodyContent,
      primaryButton: {
        label: 'Aceptar',
        class: 'btn-primary',
        icon: 'bi-info-circle',
        action: () => {
          if (onOk) onOk();
          this.close();
        }
      },
      showCloseButton: true,
      closable: true
    };

    this.modalSubject.next(config);
  }

  /**
   * Mostrar modal de éxito
   */
  showSuccess(
    title: string,
    message: string,
    onOk?: () => void,
    bodyContent?: string
  ): void {
    const config: ModalConfig = {
      type: ModalType.SUCCESS,
      title,
      message,
      bodyContent,
      primaryButton: {
        label: 'Aceptar',
        class: 'btn-success',
        icon: 'bi-check-circle',
        action: () => {
          if (onOk) onOk();
          this.close();
        }
      },
      showCloseButton: true,
      closable: true
    };

    this.modalSubject.next(config);
  }

  /**
   * Mostrar modal de error
   */
  showError(
    title: string,
    message: string,
    onOk?: () => void,
    bodyContent?: string
  ): void {
    const config: ModalConfig = {
      type: ModalType.ERROR,
      title,
      message,
      bodyContent,
      primaryButton: {
        label: 'Aceptar',
        class: 'btn-danger',
        icon: 'bi-exclamation-triangle',
        action: () => {
          if (onOk) onOk();
          this.close();
        }
      },
      showCloseButton: true,
      closable: true
    };

    this.modalSubject.next(config);
  }

  /**
   * Mostrar modal de advertencia
   */
  showWarning(
    title: string,
    message: string,
    onOk?: () => void,
    bodyContent?: string
  ): void {
    const config: ModalConfig = {
      type: ModalType.WARNING,
      title,
      message,
      bodyContent,
      primaryButton: {
        label: 'Aceptar',
        class: 'btn-warning',
        icon: 'bi-exclamation-triangle-fill',
        action: () => {
          if (onOk) onOk();
          this.close();
        }
      },
      showCloseButton: true,
      closable: true
    };

    this.modalSubject.next(config);
  }

  /**
   * Mostrar modal personalizado
   */
  showCustom(config: ModalConfig): void {
    this.modalSubject.next(config);
  }

  /**
   * Cerrar modal
   */
  close(): void {
    this.modalSubject.next(null);
  }
}

