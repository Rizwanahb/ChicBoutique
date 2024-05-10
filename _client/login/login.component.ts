import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { CartService } from 'src/app/_services/cart.service';
import { FormBuilder, FormControl, FormGroup, Validators, } from '@angular/forms';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  LoginForm!: FormGroup; //declare the login form with validation
  id:number=0;
  email: string = '';
  password: string = '';
  submitted = false;
  error = '';

  constructor( private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService,
    private cartService:CartService) {
      if (this.authService.currentUserValue != null && this.authService.currentUserValue.id > 0) {
        this.router.navigate(['login']);
  }}

  ngOnInit(): void {
    // initialization of  LoginForm
    // An array of validators for the "required" and "email" validators to the email field.
    this.LoginForm = this.fb.group({
      email: ['',[Validators.required,Validators.email]],
      password: ['', Validators.required],
    })

  }

  login(): void {
    console.log('Login function called');
    console.log('Email Control:', this.LoginForm.get('email'));
    console.log('Password Control:', this.LoginForm.get('password'));

  this.submitted= true;
  this.error = '';


    if (this.LoginForm.valid) {
      this.email= this.LoginForm.get('email')?.value;
      this.password = this.LoginForm.get('password')?.value;

      console.log('Email Control:', this.LoginForm.get('email'));
      console.log('Password Control:', this.LoginForm.get('password'));


    this.authService.login(this.email, this.password)
      .subscribe({
        next: () => {
             // Successful login
             // get return url from route parameters or default to '/'
             console.log('Successful login');
             const returnUrl =  '/';
             this.router.navigate(['/']);
             if(this.authService.currentUserValue.role==0){
              this.router.navigate(['/admin-panel']);
             } },

        error: obj => {
          console.log('login error ', obj.error);
          if (obj.error.status == 400 || obj.error.status == 401 || obj.error.status == 500) {
            this.error = 'Incorrect Username or Password';
          }
          else {
            this.error = obj.error.title;
          }
        }
      });
  }

  }
  }



