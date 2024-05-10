import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  public search = new BehaviorSubject<string>('');
  search$ = this.search.asObservable();

  updateSearchTerm(term: string) {
    this.search.next(term);
  }
}
