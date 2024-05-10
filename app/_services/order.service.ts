import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order } from '../_models/order';
import { PaymentService } from './payment.service';
import { OrderResponse } from '../_models/orderResponse';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  public shippingdetails = new BehaviorSubject<any>([]);
  public transactionId = new BehaviorSubject<any>([]);
  public paymentStatus =new BehaviorSubject<any>([]);
  public paymentMethod =new BehaviorSubject<any>([]);


  apiUrl_Order = environment.apiUrl + '/Order';
  public accessToken:any= this.paymentService.getAccessToken();

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.accessToken}` }),
  };


  constructor(private http: HttpClient, private paymentService:PaymentService) { }

  //Method for getting all orders


  setAddressData(data: any) {
    this.shippingdetails.next(data);
 }

  getAddressData() {
    return this.shippingdetails.asObservable();
 }
 setTransactionId(data: any) {
  this.transactionId.next(data);
}

getTransactionId() {
  return this.transactionId.asObservable();
}

setPaymentStatus(data: any) {
  this.paymentStatus.next(data);
}

getPaymentStatus() {
  return this.paymentStatus.asObservable();
}
setPaymentMethod(data: any) {
  this.paymentMethod.next(data);
}

getPaymentMethod() {
  return this.paymentMethod.asObservable();
}

//Method for getting all orders
getAllOrders():Observable <Order[]>{
  return this.http.get<Order[]> (this.apiUrl_Order);
}

//Mthod for getting ordeDetailsByOrderId
 getOrderDetailsByOrderId(orderId:number):Observable<any>{
  return this.http.get<OrderResponse>(`${this.apiUrl_Order}/${orderId}`, this.httpOptions);
  }

  //Storing the order in the database

  storeOrder(newOrder: Order): Observable<Order> {
    // console.log("mandag");
    console.log("storeOrder", newOrder);
    console.log(this.apiUrl_Order);
    // return this.http.get<Order[]>(this.apiUrl);
   return this.http.post<Order>(this.apiUrl_Order, newOrder, this.httpOptions);
  }


}
