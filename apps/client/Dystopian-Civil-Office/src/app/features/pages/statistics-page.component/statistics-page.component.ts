import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { map } from 'rxjs';

import { StatisticsService } from '../../services/statistics.service';

@Component({
  selector: 'app-statistics-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './statistics-page.component.html',
  styles: ``,
})
export class StatisticsPageComponent {
  private readonly statisticsService = inject(StatisticsService);

  readonly summary$ = this.statisticsService.getStatistics().pipe(
    map((entries) => {
      return {
        newRecords: entries.filter(
          (entry) => entry.method === 'POST' && entry.statusCode === 201
        ).length,
        editedRecords: entries.filter(
          (entry) =>
            entry.method === 'PUT' &&
            entry.statusCode >= 200 &&
            entry.statusCode < 300
        ).length,
        deletedRecords: entries.filter(
          (entry) =>
            entry.method === 'DELETE' &&
            entry.statusCode >= 200 &&
            entry.statusCode < 300
        ).length
      };
    })
  );
}