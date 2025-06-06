import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-notice-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatTableModule, MatCardModule],
  templateUrl: './notice-search.component.html',
  styleUrls: ['./notice-search.component.scss']
})
export class NoticeSearchComponent {
  searchForm;
  loading = false;
  error = '';
  results: any[] = [];
  displayedColumns: string[] = ['source', 'number', 'offenceDetails', 'dateIssued', 'amountDue', 'status', 'linkedDriverId', 'linkedVehicleReg'];

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.searchForm = this.fb.group({
      idNumber: [''],
      vehicleReg: ['']
    });
  }

  search() {
    this.error = '';
    this.results = [];
    this.loading = true;
    const { idNumber, vehicleReg } = this.searchForm.value;
    let token = '';
    // SSR-safe: Only access localStorage in browser
    try {
      if (isPlatformBrowser(this.platformId) && typeof localStorage !== 'undefined') {
        token = localStorage.getItem('token') || '';
      }
    } catch (e) {
      // Ignore if localStorage is not available
    }
    this.http.get<any[]>(`http://localhost:5254/api/notices/search`, {
      params: { idNumber: idNumber || '', vehicleReg: vehicleReg || '' },
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    }).subscribe({
      next: (res) => {
        // Normalize results for display
        this.results = (res || []).map(r => ({
          source: r.Source,
          number: r.Source === 'Notice' ? r.NoticeNumber : r.WoaNumber,
          offenceDetails: r.OffenceDetails,
          dateIssued: r.DateIssued,
          amountDue: r.AmountDue ?? '',
          status: r.Status,
          linkedDriverId: r.LinkedDriverId,
          linkedVehicleReg: r.LinkedVehicleReg
        }));
      },
      error: (err) => this.error = err.error || 'Search failed',
      complete: () => this.loading = false
    });
  }
}
