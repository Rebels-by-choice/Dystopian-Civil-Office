import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface TableColumn<T> {
  key: keyof T;
  label: string;
  sortable?: boolean;
  width?: string;
}

/**
 * Generic reusable table wrapper component.
 * It provides consistent table styling while letting callers define header and body content.
 */
@Component({
  selector: 'app-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: 'table.component.html',
})
export class TableComponent<T> {
  @Input() columns: TableColumn<T>[] = [];
  @Input() data: T[] = [];
  @Input() emptyMessage = 'No data found';
}
