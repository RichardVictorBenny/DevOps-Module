import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';
import { LayoutService } from './shared/services/layout.service';
import { CommonModule } from '@angular/common';
import { MatModule } from './shared/modules/mat.module';

@Component({
  selector: 'app-root',
  imports: [ CommonModule, MatModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {

  title = 'UI';

  public isStandalonePage: boolean = false;

  constructor(private layoutService: LayoutService) { }

  ngOnInit(): void {
    this.layoutService.isStandalonePage.subscribe((val: boolean) => {
      this.isStandalonePage = val;
    });
  }
  
}
