import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Product } from '../_models/product';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'; // Import the map operator
import { Guid } from 'guid-typescript';
import { ReserveQuantity } from '../_models/reservequantity';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl_product = environment.apiUrl +  '/product';
  private apiUrl_stock =environment.apiUrl+  '/Product' + '/stock';
  private apiUrl_reserveStock= environment.apiUrl+  '/Product' + '/ReserveStock';
  private apiUrl_reservationsuccess =environment.apiUrl+  '/Product' + '/ReservationSuccess';
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  constructor(private http : HttpClient) { }


  //Method for getting all products
  getAllProducts():Observable <Product[]>{
    return this.http.get<Product[]> (this.apiUrl_product);
  }
  getProductById(productId:number): Observable<Product>{
    return this.http.get<Product> (`${this.apiUrl_product}/${productId}`);
  }
  getProductsByCategoryId(categoryId:number): Observable<Product[]>{
    return this.http.get<Product[]>(`${this.apiUrl_product}/category/${categoryId} `)
  }

  getProductCount(): Observable<number> {
    // getAllProducts function to count the items in the array
    return this.getAllProducts().pipe(
      map(products => products.length)
    );
  }

  addProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl_product, product, this.httpOptions);
  }

  updateProduct(productId: number, product: Product): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl_product}/${productId}`, product, this.httpOptions);
  }

  deleteProduct(productId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl_product}/${productId}`, this.httpOptions);
  }

  getAvailableStock(productId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl_stock}/${productId}`, this.httpOptions);
  }
  reserveStock(reserveProductQuantity: ReserveQuantity):Observable<any> {
    return this.http.post<any> (this.apiUrl_reserveStock, reserveProductQuantity , this.httpOptions);

  }
  reservationSuccess(clientBasketId:string):Observable<any>{
    return this.http.put<any>(this.apiUrl_reservationsuccess,clientBasketId,this.httpOptions);
  }

}
