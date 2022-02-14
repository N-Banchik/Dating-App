import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  busyRequestCount: number = 0;
  constructor(private Spinner: NgxSpinnerService) {}

  busy() {
    this.busyRequestCount++;
    this.Spinner.show(
      undefined,
      {
        bdColor: 'rgba(255,255,255,0.7)',
        color: '#333333',
        type: 'ball-scale-multiple',
      }
    );
  }

  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.Spinner.hide();
    }
  }
}
