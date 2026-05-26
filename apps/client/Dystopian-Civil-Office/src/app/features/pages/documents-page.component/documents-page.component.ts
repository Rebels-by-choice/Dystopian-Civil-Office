import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { DocumentsService } from '../../services/documents.service';

@Component({
  selector: 'app-documents-page.component',
  standalone: true,
  imports: [ButtonComponent, AsyncPipe, DatePipe],
  templateUrl: './documents-page.component.html',
  styles: ``,
})
export class DocumentsPageComponent {
  private readonly documentsService = inject(DocumentsService);

  protected readonly documents$ = this.documentsService.getDocuments();

  public addDocument(): void {
    console.log('Adding new document (pending)');
  }
}
