import { Component, inject } from '@angular/core';
import { LoadingService } from '../../Services/LoadingService/loading-service.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'loader-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.css'
})
export class LoaderComponent {
  public loadingService = inject(LoadingService);
  public isLoading$: Observable<boolean> = this.loadingService.isLoading;

  constructor() {}
}
