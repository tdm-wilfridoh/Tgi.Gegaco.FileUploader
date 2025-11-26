import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { Subscription } from 'rxjs';
import { ModalService } from '../../services/modal.service';
import { ModalConfig, ModalType } from '../../models/modal.model';

@Component({
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.component.html',
  styleUrls: ['./confirm-modal.component.css']
})
export class ConfirmModalComponent implements OnInit, OnDestroy {
  modalConfig: ModalConfig | null = null;
  private subscription: Subscription = new Subscription();

  constructor(private modalService: ModalService) { }

  ngOnInit(): void {
    this.subscription = this.modalService.modal$.subscribe(config => {
      this.modalConfig = config;
      if (config) {
        // Prevenir scroll del body cuando el modal está abierto
        document.body.style.overflow = 'hidden';
      } else {
        // Restaurar scroll del body cuando el modal está cerrado
        document.body.style.overflow = '';
      }
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    document.body.style.overflow = '';
  }

  /**
   * Cerrar modal al presionar ESC
   */
  @HostListener('window:keydown.escape', ['$event'])
  handleEscapeKey(event: KeyboardEvent): void {
    if (this.modalConfig?.closable) {
      this.close();
    }
  }

  /**
   * Cerrar modal
   */
  close(): void {
    if (this.modalConfig?.closable !== false) {
      this.modalService.close();
    }
  }

  /**
   * Cerrar al hacer clic en el backdrop
   */
  onBackdropClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (target.classList.contains('modal-backdrop') && this.modalConfig?.closable) {
      this.close();
    }
  }

  /**
   * Obtener clase de icono según el tipo de modal
   */
  getIconClass(): string {
    if (!this.modalConfig) return '';
    
    switch (this.modalConfig.type) {
      case ModalType.SUCCESS:
        return 'bi-check-circle-fill text-success';
      case ModalType.ERROR:
        return 'bi-exclamation-triangle-fill text-danger';
      case ModalType.WARNING:
        return 'bi-exclamation-triangle-fill text-warning';
      case ModalType.INFO:
        return 'bi-info-circle-fill text-info';
      case ModalType.CONFIRM:
        return 'bi-question-circle-fill text-warning';
      default:
        return 'bi-info-circle-fill text-primary';
    }
  }

  /**
   * Verificar si el modal está visible
   */
  get isVisible(): boolean {
    return this.modalConfig !== null;
  }
}

