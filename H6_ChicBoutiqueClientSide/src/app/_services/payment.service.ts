import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private sandboxApiUrl = 'https://api-m.sandbox.paypal.com/v1/oauth2/token';

  constructor(private http: HttpClient) { }

  getAccessToken(): Observable<any> {
    const body = `grant_type=client_credentials`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded',
      'Authorization': `Basic ${btoa(`${environment.Client_Id}:${environment.Client_Secret}`)}`
    });

    return this.http.post<any>('https://api.sandbox.paypal.com/v1/oauth2/token', body, { headers });
  }

}
