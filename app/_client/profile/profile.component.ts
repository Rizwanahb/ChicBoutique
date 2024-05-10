import { User } from './../../_models/user';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: User = this.newUser();
  users: User[] = [];
  profileForm: FormGroup = this.formBuilder.group({}); // initialization
  showChangePasswordForm = false;
  currentPassword = '';
  newPassword = '';
  confirmPassword = '';


  currentUser: User =
  { id: 0,
    email: '',
    firstName: '',
    lastName: '',
    password: '',
    address: '',
    city: '',
    country: '',
    postalcode: '',
    telephone: '',
    role: 2 };


  constructor(
    private authService: AuthService,
    private userService: UserService,
    private formBuilder: FormBuilder
  ) {
    this.authService.currentUser.subscribe(user => {
      if (user) {
        this.userService.getUserbyEmail(user.email).subscribe(userData => {
          this.currentUser = userData;
          console.log('user details are: ', this.currentUser);

        });
      }
    });
  }

  ngOnInit(): void {}

  newUser(): User {
    return {
      id: 0,
      email: '',
      firstName: '',
      lastName: '',
      password: '',
      address: '',
      city: '',
      country: '',
      postalcode: '',
      telephone: '',
      role: 2
    };
  }


  cancel(): void {
    this.user = this.newUser();// Reset the form
  }

  editProfile(user: User): void {

    this.user = user;
    this.user.id = user.id || 0;
    console.log(this.user);
    this.save();
  }

  public save(): void {
    console.log("updated user: ",this.user);
    if(this.user.id == 0) {
      this.userService.registerUser(this.user)
      .subscribe({
        next: (x) => {
          console.log(x);
          this.users.push(x);
          this.user={ id: 0, email: '', firstName: '', lastName: '', password: '', address: '', city: '', country: '', postalcode: '', telephone: '', role: 2 };
      //    this.message = '';
          Swal.fire({
            title: 'Success!',
            text: 'Category added successfully',
            icon: 'success',
            confirmButtonText: 'OK'
          });
        },
        error: (err) => {
          console.log(err.error);

        }
      });
    }
         this.userService.updateUser(this.user.id ,this. user)
           .subscribe({

             error: (err) => {
            console.log("Update error is: ", err.error);
          },
          complete: () => {

       this.user={ id: 0, email: '', firstName: '', lastName: '', password: '', address: '', city: '', country: '', postalcode: '', telephone: '', role: 2 };
            Swal.fire({
              title: 'Success!',
              text: 'Profile updated successfully',
              icon: 'success',
              confirmButtonText: 'OK'
           });
          }


  });
  }
  toggleChangePasswordForm(): void {
    this.showChangePasswordForm = !this.showChangePasswordForm;
  }

  changePassword(newPassword: string, userId: number) {

    this.userService.resetPassword(this.newPassword, userId).subscribe(
      (response: string) => {
        console.log('Password changed successfully', response);
        Swal.fire('Success', 'Password changed successfully', 'success');
        console.log("New password: ", newPassword);
      },
      (error) => {
        console.error('Password change failed', error);
        Swal.fire('Error', 'An error occurred while changing the password. Please try again.', 'error');
      }
    );

  }

  cancelChangePassword(): void {
    // Reset the form and hide the change password section
    this.resetChangePasswordForm();
  }

  resetChangePasswordForm(): void {
    this.currentPassword = '';
    this.newPassword = '';
    this.confirmPassword = '';
    this.showChangePasswordForm = false;
  }

  confirmPasswordValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const password = this.profileForm.get('newPassword')?.value;
    const confirmPassword = control.value;

    return password === confirmPassword ? null : { 'passwordMismatch': true };
  }
}
