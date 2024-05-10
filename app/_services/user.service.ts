import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { BehaviorSubject, Observable, forkJoin, switchMap } from 'rxjs';
import { map,of } from 'rxjs';
import { ShippingDetails } from '../_models/shippingdetails';
import { Token } from '@angular/compiler';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  apiUrl_user = environment.apiUrl + '/User';
  apiUrl_register = environment.apiUrl + '/User/register';
  apiUrl_guestregister = environment.apiUrl + '/User/guestRegister';
  apiUrl_accountinfo = environment.apiUrl + '/AccountInfo';
  apiUrl_changepassword= environment.apiUrl+ '/User/changepassword';



  httpOptions = {
    headers: new HttpHeaders({  'Content-Type': 'application/json' }),
  };

  constructor(
    private http: HttpClient,

  ) { }
//BehaviorSubject  to manage the user role, allowing components to subscribe and receive role updates.
  public getRole: BehaviorSubject<number> = new BehaviorSubject(0);
  public getRole$: Observable<number> = this.getRole.asObservable();

//Retrieves the list of all users.
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl_user)
  }
  getRole_(roleNr: number) {
    // This function can be used to notify subscribers about the role number
    this.getRole.next(roleNr)
  }
  getMembersCount(): Observable<number> {
    // getUsers function to count the items in the array
    return this.getUsers().pipe(
      map(users => users.filter(user => user.role === 1).length)
    );
  }
  getGuestsCount(): Observable<number> {
    // getUsers function to count the items in the array
    return this.getUsers().pipe(
      map(users => users.filter(user => user.role === 2).length)

    );
  }
 // Retrieves a list of users with the role of administrators.
  getAdminsList(): Observable<User[]> {
    return this.getUsers().pipe(
      map(users => users.filter(user => user.role === 0))
    );
  }


  getMembersList(): Observable<User[]> {
    return this.getUsers().pipe(
      map(users => users.filter(user => user.role === 1))
    );
  }

  getGuestsList(): Observable<User[]> {
    return this.getUsers().pipe(
      map(users => users.filter(user => user.role === 2))
    );
  }
  guest_register(guestUser: User): Observable<User>{
    return this.http.post<User>(this.apiUrl_guestregister, guestUser, this.httpOptions);
  }
  getUserGuid(userId: number): Observable<string> {
    return this.http.get<string>(`${this.apiUrl_accountinfo}/${userId}`);
  }
  getUserbyEmail(email:string):Observable<User> {
    return this.http.get<User>(`${this.apiUrl_user}/${email}`);;
  }

  registerUser(user: User): Observable<User>{
    return this.http.post<User>(this.apiUrl_register, user, this.httpOptions);
  }


  updateUser(userId: number, user:User): Observable<User> {
    console.log('Updating user with ID:', userId);
    console.log('User data:', user);
    return this.http.put<User>(`${this.apiUrl_register}/${userId}`, user, this.httpOptions);
  }

  deleteUser(userId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl_user}/${userId}`, this.httpOptions);
  }


  resetPassword(newPassword: string, userId: number): Observable<string> {
    const passwordEntityRequest = { Password: newPassword, UserId: userId };
    return this.http.post(`${this.apiUrl_changepassword}`, passwordEntityRequest, {
      ...this.httpOptions,
      responseType: 'text',  // Set the response type to 'text'
    });
  }

}
