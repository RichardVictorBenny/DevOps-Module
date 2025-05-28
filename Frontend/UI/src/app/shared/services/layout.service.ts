import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {

  public isStandalonePage: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(router: Router) { 

    this.isStandalonePage.next(
      router.url.startsWith('/login') || 
      router.url.startsWith('/register')
    );
  }


}
