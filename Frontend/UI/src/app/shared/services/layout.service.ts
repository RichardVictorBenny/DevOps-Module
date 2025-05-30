import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

/**
 * @file layout.service.ts
 * @author Richard Benny
 * @purpose Provides layout-related state management, specifically tracking whether the current page is a standalone page (e.g., login or register).
 * @dependencies Angular core (Injectable), Angular router (Router), rxjs (BehaviorSubject)
 */
@Injectable({
  providedIn: 'root'
})
export class LayoutService {

  public isStandalonePage: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);


  constructor(router: Router) { 
    router.events.subscribe(() => {

      this.isStandalonePage.next(
        router.url === '/login' ||
        router.url === '/register'
      );
    });

    
  
  }

}
