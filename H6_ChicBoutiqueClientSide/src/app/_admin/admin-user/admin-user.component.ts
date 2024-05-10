import { User } from './../../_models/user';
import { Component, OnInit } from '@angular/core';
import { UserService} from 'src/app/_services/user.service';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { FormBuilder } from '@angular/forms';
import{Role} from 'src/app/_models/role';
import { Observable,map, of, tap } from 'rxjs';
import { SearchService } from 'src/app/_services/search.service';  // SearchService
import { OrderService } from 'src/app/_services/order.service';
import { Order } from 'src/app/_models/order';



@Component({
  selector: 'app-admin-user',
  templateUrl: './admin-user.component.html',
  styleUrls: ['./admin-user.component.css']
})
export class AdminUserComponent implements OnInit {
 selectedValue = '0';
  users: User[]= [];
  admins: User[] = [];
  members: User[] = [];
  guests: User[] = [];
  userId = 1; // replace with the desired user ID

  // Declare members$ here
  users$!: Observable<User[]>;
  searchTerm: string = '';
  searched: boolean = false; // Initialize searched to false
  message: string = '';
  roles!: Role
  orders:Order[]=[];


  user: User = {
    id: 0, email: '', firstName: '', lastName: '', password: '', address: '',
    city: '', postalcode: '', country: '', telephone: '', role: 0}


  constructor(private userService: UserService,

    private searchService: SearchService,
    private orderService:OrderService) {
       // Fetch All Users list by default when the component is initialized
    this.users$ = this.userService.getUsers();

    }




  ngOnInit(): void {
    this.userService.getUsers().subscribe(
      u => {this.users = u
        console.log(u);});

    // Fetch orders after getting users
    this.orderService.getAllOrders().subscribe(orders => {
    this.orders = orders;
    console.log("List of orders",orders);

   })



    console.log(this.users);

    // Subscribe to changes in the search term
    this.searchService.search$.subscribe((term) => {
      // Update the local searchTerm whenever the search term changes
      this.searchTerm = term;
      //  call the searchProducts() method.
      this.searchUser();
  });
}
// method to handle input changes in the search field
onSearchInputChange() {
  // Update the search term in the SearchService
  this.searchService.updateSearchTerm(this.searchTerm);
}

   // method ot fetch all users
fetchUsers() {
  this.userService.getUsers().subscribe((u) => (this.users = u));

}

// Method to search specific user
searchUser(): void {
  this.users$.subscribe((users: User[]) => {
    if (this.searchTerm == null || this.searchTerm.trim() === '') {
      // If search term is empty, do not display any list of users
      this.users = [];
      this.searched = false;  // Set searched to false when search term is empty
    } else {
      // Use a temporary array to store the filtered users
      const filteredUsers = users.filter((user) => {
        // Check if the user's first name contains the searchTerm (case-insensitive)
        return user.firstName.toLowerCase().includes(this.searchTerm.toLowerCase())  ||
        user.lastName.toLowerCase().includes(this.searchTerm.toLowerCase())||
        user.email.toLowerCase().includes(this.searchTerm.toLowerCase())||
        user.homeAddress?.address.toLowerCase().includes(this.searchTerm.toLowerCase())
      });

      this.users = filteredUsers;  // Update the main users array with the filtered results
      this.searched = true;  // Set searched to true after the search is performed

      // Check if no results were found
      if (filteredUsers.length === 0) {
        alert('No results found');
      }
    }
  });
}

 // Function to get role name based on role number
 getRoleName(roleNumber: number): string {
  switch (roleNumber) {
      case 0:
          return 'Administrator';
      case 1:
          return 'Member';
      case 2:
          return 'Guest';
      default:
          return 'Unknown';
  }
}

  // Method to handle role change
  onRoleChange(event:any): void {
  this.selectedValue=event.target.value;
  console.log('Selected Role:', this.selectedValue);

 // Fetch user information based on the selected role
  switch (this.selectedValue) {
    case '0':
      console.log('Fetching Users');
      // Assign the observable to members$
      this.users$ = this.userService.getUsers();
      break;


      case '1':
        console.log('Fetching Admins');
        // Assign the observable to members$
        this.users$ = this.userService.getAdminsList();
        break;

      case '2':
        console.log('Fetching Members');
        // Assign the observable to members$
        this.users$ = this.userService.getMembersList();
        break;

      case '3':
        console.log('Fetching Guests');
        // Assign the observable to members$
        this.users$ = this.userService.getGuestsList();
        break;

      default:
        console.error('Unexpected selectedValue:', this.selectedValue);
         // Set default to Members in case of unexpected value
         this.selectedValue = '1';
         this.users$ = this.userService.getMembersList();
        break;
    }


  // Assuming that the user has a property named 'userId' that corresponds to the 'id' in HomeAddress
  this.users$.subscribe((users) => {

    });


  }



  edit_member(user: User): void {
    this.message = '';
    this.user = user;
    this.user.id = user.id || 0;
    console.log(this.user);
  }








  cancel(): void {
    this.user =  { id: 0,email: '', firstName: '', lastName: '', password: '', address: '',
    city: '', postalcode: '', country: '', telephone: '', role: 1};
  }






}


