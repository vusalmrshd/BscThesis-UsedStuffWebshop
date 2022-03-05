import { Injectable } from '@angular/core';
import { BehaviorSubject,Observable } from 'rxjs';

@Injectable()
export class InterconnectedService {
  //for transfering data between two components when you are doing route
  private messageSource = new BehaviorSubject('default message');
  currentMessage = this.messageSource.asObservable();

  constructor() { }

  sendMessage(message:any) {
    this.messageSource.next(message)
  }

}